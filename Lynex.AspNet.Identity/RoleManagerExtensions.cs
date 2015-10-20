using System;

namespace Lynex.AspNet.Identity
{
	public static class RoleManagerExtensions
	{
		public static TRole FindById<TRole, TKey>(this RoleManager<TRole, TKey> manager, TKey roleId) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<TRole>(() => manager.FindByIdAsync(roleId));
		}

		public static TRole FindByName<TRole, TKey>(this RoleManager<TRole, TKey> manager, string roleName) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<TRole>(() => manager.FindByNameAsync(roleName));
		}

		public static IdentityResult Create<TRole, TKey>(this RoleManager<TRole, TKey> manager, TRole role) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.CreateAsync(role));
		}

		public static IdentityResult Update<TRole, TKey>(this RoleManager<TRole, TKey> manager, TRole role) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.UpdateAsync(role));
		}

		public static IdentityResult Delete<TRole, TKey>(this RoleManager<TRole, TKey> manager, TRole role) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<IdentityResult>(() => manager.DeleteAsync(role));
		}

		public static bool RoleExists<TRole, TKey>(this RoleManager<TRole, TKey> manager, string roleName) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException("manager");
			}
			return AsyncHelper.RunSync<bool>(() => manager.RoleExistsAsync(roleName));
		}
	}
}
