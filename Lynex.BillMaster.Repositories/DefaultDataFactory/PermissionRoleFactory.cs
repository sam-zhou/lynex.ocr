using System.Collections.Generic;
using NHibernate;
using WCC.Model.Domain.DbModels;

namespace WCC.Repositories.DefaultDataFactory
{
    internal class PermissionRoleFactory : DefaultDataFactoryBase<PermissionRole>
    {
        public PermissionRoleFactory(ISession session) : base(session)
        {
        }

        protected override IEnumerable<PermissionRole> GetData()
        {
            yield return PermissionRole.Admin;
            yield return PermissionRole.Patient;
        }
    }
}
