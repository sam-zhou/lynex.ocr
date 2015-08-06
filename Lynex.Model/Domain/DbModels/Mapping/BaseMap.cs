using FluentNHibernate.Mapping;
using Lynex.BillMaster.Model.Domain.DbModels.Interface;

namespace Lynex.BillMaster.Model.Domain.DbModels.Mapping
{
    public class BaseMap<TEntity> : ClassMap<TEntity> where TEntity: class, IBaseEntity
    {
        public BaseMap()
        {
            Id(q => q.Id).GeneratedBy.Identity();
        }
    }
}
