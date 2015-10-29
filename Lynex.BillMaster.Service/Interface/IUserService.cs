using System.Collections.Generic;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Model.DbModel;
using Lynex.Common.Model.Enum.Mapable;

namespace Lynex.BillMaster.Service.Interface
{
    public interface IUserService
    {
        ApplicationUser GetUser(string id);

        ApplicationUser RegisterUser(string email, string password, string mobile, string lastName, string firstName);

        bool IsEmailUnique(string email);

        UserLoginStatus Login(string email, string password);

        UserChallengeStatus ChallengeUser(string id, string challenge);

        void CreateNewChallenge(string id);

        void CreateAddress(ApplicationUser user, Address newAddress);
    }
}
