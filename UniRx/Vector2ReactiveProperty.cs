using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class Vector2ReactiveProperty : ReactiveProperty<Vector2>
	{
		public Vector2ReactiveProperty()
		{
		}

		public Vector2ReactiveProperty(Vector2 initialValue) : base(initialValue)
		{
		}
	}
}
