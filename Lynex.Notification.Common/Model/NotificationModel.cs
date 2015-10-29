using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.Notification.Common.Model
{
    public abstract class NotificationModel : INotificationModel
    {
        public string Template { get; private set; }

        public string Body { get; protected set; }

        public IUser Receiver { get; protected set; }

        protected NotificationModel(IUser receiver, string template)
        {
            Receiver = receiver;
            Template = template;
        }
    }
}
