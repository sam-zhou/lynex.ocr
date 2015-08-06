using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database;
using Lynex.Common.Service;

namespace Lynex.BillMaster.Service
{
    public class AddressService:BaseService
    {
        public AddressService(IDatabaseService dbService) : base(dbService)
        {
        }

        public Address CreateAddress(Address newaddress)
        {
            var address = new Address
            {
                AddressLine1 = "",
                AddressLine2 = "",
                AddressLine3 = "",
                Country = "Australia",
                PostCode = "",
                State = "Western Australia",
                Suburb = "Canning Vale"
            };
            DatabaseService.Save(address);
            return address;
        }
    }
}
