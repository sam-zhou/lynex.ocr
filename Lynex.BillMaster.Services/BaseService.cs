using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCC.Model.Domain;
using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;
using WCC.Repositories.Interface.Repositories;
using WCC.Services.Interface;

namespace WCC.Services
{
    public abstract class BaseService : IDisposable
    {
        protected readonly IWCCMainRepository WCCMainRepository;

        protected BaseService(IWCCMainRepository wccMainRepository)
        {
            WCCMainRepository = wccMainRepository;
        }

        public void Dispose()
        {
            if (WCCMainRepository != null)
            {
                WCCMainRepository.Dispose();
            }
        }

        //public void BeginTrans()
        //{
        //    WCCMainRepository.BeginTrans();
        //}

        //public void CommitTrans()
        //{
        //    WCCMainRepository.CommitTrans();
        //}

        //public void RollBackTrans()
        //{
        //    WCCMainRepository.RollBackTrans();
        //}

        //public Dosage CreateDosage(User user, TestResult test, List<DosageDetail> dosageDetails)
        //{
        //    return WCCMainRepository.CreateDosage(user, test, dosageDetails);
        //}

        //public Dosage AcknowledgeDosage(Dosage dosage, User user)
        //{
        //    return WCCMainRepository.AcknowledgeDosage(dosage, user);
        //}

        //public Dosage ViewDosage(long id)
        //{
        //    return WCCMainRepository.ViewDosage(id);
        //}

        //public Dosage GetCurrentDosageByUser(User user)
        //{
        //    return WCCMainRepository.GetCurrentDosageByUser(user);
        //}

        //public DosageDetail CreateDosageDetail(TestResult testResult, DaysOfWeek day, double value)
        //{
        //    return WCCMainRepository.CreateDosageDetail(testResult, day, value);
        //}

        //public Notification CreateNotification(Dosage dosage, User user, NotificationType type)
        //{
        //    return WCCMainRepository.CreateNotification(dosage, user, type);
        //}

        //public SystemSetting GetSystemSettings()
        //{
        //    return WCCMainRepository.GetSystemSettings();
        //}

        //public void ResetDatabase()
        //{
        //    WCCMainRepository.ResetDatabase();
        //}

        //public IEnumerable<TestResult> GetTestResultsByUser(User user)
        //{
        //    return WCCMainRepository.GetTestResultsByUser(user);
        //}

        //public TestResult GetCurrentTestResultByUser(User user)
        //{
        //    return WCCMainRepository.GetCurrentTestResultByUser(user);
        //}

        //public TestResult CreateTestResult(User user, double pt, double inr, int labId)
        //{
        //    return WCCMainRepository.CreateTestResult(user, pt, inr, labId);
        //}

        //public void ViewTestResult(TestResult testResult)
        //{
        //    WCCMainRepository.ViewTestResult(testResult);
        //}

        //public User GetUserId(long userId)
        //{
        //    return WCCMainRepository.GetUserId(userId);
        //}

        //public User GetUserByMedWayId(long medwayId)
        //{
        //    return WCCMainRepository.GetUserByMedWayId(medwayId);
        //}

        //public User CreateUser(long medwayUserId, string email, string mobile)
        //{
        //    return WCCMainRepository.CreateUser(medwayUserId, email, mobile);
        //}

        //public UserChallengeStatus VerifyUser(long userId, string challenge)
        //{
        //    return WCCMainRepository.VerifyUser(userId, challenge);
        //}

        //public void AddPatient(User user, User patient)
        //{
        //    WCCMainRepository.AddPatient(user, patient);
        //}

        //public void RemovePatient(User user, User patient)
        //{
        //    WCCMainRepository.RemovePatient(user, patient);
        //}

        //public void Login(User user)
        //{
        //    WCCMainRepository.Login(user);
        //}
    }
}
