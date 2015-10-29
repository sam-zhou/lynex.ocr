using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Lynex.Common.Model.Status;

namespace Lynex.Common.Model.DataContracts
{
    [DataContract]
    public class TokenError
    {
        [DataMember]
        public string Error { get; set; }

        [DataMember]
        public TokenStatus ErrorDescription { get; set; }

        [DataMember]
        public string ErrorUri { get; set; }
    }
}
