using System;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.Common.Model.DbModel
{
    public class EnumTable<T> : IBaseEntity<T> where T: struct ,IConvertible
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
    }
}
