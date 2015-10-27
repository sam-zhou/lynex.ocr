using Lynex.Notification.Common.Model;

namespace Lynex.Notification.Email.Model
{
    public interface IEmailModel: INotificationModel
    {
        string CC { get; set; }

        string BCC { get; set; }

        string Subject { get; set; }

        bool IsHtml { get; set; }
    }
}
