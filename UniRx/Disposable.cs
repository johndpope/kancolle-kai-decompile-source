using System;

namespace UniRx
{
	public static class Disposable
	{
		private class EmptyDisposable : IDisposable
		{
			public static Disposable.EmptyDisposable Singleton = new Disposable.EmptyDisposable();

			private EmptyDisposable()
			{
			}

			public void Dispose()
			{
			}
		}

		private class AnonymousDisposable : IDisposable
		{
			private bool isDisposed;

			private readonly Action dispose;

			public AnonymousDisposable(Action dispose)
			{
				this.dispose = dispose;
			}

			public void Dispose()
			{
				if (!this.isDisposed)
				{
					this.isDisposed = true;
					this.dispose.Invoke();
				}
			}
		}

		public static readonly IDisposable Empty = Disposable.EmptyDisposable.Singleton;

		public static IDisposable Create(Action disposeAction)
		{
			return new Disposable.AnonymousDisposable(disposeAction);
		}
	}
}
