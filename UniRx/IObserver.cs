using System;

namespace UniRx
{
	public interface IObserver<TValue, TResult>
	{
		TResult OnNext(TValue value);

		TResult OnError(Exception exception);

		TResult OnCompleted();
	}
	public interface IObserver<T>
	{
		void OnCompleted();

		void OnError(Exception error);

		void OnNext(T value);
	}
}
