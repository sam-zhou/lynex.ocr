using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public class RoleValidator<TRole, TKey> : IIdentityValidator<TRole> where TRole : class, IRole<TKey> where TKey : IEquatable<TKey>
	{
		private RoleManager<TRole, TKey> Manager
		{
			get;
			set;
		}

		public RoleValidator(RoleManager<TRole, TKey> manager)
		{
			if (manager == null)
			{
				throw new ArgumentNullException(nameof(manager));
			}
			Manager = manager;
		}

		public virtual async Task<IdentityResult> ValidateAsync(TRole item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}
			List<string> list = new List<string>();
			await ValidateRoleName(item, list).WithCurrentCulture();
			IdentityResult result;
			if (list.Count > 0)
			{
				result = IdentityResult.Failed(list.ToArray());
			}
			else
			{
				result = IdentityResult.Success;
			}
			return result;
		}

		private async Task ValidateRoleName(TRole role, List<string> errors)
		{
			if (string.IsNullOrWhiteSpace(role.Name))
			{
				errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.PropertyTooShort, "Name"));
			}
			else
			{
				TRole tRole = await Manager.FindByNameAsync(role.Name).WithCurrentCulture();
				if (tRole != null && !EqualityComparer<TKey>.Default.Equals(tRole.Id, role.Id))
				{
					errors.Add(string.Format(CultureInfo.CurrentCulture, Resources.DuplicateName, role.Name));
				}
			}
		}
	}
	public class RoleValidator<TRole> : RoleValidator<TRole, string> where TRole : class, IRole<string>
	{
		public RoleValidator(RoleManager<TRole, string> manager) : base(manager)
		{
		}
	}
}
