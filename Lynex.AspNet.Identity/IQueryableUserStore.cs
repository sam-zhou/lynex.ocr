using System;
using System.Linq;

namespace Lynex.AspNet.Identity
{
	public interface IQueryableUserStore<TUser, in TKey> : IUserStore<TUser, TKey> where TUser : class, IUser<TKey>
	{
		IQueryable<TUser> Users
		{
			get;
		}
	}
	public interface IQueryableUserStore<TUser> : IQueryableUserStore<TUser, string> where TUser : class, IUser<string>
	{
	}
}
