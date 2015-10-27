using System.Collections.Generic;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Policy;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Database.DefaultDataFactory;
using Lynex.Common.Extension;
using Lynex.Common.Model.AspNet.Identity;
using Lynex.Common.Model.DbModel.Interface;
using NHibernate;

namespace Lynex.BillMaster.Database.DefaultDataFactory
{
    internal class DevelopmentDataFactory : DefaultDataFactoryBase<IDbModel>
    {
        public DevelopmentDataFactory(ISession session, Assembly assembly) : base(session, assembly)
        {
        }

        protected override IEnumerable<IDbModel> GetData(Assembly assembly = null)
        {
            return new List<IDbModel>();
            //var salt = StringHelper.GenerateSalt();
            //var hash = StringHelper.GetHash("jukfrg", salt, MD5.Create());

            //var user = new ApplicationUser();
            //{
            //    Email = "samzhou.it@gmail.com",
            //    LastName = "Zhou",
            //    FirstName = "Sam",
            //    Mobile = "0430501022",
            //    Salt = salt,
            //    Hash = hash,
            //    Active = true,
            //    Id= "cb55dd1f-f6d4-4ccc-bc3e-50a73e94b825"
            //};

            

            //var challenge = new UserChallenge(StringHelper.GenerateSalt(64));
            //user.UserChallenge = challenge;

            //var wallet = new Wallet();
            //user.Wallet = wallet;

            //yield return user;
            ////yield return challenge;
            ////yield return wallet;



            //var address = new Address
            //{
            //    AddressLine1 = "8 Arklow Glen",
            //    AddressLine2 = "",
            //    AddressLine3 = "",
            //    Country = "Australia",
            //    PostCode = "6155",
            //    State = "Western Australia",
            //    Suburb = "Canning Vale"
            //};

            //yield return address;


            //var billCompany = new BillingCompany
            //{
            //    Address = address,
            //    BillTypes = BillType.Gas | BillType.Electricity,
            //    Name = "Synergy"
            //};

            //yield return billCompany;
        }
    }
}
