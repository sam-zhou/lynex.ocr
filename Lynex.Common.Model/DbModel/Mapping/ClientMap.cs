namespace Lynex.Common.Model.DbModel.Mapping
{
    public class ClientMap : BaseMap<Client>
    {
        public ClientMap()
        {
            Map(q => q.ClientId).Length(50).Not.Nullable();
            Map(q => q.Secret).Not.Nullable();
            Map(q => q.Name).Length(100).Not.Nullable();
            Map(q => q.ApplicationType).Length(50).Not.Nullable();
            Map(q => q.Active).Not.Nullable();
            Map(q => q.RefreshTokenLifeTime).Default("0");
            Map(q => q.AllowedOrigin).Length(100).Not.Nullable().Default("'*'");
        }
    }
}
