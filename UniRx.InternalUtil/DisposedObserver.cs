using System;

namespace UniRx.InternalUtil
{
	public class DisposedObserver<T> : IObserver<T>
	{
		public void OnCompleted()
		{
			throw new ObjectDisposedException(string.Empty);
		}

		public void OnError(Exception error)
		{
			throw new ObjectDisposedException(string.Empty);
		}

		public void OnNext(T value)
		{
			throw new ObjectDisposedException(string.Empty);
		}
	}
}
