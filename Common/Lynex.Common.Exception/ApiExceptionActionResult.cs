using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace Lynex.Common.Exception
{
    public class ApiExceptionActionResult : IHttpActionResult
    {
        public HttpStatusCode StatusCode { get; }
        public string Message { get; }
        public HttpRequestMessage Request { get; }

        public ApiExceptionActionResult(HttpRequestMessage request, HttpStatusCode statusCode, string message)
        {
            StatusCode = statusCode;
            Request = request;
            Message = message;
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(ExecuteResult());
        }

        public HttpResponseMessage ExecuteResult()
        {
            var response = new HttpResponseMessage(StatusCode)
            {
                Content = new StringContent(Message),
                RequestMessage = Request
            };

            return response;
        }
    }

    
}
