using FluentNHibernate.Mapping;
using NHibernate.Type;
using Lynex.Model.Enum;

namespace Lynex.Model.Domain.DbModels.Mapping
{
    public class BillMap : BaseMap<Bill>
    {
        public BillMap()
        {
            Map(q => q.Amount).Not.Nullable().Default("0");
            Map(q => q.DueDate).CustomType<DateType>().Nullable();
            Map(q => q.IssueDate).CustomType<DateType>().Nullable();
            Map(q => q.BillType).CustomType<int>().Not.Nullable();
            References(q => q.Company).Column("BillingCompanyId").ForeignKey("Bill_BillingCompany_Id");
            References(q => q.User).Column("UserId").ForeignKey("Bill_User_Id");
        }
    }
}
