using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	public class IdentityUserMap<T> : ClassMap<T>
	where T : IdentityUser
	{
		public IdentityUserMap()
		{
			Id((T x) => x.Id);
			Map((T x) => x.UserName);
			Map((T x) => x.PasswordHash);
			Map((T x) => x.SecurityStamp);
			HasManyToMany<IdentityRole>((T x) => x.Roles).Table("UserRoles").ParentKeyColumn("UserId").ChildKeyColumn("RoleId").Cascade.All();
			HasMany<IdentityUserClaim>((T x) => x.Claims).KeyColumn("UserId").Cascade.All();
			HasMany<IdentityUserLogin>((T x) => x.Logins).KeyColumn("UserId").Cascade.All();
			Table("Users");
		}
	}
}