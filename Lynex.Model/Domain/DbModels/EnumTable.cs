using System;
using Lynex.BillMaster.Model.Domain.DbModels.Interface;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class EnumTable<T> : IBaseEntity<T> where T: struct ,IConvertible
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
    }
}
