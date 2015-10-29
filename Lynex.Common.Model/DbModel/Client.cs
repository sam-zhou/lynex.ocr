using System.ComponentModel.DataAnnotations;
using Lynex.Common.Model.Enum;

namespace Lynex.Common.Model.DbModel
{
    public class Client : BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public virtual string ClientId { get; set; }
        [Required]
        public virtual string Secret { get; set; }
        [Required]
        [MaxLength(100)]
        public virtual string Name { get; set; }
        public virtual ApplicationTypes ApplicationType { get; set; }
        public virtual bool Active { get; set; }
        public virtual int RefreshTokenLifeTime { get; set; }
        [MaxLength(100)]
        public virtual string AllowedOrigin { get; set; }
    }
}
