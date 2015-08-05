using Lynex.Model.Domain.DbModels;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.SMS.Model
{
    public class SMSModel : NotificationModel, ISMSModel
    {
        public SMSModel(TestResult testResult, User receiver, string template)
            : base(testResult, receiver, template)
        {
            
        }
    }
}
