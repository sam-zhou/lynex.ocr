using System.Collections.Generic;
using System.Reflection;
using Lynex.BillMaster.Model.Domain.DbModels;
using NHibernate;

namespace Lynex.Database.Common.DefaultDataFactory
{
    internal class SettingFactory : DefaultDataFactoryBase<Setting>
    {
        public SettingFactory(ISession session, Assembly assembly) : base(session, assembly)
        {
        }

        protected override IEnumerable<Setting> GetData(Assembly assembly = null)
        {
            yield return new Setting {Name = "User Per Page", Value = "39", Type = typeof (int)};
        }
    }
}
