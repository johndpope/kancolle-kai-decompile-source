using System;
using System.Collections.Generic;

namespace UniRx
{
	public static class ReactivePropertyExtensions
	{
		public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source)
		{
			return new ReactiveProperty<T>(source);
		}

		public static ReactiveProperty<T> ToReactiveProperty<T>(this IObservable<T> source, T initialValue)
		{
			return new ReactiveProperty<T>(source, initialValue);
		}

		public static ReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source)
		{
			return new ReadOnlyReactiveProperty<T>(source);
		}

		public static ReadOnlyReactiveProperty<T> ToReadOnlyReactiveProperty<T>(this IObservable<T> source, T initialValue)
		{
			return new ReadOnlyReactiveProperty<T>(source, initialValue);
		}

		public static IObservable<bool> CombineLatestValuesAreAllTrue(this IEnumerable<IObservable<bool>> sources)
		{
			return sources.CombineLatest<bool>().Select(delegate(IList<bool> xs)
			{
				using (IEnumerator<bool> enumerator = xs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (!enumerator.get_Current())
						{
							return false;
						}
					}
				}
				return true;
			});
		}

		public static IObservable<bool> CombineLatestValuesAreAllFalse(this IEnumerable<IObservable<bool>> sources)
		{
			return sources.CombineLatest<bool>().Select(delegate(IList<bool> xs)
			{
				using (IEnumerator<bool> enumerator = xs.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						bool current = enumerator.get_Current();
						if (current)
						{
							return false;
						}
					}
				}
				return true;
			});
		}
	}
}
