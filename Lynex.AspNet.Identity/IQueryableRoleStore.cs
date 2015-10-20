using System;
using System.Linq;

namespace Lynex.AspNet.Identity
{
	public interface IQueryableRoleStore<TRole, in TKey> : IRoleStore<TRole, TKey>, IDisposable where TRole : IRole<TKey>
	{
		IQueryable<TRole> Roles
		{
			get;
		}
	}
	public interface IQueryableRoleStore<TRole> : IQueryableRoleStore<TRole, string>, IRoleStore<TRole, string>, IDisposable where TRole : IRole<string>
	{
	}
}
