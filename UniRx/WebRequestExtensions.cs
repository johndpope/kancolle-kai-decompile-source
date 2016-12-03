using System;
using System.IO;
using System.Net;
using System.Threading;

namespace UniRx
{
	public static class WebRequestExtensions
	{
		private static IObservable<TResult> AbortableDeferredAsyncRequest<TResult>(Func<AsyncCallback, object, IAsyncResult> begin, Func<IAsyncResult, TResult> end, WebRequest request)
		{
			return Observable.Create<TResult>(delegate(IObserver<TResult> observer)
			{
				int isCompleted = -1;
				IDisposable subscription = Observable.FromAsyncPattern<TResult>(begin, delegate(IAsyncResult ar)
				{
					TResult result;
					try
					{
						Interlocked.Increment(ref isCompleted);
						result = end.Invoke(ar);
					}
					catch (WebException ex)
					{
						if (ex.get_Status() != 6)
						{
							throw;
						}
						result = default(TResult);
					}
					return result;
				}).Invoke().Subscribe(observer);
				return Disposable.Create(delegate
				{
					if (Interlocked.Increment(ref isCompleted) == 0)
					{
						subscription.Dispose();
						request.Abort();
					}
				});
			});
		}

		public static IObservable<WebResponse> GetResponseAsObservable(this WebRequest request)
		{
			return WebRequestExtensions.AbortableDeferredAsyncRequest<WebResponse>(new Func<AsyncCallback, object, IAsyncResult>(request.BeginGetResponse), new Func<IAsyncResult, WebResponse>(request.EndGetResponse), request);
		}

		public static IObservable<HttpWebResponse> GetResponseObservableAsync(this HttpWebRequest request)
		{
			return WebRequestExtensions.AbortableDeferredAsyncRequest<HttpWebResponse>(new Func<AsyncCallback, object, IAsyncResult>(request.BeginGetResponse), (IAsyncResult ar) => (HttpWebResponse)request.EndGetResponse(ar), request);
		}

		public static IObservable<Stream> GetRequestStreamAsObservable(this WebRequest request)
		{
			return WebRequestExtensions.AbortableDeferredAsyncRequest<Stream>(new Func<AsyncCallback, object, IAsyncResult>(request.BeginGetRequestStream), new Func<IAsyncResult, Stream>(request.EndGetRequestStream), request);
		}
	}
}
