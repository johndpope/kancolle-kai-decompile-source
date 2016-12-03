using System;

namespace UniRx
{
	public static class ObservableExtensions
	{
		public static IDisposable Subscribe<T>(this IObservable<T> source)
		{
			return source.Subscribe(Observer.Create<T>(new Action<T>(Stubs.Ignore<T>), Stubs.Throw, Stubs.Nop));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext)
		{
			return source.Subscribe(Observer.Create<T>(onNext, Stubs.Throw, Stubs.Nop));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError)
		{
			return source.Subscribe(Observer.Create<T>(onNext, onError, Stubs.Nop));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action onCompleted)
		{
			return source.Subscribe(Observer.Create<T>(onNext, Stubs.Throw, onCompleted));
		}

		public static IDisposable Subscribe<T>(this IObservable<T> source, Action<T> onNext, Action<Exception> onError, Action onCompleted)
		{
			return source.Subscribe(Observer.Create<T>(onNext, onError, onCompleted));
		}
	}
}
