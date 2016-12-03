using System;

namespace UniRx
{
	public class CollectionAddEvent<T>
	{
		public int Index
		{
			get;
			private set;
		}

		public T Value
		{
			get;
			private set;
		}

		public CollectionAddEvent(int index, T value)
		{
			this.Index = index;
			this.Value = value;
		}

		public override string ToString()
		{
			return string.Format("Index:{0} Value:{1}", this.Index, this.Value);
		}
	}
}
