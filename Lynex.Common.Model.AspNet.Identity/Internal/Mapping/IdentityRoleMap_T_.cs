using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	public class IdentityRoleMap<T> : ClassMap<T>
	where T : IdentityRole
	{
		public IdentityRoleMap()
		{
			base.Id((T x) => x.Id);
			base.Map((T x) => x.Name);
			base.Table("UserRoles");
		}
	}
}