using System;
using Lynex.BillMaster.Model.Domain.DbModels;
using Lynex.BillMaster.Model.Enum;
using Lynex.Notification.Common.Model;

namespace Lynex.Notification.Common
{
    public interface INotificationFormatProvider<out TModel> where TModel : class, INotificationModel
    {
        TModel GetFormattedModel(TestResult testResult, User receiver);
    }

    public class NotificationFormatProvider<TModel> : INotificationFormatProvider<TModel> where TModel : class, INotificationModel
    {
        private readonly ITemplateProvider _templateProvider;

        public NotificationFormatProvider(FormatType formatType)
        {
            _templateProvider = new TemplateProvider(typeof(TModel), formatType);          
        }

        public TModel GetFormattedModel(TestResult testResult, User receiver)
        {
            var template = _templateProvider.GetTemplate();
            var output = (TModel)Activator.CreateInstance(typeof(TModel), testResult, receiver, template);
            return output;
        }
    }
}
