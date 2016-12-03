using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace UniRx
{
	public static class AotSafeExtensions
	{
		[DebuggerHidden]
		public static IEnumerable<T> AsSafeEnumerable<T>(this IEnumerable<T> source)
		{
			AotSafeExtensions.<AsSafeEnumerable>c__Iterator19<T> <AsSafeEnumerable>c__Iterator = new AotSafeExtensions.<AsSafeEnumerable>c__Iterator19<T>();
			<AsSafeEnumerable>c__Iterator.source = source;
			<AsSafeEnumerable>c__Iterator.<$>source = source;
			AotSafeExtensions.<AsSafeEnumerable>c__Iterator19<T> expr_15 = <AsSafeEnumerable>c__Iterator;
			expr_15.$PC = -2;
			return expr_15;
		}

		public static IObservable<Tuple<T>> WrapValueToClass<T>(this IObservable<T> source) where T : struct
		{
			int dummy = 0;
			return Observable.Create<Tuple<T>>((IObserver<Tuple<T>> observer) => source.Subscribe(Observer.Create<T>(delegate(T x)
			{
				dummy.GetHashCode();
				Tuple<T> value = new Tuple<T>(x);
				observer.OnNext(value);
			}, new Action<Exception>(observer.OnError), new Action(observer.OnCompleted))));
		}

		[DebuggerHidden]
		public static IEnumerable<Tuple<T>> WrapValueToClass<T>(this IEnumerable<T> source) where T : struct
		{
			AotSafeExtensions.<WrapValueToClass>c__Iterator1A<T> <WrapValueToClass>c__Iterator1A = new AotSafeExtensions.<WrapValueToClass>c__Iterator1A<T>();
			<WrapValueToClass>c__Iterator1A.source = source;
			<WrapValueToClass>c__Iterator1A.<$>source = source;
			AotSafeExtensions.<WrapValueToClass>c__Iterator1A<T> expr_15 = <WrapValueToClass>c__Iterator1A;
			expr_15.$PC = -2;
			return expr_15;
		}
	}
}
