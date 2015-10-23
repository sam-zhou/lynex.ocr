using Lynex.BillMaster.Model.Enum;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Exception;

namespace Lynex.BillMaster.Exception.UserException
{
    public class VerifyUserFailedException : LynexException
    {
        public VerifyUserFailedException(UserChallengeStatus status)
        {
            Status = status;
        }

        public UserChallengeStatus Status { get; set; }
    }
}
