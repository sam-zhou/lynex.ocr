using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Lynex.AspNet.Identity
{
	public static class TaskExtensions
	{
		public struct CultureAwaiter<T> : ICriticalNotifyCompletion
		{
			private readonly Task<T> _task;

			public bool IsCompleted
			{
				get
				{
					return _task.IsCompleted;
				}
			}

			public CultureAwaiter(Task<T> task)
			{
				_task = task;
			}

			public CultureAwaiter<T> GetAwaiter()
			{
				return this;
			}

			public T GetResult()
			{
				return _task.GetAwaiter().GetResult();
			}

			public void OnCompleted(Action continuation)
			{
				throw new NotImplementedException();
			}

			public void UnsafeOnCompleted(Action continuation)
			{
				CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
				CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
				_task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(delegate
				{
					Thread.CurrentThread.CurrentCulture = currentCulture;
					Thread.CurrentThread.CurrentUICulture = currentUiCulture;
					try
					{
						continuation();
					}
					finally
					{
						Thread.CurrentThread.CurrentCulture = currentCulture;
						Thread.CurrentThread.CurrentUICulture = currentUiCulture;
					}
				});
			}
		}

		public struct CultureAwaiter : ICriticalNotifyCompletion
		{
			private readonly Task _task;

			public bool IsCompleted
			{
				get
				{
					return _task.IsCompleted;
				}
			}

			public CultureAwaiter(Task task)
			{
				_task = task;
			}

			public CultureAwaiter GetAwaiter()
			{
				return this;
			}

			public void GetResult()
			{
				_task.GetAwaiter().GetResult();
			}

			public void OnCompleted(Action continuation)
			{
				throw new NotImplementedException();
			}

			public void UnsafeOnCompleted(Action continuation)
			{
                CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
                CultureInfo currentUiCulture = Thread.CurrentThread.CurrentUICulture;
                _task.ConfigureAwait(false).GetAwaiter().UnsafeOnCompleted(delegate
                {
                    Thread.CurrentThread.CurrentCulture = currentCulture;
                    Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    try
                    {
                        continuation();
                    }
                    finally
                    {
                        Thread.CurrentThread.CurrentCulture = currentCulture;
                        Thread.CurrentThread.CurrentUICulture = currentUiCulture;
                    }
                });
            }
		}

		public static CultureAwaiter<T> WithCurrentCulture<T>(this Task<T> task)
		{
			return new CultureAwaiter<T>(task);
		}

		public static CultureAwaiter WithCurrentCulture(this Task task)
		{
			return new CultureAwaiter(task);
		}
	}
}
