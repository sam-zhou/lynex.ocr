using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using Lynex.Common.Exception;

namespace Lynex.BillMaster.Api.Filters
{
    public class ExceptionHandlingFilterAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
            else if(context.Exception is InvalidOperationException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
        }
    }
}
