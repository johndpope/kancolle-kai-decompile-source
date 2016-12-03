using System;

namespace UniRx
{
	internal static class Stubs
	{
		public static readonly Action Nop = delegate
		{
		};

		public static readonly Action<Exception> Throw = delegate(Exception ex)
		{
			throw ex;
		};

		public static void Ignore<T>(T t)
		{
		}

		public static IObservable<TSource> CatchIgnore<TSource>(Exception ex)
		{
			return Observable.Empty<TSource>();
		}
	}
}
