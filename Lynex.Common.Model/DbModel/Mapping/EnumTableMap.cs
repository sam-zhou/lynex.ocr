using System;
using FluentNHibernate.Mapping;

namespace Lynex.Common.Model.DbModel.Mapping
{
    public abstract class EnumTableMap<T> : ClassMap<EnumTable<T>> where T: struct , IConvertible
    {
        protected EnumTableMap()
        {
            Table(typeof(T).Name);
            Id(q => q.Id).GeneratedBy.Assigned();
            Map(q => q.Name).Length(20).Not.Nullable();
        }
    }

}
