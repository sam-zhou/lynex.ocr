using Lynex.Common.Model.AspNet.Identity.Internal.Mapping;

namespace Lynex.Common.Model.DbModel.Mapping
{
    public class ApplicationUserMap : IdentityUserMap<ApplicationUser>
    {
        public ApplicationUserMap()
        {
            Table("Users");
            //Map(q => q.Email).Length(50).Unique().Not.Nullable();
            //Map(q => q.Mobile).Length(20).Nullable();
            //Map(q => q.FirstName).Length(50).Not.Nullable();
            //Map(q => q.LastName).Length(50).Not.Nullable();
            //Map(q => q.Title).Length(10).Nullable();
            //Map(q => q.Salt).Length(10).Not.Nullable();
            //Map(q => q.Hash).Length(64).Not.Nullable();
            //Map(q => q.LoginAttempt).Not.Nullable().Default("0");
            Map(q => q.IsVerified).Not.Nullable().Default("0");
            Map(q => q.Active).Not.Nullable().Default("0");
            //Map(q => q.CreatedAt).CustomType<UtcDateTimeType>().Not.Nullable().Default("getDate()");
            //Map(q => q.LastLogin).CustomType<UtcDateTimeType>().Nullable();
            //Map(q => q.LastFailedLogin).CustomType<UtcDateTimeType>().Nullable();
            //HasMany(q => q.Notifications).KeyColumn("UserId").ForeignKeyConstraintName("Notification_User_Id");
            //References(q => q.Wallet).ForeignKey("User_Wallet_Id").Column("WalletId").Cascade.All();
            References(q => q.UserChallenge).ForeignKey("User_UserChallenge_Id").Column("UserChallengeId").Cascade.All();
            //HasMany<IdentityUserClaim>((T x) => x.Claims).KeyColumn("User_id").Cascade.All();
            References(q => q.Address).ForeignKey("User_Address_Id").Column("AddressId").Cascade.All();
            //HasManyToMany(m => m.Users).ParentKeyColumn("UserId").ForeignKeyConstraintNames("User_Patient_Id", "Patient_User_Id").ChildKeyColumn("PatientId").Table("UserPatient").Cascade.SaveUpdate();
        }
    }
}
