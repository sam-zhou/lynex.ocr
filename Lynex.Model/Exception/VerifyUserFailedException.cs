using Lynex.Model.Enum;

namespace Lynex.Model.Exception
{
    public class VerifyUserFailedException : System.Exception
    {
        public VerifyUserFailedException(UserChallengeStatus status)
        {
            Status = status;
        }

        public UserChallengeStatus Status { get; set; }
    }
}
