using System;
using System.Threading;

namespace UniRx
{
	public static class Observer
	{
		private class AnonymousObserver<T> : IObserver<T>
		{
			private readonly Action<T> onNext;

			private readonly Action<Exception> onError;

			private readonly Action onCompleted;

			private readonly IDisposable disposable;

			private int isStopped;

			public AnonymousObserver(Action<T> onNext, Action<Exception> onError, Action onCompleted, IDisposable disposable)
			{
				this.onNext = onNext;
				this.onError = onError;
				this.onCompleted = onCompleted;
				this.disposable = disposable;
			}

			public void OnNext(T value)
			{
				if (this.isStopped == 0)
				{
					try
					{
						this.onNext.Invoke(value);
					}
					catch
					{
						this.disposable.Dispose();
						throw;
					}
				}
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					try
					{
						this.onError.Invoke(error);
					}
					finally
					{
						this.disposable.Dispose();
					}
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					try
					{
						this.onCompleted.Invoke();
					}
					finally
					{
						this.disposable.Dispose();
					}
				}
			}
		}

		private class EmptyOnNextAnonymousObserver<T> : IObserver<T>
		{
			private readonly Action<Exception> onError;

			private readonly Action onCompleted;

			private readonly IDisposable disposable;

			private int isStopped;

			public EmptyOnNextAnonymousObserver(Action<Exception> onError, Action onCompleted, IDisposable disposable)
			{
				this.onError = onError;
				this.onCompleted = onCompleted;
				this.disposable = disposable;
			}

			public void OnNext(T value)
			{
			}

			public void OnError(Exception error)
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					try
					{
						this.onError.Invoke(error);
					}
					finally
					{
						this.disposable.Dispose();
					}
				}
			}

			public void OnCompleted()
			{
				if (Interlocked.Increment(ref this.isStopped) == 1)
				{
					try
					{
						this.onCompleted.Invoke();
					}
					finally
					{
						this.disposable.Dispose();
					}
				}
			}
		}

		public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			return Observer.Create<T>(onNext, onError, onCompleted, Disposable.Empty);
		}

		public static IObserver<T> Create<T>(Action<T> onNext, Action<Exception> onError, Action onCompleted, IDisposable disposable)
		{
			if (onNext == null)
			{
				throw new ArgumentNullException("onNext");
			}
			if (onError == null)
			{
				throw new ArgumentNullException("onError");
			}
			if (onCompleted == null)
			{
				throw new ArgumentNullException("onCompleted");
			}
			if (disposable == null)
			{
				throw new ArgumentNullException("disposable");
			}
			if (onNext == new Action<T>(Stubs.Ignore<T>))
			{
				return new Observer.EmptyOnNextAnonymousObserver<T>(onError, onCompleted, disposable);
			}
			return new Observer.AnonymousObserver<T>(onNext, onError, onCompleted, disposable);
		}
	}
}
