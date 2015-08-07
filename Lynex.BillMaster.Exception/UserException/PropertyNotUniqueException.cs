using Lynex.Common.Exception;

namespace Lynex.BillMaster.Exception.UserException
{
    public class PropertyNotUniqueException : LynexException
    {
        public string PropertyName { get; set; }

        public object PropertyValue { get; set; }

        public PropertyNotUniqueException(string name, object value)
        {
            PropertyName = name;
            PropertyValue = value;
        }

        
    }
}
