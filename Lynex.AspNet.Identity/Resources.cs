using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Lynex.AspNet.Identity
{
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(Resources.resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Microsoft.AspNet.Identity.Resources", typeof(Resources).Assembly);
					Resources.resourceMan = resourceManager;
				}
				return Resources.resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return Resources.resourceCulture;
			}
			set
			{
				Resources.resourceCulture = value;
			}
		}

		internal static string DefaultError
		{
			get
			{
				return Resources.ResourceManager.GetString("DefaultError", Resources.resourceCulture);
			}
		}

		internal static string DuplicateEmail
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateEmail", Resources.resourceCulture);
			}
		}

		internal static string DuplicateName
		{
			get
			{
				return Resources.ResourceManager.GetString("DuplicateName", Resources.resourceCulture);
			}
		}

		internal static string ExternalLoginExists
		{
			get
			{
				return Resources.ResourceManager.GetString("ExternalLoginExists", Resources.resourceCulture);
			}
		}

		internal static string InvalidEmail
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidEmail", Resources.resourceCulture);
			}
		}

		internal static string InvalidToken
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidToken", Resources.resourceCulture);
			}
		}

		internal static string InvalidUserName
		{
			get
			{
				return Resources.ResourceManager.GetString("InvalidUserName", Resources.resourceCulture);
			}
		}

		internal static string LockoutNotEnabled
		{
			get
			{
				return Resources.ResourceManager.GetString("LockoutNotEnabled", Resources.resourceCulture);
			}
		}

		internal static string NoTokenProvider
		{
			get
			{
				return Resources.ResourceManager.GetString("NoTokenProvider", Resources.resourceCulture);
			}
		}

		internal static string NoTwoFactorProvider
		{
			get
			{
				return Resources.ResourceManager.GetString("NoTwoFactorProvider", Resources.resourceCulture);
			}
		}

		internal static string PasswordMismatch
		{
			get
			{
				return Resources.ResourceManager.GetString("PasswordMismatch", Resources.resourceCulture);
			}
		}

		internal static string PasswordRequireDigit
		{
			get
			{
				return Resources.ResourceManager.GetString("PasswordRequireDigit", Resources.resourceCulture);
			}
		}

		internal static string PasswordRequireLower
		{
			get
			{
				return Resources.ResourceManager.GetString("PasswordRequireLower", Resources.resourceCulture);
			}
		}

		internal static string PasswordRequireNonLetterOrDigit
		{
			get
			{
				return Resources.ResourceManager.GetString("PasswordRequireNonLetterOrDigit", Resources.resourceCulture);
			}
		}

		internal static string PasswordRequireUpper
		{
			get
			{
				return Resources.ResourceManager.GetString("PasswordRequireUpper", Resources.resourceCulture);
			}
		}

		internal static string PasswordTooShort
		{
			get
			{
				return Resources.ResourceManager.GetString("PasswordTooShort", Resources.resourceCulture);
			}
		}

		internal static string PropertyTooShort
		{
			get
			{
				return Resources.ResourceManager.GetString("PropertyTooShort", Resources.resourceCulture);
			}
		}

		internal static string RoleNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("RoleNotFound", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIQueryableRoleStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIQueryableRoleStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIQueryableUserStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIQueryableUserStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserClaimStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserClaimStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserConfirmationStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserConfirmationStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserEmailStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserEmailStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserLockoutStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserLockoutStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserLoginStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserLoginStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserPasswordStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserPasswordStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserPhoneNumberStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserPhoneNumberStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserRoleStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserRoleStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserSecurityStampStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserSecurityStampStore", Resources.resourceCulture);
			}
		}

		internal static string StoreNotIUserTwoFactorStore
		{
			get
			{
				return Resources.ResourceManager.GetString("StoreNotIUserTwoFactorStore", Resources.resourceCulture);
			}
		}

		internal static string UserAlreadyHasPassword
		{
			get
			{
				return Resources.ResourceManager.GetString("UserAlreadyHasPassword", Resources.resourceCulture);
			}
		}

		internal static string UserAlreadyInRole
		{
			get
			{
				return Resources.ResourceManager.GetString("UserAlreadyInRole", Resources.resourceCulture);
			}
		}

		internal static string UserIdNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("UserIdNotFound", Resources.resourceCulture);
			}
		}

		internal static string UserNameNotFound
		{
			get
			{
				return Resources.ResourceManager.GetString("UserNameNotFound", Resources.resourceCulture);
			}
		}

		internal static string UserNotInRole
		{
			get
			{
				return Resources.ResourceManager.GetString("UserNotInRole", Resources.resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
