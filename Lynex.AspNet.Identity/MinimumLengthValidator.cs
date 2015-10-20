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
			this.RequiredLength = requiredLength;
		}

		public virtual Task<IdentityResult> ValidateAsync(string item)
		{
			if (string.IsNullOrWhiteSpace(item) || item.Length < this.RequiredLength)
			{
				return Task.FromResult<IdentityResult>(IdentityResult.Failed(new string[]
				{
					string.Format(CultureInfo.CurrentCulture, Resources.PasswordTooShort, new object[]
					{
						this.RequiredLength
					})
				}));
			}
			return Task.FromResult<IdentityResult>(IdentityResult.Success);
		}
	}
}
