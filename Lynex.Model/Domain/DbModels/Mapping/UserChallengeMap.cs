using Lynex.Common.Model.DbModel.Mapping;
using NHibernate.Type;

namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class UserChallengeMap : BaseMap<UserChallenge>
    {
        public UserChallengeMap()
        {
            References(x => x.User).ForeignKey("UserChallenge_User_Id").Unique().Column("UserId").Cascade.All();
            Map(q => q.TryCount).Not.Nullable().Default("0");
            Map(q => q.Challenge).Length(64).Not.Nullable();
            Map(q => q.CreatedAt).CustomType<UtcDateTimeType>().Not.Nullable().Default("getDate()");
            //HasMany(q => q.Dosages).KeyColumn("UserId");
        }
    }
}
