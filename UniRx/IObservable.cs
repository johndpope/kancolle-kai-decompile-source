using System;

namespace UniRx
{
	public interface IObservable<T>
	{
		IDisposable Subscribe(IObserver<T> observer);
	}
}
