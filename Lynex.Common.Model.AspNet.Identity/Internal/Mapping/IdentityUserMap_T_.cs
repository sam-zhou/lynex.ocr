using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	public class IdentityUserMap<T> : ClassMap<T>
	where T : IdentityUser
	{
		public IdentityUserMap()
		{
			Id(x => x.Id);
			Map(x => x.UserName);
			Map(x => x.PasswordHash);
			Map(x => x.SecurityStamp);
			HasManyToMany(x => x.Roles).Table("UserRole").ParentKeyColumn("UserId").ChildKeyColumn("RoleId").Cascade.All();
			HasMany(x => x.Claims).KeyColumn("UserId").Cascade.All();
			HasMany(x => x.Logins).KeyColumn("UserId").Cascade.All();
			Table("Users");
		}
	}
}