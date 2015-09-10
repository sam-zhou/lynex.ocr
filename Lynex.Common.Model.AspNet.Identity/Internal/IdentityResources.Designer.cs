using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace Lynex.Common.Model.AspNet.Identity.Internal
{
	[CompilerGenerated]
	[DebuggerNonUserCode]
	[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
	internal class IdentityResources
	{
		private static System.Resources.ResourceManager resourceMan;

		private static CultureInfo resourceCulture;

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static CultureInfo Culture
		{
			get
			{
				return IdentityResources.resourceCulture;
			}
			set
			{
				IdentityResources.resourceCulture = value;
			}
		}

		[EditorBrowsable(EditorBrowsableState.Advanced)]
		internal static System.Resources.ResourceManager ResourceManager
		{
			get
			{
				if (object.ReferenceEquals(IdentityResources.resourceMan, null))
				{
					IdentityResources.resourceMan = new System.Resources.ResourceManager("NHibernate.Identity.Internal.IdentityResources", typeof(IdentityResources).Assembly);
				}
				return IdentityResources.resourceMan;
			}
		}

		internal static string RoleNotFound
		{
			get
			{
				return IdentityResources.ResourceManager.GetString("RoleNotFound", IdentityResources.resourceCulture);
			}
		}

		internal static string ValueCannotBeNullOrEmpty
		{
			get
			{
				return IdentityResources.ResourceManager.GetString("ValueCannotBeNullOrEmpty", IdentityResources.resourceCulture);
			}
		}

		internal IdentityResources()
		{
		}
	}
}