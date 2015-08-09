using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Database.DefaultDataFactory;
using Lynex.Common.Extension;
using Lynex.Common.Model.DbModel.Interface;
using NHibernate;

namespace Lynex.BillMaster.Database.DefaultDataFactory
{
    internal class DevelopmentDataFactory : DefaultDataFactoryBase<IBaseEntity>
    {
        public DevelopmentDataFactory(ISession session, Assembly assembly) : base(session, assembly)
        {
        }

        protected override IEnumerable<IBaseEntity> GetData(Assembly assembly = null)
        {
            var salt = StringHelper.GenerateSalt();
            var hash = StringHelper.GetHash("jukfrg", salt, MD5.Create());

            var user = new User
            {
                Email = "samzhou.it@gmail.com",
                LastName = "Zhou",
                FirstName = "Sam",
                Mobile = "0430501022",
                Salt = salt,
                Hash = hash,
                Active = true,
                PermissionRole = PermissionRole.User
            };

            

            var challenge = new UserChallenge(StringHelper.GenerateSalt(64)) { User = user };
            user.UserChallenge = challenge;

            var wallet = new Wallet { User = user };
            user.Wallet = wallet;

            yield return user;
            yield return challenge;
            yield return wallet;



            var address = new Address
            {
                AddressLine1 = "8 Arklow Glen",
                AddressLine2 = "",
                AddressLine3 = "",
                Country = "Australia",
                PostCode = "6155",
                State = "Western Australia",
                Suburb = "Canning Vale"
            };

            yield return address;


            var billCompany = new BillingCompany
            {
                Address = address,
                BillTypes = BillType.Gas | BillType.Electricity,
                Name = "Synergy"
            };

            yield return billCompany;
        }
    }
}
