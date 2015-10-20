using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;
using log4net;
using log4net.Repository.Hierarchy;
using Lynex.BillMaster.Api.IoC;
using Lynex.Common.Exception;
using LogManager = WebGrease.LogManager;

namespace Lynex.BillMaster.Api.Filters
{
    public class ExceptionHandlingFilterAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _log;

        public ExceptionHandlingFilterAttribute()
        {
            _log = IoCContainer.Resolve<ILog>();
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            _log.Error(context.Exception);

            if (context.Exception is NotImplementedException)
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.NotFound)
                {
                    Content = new StringContent(context.Exception.Message)
                };
            }
            else
            {
                context.Response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    ReasonPhrase = context.Exception.Message,

                };
            }
        }
    }
}
