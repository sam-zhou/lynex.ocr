using Lynex.Model.Domain.DbModels;

namespace Lynex.Notification.Common.Model
{
    public abstract class NotificationModel : INotificationModel
    {
        protected string Template { get; private set; }

        public User Receiver { get; protected set; }

        public string Body { get; protected set; }

        public TestResult TestResult { get; private set; }

        public User User
        {
            get { return TestResult.User; }
        }

        protected NotificationModel(TestResult testResult, User receiver, string template)
        {
            Receiver = receiver;
            Template = template;
            TestResult = testResult;
            Body = template;
        }
    }
}
