using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Lynex.Common.Database.FluentNHibernate;
using Lynex.Common.Model.DbModel.Interface;
using NHibernate;

namespace Lynex.Common.Database
{
    public class DatabaseService : IDatabaseService, IDisposable
    {
        private readonly string _connectionStringKey;
        private readonly string _assemblyName;
        private ISessionFactory _sessionFactory;

        private ISession _session;

        private ITransaction _transaction;

        protected ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null || _sessionFactory.IsClosed)
                {
                    _sessionFactory = NHibernateHelper.CreateSessionFactory(_connectionStringKey, Assembly.Load(_assemblyName),false);
                }
                return _sessionFactory;
            }
        }

        protected ISession Session
        {
            get
            {
                if (_session == null || !_session.IsOpen)
                {
                    _session = SessionFactory.OpenSession();
                }
                return _session;
            }
        }

        public DatabaseService(string connectionStringKey, string assemblyName)
        {
            _connectionStringKey = connectionStringKey;
            _assemblyName = assemblyName;
        }

        public void ResetDatabase()
        {
            _sessionFactory = NHibernateHelper.CreateSessionFactory(_connectionStringKey, Assembly.Load(_assemblyName), true);
        }

        public void BeginTransaction()
        {
            _transaction = Session.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Commit();
                _transaction.Dispose();
            }
            _transaction = null;
        }

        public void RollBackTransaction()
        {
            if (_transaction != null)
            {
                _transaction.Rollback();
                _transaction.Dispose();
            }
            _transaction = null;
        }


        public TEntity Get<TEntity>(IGetQuery<TEntity> query) where TEntity : class
        {
            return query.Execute(Session);
        }

        public IEnumerable<TEntity> Get<TEntity>(IGetItemsQuery<TEntity> query) where TEntity : class
        {
            return query.Execute(Session);
        }

        public void ExecuteQuery(IExecuteQuery query)
        {
            query.Execute(Session);
        }

        public TEntity Get<TEntity>(long entityId) where TEntity : class, IBaseEntity
        {
            return Session.Get<TEntity>(entityId);
        }

        public TEntity Get<TEntity>() where TEntity : class
        {
            return Session.QueryOver<TEntity>().List().FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return Session.QueryOver<TEntity>().List();
        }

        public void Save<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            Session.Save(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
        {
            Session.Delete(entity);
        }

        public void Dispose()
        {
            if (_session != null)
            {

                _session.Dispose();
            }

            if (_sessionFactory != null)
            {
                _sessionFactory.Close();
                _sessionFactory.Dispose();
            }
        }
    }
}
