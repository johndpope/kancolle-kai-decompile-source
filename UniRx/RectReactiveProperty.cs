using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class RectReactiveProperty : ReactiveProperty<Rect>
	{
		public RectReactiveProperty()
		{
		}

		public RectReactiveProperty(Rect initialValue) : base(initialValue)
		{
		}
	}
}
