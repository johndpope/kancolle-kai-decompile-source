using System;

namespace UniRx
{
	public class DictionaryReplaceEvent<TKey, TValue>
	{
		public TKey Key
		{
			get;
			private set;
		}

		public TValue OldValue
		{
			get;
			private set;
		}

		public TValue NewValue
		{
			get;
			private set;
		}

		public DictionaryReplaceEvent(TKey key, TValue oldValue, TValue newValue)
		{
			this.Key = key;
			this.OldValue = oldValue;
			this.NewValue = newValue;
		}

		public override string ToString()
		{
			return string.Format("Key:{0} OldValue:{1} NewValue:{2}", this.Key, this.OldValue, this.NewValue);
		}
	}
}
