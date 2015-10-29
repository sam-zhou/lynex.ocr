using System;
using Lynex.Common.Model.DbModel.Interface;
using Lynex.Common.Model.Enum;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.Common
{
    public interface INotificationFormatProvider<out TModel> where TModel : class, INotificationModel
    {
        TModel GetFormattedModel(IUser receiver);
    }

    public class NotificationFormatProvider<TModel> : INotificationFormatProvider<TModel> where TModel : class, INotificationModel
    {
        private readonly ITemplateProvider _templateProvider;

        public NotificationFormatProvider(FormatType formatType)
        {
            _templateProvider = new TemplateProvider(typeof(TModel), formatType);          
        }

        public TModel GetFormattedModel(IUser receiver)
        {
            var template = _templateProvider.GetTemplate();
            var output = (TModel)Activator.CreateInstance(typeof(TModel), receiver, template);
            return output;
        }
    }
}
