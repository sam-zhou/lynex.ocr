﻿using Lynex.Common.Model.DomainModel;
using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;

namespace Lynex.Common.Model
{
    public class IdentityUserClaim : EntityWithTypedId<int>
    {
        public virtual string ClaimType { get; set; }

        public virtual string ClaimValue { get; set; }

        public virtual IdentityUser User { get; set; }
    }

    public class IdentityUserClaimMap : ClassMapping<IdentityUserClaim>
    {
        public IdentityUserClaimMap()
        {
            Table("AspNetUserClaims");
            Id(x => x.Id, m => m.Generator(Generators.Identity));
            Property(x => x.ClaimType);
            Property(x => x.ClaimValue);

            ManyToOne(x => x.User, m => m.Column("UserId"));
        }
    }

}
