using System;
using System.Collections;

namespace Server_Lib
{
	public abstract class CustomCollectionBase<T> : CollectionBase
	{
		public abstract T this[int index]
		{
			get;
			set;
		}

		public abstract bool Contains(T value);

		public abstract int IndexOf(T value);

		public abstract void Insert(int index, T value);

		public abstract void Remove(T value);

		public abstract int Add(T value);

		public abstract void AddRange(T[] obj);
	}
}
