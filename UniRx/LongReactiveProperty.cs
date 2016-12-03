using System;

namespace UniRx
{
	[Serializable]
	public class LongReactiveProperty : ReactiveProperty<long>
	{
		public LongReactiveProperty()
		{
		}

		public LongReactiveProperty(long initialValue) : base(initialValue)
		{
		}
	}
}
