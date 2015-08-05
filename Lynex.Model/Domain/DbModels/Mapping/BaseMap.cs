using FluentNHibernate.Mapping;
using Lynex.Model.Domain.DbModels.Interface;

namespace Lynex.Model.Domain.DbModels.Mapping
{
    public class BaseMap<TEntity> : ClassMap<TEntity> where TEntity: class, IBaseEntity
    {
        public BaseMap()
        {
            Id(q => q.Id).GeneratedBy.Identity();
        }
    }
}
