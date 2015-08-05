﻿using System.Collections.Generic;
using Lynex.Model.Domain.DbModels;
using NHibernate;

namespace Lynex.Database.Common.DefaultDataFactory
{
    internal class PermissionRoleFactory : DefaultDataFactoryBase<PermissionRole>
    {
        public PermissionRoleFactory(ISession session) : base(session)
        {
        }

        protected override IEnumerable<PermissionRole> GetData()
        {
            yield return PermissionRole.Admin;
            yield return PermissionRole.Patient;
        }
    }
}
