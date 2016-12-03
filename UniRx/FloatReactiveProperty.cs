using System;

namespace UniRx
{
	[Serializable]
	public class FloatReactiveProperty : ReactiveProperty<float>
	{
		public FloatReactiveProperty()
		{
		}

		public FloatReactiveProperty(float initialValue) : base(initialValue)
		{
		}
	}
}
