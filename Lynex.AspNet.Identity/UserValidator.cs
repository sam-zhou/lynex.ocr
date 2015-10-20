using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class UserValidator<TUser, TKey> : IIdentityValidator<TUser> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		public bool AllowOnlyAlphanumericUserNames
		{
			get;
			set;
		}

		public bool RequireUniqueEmail
		{
			get;
			set;
		}

		private UserManager<TUser, TKey> Manager
		{
			get;
			set;
		}

		public UserValidator(UserManager<TUser, TKey> manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			this.AllowOnlyAlphanumericUserNames = true;
			this.Manager = manager;
		}

		public virtual async Task<IdentityResult> ValidateAsync(TUser item)
		{
			if (item == null)
			{
				throw new ArgumentNullException("item");
			}
			List<string> list = new List<string>();
			await this.ValidateUserName(item, list).WithCurrentCulture();
			if (this.RequireUniqueEmail)
			{
				await this.ValidateEmailAsync(item, list).WithCurrentCulture();
			}
			IdentityResult result;
			if (list.Count > 0)
			{
				result = IdentityResult.Failed(list.ToArray());
			}
			else
			{
				result = IdentityResult.Success;
			}
			return result;
		}

		private async Task ValidateUserName(TUser user, List<string> errors)
		{
			if (string.IsNullOrWhiteSpace(user.UserName))
			{
				errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, new object[]
				{
					"Name"
				}));
			}
			else if (this.AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, "^[A-Za-z0-9@_\\.]+$"))
			{
				errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.InvalidUserName, new object[]
				{
					user.UserName
				}));
			}
			else
			{
				TUser tUser = await this.Manager.FindByNameAsync(user.UserName).WithCurrentCulture<TUser>();
				if (tUser != null && !EqualityComparer<TKey>.Default.Equals(tUser.Id, user.Id))
				{
					errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateName, new object[]
					{
						user.UserName
					}));
				}
			}
		}

		private async Task ValidateEmailAsync(TUser user, List<string> errors)
		{
			string text = await this.Manager.GetEmailStore().GetEmailAsync(user).WithCurrentCulture<string>();
			if (string.IsNullOrWhiteSpace(text))
			{
				errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, new object[]
				{
					"Email"
				}));
			}
			else
			{
				try
				{
					new MailAddress(text);
				}
				catch (FormatException)
				{
					errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.InvalidEmail, new object[]
					{
						text
					}));
					return;
				}
				TUser tUser = await this.Manager.FindByEmailAsync(text).WithCurrentCulture<TUser>();
				if (tUser != null && !EqualityComparer<TKey>.Default.Equals(tUser.Id, user.Id))
				{
					errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateEmail, new object[]
					{
						text
					}));
				}
			}
		}
	}
	public class UserValidator<TUser> : UserValidator<TUser, string> where TUser : class, IUser<string>
	{
		public UserValidator(UserManager<TUser, string> manager) : base(manager)
		{
		}
	}
}
