using System;

namespace Lynex.Model.Domain.DbModels.Interface
{
    public interface IBaseEntity
    {
        long Id { get; set; }
    }

    public interface IBaseEntity<T> : IBaseEntity 
    {
    }
}
