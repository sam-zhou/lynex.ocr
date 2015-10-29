using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Service.Interface;
using Lynex.Common.Database;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Service;

namespace Lynex.BillMaster.Service
{
    public class AddressService:BaseService, IAddressService
    {
        public AddressService(IDatabaseService dbService) : base(dbService)
        {
        }

        Address IAddressService.CreateAddress(Address newaddress)
        {
            return SingleTransactionOperation(CreateAddress, newaddress);
        }

        public Address CreateAddress(Address newaddress)
        {
            return SingleTransactionOperation(x =>
            {
                DatabaseService.Save(x);
                return x;
            }, new Address
            {
                AddressLine1 = newaddress.AddressLine1,
                AddressLine2 = newaddress.AddressLine2,
                AddressLine3 = newaddress.AddressLine3,
                Country = newaddress.Country,
                PostCode = newaddress.PostCode,
                State = newaddress.State,
                Suburb = newaddress.Suburb
            });
            
        }
    }
}
