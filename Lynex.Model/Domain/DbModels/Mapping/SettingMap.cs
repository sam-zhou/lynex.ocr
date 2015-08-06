using System;

namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class SettingMap : BaseMap<Setting>
    {
        public SettingMap()
        {
            Map(q => q.Name).Length(30).Not.Nullable();
            Map(q => q.Value).Length(30).Not.Nullable();
            Map(q => q.Type).CustomType<Type>().Not.Nullable();
        }
    }
}
