using Lynex.Common.Model.DbModel.Interface;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.Email.Model
{
    public class EmailModel : NotificationModel, IEmailModel
    {
        public string CC { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        public bool IsHtml { get; set; }

        public EmailModel(IUser receiver, string template)
            : base(receiver, template)
        {
            Body = template;
        }
    }
}
