using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Security.Claims;
using System.Threading.Tasks;
using Lynex.AspNet.Identity;
using Lynex.Common.Model.AspNet.Identity.Internal;
using NHibernate;

namespace Lynex.Common.Model.AspNet.Identity
{
	public class UserStore<TUser> : IUserLoginStore<TUser>, IUserClaimStore<TUser>, IUserRoleStore<TUser>, IUserPasswordStore<TUser>, IUserSecurityStampStore<TUser>, IUserStore<TUser>, IUserEmailStore<TUser>
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
				throw new ArgumentNullException(nameof(session));
			}
			Session = session;
		}

		internal UserStore(IEntityStore<TUser> userStore, IEntityStore<IdentityRole> roleStore, IEntityStore<IdentityUserLogin> logins, IEntityStore<IdentityUserClaim> userClaims)
		{
			_userStore = userStore;
			_roleStore = roleStore;
			_logins = logins;
			_userClaims = userClaims;
		}

		public Task AddClaimAsync(TUser user, Claim claim)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (claim == null)
			{
				throw new ArgumentNullException(nameof(claim));
			}
			var claims = user.Claims;
			var identityUserClaim = new IdentityUserClaim()
			{
				User = user,
				ClaimType = claim.Type,
				ClaimValue = claim.Value
			};
			claims.Add(identityUserClaim);
			return Task.FromResult(0);
		}

		public Task AddLoginAsync(TUser user, UserLoginInfo login)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (login == null)
			{
				throw new ArgumentNullException(nameof(login));
			}
			var logins = user.Logins;
			var identityUserLogin = new IdentityUserLogin
			{
				User = user,
				ProviderKey = login.ProviderKey,
				LoginProvider = login.LoginProvider
			};
			logins.Add(identityUserLogin);
			return Task.FromResult(0);
		}

		public async Task AddToRoleAsync(TUser user, string role)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (string.IsNullOrWhiteSpace(role))
			{
				throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, nameof(role));
			}
			var identityRoles = await _roleStore.Records();
			var identityRole = identityRoles.SingleOrDefault(r => r.Name.ToUpper() == role.ToUpper());
			if (identityRole == null)
			{
				var currentCulture = CultureInfo.CurrentCulture;
				var roleNotFound = IdentityResources.RoleNotFound;
				object[] objArray = { role };
				throw new InvalidOperationException(string.Format(currentCulture, roleNotFound, objArray));
			}
			user.Roles.Add(identityRole);
		}

		public async Task CreateAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
            await _userStore.Save(user);
        }




        public async Task DeleteAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			await _userStore.Delete(user);
		}

		public void Dispose()
		{
			_disposed = true;
			if (Session != null)
			{
				Session.Dispose();
				Session = null;
			}
			_userStore = null;
		}

		public async Task<TUser> FindAsync(UserLoginInfo login)
		{
		    ThrowIfDisposed();
			if (login == null)
			{
				throw new ArgumentNullException(nameof(login));
			}
			var identityUserLogins = await _logins.Records();
			var parameterExpression = Expression.Parameter(typeof(IdentityUserLogin), "l");
		    var binaryExpression = Expression.AndAlso(
		        Expression.Equal(
		            Expression.Property(
		                parameterExpression,
		                (MethodInfo)
		                    MethodBase.GetMethodFromHandle(
		                        typeof (IdentityUserLogin).GetMethod("get_LoginProvider").MethodHandle)),
		            Expression.Property(
		                Expression.Field(Expression.Constant(null),
		                    FieldInfo.GetFieldFromHandle(typeof (UserStore<TUser>).GetField("login").FieldHandle,
		                        typeof (UserStore<TUser>).TypeHandle)),
		                (MethodInfo)
		                    MethodBase.GetMethodFromHandle(typeof (UserLoginInfo).GetMethod("get_LoginProvider").MethodHandle)),
		            false,
		            (MethodInfo)
		                MethodBase.GetMethodFromHandle(
		                    typeof (string).GetMethod("op_Equality", new[] {typeof (string), typeof (string)}).MethodHandle)),
		        Expression.Equal(
		            Expression.Property(
		                parameterExpression,
		                (MethodInfo)
		                    MethodBase.GetMethodFromHandle(
		                        typeof (IdentityUserLogin).GetMethod("get_ProviderKey").MethodHandle)),
		            Expression.Property(
		                Expression.Field(Expression.Constant(null),
		                    FieldInfo.GetFieldFromHandle(typeof (UserStore<TUser>).GetField("login").FieldHandle,
		                        typeof (UserStore<TUser>).TypeHandle)),
		                (MethodInfo)
		                    MethodBase.GetMethodFromHandle(typeof (UserLoginInfo).GetMethod("get_ProviderKey").MethodHandle)),
		            false,
		            (MethodInfo) MethodBase.GetMethodFromHandle(
		                typeof (string).GetMethod("op_Equality", new[] {typeof (string), typeof (string)}).MethodHandle)));
			var parameterExpressionArray = new[] { parameterExpression };
			var identityUserLogins1 = identityUserLogins.Where(Expression.Lambda<Func<IdentityUserLogin, bool>>(binaryExpression, parameterExpressionArray));
			var parameterExpression1 = Expression.Parameter(typeof(IdentityUserLogin), "l");
			var memberExpression = Expression.Property(parameterExpression1, (MethodInfo)MethodBase.GetMethodFromHandle(typeof(IdentityUserLogin).GetMethod("get_User").MethodHandle));
			var parameterExpressionArray1 = new[] { parameterExpression1 };
			var identityUser = identityUserLogins1.Select(Expression.Lambda<Func<IdentityUserLogin, IdentityUser>>(memberExpression, parameterExpressionArray1)).FirstOrDefault();
			return identityUser as TUser;
		}

		public async Task<TUser> FindByIdAsync(string userId)
		{
			ThrowIfDisposed();
			var tUsers = await _userStore.Records();
			var tUser = tUsers.SingleOrDefault(w => w.Id == userId);
			return tUser;
		}

		public async Task<TUser> FindByNameAsync(string userName)
		{
			ThrowIfDisposed();
			var tUsers = await _userStore.Records();
			var tUser = tUsers.SingleOrDefault(w => w.UserName == userName);
			return tUser;
		}

		public Task<IList<Claim>> GetClaimsAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}

            var list = user.Claims.Select(s => new Claim(s.ClaimType, s.ClaimValue)).ToList();
			return Task.FromResult<IList<Claim>>(list);
		}

		public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
		    var list = user.Logins.Select(s => new UserLoginInfo(s.LoginProvider, s.ProviderKey)).ToList();
			return Task.FromResult<IList<UserLoginInfo>>(list);
		}

		public Task<string> GetPasswordHashAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			return Task.FromResult(user.PasswordHash);
		}

		public Task<IList<string>> GetRolesAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			return Task.FromResult<IList<string>>(user.Roles.Select(q => q.Name).ToList());
		}

		public Task<string> GetSecurityStampAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			return Task.FromResult(user.SecurityStamp);
		}

		public Task<bool> HasPasswordAsync(TUser user)
		{
			return Task.FromResult(user.PasswordHash != null);
		}

		public Task<bool> IsInRoleAsync(TUser user, string role)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (string.IsNullOrWhiteSpace(role))
			{
				throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, nameof(role));
			}
			return Task.FromResult(user.Roles.Any(r => r.Name.ToUpper() == role.ToUpper()));
		}

		public async Task RemoveClaimAsync(TUser user, Claim claim)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (claim == null)
			{
				throw new ArgumentNullException(nameof(claim));
			}
			foreach (var list in user.Claims.Where(uc => {
				if (uc.ClaimValue != claim.Value)
				{
					return false;
				}
				return uc.ClaimType == claim.Type;
			}).ToList())
			{
				user.Claims.Remove(list);
				await _userClaims.Delete(list);
			}
		}

		public async Task RemoveFromRoleAsync(TUser user, string role)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (string.IsNullOrWhiteSpace(role))
			{
				throw new ArgumentException(IdentityResources.ValueCannotBeNullOrEmpty, nameof(role));
			}
			var identityRole = user.Roles.FirstOrDefault(r => r.Name.ToUpper() == role.ToUpper());
			if (identityRole != null)
			{
				user.Roles.Remove(identityRole);
				await _userStore.Save(user);
			}
		}

		public async Task RemoveLoginAsync(TUser user, UserLoginInfo login)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (login == null)
			{
				throw new ArgumentNullException(nameof(login));
			}
			var identityUserLogin = user.Logins.SingleOrDefault(l => {
				if (l.LoginProvider != login.LoginProvider || l.User.Id != user.Id)
				{
					return false;
				}
				return l.ProviderKey == login.ProviderKey;
			});
			if (identityUserLogin != null)
			{
				user.Logins.Remove(identityUserLogin);
				await _logins.Delete(identityUserLogin);
			}
		}

		public async Task SetPasswordHashAsync(TUser user, string passwordHash)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			user.PasswordHash = passwordHash;
			await _userStore.Save(user);
		}

		public async Task SetSecurityStampAsync(TUser user, string stamp)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			user.SecurityStamp = stamp;
			await _userStore.Save(user);
		}

		private void ThrowIfDisposed()
		{
			if (_disposed)
			{
				throw new ObjectDisposedException(GetType().Name);
			}
		}

		public async Task UpdateAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			await _userStore.Save(user);
		}

	    public async Task SetEmailAsync(TUser user, string email)
	    {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }
            user.UserName = email;
            await _userStore.Save(user);
        }

	    public async Task<string> GetEmailAsync(TUser user)
	    {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Task.FromResult(user.UserName);
        }

	    public async Task<bool> GetEmailConfirmedAsync(TUser user)
	    {
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            return await Task.FromResult(true);
        }

	    public async Task SetEmailConfirmedAsync(TUser user, bool confirmed)
	    {
	        throw new NotImplementedException();
	    }

	    public async Task<TUser> FindByEmailAsync(string email)
	    {
            ThrowIfDisposed();
            var tUsers = await _userStore.Records();
            var tUser = tUsers.SingleOrDefault(w => w.UserName == email);
            return tUser;
        }
	}
}