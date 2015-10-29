using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Lynex.Common.Exception;
using Lynex.Common.Extension.Json;
using Lynex.Common.Model.DataContracts;
using Newtonsoft.Json;

namespace Lynex.Common.ClientService
{
    public static class Authentication
    {
        public static Token GetAccessToken(string requestUrl, string username, string password, string clientId, string clientSecret)
        {
            string postData = $"grant_type=password&username={username}&password={password}&client_id={clientId}&client_secret={clientSecret}";
            return GetToken(requestUrl, postData);

        }

        public static Token RefreshAccessToken(string requestUrl, string refreshToken, string clientId, string clientSecret)
        {
            string postData = $"grant_type=refresh_token&client_id={clientId}&client_secret={clientSecret}&refresh_token={refreshToken}";
            return GetToken(requestUrl, postData);
        }

        private static Token GetToken(string requestUrl, string postData)
        {
            byte[] postDataEncoded = Encoding.UTF8.GetBytes(postData);

            var req = WebRequest.Create(requestUrl);
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = postDataEncoded.Length;

            Stream requestStream = req.GetRequestStream();
            requestStream.Write(postDataEncoded, 0, postDataEncoded.Length);
            try
            {
                WebResponse res = req.GetResponse();

                using (var rs = res.GetResponseStream())
                {
                    

                    if (rs != null)
                    {
                        using (var d = new StreamReader(rs))
                        {
                            var response = d.ReadToEnd();
                            var settings = new JsonSerializerSettings();

                            settings.ContractResolver = new SnakeCaseContractResolver();
                            settings.NullValueHandling = NullValueHandling.Ignore;
                            return JsonConvert.DeserializeObject<Token>(response, settings);
                            
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                var apiEx = ApiException.CreateApiException(ex);
                throw apiEx;
            }
            

            


            return null;
        }
    }
}
