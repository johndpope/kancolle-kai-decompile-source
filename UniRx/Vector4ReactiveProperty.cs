using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class Vector4ReactiveProperty : ReactiveProperty<Vector4>
	{
		public Vector4ReactiveProperty()
		{
		}

		public Vector4ReactiveProperty(Vector4 initialValue) : base(initialValue)
		{
		}
	}
}
