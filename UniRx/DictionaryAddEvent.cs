using System;

namespace UniRx
{
	public class DictionaryAddEvent<TKey, TValue>
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

		public DictionaryAddEvent(TKey Key, TValue value)
		{
			this.Key = Key;
			this.Value = value;
		}

		public override string ToString()
		{
			return string.Format("Key:{0} Value:{1}", this.Key, this.Value);
		}
	}
}
