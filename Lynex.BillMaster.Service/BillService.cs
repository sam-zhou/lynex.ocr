using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Repository.BillRepo;
using Lynex.BillMaster.Service.Interface;
using Lynex.Common.Database;
using Lynex.Common.Service;

namespace Lynex.BillMaster.Service
{
    public class BillService:BaseService, IBillService
    {
        public BillService(IDatabaseService dbService) : base(dbService)
        {
        }

        public IEnumerable<Bill> GetBillsForUser(long id)
        {
            return DatabaseService.Get(new GetBillsByUser(id));
        }

        public Bill CreateBill(Bill bill, BillingCompany company, User user)
        {
            return bill;
        }
    }
}
