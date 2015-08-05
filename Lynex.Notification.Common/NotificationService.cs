using Lynex.Model.Domain.DbModels;
using Lynex.Model.Enum;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.Common
{
    public interface INotificationService
    {
        bool SendNotification(TestResult dosage, User receiver);
    }

    public abstract class NotificationService<TModel>: INotificationService where TModel: class, INotificationModel
    {
        protected INotificationFormatProvider<TModel> FormatProvider { get; set; }

        protected NotificationService(FormatType type)
        {
            FormatProvider = new NotificationFormatProvider<TModel>(type);
        }

        public abstract bool SendNotification(TestResult dosage, User receiver);
    }
}
