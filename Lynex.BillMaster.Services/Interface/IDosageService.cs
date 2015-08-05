using System.Collections.Generic;
using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;

namespace WCC.Services.Interface
{
    public interface IDosageService
    {
        Dosage CreateDosage(User user, TestResult test, List<DosageDetail> dosageDetails);
        Dosage AcknowledgeDosage(Dosage dosage, User user);
        Dosage ViewDosage(long id);
        Dosage GetCurrentDosageByUser(User user);

        DosageDetail CreateDosageDetail(TestResult testResult, DaysOfWeek day, double value);
        Notification CreateNotification(Dosage dosage, User user, NotificationType type);
    }
}
