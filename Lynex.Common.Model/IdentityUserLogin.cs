using Lynex.Common.Model.DomainModel;

namespace Lynex.Common.Model
{
    public class IdentityUserLogin : ValueObject
    {
        public virtual string LoginProvider { get; set; }

        public virtual string ProviderKey { get; set; }

    }
}
