using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class PasswordValidator : IIdentityValidator<string>
	{
		public int RequiredLength
		{
			get;
			set;
		}

		public bool RequireNonLetterOrDigit
		{
			get;
			set;
		}

		public bool RequireLowercase
		{
			get;
			set;
		}

		public bool RequireUppercase
		{
			get;
			set;
		}

		public bool RequireDigit
		{
			get;
			set;
		}

		public virtual Task<IdentityResult> ValidateAsync(string item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}
			List<string> list = new List<string>();
			if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
			{
				list.Add(string.Format(CultureInfo.CurrentCulture, Resources.PasswordTooShort, RequiredLength));
			}
			if (RequireNonLetterOrDigit && item.All(IsLetterOrDigit))
			{
				list.Add(Resources.PasswordRequireNonLetterOrDigit);
			}
			if (RequireDigit && item.All(c => !IsDigit(c)))
			{
				list.Add(Resources.PasswordRequireDigit);
			}
			if (RequireLowercase && item.All(c => !IsLower(c)))
			{
				list.Add(Resources.PasswordRequireLower);
			}
			if (RequireUppercase && item.All(c => !IsUpper(c)))
			{
				list.Add(Resources.PasswordRequireUpper);
			}
			if (list.Count == 0)
			{
				return Task.FromResult(IdentityResult.Success);
			}
			return Task.FromResult(IdentityResult.Failed(string.Join(" ", list)));
		}

		public virtual bool IsDigit(char c)
		{
			return c >= '0' && c <= '9';
		}

		public virtual bool IsLower(char c)
		{
			return c >= 'a' && c <= 'z';
		}

		public virtual bool IsUpper(char c)
		{
			return c >= 'A' && c <= 'Z';
		}

		public virtual bool IsLetterOrDigit(char c)
		{
			return IsUpper(c) || IsLower(c) || IsDigit(c);
		}
	}
}
