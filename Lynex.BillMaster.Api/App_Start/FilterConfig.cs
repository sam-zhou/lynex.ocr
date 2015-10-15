using System.Web;
using System.Web.Http;
using System.Web.Http.Filters;
using System.Web.Mvc;
using Lynex.BillMaster.Api.Filters;

namespace Lynex.BillMaster.Api
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
