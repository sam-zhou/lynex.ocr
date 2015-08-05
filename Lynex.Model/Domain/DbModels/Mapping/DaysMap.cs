namespace WCC.Model.Domain.DbModels.Mapping
{
    public class DaysMap : BaseMap<Days>
    {
        public DaysMap()
        {
            Map(q => q.Value).Length(10).Not.Nullable();
            Table("DaysOfWeek");
        }
    }
}
