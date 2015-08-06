using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using Lynex.Common.Database.DefaultDataFactory;
using Lynex.Common.Extension;
using Lynex.Common.Model;
using Lynex.Common.Model.DbModel.Mapping;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Lynex.Common.Database.FluentNHibernate
{
    internal static class NHibernateHelper
    {
        public static ISessionFactory CreateSessionFactory(string connectionStringKey, Assembly assembly, bool createNew)
        {
            var configuration = BuildConfiguration(connectionStringKey, assembly);
            if (createNew)
            {
                configuration.ExposeConfiguration(CreateSchemaExport);
            }

            var sessionFactory = configuration.BuildSessionFactory();

            if (createNew)
            {
                PopulateDefaultData(sessionFactory, assembly);
            }

            return sessionFactory;
        }

        private static FluentConfiguration BuildConfiguration(string connectionStringKey, Assembly assembly)
        {
            var configuration = Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2012
                        .ConnectionString(q => q.FromConnectionStringWithKey(connectionStringKey)))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssembly(assembly).AddFromAssembly(CreateGenericClassMappingAssembly(assembly)));
#if DEBUG || TRACE
            configuration.ExposeConfiguration(SetInterceptors);
#endif

            return configuration;
        }

        private static Assembly CreateGenericClassMappingAssembly(Assembly assembly)
        {
            var types = new List<DynamicTypeInfo>();
            var enumTypes = assembly.GetMapableEnumTypes();
            foreach (var enumType in enumTypes)
            {
                var parentBase = typeof (EnumTableMap<>);
                var parent = parentBase.MakeGenericType(enumType);
                types.Add(new DynamicTypeInfo(enumType.Name + "Map", parent));
            }

            return DynamicClassHelper.CreateDynamicAssembly(assembly.GetName().Name + ".Dynamic", types);
        }

        private static void SetInterceptors(Configuration cfg)
        {
            cfg.SetInterceptor(new SqlStatementInterceptor());
        }

        private static void CreateSchemaExport(Configuration cfg)
        {
            var schemaExport = new SchemaExport(cfg);
            schemaExport.Create(true, true);
        }

        private static void PopulateDefaultData(ISessionFactory sessionFactory, Assembly assembly)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var types =
                    Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IDefaultDataFactory).IsAssignableFrom(type) && !type.IsAbstract);

                foreach (var type in types)
                {
                    var instance = Activator.CreateInstance(type, session, assembly) as IDefaultDataFactory;
                    if (instance != null)
                    {
                        instance.Populate();
                    }
                }
            }
        }
    }
}
