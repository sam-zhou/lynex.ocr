using System.Linq;
using NHibernate;
using NHibernate.Linq;
using WCC.Model.Domain.DbModels;
using WCC.Repositories.Interface;

namespace WCC.Repositories.UserRepositories
{
    internal class GetUserByMedWayId: IGetQuery<User>
    {
        private readonly long _medwayId;

        public GetUserByMedWayId(long medwayId)
        {
            _medwayId = medwayId;
        }

        public User Execute(ISession session)
        {
            return session.QueryOver<User>().Where(q => q.MedWayUserId == _medwayId).Take(1).List().FirstOrDefault();
        }
    }
}
