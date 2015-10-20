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
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<ClaimsIdentity>(() => manager.CreateIdentityAsync(user, authenticationType));
		}

		public static TUser FindById<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<TUser>(() => manager.FindByIdAsync(userId));
		}

		public static TUser Find<TUser, TKey>(this UserManager<TUser, TKey> manager, string userName, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<TUser>(() => manager.FindAsync(userName, password));
		}

		public static TUser FindByName<TUser, TKey>(this UserManager<TUser, TKey> manager, string userName) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<TUser>(() => manager.FindByNameAsync(userName));
		}

		public static TUser FindByEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, string email) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<TUser>(() => manager.FindByEmailAsync(email));
		}

		public static IdentityResult Create<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.CreateAsync(user));
		}

		public static IdentityResult Create<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.CreateAsync(user, password));
		}

		public static IdentityResult Update<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.UpdateAsync(user));
		}

		public static IdentityResult Delete<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.DeleteAsync(user));
		}

		public static bool HasPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.HasPasswordAsync(userId));
		}

		public static IdentityResult AddPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.AddPasswordAsync(userId, password));
		}

		public static IdentityResult ChangePassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string currentPassword, string newPassword) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.ChangePasswordAsync(userId, currentPassword, newPassword));
		}

		public static IdentityResult ResetPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string token, string newPassword) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.ResetPasswordAsync(userId, token, newPassword));
		}

		public static string GeneratePasswordResetToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GeneratePasswordResetTokenAsync(userId));
		}

		public static string GetSecurityStamp<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GetSecurityStampAsync(userId));
		}

		public static string GenerateEmailConfirmationToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GenerateEmailConfirmationTokenAsync(userId));
		}

		public static IdentityResult ConfirmEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.ConfirmEmailAsync(userId, token));
		}

		public static bool IsEmailConfirmed<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.IsEmailConfirmedAsync(userId));
		}

		public static IdentityResult UpdateSecurityStamp<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.UpdateSecurityStampAsync(userId));
		}

		public static bool CheckPassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TUser user, string password) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.CheckPasswordAsync(user, password));
		}

		public static IdentityResult RemovePassword<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.RemovePasswordAsync(userId));
		}

		public static IdentityResult AddLogin<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, UserLoginInfo login) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.AddLoginAsync(userId, login));
		}

		public static IdentityResult RemoveLogin<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, UserLoginInfo login) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.RemoveLoginAsync(userId, login));
		}

		public static IList<UserLoginInfo> GetLogins<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IList<UserLoginInfo>>(() => manager.GetLoginsAsync(userId));
		}

		public static TUser Find<TUser, TKey>(this UserManager<TUser, TKey> manager, UserLoginInfo login) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<TUser>(() => manager.FindAsync(login));
		}

		public static IdentityResult AddClaim<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, Claim claim) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.AddClaimAsync(userId, claim));
		}

		public static IdentityResult RemoveClaim<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, Claim claim) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.RemoveClaimAsync(userId, claim));
		}

		public static IList<Claim> GetClaims<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IList<Claim>>(() => manager.GetClaimsAsync(userId));
		}

		public static IdentityResult AddToRole<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string role) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.AddToRoleAsync(userId, role));
		}

		public static IdentityResult AddToRoles<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, params string[] roles) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.AddToRolesAsync(userId, roles));
		}

		public static IdentityResult RemoveFromRole<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string role) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.RemoveFromRoleAsync(userId, role));
		}

		public static IdentityResult RemoveFromRoles<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, params string[] roles) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.RemoveFromRolesAsync(userId, roles));
		}

		public static IList<string> GetRoles<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IList<string>>(() => manager.GetRolesAsync(userId));
		}

		public static bool IsInRole<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string role) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.IsInRoleAsync(userId, role));
		}

		public static string GetEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GetEmailAsync(userId));
		}

		public static IdentityResult SetEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string email) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.SetEmailAsync(userId, email));
		}

		public static string GetPhoneNumber<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GetPhoneNumberAsync(userId));
		}

		public static IdentityResult SetPhoneNumber<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string phoneNumber) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.SetPhoneNumberAsync(userId, phoneNumber));
		}

		public static IdentityResult ChangePhoneNumber<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string phoneNumber, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.ChangePhoneNumberAsync(userId, phoneNumber, token));
		}

		public static string GenerateChangePhoneNumberToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string phoneNumber) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GenerateChangePhoneNumberTokenAsync(userId, phoneNumber));
		}

		public static bool VerifyChangePhoneNumberToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string token, string phoneNumber) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.VerifyChangePhoneNumberTokenAsync(userId, token, phoneNumber));
		}

		public static bool IsPhoneNumberConfirmed<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.IsPhoneNumberConfirmedAsync(userId));
		}

		public static string GenerateTwoFactorToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string providerId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GenerateTwoFactorTokenAsync(userId, providerId));
		}

		public static bool VerifyTwoFactorToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string providerId, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.VerifyTwoFactorTokenAsync(userId, providerId, token));
		}

		public static IList<string> GetValidTwoFactorProviders<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IList<string>>(() => manager.GetValidTwoFactorProvidersAsync(userId));
		}

		public static string GenerateUserToken<TUser, TKey>(this UserManager<TUser, TKey> manager, string purpose, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<string>(() => manager.GenerateUserTokenAsync(purpose, userId));
		}

		public static bool VerifyUserToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string purpose, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.VerifyUserTokenAsync(userId, purpose, token));
		}

		public static IdentityResult NotifyTwoFactorToken<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string twoFactorProvider, string token) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.NotifyTwoFactorTokenAsync(userId, twoFactorProvider, token));
		}

		public static bool GetTwoFactorEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.GetTwoFactorEnabledAsync(userId));
		}

		public static IdentityResult SetTwoFactorEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, bool enabled) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.SetTwoFactorEnabledAsync(userId, enabled));
		}

		public static void SendEmail<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string subject, string body) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			AsyncHelper.RunSync(() => manager.SendEmailAsync(userId, subject, body));
		}

		public static void SendSms<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, string message) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			AsyncHelper.RunSync(() => manager.SendSmsAsync(userId, message));
		}

		public static bool IsLockedOut<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.IsLockedOutAsync(userId));
		}

		public static IdentityResult SetLockoutEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, bool enabled) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.SetLockoutEnabledAsync(userId, enabled));
		}

		public static bool GetLockoutEnabled<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.GetLockoutEnabledAsync(userId));
		}

		public static DateTimeOffset GetLockoutEndDate<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<DateTimeOffset>(() => manager.GetLockoutEndDateAsync(userId));
		}

		public static IdentityResult SetLockoutEndDate<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId, DateTimeOffset lockoutEnd) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.SetLockoutEndDateAsync(userId, lockoutEnd));
		}

		public static IdentityResult AccessFailed<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.AccessFailedAsync(userId));
		}

		public static IdentityResult ResetAccessFailedCount<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.ResetAccessFailedCountAsync(userId));
		}

		public static int GetAccessFailedCount<TUser, TKey>(this UserManager<TUser, TKey> manager, TKey userId) where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<int>(() => manager.GetAccessFailedCountAsync(userId));
		}
	}
}
