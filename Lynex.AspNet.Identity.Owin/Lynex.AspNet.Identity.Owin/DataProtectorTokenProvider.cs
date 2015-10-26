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
				throw new ArgumentNullException(nameof(protector));
			}
			Protector = protector;
			TokenLifespan = TimeSpan.FromDays(1.0);
		}

		public async Task<string> GenerateAsync(string purpose, UserManager<TUser, TKey> manager, TUser user)
		{
			if (user == null)
			{
				throw new ArgumentNullException(nameof(user));
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
					text = await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture();
				}
				binaryWriter.Write(text ?? "");
			}
			byte[] inArray = Protector.Protect(memoryStream.ToArray());
			return Convert.ToBase64String(inArray);
		}

		public async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser, TKey> manager, TUser user)
		{
		    try
			{
				byte[] buffer = Protector.Unprotect(Convert.FromBase64String(token));
				MemoryStream stream = new MemoryStream(buffer);
				using (BinaryReader binaryReader = stream.CreateReader())
				{
					DateTimeOffset dateTimeOffset = binaryReader.ReadDateTimeOffset();
					DateTimeOffset left = dateTimeOffset + TokenLifespan;
					if (left < DateTimeOffset.UtcNow)
					{
						return false;
					}
					string a = binaryReader.ReadString();
					if (!string.Equals(a, Convert.ToString(user.Id, CultureInfo.InvariantCulture)))
					{
						return false;
					}
					string a2 = binaryReader.ReadString();
					if (!string.Equals(a2, purpose))
					{
						return false;
					}
					string a3 = binaryReader.ReadString();
					if (binaryReader.PeekChar() != -1)
					{
						return false;
					}
				    if (manager.SupportsUserSecurityStamp)
					{
						string b = await manager.GetSecurityStampAsync(user.Id).WithCurrentCulture();
						return (a3 == b);
					}
					return (a3 == "");
				}
			}
			catch (Exception)
			{
			    // ignored
			}
		    return false;
		}

		public Task<bool> IsValidProviderForUserAsync(UserManager<TUser, TKey> manager, TUser user)
		{
			return Task.FromResult(true);
		}

		public Task NotifyAsync(string token, UserManager<TUser, TKey> manager, TUser user)
		{
			return Task.FromResult(0);
		}
	}
	public class DataProtectorTokenProvider<TUser> : DataProtectorTokenProvider<TUser, string> where TUser : class, IUser<string>
	{
		public DataProtectorTokenProvider(IDataProtector protector) : base(protector)
		{
		}
	}
}
