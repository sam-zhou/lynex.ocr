using System.Collections.Generic;
using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;

namespace WCC.Repositories.Interface.Repositories
{
    public interface IDosageRepository
    {
        Dosage CreateDosage(User user, TestResult test, List<DosageDetail> dosageDetails);
        Dosage AcknowledgeDosage(Dosage dosage, User user);
        Dosage ViewDosage(long id);
        Dosage GetCurrentDosageByUser(User user);

        Notification CreateNotification(Dosage dosage, User user, NotificationType type);
    }
}
