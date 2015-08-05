using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCC.Model.Domain;
using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;
using WCC.Repositories.BackendService;
using WCC.Repositories.DosageRepositories;
using WCC.Repositories.Helpers;
using WCC.Repositories.Interface.Repositories;
using WCC.Repositories.TestResultRepositories;
using WCC.Repositories.UserRepositories;

namespace WCC.Repositories
{
    public class WCCMainRepository : IWCCMainRepository
    {
        private static readonly object Obj = new object();

        private static IDatabaseService _databaseService;

        internal static IDatabaseService DatabaseService
        {
            get
            {
                lock (Obj)
                {
                    if (_databaseService == null)
                    {
                        _databaseService = new DatabaseService();
                    }
                    return _databaseService;
                }
                
            }
        }

        public void Dispose()
        {
            if (DatabaseService != null)
            {
                DatabaseService.Dispose();
            }
        }

        public void BeginTrans()
        {
            DatabaseService.BeginTransaction();
        }

        public void CommitTrans()
        {
            DatabaseService.CommitTransaction();
        }

        public void RollBackTrans()
        {
            DatabaseService.RollBackTransaction();
        }

        #region IDosageRepository
        public Dosage AcknowledgeDosage(Dosage dosage, User user)
        {
            var theDosage = DatabaseService.Get<Dosage>(dosage.Id);
            if (theDosage != null && user != null)
            {
                if (theDosage.Acknowledgements != null && theDosage.Acknowledgements.All(q => q.User.Id != user.Id))
                {
                    DatabaseService.BeginTransaction();
                    var acknowledgement = new Acknowledgement
                    {
                        AcknowledgedAt = DateTime.UtcNow,
                        User = user,
                        Dosage = theDosage
                    };

                    theDosage.Acknowledgements.Add(acknowledgement);
                    DatabaseService.Save(acknowledgement);
                    DatabaseService.CommitTransaction();
                }
            }
            return theDosage;
        }

        public Dosage GetCurrentDosageByUser(User user)
        {
            return DatabaseService.Get(new GetCurrentDosageByUser(user));
        }

        public Dosage ViewDosage(long id)
        {
            DatabaseService.BeginTransaction();
            var dateTime = DateTime.UtcNow;
            var dosage = DatabaseService.Get<Dosage>(id);
            if (dosage != null)
            {
                dosage.ViewedAt = dateTime;
                DatabaseService.Save(dosage);
            }
            DatabaseService.CommitTransaction();
            return dosage;
        }



        public Dosage CreateDosage(User user, TestResult test, List<DosageDetail> dosageDetails)
        {
            

            var theTest = DatabaseService.Get<TestResult>(test.Id);

            if (theTest != null)
            {
                DatabaseService.BeginTransaction();

                foreach (var oldDosage in theTest.Dosages)
                {
                    oldDosage.IsValid = false;
                    DatabaseService.Save(oldDosage);
                }

                var dosage = new Dosage(dosageDetails)
                {
                    TestResult = test
                };

                test.Dosages.Add(dosage);
                //DatabaseService.Save(dosage);

                
                DatabaseService.Save(dosage);

                DatabaseService.CommitTransaction();
                return dosage;
            }
            return null;
        }

        public DosageDetail CreateDosageDetail(TestResult testResult, DaysOfWeek day, double value)
        {
            DatabaseService.BeginTransaction();
            var dosageDetail = new DosageDetail
            {
                Day = day,
                Value = value
            };

            if (testResult != null)
            {

                //testResult.DosageDetails.Add(dosageDetail);
                //DatabaseService.Save(dosageDetail);
            }
            DatabaseService.CommitTransaction();
            return dosageDetail;
        }

        public Notification CreateNotification(Dosage dosage, User user, NotificationType type)
        {
            DatabaseService.BeginTransaction();
            var notification = new Notification
            {
                Type = type,
                NotifiedAt = DateTime.UtcNow,
            };
            
            if (dosage != null && user != null)
            {

                dosage.Notifications.Add(notification);
                user.Notifications.Add(notification);
                DatabaseService.Save(notification);
            }
            DatabaseService.CommitTransaction();
            return notification;
        }

        #endregion

        #region ISystemRepository
        public void ResetDatabase()
        {
            DatabaseService.ResetDatabase();
        }

        public SystemSetting GetSystemSettings()
        {
            var items = DatabaseService.GetAll<Setting>();
            return new SystemSetting(items);
        }
        #endregion

        #region ITestResultRepository
        public IEnumerable<TestResult> GetTestResultsByUser(User user)
        {
            return DatabaseService.Get(new GetTestResultsByUser(user));
        }

        public TestResult GetCurrentTestResultByUser(User user)
        {
            return DatabaseService.Get(new GetCurrentTestResultByUser(user));
        }

        public TestResult CreateTestResult(User user, double pt, double inr, int labId)
        {
            var testResult = new TestResult
            {
                User = user,
                PT = pt,
                INR = inr,
                LabId = labId
            };

            DatabaseService.Save(testResult);
            return testResult;
        }

        public void ViewTestResult(TestResult testResult)
        {
            var foundTestResult = DatabaseService.Get<TestResult>(testResult.Id);
            if (foundTestResult != null)
            {
                DatabaseService.BeginTransaction();
                foundTestResult.ViewedAt = DateTime.UtcNow;
                DatabaseService.Save(foundTestResult);
                DatabaseService.CommitTransaction();
            }
        }
        #endregion

        #region IUserRepository
        public User CreateUser(long medwayUserId, string email, string mobile)
        {
            DatabaseService.BeginTransaction();


            var user = new User
            {
                MedWayUserId = medwayUserId,
                Email = email,
                Mobile = mobile
            };


            DatabaseService.Save(user);
            CreateNewUserChallenge(user);
            DatabaseService.CommitTransaction();
            return user;
        }

        private void CreateNewUserChallenge(User user)
        {
            DatabaseService.BeginTransaction();
            if (user.UserChallenge != null)
            {
                DatabaseService.Delete(user.UserChallenge);
            }

            var userChallenge = new UserChallenge
            {
                Challenge = StringHelper.GetRandomString(),
                //User = user
            };

            user.UserChallenge = userChallenge;
            DatabaseService.Save(userChallenge);
            DatabaseService.CommitTransaction();
        }

        public User GetUserId(long userId)
        {
            return DatabaseService.Get<User>(userId);
        }

        public User GetUserByMedWayId(long medwayId)
        {
            return DatabaseService.Get(new GetUserByMedWayId(medwayId));
        }

        public void Login(User user)
        {
            user.LastLogin = DateTime.UtcNow;
            DatabaseService.Save(user);
        }

        public void AddPatient(User user, User patient)
        {
            if (!user.Patients.Contains(patient))
            {
                user.Patients.Add(patient);
                DatabaseService.Save(user);
            }
        }

        public void RemovePatient(User user, User patient)
        {
            if (user.Patients.Contains(patient))
            {
                user.Patients.Remove(patient);
                DatabaseService.Save(user);
            }
        }

        public UserChallengeStatus VerifyUser(long userId, string challenge)
        {
            var status = UserChallengeStatus.Created;
            DatabaseService.BeginTransaction();
            var user = DatabaseService.Get<User>(userId);
            if (user != null)
            {
                var userChallenge = user.UserChallenge;
                if (userChallenge != null)
                {
                    if (userChallenge.CreatedAt < DateTime.UtcNow.AddHours(-1))
                    {
                        CreateNewUserChallenge(user);
                        status = UserChallengeStatus.Expired;
                    }
                    else if (userChallenge.Challenge != challenge)
                    {
                        userChallenge.TryCount++;

                        if (userChallenge.TryCount >= 3)
                        {
                            CreateNewUserChallenge(user);
                            status = UserChallengeStatus.OverLimit;
                        }
                        else
                        {
                            DatabaseService.Save(userChallenge);
                            status = UserChallengeStatus.Mismatch;
                        }
                    }
                    else
                    {
                        user.IsVerified = true;

                        DatabaseService.Delete(userChallenge);
                        DatabaseService.Save(user);
                        status = UserChallengeStatus.Successed;
                    }
                }
                else
                {
                    status = UserChallengeStatus.NotFound;
                }
            }
            DatabaseService.CommitTransaction();
            return status;
        }
        #endregion
    }
}
