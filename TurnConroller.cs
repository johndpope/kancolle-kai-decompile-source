using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

public class TurnConroller : MonoBehaviour
{
	[Serializable]
	public struct Anim
	{
		public bool isEval;

		public float z_progress;

		public float rotation_progress;
	}

	public Transform _referenceCameraTransform;

	public Vector3 _cameraBeginPivot;

	public Vector3 _cameraEndPivot;

	public TurnConroller.Anim _anim;

	public Animation _animation;

	private bool _isAnimating;

	private Quaternion? _beginRotation;

	public bool _isCCW;

	public bool _launchLoop;

	private Subject<int> _explosionTargetSubject = new Subject<int>();

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

	public Vector3 CameraBeginPivot
	{
		get
		{
			return this._cameraBeginPivot;
		}
		set
		{
			this._cameraBeginPivot = value;
		}
	}

	public Vector3 CameraEndPivot
	{
		get
		{
			return this._cameraEndPivot;
		}
		set
		{
			this._cameraEndPivot = value;
		}
	}

	public bool IsCCW
	{
		get
		{
			return this._isCCW;
		}
		set
		{
			this._isCCW = value;
		}
	}

	public IObservable<int> ExplosionTargetObservable
	{
		get
		{
			return this._explosionTargetSubject;
		}
	}

	private Vector3 DirectionXZ
	{
		get
		{
			Vector3 vector = this.CameraEndPivot - this.CameraBeginPivot;
			Vector3 vector2 = new Vector3(vector.x, 0f, vector.z);
			return vector2.get_normalized();
		}
	}

	private void Start()
	{
		if (this._launchLoop)
		{
			this.PlayAnimation().Delay(TimeSpan.FromSeconds(0.5)).Repeat<int>().Subscribe(delegate(int _)
			{
			});
		}
	}

	private void OnDestroy()
	{
		this._explosionTargetSubject.OnCompleted();
	}

	private void LateUpdate()
	{
		if (this._anim.isEval && this._referenceCameraTransform != null)
		{
			Vector3 position;
			Quaternion rotation;
			this.ApplyTransform(out position, out rotation);
			this._referenceCameraTransform.set_position(position);
			this._referenceCameraTransform.set_rotation(rotation);
		}
	}

	private void ApplyTransform(out Vector3 position, out Quaternion rotation)
	{
		Vector3 cameraBeginPivot = this.CameraBeginPivot;
		Vector3 cameraEndPivot = this.CameraEndPivot;
		Vector3 vector = Vector3.Lerp(cameraBeginPivot, cameraEndPivot, this._anim.z_progress);
		position = vector;
		float num = 180f;
		float num2 = Mathf.Lerp(0f, (!this._isCCW) ? num : (-num), this._anim.rotation_progress);
		rotation = ((!this._beginRotation.get_HasValue()) ? Quaternion.get_identity() : this._beginRotation.get_Value()) * Quaternion.AngleAxis(num2, Vector3.get_up());
	}

	private void ExplosionTargetEvent()
	{
		this._explosionTargetSubject.OnNext(0);
	}

	[DebuggerHidden]
	private IEnumerator AnimationCoroutine(IObserver<int> observer)
	{
		TurnConroller.<AnimationCoroutine>c__Iterator23 <AnimationCoroutine>c__Iterator = new TurnConroller.<AnimationCoroutine>c__Iterator23();
		<AnimationCoroutine>c__Iterator.observer = observer;
		<AnimationCoroutine>c__Iterator.<$>observer = observer;
		<AnimationCoroutine>c__Iterator.<>f__this = this;
		return <AnimationCoroutine>c__Iterator;
	}

	public IObservable<int> PlayAnimation()
	{
		return Observable.FromCoroutine<int>((IObserver<int> observer) => this.AnimationCoroutine(observer));
	}
}
