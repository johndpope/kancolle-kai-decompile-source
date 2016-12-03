using System;

namespace UniRx
{
	[Serializable]
	public class ByteReactiveProperty : ReactiveProperty<byte>
	{
		public ByteReactiveProperty()
		{
		}

		public ByteReactiveProperty(byte initialValue) : base(initialValue)
		{
		}
	}
}
