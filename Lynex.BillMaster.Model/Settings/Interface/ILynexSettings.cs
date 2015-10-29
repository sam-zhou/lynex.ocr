using Lynex.Common.Model.Settings.Interface;

namespace Lynex.BillMaster.Model.Settings.Interface
{
    public interface ILynexSettings : IEmailNotificationSettings, ISMSNotificationSettings
    {
    }
}
