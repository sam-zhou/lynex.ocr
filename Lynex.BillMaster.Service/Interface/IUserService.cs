using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum;

namespace Lynex.BillMaster.Service.Interface
{
    public interface IUserService
    {
        User RegisterUser(string email, string password, string mobile, string lastName, string firstName);

        bool IsEmailUnique(string email);

        UserLoginStatus Login(string email, string password);

        UserChallengeStatus ChallengeUser(long id, string challenge);

        void CreateNewChallenge(long id);
    }
}
