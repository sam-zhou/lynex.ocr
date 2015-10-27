using Lynex.Common.Model.DbModel.Mapping;
using NHibernate.Type;

namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class WalletMap : BaseMap<Wallet>
    {
        public WalletMap()
        {
            Map(q => q.Balance).Not.Nullable().Default("0");
            //References(x => x.User).ForeignKey("Wallet_User_Id").Unique().Column("UserId").Cascade.All();
            //HasManyToMany(m => m.Users).ParentKeyColumn("UserId").ForeignKeyConstraintNames("User_Patient_Id", "Patient_User_Id").ChildKeyColumn("PatientId").Table("UserPatient").Cascade.SaveUpdate();
        }
    }
}
