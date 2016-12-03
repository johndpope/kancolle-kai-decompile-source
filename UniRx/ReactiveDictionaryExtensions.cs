using System;
using System.Collections.Generic;

namespace UniRx
{
	public static class ReactiveDictionaryExtensions
	{
		public static ReactiveDictionary<TKey, TValue> ToReactiveDictionary<TKey, TValue>(this Dictionary<TKey, TValue> dictionary)
		{
			return new ReactiveDictionary<TKey, TValue>(dictionary);
		}
	}
}
