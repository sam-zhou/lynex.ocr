using System;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class Setting : BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual string Value { get; set; }

        public virtual Type Type { get; set; }
    }
}
