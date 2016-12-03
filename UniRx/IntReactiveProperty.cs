using System;

namespace UniRx
{
	[Serializable]
	public class IntReactiveProperty : ReactiveProperty<int>
	{
		public IntReactiveProperty()
		{
		}

		public IntReactiveProperty(int initialValue) : base(initialValue)
		{
		}
	}
}
