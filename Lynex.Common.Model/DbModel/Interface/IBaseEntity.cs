namespace Lynex.Common.Model.DbModel.Interface
{
    public interface IBaseEntity
    {
        long Id { get; set; }
    }

    public interface IBaseEntity<T> : IBaseEntity 
    {
    }
}
