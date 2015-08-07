using Lynex.BillMaster.Model.Domain.DbModels;

namespace Lynex.BillMaster.Admin.Service.Interface
{
    public interface IAdminBillService
    {
        BillingCompany CreateBillCompany(string name, Address newAddress);

        bool IsBillCompanyUnique(string name);
    }
}
