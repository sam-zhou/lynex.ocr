using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Lynex.Common.Database;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Model.Enum;
using Lynex.Common.Service.Interface;
using Lynex.Common.Service.Query;
using NHibernate.Linq;
using Lynex.Common.Extension;

namespace Lynex.Common.Service
{
    public class AuthenticationService : BaseService, IAuthenticationService
    {
        public AuthenticationService(IDatabaseService dbService) : base(dbService)
        {
        }


        public Client FindClient(string clientId)
        {
            return DatabaseService.Session.Query<Client>().FirstOrDefault(q => q.ClientId == clientId);
        }

        public Client AddClient(string clientId, string secret, string name, ApplicationTypes type, bool active, int lifeTime,
            string allowedOrigin = "*")
        {
            var client = new Client
            {
                ClientId = clientId,
                Active = active,
                Secret = StringHelper.GetHash(secret),
                AllowedOrigin = allowedOrigin,
                ApplicationType = type,
                Name = name,
                RefreshTokenLifeTime = lifeTime
            };
            DatabaseService.Save(client);
            return client;
        }

        public bool RemoveClient(string clientId)
        {
            var client = DatabaseService.Session.Query<Client>().FirstOrDefault(q => q.ClientId == clientId);
            DatabaseService.Save(client);
            return true;
        }


        public bool AddRefreshToken(RefreshToken token)
        {
            var exist = DatabaseService.Get(new IsRefreshTokenExist(token));
            try
            {
                DatabaseService.BeginTransaction();
                if (exist)
                {
                    RemoveRefreshToken(token);
                }
                DatabaseService.Save(token);
                DatabaseService.CommitTransaction();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveRefreshToken(string refreshTokenId)
        {
            var exist = DatabaseService.Get<RefreshToken>(refreshTokenId);
            try
            {
                if (exist != null)
                {
                    DatabaseService.Delete(exist);
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool RemoveRefreshToken(RefreshToken refreshToken)
        {
            try
            {
                DatabaseService.Delete(refreshToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public RefreshToken FindRefreshToken(string refreshTokenId)
        {
            return DatabaseService.Get<RefreshToken>(refreshTokenId);
        }

        public IEnumerable<RefreshToken> GetAllRefreshTokens()
        {
            return DatabaseService.GetAll<RefreshToken>();
        }
    }
}
