using System.Collections.Generic;
using System.ComponentModel;
using Lynex.BillMaster.Model.Domain.DbModels;

namespace Lynex.BillMaster.Model.Domain
{
    public class SystemSetting
    {
        private IDictionary<string, object> _settings;

        private IDictionary<string, object> Settings
        {
            get
            {
                if (_settings == null)
                {
                    _settings = new Dictionary<string, object>();
                }
                return _settings;
            }
        }

        public SystemSetting(IEnumerable<Setting> settings)
        {
            if (settings != null)
            {
                foreach (var setting in settings)
                {
                    var typeConverter = TypeDescriptor.GetConverter(setting.Type);
                    var value = typeConverter.ConvertFromInvariantString(setting.Value);
                    Settings.Add(setting.Name, value);
                }
            }
        }

        private object GetValue(string key)
        {
            if (Settings.ContainsKey(key))
            {
                return Settings[key];
            }
            return null;
        }
    }
}
