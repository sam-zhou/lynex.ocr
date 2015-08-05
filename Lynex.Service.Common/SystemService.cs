using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Database.Common;
using Lynex.Service.Common.Interface;

namespace Lynex.Service.Common
{
    public class SystemService: ISystemService
    {
        public SystemService(IDatabaseService dbService)
        {
            DatabaseService = dbService;
        }
        private IDatabaseService DatabaseService { get; }
        public void ResetDatabase()
        {
            DatabaseService.ResetDatabase();
        }
    }
}
