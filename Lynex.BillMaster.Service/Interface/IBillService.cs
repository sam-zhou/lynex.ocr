using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels;

namespace Lynex.BillMaster.Service.Interface
{
    public interface IBillService
    {
        IEnumerable<Bill> GetBillsForUser(User user);
        Bill CreateBill(Bill bill, BillingCompany company, User user);
    }
}
