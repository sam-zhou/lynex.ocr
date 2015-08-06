using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.BillMaster.Model.Domain.DbModels;

namespace Lynex.BillMaster.Service
{
    public interface IBillService
    {
        IEnumerable<Bill> GetBillsForUser(User user);
        Bill CreateBill(Bill bill, BillingCompany company, User user);
    }
}
