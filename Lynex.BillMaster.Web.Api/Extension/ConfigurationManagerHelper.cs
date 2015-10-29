using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lynex.BillMaster.Web.Api.Extension
{
    public static class ConfigurationManagerHelper
    {
        public static int GetIntSetting(string key)
        {
            var value = ConfigurationManager.AppSettings[key];
            int result;
            int.TryParse(value, out result);
            return result;
        }
    }
}
