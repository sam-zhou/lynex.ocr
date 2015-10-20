using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class UserManager<TUser, TKey> : IDisposable where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		private readonly Dictionary<string, IUserTokenProvider<TUser, TKey>> _factors = new Dictionary<string, IUserTokenProvider<TUser, TKey>>();

		private IClaimsIdentityFactory<TUser, TKey> _claimsFactory;

		private TimeSpan _defaultLockout = TimeSpan.Zero;

		private bool _disposed;

		private IPasswordHasher _passwordHasher;

		private IIdentityValidator<string> _passwordValidator;

		private IIdentityValidator<TUser> _userValidator;

		protected internal IUserStore<TUser, TKey> Store
		{
			get;
			set;
		}

		public IPasswordHasher PasswordHasher
		{
			get
			{
				this.ThrowIfDisposed();
				return this._passwordHasher;
			}
			set
			{
				this.ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._passwordHasher = value;
			}
		}

		public IIdentityValidator<TUser> UserValidator
		{
			get
			{
				this.ThrowIfDisposed();
				return this._userValidator;
			}
			set
			{
				this.ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._userValidator = value;
			}
		}

		public IIdentityValidator<string> PasswordValidator
		{
			get
			{
				this.ThrowIfDisposed();
				return this._passwordValidator;
			}
			set
			{
				this.ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._passwordValidator = value;
			}
		}

		public IClaimsIdentityFactory<TUser, TKey> ClaimsIdentityFactory
		{
			get
			{
				this.ThrowIfDisposed();
				return this._claimsFactory;
			}
			set
			{
				this.ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this._claimsFactory = value;
			}
		}

		public IIdentityMessageService EmailService
		{
			get;
			set;
		}

		public IIdentityMessageService SmsService
		{
			get;
			set;
		}

		public IUserTokenProvider<TUser, TKey> UserTokenProvider
		{
			get;
			set;
		}

		public bool UserLockoutEnabledByDefault
		{
			get;
			set;
		}

		public int MaxFailedAccessAttemptsBeforeLockout
		{
			get;
			set;
		}

		public TimeSpan DefaultAccountLockoutTimeSpan
		{
			get
			{
				return this._defaultLockout;
			}
			set
			{
				this._defaultLockout = value;
			}
		}

		public virtual bool SupportsUserTwoFactor
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserTwoFactorStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserPassword
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserPasswordStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserSecurityStamp
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserSecurityStampStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserRole
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserRoleStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserLogin
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserLoginStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserEmail
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserEmailStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserPhoneNumber
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserPhoneNumberStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserClaim
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserClaimStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserLockout
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IUserLockoutStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsQueryableUsers
		{
			get
			{
				this.ThrowIfDisposed();
				return this.Store is IQueryableUserStore<TUser, TKey>;
			}
		}

		public virtual IQueryable<TUser> Users
		{
			get
			{
				IQueryableUserStore<TUser, TKey> queryableUserStore = this.Store as IQueryableUserStore<TUser, TKey>;
				if (queryableUserStore == null)
				{
					throw new NotSupportedException(Resources.StoreNotIQueryableUserStore);
				}
				return queryableUserStore.Users;
			}
		}

		public IDictionary<string, IUserTokenProvider<TUser, TKey>> TwoFactorProviders
		{
			get
			{
				return this._factors;
			}
		}

		public UserManager(IUserStore<TUser, TKey> store)
		{
			if (store == null)
			{
				throw new ArgumentNullException("store");
			}
			this.Store = store;
			this.UserValidator = new UserValidator<TUser, TKey>(this);
			this.PasswordValidator = new MinimumLengthValidator(6);
			this.PasswordHasher = new PasswordHasher();
			this.ClaimsIdentityFactory = new ClaimsIdentityFactory<TUser, TKey>();
		}

		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			return this.ClaimsIdentityFactory.CreateAsync(this, user, authenticationType);
		}

		public virtual async Task<IdentityResult> CreateAsync(TUser user)
		{
			this.ThrowIfDisposed();
			await this.UpdateSecurityStampInternal(user).WithCurrentCulture();
			IdentityResult identityResult = await this.UserValidator.ValidateAsync(user).WithCurrentCulture<IdentityResult>();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				if (this.UserLockoutEnabledByDefault && this.SupportsUserLockout)
				{
					await this.GetUserLockoutStore().SetLockoutEnabledAsync(user, true).WithCurrentCulture();
				}
				await this.Store.CreateAsync(user).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> UpdateAsync(TUser user)
		{
			this.ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			IdentityResult identityResult = await this.UserValidator.ValidateAsync(user).WithCurrentCulture<IdentityResult>();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await this.Store.UpdateAsync(user).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> DeleteAsync(TUser user)
		{
			this.ThrowIfDisposed();
			await this.Store.DeleteAsync(user).WithCurrentCulture();
			return IdentityResult.Success;
		}

		public virtual Task<TUser> FindByIdAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			return this.Store.FindByIdAsync(userId);
		}

		public virtual Task<TUser> FindByNameAsync(string userName)
		{
			this.ThrowIfDisposed();
			if (userName == null)
			{
				throw new ArgumentNullException("userName");
			}
			return this.Store.FindByNameAsync(userName);
		}

		private IUserPasswordStore<TUser, TKey> GetPasswordStore()
		{
			IUserPasswordStore<TUser, TKey> userPasswordStore = this.Store as IUserPasswordStore<TUser, TKey>;
			if (userPasswordStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserPasswordStore);
			}
			return userPasswordStore;
		}

		public virtual async Task<IdentityResult> CreateAsync(TUser user, string password)
		{
			this.ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = this.GetPasswordStore();
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			if (password == null)
			{
				throw new ArgumentNullException("password");
			}
			IdentityResult identityResult = await this.UpdatePassword(passwordStore, user, password).WithCurrentCulture<IdentityResult>();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				result = await this.CreateAsync(user).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}

		public virtual async Task<TUser> FindAsync(string userName, string password)
		{
			this.ThrowIfDisposed();
			TUser tUser = await this.FindByNameAsync(userName).WithCurrentCulture<TUser>();
			TUser result;
			if (tUser == null)
			{
				result = default(TUser);
			}
			else
			{
				result = ((await this.CheckPasswordAsync(tUser, password).WithCurrentCulture<bool>()) ? tUser : default(TUser));
			}
			return result;
		}

		public virtual async Task<bool> CheckPasswordAsync(TUser user, string password)
		{
			this.ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = this.GetPasswordStore();
			bool result;
			if (user == null)
			{
				result = false;
			}
			else
			{
				result = await this.VerifyPasswordAsync(passwordStore, user, password).WithCurrentCulture<bool>();
			}
			return result;
		}

		public virtual async Task<bool> HasPasswordAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = this.GetPasswordStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await passwordStore.HasPasswordAsync(tUser).WithCurrentCulture<bool>();
		}

		public virtual async Task<IdentityResult> AddPasswordAsync(TKey userId, string password)
		{
			this.ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = this.GetPasswordStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			string text = await passwordStore.GetPasswordHashAsync(tUser).WithCurrentCulture<string>();
			IdentityResult result;
			if (text != null)
			{
				result = new IdentityResult(new string[]
				{
					Resources.UserAlreadyHasPassword
				});
			}
			else
			{
				IdentityResult identityResult = await this.UpdatePassword(passwordStore, tUser, password).WithCurrentCulture<IdentityResult>();
				if (!identityResult.Succeeded)
				{
					result = identityResult;
				}
				else
				{
					result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
				}
			}
			return result;
		}

		public virtual async Task<IdentityResult> ChangePasswordAsync(TKey userId, string currentPassword, string newPassword)
		{
			this.ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = this.GetPasswordStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IdentityResult result;
			if (await this.VerifyPasswordAsync(passwordStore, tUser, currentPassword).WithCurrentCulture<bool>())
			{
				IdentityResult identityResult = await this.UpdatePassword(passwordStore, tUser, newPassword).WithCurrentCulture<IdentityResult>();
				if (!identityResult.Succeeded)
				{
					result = identityResult;
				}
				else
				{
					result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
				}
			}
			else
			{
				result = IdentityResult.Failed(new string[]
				{
					Resources.PasswordMismatch
				});
			}
			return result;
		}

		public virtual async Task<IdentityResult> RemovePasswordAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = this.GetPasswordStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await passwordStore.SetPasswordHashAsync(tUser, null).WithCurrentCulture();
			await this.UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		protected virtual async Task<IdentityResult> UpdatePassword(IUserPasswordStore<TUser, TKey> passwordStore, TUser user, string newPassword)
		{
			IdentityResult identityResult = await this.PasswordValidator.ValidateAsync(newPassword).WithCurrentCulture<IdentityResult>();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await passwordStore.SetPasswordHashAsync(user, this.PasswordHasher.HashPassword(newPassword)).WithCurrentCulture();
				await this.UpdateSecurityStampInternal(user).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		protected virtual async Task<bool> VerifyPasswordAsync(IUserPasswordStore<TUser, TKey> store, TUser user, string password)
		{
			string hashedPassword = await store.GetPasswordHashAsync(user).WithCurrentCulture<string>();
			return this.PasswordHasher.VerifyHashedPassword(hashedPassword, password) != PasswordVerificationResult.Failed;
		}

		private IUserSecurityStampStore<TUser, TKey> GetSecurityStore()
		{
			IUserSecurityStampStore<TUser, TKey> userSecurityStampStore = this.Store as IUserSecurityStampStore<TUser, TKey>;
			if (userSecurityStampStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserSecurityStampStore);
			}
			return userSecurityStampStore;
		}

		public virtual async Task<string> GetSecurityStampAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserSecurityStampStore<TUser, TKey> securityStore = this.GetSecurityStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await securityStore.GetSecurityStampAsync(tUser).WithCurrentCulture<string>();
		}

		public virtual async Task<IdentityResult> UpdateSecurityStampAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserSecurityStampStore<TUser, TKey> securityStore = this.GetSecurityStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await securityStore.SetSecurityStampAsync(tUser, UserManager<TUser, TKey>.NewSecurityStamp()).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual Task<string> GeneratePasswordResetTokenAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			return this.GenerateUserTokenAsync("ResetPassword", userId);
		}

		public virtual async Task<IdentityResult> ResetPasswordAsync(TKey userId, string token, string newPassword)
		{
			this.ThrowIfDisposed();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IdentityResult result;
			if (!(await this.VerifyUserTokenAsync(userId, "ResetPassword", token).WithCurrentCulture<bool>()))
			{
				result = IdentityResult.Failed(new string[]
				{
					Resources.InvalidToken
				});
			}
			else
			{
				IUserPasswordStore<TUser, TKey> passwordStore = this.GetPasswordStore();
				IdentityResult identityResult = await this.UpdatePassword(passwordStore, tUser, newPassword).WithCurrentCulture<IdentityResult>();
				if (!identityResult.Succeeded)
				{
					result = identityResult;
				}
				else
				{
					result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
				}
			}
			return result;
		}

		internal async Task UpdateSecurityStampInternal(TUser user)
		{
			if (this.SupportsUserSecurityStamp)
			{
				await this.GetSecurityStore().SetSecurityStampAsync(user, UserManager<TUser, TKey>.NewSecurityStamp()).WithCurrentCulture();
			}
		}

		private static string NewSecurityStamp()
		{
			return Guid.NewGuid().ToString();
		}

		private IUserLoginStore<TUser, TKey> GetLoginStore()
		{
			IUserLoginStore<TUser, TKey> userLoginStore = this.Store as IUserLoginStore<TUser, TKey>;
			if (userLoginStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserLoginStore);
			}
			return userLoginStore;
		}

		public virtual Task<TUser> FindAsync(UserLoginInfo login)
		{
			this.ThrowIfDisposed();
			return this.GetLoginStore().FindAsync(login);
		}

		public virtual async Task<IdentityResult> RemoveLoginAsync(TKey userId, UserLoginInfo login)
		{
			this.ThrowIfDisposed();
			IUserLoginStore<TUser, TKey> loginStore = this.GetLoginStore();
			if (login == null)
			{
				throw new ArgumentNullException("login");
			}
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await loginStore.RemoveLoginAsync(tUser, login).WithCurrentCulture();
			await this.UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual async Task<IdentityResult> AddLoginAsync(TKey userId, UserLoginInfo login)
		{
			this.ThrowIfDisposed();
			IUserLoginStore<TUser, TKey> loginStore = this.GetLoginStore();
			if (login == null)
			{
				throw new ArgumentNullException("login");
			}
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			TUser tUser2 = await this.FindAsync(login).WithCurrentCulture<TUser>();
			IdentityResult result;
			if (tUser2 != null)
			{
				result = IdentityResult.Failed(new string[]
				{
					Resources.ExternalLoginExists
				});
			}
			else
			{
				await loginStore.AddLoginAsync(tUser, login).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}

		public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserLoginStore<TUser, TKey> loginStore = this.GetLoginStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await loginStore.GetLoginsAsync(tUser).WithCurrentCulture<IList<UserLoginInfo>>();
		}

		private IUserClaimStore<TUser, TKey> GetClaimStore()
		{
			IUserClaimStore<TUser, TKey> userClaimStore = this.Store as IUserClaimStore<TUser, TKey>;
			if (userClaimStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserClaimStore);
			}
			return userClaimStore;
		}

		public virtual async Task<IdentityResult> AddClaimAsync(TKey userId, Claim claim)
		{
			this.ThrowIfDisposed();
			IUserClaimStore<TUser, TKey> claimStore = this.GetClaimStore();
			if (claim == null)
			{
				throw new ArgumentNullException("claim");
			}
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await claimStore.AddClaimAsync(tUser, claim).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual async Task<IdentityResult> RemoveClaimAsync(TKey userId, Claim claim)
		{
			this.ThrowIfDisposed();
			IUserClaimStore<TUser, TKey> claimStore = this.GetClaimStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await claimStore.RemoveClaimAsync(tUser, claim).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual async Task<IList<Claim>> GetClaimsAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserClaimStore<TUser, TKey> claimStore = this.GetClaimStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await claimStore.GetClaimsAsync(tUser).WithCurrentCulture<IList<Claim>>();
		}

		private IUserRoleStore<TUser, TKey> GetUserRoleStore()
		{
			IUserRoleStore<TUser, TKey> userRoleStore = this.Store as IUserRoleStore<TUser, TKey>;
			if (userRoleStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserRoleStore);
			}
			return userRoleStore;
		}

		public virtual async Task<IdentityResult> AddToRoleAsync(TKey userId, string role)
		{
			this.ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = this.GetUserRoleStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IList<string> list = await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture<IList<string>>();
			IdentityResult result;
			if (list.Contains(role))
			{
				result = new IdentityResult(new string[]
				{
					Resources.UserAlreadyInRole
				});
			}
			else
			{
				await userRoleStore.AddToRoleAsync(tUser, role).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}


        public virtual async Task<IdentityResult> AddToRolesAsync(TKey userId, params string[] roles)
        {
            this.ThrowIfDisposed();
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var newRoles = roles.ToList();

            IUserRoleStore<TUser, TKey> userRoleStore = this.GetUserRoleStore();
            TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
            if (tUser == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
                {
                    userId
                }));
            }
            IList<string> list = await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture<IList<string>>();
            IdentityResult result = null;

            if (newRoles.Any(q => !list.Contains(q)))
            {
                foreach (var role in newRoles.Where(q => !list.Contains(q)))
                {
                    await userRoleStore.AddToRoleAsync(tUser, role).WithCurrentCulture();
                    result = await UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
                }
            }
            else
            {
                result = new IdentityResult(new string[]
                {
                    Resources.UserAlreadyInRole
                });
            }

            
            
            return result;
        }


        public virtual async Task<IdentityResult> RemoveFromRolesAsync(TKey userId, params string[] roles)
        {
            this.ThrowIfDisposed();
            IUserRoleStore<TUser, TKey> userRoleStore = this.GetUserRoleStore();
            TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
            if (tUser == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
                {
                    userId
                }));
            }

            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var newRoles = roles.ToList();

            IList<string> list = await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture<IList<string>>();

            IdentityResult result = null;

            if (newRoles.Any(q => list.Contains(q)))
            {
                foreach (var role in newRoles.Where(q => list.Contains(q)))
                {
                    await userRoleStore.RemoveFromRoleAsync(tUser, role).WithCurrentCulture();
                    result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
                }
            }
            else
            {
                result = new IdentityResult(new string[]
                {
                    Resources.UserNotInRole
                });
            }

            return result;
        }

        public virtual async Task<IdentityResult> RemoveFromRoleAsync(TKey userId, string role)
		{
			this.ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = this.GetUserRoleStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IdentityResult result;
			if (!(await userRoleStore.IsInRoleAsync(tUser, role).WithCurrentCulture<bool>()))
			{
				result = new IdentityResult(new string[]
				{
					Resources.UserNotInRole
				});
			}
			else
			{
				await userRoleStore.RemoveFromRoleAsync(tUser, role).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}

		public virtual async Task<IList<string>> GetRolesAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = this.GetUserRoleStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture<IList<string>>();
		}

		public virtual async Task<bool> IsInRoleAsync(TKey userId, string role)
		{
			this.ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = this.GetUserRoleStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await userRoleStore.IsInRoleAsync(tUser, role).WithCurrentCulture<bool>();
		}

		internal IUserEmailStore<TUser, TKey> GetEmailStore()
		{
			IUserEmailStore<TUser, TKey> userEmailStore = this.Store as IUserEmailStore<TUser, TKey>;
			if (userEmailStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserEmailStore);
			}
			return userEmailStore;
		}

		public virtual async Task<string> GetEmailAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = this.GetEmailStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await emailStore.GetEmailAsync(tUser).WithCurrentCulture<string>();
		}

		public virtual async Task<IdentityResult> SetEmailAsync(TKey userId, string email)
		{
			this.ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = this.GetEmailStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await emailStore.SetEmailAsync(tUser, email).WithCurrentCulture();
			await emailStore.SetEmailConfirmedAsync(tUser, false).WithCurrentCulture();
			await this.UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual Task<TUser> FindByEmailAsync(string email)
		{
			this.ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = this.GetEmailStore();
			if (email == null)
			{
				throw new ArgumentNullException("email");
			}
			return emailStore.FindByEmailAsync(email);
		}

		public virtual Task<string> GenerateEmailConfirmationTokenAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			return this.GenerateUserTokenAsync("Confirmation", userId);
		}

		public virtual async Task<IdentityResult> ConfirmEmailAsync(TKey userId, string token)
		{
			this.ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = this.GetEmailStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IdentityResult result;
			if (!(await this.VerifyUserTokenAsync(userId, "Confirmation", token).WithCurrentCulture<bool>()))
			{
				result = IdentityResult.Failed(new string[]
				{
					Resources.InvalidToken
				});
			}
			else
			{
				await emailStore.SetEmailConfirmedAsync(tUser, true).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}

		public virtual async Task<bool> IsEmailConfirmedAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = this.GetEmailStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await emailStore.GetEmailConfirmedAsync(tUser).WithCurrentCulture<bool>();
		}

		internal IUserPhoneNumberStore<TUser, TKey> GetPhoneNumberStore()
		{
			IUserPhoneNumberStore<TUser, TKey> userPhoneNumberStore = this.Store as IUserPhoneNumberStore<TUser, TKey>;
			if (userPhoneNumberStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserPhoneNumberStore);
			}
			return userPhoneNumberStore;
		}

		public virtual async Task<string> GetPhoneNumberAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = this.GetPhoneNumberStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await phoneNumberStore.GetPhoneNumberAsync(tUser).WithCurrentCulture<string>();
		}

		public virtual async Task<IdentityResult> SetPhoneNumberAsync(TKey userId, string phoneNumber)
		{
			this.ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = this.GetPhoneNumberStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await phoneNumberStore.SetPhoneNumberAsync(tUser, phoneNumber).WithCurrentCulture();
			await phoneNumberStore.SetPhoneNumberConfirmedAsync(tUser, false).WithCurrentCulture();
			await this.UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual async Task<IdentityResult> ChangePhoneNumberAsync(TKey userId, string phoneNumber, string token)
		{
			this.ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = this.GetPhoneNumberStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IdentityResult result;
			if (await this.VerifyChangePhoneNumberTokenAsync(userId, token, phoneNumber).WithCurrentCulture<bool>())
			{
				await phoneNumberStore.SetPhoneNumberAsync(tUser, phoneNumber).WithCurrentCulture();
				await phoneNumberStore.SetPhoneNumberConfirmedAsync(tUser, true).WithCurrentCulture();
				await this.UpdateSecurityStampInternal(tUser).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			else
			{
				result = IdentityResult.Failed(new string[]
				{
					Resources.InvalidToken
				});
			}
			return result;
		}

		public virtual async Task<bool> IsPhoneNumberConfirmedAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = this.GetPhoneNumberStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await phoneNumberStore.GetPhoneNumberConfirmedAsync(tUser).WithCurrentCulture<bool>();
		}

		internal async Task<SecurityToken> CreateSecurityTokenAsync(TKey userId)
		{
			return new SecurityToken(Encoding.Unicode.GetBytes(await this.GetSecurityStampAsync(userId).WithCurrentCulture<string>()));
		}

		public virtual async Task<string> GenerateChangePhoneNumberTokenAsync(TKey userId, string phoneNumber)
		{
			this.ThrowIfDisposed();
			return Rfc6238AuthenticationService.GenerateCode(await this.CreateSecurityTokenAsync(userId).WithCurrentCulture<SecurityToken>(), phoneNumber).ToString("D6", CultureInfo.InvariantCulture);
		}

		public virtual async Task<bool> VerifyChangePhoneNumberTokenAsync(TKey userId, string token, string phoneNumber)
		{
			this.ThrowIfDisposed();
			SecurityToken securityToken = await this.CreateSecurityTokenAsync(userId).WithCurrentCulture<SecurityToken>();
			int code;
			bool result;
			if (securityToken != null && int.TryParse(token, out code))
			{
				result = Rfc6238AuthenticationService.ValidateCode(securityToken, code, phoneNumber);
			}
			else
			{
				result = false;
			}
			return result;
		}

		public virtual async Task<bool> VerifyUserTokenAsync(TKey userId, string purpose, string token)
		{
			this.ThrowIfDisposed();
			if (this.UserTokenProvider == null)
			{
				throw new NotSupportedException(Resources.NoTokenProvider);
			}
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await this.UserTokenProvider.ValidateAsync(purpose, token, this, tUser).WithCurrentCulture<bool>();
		}

		public virtual async Task<string> GenerateUserTokenAsync(string purpose, TKey userId)
		{
			this.ThrowIfDisposed();
			if (this.UserTokenProvider == null)
			{
				throw new NotSupportedException(Resources.NoTokenProvider);
			}
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await this.UserTokenProvider.GenerateAsync(purpose, this, tUser).WithCurrentCulture<string>();
		}

		public virtual void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<TUser, TKey> provider)
		{
			this.ThrowIfDisposed();
			if (twoFactorProvider == null)
			{
				throw new ArgumentNullException("twoFactorProvider");
			}
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}
			this.TwoFactorProviders[twoFactorProvider] = provider;
		}

		public virtual async Task<IList<string>> GetValidTwoFactorProvidersAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, IUserTokenProvider<TUser, TKey>> current in this.TwoFactorProviders)
			{
				KeyValuePair<string, IUserTokenProvider<TUser, TKey>> var_10_12D = current;
				if (await var_10_12D.Value.IsValidProviderForUserAsync(this, tUser).WithCurrentCulture<bool>())
				{
					List<string> arg_1C9_0 = list;
					KeyValuePair<string, IUserTokenProvider<TUser, TKey>> var_14_1C0 = current;
					arg_1C9_0.Add(var_14_1C0.Key);
				}
			}
			return list;
		}

		public virtual async Task<bool> VerifyTwoFactorTokenAsync(TKey userId, string twoFactorProvider, string token)
		{
			this.ThrowIfDisposed();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			if (!this._factors.ContainsKey(twoFactorProvider))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.NoTwoFactorProvider, new object[]
				{
					twoFactorProvider
				}));
			}
			IUserTokenProvider<TUser, TKey> userTokenProvider = this._factors[twoFactorProvider];
			return await userTokenProvider.ValidateAsync(twoFactorProvider, token, this, tUser).WithCurrentCulture<bool>();
		}

		public virtual async Task<string> GenerateTwoFactorTokenAsync(TKey userId, string twoFactorProvider)
		{
			this.ThrowIfDisposed();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			if (!this._factors.ContainsKey(twoFactorProvider))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.NoTwoFactorProvider, new object[]
				{
					twoFactorProvider
				}));
			}
			return await this._factors[twoFactorProvider].GenerateAsync(twoFactorProvider, this, tUser).WithCurrentCulture<string>();
		}

		public virtual async Task<IdentityResult> NotifyTwoFactorTokenAsync(TKey userId, string twoFactorProvider, string token)
		{
			this.ThrowIfDisposed();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			if (!this._factors.ContainsKey(twoFactorProvider))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.NoTwoFactorProvider, new object[]
				{
					twoFactorProvider
				}));
			}
			await this._factors[twoFactorProvider].NotifyAsync(token, this, tUser).WithCurrentCulture();
			return IdentityResult.Success;
		}

		internal IUserTwoFactorStore<TUser, TKey> GetUserTwoFactorStore()
		{
			IUserTwoFactorStore<TUser, TKey> userTwoFactorStore = this.Store as IUserTwoFactorStore<TUser, TKey>;
			if (userTwoFactorStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserTwoFactorStore);
			}
			return userTwoFactorStore;
		}

		public virtual async Task<bool> GetTwoFactorEnabledAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserTwoFactorStore<TUser, TKey> userTwoFactorStore = this.GetUserTwoFactorStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await userTwoFactorStore.GetTwoFactorEnabledAsync(tUser).WithCurrentCulture<bool>();
		}

		public virtual async Task<IdentityResult> SetTwoFactorEnabledAsync(TKey userId, bool enabled)
		{
			this.ThrowIfDisposed();
			IUserTwoFactorStore<TUser, TKey> userTwoFactorStore = this.GetUserTwoFactorStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await userTwoFactorStore.SetTwoFactorEnabledAsync(tUser, enabled).WithCurrentCulture();
			await this.UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual async Task SendEmailAsync(TKey userId, string subject, string body)
		{
			this.ThrowIfDisposed();
			if (this.EmailService != null)
			{
				IdentityMessage message = new IdentityMessage
				{
					Destination = await this.GetEmailAsync(userId).WithCurrentCulture<string>(),
					Subject = subject,
					Body = body
				};
				await this.EmailService.SendAsync(message).WithCurrentCulture();
			}
		}

		public virtual async Task SendSmsAsync(TKey userId, string message)
		{
			this.ThrowIfDisposed();
			if (this.SmsService != null)
			{
				IdentityMessage message2 = new IdentityMessage
				{
					Destination = await this.GetPhoneNumberAsync(userId).WithCurrentCulture<string>(),
					Body = message
				};
				await this.SmsService.SendAsync(message2).WithCurrentCulture();
			}
		}

		internal IUserLockoutStore<TUser, TKey> GetUserLockoutStore()
		{
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.Store as IUserLockoutStore<TUser, TKey>;
			if (userLockoutStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserLockoutStore);
			}
			return userLockoutStore;
		}

		public virtual async Task<bool> IsLockedOutAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			bool result;
			if (!(await userLockoutStore.GetLockoutEnabledAsync(tUser).WithCurrentCulture<bool>()))
			{
				result = false;
			}
			else
			{
				DateTimeOffset left = await userLockoutStore.GetLockoutEndDateAsync(tUser).WithCurrentCulture<DateTimeOffset>();
				result = (left >= DateTimeOffset.UtcNow);
			}
			return result;
		}

		public virtual async Task<IdentityResult> SetLockoutEnabledAsync(TKey userId, bool enabled)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			await userLockoutStore.SetLockoutEnabledAsync(tUser, enabled).WithCurrentCulture();
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual async Task<bool> GetLockoutEnabledAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await userLockoutStore.GetLockoutEnabledAsync(tUser).WithCurrentCulture<bool>();
		}

		public virtual async Task<DateTimeOffset> GetLockoutEndDateAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await userLockoutStore.GetLockoutEndDateAsync(tUser).WithCurrentCulture<DateTimeOffset>();
		}

		public virtual async Task<IdentityResult> SetLockoutEndDateAsync(TKey userId, DateTimeOffset lockoutEnd)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IdentityResult result;
			if (!(await userLockoutStore.GetLockoutEnabledAsync(tUser).WithCurrentCulture<bool>()))
			{
				result = IdentityResult.Failed(new string[]
				{
					Resources.LockoutNotEnabled
				});
			}
			else
			{
				await userLockoutStore.SetLockoutEndDateAsync(tUser, lockoutEnd).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}

		public virtual async Task<IdentityResult> AccessFailedAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			int num = await userLockoutStore.IncrementAccessFailedCountAsync(tUser).WithCurrentCulture<int>();
			if (num >= this.MaxFailedAccessAttemptsBeforeLockout)
			{
				await userLockoutStore.SetLockoutEndDateAsync(tUser, DateTimeOffset.UtcNow.Add(this.DefaultAccountLockoutTimeSpan)).WithCurrentCulture();
				await userLockoutStore.ResetAccessFailedCountAsync(tUser).WithCurrentCulture();
			}
			return await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
		}

		public virtual async Task<IdentityResult> ResetAccessFailedCountAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			IdentityResult result;
			if ((await GetAccessFailedCountAsync(tUser.Id).WithCurrentCulture<int>()) != 0)
			{
				result = IdentityResult.Success;
			}
			else
			{
				await userLockoutStore.ResetAccessFailedCountAsync(tUser).WithCurrentCulture();
				result = await this.UpdateAsync(tUser).WithCurrentCulture<IdentityResult>();
			}
			return result;
		}

		public virtual async Task<int> GetAccessFailedCountAsync(TKey userId)
		{
			this.ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = this.GetUserLockoutStore();
			TUser tUser = await this.FindByIdAsync(userId).WithCurrentCulture<TUser>();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, new object[]
				{
					userId
				}));
			}
			return await userLockoutStore.GetAccessFailedCountAsync(tUser).WithCurrentCulture<int>();
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
				this._disposed = true;
			}
		}
	}
	public class UserManager<TUser> : UserManager<TUser, string> where TUser : class, IUser<string>
	{
		public UserManager(IUserStore<TUser> store) : base(store)
		{
		}
	}
}
