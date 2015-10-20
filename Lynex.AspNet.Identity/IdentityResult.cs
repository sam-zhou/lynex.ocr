using System.Collections.Generic;

namespace Lynex.AspNet.Identity
{
	public class IdentityResult
	{
		private static readonly IdentityResult _success = new IdentityResult(true);

		public bool Succeeded
		{
			get;
			private set;
		}

		public IEnumerable<string> Errors
		{
			get;
			private set;
		}

		public static IdentityResult Success
		{
			get
			{
				return IdentityResult._success;
			}
		}

		public IdentityResult(params string[] errors) : this((IEnumerable<string>)errors)
		{
		}

		public IdentityResult(IEnumerable<string> errors)
		{
			if (errors == null)
			{
				errors = new string[]
				{
					Resources.DefaultError
				};
			}
			this.Succeeded = false;
			this.Errors = errors;
		}

		protected IdentityResult(bool success)
		{
			this.Succeeded = success;
			this.Errors = new string[0];
		}

		public static IdentityResult Failed(params string[] errors)
		{
			return new IdentityResult(errors);
		}
	}
}
