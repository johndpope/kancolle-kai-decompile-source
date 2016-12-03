using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class QuaternionReactiveProperty : ReactiveProperty<Quaternion>
	{
		public QuaternionReactiveProperty()
		{
		}

		public QuaternionReactiveProperty(Quaternion initialValue) : base(initialValue)
		{
		}
	}
}
