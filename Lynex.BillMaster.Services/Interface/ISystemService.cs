using WCC.Model.Domain;

namespace WCC.Services.Interface
{
    public interface ISystemService
    {
        SystemSetting GetSystemSettings();
        void ResetDatabase();
    }
}
