using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace Lynex.AspNet.Identity
{
	internal static class Crypto
	{
		private const int Pbkdf2IterCount = 1000;

		private const int Pbkdf2SubkeyLength = 32;

		private const int SaltSize = 16;

		public static string HashPassword(string password)
		{
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}
			byte[] salt;
			byte[] bytes;
			using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, SaltSize, Pbkdf2IterCount))
			{
				salt = rfc2898DeriveBytes.Salt;
				bytes = rfc2898DeriveBytes.GetBytes(Pbkdf2SubkeyLength);
			}
			byte[] array = new byte[Pbkdf2SubkeyLength + SaltSize + 1];
			Buffer.BlockCopy(salt, 0, array, 1, SaltSize);
			Buffer.BlockCopy(bytes, 0, array, SaltSize + 1, Pbkdf2SubkeyLength);
			return Convert.ToBase64String(array);
		}

		public static bool VerifyHashedPassword(string hashedPassword, string password)
		{
			if (hashedPassword == null)
			{
				return false;
			}
			if (password == null)
			{
				throw new ArgumentNullException(nameof(password));
			}
			byte[] array = Convert.FromBase64String(hashedPassword);
			if (array.Length != Pbkdf2SubkeyLength + SaltSize + 1 || array[0] != 0)
			{
				return false;
			}
			byte[] array2 = new byte[SaltSize];
			Buffer.BlockCopy(array, 1, array2, 0, SaltSize);
			byte[] array3 = new byte[Pbkdf2SubkeyLength];
			Buffer.BlockCopy(array, SaltSize + 1, array3, 0, Pbkdf2SubkeyLength);
			byte[] bytes;
			using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, array2, Pbkdf2IterCount))
			{
				bytes = rfc2898DeriveBytes.GetBytes(Pbkdf2SubkeyLength);
			}
			return ByteArraysEqual(array3, bytes);
		}

		[MethodImpl(MethodImplOptions.NoOptimization)]
		private static bool ByteArraysEqual(byte[] a, byte[] b)
		{
			if (ReferenceEquals(a, b))
			{
				return true;
			}
			if (a == null || b == null || a.Length != b.Length)
			{
				return false;
			}
			bool flag = true;
			for (int i = 0; i < a.Length; i++)
			{
				flag &= (a[i] == b[i]);
			}
			return flag;
		}
	}
}
