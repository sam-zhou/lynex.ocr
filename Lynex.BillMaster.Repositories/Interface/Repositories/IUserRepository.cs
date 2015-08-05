using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;

namespace WCC.Repositories.Interface.Repositories
{
    public interface IUserRepository
    {
        User GetUserId(long userId);
        User GetUserByMedWayId(long medwayId);
        User CreateUser(long medwayUserId, string email, string mobile);
        UserChallengeStatus VerifyUser(long userId, string challenge);

        void AddPatient(User user, User patient);
        void RemovePatient(User user, User patient);

        void Login(User user);
    }
}
