namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class BillingCompanyMap : BaseMap<BillingCompany>
    {
        public BillingCompanyMap()
        {
            Map(q => q.Name).Length(100).Not.Nullable();
            References(q => q.Address).Column("AddressId").ForeignKey("BillingCompany_Address_Id");
        }
    }
}
