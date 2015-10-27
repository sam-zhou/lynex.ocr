using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	internal class IdentityUserLoginMap : ClassMap<IdentityUserLogin>
	{
		public IdentityUserLoginMap()
		{
			Id(x => x.Id);
			Map(x => x.LoginProvider);
			Map(x => x.ProviderKey);
			References(x => x.User).Column("UserId");
			Table("UserLogins");
		}
	}
}