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

            WebResponse res = req.GetResponse();

            using (var rs = res.GetResponseStream())
            {
                if (rs != null)
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Token));
                    //Get deserialized object from JSON stream
                    var token = (Token)serializer.ReadObject(rs);
                    return token;

                    // Process FORM POST.

                }
            }


            return null;
        }
    }
}
