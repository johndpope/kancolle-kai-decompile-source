using System;
using UnityEngine;

[RequireComponent(typeof(Camera)), RequireComponent(typeof(UICamera))]
public class BaseCamera : SingletonMonoBehaviour<BaseCamera>
{
	protected Camera _camCamera;

	protected UICamera _camUICamera;

	public Camera camera
	{
		get
		{
			if (this._camCamera == null)
			{
				this._camCamera = base.GetComponent<Camera>();
			}
			return this._camCamera;
		}
	}

	public UICamera uiCamera
	{
		get
		{
			if (this._camUICamera == null)
			{
				this._camUICamera = base.GetComponent<UICamera>();
			}
			return this._camUICamera;
		}
	}

	public virtual bool isCulling
	{
		get
		{
			return this._camCamera.get_enabled() && this._camUICamera.get_enabled();
		}
		set
		{
			Behaviour arg_1E_0 = this.camera;
			base.set_enabled(value);
			this.uiCamera.set_enabled(value);
			arg_1E_0.set_enabled(value);
		}
	}

	public virtual Generics.Layers cullingMask
	{
		get
		{
			if (this._camCamera == null)
			{
				return Generics.Layers.Nothing;
			}
			return (Generics.Layers)this._camCamera.get_cullingMask();
		}
		set
		{
			if (this._camCamera == null)
			{
				this._camCamera = this.SafeGetComponent<Camera>();
			}
			this._camCamera.set_cullingMask((int)value);
		}
	}

	public virtual Generics.Layers eventMask
	{
		get
		{
			if (this._camUICamera == null)
			{
				return Generics.Layers.Nothing;
			}
			return (Generics.Layers)this._camUICamera.eventReceiverMask.get_value();
		}
		set
		{
			if (this._camUICamera == null)
			{
				this._camUICamera = this.SafeGetComponent<UICamera>();
			}
			this._camUICamera.eventReceiverMask = (int)value;
		}
	}

	public virtual Generics.Layers sameMask
	{
		set
		{
			this._camCamera.set_cullingMask(this._camUICamera.eventReceiverMask = (int)value);
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

	public virtual Color backgroundColor
	{
		get
		{
			return this.camera.get_backgroundColor();
		}
		set
		{
			this.camera.set_backgroundColor(value);
		}
	}

	public virtual float nearClip
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

	public virtual float farClip
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

	public virtual Rect viewportRect
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

	public virtual float depth
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

	public virtual RenderingPath renderingPath
	{
		get
		{
			return this.camera.get_renderingPath();
		}
		set
		{
			this.camera.set_renderingPath(value);
		}
	}

	public virtual RenderTexture targetTexture
	{
		get
		{
			return this.camera.get_targetTexture();
		}
		set
		{
			this.camera.set_targetTexture(value);
		}
	}

	public virtual bool isOcclisionCulling
	{
		get
		{
			return this.camera.get_useOcclusionCulling();
		}
		set
		{
			this.camera.set_useOcclusionCulling(value);
		}
	}

	public virtual bool isHDR
	{
		get
		{
			return this.camera.get_hdr();
		}
		set
		{
			this.camera.set_hdr(value);
		}
	}

	protected override void Awake()
	{
		this._camCamera = base.GetComponent<Camera>();
		this._camUICamera = this.SafeGetComponent<UICamera>();
	}

	private void OnDestroy()
	{
		Mem.Del<Camera>(ref this._camCamera);
		Mem.Del<UICamera>(ref this._camUICamera);
		this.OnUnInit();
	}

	protected virtual void OnUnInit()
	{
	}

	[Obsolete]
	public virtual void EventMask(Generics.Layers layer)
	{
		this._camUICamera.eventReceiverMask = (int)layer;
	}

	[Obsolete]
	public virtual void CullingMask(Generics.Layers layer)
	{
		this._camCamera.set_cullingMask((int)layer);
	}

	[Obsolete]
	public virtual void SameMask(Generics.Layers layer)
	{
		this._camCamera.set_cullingMask(this._camUICamera.eventReceiverMask = (int)layer);
	}
}
