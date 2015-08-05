namespace WCC.Model.Domain.DbModels.Mapping
{
    public class DosageDetailMap : BaseMap<DosageDetail>
    {
        public DosageDetailMap()
        {
            Map(q => q.Value).Not.Nullable();
            Map(q => q.Day).CustomType<int>().Not.Nullable();
            //References<Days>(q => q.Day, "DayId").Not.Nullable();
            //Map(q => q.Day).CustomType<int>().Not.Nullable();
            //Map(q => q.DosageId).Not.Nullable();
        }
    }
}
