using Lynex.BillMaster.Model.Enum;
using Lynex.BillMaster.Model.Enum.Mapable;
using Lynex.Common.Exception;

namespace Lynex.BillMaster.Exception.UserException
{
    public class UserLoginFailedException : LynexException
    {
        public UserLoginFailedException(UserLoginStatus status)
        {
            Status = status;
        }

        public UserLoginStatus Status { get; set; }
    }
}
