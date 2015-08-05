using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;
using WCC.Repositories.Interface.Repositories;
using WCC.Services.Interface;

namespace WCC.Services
{
    public class UserService : BaseService, IUserService
    {
        public UserService(IWCCMainRepository wccMainRepository) : base(wccMainRepository)
        {
        }

        public User GetUserId(long userId)
        {
            return WCCMainRepository.GetUserId(userId);
        }

        public User GetUserByMedWayId(long medwayId)
        {
            return WCCMainRepository.GetUserByMedWayId(medwayId);
        }

        public User CreateUser(long medwayUserId, string email, string mobile)
        {
            return WCCMainRepository.CreateUser(medwayUserId, email, mobile);
        }

        public UserChallengeStatus VerifyUser(long userId, string challenge)
        {
            return WCCMainRepository.VerifyUser(userId, challenge);
        }

        public void AddPatient(User user, User patient)
        {
            WCCMainRepository.AddPatient(user, patient);
        }

        public void RemovePatient(User user, User patient)
        {
            WCCMainRepository.RemovePatient(user, patient);
        }

        public void Login(User user)
        {
            WCCMainRepository.Login(user);
        }
    }
}
