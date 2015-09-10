using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Lynex.Common.Model.AspNet.Identity.Internal;
using Microsoft.AspNet.Identity;
using NHibernate;

namespace Lynex.Common.Model.AspNet.Identity
{
	public class UserStore<TUser> : IUserLoginStore<TUser>, IUserClaimStore<TUser>, IUserRoleStore<TUser>, IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IUserStore<TUser>, IDisposable
	where TUser : IdentityUser
	{
		private IEntityStore<TUser> _userStore;

		private readonly IEntityStore<IdentityRole> _roleStore;

		private readonly IEntityStore<IdentityUserLogin> _logins;

		private readonly IEntityStore<IdentityUserClaim> _userClaims;

		private bool _disposed;

		public ISession Session
		{
			get;
			private set;
		}

		public UserStore(ISession session) : this(new EntityStore<TUser>(session), new EntityStore<IdentityRole>(session), new EntityStore<IdentityUserLogin>(session), new EntityStore<IdentityUserClaim>(session))
		{
			if (session == null)
			{
				throw new ArgumentNullException("session");
			}
			this.Session = session;
		}

		internal UserStore(IEntityStore<TUser> userStore, IEntityStore<IdentityRole> roleStore, IEntityStore<IdentityUserLogin> logins, IEntityStore<IdentityUserClaim> userClaims)
		{
			this._userStore = userStore;
			this._roleStore = roleStore;
			this._logins = logins;
			this._userClaims = userClaims;
		}

		public Task AddClaimAsync(TUser user, Claim claim)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (claim == null)
			{
				throw new ArgumentNullException("claim");
			}
			IList<IdentityUserClaim> claims = user.Claims;
			IdentityUserClaim identityUserClaim = new IdentityUserClaim()
			{
				User = user,
				ClaimType = claim.Type,
				ClaimValue = claim.Value
			};
			claims.Add(identityUserClaim);
			return Task.FromResult<int>(0);
		}

		public Task AddLoginAsync(TUser user, UserLoginInfo login)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (login == null)
			{
				throw new ArgumentNullException("login");
			}
			IList<IdentityUserLogin> logins = user.Logins;
			IdentityUserLogin identityUserLogin = new IdentityUserLogin()
			{
				User = user,
				ProviderKey = login.ProviderKey,
				LoginProvider = login.LoginProvider
			};
			logins.Add(identityUserLogin);
			return Task.FromResult<int>(0);
		}

		public async Task AddToRoleAsync(TUser user, string role)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (string.IsNullOrWhiteSpace(role))
			{
				throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, "role");
			}
			IQueryable<IdentityRole> identityRoles = await this._roleStore.Records();
			IdentityRole identityRole = identityRoles.SingleOrDefault<IdentityRole>((IdentityRole r) => r.Name.ToUpper() == role.ToUpper());
			if (identityRole == null)
			{
				CultureInfo currentCulture = CultureInfo.CurrentCulture;
				string roleNotFound = IdentityResources.RoleNotFound;
				object[] objArray = new object[] { role };
				throw new InvalidOperationException(string.Format(currentCulture, roleNotFound, objArray));
			}
			user.Roles.Add(identityRole);
		}

		public async Task CreateAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			await this._userStore.Save(user);
		}

		public async Task DeleteAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			await this._userStore.Delete(user);
		}

		public void Dispose()
		{
			this._disposed = true;
			if (this.Session != null)
			{
				this.Session.Dispose();
				this.Session = null;
			}
			this._userStore = null;
		}

		public async Task<TUser> FindAsync(UserLoginInfo login)
		{
			UserStore<TUser> variable = null;
			this.ThrowIfDisposed();
			if (login == null)
			{
				throw new ArgumentNullException("login");
			}
			IQueryable<IdentityUserLogin> identityUserLogins = await this._logins.Records();
			ParameterExpression parameterExpression = Expression.Parameter(typeof(IdentityUserLogin), "l");
			BinaryExpression binaryExpression = Expression.AndAlso(Expression.Equal(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(IdentityUserLogin).GetMethod("get_LoginProvider").MethodHandle)), Expression.Property(Expression.Field(Expression.Constant(variable), FieldInfo.GetFieldFromHandle(typeof(UserStore<TUser>).GetField("login").FieldHandle, typeof(UserStore<TUser>).TypeHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(UserLoginInfo).GetMethod("get_LoginProvider").MethodHandle)), false, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("op_Equality", new System.Type[] { typeof(string), typeof(string) }).MethodHandle)), Expression.Equal(Expression.Property(parameterExpression, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(IdentityUserLogin).GetMethod("get_ProviderKey").MethodHandle)), Expression.Property(Expression.Field(Expression.Constant(variable), FieldInfo.GetFieldFromHandle(typeof(UserStore<TUser>).GetField("login").FieldHandle, typeof(UserStore<TUser>).TypeHandle)), (MethodInfo)MethodBase.GetMethodFromHandle(typeof(UserLoginInfo).GetMethod("get_ProviderKey").MethodHandle)), false, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(string).GetMethod("op_Equality", new System.Type[] { typeof(string), typeof(string) }).MethodHandle)));
			ParameterExpression[] parameterExpressionArray = new ParameterExpression[] { parameterExpression };
			IQueryable<IdentityUserLogin> identityUserLogins1 = identityUserLogins.Where<IdentityUserLogin>(Expression.Lambda<Func<IdentityUserLogin, bool>>(binaryExpression, parameterExpressionArray));
			ParameterExpression parameterExpression1 = Expression.Parameter(typeof(IdentityUserLogin), "l");
			MemberExpression memberExpression = Expression.Property(parameterExpression1, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(IdentityUserLogin).GetMethod("get_User").MethodHandle));
			ParameterExpression[] parameterExpressionArray1 = new ParameterExpression[] { parameterExpression1 };
			IdentityUser identityUser = identityUserLogins1.Select<IdentityUserLogin, IdentityUser>(Expression.Lambda<Func<IdentityUserLogin, IdentityUser>>(memberExpression, parameterExpressionArray1)).FirstOrDefault<IdentityUser>();
			return (TUser)(identityUser as TUser);
		}

		public async Task<TUser> FindByIdAsync(string userId)
		{
			this.ThrowIfDisposed();
			IQueryable<TUser> tUsers = await this._userStore.Records();
			TUser tUser = tUsers.SingleOrDefault<TUser>((TUser w) => w.Id == userId);
			return tUser;
		}

		public async Task<TUser> FindByNameAsync(string userName)
		{
			this.ThrowIfDisposed();
			IQueryable<TUser> tUsers = await this._userStore.Records();
			TUser tUser = tUsers.SingleOrDefault<TUser>((TUser w) => w.UserName == userName);
			return tUser;
		}

		public Task<IList<Claim>> GetClaimsAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}

            var list = user.Claims.Select(s => new Claim(s.ClaimType, s.ClaimValue)).ToList();
			return Task.FromResult<IList<Claim>>(list);
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
		    var list = user.Logins.Select(s => new UserLoginInfo(s.LoginProvider, s.ProviderKey)).ToList();
			return Task.FromResult<IList<UserLoginInfo>>(list);
		}

		public Task<string> GetPasswordHashAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.PasswordHash);
		}

		public Task<IList<string>> GetRolesAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<IList<string>>(user.Roles.Select(q => q.Name).ToList());
		}

		public Task<string> GetSecurityStampAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return Task.FromResult<string>(user.SecurityStamp);
		}

		public Task<bool> HasPasswordAsync(TUser user)
		{
			return Task.FromResult<bool>(user.PasswordHash != null);
		}

		public Task<bool> IsInRoleAsync(TUser user, string role)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (string.IsNullOrWhiteSpace(role))
			{
				throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, "role");
			}
			return Task.FromResult<bool>(user.Roles.Any<IdentityRole>((IdentityRole r) => r.Name.ToUpper() == role.ToUpper()));
		}

		public async Task RemoveClaimAsync(TUser user, Claim claim)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (claim == null)
			{
				throw new ArgumentNullException("claim");
			}
			foreach (IdentityUserClaim list in user.Claims.Where<IdentityUserClaim>((IdentityUserClaim uc) => {
				if (uc.ClaimValue != claim.Value)
				{
					return false;
				}
				return uc.ClaimType == claim.Type;
			}).ToList<IdentityUserClaim>())
			{
				user.Claims.Remove(list);
				await this._userClaims.Delete(list);
			}
		}

		public async Task RemoveFromRoleAsync(TUser user, string role)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (string.IsNullOrWhiteSpace(role))
			{
				throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, "role");
			}
			IdentityRole identityRole = user.Roles.FirstOrDefault<IdentityRole>((IdentityRole r) => r.Name.ToUpper() == role.ToUpper());
			if (identityRole != null)
			{
				user.Roles.Remove(identityRole);
				await this._userStore.Save(user);
			}
		}

		public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (login == null)
			{
				throw new ArgumentNullException("login");
			}
			IdentityUserLogin identityUserLogin = user.Logins.SingleOrDefault<IdentityUserLogin>((IdentityUserLogin l) => {
				if (l.LoginProvider != login.LoginProvider || l.User.Id != user.Id)
				{
					return false;
				}
				return l.ProviderKey == login.ProviderKey;
			});
			if (identityUserLogin != null)
			{
				user.Logins.Remove(identityUserLogin);
				await this._logins.Delete(identityUserLogin);
			}
		}

		public async Task SetPasswordHashAsync(TUser user, string passwordHash)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.PasswordHash = passwordHash;
			await this._userStore.Save(user);
		}

		public async Task SetSecurityStampAsync(TUser user, string stamp)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			user.SecurityStamp = stamp;
			await this._userStore.Save(user);
		}

		private void ThrowIfDisposed()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException(this.GetType().Name);
			}
		}

		public async Task UpdateAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			await this._userStore.Save(user);
		}
	}
}