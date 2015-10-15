using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	public class IdentityRoleMap<T> : ClassMap<T>
	where T : IdentityRole
	{
		public IdentityRoleMap()
		{
			Id(x => x.Id);
			Map(x => x.Name);
			Table("IdentityRole");
		}
	}
}