using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Admin.Repository.BillRepo;
using Lynex.BillMaster.Admin.Service.Interface;
using Lynex.BillMaster.Exception.UserException;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Repository.BillRepo;
using Lynex.BillMaster.Service;
using Lynex.BillMaster.Service.Interface;
using Lynex.Common.Database;
using Lynex.Common.Service;

namespace Lynex.BillMaster.Admin.Service
{
    public class AdminBillService: BaseService, IAdminBillService
    {
        private readonly IAddressService _addressService;

        public AdminBillService(IDatabaseService dbService, IAddressService addressService) : base(dbService)
        {
            _addressService = addressService;
        }

        BillingCompany IAdminBillService.CreateBillCompany(string name, Address newAddress)
        {
            if (IsBillCompanyUnique(name))
            {
                return SingleTransactionOperation(CreateBillCompany, name, newAddress);
            }
            throw new PropertyNotUniqueException("Name", name);
        }

        public BillingCompany CreateBillCompany(string name, Address newAddress)
        {
            var address = ((AddressService)_addressService).CreateAddress(newAddress);
            var billCompany = new BillingCompany
            {
                Name = name,
                Address = address
            };

            DatabaseService.Save(billCompany);
            return billCompany;
        }

        public void UpdateBillCompany(BillingCompany billCompany)
        {
            var theBillCompany = DatabaseService.Get<BillingCompany>(billCompany.Id);
            if (theBillCompany != null)
            {
                theBillCompany.Name = billCompany.Name;
                DatabaseService.Save(theBillCompany);
            }
            throw new EntityNotFoundException<BillingCompany>(billCompany);
        }

        public void DeleteBillCompany(BillingCompany billCompany)
        {
            try
            {
                var theBillCompany = DatabaseService.Get<BillingCompany>(billCompany.Id);
                if (theBillCompany != null)
                {
                    if (DatabaseService.Get(new IsBillCompanyHasBill(theBillCompany)))
                    {
                        
                    }
                }
            }
            catch (System.Exception)
            {
                
                throw;
            }
        }

        public bool IsBillCompanyUnique(string name)
        {
            return DatabaseService.Get(new IsBillCompanyUnique(name));
        }
    }
}
