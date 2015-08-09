using System.Collections.Generic;
using System.Reflection;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.Common.Database.DefaultDataFactory;
using NHibernate;

namespace Lynex.BillMaster.Database.DefaultDataFactory
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
