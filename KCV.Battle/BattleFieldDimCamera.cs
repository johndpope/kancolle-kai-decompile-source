using LT.Tweening;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[RequireComponent(typeof(Skybox)), RequireComponent(typeof(Camera))]
	public class BattleFieldDimCamera : BaseCamera
	{
		[SerializeField]
		private Transform _traSyncTarget;

		[SerializeField]
		private MeshRenderer _mrRender;

		[Button("syncTransform", "sync to transform.", new object[]
		{

		}), SerializeField]
		private int syncTrans;

		[Button("SyncCameraProperty", "sync to camera properties", new object[]
		{

		}), SerializeField]
		private int syncCameraProperty;

		[Button("SetMaskPlane", "set to mask plane.", new object[]
		{

		}), SerializeField]
		private int setMaskPlane;

		private bool _isSync;

		private Color _cColor = Color.get_clear();

		private Skybox _skybox;

		public Transform syncTarget
		{
			get
			{
				if (this._traSyncTarget == null)
				{
					return null;
				}
				return this._traSyncTarget;
			}
			set
			{
				if (this._traSyncTarget != value)
				{
					this._traSyncTarget = value;
				}
			}
		}

		public bool isSync
		{
			get
			{
				return this._isSync;
			}
			set
			{
				if (this._isSync != value)
				{
					this._isSync = value;
				}
			}
		}

		public Color maskColor
		{
			get
			{
				if (this._cColor == Color.get_clear())
				{
					this._mrRender.get_material().GetColor("_Color");
				}
				return this._cColor;
			}
			set
			{
				if (value != this.maskColor)
				{
					this._cColor = value;
					this._mrRender.get_material().SetColor("_Color", this._cColor);
				}
			}
		}

		public float maskAlpha
		{
			get
			{
				return this.maskColor.a;
			}
			set
			{
				if (value != this.maskColor.a)
				{
					this._cColor.a = value;
					this._mrRender.get_material().SetColor("_Color", new Color(this.maskColor.r, this.maskColor.g, this.maskColor.b, value));
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

		public static BattleFieldDimCamera Instantiate(BattleFieldDimCamera prefab, Transform parent)
		{
			BattleFieldDimCamera battleFieldDimCamera = Object.Instantiate<BattleFieldDimCamera>(prefab);
			battleFieldDimCamera.get_transform().set_parent(parent);
			battleFieldDimCamera.get_transform().set_position(Vector3.get_zero());
			battleFieldDimCamera.get_transform().set_localScale(Vector3.get_one());
			battleFieldDimCamera.set_name("BattleFieldDimCamera");
			return battleFieldDimCamera;
		}

		protected override void Awake()
		{
			base.Awake();
			this.isCulling = false;
			this.isSync = this.SyncCameraProperty();
		}

		protected override void OnUnInit()
		{
			Transform transform = this._mrRender.get_transform();
			Mem.DelMeshSafe(ref transform);
			Mem.Del<Transform>(ref this._traSyncTarget);
			Mem.Del<MeshRenderer>(ref this._mrRender);
			Mem.Del<bool>(ref this._isSync);
			Mem.Del<Color>(ref this._cColor);
			Mem.DelSkyboxSafe(ref this._skybox);
			Mem.Del<Transform>(ref transform);
			base.OnUnInit();
		}

		private void FixedUpdate()
		{
			if (this.isCulling)
			{
				if (!this.isSync)
				{
					return;
				}
				this.SyncTransform();
			}
		}

		public void SyncTransform()
		{
			if (this.syncTarget != null)
			{
				if (this.syncTarget.get_position() != base.get_transform().get_position())
				{
					base.get_transform().set_position(this.syncTarget.get_position());
				}
				if (this.syncTarget.get_rotation() != base.get_transform().get_rotation())
				{
					base.get_transform().set_rotation(this.syncTarget.get_rotation());
				}
			}
		}

		public bool SyncCameraProperty()
		{
			if (this.syncTarget != null && this.syncTarget.GetComponent<Camera>())
			{
				Camera component = this.syncTarget.GetComponent<Camera>();
				this.clearFlags = component.get_clearFlags();
				this.backgroundColor = component.get_backgroundColor();
				this.cullingMask = (Generics.Layers)component.get_cullingMask();
				this.nearClip = component.get_nearClipPlane();
				this.farClip = component.get_farClipPlane();
				this.viewportRect = component.get_rect();
				this.isOcclisionCulling = component.get_useOcclusionCulling();
				this.isHDR = component.get_hdr();
				return true;
			}
			return false;
		}

		public void SetMaskPlane()
		{
			if (this._mrRender == null)
			{
				return;
			}
			this._mrRender.get_transform().localPositionZ(this.nearClip + 0.1f);
			this._mrRender.get_transform().set_localRotation(Quaternion.Euler(90f, 0f, 0f));
		}

		public void SetMaskPlaneAlpha(float from, float to, float time)
		{
			base.get_transform().LTValue(from, to, time).setOnUpdate(delegate(float x)
			{
				this.maskAlpha = x;
			});
		}

		public void SetMaskPlaneAlpha(float val)
		{
			this.maskAlpha = val;
		}
	}
}
