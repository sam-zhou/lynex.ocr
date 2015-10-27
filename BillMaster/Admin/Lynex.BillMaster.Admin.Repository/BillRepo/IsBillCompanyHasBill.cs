using System.Linq;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database;
using NHibernate;
using NHibernate.Linq;

namespace Lynex.BillMaster.Admin.Repository.BillRepo
{
    public class IsBillCompanyHasBill : IGetItemQuery<bool>
    {
        private readonly BillingCompany _billingCompany;

        public IsBillCompanyHasBill(BillingCompany billingCompany)
        {
            _billingCompany = billingCompany;
        }

        public bool Execute(ISession session)
        {
            return session.Query<Bill>().Any(x => x.Company == _billingCompany);
        }
    }
}
