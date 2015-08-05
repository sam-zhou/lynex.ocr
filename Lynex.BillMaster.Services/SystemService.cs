using WCC.Model.Domain;
using WCC.Repositories.Interface.Repositories;
using WCC.Services.Interface;

namespace WCC.Services
{
    public class SystemService : BaseService, ISystemService
    {
        public SystemService(IWCCMainRepository wccMainRepository) : base(wccMainRepository)
        {
        }

        public SystemSetting GetSystemSettings()
        {
            return WCCMainRepository.GetSystemSettings();
        }

        public void ResetDatabase()
        {
            WCCMainRepository.ResetDatabase();
        }
    }
}
