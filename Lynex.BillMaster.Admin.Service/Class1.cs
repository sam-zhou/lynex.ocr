using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database;
using Lynex.Common.Service;

namespace Lynex.BillMaster.Admin.Service
{
    public class AdminBillService: BaseService
    {
        public AdminBillService(IDatabaseService dbService) : base(dbService)
        {
        }

        public void CreateBillCompany(string name)
        {
            try
            {
                
                var billCompany = new BillingCompany
                {
                    Name = name
                };
                DatabaseService.Save(billCompany);
            }
            catch (System.Exception)
            {
                
                throw;
            }
            
        }
    }
}
