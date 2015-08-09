using System.Collections.Generic;
using System.Reflection;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database.DefaultDataFactory;
using NHibernate;

namespace Lynex.BillMaster.Database.DefaultDataFactory
{
    internal class PermissionRoleFactory : DefaultDataFactoryBase<PermissionRole>
    {
        public PermissionRoleFactory(ISession session, Assembly assembly) : base(session, assembly)
        {
        }

        protected override IEnumerable<PermissionRole> GetData(Assembly assembly = null)
        {
            yield return PermissionRole.Admin;
            yield return PermissionRole.User;
        }
    }
}
