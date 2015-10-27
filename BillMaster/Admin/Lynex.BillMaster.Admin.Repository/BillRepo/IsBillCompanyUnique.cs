using System.Linq;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database;
using NHibernate;
using NHibernate.Linq;

namespace Lynex.BillMaster.Admin.Repository.BillRepo
{
    public class IsBillCompanyUnique : IGetItemQuery<bool>
    {
        private readonly string _name;

        public IsBillCompanyUnique(string name)
        {
            _name = name;
        }

        public bool Execute(ISession session)
        {
            return !session.Query<BillingCompany>().Any(x => x.Name == _name);
        }
    }
}
