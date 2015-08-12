using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum;

namespace Lynex.BillMaster.Service.Interface
{
    public interface IUserService
    {
        User GetUser(string id);

        User RegisterUser(string email, string password, string mobile, string lastName, string firstName);

        bool IsEmailUnique(string email);

        UserLoginStatus Login(string email, string password);

        UserChallengeStatus ChallengeUser(string id, string challenge);

        void CreateNewChallenge(string id);

        void CreateAddress(User user, Address newAddress);
    }
}
