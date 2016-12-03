using System;
using UnityEngine;

namespace UniRx
{
	[Serializable]
	public class AnimationCurveReactiveProperty : ReactiveProperty<AnimationCurve>
	{
		public AnimationCurveReactiveProperty()
		{
		}

		public AnimationCurveReactiveProperty(AnimationCurve initialValue) : base(initialValue)
		{
		}
	}
}
