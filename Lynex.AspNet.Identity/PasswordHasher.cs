namespace Lynex.AspNet.Identity
{
	public class PasswordHasher : IPasswordHasher
	{
		public virtual string HashPassword(string password)
		{
			return Crypto.HashPassword(password);
		}

		public virtual PasswordVerificationResult VerifyHashedPassword(string hashedPassword, string providedPassword)
		{
			if (Crypto.VerifyHashedPassword(hashedPassword, providedPassword))
			{
				return PasswordVerificationResult.Success;
			}
			return PasswordVerificationResult.Failed;
		}
	}
}
