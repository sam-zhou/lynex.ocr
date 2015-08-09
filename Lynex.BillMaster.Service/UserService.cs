using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Exception;
using Lynex.BillMaster.Exception.UserException;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum;
using Lynex.BillMaster.Repository.BillRepo;
using Lynex.BillMaster.Repository.UserRepo;
using Lynex.BillMaster.Service.Interface;
using Lynex.Common.Database;
using Lynex.Common.Extension;
using Lynex.Common.Service;

namespace Lynex.BillMaster.Service
{
    public class UserService : BaseService, IUserService
    {
        private readonly IAddressService _addressService;

        public UserService(IDatabaseService dbService, IAddressService addressService) : base(dbService)
        {
            _addressService = addressService;
        }

        public User RegisterUser(string email, string password, string mobile, string lastName, string firstName)
        {
            email = email.ToLower();

            if (!IsEmailUnique(email))
            {
                throw new PropertyNotUniqueException("Email",email);
            }

            try
            {
                
                DatabaseService.BeginTransaction();
                var salt = StringHelper.GenerateSalt();
                var hash = StringHelper.GetHash(password, salt, MD5.Create());

                var user = new User
                {
                    Email = email,
                    LastName = lastName.ToTitleCase(),
                    FirstName = firstName.ToTitleCase(),
                    Mobile = mobile,
                    Salt = salt,
                    Hash = hash,
                    Active = true,
                    PermissionRole = PermissionRole.User
                };

                var challenge = new UserChallenge(StringHelper.GenerateSalt(64)) { User = user };
                user.UserChallenge = challenge;

                var wallet = new Wallet { User = user };
                user.Wallet = wallet;

                DatabaseService.Save(user);
                DatabaseService.Save(challenge);
                DatabaseService.Save(wallet);
                DatabaseService.CommitTransaction();
                return user;
            }
            catch (System.Exception)
            {
                DatabaseService.RollBackTransaction();
                throw;
            }
        }

        public void CreateNewChallenge(long id)
        {
            var user = DatabaseService.Get<User>(id);
            try
            {
                DatabaseService.BeginTransaction();
                if (user.UserChallenge != null)
                {
                    user.UserChallenge.Challenge = StringHelper.GenerateSalt(64);
                    user.UserChallenge.CreatedAt = DateTime.UtcNow;
                    user.UserChallenge.TryCount = 0;
                    DatabaseService.Save(user.UserChallenge);
                }

            }
            catch (System.Exception)
            {
                DatabaseService.RollBackTransaction();
                throw;
            }
        }

        public bool IsEmailUnique(string email)
        {
            return DatabaseService.Get(new IsEmailUnique(email));
        }

        public UserLoginStatus Login(string email, string password)
        {
            email = email.ToLower();

            var status = UserLoginStatus.Successed;
            var user = DatabaseService.Get(new GetUserByEmail(email));
            if (user != null)
            {
                if (user.LoginAttempt >= 5 && user.LastFailedLogin.HasValue && user.LastFailedLogin.Value.AddHours(1) >= DateTime.UtcNow)
                {
                    status = UserLoginStatus.OverTryLimit;
                }
                else
                {
                    try
                    {
                        DatabaseService.BeginTransaction();
                        if (user.LoginAttempt >= 5)
                        {
                            user.LoginAttempt = 0;
                        }

                        var hash = StringHelper.GetHash(password, user.Salt, MD5.Create());
                        if (hash != user.Hash)
                        {
                            status = UserLoginStatus.PasswordMismatch;
                            user.LoginAttempt++;
                            user.LastFailedLogin = DateTime.UtcNow;
                        }
                        else if (!user.IsVerified)
                        {
                            status = UserLoginStatus.Unverified;
                            user.LastFailedLogin = DateTime.UtcNow;
                        }
                        else if (!user.Active)
                        {
                            status = UserLoginStatus.UserDisabled;
                            user.LastFailedLogin = DateTime.UtcNow;
                        }
                        else
                        {
                            user.LastLogin = DateTime.UtcNow;
                        }
                        DatabaseService.Save(user);
                        DatabaseService.CommitTransaction();
                    }
                    catch (System.Exception)
                    {
                        DatabaseService.RollBackTransaction();
                        status = UserLoginStatus.UnknownException;
                    }
                    
                }
                
            }
            else
            {
                status = UserLoginStatus.UserDoesNotExist;
            }
            return status;
        }

        public UserChallengeStatus ChallengeUser(long id, string challenge)
        {
            UserChallengeStatus status;

            var user = DatabaseService.Get<User>(id);
            if (user != null)
            {
                if (!user.IsVerified)
                {
                    if (user.UserChallenge != null)
                    {
                        if (user.UserChallenge.CreatedAt.AddDays(1) <= DateTime.UtcNow)
                        {
                            status = UserChallengeStatus.Expired;
                        }
                        else if (user.UserChallenge.TryCount >= 5)
                        {
                            status = UserChallengeStatus.OverLimit;
                        }
                        else
                        {
                            try
                            {
                                DatabaseService.BeginTransaction();
                                if (user.UserChallenge.Challenge.Equals(challenge, StringComparison.OrdinalIgnoreCase))
                                {
                                    user.IsVerified = true;
                                    user.UserChallenge.VerifiedAt = DateTime.UtcNow;
                                    status = UserChallengeStatus.Successed;
                                    DatabaseService.Save(user);
                                    DatabaseService.Save(user.UserChallenge);
                                }
                                else
                                {
                                    user.UserChallenge.TryCount++;
                                    status = UserChallengeStatus.Mismatch;
                                    DatabaseService.Save(user.UserChallenge);
                                }
                                DatabaseService.CommitTransaction();
                            }
                            catch (System.Exception)
                            {
                                DatabaseService.RollBackTransaction();
                                status = UserChallengeStatus.UnknownException;
                            }
                            
                        }
                    }
                    else
                    {
                        var userChallenge = new UserChallenge(StringHelper.GenerateSalt(64));
                        userChallenge.User = user;
                        DatabaseService.Save(userChallenge);
                        status = UserChallengeStatus.TryAgain;
                    }
                }
                else
                {
                    status = UserChallengeStatus.AlreadyVerified;
                }
            }
            else
            {
                status = UserChallengeStatus.NotFound;
            }
            return status;
        }

        void IUserService.CreateAddress(User user, Address newAddress)
        {
            SingleTransactionAction(CreateAddress, user, newAddress);
        }

        public void CreateAddress(User user, Address newAddress)
        {
            var theUser = DatabaseService.Get<User>(user.Id);
            if (theUser != null)
            {
                theUser.Address = ((AddressService) _addressService).CreateAddress(newAddress);
                DatabaseService.Save(theUser);
            }
            else
            {
                throw new EntityNotFoundException<User>(user);
            }

        }

        public User GetUser(long id)
        {
            return DatabaseService.Get<User>(id);
        }
    }
}
