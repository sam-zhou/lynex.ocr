using FluentNHibernate.Mapping;
using NHibernate.Type;
using Lynex.Model.Enum;

namespace Lynex.Model.Domain.DbModels.Mapping
{
    public class NotificationMap : BaseMap<Notification>
    {
        public NotificationMap()
        {
            //References<User>(q => q.UserId, "UserId").ForeignKey("Notification_User_Id");
            //Map(q => q.DosageId).Not.Nullable();
            Map(q => q.NotifiedAt).CustomType<UtcDateTimeType>().Not.Nullable();
            Map(q => q.Type).CustomType<GenericEnumMapper<NotificationType>>().Length(5).Not.Nullable();
            
        }
    }
}
