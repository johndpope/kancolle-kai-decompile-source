using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class ColorReactiveProperty : ReactiveProperty<Color>
	{
		public ColorReactiveProperty()
		{
		}

		public ColorReactiveProperty(Color initialValue) : base(initialValue)
		{
		}
	}
}
