using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace Lynex.AspNet.Identity
{
	[GeneratedCode("System.Tools.StronglyTypedResourceBuilder", "4.0.0.0"), DebuggerNonUserCode, CompilerGenerated]
	internal class Resources
	{
		private static ResourceManager _resourceMan;

		private static CultureInfo _resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static ResourceManager ResourceManager
		{
			get
			{
				if (ReferenceEquals(_resourceMan, null))
				{
					ResourceManager resourceManager = new ResourceManager("Lynex.AspNet.Identity.Resources", typeof(Resources).Assembly);
					_resourceMan = resourceManager;
				}
				return _resourceMan;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return _resourceCulture;
			}
			set
			{
				_resourceCulture = value;
			}
		}

		internal static string DefaultError
		{
			get
			{
				return ResourceManager.GetString("DefaultError", _resourceCulture);
			}
		}

		internal static string DuplicateEmail
		{
			get
			{
				return ResourceManager.GetString("DuplicateEmail", _resourceCulture);
			}
		}

		internal static string DuplicateName
		{
			get
			{
				return ResourceManager.GetString("DuplicateName", _resourceCulture);
			}
		}

		internal static string ExternalLoginExists
		{
			get
			{
				return ResourceManager.GetString("ExternalLoginExists", _resourceCulture);
			}
		}

		internal static string InvalidEmail
		{
			get
			{
				return ResourceManager.GetString("InvalidEmail", _resourceCulture);
			}
		}

		internal static string InvalidToken
		{
			get
			{
				return ResourceManager.GetString("InvalidToken", _resourceCulture);
			}
		}

		internal static string InvalidUserName
		{
			get
			{
				return ResourceManager.GetString("InvalidUserName", _resourceCulture);
			}
		}

		internal static string LockoutNotEnabled
		{
			get
			{
				return ResourceManager.GetString("LockoutNotEnabled", _resourceCulture);
			}
		}

		internal static string NoTokenProvider
		{
			get
			{
				return ResourceManager.GetString("NoTokenProvider", _resourceCulture);
			}
		}

		internal static string NoTwoFactorProvider
		{
			get
			{
				return ResourceManager.GetString("NoTwoFactorProvider", _resourceCulture);
			}
		}

		internal static string PasswordMismatch
		{
			get
			{
				return ResourceManager.GetString("PasswordMismatch", _resourceCulture);
			}
		}

		internal static string PasswordRequireDigit
		{
			get
			{
				return ResourceManager.GetString("PasswordRequireDigit", _resourceCulture);
			}
		}

		internal static string PasswordRequireLower
		{
			get
			{
				return ResourceManager.GetString("PasswordRequireLower", _resourceCulture);
			}
		}

		internal static string PasswordRequireNonLetterOrDigit
		{
			get
			{
				return ResourceManager.GetString("PasswordRequireNonLetterOrDigit", _resourceCulture);
			}
		}

		internal static string PasswordRequireUpper
		{
			get
			{
				return ResourceManager.GetString("PasswordRequireUpper", _resourceCulture);
			}
		}

		internal static string PasswordTooShort
		{
			get
			{
				return ResourceManager.GetString("PasswordTooShort", _resourceCulture);
			}
		}

		internal static string PropertyTooShort
		{
			get
			{
				return ResourceManager.GetString("PropertyTooShort", _resourceCulture);
			}
		}

		internal static string RoleNotFound
		{
			get
			{
				return ResourceManager.GetString("RoleNotFound", _resourceCulture);
			}
		}

		internal static string StoreNotIQueryableRoleStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIQueryableRoleStore", _resourceCulture);
			}
		}

		internal static string StoreNotIQueryableUserStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIQueryableUserStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserClaimStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserClaimStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserConfirmationStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserConfirmationStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserEmailStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserEmailStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserLockoutStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserLockoutStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserLoginStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserLoginStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserPasswordStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserPasswordStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserPhoneNumberStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserPhoneNumberStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserRoleStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserRoleStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserSecurityStampStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserSecurityStampStore", _resourceCulture);
			}
		}

		internal static string StoreNotIUserTwoFactorStore
		{
			get
			{
				return ResourceManager.GetString("StoreNotIUserTwoFactorStore", _resourceCulture);
			}
		}

		internal static string UserAlreadyHasPassword
		{
			get
			{
				return ResourceManager.GetString("UserAlreadyHasPassword", _resourceCulture);
			}
		}

		internal static string UserAlreadyInRole
		{
			get
			{
				return ResourceManager.GetString("UserAlreadyInRole", _resourceCulture);
			}
		}

		internal static string UserIdNotFound
		{
			get
			{
				return ResourceManager.GetString("UserIdNotFound", _resourceCulture);
			}
		}

		internal static string UserNameNotFound
		{
			get
			{
				return ResourceManager.GetString("UserNameNotFound", _resourceCulture);
			}
		}

		internal static string UserNotInRole
		{
			get
			{
				return ResourceManager.GetString("UserNotInRole", _resourceCulture);
			}
		}

		internal Resources()
		{
		}
	}
}
