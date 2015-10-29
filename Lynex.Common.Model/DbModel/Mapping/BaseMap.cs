using FluentNHibernate.Mapping;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.Common.Model.DbModel.Mapping
{
    public abstract class BaseMap<TEntity> : ClassMap<TEntity> where TEntity: class, IBaseEntity
    {
        protected BaseMap()
        {
            Id(q => q.Id).GeneratedBy.Identity();
        }
    }
}
