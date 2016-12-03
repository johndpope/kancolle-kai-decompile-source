using System;

namespace UniRx
{
	public interface IConnectableObservable<T> : IObservable<T>
	{
		IDisposable Connect();
	}
}
