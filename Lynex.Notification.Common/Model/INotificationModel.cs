using Lynex.BillMaster.Model.Domain.DbModels;

namespace Lynex.Notification.Common.Model
{
    public interface INotificationModel
    {
        User Receiver { get; }

        string Body { get;  }

        TestResult TestResult { get; }

        User User { get; }
    }
}
