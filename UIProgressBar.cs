using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/NGUI Progress Bar"), ExecuteInEditMode]
public class UIProgressBar : UIWidgetContainer
{
	public enum FillDirection
	{
		LeftToRight,
		RightToLeft,
		BottomToTop,
		TopToBottom
	}

	public delegate void OnDragFinished();

	public static UIProgressBar current;

	public UIProgressBar.OnDragFinished onDragFinished;

	public Transform thumb;

	[HideInInspector, SerializeField]
	protected UIWidget mBG;

	[HideInInspector, SerializeField]
	protected UIWidget mFG;

	[HideInInspector, SerializeField]
	protected float mValue = 1f;

	[HideInInspector, SerializeField]
	protected UIProgressBar.FillDirection mFill;

	protected Transform mTrans;

	protected bool mIsDirty;

	protected Camera mCam;

	protected float mOffset;

	public int numberOfSteps;

	public List<EventDelegate> onChange = new List<EventDelegate>();

	public Transform cachedTransform
	{
		get
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.get_transform();
			}
			return this.mTrans;
		}
	}

	public Camera cachedCamera
	{
		get
		{
			if (this.mCam == null)
			{
				this.mCam = NGUITools.FindCameraForLayer(base.get_gameObject().get_layer());
			}
			return this.mCam;
		}
	}

	public UIWidget foregroundWidget
	{
		get
		{
			return this.mFG;
		}
		set
		{
			if (this.mFG != value)
			{
				this.mFG = value;
				this.mIsDirty = true;
			}
		}
	}

	public UIWidget backgroundWidget
	{
		get
		{
			return this.mBG;
		}
		set
		{
			if (this.mBG != value)
			{
				this.mBG = value;
				this.mIsDirty = true;
			}
		}
	}

	public UIProgressBar.FillDirection fillDirection
	{
		get
		{
			return this.mFill;
		}
		set
		{
			if (this.mFill != value)
			{
				this.mFill = value;
				this.ForceUpdate();
			}
		}
	}

	public float value
	{
		get
		{
			if (this.numberOfSteps > 1)
			{
				return Mathf.Round(this.mValue * (float)(this.numberOfSteps - 1)) / (float)(this.numberOfSteps - 1);
			}
			return this.mValue;
		}
		set
		{
			float num = Mathf.Clamp01(value);
			if (this.mValue != num)
			{
				float value2 = this.value;
				this.mValue = num;
				if (value2 != this.value)
				{
					this.ForceUpdate();
					if (UIProgressBar.current == null && NGUITools.GetActive(this) && EventDelegate.IsValid(this.onChange))
					{
						UIProgressBar.current = this;
						EventDelegate.Execute(this.onChange);
						UIProgressBar.current = null;
					}
				}
			}
		}
	}

	public float alpha
	{
		get
		{
			if (this.mFG != null)
			{
				return this.mFG.alpha;
			}
			if (this.mBG != null)
			{
				return this.mBG.alpha;
			}
			return 1f;
		}
		set
		{
			if (this.mFG != null)
			{
				this.mFG.alpha = value;
				if (this.mFG.GetComponent<Collider>() != null)
				{
					this.mFG.GetComponent<Collider>().set_enabled(this.mFG.alpha > 0.001f);
				}
				else if (this.mFG.GetComponent<Collider2D>() != null)
				{
					this.mFG.GetComponent<Collider2D>().set_enabled(this.mFG.alpha > 0.001f);
				}
			}
			if (this.mBG != null)
			{
				this.mBG.alpha = value;
				if (this.mBG.GetComponent<Collider>() != null)
				{
					this.mBG.GetComponent<Collider>().set_enabled(this.mBG.alpha > 0.001f);
				}
				else if (this.mBG.GetComponent<Collider2D>() != null)
				{
					this.mBG.GetComponent<Collider2D>().set_enabled(this.mBG.alpha > 0.001f);
				}
			}
			if (this.thumb != null)
			{
				UIWidget component = this.thumb.GetComponent<UIWidget>();
				if (component != null)
				{
					component.alpha = value;
					if (component.GetComponent<Collider>() != null)
					{
						component.GetComponent<Collider>().set_enabled(component.alpha > 0.001f);
					}
					else if (component.GetComponent<Collider2D>() != null)
					{
						component.GetComponent<Collider2D>().set_enabled(component.alpha > 0.001f);
					}
				}
			}
		}
	}

	protected bool isHorizontal
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.LeftToRight || this.mFill == UIProgressBar.FillDirection.RightToLeft;
		}
	}

	protected bool isInverted
	{
		get
		{
			return this.mFill == UIProgressBar.FillDirection.RightToLeft || this.mFill == UIProgressBar.FillDirection.TopToBottom;
		}
	}

	protected void Start()
	{
		this.Upgrade();
		if (Application.get_isPlaying())
		{
			if (this.mBG != null)
			{
				this.mBG.autoResizeBoxCollider = true;
			}
			this.OnStart();
			if (UIProgressBar.current == null && this.onChange != null)
			{
				UIProgressBar.current = this;
				EventDelegate.Execute(this.onChange);
				UIProgressBar.current = null;
			}
		}
		this.ForceUpdate();
	}

	protected virtual void Upgrade()
	{
	}

	protected virtual void OnStart()
	{
	}

	protected void Update()
	{
		if (this.mIsDirty)
		{
			this.ForceUpdate();
		}
	}

	protected void OnValidate()
	{
		if (NGUITools.GetActive(this))
		{
			this.Upgrade();
			this.mIsDirty = true;
			float num = Mathf.Clamp01(this.mValue);
			if (this.mValue != num)
			{
				this.mValue = num;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
			this.ForceUpdate();
		}
		else
		{
			float num2 = Mathf.Clamp01(this.mValue);
			if (this.mValue != num2)
			{
				this.mValue = num2;
			}
			if (this.numberOfSteps < 0)
			{
				this.numberOfSteps = 0;
			}
			else if (this.numberOfSteps > 20)
			{
				this.numberOfSteps = 20;
			}
		}
	}

	protected float ScreenToValue(Vector2 screenPos)
	{
		Transform cachedTransform = this.cachedTransform;
		Plane plane = new Plane(cachedTransform.get_rotation() * Vector3.get_back(), cachedTransform.get_position());
		Ray ray = this.cachedCamera.ScreenPointToRay(screenPos);
		float num;
		if (!plane.Raycast(ray, ref num))
		{
			return this.value;
		}
		return this.LocalToValue(cachedTransform.InverseTransformPoint(ray.GetPoint(num)));
	}

	protected virtual float LocalToValue(Vector2 localPos)
	{
		if (!(this.mFG != null))
		{
			return this.value;
		}
		Vector3[] localCorners = this.mFG.localCorners;
		Vector3 vector = localCorners[2] - localCorners[0];
		if (this.isHorizontal)
		{
			float num = (localPos.x - localCorners[0].x) / vector.x;
			return (!this.isInverted) ? num : (1f - num);
		}
		float num2 = (localPos.y - localCorners[0].y) / vector.y;
		return (!this.isInverted) ? num2 : (1f - num2);
	}

	public virtual void ForceUpdate()
	{
		this.mIsDirty = false;
		bool flag = false;
		if (this.mFG != null)
		{
			UIBasicSprite uIBasicSprite = this.mFG as UIBasicSprite;
			if (this.isHorizontal)
			{
				if (uIBasicSprite != null && uIBasicSprite.type == UIBasicSprite.Type.Filled)
				{
					if (uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
					{
						uIBasicSprite.fillDirection = UIBasicSprite.FillDirection.Horizontal;
						uIBasicSprite.invert = this.isInverted;
					}
					uIBasicSprite.fillAmount = this.value;
				}
				else
				{
					this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, this.value, 1f) : new Vector4(1f - this.value, 0f, 1f, 1f));
					this.mFG.set_enabled(true);
					flag = (this.value < 0.001f);
				}
			}
			else if (uIBasicSprite != null && uIBasicSprite.type == UIBasicSprite.Type.Filled)
			{
				if (uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Horizontal || uIBasicSprite.fillDirection == UIBasicSprite.FillDirection.Vertical)
				{
					uIBasicSprite.fillDirection = UIBasicSprite.FillDirection.Vertical;
					uIBasicSprite.invert = this.isInverted;
				}
				uIBasicSprite.fillAmount = this.value;
			}
			else
			{
				this.mFG.drawRegion = ((!this.isInverted) ? new Vector4(0f, 0f, 1f, this.value) : new Vector4(0f, 1f - this.value, 1f, 1f));
				this.mFG.set_enabled(true);
				flag = (this.value < 0.001f);
			}
		}
		if (this.thumb != null && (this.mFG != null || this.mBG != null))
		{
			Vector3[] array = (!(this.mFG != null)) ? this.mBG.localCorners : this.mFG.localCorners;
			Vector4 vector = (!(this.mFG != null)) ? this.mBG.border : this.mFG.border;
			Vector3[] expr_24E_cp_0 = array;
			int expr_24E_cp_1 = 0;
			expr_24E_cp_0[expr_24E_cp_1].x = expr_24E_cp_0[expr_24E_cp_1].x + vector.x;
			Vector3[] expr_268_cp_0 = array;
			int expr_268_cp_1 = 1;
			expr_268_cp_0[expr_268_cp_1].x = expr_268_cp_0[expr_268_cp_1].x + vector.x;
			Vector3[] expr_282_cp_0 = array;
			int expr_282_cp_1 = 2;
			expr_282_cp_0[expr_282_cp_1].x = expr_282_cp_0[expr_282_cp_1].x - vector.z;
			Vector3[] expr_29C_cp_0 = array;
			int expr_29C_cp_1 = 3;
			expr_29C_cp_0[expr_29C_cp_1].x = expr_29C_cp_0[expr_29C_cp_1].x - vector.z;
			Vector3[] expr_2B6_cp_0 = array;
			int expr_2B6_cp_1 = 0;
			expr_2B6_cp_0[expr_2B6_cp_1].y = expr_2B6_cp_0[expr_2B6_cp_1].y + vector.y;
			Vector3[] expr_2D0_cp_0 = array;
			int expr_2D0_cp_1 = 1;
			expr_2D0_cp_0[expr_2D0_cp_1].y = expr_2D0_cp_0[expr_2D0_cp_1].y - vector.w;
			Vector3[] expr_2EA_cp_0 = array;
			int expr_2EA_cp_1 = 2;
			expr_2EA_cp_0[expr_2EA_cp_1].y = expr_2EA_cp_0[expr_2EA_cp_1].y - vector.w;
			Vector3[] expr_304_cp_0 = array;
			int expr_304_cp_1 = 3;
			expr_304_cp_0[expr_304_cp_1].y = expr_304_cp_0[expr_304_cp_1].y + vector.y;
			Transform transform = (!(this.mFG != null)) ? this.mBG.cachedTransform : this.mFG.cachedTransform;
			for (int i = 0; i < 4; i++)
			{
				array[i] = transform.TransformPoint(array[i]);
			}
			if (this.isHorizontal)
			{
				Vector3 vector2 = Vector3.Lerp(array[0], array[1], 0.5f);
				Vector3 vector3 = Vector3.Lerp(array[2], array[3], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector2, vector3, (!this.isInverted) ? this.value : (1f - this.value)));
			}
			else
			{
				Vector3 vector4 = Vector3.Lerp(array[0], array[3], 0.5f);
				Vector3 vector5 = Vector3.Lerp(array[1], array[2], 0.5f);
				this.SetThumbPosition(Vector3.Lerp(vector4, vector5, (!this.isInverted) ? this.value : (1f - this.value)));
			}
		}
		if (flag)
		{
			this.mFG.set_enabled(false);
		}
	}

	protected void SetThumbPosition(Vector3 worldPos)
	{
		Transform parent = this.thumb.get_parent();
		if (parent != null)
		{
			worldPos = parent.InverseTransformPoint(worldPos);
			worldPos.x = Mathf.Round(worldPos.x);
			worldPos.y = Mathf.Round(worldPos.y);
			worldPos.z = 0f;
			if (Vector3.Distance(this.thumb.get_localPosition(), worldPos) > 0.001f)
			{
				this.thumb.set_localPosition(worldPos);
			}
		}
		else if (Vector3.Distance(this.thumb.get_position(), worldPos) > 1E-05f)
		{
			this.thumb.set_position(worldPos);
		}
	}
}
