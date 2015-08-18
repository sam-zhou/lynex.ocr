using System;
using System.Linq;
using System.Threading.Tasks;
using Lynex.Common.Model.AspNet.Identity.Internal;
using Microsoft.AspNet.Identity;
using NHibernate;

namespace Lynex.Common.Model.AspNet.Identity
{
	public class RoleStore<TRole> : IRoleStore<TRole>, IDisposable
	where TRole : IdentityRole
	{
		private IEntityStore<TRole> _roleStore;

		private bool _disposed;

		public ISession Session
		{
			get;
			private set;
		}

		public RoleStore(ISession session) : this(new EntityStore<TRole>(session))
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			Session = session;
		}

		internal RoleStore(IEntityStore<TRole> roleStore)
		{
			_roleStore = roleStore;
		}

		public async Task CreateAsync(TRole role)
		{
			this.ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			await this._roleStore.Save(role);
		}

		public async Task DeleteAsync(TRole role)
		{
			this.ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			await this._roleStore.Delete(role);
		}

		public void Dispose()
		{
			this._disposed = true;
			if (this.Session != null)
			{
				this.Session.Dispose();
				this.Session = null;
			}
			this._roleStore = null;
		}

		public async Task<TRole> FindByIdAsync(string roleId)
		{
			this.ThrowIfDisposed();
			IQueryable<TRole> tRoles = await this._roleStore.Records();
			TRole tRole = tRoles.SingleOrDefault<TRole>((TRole w) => w.Id == roleId);
			return tRole;
		}

		public async Task<TRole> FindByNameAsync(string roleName)
		{
			this.ThrowIfDisposed();
			IQueryable<TRole> tRoles = await this._roleStore.Records();
			TRole tRole = tRoles.SingleOrDefault<TRole>((TRole w) => string.Equals(w.Name, roleName, StringComparison.CurrentCultureIgnoreCase));
			return tRole;
		}

		private void ThrowIfDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		public async Task UpdateAsync(TRole role)
		{
			this.ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			await this._roleStore.Save(role);
		}
	}
}