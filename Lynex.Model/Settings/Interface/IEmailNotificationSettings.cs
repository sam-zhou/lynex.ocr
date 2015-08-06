using Lynex.BillMaster.Model.Enum;

namespace Lynex.BillMaster.Model.Settings.Interface
{
    public interface IEmailNotificationSettings : INotificationSettings
    {
        string EmailServer { get; }

        int EmailPort { get; }

        string EmailUser { get; }

        string EmailPassword { get; }

        string EmailSenderEmail { get; }

        FormatType EmailFormatType { get; }
    }
}
