using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Model.Domain.DbModels.Interface;

namespace Lynex.Model.Domain.DbModels
{
    public class EnumTable<T> : IBaseEntity<T> 
    {
        public virtual long Id { get; set; }

        public virtual string Name { get; set; }
    }
}
