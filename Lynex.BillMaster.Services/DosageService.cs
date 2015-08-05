using System.Collections.Generic;
using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;
using WCC.Repositories.Interface.Repositories;
using WCC.Services.Interface;


namespace WCC.Services
{
    public class DosageService : BaseService, IDosageService
    {
        public DosageService(IWCCMainRepository wccMainRepository) : base(wccMainRepository)
        {
        }

        public Dosage CreateDosage(User user, TestResult test, List<DosageDetail> dosageDetails)
        {
            return WCCMainRepository.CreateDosage(user, test, dosageDetails);
        }

        public Dosage AcknowledgeDosage(Dosage dosage, User user)
        {
            return WCCMainRepository.AcknowledgeDosage(dosage, user);
        }

        public Dosage ViewDosage(long id)
        {
            return WCCMainRepository.ViewDosage(id);
        }

        public Dosage GetCurrentDosageByUser(User user)
        {
            return WCCMainRepository.GetCurrentDosageByUser(user);
        }

        public DosageDetail CreateDosageDetail(TestResult testResult, DaysOfWeek day, double value)
        {
            return null; //WCCMainRepository.CreateDosageDetail(testResult, day, value);
        }

        public Notification CreateNotification(Dosage dosage, User user, NotificationType type)
        {
            return WCCMainRepository.CreateNotification(dosage, user, type);
        }
    }
}
