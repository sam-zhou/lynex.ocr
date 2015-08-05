using FluentNHibernate.Mapping;
using NHibernate.Type;
using Lynex.Model.Enum;
using Lynex.Model.Enum.Mapable;

namespace Lynex.Model.Domain.DbModels.Mapping
{
    public class BillTypeMap : BaseMap<EnumTable<BillType>>
    {
        public BillTypeMap()
        {
            Table("BillType");
            Map(q => q.Name).Length(20).Not.Nullable();
        }
    }
}
