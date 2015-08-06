using FluentNHibernate.Mapping;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.Common.Model.DbModel.Mapping
{
    public class BaseMap<TEntity> : ClassMap<TEntity> where TEntity: class, IBaseEntity
    {
        public BaseMap()
        {
            Id(q => q.Id).GeneratedBy.Identity();
        }
    }
}
