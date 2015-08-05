using System;
using System.Linq;
using System.Reflection;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using WCC.Repositories.DefaultDataFactory;

namespace WCC.Repositories.FluentNHibernate
{
    internal static class NHibernateHelper
    {
        public static ISessionFactory CreateSessionFactory(bool createNew)
        {
            var configuration = BuildConfiguration();
            if (createNew)
            {
                configuration.ExposeConfiguration(CreateSchemaExport);
            }

            var sessionFactory = configuration.BuildSessionFactory();

            if (createNew)
            {
                PopulateDefaultData(sessionFactory);
            }

            return sessionFactory;
        }

        private static FluentConfiguration BuildConfiguration()
        {
            var configuration = Fluently.Configure()
                .Database(
                    MsSqlConfiguration.MsSql2012
                        .ConnectionString(q => q.FromConnectionStringWithKey("DefaultConnectionString")))
                .Mappings(m =>
                    m.FluentMappings.AddFromAssembly(Assembly.Load("WCC.Model")));
#if DEBUG || TRACE
            configuration.ExposeConfiguration(SetInterceptors);
#endif

            return configuration;
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

        private static void PopulateDefaultData(ISessionFactory sessionFactory)
        {
            using (var session = sessionFactory.OpenSession())
            {
                var types =
                    Assembly.GetExecutingAssembly().GetTypes().Where(type => typeof(IDefaultDataFactory).IsAssignableFrom(type) && !type.IsAbstract);

                foreach (var type in types)
                {
                    var instance = Activator.CreateInstance(type, session) as IDefaultDataFactory;
                    if (instance != null)
                    {
                        instance.Populate();
                    }
                }
            }
        }
    }
}
