using Lynex.BillMaster.Model.Enum;

namespace Lynex.BillMaster.Model.Exception
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
