using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Lynex.AspNet.Identity
{
	public static class UserManagerExtensions
	{
		public static ClaimsIdentity CreateIdentity<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user, string authenticationType) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.CreateIdentityAsync(user, authenticationType));
		}

		public static TUser FindById<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.FindByIdAsync(userId));
		}

		public static TUser Find<TUser, TKey>(this UserManager<TUser, TKey> manager, string userName, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.FindAsync(userName, password));
		}

		public static TUser FindByName<TUser, TKey>(this UserManager<TUser, TKey> manager, string userName) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.FindByNameAsync(userName));
		}

		public static TUser FindByEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, string email) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.FindByEmailAsync(email));
		}

		public static IdentityResult Create<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.CreateAsync(user));
		}

		public static IdentityResult Create<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.CreateAsync(user, password));
		}

		public static IdentityResult Update<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.UpdateAsync(user));
		}

		public static IdentityResult Delete<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.DeleteAsync(user));
		}

		public static bool HasPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.HasPasswordAsync(userId));
		}

		public static IdentityResult AddPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.AddPasswordAsync(userId, password));
		}

		public static IdentityResult ChangePassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string currentPassword, string newPassword) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.ChangePasswordAsync(userId, currentPassword, newPassword));
		}

		public static IdentityResult ResetPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string token, string newPassword) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.ResetPasswordAsync(userId, token, newPassword));
		}

		public static string GeneratePasswordResetToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GeneratePasswordResetTokenAsync(userId));
		}

		public static string GetSecurityStamp<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetSecurityStampAsync(userId));
		}

		public static string GenerateEmailConfirmationToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GenerateEmailConfirmationTokenAsync(userId));
		}

		public static IdentityResult ConfirmEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.ConfirmEmailAsync(userId, token));
		}

		public static bool IsEmailConfirmed<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.IsEmailConfirmedAsync(userId));
		}

		public static IdentityResult UpdateSecurityStamp<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.UpdateSecurityStampAsync(userId));
		}

		public static bool CheckPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.CheckPasswordAsync(user, password));
		}

		public static IdentityResult RemovePassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.RemovePasswordAsync(userId));
		}

		public static IdentityResult AddLogin<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, UserLoginInfo login) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.AddLoginAsync(userId, login));
		}

		public static IdentityResult RemoveLogin<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, UserLoginInfo login) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.RemoveLoginAsync(userId, login));
		}

		public static IList<UserLoginInfo> GetLogins<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetLoginsAsync(userId));
		}

		public static TUser Find<TUser, TKey>(this UserManager<TUser, TKey> manager, UserLoginInfo login) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.FindAsync(login));
		}

		public static IdentityResult AddClaim<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, Claim claim) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.AddClaimAsync(userId, claim));
		}

		public static IdentityResult RemoveClaim<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, Claim claim) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.RemoveClaimAsync(userId, claim));
		}

		public static IList<Claim> GetClaims<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetClaimsAsync(userId));
		}

		public static IdentityResult AddToRole<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string role) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.AddToRoleAsync(userId, role));
		}

		public static IdentityResult AddToRoles<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, params string[] roles) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.AddToRolesAsync(userId, roles));
		}

		public static IdentityResult RemoveFromRole<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string role) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.RemoveFromRoleAsync(userId, role));
		}

		public static IdentityResult RemoveFromRoles<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, params string[] roles) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.RemoveFromRolesAsync(userId, roles));
		}

		public static IList<string> GetRoles<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetRolesAsync(userId));
		}

		public static bool IsInRole<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string role) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.IsInRoleAsync(userId, role));
		}

		public static string GetEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetEmailAsync(userId));
		}

		public static IdentityResult SetEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string email) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.SetEmailAsync(userId, email));
		}

		public static string GetPhoneNumber<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetPhoneNumberAsync(userId));
		}

		public static IdentityResult SetPhoneNumber<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string phoneNumber) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.SetPhoneNumberAsync(userId, phoneNumber));
		}

		public static IdentityResult ChangePhoneNumber<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string phoneNumber, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.ChangePhoneNumberAsync(userId, phoneNumber, token));
		}

		public static string GenerateChangePhoneNumberToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string phoneNumber) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber));
		}

		public static bool VerifyChangePhoneNumberToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string token, string phoneNumber) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.VerifyChangePhoneNumberTokenAsync(userId, token, phoneNumber));
		}

		public static bool IsPhoneNumberConfirmed<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.IsPhoneNumberConfirmedAsync(userId));
		}

		public static string GenerateTwoFactorToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string providerId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GenerateTwoFactorTokenAsync(userId, providerId));
		}

		public static bool VerifyTwoFactorToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string providerId, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.VerifyTwoFactorTokenAsync(userId, providerId, token));
		}

		public static IList<string> GetValidTwoFactorProviders<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetValidTwoFactorProvidersAsync(userId));
		}

		public static string GenerateUserToken<TUser, TKey>(this UserManager<TUser, TKey> manager, string purpose, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("mana" + "ger");
			}
			return AsyncHelper.RunSync(() => manager.GenerateUserTokenAsync(purpose, userId));
		}

		public static bool VerifyUserToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string purpose, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.VerifyUserTokenAsync(userId, purpose, token));
		}

		public static IdentityResult NotifyTwoFactorToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string twoFactorProvider, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.NotifyTwoFactorTokenAsync(userId, twoFactorProvider, token));
		}

		public static bool GetTwoFactorEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetTwoFactorEnabledAsync(userId));
		}

		public static IdentityResult SetTwoFactorEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, bool enabled) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.SetTwoFactorEnabledAsync(userId, enabled));
		}

		public static void SendEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string subject, string body) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			AsyncHelper.RunSync(() => manager.SendEmailAsync(userId, subject, body));
		}

		public static void SendSms<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string message) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			AsyncHelper.RunSync(() => manager.SendSmsAsync(userId, message));
		}

		public static bool IsLockedOut<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.IsLockedOutAsync(userId));
		}

		public static IdentityResult SetLockoutEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, bool enabled) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.SetLockoutEnabledAsync(userId, enabled));
		}

		public static bool GetLockoutEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetLockoutEnabledAsync(userId));
		}

		public static DateTimeOffset GetLockoutEndDate<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetLockoutEndDateAsync(userId));
		}

		public static IdentityResult SetLockoutEndDate<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, DateTimeOffset lockoutEnd) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.SetLockoutEndDateAsync(userId, lockoutEnd));
		}

		public static IdentityResult AccessFailed<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.AccessFailedAsync(userId));
		}

		public static IdentityResult ResetAccessFailedCount<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.ResetAccessFailedCountAsync(userId));
		}

		public static int GetAccessFailedCount<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.GetAccessFailedCountAsync(userId));
		}
	}
}
