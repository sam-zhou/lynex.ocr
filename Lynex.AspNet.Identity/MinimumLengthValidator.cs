using System.Globalization;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class MinimumLengthValidator : IIdentityValidator<string>
	{
		public int RequiredLength
		{
			get;
			set;
		}

		public MinimumLengthValidator(int requiredLength)
		{
			RequiredLength = requiredLength;
		}

		public virtual Task<IdentityResult> ValidateAsync(string item)
		{
			if (string.IsNullOrWhiteSpace(item) || item.Length < RequiredLength)
			{
				return Task.FromResult(IdentityResult.Failed(string.Format(CultureInfo.CurrentCulture, Resources.PasswordTooShort, RequiredLength)));
			}
			return Task.FromResult(IdentityResult.Success);
		}
	}
}
