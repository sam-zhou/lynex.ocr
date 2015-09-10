using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	internal class IdentityUserLoginMap : ClassMap<IdentityUserLogin>
	{
		public IdentityUserLoginMap()
		{
			base.Id((IdentityUserLogin x) => x.Id);
			base.Map((IdentityUserLogin x) => x.LoginProvider);
			base.Map((IdentityUserLogin x) => x.ProviderKey);
			base.References<IdentityUser>((IdentityUserLogin x) => x.User).Column("UserId");
			base.Table("UserLogins");
		}
	}
}