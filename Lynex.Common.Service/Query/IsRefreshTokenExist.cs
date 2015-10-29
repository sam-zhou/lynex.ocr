using System.Linq;
using Lynex.Common.Database;
using Lynex.Common.Model.DbModel;
using NHibernate;
using NHibernate.Linq;

namespace Lynex.Common.Service.Query
{
    public class IsRefreshTokenExist: IGetItemQuery<bool>
    {
        private readonly RefreshToken _refreshToken;

        public IsRefreshTokenExist(RefreshToken refreshToken)
        {
            _refreshToken = refreshToken;
        }

        public bool Execute(ISession session)
        {
            return session.Query<RefreshToken>()
                .Any(x => x.Subject == _refreshToken.Subject && x.ClientId == _refreshToken.ClientId);
        }
    }
}
