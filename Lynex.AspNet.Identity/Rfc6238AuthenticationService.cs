using System;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace Lynex.AspNet.Identity
{
	internal static class Rfc6238AuthenticationService
	{
		private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		private static readonly TimeSpan Timestep = TimeSpan.FromMinutes(3.0);

		private static readonly Encoding Encoding = new UTF8Encoding(false, true);

		private static int ComputeTotp(HashAlgorithm hashAlgorithm, ulong timestepNumber, string modifier)
		{
			byte[] bytes = BitConverter.GetBytes(IPAddress.HostToNetworkOrder((long)timestepNumber));
			byte[] array = hashAlgorithm.ComputeHash(ApplyModifier(bytes, modifier));
			int num = array[array.Length - 1] & 15;
			int num2 = (array[num] & 127) << 24 | (array[num + 1] & 255) << 16 | (array[num + 2] & 255) << 8 | array[num + 3] & 255;
			return num2 % 1000000;
		}

		private static byte[] ApplyModifier(byte[] input, string modifier)
		{
			if (string.IsNullOrEmpty(modifier))
			{
				return input;
			}
			byte[] bytes = Encoding.GetBytes(modifier);
			byte[] array = new byte[checked(input.Length + bytes.Length)];
			Buffer.BlockCopy(input, 0, array, 0, input.Length);
			Buffer.BlockCopy(bytes, 0, array, input.Length, bytes.Length);
			return array;
		}

		private static ulong GetCurrentTimeStepNumber()
		{
			return (ulong)((DateTime.UtcNow - UnixEpoch).Ticks / Timestep.Ticks);
		}

		public static int GenerateCode(SecurityToken securityToken, string modifier = null)
		{
			if (securityToken == null)
			{
				throw new ArgumentNullException(nameof(securityToken));
			}
			ulong currentTimeStepNumber = GetCurrentTimeStepNumber();
			int result;
			using (HMACSHA1 hMacsha = new HMACSHA1(securityToken.GetDataNoClone()))
			{
				result = ComputeTotp(hMacsha, currentTimeStepNumber, modifier);
			}
			return result;
		}

		public static bool ValidateCode(SecurityToken securityToken, int code, string modifier = null)
		{
			if (securityToken == null)
			{
				throw new ArgumentNullException(nameof(securityToken));
			}
			ulong currentTimeStepNumber = GetCurrentTimeStepNumber();
			using (HMACSHA1 hMacsha = new HMACSHA1(securityToken.GetDataNoClone()))
			{
				for (int i = -2; i <= 2; i++)
				{
					int num = ComputeTotp(hMacsha, currentTimeStepNumber + (ulong)i, modifier);
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
