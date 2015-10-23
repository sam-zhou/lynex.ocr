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
				throw new ArgumentNullException(nameof(manager));
			}
			AllowOnlyAlphanumericUserNames = true;
			Manager = manager;
		}

		public virtual async Task<IdentityResult> ValidateAsync(TUser item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}
			List<string> list = new List<string>();
			await ValidateUserName(item, list).WithCurrentCulture();
			if (RequireUniqueEmail)
			{
				await ValidateEmailAsync(item, list).WithCurrentCulture();
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
				errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, "Name"));
			}
			else if (AllowOnlyAlphanumericUserNames && !Regex.IsMatch(user.UserName, "^[A-Za-z0-9@_\\.]+$"))
			{
				errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.InvalidUserName, user.UserName));
			}
			else
			{
				TUser tUser = await Manager.FindByNameAsync(user.UserName).WithCurrentCulture();
				if (tUser != null && !EqualityComparer<TKey>.Default.Equals(tUser.Id, user.Id))
				{
					errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateName, user.UserName));
				}
			}
		}

		private async Task ValidateEmailAsync(TUser user, List<string> errors)
		{
			string text = await Manager.GetEmailStore().GetEmailAsync(user).WithCurrentCulture();
			if (string.IsNullOrWhiteSpace(text))
			{
				errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, "Email"));
			}
			else
			{
				try
				{
                    new MailAddress(text);
				}
				catch (FormatException)
				{
					errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.InvalidEmail, text));
					return;
				}
				TUser tUser = await Manager.FindByEmailAsync(text).WithCurrentCulture();
				if (tUser != null && !EqualityComparer<TKey>.Default.Equals(tUser.Id, user.Id))
				{
					errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateEmail, text));
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
