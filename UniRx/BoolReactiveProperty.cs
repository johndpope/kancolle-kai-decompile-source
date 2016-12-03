using System;

namespace UniRx
{
	[Serializable]
	public class BoolReactiveProperty : ReactiveProperty<bool>
	{
		public BoolReactiveProperty()
		{
		}

		public BoolReactiveProperty(bool initialValue) : base(initialValue)
		{
		}
	}
}
