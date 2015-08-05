using Lynex.Model.Domain.DbModels;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.Email.Model
{
    public class EmailModel : NotificationModel, IEmailModel
    {
        public string CC { get; set; }

        public string BCC { get; set; }

        public string Subject { get; set; }

        public bool IsHtml { get; set; }

        public EmailModel(TestResult testResult, User receiver, string template)
            : base(testResult, receiver, template)
        {
            Subject = string.Format("You TestResult {0:dd/MM/yyyy} is availavle for view", testResult.LocalCreatedAt);
            Body = template;
        }
    }
}
