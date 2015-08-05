using System.Collections.Generic;
using NHibernate;
using WCC.Model.Domain.DbModels;

namespace WCC.Repositories.DefaultDataFactory
{
    internal class SettingFactory : DefaultDataFactoryBase<Setting>
    {
        public SettingFactory(ISession session) : base(session)
        {
        }

        protected override IEnumerable<Setting> GetData()
        {
            yield return new Setting {Name = "User Per Page", Value = "39", Type = typeof (int)};
        }
    }
}
