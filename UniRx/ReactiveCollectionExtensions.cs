using System;
using System.Collections.Generic;

namespace UniRx
{
	public static class ReactiveCollectionExtensions
	{
		public static ReactiveCollection<T> ToReactiveCollection<T>(this IEnumerable<T> source)
		{
			return new ReactiveCollection<T>(source);
		}
	}
}
