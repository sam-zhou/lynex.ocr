using Lynex.Common.Model.DbModel.Mapping;
using NHibernate.Type;

namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class UserMap : BaseMap<User>
    {
        public UserMap()
        {
            Map(q => q.Email).Length(50).Unique().Not.Nullable();
            Map(q => q.Mobile).Length(20).Nullable();
            Map(q => q.FirstName).Length(50).Not.Nullable();
            Map(q => q.LastName).Length(50).Not.Nullable();
            Map(q => q.Title).Length(10).Nullable();
            Map(q => q.Salt).Length(10).Not.Nullable();
            Map(q => q.Hash).Length(64).Not.Nullable();
            Map(q => q.LoginAttempt).Not.Nullable().Default("0");
            Map(q => q.IsVerified).Not.Nullable().Default("0");
            Map(q => q.Active).Not.Nullable().Default("0");
            Map(q => q.CreatedAt).CustomType<UtcDateTimeType>().Not.Nullable().Default("getDate()");
            Map(q => q.LastLogin).CustomType<UtcDateTimeType>().Nullable();
            Map(q => q.LastFailedLogin).CustomType<UtcDateTimeType>().Nullable();
            //HasMany(q => q.Notifications).KeyColumn("UserId").ForeignKeyConstraintName("Notification_User_Id");
            HasOne(q => q.Wallet).LazyLoad().ForeignKey("User_Wallet_Id");
            HasOne(q => q.UserChallenge).LazyLoad().ForeignKey("User_UserChallenge_Id");
            References(q => q.PermissionRole).Column("PermissionRoleId").ForeignKey("User_Permission_Id");
            References(q => q.Address).Column("AddressId").ForeignKey("User_Address_Id").LazyLoad();
            //HasManyToMany(m => m.Users).ParentKeyColumn("UserId").ForeignKeyConstraintNames("User_Patient_Id", "Patient_User_Id").ChildKeyColumn("PatientId").Table("UserPatient").Cascade.SaveUpdate();
        }
    }
}
