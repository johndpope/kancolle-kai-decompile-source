using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class TorpedoStraightController : MonoBehaviour
	{
		[Serializable]
		public class Params
		{
			public Vector3 beginPivot;

			public Vector3 targetPivot;

			public float gazeHeight = 5f;

			public float targetCameraBackLength = 10f;

			public float quakePower = 1f;

			public float quakeSpeed = 10f;
		}

		[Serializable]
		public struct Anim
		{
			public bool isEval;

			public float z_progress;

			public float y;

			public float pitch_progress;

			public float quake;
		}

		private static readonly string AnimationClipName = "TorpedoStraight";

		public bool _isAnimating;

		private float _tilt;

		private float _animationBegan;

		private Subject<int> _flyingFinishSubject2F = new Subject<int>();

		public TorpedoStraightController.Anim _anim;

		public Animation _animation;

		public TorpedoStraightController.Params _params = new TorpedoStraightController.Params();

		public Transform _referenceCameraTransform;

		public Transform ReferenceCameraTransform
		{
			get
			{
				return this._referenceCameraTransform;
			}
			set
			{
				this._referenceCameraTransform = value;
			}
		}

		public Vector3 BeginPivot
		{
			get
			{
				return this._params.beginPivot;
			}
			set
			{
				this._params.beginPivot = value;
				this._params.beginPivot.y = 0f;
			}
		}

		public Vector3 TargetPivot
		{
			get
			{
				return this._params.targetPivot;
			}
			set
			{
				this._params.targetPivot = value;
				this._params.targetPivot.y = 0f;
			}
		}

		public IObservable<int> FlyingFinish2F
		{
			get
			{
				return this._flyingFinishSubject2F;
			}
		}

		public Vector3 CameraEndPosition
		{
			get
			{
				Vector3 normalized = (this.BeginPivot - this.TargetPivot).get_normalized();
				return this.TargetPivot + normalized * this._params.targetCameraBackLength;
			}
		}

		public Vector3 LookDir
		{
			get
			{
				return (this.TargetPivot - this.BeginPivot).get_normalized();
			}
		}

		public Vector3 RightDir
		{
			get
			{
				return Vector3.Cross(Vector3.get_up(), this.LookDir);
			}
		}

		private void OnValidate()
		{
			this._params.beginPivot.y = 0f;
			this._params.targetPivot.y = 0f;
		}

		private void MakeCameraTransform(out Vector3 position, out Quaternion rotation)
		{
			Vector3 vector = Vector3.Lerp(this.BeginPivot, this.CameraEndPosition, this._anim.z_progress);
			position = vector + Vector3.get_up() * this._params.gazeHeight * this._anim.y;
			Vector3 vector2 = Vector3.Slerp(Vector3.get_down(), this.LookDir, this._anim.pitch_progress);
			Vector3 vector3 = Vector3.Cross(vector2, this.RightDir);
			rotation = Quaternion.LookRotation(vector2, vector3);
		}

		private void LateUpdate()
		{
			if (!this._isAnimating)
			{
				return;
			}
			if (this._anim.isEval && this._referenceCameraTransform != null)
			{
				Vector3 position;
				Quaternion rotation;
				this.MakeCameraTransform(out position, out rotation);
				this._referenceCameraTransform.set_position(position);
				this._referenceCameraTransform.set_rotation(rotation);
				this._referenceCameraTransform.Rotate(this._referenceCameraTransform.get_right(), 1f);
				this._referenceCameraTransform.Rotate(this._referenceCameraTransform.get_forward(), this._tilt);
				float time = Time.get_time();
				float num = time - this._animationBegan;
				float num2 = (Mathf.PerlinNoise(num * this._params.quakeSpeed, 0f) * 2f - 1f) * this._params.quakePower * this._anim.quake;
				float num3 = (Mathf.PerlinNoise(num * this._params.quakeSpeed, 10f) * 2f - 1f) * this._params.quakePower * this._anim.quake;
				Transform expr_11A = this._referenceCameraTransform.get_transform();
				expr_11A.set_position(expr_11A.get_position() + (this._referenceCameraTransform.get_transform().get_right() * num2 + this._referenceCameraTransform.get_transform().get_up() * num3));
			}
		}

		private void Start()
		{
			this._tilt = ((Random.get_value() <= 0.5f) ? -2f : 2f);
		}

		private void OnDestroy()
		{
			this._flyingFinishSubject2F.OnCompleted();
		}

		[DebuggerHidden]
		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			TorpedoStraightController.<AnimationCoroutine>c__IteratorF3 <AnimationCoroutine>c__IteratorF = new TorpedoStraightController.<AnimationCoroutine>c__IteratorF3();
			<AnimationCoroutine>c__IteratorF.observer = observer;
			<AnimationCoroutine>c__IteratorF.<$>observer = observer;
			<AnimationCoroutine>c__IteratorF.<>f__this = this;
			return <AnimationCoroutine>c__IteratorF;
		}

		public IObservable<int> PlayAnimation()
		{
			return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
		}

		private void FlyingFinishEvent()
		{
			this._flyingFinishSubject2F.OnNext(0);
		}
	}
}
