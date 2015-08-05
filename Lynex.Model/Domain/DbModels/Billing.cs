using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.Model.Domain.DbModels
{
    public class Billing : BaseEntity
    {
        public virtual double Amount { get; set; }

        public virtual DateTime DueDate { get; set; }
    }
}
