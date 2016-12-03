using System;

namespace UniRx
{
	[Serializable]
	public class StringReactiveProperty : ReactiveProperty<string>
	{
		public StringReactiveProperty()
		{
		}

		public StringReactiveProperty(string initialValue) : base(initialValue)
		{
		}
	}
}
