using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class BoundsReactiveProperty : ReactiveProperty<Bounds>
	{
		public BoundsReactiveProperty()
		{
		}

		public BoundsReactiveProperty(Bounds initialValue) : base(initialValue)
		{
		}
	}
}
