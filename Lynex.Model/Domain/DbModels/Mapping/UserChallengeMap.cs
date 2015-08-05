using NHibernate.Type;

namespace Lynex.Model.Domain.DbModels.Mapping
{
    public class UserChallengeMap : BaseMap<UserChallenge>
    {
        public UserChallengeMap()
        {
            //References(x => x.User).Unique().Column("UserId");
            Map(q => q.TryCount).Not.Nullable().Default("0");
            Map(q => q.Challenge).Length(64).Not.Nullable();
            Map(q => q.CreatedAt).CustomType<UtcDateTimeType>().Not.Nullable().Default("getDate()");
            //HasMany(q => q.Dosages).KeyColumn("UserId");
        }
    }
}
