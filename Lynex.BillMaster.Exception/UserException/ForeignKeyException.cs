using System;
using Lynex.Common.Exception;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.BillMaster.Exception.UserException
{
    public class ForeignKeyException : LynexException
    {
        public string EntityName { get; set; }

        public string AffectedEntityName { get; set; }

        public ForeignKeyException(string entityName, string affectedEntityName)
        {
            EntityName = entityName;
            AffectedEntityName = affectedEntityName;
        }

        
    }
}
