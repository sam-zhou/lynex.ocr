using Lynex.Common.Service.Interface;
using Lynex.Database.Common;

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
