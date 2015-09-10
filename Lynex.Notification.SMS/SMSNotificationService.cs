using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Domain.DbModels.Interface;
using Lynex.BillMaster.Model.Settings.Interface;
using Lynex.Notification.Common;
using Lynex.Notification.SMS.Model;

namespace Lynex.Notification.SMS
{
    public interface ISMSNotificationService : INotificationService
    {
        
    }

    public class SMSNotificationService : NotificationService<SMSModel>, ISMSNotificationService
    {
        private readonly ISMSNotificationSettings _settings;

        public SMSNotificationService(ISMSNotificationSettings settings)
            : base(settings.SMSFormatType)
        {
            _settings = settings;
        }


        public override bool SendNotification(IUser receiver)
        {
            var item = FormatProvider.GetFormattedModel(receiver);
            
            return true;
        }
    }
}
