using System;

namespace UniRx
{
	[Serializable]
	public class DoubleReactiveProperty : ReactiveProperty<double>
	{
		public DoubleReactiveProperty()
		{
		}

		public DoubleReactiveProperty(double initialValue) : base(initialValue)
		{
		}
	}
}
