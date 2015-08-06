using FluentNHibernate.Mapping;

namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class PermissionRoleMap : ClassMap<PermissionRole>
    {
        public PermissionRoleMap()
        {
            Id(q => q.Id).GeneratedBy.Assigned();
            Map(q => q.Name).Length(10).Not.Nullable();
            Map(q => q.AccessMyDosage).Not.Nullable().Default("0");
            Map(q => q.AccessMyHistory).Not.Nullable().Default("0");
            Map(q => q.ManageMyAccount).Not.Nullable().Default("0");
            Map(q => q.ManageUsers).Not.Nullable().Default("0");
            Map(q => q.AlterPreRequisites).Not.Nullable().Default("0");
            //Map(q => q.ManageMyPaientDetails).Not.Nullable().Default("0");
            //HasMany(q => q.Dosages).KeyColumn("UserId");
        }
    }
}
