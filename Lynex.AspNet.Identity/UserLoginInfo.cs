namespace Lynex.AspNet.Identity
{
	public sealed class UserLoginInfo
	{
		public string LoginProvider
		{
			get;
			set;
		}

		public string ProviderKey
		{
			get;
			set;
		}

		public UserLoginInfo(string loginProvider, string providerKey)
		{
			this.LoginProvider = loginProvider;
			this.ProviderKey = providerKey;
		}
	}
}
