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
					throw new ArgumentNullException("value");
				}
				this._roleValidator = value;
			}
		}

		public virtual IQueryable<TRole> Roles
		{
			get
			{
				IQueryableRoleStore<TRole, TKey> queryableRoleStore = this.Store as IQueryableRoleStore<TRole, TKey>;
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
				throw new ArgumentNullException("store");
			}
			this.Store = store;
			this.RoleValidator = new RoleValidator<TRole, TKey>(this);
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual async Task<IdentityResult> CreateAsync(TRole role)
		{
			this.ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			IdentityResult identityResult = await this.RoleValidator.ValidateAsync(role).WithCurrentCulture<IdentityResult>();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await this.Store.CreateAsync(role).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> UpdateAsync(TRole role)
		{
			this.ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			IdentityResult identityResult = await this.RoleValidator.ValidateAsync(role).WithCurrentCulture<IdentityResult>();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await this.Store.UpdateAsync(role).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> DeleteAsync(TRole role)
		{
			this.ThrowIfDisposed();
			if (role == null)
			{
				throw new ArgumentNullException("role");
			}
			await this.Store.DeleteAsync(role).WithCurrentCulture();
			return IdentityResult.Success;
		}

		public virtual async Task<bool> RoleExistsAsync(string roleName)
		{
			this.ThrowIfDisposed();
			if (roleName == null)
			{
				throw new ArgumentNullException("roleName");
			}
			return await this.FindByNameAsync(roleName).WithCurrentCulture<TRole>() != null;
		}

		public virtual async Task<TRole> FindByIdAsync(TKey roleId)
		{
			this.ThrowIfDisposed();
			return await this.Store.FindByIdAsync(roleId).WithCurrentCulture<TRole>();
		}

		public virtual async Task<TRole> FindByNameAsync(string roleName)
		{
			this.ThrowIfDisposed();
			if (roleName == null)
			{
				throw new ArgumentNullException("roleName");
			}
			return await this.Store.FindByNameAsync(roleName).WithCurrentCulture<TRole>();
		}

		private void ThrowIfDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing && !this._disposed)
			{
				this.Store.Dispose();
			}
			this._disposed = true;
		}
	}
	public class RoleManager<TRole> : RoleManager<TRole, string> where TRole : class, IRole<string>
	{
		public RoleManager(IRoleStore<TRole, string> store) : base(store)
		{
		}
	}
}
