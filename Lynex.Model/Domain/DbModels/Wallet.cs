using System;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Model.Domain.DbModels
{
    public class Wallet : BaseEntity
    {
        public virtual double Balance { get; set; }
    }
}
