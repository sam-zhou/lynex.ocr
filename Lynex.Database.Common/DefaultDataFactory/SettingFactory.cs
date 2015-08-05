using System.Collections.Generic;
using Lynex.Model.Domain.DbModels;
using NHibernate;

namespace Lynex.Database.Common.DefaultDataFactory
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
