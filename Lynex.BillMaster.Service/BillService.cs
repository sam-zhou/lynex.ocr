using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Service;
using Lynex.Database.Common;

namespace Lynex.BillMaster.Service
{
    public class BillService:BaseService, IBillService
    {
        public BillService(IDatabaseService dbService) : base(dbService)
        {
        }

        public IEnumerable<Bill> GetBillsForUser(User user)
        {
            return DatabaseService.GetAll<Bill>();
        }

        public Bill CreateBill(Bill bill, BillingCompany company, User user)
        {
            return bill;
        }
    }
}
