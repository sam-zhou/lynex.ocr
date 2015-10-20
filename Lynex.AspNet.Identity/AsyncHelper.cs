using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public static class AsyncHelper
	{
		private static readonly TaskFactory _myTaskFactory = new TaskFactory(CancellationToken.None, TaskCreationOptions.None, TaskContinuationOptions.None, TaskScheduler.Default);

		public static TResult RunSync<TResult>(Func<Task<TResult>> func)
		{
			CultureInfo cultureUi = CultureInfo.CurrentUICulture;
			CultureInfo culture = CultureInfo.CurrentCulture;
			return AsyncHelper._myTaskFactory.StartNew<Task<TResult>>(delegate
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = cultureUi;
				return func();
			}).Unwrap<TResult>().GetAwaiter().GetResult();
		}

		public static void RunSync(Func<Task> func)
		{
			CultureInfo cultureUi = CultureInfo.CurrentUICulture;
			CultureInfo culture = CultureInfo.CurrentCulture;
			AsyncHelper._myTaskFactory.StartNew<Task>(delegate
			{
				Thread.CurrentThread.CurrentCulture = culture;
				Thread.CurrentThread.CurrentUICulture = cultureUi;
				return func();
			}).Unwrap().GetAwaiter().GetResult();
		}
	}
}
