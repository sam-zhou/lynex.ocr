using Lynex.Common.Model.DbModel.Interface;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.SMS.Model
{
    public class SMSModel : NotificationModel, ISMSModel
    {
        public SMSModel(IUser receiver, string template)
            : base(receiver, template)
        {
            
        }
        
    }
}
