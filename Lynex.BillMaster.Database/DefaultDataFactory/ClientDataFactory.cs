using System.Collections.Generic;
using System.Reflection;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database.DefaultDataFactory;
using Lynex.Common.Extension;
using Lynex.Common.Model.AspNet.Identity;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Model.Enum;
using NHibernate;

namespace Lynex.BillMaster.Database.DefaultDataFactory
{
    internal class ClientDataFactory : DefaultDataFactoryBase<Client>
    {
        public ClientDataFactory(ISession session, Assembly assembly) : base(session, assembly)
        {
        }

        protected override IEnumerable<Client> GetData(Assembly assembly = null)
        {
            yield return new Client
            {
                ClientId = "fgJD57mSbYTHG88HwEaQsN3CjV2slKZidl1s",
                Active = true,
                AllowedOrigin = "*",
                ApplicationType = ApplicationTypes.ConsoleApp,
                Name = "Lynex.Common.Console",
                RefreshTokenLifeTime = 20,
                Secret = StringHelper.GetHash("91AE616A66D6E530C6D519C07D29F12C0D8939523DE35FCC921E1508C72A60CE")
            };
            
        }
    }
}
