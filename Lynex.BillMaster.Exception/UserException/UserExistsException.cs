using Lynex.BillMaster.Model.Enum;
using Lynex.Common.Exception;

namespace Lynex.BillMaster.Exception.UserException
{
    public class UserExistsException : LynexException
    {
        public UserExistsException(string userName)
        {
            UserName = userName;
        }

        public string UserName { get; set; }
    }
}
