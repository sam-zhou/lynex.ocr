using System;

namespace Lynex.AspNet.Identity
{
	public static class RoleManagerExtensions
	{
		public static TRole FindById<TRole, TKey>(this RoleManager<TRole, TKey> manager, TKey roleId) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.FindByIdAsync(roleId));
		}

		public static TRole FindByName<TRole, TKey>(this RoleManager<TRole, TKey> manager, string roleName) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.FindByNameAsync(roleName));
		}

		public static IdentityResult Create<TRole, TKey>(this RoleManager<TRole, TKey> manager, TRole role) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.CreateAsync(role));
		}

		public static IdentityResult Update<TRole, TKey>(this RoleManager<TRole, TKey> manager, TRole role) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.UpdateAsync(role));
		}

		public static IdentityResult Delete<TRole, TKey>(this RoleManager<TRole, TKey> manager, TRole role) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.DeleteAsync(role));
		}

		public static bool RoleExists<TRole, TKey>(this RoleManager<TRole, TKey> manager, string roleName) where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			return AsyncHelper.RunSync(() => manager.RoleExistsAsync(roleName));
		}
	}
}
