using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Lynex.AspNet.Identity
{
	internal static class Rfc6238AuthenticationService
	{
		private static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static readonly TimeSpan _timestep = TimeSpan.FromMinutes(3.0);

		private static readonly Encoding _encoding = new UTF8Encoding(false, true);

		private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
			byte[] array = hashAlgorithm.ComputeHash(Rfc6238AuthenticationService.ApplyModifier(bytes, modifier));
			int num = (int)(array[array.Length - 1] & 15);
			int num2 = (int)(array[num] & 127) << 24 | (int)(array[num + 1] & 255) << 16 | (int)(array[num + 2] & 255) << 8 | (int)(array[num + 3] & 255);
			return num2 % 1000000;
		}

		private static byte[] ApplyModifier(byte[] input, string modifier)
		{
			if (string.IsNullOrEmpty(modifier))
			{
				return input;
			}
			byte[] bytes = Rfc6238AuthenticationService._encoding.GetBytes(modifier);
			byte[] array = new byte[checked(input.Length + bytes.Length)];
			Buffer.BlockCopy(input, 0, array, 0, input.Length);
			Buffer.BlockCopy(bytes, 0, array, input.Length, bytes.Length);
			return array;
		}

		private static ulong GetCurrentTimeStepNumber()
		{
			return (ulong)((DateTime.UtcNow - Rfc6238AuthenticationService._unixEpoch).Ticks / Rfc6238AuthenticationService._timestep.Ticks);
		}

		public static int GenerateCode(SecurityToken securityToken, string modifier = null)
		{
			if (securityToken == null)
			{
				throw new ArgumentNullException("securityToken");
			}
			ulong currentTimeStepNumber = Rfc6238AuthenticationService.GetCurrentTimeStepNumber();
			int result;
			using (HMACSHA1 hMACSHA = new HMACSHA1(securityToken.GetDataNoClone()))
			{
				result = Rfc6238AuthenticationService.ComputeTotp(hMACSHA, currentTimeStepNumber, modifier);
			}
			return result;
		}

		public static bool ValidateCode(SecurityToken securityToken, int code, string modifier = null)
		{
			if (securityToken == null)
			{
				throw new ArgumentNullException("securityToken");
			}
			ulong currentTimeStepNumber = Rfc6238AuthenticationService.GetCurrentTimeStepNumber();
			using (HMACSHA1 hMACSHA = new HMACSHA1(securityToken.GetDataNoClone()))
			{
				for (int i = -2; i <= 2; i++)
				{
					int num = Rfc6238AuthenticationService.ComputeTotp(hMACSHA, currentTimeStepNumber + (ulong)((long)i), modifier);
					if (num == code)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
