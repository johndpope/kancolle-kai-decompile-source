using Librarys.Cameras;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(FlareLayer)), RequireComponent(typeof(Camera)), RequireComponent(typeof(MotionBlur)), RequireComponent(typeof(CameraShake)), RequireComponent(typeof(Skybox)), RequireComponent(typeof(Vignetting)), RequireComponent(typeof(GlowEffect))]
	public class BattleFieldCamera : CameraActor
	{
		private bool _isMove;

		private Skybox _skybox;

		private MotionBlur _clsMotionBlur;

		private CameraShake _clsCameraShake;

		private FlareLayer _clsFlareLayer;

		private Vignetting _clsVignetting;

		private GlowEffect _clsGlowEffect;

		private float _fMotionBlurTime;

		private float _fMotionBlurDecay;

		public bool isMove
		{
			get
			{
				return this._isMove;
			}
			set
			{
				if (value != this._isMove)
				{
					this._isMove = value;
				}
			}
		}

		public Skybox skybox
		{
			get
			{
				if (this._skybox == null)
				{
					this._skybox = base.GetComponent<Skybox>();
				}
				return this._skybox;
			}
			set
			{
				if (value != this._skybox)
				{
					this._skybox = value;
				}
			}
		}

		public MotionBlur motionBlur
		{
			get
			{
				if (this._clsMotionBlur == null)
				{
					this._clsMotionBlur = this.AddComponent<MotionBlur>();
				}
				return this._clsMotionBlur;
			}
		}

		public FlareLayer flareLayer
		{
			get
			{
				if (this._clsFlareLayer == null)
				{
					this._clsFlareLayer = base.GetComponent<FlareLayer>();
				}
				return this._clsFlareLayer;
			}
		}

		public float motionBlurTime
		{
			get
			{
				return this._fMotionBlurTime;
			}
			set
			{
				if (value != this._fMotionBlurTime)
				{
					this._fMotionBlurTime = value;
				}
			}
		}

		public float motionBlurDecay
		{
			get
			{
				return this._fMotionBlurDecay;
			}
			set
			{
				if (value != this._fMotionBlurDecay)
				{
					this._fMotionBlurDecay = value;
				}
			}
		}

		public CameraShake cameraShake
		{
			get
			{
				if (this._clsCameraShake == null)
				{
					this._clsCameraShake = this.SafeGetComponent<CameraShake>();
				}
				return this._clsCameraShake;
			}
		}

		public new Generics.Layers cullingMask
		{
			get
			{
				return (Generics.Layers)base.cullingMask;
			}
			set
			{
				base.cullingMask = (int)value;
			}
		}

		public Vignetting vignetting
		{
			get
			{
				return this.GetComponentThis(ref this._clsVignetting);
			}
		}

		public GlowEffect glowEffect
		{
			get
			{
				return this.GetComponentThis(ref this._clsGlowEffect);
			}
		}

		protected override void Awake()
		{
			base.Awake();
			this._skybox = base.GetComponent<Skybox>();
			this._isMove = false;
			this._clsMotionBlur = this.SafeGetComponent<MotionBlur>();
			this._clsMotionBlur.blurAmount = 0.8f;
			this._clsMotionBlur.set_enabled(false);
			if (this._clsMotionBlur.shader == null)
			{
				this._clsMotionBlur.shader = Shader.Find("Hidden/MotionBlur");
			}
			this._clsCameraShake = this.SafeGetComponent<CameraShake>();
			this.flareLayer.set_enabled(false);
			this.vignetting.set_enabled(false);
			this.glowEffect.set_enabled(false);
		}

		private void Reset()
		{
			this.cullingMask = (Generics.Layers.TransparentFX | Generics.Layers.Water | Generics.Layers.Background | Generics.Layers.ShipGirl | Generics.Layers.Effects);
		}

		protected override void OnUnInit()
		{
			Mem.Del<bool>(ref this._isMove);
			Mem.DelSkyboxSafe(ref this._skybox);
			Mem.Del<MotionBlur>(ref this._clsMotionBlur);
			Mem.Del<CameraShake>(ref this._clsCameraShake);
			Mem.Del<FlareLayer>(ref this._clsFlareLayer);
			Mem.Del<Vignetting>(ref this._clsVignetting);
			Mem.Del<GlowEffect>(ref this._clsGlowEffect);
			Mem.Del<float>(ref this._fMotionBlurTime);
			Mem.Del<float>(ref this._fMotionBlurDecay);
			base.OnUnInit();
		}

		public void SetRotationCamera(Vector3 srcPos, Quaternion srcRot, Quaternion destRot, float rotSpeed)
		{
			this.ReqViewMode(CameraActor.ViewMode.Rotation);
			this._vSrcPos = srcPos;
			this._quaSrcRot = srcRot;
			this._quaDestRot = destRot;
			this._fRotateSpeed = rotSpeed;
			this._initCameraState();
		}

		public void SetZoomCamera(Vector3 srcPos, Quaternion srcRot, Vector3 targetPos, Quaternion targetRot, float speed)
		{
			this.ReqViewMode(CameraActor.ViewMode.SmoothMoveKI2ndEdition);
			this._vSrcPos = srcPos;
			this._quaSrcRot = srcRot;
			this._fSmoothTime = speed;
			this.SetSmoothMoveCamera(targetPos, targetRot, srcPos, srcRot);
		}

		protected override void FixedUpdate()
		{
			switch (this._iViewMode)
			{
			case CameraActor.ViewMode.SmoothMoveKI2ndEdition:
			{
				Vector3 eyePosition = base.eyePosition;
				Vector3 vDestPos = this._vDestPos;
				if ((double)Vector3.Distance(eyePosition, vDestPos) <= Math.Floor(1.0))
				{
					this._isMove = true;
					this.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
				}
				break;
			}
			case CameraActor.ViewMode.Rotation:
				if (Mathf.Sign(this._fRotateSpeed) == 1f)
				{
					if ((double)Vector3.Distance(base.eyeRotation.get_eulerAngles(), this._quaDestRot.get_eulerAngles()) <= Math.Floor(1.0) || (double)Vector3.Distance(base.eyeRotation.get_eulerAngles(), Vector3.get_up() * 360f) <= Math.Floor(1.0))
					{
						this._isMove = true;
						this.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
					}
				}
				else
				{
					if (Mathf.Sign(this._fRotateSpeed) != -1f)
					{
						return;
					}
					if ((double)Vector3.Distance(base.eyeRotation.get_eulerAngles(), this._quaDestRot.get_eulerAngles()) <= Math.Floor(-1.0))
					{
						this._isMove = true;
						this.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
					}
				}
				break;
			}
			base.FixedUpdate();
		}

		public void SetMotionBlur(float blurAmount, float blurTime, float blurDecay)
		{
			this.motionBlur.blurAmount = blurAmount;
			this._fMotionBlurTime = blurTime;
			this._fMotionBlurDecay = blurDecay;
			this.motionBlur.set_enabled(true);
		}

		public void ResetMotionBlur()
		{
			this.motionBlur.extraBlur = false;
			this.motionBlur.blurAmount = 0.8f;
			this.motionBlur.set_enabled(false);
		}
	}
}
