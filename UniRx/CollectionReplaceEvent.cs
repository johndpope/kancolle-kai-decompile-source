using System;

namespace UniRx
{
	public class CollectionReplaceEvent<T>
	{
		public int Index
		{
			get;
			private set;
		}

		public T OldValue
		{
			get;
			private set;
		}

		public T NewValue
		{
			get;
			private set;
		}

		public CollectionReplaceEvent(int index, T oldValue, T newValue)
		{
			this.Index = index;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public override string ToString()
		{
			return string.Format("Index:{0} OldValue:{1} NewValue:{2}", this.Index, this.OldValue, this.NewValue);
		}
	}
}
