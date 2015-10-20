using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IIdentityMessageService
	{
		Task SendAsync(IdentityMessage message);
	}
}
