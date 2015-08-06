using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Extension;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Repository.BillRepo;
using Lynex.BillMaster.Service.Interface;
using Lynex.Common.Database;
using Lynex.Common.Service;

namespace Lynex.BillMaster.Service
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IDatabaseService dbService) : base(dbService)
        {
        }

        public User RegisterUser(string email, string password, string mobile)
        {
            var salt = PasswordHelper.GenerateSalt();
            var hash = PasswordHelper.GetHash(password, salt, MD5.Create());

            var user = new User
            {
                Email = email,
                Mobile = mobile,
                Salt = salt,
                Hash = hash

            };
            DatabaseService.Save(user);
            return user;
        }

        public bool Login(string query, string password)
        {
            throw new NotImplementedException();
        }
    }
}
