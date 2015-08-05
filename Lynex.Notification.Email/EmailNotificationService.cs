using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using Lynex.Model.Domain.DbModels;
using Lynex.Model.Settings.Interface;
using Lynex.Notification.Common;
using Lynex.Notification.Email.Model;

namespace Lynex.Notification.Email
{
    public interface IEmailNotificationService : INotificationService
    {
        
    }

    public class EmailNotificationService : NotificationService<EmailModel>, IEmailNotificationService
    {
        private readonly IEmailNotificationSettings _setting;

        public EmailNotificationService(ILynexSettings setting)
            : base(setting.EmailFormatType)
        {
            _setting = setting;
        }


        public override bool SendNotification(User receiver)
        {
            var item = FormatProvider.GetFormattedModel(testResult, receiver);

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Port = _setting.EmailPort;
                    client.Host = _setting.EmailServer;
                    client.EnableSsl = true;
                    client.Timeout = 10000;
                    client.DeliveryMethod = SmtpDeliveryMethod.Network;
                    client.UseDefaultCredentials = false;
                    client.Credentials = new NetworkCredential(_setting.EmailUser, _setting.EmailPassword);

                    var message = new MailMessage(_setting.EmailSenderEmail, receiver.Email, item.Subject, item.Body)
                    {
                        IsBodyHtml = item.IsHtml,
                        BodyEncoding = Encoding.UTF8,
                        DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure
                    };

                    client.Send(message);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
