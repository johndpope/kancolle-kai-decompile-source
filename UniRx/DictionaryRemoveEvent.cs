using System;

namespace UniRx
{
	public class DictionaryRemoveEvent<TKey, TValue>
	{
		public TKey Key
		{
			get;
			private set;
		}

		public TValue Value
		{
			get;
			private set;
		}

		public DictionaryRemoveEvent(TKey key, TValue value)
		{
			this.Key = key;
			this.Value = value;
		}

		public override string ToString()
		{
			return string.Format("Key:{0} Value:{1}", this.Key, this.Value);
		}
	}
}
