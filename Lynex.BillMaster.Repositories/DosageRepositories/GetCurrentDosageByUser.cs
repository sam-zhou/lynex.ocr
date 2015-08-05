using System.Linq;
using NHibernate;
using WCC.Model.Domain.DbModels;
using WCC.Repositories.Interface;

namespace WCC.Repositories.DosageRepositories
{
    internal class GetCurrentDosageByUser : IGetQuery<Dosage>
    {
        private readonly User _user;

        public GetCurrentDosageByUser(User user)
        {
            _user = user;
        }

        public Dosage Execute(ISession session)
        {
            Dosage d = null;
            TestResult t = null;
            return session.QueryOver(() => d).JoinAlias(() => d.TestResult, () => t).Where(() => t.User == _user).OrderBy(q => q.Id).Desc.Take(1).List().FirstOrDefault();
        }
    }
}
