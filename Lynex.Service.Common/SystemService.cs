using Lynex.Common.Database;
using Lynex.Common.Service.Interface;

namespace Lynex.Common.Service
{
    public class SystemService: BaseService, ISystemService
    {
        public SystemService(IDatabaseService dbService) : base(dbService)
        {
        }

        public void ResetDatabase()
        {
            DatabaseService.ResetDatabase();
        }
    }
}
