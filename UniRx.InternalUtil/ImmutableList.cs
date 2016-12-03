using System;

namespace UniRx.InternalUtil
{
	public class ImmutableList<T>
	{
		private T[] data;

		public T[] Data
		{
			get
			{
				return this.data;
			}
		}

		public ImmutableList()
		{
			this.data = new T[0];
		}

		public ImmutableList(T[] data)
		{
			this.data = data;
		}

		public ImmutableList<T> Add(T value)
		{
			T[] array = new T[this.data.Length + 1];
			Array.Copy(this.data, array, this.data.Length);
			array[this.data.Length] = value;
			return new ImmutableList<T>(array);
		}

		public ImmutableList<T> Remove(T value)
		{
			int num = this.IndexOf(value);
			if (num < 0)
			{
				return this;
			}
			T[] array = new T[this.data.Length - 1];
			Array.Copy(this.data, 0, array, 0, num);
			Array.Copy(this.data, num + 1, array, num, this.data.Length - num - 1);
			return new ImmutableList<T>(array);
		}

		public int IndexOf(T value)
		{
			for (int i = 0; i < this.data.Length; i++)
			{
				if (this.data[i].Equals(value))
				{
					return i;
				}
			}
			return -1;
		}
	}
}
