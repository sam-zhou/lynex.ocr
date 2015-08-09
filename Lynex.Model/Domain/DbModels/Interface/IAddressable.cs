using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.BillMaster.Model.Domain.DbModels.Interface
{
    public interface IAddressable
    {
        Address Address { get; set; }
    }
}
