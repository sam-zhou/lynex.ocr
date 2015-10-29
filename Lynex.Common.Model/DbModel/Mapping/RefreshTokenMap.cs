using System.Data.SqlTypes;
using FluentNHibernate.Mapping;
using NHibernate.Type;

namespace Lynex.Common.Model.DbModel.Mapping
{
    public class RefreshTokenMap : ClassMap<RefreshToken>
    {
        public RefreshTokenMap()
        {
            Id(q => q.Id);
            Map(q => q.Subject).Length(50).Not.Nullable();
            Map(q => q.ClientId).Length(50).Not.Nullable();
            Map(q => q.IssuedUtc).CustomType<UtcDateTimeType>().Not.Nullable().Default("getDate()");
            Map(q => q.ExpiresUtc).CustomType<UtcDateTimeType>().Not.Nullable();
            Map(q => q.ProtectedTicket).Length(4001).Not.Nullable();
        }
    }
}
