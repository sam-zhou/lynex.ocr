using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.AspNet.Identity.Internal.Mapping
{
	internal class IdentityUserClaimMap : ClassMap<IdentityUserClaim>
	{
		public IdentityUserClaimMap()
		{
			base.Id((IdentityUserClaim x) => x.Id);
			base.Map((IdentityUserClaim x) => x.ClaimType);
			base.Map((IdentityUserClaim x) => x.ClaimValue);
			base.References<IdentityUser>((IdentityUserClaim x) => x.User).Column("UserId");
			base.Table("UserClaims");
		}
	}
}