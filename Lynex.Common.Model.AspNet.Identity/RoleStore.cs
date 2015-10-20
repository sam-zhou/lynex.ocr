using System;
using System.Linq;
using System.Threading.Tasks;
using Lynex.AspNet.Identity;
using Lynex.Common.Model.AspNet.Identity.Internal;
using NHibernate;

namespace Lynex.Common.Model.AspNet.Identity
{
	public class RoleStore<TRole> : IRoleStore<TRole>
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
				throw new ArgumentNullException(nameof(session));
			}
			Session = session;
		}

		internal RoleStore(IEntityStore<TRole> roleStore)
		{
			_roleStore = roleStore;
		}

		public async Task CreateAsync(TRole role)
		{
			ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			await _roleStore.Save(role);
		}

		public async Task DeleteAsync(TRole role)
		{
			ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			await _roleStore.Delete(role);
		}

		public void Dispose()
		{
			_disposed = true;
			if (Session != null)
			{
				Session.Dispose();
				Session = null;
			}
			_roleStore = null;
		}

		public async Task<TRole> FindByIdAsync(string roleId)
		{
			ThrowIfDisposed();
			var tRoles = await _roleStore.Records();
			var tRole = tRoles.SingleOrDefault(w => w.Id == roleId);
			return tRole;
		}

		public async Task<TRole> FindByNameAsync(string roleName)
		{
			ThrowIfDisposed();
			var tRoles = await _roleStore.Records();
			var tRole = tRoles.SingleOrDefault(w => string.Equals(w.Name, roleName, StringComparison.CurrentCultureIgnoreCase));
			return tRole;
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		public async Task UpdateAsync(TRole role)
		{
			ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			await _roleStore.Save(role);
		}
	}
}