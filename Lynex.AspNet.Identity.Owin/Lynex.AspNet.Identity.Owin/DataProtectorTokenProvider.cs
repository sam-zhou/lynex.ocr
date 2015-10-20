using System;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;
using Lynex.AspNet.Identity;
using Microsoft.Owin.Security.DataProtection;

namespace Lynex.AspNet.Identity.Owin
{
	public class DataProtectorTokenProvider<TUser, TKey> : IUserTokenProvider<TUser, TKey> where TUser : class, IUser<TKey> where TKey : IEquatable<TKey>
	{
		public IDataProtector Protector
		{
			get;
			private set;
		}

		public TimeSpan TokenLifespan
		{
			get;
			set;
		}

		public DataProtectorTokenProvider(IDataProtector protector)
		{
			if (protector == null)
			{
				throw new ArgumentNullException("protector");
			}
			this.Protector = protector;
			this.TokenLifespan = TimeSpan.FromDays(1.0);
		}

		public async Task<string> GenerateAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException("user");
			}
			MemoryStream memoryStream = new MemoryStream();
			using (BinaryWriter binaryWriter = memoryStream.CreateWriter())
			{
				binaryWriter.Write(DateTimeOffset.UtcNow);
				binaryWriter.Write(Convert.ToString(user.Id, CultureInfo.InvariantCulture));
				binaryWriter.Write(purpose ?? "");
				string text = null;
				if (manager.SupportsUserSecurityStamp)
				{
					text = await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture<string>();
				}
				binaryWriter.Write(text ?? "");
			}
			byte[] inArray = this.Protector.Protect(memoryStream.ToArray());
			return Convert.ToBase64String(inArray);
		}

		public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser, TKey> manager, TUser user)
		{
			bool result;
			try
			{
				byte[] buffer = this.Protector.Unprotect(Convert.FromBase64String(token));
				MemoryStream stream = new MemoryStream(buffer);
				using (BinaryReader binaryReader = stream.CreateReader())
				{
					DateTimeOffset dateTimeOffset = binaryReader.ReadDateTimeOffset();
					DateTimeOffset left = dateTimeOffset + this.TokenLifespan;
					if (left < DateTimeOffset.UtcNow)
					{
						result = false;
						return result;
					}
					string a = binaryReader.ReadString();
					if (!string.Equals(a, Convert.ToString(user.Id, CultureInfo.InvariantCulture)))
					{
						result = false;
						return result;
					}
					string a2 = binaryReader.ReadString();
					if (!string.Equals(a2, purpose))
					{
						result = false;
						return result;
					}
					string a3 = binaryReader.ReadString();
					if (binaryReader.PeekChar() != -1)
					{
						result = false;
						return result;
					}
					if (manager.SupportsUserSecurityStamp)
					{
						string b = await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture<string>();
						result = (a3 == b);
						return result;
					}
					result = (a3 == "");
					return result;
				}
			}
			catch
			{
			}
			result = false;
			return result;
		}

		public Task<bool> IsValidProviderForUserAsync(UserManager<TUser, TKey> manager, TUser user)
		{
			return Task.FromResult<bool>(true);
		}

		public Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user)
		{
			return Task.FromResult<int>(0);
		}
	}
	public class DataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser, string> where TUser : class, IUser<string>
	{
		public DataProtectorTokenProvider(IDataProtector protector) : base(protector)
		{
		}
	}
}
