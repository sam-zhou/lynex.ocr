using Lynex.Common.Exception;

namespace Lynex.BillMaster.Exception.UserException
{
    public class EmailNotUniqueException : LynexException
    {
        public string Email { get; set; }

        public EmailNotUniqueException(string email)
        {
            Email = email;
        }

        
    }
}
