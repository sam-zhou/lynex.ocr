using System.Collections.Generic;
using System.Threading.Tasks;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Model.Enum;

namespace Lynex.Common.Service.Interface
{
    public interface IAuthenticationService
    {
        Client FindClient(string clientId);

        Client AddClient(string clientId, string secret, string name, ApplicationTypes type, bool active, int lifeTime, string allowedOrigin = "*");

        bool RemoveClient(string clientId);

        bool AddRefreshToken(RefreshToken token);

        bool RemoveRefreshToken(string refreshTokenId);

        bool RemoveRefreshToken(RefreshToken refreshToken);

        RefreshToken FindRefreshToken(string refreshTokenId);

        IEnumerable<RefreshToken> GetAllRefreshTokens();

    }
}
