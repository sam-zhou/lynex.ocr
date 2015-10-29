using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Model.DbModel;

namespace Lynex.BillMaster.Admin.Service.Interface
{
    public interface IAdminBillService
    {
        BillingCompany CreateBillCompany(string name, BillType billTypes, Address newAddress);

        bool IsBillCompanyUnique(string name);

        void UpdateBillCompany(BillingCompany billCompany);

        void DeleteBillCompany(BillingCompany billCompany);
    }
}
