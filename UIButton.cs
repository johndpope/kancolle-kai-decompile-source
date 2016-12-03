using System;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button")]
public class UIButton : UIButtonColor
{
	public delegate void CallBackSetState(UIButton button);

	public UIButton.CallBackSetState callBack;

	public bool isOnHover;

	public static UIButton current;

	public bool dragHighlight;

	public string hoverSprite;

	public string pressedSprite;

	public string disabledSprite;

	public Sprite hoverSprite2D;

	public Sprite pressedSprite2D;

	public Sprite disabledSprite2D;

	public bool pixelSnap;

	public List<EventDelegate> onClick = new List<EventDelegate>();

	[NonSerialized]
	private UISprite mSprite;

	[NonSerialized]
	private UI2DSprite mSprite2D;

	[NonSerialized]
	private string mNormalSprite;

	[NonSerialized]
	private Sprite mNormalSprite2D;

	public override bool isEnabled
	{
		get
		{
			if (!base.get_enabled())
			{
				return false;
			}
			Collider component = base.get_gameObject().GetComponent<Collider>();
			if (component && component.get_enabled())
			{
				return true;
			}
			Collider2D component2 = base.GetComponent<Collider2D>();
			return component2 && component2.get_enabled();
		}
		set
		{
			if (this.isEnabled != value)
			{
				Collider component = base.get_gameObject().GetComponent<Collider>();
				if (component != null)
				{
					component.set_enabled(value);
					UIButton[] components = base.GetComponents<UIButton>();
					UIButton[] array = components;
					for (int i = 0; i < array.Length; i++)
					{
						UIButton uIButton = array[i];
						uIButton.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
					}
				}
				else
				{
					Collider2D component2 = base.GetComponent<Collider2D>();
					if (component2 != null)
					{
						component2.set_enabled(value);
						UIButton[] components2 = base.GetComponents<UIButton>();
						UIButton[] array2 = components2;
						for (int j = 0; j < array2.Length; j++)
						{
							UIButton uIButton2 = array2[j];
							uIButton2.SetState((!value) ? UIButtonColor.State.Disabled : UIButtonColor.State.Normal, false);
						}
					}
					else
					{
						base.set_enabled(value);
					}
				}
			}
		}
	}

	public string normalSprite
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite != null && !string.IsNullOrEmpty(this.mNormalSprite) && this.mNormalSprite == this.mSprite.spriteName)
			{
				this.mNormalSprite = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	public Sprite normalSprite2D
	{
		get
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			return this.mNormalSprite2D;
		}
		set
		{
			if (!this.mInitDone)
			{
				this.OnInit();
			}
			if (this.mSprite2D != null && this.mNormalSprite2D == this.mSprite2D.sprite2D)
			{
				this.mNormalSprite2D = value;
				this.SetSprite(value);
				NGUITools.SetDirty(this.mSprite);
			}
			else
			{
				this.mNormalSprite2D = value;
				if (this.mState == UIButtonColor.State.Normal)
				{
					this.SetSprite(value);
				}
			}
		}
	}

	protected override void OnCallDestroy()
	{
		if (this.onClick != null)
		{
			this.onClick.Clear();
		}
		this.onClick = null;
	}

	public UISprite GetSprite()
	{
		return base.GetComponent<UISprite>();
	}

	public void SetEnableCollider2D(bool enabled)
	{
		Collider2D component = base.GetComponent<Collider2D>();
		component.set_enabled(enabled);
	}

	protected override void OnInit()
	{
		base.OnInit();
		this.mSprite = (this.mWidget as UISprite);
		this.mSprite2D = (this.mWidget as UI2DSprite);
		if (this.mSprite != null)
		{
			this.mNormalSprite = this.mSprite.spriteName;
		}
		if (this.mSprite2D != null)
		{
			this.mNormalSprite2D = this.mSprite2D.sprite2D;
		}
	}

	protected override void OnEnable()
	{
		if (this.isEnabled)
		{
			if (this.mInitDone)
			{
				if (UICamera.currentScheme == UICamera.ControlScheme.Controller)
				{
					this.OnHover(UICamera.selectedObject == base.get_gameObject());
				}
				else if (UICamera.currentScheme == UICamera.ControlScheme.Mouse)
				{
					this.OnHover(UICamera.hoveredObject == base.get_gameObject());
				}
				else if (!this.OnEnableAndOnDisableChangeState)
				{
					this.SetState(UIButtonColor.State.Normal, false);
				}
			}
		}
		else
		{
			this.SetState(UIButtonColor.State.Disabled, true);
		}
	}

	protected override void OnDragOver()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.get_gameObject()))
		{
			base.OnDragOver();
		}
	}

	protected override void OnDragOut()
	{
		if (this.isEnabled && (this.dragHighlight || UICamera.currentTouch.pressed == base.get_gameObject()))
		{
			base.OnDragOut();
		}
	}

	protected virtual void OnClick()
	{
		if (UIButton.current == null && this.isEnabled)
		{
			UIButton.current = this;
			EventDelegate.Execute(this.onClick);
			UIButton.current = null;
		}
	}

	public override void SetState(UIButtonColor.State state, bool immediate)
	{
		base.SetState(state, immediate);
		if (this.mSprite != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!string.IsNullOrEmpty(this.hoverSprite)) ? this.hoverSprite : this.mNormalSprite);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite);
				break;
			}
		}
		else if (this.mSprite2D != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!(this.hoverSprite2D == null)) ? this.hoverSprite2D : this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite2D);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite2D);
				break;
			}
		}
		if (this.callBack != null && (state == UIButtonColor.State.Hover || state == UIButtonColor.State.Pressed))
		{
			this.callBack(this);
		}
	}

	public void SetStateNonCallBack(UIButtonColor.State state)
	{
		base.SetState(state, true);
		if (this.mSprite != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!string.IsNullOrEmpty(this.hoverSprite)) ? this.hoverSprite : this.mNormalSprite);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite);
				break;
			}
		}
		else if (this.mSprite2D != null)
		{
			switch (state)
			{
			case UIButtonColor.State.Normal:
				this.SetSprite(this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Hover:
				this.SetSprite((!(this.hoverSprite2D == null)) ? this.hoverSprite2D : this.mNormalSprite2D);
				break;
			case UIButtonColor.State.Pressed:
				this.SetSprite(this.pressedSprite2D);
				break;
			case UIButtonColor.State.Disabled:
				this.SetSprite(this.disabledSprite2D);
				break;
			}
		}
	}

	protected void SetSprite(string sp)
	{
		if (this.mSprite != null && !string.IsNullOrEmpty(sp) && this.mSprite.spriteName != sp)
		{
			this.mSprite.spriteName = sp;
			if (this.pixelSnap)
			{
				this.mSprite.MakePixelPerfect();
			}
		}
	}

	protected void SetSprite(Sprite sp)
	{
		if (sp != null && this.mSprite2D != null && this.mSprite2D.sprite2D != sp)
		{
			this.mSprite2D.sprite2D = sp;
			if (this.pixelSnap)
			{
				this.mSprite2D.MakePixelPerfect();
			}
		}
	}

	internal void Release()
	{
		if (this != null)
		{
			if (this.mSprite != null)
			{
				this.mSprite.RemoveFromPanel();
				this.mSprite.atlas = null;
				this.mSprite = null;
			}
			if (this.onClick != null)
			{
				this.onClick.Clear();
			}
		}
	}
}
