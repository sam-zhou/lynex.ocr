using System;
using System.Linq;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class RoleManager<TRole, TKey> : IDisposable where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
	{
		private bool _disposed;

		private IIdentityValidator<TRole> _roleValidator;

		protected IRoleStore<TRole, TKey> Store
		{
			get;
			private set;
		}

		public IIdentityValidator<TRole> RoleValidator
		{
			get
			{
				return this._roleValidator;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_roleValidator = value;
			}
		}

		public virtual IQueryable<TRole> Roles
		{
			get
			{
				IQueryableRoleStore<TRole, TKey> queryableRoleStore = Store as IQueryableRoleStore<TRole, TKey>;
				if (queryableRoleStore == null)
				{
					throw new NotSupportedException(Resources.StoreNotIQueryableRoleStore);
				}
				return queryableRoleStore.Roles;
			}
		}

		public RoleManager(IRoleStore<TRole, TKey> store)
		{
			if (store == null)
			{
				throw new ArgumentNullException(nameof(store));
			}
			Store = store;
			RoleValidator = new RoleValidator<TRole, TKey>(this);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual async Task<IdentityResult> CreateAsync(TRole role)
		{
			ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			IdentityResult identityResult = await RoleValidator.ValidateAsync(role).WithCurrentCulture();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await Store.CreateAsync(role).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> UpdateAsync(TRole role)
		{
			ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			IdentityResult identityResult = await RoleValidator.ValidateAsync(role).WithCurrentCulture();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await Store.UpdateAsync(role).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> DeleteAsync(TRole role)
		{
			ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException(nameof(role));
			}
			await Store.DeleteAsync(role).WithCurrentCulture();
			return IdentityResult.Success;
		}

		public virtual async Task<bool> RoleExistsAsync(string roleName)
		{
			ThrowIfDisposed();
			if (roleName == null)
			{
				throw new ArgumentNullException(nameof(roleName));
			}
			return await FindByNameAsync(roleName).WithCurrentCulture() != null;
		}

		public virtual async Task<TRole> FindByIdAsync(TKey roleId)
		{
			ThrowIfDisposed();
			return await Store.FindByIdAsync(roleId).WithCurrentCulture();
		}

		public virtual async Task<TRole> FindByNameAsync(string roleName)
		{
			ThrowIfDisposed();
			if (roleName == null)
			{
				throw new ArgumentNullException(nameof(roleName));
			}
			return await Store.FindByNameAsync(roleName).WithCurrentCulture();
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !_disposed)
			{
				Store.Dispose();
			}
			_disposed = true;
		}
	}
	public class RoleManager<TRole> : RoleManager<TRole, string> where TRole : class, IRole<string>
	{
		public RoleManager(IRoleStore<TRole, string> store) : base(store)
		{
		}
	}
}
