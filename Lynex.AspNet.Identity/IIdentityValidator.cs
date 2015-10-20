using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public interface IIdentityValidator<in T>
	{
		Task<IdentityResult> ValidateAsync(T item);
	}
}
