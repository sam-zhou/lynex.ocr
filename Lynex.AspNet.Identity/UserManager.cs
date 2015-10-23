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
				ThrowIfDisposed();
				return _passwordHasher;
			}
			set
			{
				ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_passwordHasher = value;
			}
		}

		public IIdentityValidator<TUser> UserValidator
		{
			get
			{
				ThrowIfDisposed();
				return _userValidator;
			}
			set
			{
				ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_userValidator = value;
			}
		}

		public IIdentityValidator<string> PasswordValidator
		{
			get
			{
				ThrowIfDisposed();
				return _passwordValidator;
			}
			set
			{
				ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_passwordValidator = value;
			}
		}

		public IClaimsIdentityFactory<TUser, TKey> ClaimsIdentityFactory
		{
			get
			{
				ThrowIfDisposed();
				return _claimsFactory;
			}
			set
			{
				ThrowIfDisposed();
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}
				_claimsFactory = value;
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
				return _defaultLockout;
			}
			set
			{
				_defaultLockout = value;
			}
		}

		public virtual bool SupportsUserTwoFactor
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserTwoFactorStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserPassword
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserPasswordStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserSecurityStamp
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserSecurityStampStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserRole
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserRoleStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserLogin
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserLoginStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserEmail
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserEmailStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserPhoneNumber
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserPhoneNumberStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserClaim
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserClaimStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsUserLockout
		{
			get
			{
				ThrowIfDisposed();
				return Store is IUserLockoutStore<TUser, TKey>;
			}
		}

		public virtual bool SupportsQueryableUsers
		{
			get
			{
				ThrowIfDisposed();
				return Store is IQueryableUserStore<TUser, TKey>;
			}
		}

		public virtual IQueryable<TUser> Users
		{
			get
			{
				IQueryableUserStore<TUser, TKey> queryableUserStore = Store as IQueryableUserStore<TUser, TKey>;
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
				return _factors;
			}
		}

		public UserManager(IUserStore<TUser, TKey> store)
		{
			if (store == null)
			{
				throw new ArgumentNullException(nameof(store));
			}
			Store = store;
			UserValidator = new UserValidator<TUser, TKey>(this);
			PasswordValidator = new MinimumLengthValidator(6);
			PasswordHasher = new PasswordHasher();
			ClaimsIdentityFactory = new ClaimsIdentityFactory<TUser, TKey>();
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		public virtual Task<ClaimsIdentity> CreateIdentityAsync(TUser user, string authenticationType)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			return ClaimsIdentityFactory.CreateAsync(this, user, authenticationType);
		}

		public virtual async Task<IdentityResult> CreateAsync(TUser user)
		{
			ThrowIfDisposed();

			IdentityResult identityResult = await UserValidator.ValidateAsync(user).WithCurrentCulture();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
                await UpdateSecurityStampInternal(user).WithCurrentCulture();

                if (UserLockoutEnabledByDefault && SupportsUserLockout)
				{
					await GetUserLockoutStore().SetLockoutEnabledAsync(user, true).WithCurrentCulture();
				}
				await Store.CreateAsync(user).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> UpdateAsync(TUser user)
		{
			ThrowIfDisposed();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			IdentityResult identityResult = await UserValidator.ValidateAsync(user).WithCurrentCulture();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await Store.UpdateAsync(user).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		public virtual async Task<IdentityResult> DeleteAsync(TUser user)
		{
			ThrowIfDisposed();
			await Store.DeleteAsync(user).WithCurrentCulture();
			return IdentityResult.Success;
		}

		public virtual Task<TUser> FindByIdAsync(TKey userId)
		{
			ThrowIfDisposed();
			return Store.FindByIdAsync(userId);
		}

		public virtual Task<TUser> FindByNameAsync(string userName)
		{
			ThrowIfDisposed();
			if (userName == null)
			{
				throw new ArgumentNullException(nameof(userName));
			}
			return Store.FindByNameAsync(userName);
		}

		private IUserPasswordStore<TUser, TKey> GetPasswordStore()
		{
			IUserPasswordStore<TUser, TKey> userPasswordStore = Store as IUserPasswordStore<TUser, TKey>;
			if (userPasswordStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserPasswordStore);
			}
			return userPasswordStore;
		}

		public virtual async Task<IdentityResult> CreateAsync(TUser user, string password)
		{
			ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = GetPasswordStore();
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
			}
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}
		    
			IdentityResult createResult = await CreateAsync(user).WithCurrentCulture();

            IdentityResult identityResult;
            if (!createResult.Succeeded)
			{
                identityResult = createResult;
			}
			else
			{
                identityResult = await UpdatePassword(passwordStore, user, password).WithCurrentCulture();
            }
			return identityResult;
		}

		public virtual async Task<TUser> FindAsync(string userName, string password)
		{
			ThrowIfDisposed();
			TUser tUser = await FindByNameAsync(userName).WithCurrentCulture();
			TUser result;
			if (tUser == null)
			{
				result = default(TUser);
			}
			else
			{
				result = ((await CheckPasswordAsync(tUser, password).WithCurrentCulture()) ? tUser : default(TUser));
			}
			return result;
		}

		public virtual async Task<bool> CheckPasswordAsync(TUser user, string password)
		{
			ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = GetPasswordStore();
			bool result;
			if (user == null)
			{
				result = false;
			}
			else
			{
				result = await VerifyPasswordAsync(passwordStore, user, password).WithCurrentCulture();
			}
			return result;
		}

		public virtual async Task<bool> HasPasswordAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = GetPasswordStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await passwordStore.HasPasswordAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> AddPasswordAsync(TKey userId, string password)
		{
			ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = GetPasswordStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			string text = await passwordStore.GetPasswordHashAsync(tUser).WithCurrentCulture();
			IdentityResult result;
			if (text != null)
			{
				result = new IdentityResult(Resources.UserAlreadyHasPassword);
			}
			else
			{
				IdentityResult identityResult = await UpdatePassword(passwordStore, tUser, password).WithCurrentCulture();
				if (!identityResult.Succeeded)
				{
					result = identityResult;
				}
				else
				{
					result = await UpdateAsync(tUser).WithCurrentCulture();
				}
			}
			return result;
		}

		public virtual async Task<IdentityResult> ChangePasswordAsync(TKey userId, string currentPassword, string newPassword)
		{
			ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = GetPasswordStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IdentityResult result;
			if (await VerifyPasswordAsync(passwordStore, tUser, currentPassword).WithCurrentCulture())
			{
				IdentityResult identityResult = await UpdatePassword(passwordStore, tUser, newPassword).WithCurrentCulture();
				if (!identityResult.Succeeded)
				{
					result = identityResult;
				}
				else
				{
					result = await UpdateAsync(tUser).WithCurrentCulture();
				}
			}
			else
			{
				result = IdentityResult.Failed(Resources.PasswordMismatch);
			}
			return result;
		}

		public virtual async Task<IdentityResult> RemovePasswordAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserPasswordStore<TUser, TKey> passwordStore = GetPasswordStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await passwordStore.SetPasswordHashAsync(tUser, null).WithCurrentCulture();
			await UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		protected virtual async Task<IdentityResult> UpdatePassword(IUserPasswordStore<TUser, TKey> passwordStore, TUser user, string newPassword)
		{
			IdentityResult identityResult = await PasswordValidator.ValidateAsync(newPassword).WithCurrentCulture();
			IdentityResult result;
			if (!identityResult.Succeeded)
			{
				result = identityResult;
			}
			else
			{
				await passwordStore.SetPasswordHashAsync(user, PasswordHasher.HashPassword(newPassword)).WithCurrentCulture();
				await UpdateSecurityStampInternal(user).WithCurrentCulture();
				result = IdentityResult.Success;
			}
			return result;
		}

		protected virtual async Task<bool> VerifyPasswordAsync(IUserPasswordStore<TUser, TKey> store, TUser user, string password)
		{
			string hashedPassword = await store.GetPasswordHashAsync(user).WithCurrentCulture();
			return PasswordHasher.VerifyHashedPassword(hashedPassword, password) != PasswordVerificationResult.Failed;
		}

		private IUserSecurityStampStore<TUser, TKey> GetSecurityStore()
		{
			IUserSecurityStampStore<TUser, TKey> userSecurityStampStore = Store as IUserSecurityStampStore<TUser, TKey>;
			if (userSecurityStampStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserSecurityStampStore);
			}
			return userSecurityStampStore;
		}

		public virtual async Task<string> GetSecurityStampAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserSecurityStampStore<TUser, TKey> securityStore = GetSecurityStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await securityStore.GetSecurityStampAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> UpdateSecurityStampAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserSecurityStampStore<TUser, TKey> securityStore = GetSecurityStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await securityStore.SetSecurityStampAsync(tUser, NewSecurityStamp()).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual Task<string> GeneratePasswordResetTokenAsync(TKey userId)
		{
			ThrowIfDisposed();
			return GenerateUserTokenAsync("ResetPassword", userId);
		}

		public virtual async Task<IdentityResult> ResetPasswordAsync(TKey userId, string token, string newPassword)
		{
			ThrowIfDisposed();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IdentityResult result;
			if (!(await VerifyUserTokenAsync(userId, "ResetPassword", token).WithCurrentCulture()))
			{
				result = IdentityResult.Failed(Resources.InvalidToken);
			}
			else
			{
				IUserPasswordStore<TUser, TKey> passwordStore = GetPasswordStore();
				IdentityResult identityResult = await UpdatePassword(passwordStore, tUser, newPassword).WithCurrentCulture();
				if (!identityResult.Succeeded)
				{
					result = identityResult;
				}
				else
				{
					result = await UpdateAsync(tUser).WithCurrentCulture();
				}
			}
			return result;
		}

		internal async Task UpdateSecurityStampInternal(TUser user)
		{
			if (SupportsUserSecurityStamp)
			{
				await GetSecurityStore().SetSecurityStampAsync(user, NewSecurityStamp()).WithCurrentCulture();
			}
		}

		private static string NewSecurityStamp()
		{
			return Guid.NewGuid().ToString();
		}

		private IUserLoginStore<TUser, TKey> GetLoginStore()
		{
			IUserLoginStore<TUser, TKey> userLoginStore = Store as IUserLoginStore<TUser, TKey>;
			if (userLoginStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserLoginStore);
			}
			return userLoginStore;
		}

		public virtual Task<TUser> FindAsync(UserLoginInfo login)
		{
			ThrowIfDisposed();
			return GetLoginStore().FindAsync(login);
		}

		public virtual async Task<IdentityResult> RemoveLoginAsync(TKey userId, UserLoginInfo login)
		{
			ThrowIfDisposed();
			IUserLoginStore<TUser, TKey> loginStore = GetLoginStore();
			if (login == null)
			{
				throw new ArgumentNullException(nameof(login));
			}
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await loginStore.RemoveLoginAsync(tUser, login).WithCurrentCulture();
			await UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> AddLoginAsync(TKey userId, UserLoginInfo login)
		{
			ThrowIfDisposed();
			IUserLoginStore<TUser, TKey> loginStore = GetLoginStore();
			if (login == null)
			{
				throw new ArgumentNullException(nameof(login));
			}
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			TUser tUser2 = await FindAsync(login).WithCurrentCulture();
			IdentityResult result;
			if (tUser2 != null)
			{
				result = IdentityResult.Failed(Resources.ExternalLoginExists);
			}
			else
			{
				await loginStore.AddLoginAsync(tUser, login).WithCurrentCulture();
				result = await UpdateAsync(tUser).WithCurrentCulture();
			}
			return result;
		}

		public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserLoginStore<TUser, TKey> loginStore = GetLoginStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await loginStore.GetLoginsAsync(tUser).WithCurrentCulture();
		}

		private IUserClaimStore<TUser, TKey> GetClaimStore()
		{
			IUserClaimStore<TUser, TKey> userClaimStore = Store as IUserClaimStore<TUser, TKey>;
			if (userClaimStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserClaimStore);
			}
			return userClaimStore;
		}

		public virtual async Task<IdentityResult> AddClaimAsync(TKey userId, Claim claim)
		{
			ThrowIfDisposed();
			IUserClaimStore<TUser, TKey> claimStore = GetClaimStore();
			if (claim == null)
			{
				throw new ArgumentNullException(nameof(claim));
			}
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await claimStore.AddClaimAsync(tUser, claim).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> RemoveClaimAsync(TKey userId, Claim claim)
		{
			ThrowIfDisposed();
			IUserClaimStore<TUser, TKey> claimStore = GetClaimStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await claimStore.RemoveClaimAsync(tUser, claim).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IList<Claim>> GetClaimsAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserClaimStore<TUser, TKey> claimStore = GetClaimStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await claimStore.GetClaimsAsync(tUser).WithCurrentCulture();
		}

		private IUserRoleStore<TUser, TKey> GetUserRoleStore()
		{
			IUserRoleStore<TUser, TKey> userRoleStore = Store as IUserRoleStore<TUser, TKey>;
			if (userRoleStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserRoleStore);
			}
			return userRoleStore;
		}

		public virtual async Task<IdentityResult> AddToRoleAsync(TKey userId, string role)
		{
			ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = GetUserRoleStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IList<string> list = await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture();
			IdentityResult result;
			if (list.Contains(role))
			{
				result = new IdentityResult(Resources.UserAlreadyInRole);
			}
			else
			{
				await userRoleStore.AddToRoleAsync(tUser, role).WithCurrentCulture();
				result = await UpdateAsync(tUser).WithCurrentCulture();
			}
			return result;
		}


        public virtual async Task<IdentityResult> AddToRolesAsync(TKey userId, params string[] roles)
        {
            ThrowIfDisposed();
            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var newRoles = roles.ToList();

            IUserRoleStore<TUser, TKey> userRoleStore = GetUserRoleStore();
            TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
            if (tUser == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
            }
            IList<string> list = await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture();
            IdentityResult result = null;

            if (newRoles.Any(q => !list.Contains(q)))
            {
                foreach (var role in newRoles.Where(q => !list.Contains(q)))
                {
                    await userRoleStore.AddToRoleAsync(tUser, role).WithCurrentCulture();
                    result = await UpdateAsync(tUser).WithCurrentCulture();
                }
            }
            else
            {
                result = new IdentityResult(Resources.UserAlreadyInRole);
            }

            
            
            return result;
        }


        public virtual async Task<IdentityResult> RemoveFromRolesAsync(TKey userId, params string[] roles)
        {
            ThrowIfDisposed();
            IUserRoleStore<TUser, TKey> userRoleStore = GetUserRoleStore();
            TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
            if (tUser == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
            }

            if (roles == null)
            {
                throw new ArgumentNullException(nameof(roles));
            }

            var newRoles = roles.ToList();

            IList<string> list = await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture();

            IdentityResult result = null;

            if (newRoles.Any(q => list.Contains(q)))
            {
                foreach (var role in newRoles.Where(q => list.Contains(q)))
                {
                    await userRoleStore.RemoveFromRoleAsync(tUser, role).WithCurrentCulture();
                    result = await UpdateAsync(tUser).WithCurrentCulture();
                }
            }
            else
            {
                result = new IdentityResult(Resources.UserNotInRole);
            }

            return result;
        }

        public virtual async Task<IdentityResult> RemoveFromRoleAsync(TKey userId, string role)
		{
			ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = GetUserRoleStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IdentityResult result;
			if (!(await userRoleStore.IsInRoleAsync(tUser, role).WithCurrentCulture()))
			{
				result = new IdentityResult(Resources.UserNotInRole);
			}
			else
			{
				await userRoleStore.RemoveFromRoleAsync(tUser, role).WithCurrentCulture();
				result = await UpdateAsync(tUser).WithCurrentCulture();
			}
			return result;
		}

		public virtual async Task<IList<string>> GetRolesAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = GetUserRoleStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await userRoleStore.GetRolesAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<bool> IsInRoleAsync(TKey userId, string role)
		{
			ThrowIfDisposed();
			IUserRoleStore<TUser, TKey> userRoleStore = GetUserRoleStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await userRoleStore.IsInRoleAsync(tUser, role).WithCurrentCulture();
		}

		internal IUserEmailStore<TUser, TKey> GetEmailStore()
		{
			IUserEmailStore<TUser, TKey> userEmailStore = Store as IUserEmailStore<TUser, TKey>;
			if (userEmailStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserEmailStore);
			}
			return userEmailStore;
		}

		public virtual async Task<string> GetEmailAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = GetEmailStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await emailStore.GetEmailAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> SetEmailAsync(TKey userId, string email)
		{
			ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = GetEmailStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await emailStore.SetEmailAsync(tUser, email).WithCurrentCulture();
			await emailStore.SetEmailConfirmedAsync(tUser, false).WithCurrentCulture();
			await UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual Task<TUser> FindByEmailAsync(string email)
		{
			ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = GetEmailStore();
			if (email == null)
			{
				throw new ArgumentNullException(nameof(email));
			}
			return emailStore.FindByEmailAsync(email);
		}

		public virtual Task<string> GenerateEmailConfirmationTokenAsync(TKey userId)
		{
			ThrowIfDisposed();
			return GenerateUserTokenAsync("Confirmation", userId);
		}

		public virtual async Task<IdentityResult> ConfirmEmailAsync(TKey userId, string token)
		{
			ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = GetEmailStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IdentityResult result;
			if (!(await VerifyUserTokenAsync(userId, "Confirmation", token).WithCurrentCulture()))
			{
				result = IdentityResult.Failed(Resources.InvalidToken);
			}
			else
			{
				await emailStore.SetEmailConfirmedAsync(tUser, true).WithCurrentCulture();
				result = await UpdateAsync(tUser).WithCurrentCulture();
			}
			return result;
		}

		public virtual async Task<bool> IsEmailConfirmedAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserEmailStore<TUser, TKey> emailStore = GetEmailStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await emailStore.GetEmailConfirmedAsync(tUser).WithCurrentCulture();
		}

		internal IUserPhoneNumberStore<TUser, TKey> GetPhoneNumberStore()
		{
			IUserPhoneNumberStore<TUser, TKey> userPhoneNumberStore = Store as IUserPhoneNumberStore<TUser, TKey>;
			if (userPhoneNumberStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserPhoneNumberStore);
			}
			return userPhoneNumberStore;
		}

		public virtual async Task<string> GetPhoneNumberAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = GetPhoneNumberStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await phoneNumberStore.GetPhoneNumberAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> SetPhoneNumberAsync(TKey userId, string phoneNumber)
		{
			ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = GetPhoneNumberStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await phoneNumberStore.SetPhoneNumberAsync(tUser, phoneNumber).WithCurrentCulture();
			await phoneNumberStore.SetPhoneNumberConfirmedAsync(tUser, false).WithCurrentCulture();
			await UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> ChangePhoneNumberAsync(TKey userId, string phoneNumber, string token)
		{
			ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = GetPhoneNumberStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IdentityResult result;
			if (await VerifyChangePhoneNumberTokenAsync(userId, token, phoneNumber).WithCurrentCulture())
			{
				await phoneNumberStore.SetPhoneNumberAsync(tUser, phoneNumber).WithCurrentCulture();
				await phoneNumberStore.SetPhoneNumberConfirmedAsync(tUser, true).WithCurrentCulture();
				await UpdateSecurityStampInternal(tUser).WithCurrentCulture();
				result = await UpdateAsync(tUser).WithCurrentCulture();
			}
			else
			{
				result = IdentityResult.Failed(Resources.InvalidToken);
			}
			return result;
		}

		public virtual async Task<bool> IsPhoneNumberConfirmedAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserPhoneNumberStore<TUser, TKey> phoneNumberStore = GetPhoneNumberStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await phoneNumberStore.GetPhoneNumberConfirmedAsync(tUser).WithCurrentCulture();
		}

		internal async Task<SecurityToken> CreateSecurityTokenAsync(TKey userId)
		{
			return new SecurityToken(Encoding.Unicode.GetBytes(await GetSecurityStampAsync(userId).WithCurrentCulture()));
		}

		public virtual async Task<string> GenerateChangePhoneNumberTokenAsync(TKey userId, string phoneNumber)
		{
			ThrowIfDisposed();
			return Rfc6238AuthenticationService.GenerateCode(await CreateSecurityTokenAsync(userId).WithCurrentCulture(), phoneNumber).ToString("D6", CultureInfo.InvariantCulture);
		}

		public virtual async Task<bool> VerifyChangePhoneNumberTokenAsync(TKey userId, string token, string phoneNumber)
		{
			ThrowIfDisposed();
			SecurityToken securityToken = await CreateSecurityTokenAsync(userId).WithCurrentCulture();
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
			ThrowIfDisposed();
			if (UserTokenProvider == null)
			{
				throw new NotSupportedException(Resources.NoTokenProvider);
			}
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await UserTokenProvider.ValidateAsync(purpose, token, this, tUser).WithCurrentCulture();
		}

		public virtual async Task<string> GenerateUserTokenAsync(string purpose, TKey userId)
		{
			ThrowIfDisposed();
			if (UserTokenProvider == null)
			{
				throw new NotSupportedException(Resources.NoTokenProvider);
			}
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await UserTokenProvider.GenerateAsync(purpose, this, tUser).WithCurrentCulture();
		}

		public virtual void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<TUser, TKey> provider)
		{
			ThrowIfDisposed();
			if (twoFactorProvider == null)
			{
				throw new ArgumentNullException(nameof(twoFactorProvider));
			}
			if (provider == null)
			{
				throw new ArgumentNullException(nameof(provider));
			}
			TwoFactorProviders[twoFactorProvider] = provider;
		}

		public virtual async Task<IList<string>> GetValidTwoFactorProvidersAsync(TKey userId)
		{
			ThrowIfDisposed();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			List<string> list = new List<string>();
			foreach (KeyValuePair<string, IUserTokenProvider<TUser, TKey>> current in TwoFactorProviders)
			{
				if (await current.Value.IsValidProviderForUserAsync(this, tUser).WithCurrentCulture())
				{
                    list.Add(current.Key);
				}
			}
			return list;
		}

		public virtual async Task<bool> VerifyTwoFactorTokenAsync(TKey userId, string twoFactorProvider, string token)
		{
			ThrowIfDisposed();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			if (!_factors.ContainsKey(twoFactorProvider))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.NoTwoFactorProvider, twoFactorProvider));
			}
			IUserTokenProvider<TUser, TKey> userTokenProvider = _factors[twoFactorProvider];
			return await userTokenProvider.ValidateAsync(twoFactorProvider, token, this, tUser).WithCurrentCulture();
		}

		public virtual async Task<string> GenerateTwoFactorTokenAsync(TKey userId, string twoFactorProvider)
		{
			ThrowIfDisposed();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			if (!_factors.ContainsKey(twoFactorProvider))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.NoTwoFactorProvider, twoFactorProvider));
			}
			return await _factors[twoFactorProvider].GenerateAsync(twoFactorProvider, this, tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> NotifyTwoFactorTokenAsync(TKey userId, string twoFactorProvider, string token)
		{
			ThrowIfDisposed();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			if (!_factors.ContainsKey(twoFactorProvider))
			{
				throw new NotSupportedException(string.Format(CultureInfo.CurrentCulture, Resources.NoTwoFactorProvider, twoFactorProvider));
			}
			await _factors[twoFactorProvider].NotifyAsync(token, this, tUser).WithCurrentCulture();
			return IdentityResult.Success;
		}

		internal IUserTwoFactorStore<TUser, TKey> GetUserTwoFactorStore()
		{
			IUserTwoFactorStore<TUser, TKey> userTwoFactorStore = Store as IUserTwoFactorStore<TUser, TKey>;
			if (userTwoFactorStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserTwoFactorStore);
			}
			return userTwoFactorStore;
		}

		public virtual async Task<bool> GetTwoFactorEnabledAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserTwoFactorStore<TUser, TKey> userTwoFactorStore = GetUserTwoFactorStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await userTwoFactorStore.GetTwoFactorEnabledAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> SetTwoFactorEnabledAsync(TKey userId, bool enabled)
		{
			ThrowIfDisposed();
			IUserTwoFactorStore<TUser, TKey> userTwoFactorStore = GetUserTwoFactorStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await userTwoFactorStore.SetTwoFactorEnabledAsync(tUser, enabled).WithCurrentCulture();
			await UpdateSecurityStampInternal(tUser).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task SendEmailAsync(TKey userId, string subject, string body)
		{
			ThrowIfDisposed();
			if (EmailService != null)
			{
				IdentityMessage message = new IdentityMessage
				{
					Destination = await GetEmailAsync(userId).WithCurrentCulture(),
					Subject = subject,
					Body = body
				};
				await EmailService.SendAsync(message).WithCurrentCulture();
			}
		}

		public virtual async Task SendSmsAsync(TKey userId, string message)
		{
			ThrowIfDisposed();
			if (SmsService != null)
			{
				IdentityMessage message2 = new IdentityMessage
				{
					Destination = await GetPhoneNumberAsync(userId).WithCurrentCulture(),
					Body = message
				};
				await SmsService.SendAsync(message2).WithCurrentCulture();
			}
		}

		internal IUserLockoutStore<TUser, TKey> GetUserLockoutStore()
		{
			IUserLockoutStore<TUser, TKey> userLockoutStore = Store as IUserLockoutStore<TUser, TKey>;
			if (userLockoutStore == null)
			{
				throw new NotSupportedException(Resources.StoreNotIUserLockoutStore);
			}
			return userLockoutStore;
		}

		public virtual async Task<bool> IsLockedOutAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			bool result;
			if (!(await userLockoutStore.GetLockoutEnabledAsync(tUser).WithCurrentCulture()))
			{
				result = false;
			}
			else
			{
				DateTimeOffset left = await userLockoutStore.GetLockoutEndDateAsync(tUser).WithCurrentCulture();
				result = (left >= DateTimeOffset.UtcNow);
			}
			return result;
		}

		public virtual async Task<IdentityResult> SetLockoutEnabledAsync(TKey userId, bool enabled)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			await userLockoutStore.SetLockoutEnabledAsync(tUser, enabled).WithCurrentCulture();
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<bool> GetLockoutEnabledAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await userLockoutStore.GetLockoutEnabledAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<DateTimeOffset> GetLockoutEndDateAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await userLockoutStore.GetLockoutEndDateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> SetLockoutEndDateAsync(TKey userId, DateTimeOffset lockoutEnd)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IdentityResult result;
			if (!(await userLockoutStore.GetLockoutEnabledAsync(tUser).WithCurrentCulture()))
			{
				result = IdentityResult.Failed(Resources.LockoutNotEnabled);
			}
			else
			{
				await userLockoutStore.SetLockoutEndDateAsync(tUser, lockoutEnd).WithCurrentCulture();
				result = await UpdateAsync(tUser).WithCurrentCulture();
			}
			return result;
		}

		public virtual async Task<IdentityResult> AccessFailedAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			int num = await userLockoutStore.IncrementAccessFailedCountAsync(tUser).WithCurrentCulture();
			if (num >= MaxFailedAccessAttemptsBeforeLockout)
			{
				await userLockoutStore.SetLockoutEndDateAsync(tUser, DateTimeOffset.UtcNow.Add(DefaultAccountLockoutTimeSpan)).WithCurrentCulture();
				await userLockoutStore.ResetAccessFailedCountAsync(tUser).WithCurrentCulture();
			}
			return await UpdateAsync(tUser).WithCurrentCulture();
		}

		public virtual async Task<IdentityResult> ResetAccessFailedCountAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			IdentityResult result;
			if ((await GetAccessFailedCountAsync(tUser.Id).WithCurrentCulture()) != 0)
			{
				result = IdentityResult.Success;
			}
			else
			{
				await userLockoutStore.ResetAccessFailedCountAsync(tUser).WithCurrentCulture();
				result = await UpdateAsync(tUser).WithCurrentCulture();
			}
			return result;
		}

		public virtual async Task<int> GetAccessFailedCountAsync(TKey userId)
		{
			ThrowIfDisposed();
			IUserLockoutStore<TUser, TKey> userLockoutStore = GetUserLockoutStore();
			TUser tUser = await FindByIdAsync(userId).WithCurrentCulture();
			if (tUser == null)
			{
				throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, Resources.UserIdNotFound, userId));
			}
			return await userLockoutStore.GetAccessFailedCountAsync(tUser).WithCurrentCulture();
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
				_disposed = true;
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
