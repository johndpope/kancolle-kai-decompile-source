using System;

namespace UniRx
{
	public class CollectionMoveEvent<T>
	{
		public int OldIndex
		{
			get;
			private set;
		}

		public int NewIndex
		{
			get;
			private set;
		}

		public T Value
		{
			get;
			private set;
		}

		public CollectionMoveEvent(int oldIndex, int newIndex, T value)
		{
			this.OldIndex = oldIndex;
			this.NewIndex = newIndex;
			this.Value = value;
		}

		public override string ToString()
		{
			return string.Format("OldIndex:{0} NewIndex:{1} Value:{2}", this.OldIndex, this.NewIndex, this.Value);
		}
	}
}
