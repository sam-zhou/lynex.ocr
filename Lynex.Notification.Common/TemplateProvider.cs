using System;
using System.IO;
using System.Linq;
using Lynex.Model.Enum;

namespace Lynex.Notification.Common
{
    public interface ITemplateProvider
    {
        string GetTemplate();
    }

    public class TemplateProvider : ITemplateProvider
    {
        private readonly string _template;

        public TemplateProvider(Type type, FormatType formatType)
        {
            var resourceName =
                type.Assembly.GetManifestResourceNames()
                    .FirstOrDefault(q => q.Contains(".Template." + formatType.ToString()));
            if (resourceName != null)
            {
                using (var stream = type.Assembly.GetManifestResourceStream(resourceName))
                {
                    if (stream != null)
                    {
                        using (var reader = new StreamReader(stream))
                        {
                            _template = reader.ReadToEnd();
                        }
                    }
                    else
                    {
                        _template = resourceName;
                    }
                }
            }
            
            
        }


        public string GetTemplate()
        {
            return _template;
        }

    }
}
