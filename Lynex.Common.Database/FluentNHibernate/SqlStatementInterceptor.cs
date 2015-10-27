using System.Diagnostics;
using NHibernate;

namespace Lynex.Common.Database.FluentNHibernate
{
    internal class SqlStatementInterceptor : EmptyInterceptor
    {
        public override NHibernate.SqlCommand.SqlString OnPrepareStatement(NHibernate.SqlCommand.SqlString sql)
        {
            Trace.WriteLine(sql.ToString());
            return sql;
        }
    }
}
