using System.Collections.Generic;

namespace Lynex.AspNet.Identity
{
	public class IdentityResult
	{
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



		public static IdentityResult Success { get; } = new IdentityResult(true);

	    public IdentityResult(params string[] errors) : this((IEnumerable<string>)errors)
		{
		}

		public IdentityResult(IEnumerable<string> errors)
		{
			if (errors == null)
			{
				errors = new[]
				{
					Resources.DefaultError
				};
			}
			Succeeded = false;
			Errors = errors;
		}

		protected IdentityResult(bool success)
		{
			Succeeded = success;
			Errors = new string[0];
		}

		public static IdentityResult Failed(params string[] errors)
		{
			return new IdentityResult(errors);
		}
	}
}
