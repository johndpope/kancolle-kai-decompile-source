using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/NGUI Slider"), ExecuteInEditMode]
public class UISlider : UIProgressBar
{
	private enum Direction
	{
		Horizontal,
		Vertical,
		Upgraded
	}

	[HideInInspector, SerializeField]
	private Transform foreground;

	[HideInInspector, SerializeField]
	private float rawValue = 1f;

	[HideInInspector, SerializeField]
	private UISlider.Direction direction = UISlider.Direction.Upgraded;

	[HideInInspector, SerializeField]
	protected bool mInverted;

	[Obsolete("Use 'value' instead")]
	public float sliderValue
	{
		get
		{
			return base.value;
		}
		set
		{
			base.value = value;
		}
	}

	[Obsolete("Use 'fillDirection' instead")]
	public bool inverted
	{
		get
		{
			return base.isInverted;
		}
		set
		{
		}
	}

	protected override void Upgrade()
	{
		if (this.direction != UISlider.Direction.Upgraded)
		{
			this.mValue = this.rawValue;
			if (this.foreground != null)
			{
				this.mFG = this.foreground.GetComponent<UIWidget>();
			}
			if (this.direction == UISlider.Direction.Horizontal)
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.LeftToRight : UIProgressBar.FillDirection.RightToLeft);
			}
			else
			{
				this.mFill = ((!this.mInverted) ? UIProgressBar.FillDirection.BottomToTop : UIProgressBar.FillDirection.TopToBottom);
			}
			this.direction = UISlider.Direction.Upgraded;
		}
	}

	protected override void OnStart()
	{
		GameObject go = (!(this.mBG != null) || (!(this.mBG.GetComponent<Collider>() != null) && !(this.mBG.GetComponent<Collider2D>() != null))) ? base.get_gameObject() : this.mBG.get_gameObject();
		UIEventListener uIEventListener = UIEventListener.Get(go);
		UIEventListener expr_5C = uIEventListener;
		expr_5C.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_5C.onPress, new UIEventListener.BoolDelegate(this.OnPressBackground));
		UIEventListener expr_7E = uIEventListener;
		expr_7E.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_7E.onDrag, new UIEventListener.VectorDelegate(this.OnDragBackground));
		if (this.thumb != null && (this.thumb.GetComponent<Collider>() != null || this.thumb.GetComponent<Collider2D>() != null) && (this.mFG == null || this.thumb != this.mFG.cachedTransform))
		{
			UIEventListener uIEventListener2 = UIEventListener.Get(this.thumb.get_gameObject());
			UIEventListener expr_11A = uIEventListener2;
			expr_11A.onPress = (UIEventListener.BoolDelegate)Delegate.Combine(expr_11A.onPress, new UIEventListener.BoolDelegate(this.OnPressForeground));
			UIEventListener expr_13C = uIEventListener2;
			expr_13C.onDrag = (UIEventListener.VectorDelegate)Delegate.Combine(expr_13C.onDrag, new UIEventListener.VectorDelegate(this.OnDragForeground));
		}
	}

	protected void OnPressBackground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
		if (!isPressed && this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected void OnDragBackground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = base.ScreenToValue(UICamera.lastTouchPosition);
	}

	protected void OnPressForeground(GameObject go, bool isPressed)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		if (isPressed)
		{
			this.mOffset = ((!(this.mFG == null)) ? (base.value - base.ScreenToValue(UICamera.lastTouchPosition)) : 0f);
		}
		else if (this.onDragFinished != null)
		{
			this.onDragFinished();
		}
	}

	protected void OnDragForeground(GameObject go, Vector2 delta)
	{
		if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
		{
			return;
		}
		this.mCam = UICamera.currentCamera;
		base.value = this.mOffset + base.ScreenToValue(UICamera.lastTouchPosition);
	}

	protected void OnKey(KeyCode key)
	{
		if (base.get_enabled())
		{
			float num = ((float)this.numberOfSteps <= 1f) ? 0.125f : (1f / (float)(this.numberOfSteps - 1));
			switch (this.mFill)
			{
			case UIProgressBar.FillDirection.LeftToRight:
				if (key == 276)
				{
					base.value = this.mValue - num;
				}
				else if (key == 275)
				{
					base.value = this.mValue + num;
				}
				break;
			case UIProgressBar.FillDirection.RightToLeft:
				if (key == 276)
				{
					base.value = this.mValue + num;
				}
				else if (key == 275)
				{
					base.value = this.mValue - num;
				}
				break;
			case UIProgressBar.FillDirection.BottomToTop:
				if (key == 274)
				{
					base.value = this.mValue - num;
				}
				else if (key == 273)
				{
					base.value = this.mValue + num;
				}
				break;
			case UIProgressBar.FillDirection.TopToBottom:
				if (key == 274)
				{
					base.value = this.mValue + num;
				}
				else if (key == 273)
				{
					base.value = this.mValue - num;
				}
				break;
			}
		}
	}
}
