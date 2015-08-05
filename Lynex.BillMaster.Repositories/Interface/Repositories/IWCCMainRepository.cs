using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WCC.Repositories.Interface.Repositories
{
    public interface IWCCMainRepository : IDisposable, IBaseRepository, IDosageRepository, ISystemRepository, ITestResultRepository, IUserRepository
    {
    }
}
