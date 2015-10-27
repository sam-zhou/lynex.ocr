namespace Lynex.Common.Model.DbModel.Interface
{
    public interface IDbModel
    {

    }

    public interface IBaseEntity: IDbModel
    {
        long Id { get; set; }
    }

    public interface IBaseEntity<T> : IBaseEntity 
    {
    }
}
