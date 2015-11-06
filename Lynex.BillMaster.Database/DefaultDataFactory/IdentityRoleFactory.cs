using System.Collections.Generic;
using System.Reflection;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database.DefaultDataFactory;
using Lynex.Common.Model.AspNet.Identity;
using NHibernate;

namespace Lynex.BillMaster.Database.DefaultDataFactory
{
    internal class IdentityRoleFactory : DefaultDataFactoryBase<IdentityRole>
    {
        public IdentityRoleFactory(ISession session, Assembly assembly) : base(session, assembly)
        {
        }

        protected override IEnumerable<IdentityRole> GetData(Assembly assembly = null)
        {
            yield return IdentityRole.Guest;
            yield return IdentityRole.User;
            yield return IdentityRole.Administrator;
        }
    }
}
