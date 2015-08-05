using System;

namespace Lynex.Model.Domain.DbModels
{
    public class Setting : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual Type Type { get; set; }
    }
}
