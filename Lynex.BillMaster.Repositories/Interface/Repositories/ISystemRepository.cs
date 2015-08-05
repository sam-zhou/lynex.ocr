using WCC.Model.Domain;

namespace WCC.Repositories.Interface.Repositories
{
    public interface ISystemRepository
    {
        SystemSetting GetSystemSettings();
        void ResetDatabase();
    }
}
