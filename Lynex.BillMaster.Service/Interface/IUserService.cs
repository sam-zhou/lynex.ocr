using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels;

namespace Lynex.BillMaster.Service.Interface
{
    public interface IUserService
    {
        User RegisterUser(string email, string password, string mobile);


        bool Login(string query, string password);
    }
}
