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
				return this._body ?? "{0}";
			}
			set
			{
				this._body = value;
			}
		}

		public override async Task<bool> IsValidProviderForUserAsync(UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			string value = await manager.GetPhoneNumberAsync(user.Id).WithCurrentCulture<string>();
			return !string.IsNullOrWhiteSpace(value) && await manager.IsPhoneNumberConfirmedAsync(user.Id).WithCurrentCulture<bool>();
		}

		public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			string str = await manager.GetPhoneNumberAsync(user.Id).WithCurrentCulture<string>();
			return "PhoneNumber:" + purpose + ":" + str;
		}

		public override Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return manager.SendSmsAsync(user.Id, string.Format(CultureInfo.CurrentCulture, this.MessageFormat, new object[]
			{
				token
			}));
		}
	}
	public class PhoneNumberTokenProvider<TUser> : PhoneNumberTokenProvider<TUser, string> where TUser : class, IUser<string>
	{
	}
}
