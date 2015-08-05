using System;
using System.Collections.Generic;
using System.Linq;
using NHibernate;
using WCC.Model.Domain.DbModels;
using WCC.Model.Enum;

namespace WCC.Repositories.DefaultDataFactory
{
    internal class DaysOfWeekFactory : DefaultDataFactoryBase<Days>
    {
        public DaysOfWeekFactory(ISession session) : base(session)
        {
        }

        protected override IEnumerable<Days> GetData()
        {
            return from object day in Enum.GetValues(typeof(DaysOfWeek)) select new Days { Id = (int)day, Value = day.ToString() };
        }
    }
}
