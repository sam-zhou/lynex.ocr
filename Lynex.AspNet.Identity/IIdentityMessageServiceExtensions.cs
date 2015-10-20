using System;

namespace Lynex.AspNet.Identity
{
	public static class IIdentityMessageServiceExtensions
	{
		public static void Send(this IIdentityMessageService service, IdentityMessage message)
		{
			if (service == null)
			{
				throw new ArgumentNullException("service");
			}
			AsyncHelper.RunSync(() => service.SendAsync(message));
		}
	}
}
