namespace Lynex.BillMaster.Model.Domain.DbModels.Interface
{
    public interface IBaseEntity
    {
        long Id { get; set; }
    }

    public interface IBaseEntity<T> : IBaseEntity 
    {
    }
}
