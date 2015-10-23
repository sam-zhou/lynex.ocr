using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class PhoneNumberTokenProvider<TUser, TKey> : TotpSecurityStampBasedTokenProvider<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		private string _body;

		public string MessageFormat
		{
			get
			{
				return _body ?? "{0}";
			}
			set
			{
				_body = value;
			}
		}

		public override async Task<bool> IsValidProviderForUserAsync(UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			string value = await manager.GetPhoneNumberAsync(user.Id).WithCurrentCulture();
			return !string.IsNullOrWhiteSpace(value) && await manager.IsPhoneNumberConfirmedAsync(user.Id).WithCurrentCulture();
		}

		public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			string str = await manager.GetPhoneNumberAsync(user.Id).WithCurrentCulture();
			return "PhoneNumber:" + purpose + ":" + str;
		}

		public override Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return manager.SendSmsAsync(user.Id, string.Format(CultureInfo.CurrentCulture, MessageFormat, token));
		}
	}
	public class PhoneNumberTokenProvider<TUser> : PhoneNumberTokenProvider<TUser, string> where TUser : class, IUser<string>
	{
	}
}
