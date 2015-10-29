using System;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lynex.Common.Model.DataContracts
{
    [DataContract]
    public class Token
    {
        [DataMember]
        public string AccessToken { get; set; }
        [DataMember]
        public string TokenType { get; set; }
        [DataMember]
        public int ExpiresIn { get; set; }
        [DataMember]
        public string Scope { get; set; }
        [DataMember]
        public string RefreshToken { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = ".issued")]
        public DateTime Issued { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = ".expires")]
        public DateTime Expires { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = "as:client_id")]
        public string ClientId { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = "userName")]
        public string UserName { get; set; }
    }
}
