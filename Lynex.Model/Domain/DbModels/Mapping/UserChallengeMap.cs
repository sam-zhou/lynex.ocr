using Lynex.Common.Model.DbModel.Mapping;
using NHibernate.Type;

namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class UserChallengeMap : BaseMap<UserChallenge>
    {
        public UserChallengeMap()
        {
            //References(x => x.User).ForeignKey("UserChallenge_User_Id").Unique().Column("UserId").Cascade.All();
            Map(q => q.TryCount).Not.Nullable().Default("0");
            Map(q => q.Challenge).Length(128).Not.Nullable();
            Map(q => q.CreatedAt).CustomType<UtcDateTimeType>().Not.Nullable().Default("getDate()");
            Map(q => q.VerifiedAt).CustomType<UtcDateTimeType>().Nullable();
            //HasMany(q => q.Dosages).KeyColumn("UserId");
        }
    }
}
