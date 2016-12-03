using KCV.Utils;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle.Production
{
	[ExecuteInEditMode]
	public class FlareBulletSceneController : MonoBehaviour
	{
		[Serializable]
		public struct FlareBulletFire
		{
			public bool isActive;

			public float z_progress;

			public float xy_progress;

			public bool isSpinMode;

			public float spinmove_z_progress;

			public float spinrotation_progress;
		}

		private Vector3 _friendFrontDirection = Vector3.get_back();

		public Transform _referenceCameraTransform;

		public Vector3 _flareBulletCameraStartPivot;

		public Vector3 _flareBulletFirePivot;

		public Vector3 _flareBulletEnemyCameraPivot;

		public float _focusDistance = 5f;

		public FlareBulletSceneController.FlareBulletFire _flareBulletFire;

		public FlareBullet _flareBullet1;

		public FlareBullet _flareBullet2;

		private Animation _animation;

		private bool _isAnimating;

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

		public Vector3 FlareBulletCameraStartPivot
		{
			get
			{
				return this._flareBulletCameraStartPivot;
			}
			set
			{
				this._flareBulletCameraStartPivot = value;
			}
		}

		public Vector3 FlareBulletFirePivot
		{
			get
			{
				return this._flareBulletFirePivot;
			}
			set
			{
				this._flareBulletFirePivot = value;
			}
		}

		public Vector3 FlareBulletEnemyCameraPivot
		{
			get
			{
				return this._flareBulletEnemyCameraPivot;
			}
			set
			{
				this._flareBulletEnemyCameraPivot = value;
			}
		}

		private Vector3 FlareBulletFocusCameraPoint
		{
			get
			{
				return this._flareBulletFirePivot + this._friendFrontDirection * this._focusDistance;
			}
		}

		private void Start()
		{
			this._animation = base.GetComponent<Animation>();
			this._flareBulletFire = default(FlareBulletSceneController.FlareBulletFire);
		}

		private void OnDestroy()
		{
			Mem.Del<Vector3>(ref this._friendFrontDirection);
			Mem.Del<Transform>(ref this._referenceCameraTransform);
			Mem.Del<Vector3>(ref this._flareBulletCameraStartPivot);
			Mem.Del<Vector3>(ref this._flareBulletFirePivot);
			Mem.Del<Vector3>(ref this._flareBulletEnemyCameraPivot);
			Mem.Del<float>(ref this._focusDistance);
			Mem.Del<FlareBulletSceneController.FlareBulletFire>(ref this._flareBulletFire);
			Mem.Del<FlareBullet>(ref this._flareBullet1);
			Mem.Del<FlareBullet>(ref this._flareBullet2);
			Mem.Del<Animation>(ref this._animation);
			Mem.Del<bool>(ref this._isAnimating);
		}

		private void LateUpdate()
		{
			if (this._flareBulletFire.isActive)
			{
				if (!this._flareBulletFire.isSpinMode)
				{
					Vector3 flareBulletCameraStartPivot = this._flareBulletCameraStartPivot;
					Vector3 flareBulletFocusCameraPoint = this.FlareBulletFocusCameraPoint;
					Vector2 vector = Vector2.Lerp(new Vector2(flareBulletCameraStartPivot.x, flareBulletCameraStartPivot.y), new Vector2(flareBulletFocusCameraPoint.x, flareBulletFocusCameraPoint.y), this._flareBulletFire.xy_progress);
					float num = Mathf.Lerp(flareBulletCameraStartPivot.z, flareBulletFocusCameraPoint.z, this._flareBulletFire.z_progress);
					this._referenceCameraTransform.set_position(new Vector3(vector.x, vector.y, num));
				}
				else
				{
					Vector3 flareBulletFocusCameraPoint2 = this.FlareBulletFocusCameraPoint;
					Vector3 flareBulletEnemyCameraPivot = this._flareBulletEnemyCameraPivot;
					Vector3 position = Vector3.Lerp(flareBulletFocusCameraPoint2, flareBulletEnemyCameraPivot, this._flareBulletFire.spinmove_z_progress);
					this._referenceCameraTransform.set_position(position);
				}
				float num2 = Mathf.Lerp(0f, 180f, this._flareBulletFire.spinrotation_progress);
				this._referenceCameraTransform.set_eulerAngles(new Vector3(0f, num2, 0f));
			}
		}

		private void FireFlareBullet()
		{
			SoundUtils.PlaySE(SEFIleInfos.SE_055);
			this._flareBullet1.PlayAnimation().Subscribe(delegate(int _)
			{
			});
			this._flareBullet2.PlayAnimation().Subscribe(delegate(int _)
			{
			});
		}

		[DebuggerHidden]
		private IEnumerator AnimationCoroutine(IObserver<int> observer)
		{
			FlareBulletSceneController.<AnimationCoroutine>c__IteratorE4 <AnimationCoroutine>c__IteratorE = new FlareBulletSceneController.<AnimationCoroutine>c__IteratorE4();
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
