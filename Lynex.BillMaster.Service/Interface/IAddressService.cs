using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels;

namespace Lynex.BillMaster.Service.Interface
{
    public interface IAddressService
    {
        Address CreateAddress(Address newaddress);
    }
}
