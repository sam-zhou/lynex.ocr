using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database;
using NHibernate;
using NHibernate.Criterion;

namespace Lynex.BillMaster.Repository.BillRepo
{
    public class GetBillsByUser: IGetItemsQuery<Bill>
    {
        private readonly long _userId;

        public GetBillsByUser(long userId)
        {
            _userId = userId;
        }

        public IEnumerable<Bill> Execute(ISession session)
        {
            var output = session.CreateCriteria<Bill>().Add(Restrictions.Eq("UserId", _userId)).List<Bill>();
            return output;
        }
    }
}
