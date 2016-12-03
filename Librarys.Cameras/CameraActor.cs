using System;
using UnityEngine;

namespace Librarys.Cameras
{
	[RequireComponent(typeof(Camera))]
	public class CameraActor : MonoBehaviour
	{
		[Flags]
		public enum Axis
		{
			None = 1,
			XAxis = 2,
			YAxis = 4,
			ZAxis = 8
		}

		public enum ViewMode
		{
			NotViewModeCtrl = -1,
			Fix,
			FixChasing,
			FixChasingRot,
			ZoomChasing,
			ZoomChasingUp,
			SmoothMove,
			SmoothMoveKI2ndEdition,
			FixedPositionChasing,
			RotateAroundObject,
			Rotation,
			Bezier
		}

		protected Camera _cam;

		[SerializeField]
		protected CameraActor.ViewMode _iViewMode;

		[SerializeField]
		protected Vector3 _vPointOfGaze;

		[SerializeField]
		protected float _fRotateSpeed = 10f;

		[SerializeField]
		protected float _fRotateDistance = 10f;

		protected CameraActor.Axis _iAxis;

		protected Vector3 _vSrcPos;

		protected Quaternion _quaSrcRot;

		protected Vector3 _vDestPos;

		protected Quaternion _quaDestRot;

		protected Quaternion _quaTempRot;

		protected Vector3 _vLeaveRotEuler = Vector3.get_zero();

		protected Vector3 _vLeavePosEuler = Vector3.get_zero();

		protected float _fLeavePosDistance;

		[SerializeField]
		protected float _fSmoothTime = 0.3f;

		[SerializeField]
		protected float _fSmoothDistance = 10f;

		[SerializeField]
		protected float _fSmoothRotDamping = 2f;

		[SerializeField]
		protected float _fSmoothCorrectionY;

		protected Vector3 _vSmoothVelocity;

		protected bool _isRotSmoothX = true;

		protected bool _isRotSmoothY = true;

		protected bool _isRotSmoothZ = true;

		[SerializeField]
		protected LayerMask _lmNearCollisionLayer;

		[SerializeField]
		protected Transform _traNearLeftUp;

		[SerializeField]
		protected Transform _traNearLeftDown;

		[SerializeField]
		protected Transform _traNearRightUp;

		[SerializeField]
		protected Transform _traNearRightDown;

		private Bezier _clsBezier;

		[SerializeField]
		protected float _fBezierTime;

		private bool _isAdjust;

		private float _fAdjustY;

		[SerializeField]
		protected float _fYAxisLimit;

		[SerializeField]
		protected bool _isThroughMaxY;

		[SerializeField]
		protected float _fSpecificOrbitLimitY;

		private float _fCorrectAccel = 0.005f;

		private float _fBackPosY;

		private float hosei;

		public Camera camera
		{
			get
			{
				if (this._cam == null)
				{
					this._cam = base.GetComponent<Camera>();
				}
				return this._cam;
			}
		}

		public Vector3 eyePosition
		{
			get
			{
				return base.get_transform().get_position();
			}
			set
			{
				base.get_transform().set_position(value);
			}
		}

		public Quaternion eyeRotation
		{
			get
			{
				return base.get_transform().get_rotation();
			}
			set
			{
				base.get_transform().set_rotation(value);
			}
		}

		public virtual int cullingMask
		{
			get
			{
				return this.camera.get_cullingMask();
			}
			set
			{
				this.camera.set_cullingMask(value);
			}
		}

		public virtual float fieldOfView
		{
			get
			{
				return this.camera.get_fieldOfView();
			}
			set
			{
				if (this.camera.get_fieldOfView() != value)
				{
					this.camera.set_fieldOfView(value);
				}
			}
		}

		public virtual CameraClearFlags clearFlags
		{
			get
			{
				return this.camera.get_clearFlags();
			}
			set
			{
				this.camera.set_clearFlags(value);
			}
		}

		public float nearClip
		{
			get
			{
				return this.camera.get_nearClipPlane();
			}
			set
			{
				this.camera.set_nearClipPlane(value);
			}
		}

		public float farClip
		{
			get
			{
				return this.camera.get_farClipPlane();
			}
			set
			{
				this.camera.set_farClipPlane(value);
			}
		}

		public float rotateSpeed
		{
			get
			{
				return this._fRotateSpeed;
			}
			set
			{
				this._fRotateSpeed = value;
			}
		}

		public float rotateDistance
		{
			get
			{
				return this._fRotateDistance;
			}
			set
			{
				this._fRotateDistance = value;
			}
		}

		public bool isCulling
		{
			get
			{
				return this.camera.get_enabled();
			}
			set
			{
				this.camera.set_enabled(value);
			}
		}

		public Rect viewportRect
		{
			get
			{
				return this.camera.get_rect();
			}
			set
			{
				this.camera.set_rect(value);
			}
		}

		public float depth
		{
			get
			{
				return this.camera.get_depth();
			}
			set
			{
				this.camera.set_depth(value);
			}
		}

		public virtual CameraActor.ViewMode viewMode
		{
			get
			{
				return this._iViewMode;
			}
			set
			{
				this._iViewMode = value;
			}
		}

		public Vector3 pointOfGaze
		{
			get
			{
				return this._vPointOfGaze;
			}
			set
			{
				this._vPointOfGaze = value;
			}
		}

		public Vector3 leaveRotateEuler
		{
			get
			{
				return this._vLeaveRotEuler;
			}
			set
			{
				this._vLeaveRotEuler = value;
			}
		}

		public Vector3 leavePositionEuler
		{
			get
			{
				return this._vLeavePosEuler;
			}
			set
			{
				this._vLeavePosEuler = value;
			}
		}

		public float leavePositionDistance
		{
			get
			{
				return this._fLeavePosDistance;
			}
			set
			{
				this._fLeavePosDistance = value;
			}
		}

		public float smoothTime
		{
			get
			{
				return this._fSmoothTime;
			}
			set
			{
				this._fSmoothTime = value;
			}
		}

		public float smoothDistance
		{
			get
			{
				return this._fSmoothDistance;
			}
			set
			{
				this._fSmoothDistance = value;
			}
		}

		public float smoothRotationDamping
		{
			get
			{
				return this._fSmoothRotDamping;
			}
			set
			{
				this._fSmoothRotDamping = value;
			}
		}

		public float smoothCorrectionY
		{
			get
			{
				return this._fSmoothCorrectionY;
			}
			set
			{
				this._fSmoothCorrectionY = value;
			}
		}

		public bool isRotationSmoothX
		{
			get
			{
				return this._isRotSmoothX;
			}
			set
			{
				this._isRotSmoothX = value;
			}
		}

		public bool isRotationSmoothY
		{
			get
			{
				return this._isRotSmoothY;
			}
			set
			{
				this._isRotSmoothY = value;
			}
		}

		public bool isRotationSmoothZ
		{
			get
			{
				return this._isRotSmoothZ;
			}
			set
			{
				this._isRotSmoothZ = value;
			}
		}

		public LayerMask nearCollitionLayer
		{
			get
			{
				return this._lmNearCollisionLayer;
			}
			set
			{
				this._lmNearCollisionLayer = value;
			}
		}

		public Transform nearClipCollitionLeftUp
		{
			get
			{
				return this._traNearLeftUp;
			}
			set
			{
				this._traNearLeftUp = value;
			}
		}

		public Transform nearClipCollitionLeftDown
		{
			get
			{
				return this._traNearLeftDown;
			}
			set
			{
				this._traNearLeftDown = value;
			}
		}

		public Transform nearClipCollitionRightUp
		{
			get
			{
				return this._traNearRightUp;
			}
			set
			{
				this._traNearRightUp = value;
			}
		}

		public Transform nearClipCollitionRightDown
		{
			get
			{
				return this._traNearRightDown;
			}
			set
			{
				this._traNearRightDown = value;
			}
		}

		public float YAxisLimit
		{
			get
			{
				return this._fYAxisLimit;
			}
			set
			{
				this._fYAxisLimit = value;
			}
		}

		public bool isThroughMaxY
		{
			get
			{
				return this._isThroughMaxY;
			}
			set
			{
				this._isThroughMaxY = value;
			}
		}

		public float specificOrbitLimitY
		{
			get
			{
				return this._fSpecificOrbitLimitY;
			}
			set
			{
				this._fSpecificOrbitLimitY = value;
			}
		}

		public Bezier bezier
		{
			get
			{
				return this._clsBezier;
			}
			set
			{
				this._clsBezier = value;
			}
		}

		public float bezierTime
		{
			get
			{
				return this._fBezierTime;
			}
			set
			{
				this._fBezierTime = Mathe.MinMax2(value, 0f, 1f);
			}
		}

		protected virtual void Awake()
		{
			this._iViewMode = CameraActor.ViewMode.Fix;
			this._vSmoothVelocity = Vector3.get_zero();
			this._iAxis = CameraActor.Axis.None;
		}

		protected virtual void Start()
		{
			if (this._traNearLeftUp != null)
			{
				this._traNearLeftUp.set_position(base.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 1f, base.GetComponent<Camera>().get_nearClipPlane())));
			}
			if (this._traNearLeftDown != null)
			{
				this._traNearLeftDown.set_position(base.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0f, 0f, base.GetComponent<Camera>().get_nearClipPlane())));
			}
			if (this._traNearRightUp != null)
			{
				this._traNearRightUp.set_position(base.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 1f, base.GetComponent<Camera>().get_nearClipPlane())));
			}
			if (this._traNearRightDown != null)
			{
				this._traNearRightDown.set_position(base.GetComponent<Camera>().ViewportToWorldPoint(new Vector3(1f, 0f, base.GetComponent<Camera>().get_nearClipPlane())));
			}
		}

		private void OnDestroy()
		{
			Mem.Del<Camera>(ref this._cam);
			Mem.Del<CameraActor.ViewMode>(ref this._iViewMode);
			Mem.Del<Vector3>(ref this._vPointOfGaze);
			Mem.Del<float>(ref this._fRotateSpeed);
			Mem.Del<float>(ref this._fRotateDistance);
			Mem.Del<CameraActor.Axis>(ref this._iAxis);
			Mem.Del<Vector3>(ref this._vSrcPos);
			Mem.Del<Quaternion>(ref this._quaSrcRot);
			Mem.Del<Vector3>(ref this._vDestPos);
			Mem.Del<Quaternion>(ref this._quaDestRot);
			Mem.Del<Quaternion>(ref this._quaTempRot);
			Mem.Del<Vector3>(ref this._vLeavePosEuler);
			Mem.Del<Vector3>(ref this._vLeaveRotEuler);
			Mem.Del<float>(ref this._fLeavePosDistance);
			Mem.Del<float>(ref this._fSmoothTime);
			Mem.Del<float>(ref this._fSmoothDistance);
			Mem.Del<float>(ref this._fSmoothRotDamping);
			Mem.Del<float>(ref this._fSmoothCorrectionY);
			Mem.Del<Vector3>(ref this._vSmoothVelocity);
			Mem.Del<bool>(ref this._isRotSmoothX);
			Mem.Del<bool>(ref this._isRotSmoothY);
			Mem.Del<bool>(ref this._isRotSmoothZ);
			Mem.Del<LayerMask>(ref this._lmNearCollisionLayer);
			Mem.Del<Transform>(ref this._traNearLeftUp);
			Mem.Del<Transform>(ref this._traNearLeftDown);
			Mem.Del<Transform>(ref this._traNearRightUp);
			Mem.Del<Transform>(ref this._traNearRightDown);
			Mem.Del<Bezier>(ref this._clsBezier);
			Mem.Del<float>(ref this._fBezierTime);
			Mem.Del<bool>(ref this._isAdjust);
			Mem.Del<float>(ref this._fAdjustY);
			Mem.Del<float>(ref this._fYAxisLimit);
			Mem.Del<bool>(ref this._isThroughMaxY);
			Mem.Del<float>(ref this._fSpecificOrbitLimitY);
			Mem.Del<float>(ref this._fCorrectAccel);
			Mem.Del<float>(ref this._fBackPosY);
			this.OnUnInit();
		}

		protected virtual void OnUnInit()
		{
		}

		public virtual void ReqViewMode(CameraActor.ViewMode iView)
		{
			this._iViewMode = iView;
		}

		public virtual void VelocityReset()
		{
			this._vSmoothVelocity = Vector3.get_zero();
		}

		public virtual void SetPosition(Vector3 src, Vector3 dst)
		{
			this._vSrcPos = src;
			this._vDestPos = dst;
			this._fBezierTime = 0f;
			this._initCameraState();
		}

		public virtual void SetPosition(Vector3 src, Quaternion rot, Quaternion dest)
		{
			this._vSrcPos = src;
			this._quaSrcRot = rot;
			this._quaDestRot = dest;
			this._initCameraState();
		}

		public virtual void SetPosition(Vector3 pos, Quaternion rot)
		{
			Vector3 vector = rot * Quaternion.Euler(this._vLeavePosEuler) * Vector3.get_back();
			this._vDestPos = pos + vector * this._fLeavePosDistance;
			this._quaDestRot = rot * Quaternion.Euler(this._vLeaveRotEuler);
			this._vSrcPos = base.get_transform().get_position();
			this._quaSrcRot = base.get_transform().get_rotation();
			this._initCameraState();
		}

		public virtual void SetRawPosition(Vector3 pos)
		{
			base.get_transform().set_position(pos);
		}

		public virtual void SetRawRotate(Vector3 rot)
		{
			base.get_transform().set_rotation(Quaternion.Euler(rot));
		}

		public virtual void SetFixCamera(Vector3 srcPos, Quaternion srcRot)
		{
			this._vSrcPos = srcPos;
			this._quaSrcRot = srcRot;
			this._initCameraState();
		}

		public virtual void SetFixChasingCamera(Vector3 srcPos)
		{
			this._vDestPos = srcPos;
			this._vSrcPos = srcPos;
			this._initCameraState();
		}

		public virtual void SetFixChasingRotCamera(CameraActor.Axis iAxis, Vector3 srcPos, float rotSpeed)
		{
			this._vSrcPos = srcPos;
			this._iAxis = iAxis;
			this._fRotateSpeed = rotSpeed;
			this._initCameraState();
		}

		public virtual void SetSmoothMoveCamera(Vector3 targetPos, Quaternion targetRot, Vector3 srcPos, Quaternion srcRot)
		{
			Vector3 vector = targetRot * Quaternion.Euler(this._vLeavePosEuler) * Vector3.get_back();
			this._vDestPos = targetPos + vector * this._fLeavePosDistance;
			this._quaDestRot = targetRot * Quaternion.Euler(this._vLeaveRotEuler);
			this._vSrcPos = srcPos;
			this._quaSrcRot = srcRot;
			this._initCameraState();
		}

		public virtual void SetSmoothMoveCamera(Vector3 targetPos, Quaternion targetRot, Vector3 srcPos, Quaternion srcRot, float smoothDst)
		{
			this._vDestPos = targetPos * this._fLeavePosDistance;
			this._fSmoothDistance = this.smoothDistance;
			this._vSrcPos = srcPos;
			this._quaSrcRot = srcRot;
			this._initCameraState();
		}

		public virtual void SetRotateAroundObjectCamera(Vector3 pog, float dir, float rotSpeed)
		{
			this._vPointOfGaze = pog;
			this._fRotateDistance = dir;
			Vector3 vector = Mathe.NormalizeDirection(this._vPointOfGaze, base.get_transform().get_position());
			Vector3 vDestPos = pog + vector * this._fRotateDistance;
			this._vDestPos = vDestPos;
			this._quaDestRot = Quaternion.Euler(vector * -1f);
			this._fRotateSpeed = rotSpeed;
			this._initCameraState();
		}

		public virtual void SetRotateAroundObjectCamera(Vector3 pog, Vector3 srcPos, float rotSpeed)
		{
			this._vPointOfGaze = pog;
			this._fRotateDistance = Vector3.Distance(srcPos, this._vPointOfGaze);
			Vector3 vector = Mathe.NormalizeDirection(this._vPointOfGaze, srcPos);
			this._vDestPos = srcPos;
			this._quaDestRot = Quaternion.Euler(vector * -1f);
			this._fRotateSpeed = rotSpeed;
			this._initCameraState();
		}

		public virtual void SetBezierCamera(Bezier bezier, Vector3 srcPos, Quaternion srcRot)
		{
			this._clsBezier = bezier;
			this._vSrcPos = srcPos;
			this._quaSrcRot = srcRot;
			this._vDestPos = this._clsBezier.Interpolate(1f);
			this._initCameraState();
		}

		public virtual void SetBezierCamera(Vector3 pog, Bezier bezier, Vector3 srcPos, Quaternion srcRot)
		{
			this._vPointOfGaze = pog;
			this.SetBezierCamera(bezier, srcPos, srcRot);
		}

		public virtual void SetRotationCamera(Vector3 srcPos, Quaternion srcRot, float rotSpeed)
		{
			this._vSrcPos = srcPos;
			this._quaSrcRot = srcRot;
			this._fRotateSpeed = rotSpeed;
			this._initCameraState();
		}

		protected virtual void _initCameraState()
		{
			this._fYAxisLimit = 0f;
			this._fSmoothCorrectionY = 0f;
			this._isThroughMaxY = false;
			CameraActor.ViewMode iViewMode = this._iViewMode;
			switch (iViewMode + 1)
			{
			case CameraActor.ViewMode.FixChasing:
				base.get_transform().set_position(this._vSrcPos);
				base.get_transform().set_rotation(this._quaSrcRot);
				this._vSmoothVelocity = Vector3.get_zero();
				break;
			case CameraActor.ViewMode.FixChasingRot:
				base.get_transform().set_position(this._vDestPos);
				base.get_transform().LookAt(this._vPointOfGaze);
				break;
			case CameraActor.ViewMode.ZoomChasing:
				base.get_transform().set_position(this._vSrcPos);
				this._quaTempRot = base.get_transform().get_rotation();
				break;
			case CameraActor.ViewMode.ZoomChasingUp:
			{
				float num = this._fSmoothDistance / Vector3.Distance(this._vPointOfGaze, base.get_transform().get_position());
				this._vDestPos = this._vPointOfGaze + (base.get_transform().get_position() - this._vPointOfGaze) * num;
				this._vSmoothVelocity = Vector3.get_zero();
				this._fSmoothTime = 0.2f;
				this._isAdjust = false;
				base.get_transform().LookAt(this._vPointOfGaze);
				break;
			}
			case CameraActor.ViewMode.SmoothMove:
			{
				float num2 = this._fSmoothDistance / Vector3.Distance(this._vPointOfGaze, base.get_transform().get_position());
				this._vDestPos = this._vPointOfGaze + (base.get_transform().get_position() - this._vPointOfGaze) * num2;
				this._vSmoothVelocity = Vector3.get_zero();
				this._fSmoothTime = 0.2f;
				this._isAdjust = false;
				this._fAdjustY = base.get_transform().get_position().y + 10f;
				base.get_transform().LookAt(this._vPointOfGaze);
				break;
			}
			case CameraActor.ViewMode.SmoothMoveKI2ndEdition:
			case CameraActor.ViewMode.FixedPositionChasing:
				base.get_transform().set_position(this._vSrcPos);
				base.get_transform().set_rotation(this._quaSrcRot);
				break;
			case CameraActor.ViewMode.RotateAroundObject:
				base.get_transform().set_position(this._vPointOfGaze + base.get_transform().TransformDirection(Vector3.get_back() * this._fSmoothDistance));
				base.get_transform().set_rotation(this._quaDestRot);
				this._vSmoothVelocity = Vector3.get_zero();
				this._fSmoothTime = 0.1f;
				this._fSmoothCorrectionY = 6f;
				break;
			case CameraActor.ViewMode.Rotation:
			{
				Vector3 vector = Mathe.Direction(this._vPointOfGaze, this._vDestPos);
				Vector3 vector2 = this._quaDestRot * vector;
				base.get_transform().set_position(this._vPointOfGaze + vector2);
				base.get_transform().LookAt(this._vPointOfGaze);
				this._vSmoothVelocity = Vector3.get_zero();
				break;
			}
			case CameraActor.ViewMode.Bezier:
				base.get_transform().set_position(this._vSrcPos);
				base.get_transform().set_rotation(this._quaSrcRot);
				break;
			case (CameraActor.ViewMode)11:
				this.eyePosition = this._vSrcPos;
				this.eyeRotation = this._quaSrcRot;
				this._fBezierTime = 0f;
				break;
			}
		}

		protected virtual void FixedUpdate()
		{
			CameraActor.ViewMode iViewMode = this._iViewMode;
			switch (iViewMode + 1)
			{
			case CameraActor.ViewMode.FixChasingRot:
				base.get_transform().LookAt(this._vPointOfGaze);
				break;
			case CameraActor.ViewMode.ZoomChasing:
				this._fixChasingRot();
				break;
			case CameraActor.ViewMode.ZoomChasingUp:
				this._zoomChasing();
				break;
			case CameraActor.ViewMode.SmoothMove:
				this._zoomChasingUp();
				break;
			case CameraActor.ViewMode.SmoothMoveKI2ndEdition:
				this._smoothMove();
				break;
			case CameraActor.ViewMode.FixedPositionChasing:
				this._smoothMoveKI2ndEdition();
				break;
			case CameraActor.ViewMode.RotateAroundObject:
				this._fixedPositionChasing();
				break;
			case CameraActor.ViewMode.Rotation:
				this._rotateAroundObject();
				break;
			case CameraActor.ViewMode.Bezier:
				this._rotation();
				break;
			case (CameraActor.ViewMode)11:
				this._bezier();
				break;
			}
		}

		protected virtual void _fixChasingRot()
		{
			this._quaTempRot = base.get_transform().get_rotation();
			base.get_transform().LookAt(this._vPointOfGaze);
			Quaternion quaternion = Quaternion.get_identity();
			if (this._iAxis.HasFlag(CameraActor.Axis.XAxis))
			{
				quaternion = Quaternion.Euler(this._fRotateSpeed * Time.get_deltaTime(), 0f, 0f);
				this._quaTempRot *= quaternion;
				base.get_transform().set_rotation(Quaternion.Euler(this._quaTempRot.get_eulerAngles().x, base.get_transform().get_rotation().get_eulerAngles().y, base.get_transform().get_rotation().get_eulerAngles().z));
			}
			else if (this._iAxis.HasFlag(CameraActor.Axis.YAxis))
			{
				quaternion = Quaternion.Euler(0f, this._fRotateSpeed * Time.get_deltaTime(), 0f);
				this._quaTempRot *= quaternion;
				base.get_transform().set_rotation(Quaternion.Euler(base.get_transform().get_rotation().get_eulerAngles().x, this._quaTempRot.get_eulerAngles().y, base.get_transform().get_rotation().get_eulerAngles().z));
			}
			else if (this._iAxis.HasFlag(CameraActor.Axis.ZAxis))
			{
				quaternion = Quaternion.Euler(0f, 0f, this._fRotateSpeed * Time.get_deltaTime());
				this._quaTempRot *= quaternion;
				base.get_transform().set_rotation(Quaternion.Euler(base.get_transform().get_rotation().get_eulerAngles().x, base.get_transform().get_rotation().get_eulerAngles().y, this._quaTempRot.get_eulerAngles().z));
			}
		}

		protected virtual void _zoomChasing()
		{
			Vector3 vector = this._vDestPos;
			vector = Vector3.SmoothDamp(base.get_transform().get_position(), vector, ref this._vSmoothVelocity, this._fSmoothTime);
			if (this._isAdjust)
			{
				vector.y = Mathf.Lerp(base.get_transform().get_position().y, this._fAdjustY, 0.05f);
				base.get_transform().set_position(vector);
				base.get_transform().LookAt(this._vPointOfGaze, Vector3.get_up());
			}
			else
			{
				base.get_transform().set_position(vector);
				if (this.PositionGroundAdjust(vector) != 0)
				{
					base.get_transform().LookAt(this._vPointOfGaze, Vector3.get_up());
					this._fAdjustY = vector.y + 10f;
					this._isAdjust = true;
				}
				else
				{
					base.get_transform().LookAt(this._vPointOfGaze);
				}
			}
		}

		protected virtual void _zoomChasingUp()
		{
			Vector3 vector = this._vDestPos;
			vector = Vector3.SmoothDamp(base.get_transform().get_position(), vector, ref this._vSmoothVelocity, this._fSmoothTime);
			vector.y = Mathf.Lerp(base.get_transform().get_position().y, this._fAdjustY, 0.05f);
			base.get_transform().set_position(vector);
			base.get_transform().LookAt(this._vPointOfGaze, Vector3.get_up());
		}

		protected virtual void _smoothMove()
		{
			Vector3 eulerAngles = base.get_transform().get_eulerAngles();
			Vector3 eulerAngles2 = this._quaDestRot.get_eulerAngles();
			Vector3 vector = eulerAngles2;
			if (this._isRotSmoothX)
			{
				vector.x = eulerAngles.x;
			}
			if (this._isRotSmoothY)
			{
				vector.y = eulerAngles.y;
			}
			if (this._isRotSmoothZ)
			{
				vector.z = eulerAngles.z;
			}
			base.get_transform().set_rotation(Quaternion.Euler(vector));
			eulerAngles = base.get_transform().get_eulerAngles();
			eulerAngles.x = Mathf.LerpAngle(eulerAngles.x, eulerAngles2.x, this._fSmoothRotDamping * Time.get_deltaTime());
			eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, eulerAngles2.y, this._fSmoothRotDamping * Time.get_deltaTime());
			eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, eulerAngles2.z, this._fSmoothRotDamping * Time.get_deltaTime());
			Quaternion rotation = Quaternion.Euler(eulerAngles);
			base.get_transform().set_rotation(rotation);
			vector = this._vDestPos;
			vector += base.get_transform().TransformDirection(Vector3.get_back() * this._fSmoothDistance);
			base.get_transform().set_position(Vector3.SmoothDamp(base.get_transform().get_position(), vector, ref this._vSmoothVelocity, this._fSmoothTime));
			this.PositionGroundAdjust(this.eyePosition);
		}

		protected virtual void _smoothMoveKI2ndEdition()
		{
			Vector3 eulerAngles = base.get_transform().get_eulerAngles();
			Vector3 eulerAngles2 = this._quaDestRot.get_eulerAngles();
			Vector3 vector = eulerAngles2;
			if (this._isRotSmoothX)
			{
				vector.x = eulerAngles.x;
			}
			if (this._isRotSmoothY)
			{
				vector.y = eulerAngles.y;
			}
			if (this._isRotSmoothZ)
			{
				vector.z = eulerAngles.z;
			}
			base.get_transform().set_rotation(Quaternion.Euler(vector));
			eulerAngles = base.get_transform().get_eulerAngles();
			eulerAngles.x = Mathf.LerpAngle(eulerAngles.x, eulerAngles2.x, this._fSmoothRotDamping * Time.get_deltaTime());
			eulerAngles.y = Mathf.LerpAngle(eulerAngles.y, eulerAngles2.y, this._fSmoothRotDamping * Time.get_deltaTime());
			eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, eulerAngles2.z, this._fSmoothRotDamping * Time.get_deltaTime());
			Quaternion rotation = Quaternion.Euler(eulerAngles);
			base.get_transform().set_rotation(rotation);
			vector = this._vDestPos;
			vector += Mathe.NormalizeDirection(this.eyePosition, this._vDestPos) * this._fSmoothDistance;
			base.get_transform().set_position(Vector3.SmoothDamp(base.get_transform().get_position(), vector, ref this._vSmoothVelocity, this._fSmoothTime));
		}

		protected virtual void _fixedPositionChasing()
		{
			Vector3 vector;
			vector.x = this._vPointOfGaze.x;
			vector.y = this._vPointOfGaze.y + 1f;
			vector.z = this._vPointOfGaze.z;
			Vector3 vector2 = this._quaDestRot * Quaternion.Euler(this._vLeavePosEuler) * Vector3.get_back();
			this._vDestPos = vector + vector2 * this._fLeavePosDistance;
			if (this._isAdjust)
			{
				this._fSmoothCorrectionY = Mathf.Lerp(this._fSmoothCorrectionY, this._fYAxisLimit, 0.05f);
				if (this._fSmoothCorrectionY >= this._fYAxisLimit)
				{
					this._isAdjust = false;
				}
			}
			Vector3 vector3;
			vector3.x = this._vDestPos.x;
			vector3.y = this._vDestPos.y + this._fSmoothCorrectionY;
			vector3.z = this._vDestPos.z;
			vector3 += base.get_transform().TransformDirection(Quaternion.Euler(0f, 0f, 0f) * Vector3.get_back() * this._fSmoothDistance);
			Vector3 vector4 = Vector3.SmoothDamp(base.get_transform().get_position(), vector3, ref this._vSmoothVelocity, this._fSmoothTime);
			RaycastHit raycastHit;
			if (Physics.Linecast(vector4 + new Vector3(0f, 4000f, 0f), vector4, ref raycastHit, this._lmNearCollisionLayer.get_value()))
			{
				Debug.Log(string.Format("高さ補正:{0}|{1}", vector4, raycastHit.get_point()));
				vector4 = raycastHit.get_point() + new Vector3(0f, 1f, 0f);
			}
			base.get_transform().set_position(vector4);
			int num = this.PositionGroundAdjust(vector4);
			if (num != 0)
			{
				this._isAdjust = true;
				this._fYAxisLimit = this._fSmoothCorrectionY + 3f;
			}
			if (this._vPointOfGaze.y > this._fSpecificOrbitLimitY * 0.9f && this._vPointOfGaze.y > this._fBackPosY)
			{
				this._fSmoothCorrectionY = Mathf.Max(2f, this._fSmoothCorrectionY - Mathf.Pow(this._fCorrectAccel, 2f) * 1.3E-05f);
				this._isThroughMaxY = true;
				this._fCorrectAccel += 1f;
			}
			else if (this._isThroughMaxY && this._vPointOfGaze.y < this._fSpecificOrbitLimitY * 0.95f)
			{
				this._fSmoothCorrectionY = Mathf.Lerp(this._fSmoothCorrectionY, 0f, 0.1f);
			}
			this._fBackPosY = this._vPointOfGaze.y;
		}

		protected virtual void _rotateAroundObject()
		{
			Vector3 vector = Mathe.NormalizeDirection(this._vPointOfGaze, base.get_transform().get_position());
			Quaternion quaternion = Quaternion.Euler(0f, this._fRotateSpeed * Time.get_deltaTime(), 0f);
			Vector3 vector2 = quaternion * (vector * this._fRotateDistance);
			base.get_transform().set_position(this._vPointOfGaze + vector2);
			base.get_transform().LookAt(this._vPointOfGaze);
		}

		protected virtual void _rotation()
		{
			Quaternion quaternion = Quaternion.Euler(0f, this._fRotateSpeed * Time.get_deltaTime(), 0f);
			Transform expr_22 = base.get_transform();
			expr_22.set_rotation(expr_22.get_rotation() * quaternion);
		}

		protected virtual void _bezier()
		{
			base.get_transform().LookAt(this.pointOfGaze);
			Vector3 position = this._clsBezier.Interpolate(this._fBezierTime);
			base.get_transform().set_position(position);
		}

		public int CheckNearCollision(out Vector3 lhit, out Vector3 rhit)
		{
			int num = 0;
			if (this._traNearLeftDown == null || this._traNearLeftUp == null || this._traNearRightDown == null || this._traNearRightUp == null)
			{
				lhit = Vector3.get_zero();
				rhit = Vector3.get_zero();
				return 0;
			}
			RaycastHit raycastHit;
			if (Physics.Linecast(this._traNearLeftUp.get_position(), this._traNearLeftDown.get_position(), ref raycastHit, this._lmNearCollisionLayer.get_value()))
			{
				lhit = raycastHit.get_point();
				num++;
			}
			else
			{
				lhit = Vector3.get_zero();
			}
			if (Physics.Linecast(this._traNearRightUp.get_position(), this._traNearRightDown.get_position(), ref raycastHit, this._lmNearCollisionLayer.get_value()))
			{
				rhit = raycastHit.get_point();
				num += 2;
			}
			else
			{
				rhit = Vector3.get_zero();
			}
			return num;
		}

		public int PositionGroundAdjust(Vector3 newpos)
		{
			Vector3 vector;
			Vector3 vector2;
			int num = this.CheckNearCollision(out vector, out vector2);
			if (num == 3)
			{
				if (vector.y > vector2.y)
				{
					num = 1;
				}
				else
				{
					num = 2;
				}
			}
			if (num == 1)
			{
				newpos += new Vector3(0f, vector.y - this._traNearLeftDown.get_position().y + 0.2f, 0f);
			}
			else if (num == 2)
			{
				newpos += new Vector3(0f, vector2.y - this._traNearRightDown.get_position().y + 0.2f, 0f);
			}
			base.get_transform().set_position(newpos);
			return num;
		}

		public virtual void LookAt(Vector3 target)
		{
			this._vPointOfGaze = target;
			base.get_transform().LookAt(target);
		}

		public virtual void LookTo(Vector3 target, float time)
		{
			iTween.LookTo(base.get_gameObject(), target, time);
		}
	}
}
