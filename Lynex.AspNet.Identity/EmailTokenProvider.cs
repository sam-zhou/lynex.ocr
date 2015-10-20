using System;
using System.Globalization;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class EmailTokenProvider<TUser, TKey> : TotpSecurityStampBasedTokenProvider<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		private string _body;

		private string _subject;

		public string Subject
		{
			get
			{
				return this._subject ?? string.Empty;
			}
			set
			{
				this._subject = value;
			}
		}

		public string BodyFormat
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
			string value = await manager.GetEmailAsync(user.Id).WithCurrentCulture<string>();
			return !string.IsNullOrWhiteSpace(value) && await manager.IsEmailConfirmedAsync(user.Id).WithCurrentCulture<bool>();
		}

		public override async Task<string> GetUserModifierAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
		{
			string str = await manager.GetEmailAsync(user.Id).WithCurrentCulture<string>();
			return "Email:" + purpose + ":" + str;
		}

		public override Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return manager.SendEmailAsync(user.Id, this.Subject, string.Format(CultureInfo.CurrentCulture, this.BodyFormat, new object[]
			{
				token
			}));
		}
	}
	public class EmailTokenProvider<TUser> : EmailTokenProvider<TUser, string> where TUser : class, IUser<string>
	{
	}
}
