using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Domain.DbModels.Interface;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.SMS.Model
{
    public class SMSModel : NotificationModel, ISMSModel
    {
        public SMSModel(IUser sender, IUser receiver, string template)
            : base(sender, receiver, template)
        {
            
        }
        
    }
}
