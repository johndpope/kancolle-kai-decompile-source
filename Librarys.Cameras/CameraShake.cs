using System;
using UniRx;
using UnityEngine;

namespace Librarys.Cameras
{
	[RequireComponent(typeof(Camera))]
	public class CameraShake : MonoBehaviour
	{
		private bool _isShake;

		private bool _isShakePos;

		private bool _isShakeRot;

		private float _fShakeIntensity;

		private float _fDelayTimer;

		private Vector3 _vOriginPosition;

		private Quaternion _quaOriginRotation;

		private Action _actCallback;

		[SerializeField]
		private float _fShakeDecay = 0.002f;

		[SerializeField]
		private float _fCoefShakeIntensity = 0.1f;

		public bool isShake
		{
			get
			{
				return this._isShake;
			}
		}

		public float shakeDecay
		{
			get
			{
				return this._fShakeDecay;
			}
			set
			{
				if (value != this._fShakeDecay)
				{
					this._fShakeDecay = value;
				}
			}
		}

		public float coefShakeIntensity
		{
			get
			{
				return this._fCoefShakeIntensity;
			}
			set
			{
				if (value != this._fCoefShakeIntensity)
				{
					this._fCoefShakeIntensity = value;
				}
			}
		}

		public Vector3 originPosition
		{
			get
			{
				return this._vOriginPosition;
			}
		}

		private Quaternion originRotation
		{
			get
			{
				return this._quaOriginRotation;
			}
		}

		private void FixedUpdate()
		{
			if (this._isShake)
			{
				if (this._fShakeIntensity > 0f)
				{
					if (this._isShakePos)
					{
						base.get_transform().set_position(this._vOriginPosition + XorRandom.GetInsideUnitSphere(this._fShakeIntensity));
					}
					if (this._isShakeRot)
					{
						base.get_transform().set_rotation(new Quaternion(this._quaOriginRotation.x + XorRandom.GetFLim(-this._fShakeIntensity, this._fShakeIntensity) * 2f, this._quaOriginRotation.y + XorRandom.GetFLim(-this._fShakeIntensity, this._fShakeIntensity) * 2f, this._quaOriginRotation.z + XorRandom.GetFLim(-this._fShakeIntensity, this._fShakeIntensity) * 2f, this._quaOriginRotation.w + XorRandom.GetFLim(-this._fShakeIntensity, this._fShakeIntensity) * 2f));
					}
					this._fShakeIntensity -= this._fShakeDecay;
				}
				else
				{
					Observable.TimerFrame(2, FrameCountType.EndOfFrame).Subscribe(delegate(long _)
					{
						this._isShake = false;
						if (this._actCallback != null)
						{
							this._actCallback.Invoke();
						}
					});
				}
			}
		}

		public void Shake()
		{
			this.Shake(null);
		}

		public void Shake(Action callback)
		{
			if (this._isShake)
			{
				return;
			}
			this._isShakePos = true;
			this._isShakeRot = true;
			this._vOriginPosition = base.get_transform().get_position();
			this._quaOriginRotation = base.get_transform().get_rotation();
			this._fShakeIntensity = this._fCoefShakeIntensity;
			this._fDelayTimer = 0f;
			this._isShake = true;
			this._actCallback = callback;
		}

		public void ShakePos(Action callback)
		{
			if (this._isShake)
			{
				return;
			}
			this._isShakePos = true;
			this._isShakeRot = false;
			this._vOriginPosition = base.get_transform().get_position();
			this._quaOriginRotation = base.get_transform().get_rotation();
			this._fShakeIntensity = this._fCoefShakeIntensity;
			this._fDelayTimer = 0f;
			this._isShake = true;
			this._actCallback = callback;
		}

		public void ShakeRot(Action callback)
		{
			if (this._isShake)
			{
				return;
			}
			this._isShakePos = false;
			this._isShakeRot = true;
			this._vOriginPosition = base.get_transform().get_position();
			this._quaOriginRotation = base.get_transform().get_rotation();
			this._fShakeIntensity = this._fCoefShakeIntensity;
			this._fDelayTimer = 0f;
			this._isShake = true;
			this._actCallback = callback;
		}
	}
}
