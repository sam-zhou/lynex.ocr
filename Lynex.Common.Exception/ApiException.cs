using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using Lynex.Common.Extension.Json;
using Lynex.Common.Model.DataContracts;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Lynex.Common.Exception
{
    public class ApiException : System.Exception
    {
        public HttpResponseMessage Response { get; set; }
        public ApiException(HttpResponseMessage response)
        {
            Response = response;
        }

        public ApiException()
        {
            
        }

        public HttpStatusCode StatusCode
        {
            get
            {
                return Response.StatusCode;
            }
        }


        public IEnumerable<string> Errors
        {
            get
            {
                return Data.Values.Cast<string>().ToList();
            }
        }

        public static ApiException CreateApiException(WebException ex)
        {
            var message = new HttpResponseMessage(((HttpWebResponse)ex.Response).StatusCode);
            var exception = new ApiException(message);
            if (ex.Response != null)
            {
                var rs = ex.Response.GetResponseStream();
                if (rs != null)
                {
                    using (var d = new StreamReader(rs))
                    {
                        var response = d.ReadToEnd();
                        var settings = new JsonSerializerSettings();

                        settings.ContractResolver = new SnakeCaseContractResolver();
                        settings.NullValueHandling = NullValueHandling.Ignore;
                        var error = JsonConvert.DeserializeObject<TokenError>(response, settings);
                        exception.Data.Add(0, error.ErrorDescription.ToString());
                    }
                }
                
            }
            return exception;
        }

        public static ApiException CreateApiException(HttpResponseMessage response)
        {
            var httpErrorObject = response.Content.ReadAsStringAsync().Result;

            // Create an anonymous object to use as the template for deserialization:
            var anonymousErrorObject =
                new { message = "", ModelState = new Dictionary<string, string[]>() };

            // Deserialize:
            var deserializedErrorObject =
                JsonConvert.DeserializeAnonymousType(httpErrorObject, anonymousErrorObject);

            // Now wrap into an exception which best fullfills the needs of your application:
            var ex = new ApiException(response);

            // Sometimes, there may be Model Errors:
            if (deserializedErrorObject.ModelState != null)
            {
                var errors =
                    deserializedErrorObject.ModelState
                                            .Select(kvp => string.Join(". ", kvp.Value)).ToList();

                for (int i = 0; i < errors.Count(); i++)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(i, errors.ElementAt(i));
                }
            }
            // Othertimes, there may not be Model Errors:
            else
            {
                var error =
                    JsonConvert.DeserializeObject<Dictionary<string, string>>(httpErrorObject);
                foreach (var kvp in error)
                {
                    // Wrap the errors up into the base Exception.Data Dictionary:
                    ex.Data.Add(kvp.Key, kvp.Value);
                }
            }
            return ex;
        }
    }
}
