using System;
using UnityEngine;

namespace KCV
{
	public class UtilCurves
	{
		public static AnimationCurve TweenEaseOutBack = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 4.085954f, 4.085954f),
			new Keyframe(0.1f, 0.408828f, 3.402454f, 3.402454f),
			new Keyframe(0.2f, 0.705802f, 2.398849f, 2.398849f),
			new Keyframe(0.3f, 0.907132f, 1.650712f, 1.650712f),
			new Keyframe(0.4f, 1.029027f, 0.809279f, 0.809279f),
			new Keyframe(0.5f, 1.087698f, 0.212121f, 0.212121f),
			new Keyframe(0.6f, 1.099352f, 0f, 0f),
			new Keyframe(0.7f, 1.0802f, -0.191919f, -0.191919f),
			new Keyframe(0.8f, 1.046451f, -0.360899f, -0.360899f),
			new Keyframe(0.9f, 1.014314f, 0f, 0f),
			new Keyframe(1f, 1f, 0f, 0f)
		});

		public static AnimationCurve TweenEaseInOutQuad = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 0.2f, 0.2f),
			new Keyframe(0.1f, 0.02f, 0.4f, 0.4f),
			new Keyframe(0.2f, 0.08f, 0.8f, 0.8f),
			new Keyframe(0.3f, 0.18f, 1.2f, 1.2f),
			new Keyframe(0.4f, 0.32f, 1.6f, 1.6f),
			new Keyframe(0.5f, 0.5f, 1.8f, 1.8f),
			new Keyframe(0.6f, 0.68f, 1.6f, 1.6f),
			new Keyframe(0.7f, 0.82f, 1.2f, 1.2f),
			new Keyframe(0.8f, 0.92f, 0.8f, 0.8f),
			new Keyframe(0.9f, 0.98f, 0.4f, 0.4f),
			new Keyframe(1f, 1f, 0.2f, 0.2f)
		});

		public static AnimationCurve TweenEaseOutExpo = new AnimationCurve(new Keyframe[]
		{
			new Keyframe(0f, 0f, 5f, 5f),
			new Keyframe(0.1f, 0.5f, 3.75f, 3.75f),
			new Keyframe(0.2f, 0.75f, 1.875f, 1.875f),
			new Keyframe(0.3f, 0.875f, 0.9375f, 0.9375f),
			new Keyframe(0.4f, 0.9375f, 0.46875f, 0.46875f),
			new Keyframe(0.5f, 0.96875f, 0.234375f, 0.234375f),
			new Keyframe(0.6f, 0.984375f, 0.11719f, 0.11719f),
			new Keyframe(0.7f, 0.992188f, 0.058595f, 0.058595f),
			new Keyframe(0.8f, 0.996094f, 0.029295f, 0.029295f),
			new Keyframe(0.9f, 0.998047f, 0.01953f, 0.01953f),
			new Keyframe(1f, 1f, 0f, 0f)
		});
	}
}
