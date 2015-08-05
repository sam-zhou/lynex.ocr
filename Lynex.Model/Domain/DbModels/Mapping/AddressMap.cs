namespace Lynex.Model.Domain.DbModels.Mapping
{
    public class AddressMap : BaseMap<Address>
    {
        public AddressMap()
        {
            Map(q => q.AddressLine1).Length(50).Not.Nullable();
            Map(q => q.AddressLine2).Length(50).Nullable();
            Map(q => q.AddressLine3).Length(50).Nullable();
            Map(q => q.PostCode).Length(10).Nullable();
            Map(q => q.Suburb).Length(20).Nullable();
            Map(q => q.State).Length(20).Nullable();
            Map(q => q.Country).Length(20).Nullable();
            //Map(q => q.ManageMyPaientDetails).Not.Nullable().Default("0");
            //HasMany(q => q.Dosages).KeyColumn("UserId");
        }
    }
}
