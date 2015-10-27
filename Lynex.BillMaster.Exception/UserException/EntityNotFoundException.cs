using System;
using Lynex.Common.Exception;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.BillMaster.Exception.UserException
{
    public class EntityNotFoundException<T> : LynexException where T: IDbModel
    {
        public T Entity { get; set; }

        public EntityNotFoundException(T entity)
        {
            Entity = entity;
        }

        
    }
}
