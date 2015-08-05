using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.Model.Domain.DbModels
{
    public class BillingCompany:BaseEntity
    {
        public virtual string Name { get; set; }

        public virtual Address Address { get; set; }
    }
}
