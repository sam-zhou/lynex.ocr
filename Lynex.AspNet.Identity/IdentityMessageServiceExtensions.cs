using System;

namespace Lynex.AspNet.Identity
{
	public static class IdentityMessageServiceExtensions
	{
		public static void Send(this IIdentityMessageService service, IdentityMessage message)
		{
			if (service == null)
			{
				throw new ArgumentNullException(nameof(service));
			}
			AsyncHelper.RunSync(() => service.SendAsync(message));
		}
	}
}
