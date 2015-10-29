using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lynex.Common.Model.DbModel.Interface;

namespace Lynex.Common.Model.DbModel
{
    public class RefreshToken : IDbModel
    {
        [Key]
        public virtual string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string Subject { get; set; }
        [Required]
        [MaxLength(50)]
        public virtual string ClientId { get; set; }
        public virtual DateTime IssuedUtc { get; set; }
        public virtual DateTime ExpiresUtc { get; set; }
        [Required]
        public virtual string ProtectedTicket { get; set; }
    }
}
