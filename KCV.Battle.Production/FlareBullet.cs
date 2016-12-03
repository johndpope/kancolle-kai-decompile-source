using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode, RequireComponent(typeof(LensFlare))]
	public class FlareBullet : MonoBehaviour
	{
		[Serializable]
		public class Params
		{
			[Range(0f, 50f)]
			public float _noiseSpeed = 8f;

			public float _fallLength = 20f;

			public float _delay;

			public Color _flareColor = Color.get_white();
		}

		[Serializable]
		public struct Anim
		{
			[Range(0f, 1f)]
			public float _shine;

			public float _y;
		}

		[Serializable]
		public struct Binding
		{
			public ParticleSystem _smoke;

			public LensFlare _flare;

			public Animation _animation;
		}

		public FlareBullet.Params _params = new FlareBullet.Params();

		public FlareBullet.Anim _anim = default(FlareBullet.Anim);

		public FlareBullet.Binding _binding;

		private float _outputPower;

		private float _moveRandomSeed;

		private bool _isAnimating;

		public float OutputPower
		{
			get
			{
				return this._outputPower;
			}
		}

		private static float remap(float value, float inputMin, float inputMax, float outputMin, float outputMax, bool isClamp)
		{
			if (isClamp)
			{
				value = Mathf.Clamp(value, inputMin, inputMax);
			}
			return (value - inputMin) * ((outputMax - outputMin) / (inputMax - inputMin)) + outputMin;
		}

		private void OnDestroy()
		{
			Mem.Del<FlareBullet.Binding>(ref this._binding);
			Mem.Del<FlareBullet.Anim>(ref this._anim);
			Mem.Del<FlareBullet.Params>(ref this._params);
		}

		private void Start()
		{
			this._moveRandomSeed = Random.get_value() * 100f;
			this._binding._smoke.set_enableEmission(false);
			this._anim._shine = 0f;
		}

		private void Update()
		{
			float time = Time.get_time();
			float value = 1f - Mathf.Pow(Mathf.PerlinNoise(time * this._params._noiseSpeed, this._moveRandomSeed), 2f);
			float num = FlareBullet.remap(value, 0f, 1f, 0.6f, 1f, false);
			this._outputPower = this._anim._shine * num;
			this._binding._flare.set_color(this._params._flareColor * this._outputPower);
			this._binding._flare.set_brightness(FlareBullet.remap(this._outputPower, 0f, 1f, 0.6f, 1f, true));
			float num2 = 1.5f;
			float num3 = -this._anim._y * this._params._fallLength;
			float num4 = FlareBullet.remap(Mathf.PerlinNoise(this._anim._y * 2f, this._moveRandomSeed), 0f, 1f, -1f, 1f, false) * num2;
			base.get_transform().set_localPosition(new Vector3(num4, num3, 0f));
		}

		[DebuggerHidden]
		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			FlareBullet.<AnimationCoroutine>c__IteratorE3 <AnimationCoroutine>c__IteratorE = new FlareBullet.<AnimationCoroutine>c__IteratorE3();
			<AnimationCoroutine>c__IteratorE.observer = observer;
			<AnimationCoroutine>c__IteratorE.<$>observer = observer;
			<AnimationCoroutine>c__IteratorE.<>f__this = this;
			return <AnimationCoroutine>c__IteratorE;
		}

		public IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
		}
	}
}
