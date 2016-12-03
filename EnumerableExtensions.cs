using System;
using System.Collections;
using System.Collections.Generic;

public static class EnumerableExtensions
{
	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		using (IEnumerator<T> enumerator = enumerable.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				T current = enumerator.get_Current();
				action.Invoke(current);
			}
		}
	}

	public static void ForEach(this IEnumerable enumerable, Action<object> action)
	{
		using (IEnumerator enumerator = enumerable.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				object current = enumerator.get_Current();
				action.Invoke(current);
			}
		}
	}
}
