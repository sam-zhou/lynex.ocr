using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	internal class IdentityUserClaimMap : ClassMap<IdentityUserClaim>
	{
		public IdentityUserClaimMap()
		{
			base.Id(x => x.Id);
			base.Map(x => x.ClaimType);
			base.Map(x => x.ClaimValue);
			base.References(x => x.User).Column("UserId");
			base.Table("UserClaims");
		}
	}
}