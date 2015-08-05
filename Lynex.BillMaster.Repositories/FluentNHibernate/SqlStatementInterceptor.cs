using System.Diagnostics;
using NHibernate;

namespace WCC.Repositories.FluentNHibernate
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
