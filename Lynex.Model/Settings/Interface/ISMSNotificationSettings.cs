using Lynex.BillMaster.Model.Enum;

namespace Lynex.BillMaster.Model.Settings.Interface
{
    public interface ISMSNotificationSettings : INotificationSettings
    {
        string SMSServer { get; }

        string SMSUser { get; }

        string SMSPassword { get; }

        string SMSSenderNumber { get; }

        string SMSSenderName { get; }

        FormatType SMSFormatType { get; }
    }
}
