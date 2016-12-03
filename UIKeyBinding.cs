using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Key Binding")]
public class UIKeyBinding : MonoBehaviour
{
	public enum Action
	{
		PressAndClick,
		Select,
		All
	}

	public enum Modifier
	{
		Any,
		Shift,
		Control,
		Alt,
		None
	}

	public KeyCode keyCode;

	public UIKeyBinding.Modifier modifier;

	public UIKeyBinding.Action action;

	private bool mIgnoreUp;

	private bool mIsInput;

	private bool mPress;

	protected virtual void Start()
	{
		UIInput component = base.GetComponent<UIInput>();
		this.mIsInput = (component != null);
		if (component != null)
		{
			EventDelegate.Add(component.onSubmit, new EventDelegate.Callback(this.OnSubmit));
		}
	}

	protected virtual void OnSubmit()
	{
		if (UICamera.currentKey == this.keyCode && this.IsModifierActive())
		{
			this.mIgnoreUp = true;
		}
	}

	protected virtual bool IsModifierActive()
	{
		if (this.modifier == UIKeyBinding.Modifier.Any)
		{
			return true;
		}
		if (this.modifier == UIKeyBinding.Modifier.Alt)
		{
			if (Input.GetKey(308) || Input.GetKey(307))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Control)
		{
			if (Input.GetKey(306) || Input.GetKey(305))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.Shift)
		{
			if (Input.GetKey(304) || Input.GetKey(303))
			{
				return true;
			}
		}
		else if (this.modifier == UIKeyBinding.Modifier.None)
		{
			return !Input.GetKey(308) && !Input.GetKey(307) && !Input.GetKey(306) && !Input.GetKey(305) && !Input.GetKey(304) && !Input.GetKey(303);
		}
		return false;
	}

	protected virtual void Update()
	{
		if (UICamera.inputHasFocus)
		{
			return;
		}
		if (this.keyCode == null || !this.IsModifierActive())
		{
			return;
		}
		bool keyDown = Input.GetKeyDown(this.keyCode);
		bool keyUp = Input.GetKeyUp(this.keyCode);
		if (keyDown)
		{
			this.mPress = true;
		}
		if (this.action == UIKeyBinding.Action.PressAndClick || this.action == UIKeyBinding.Action.All)
		{
			if (keyDown)
			{
				UICamera.currentTouch = UICamera.controller;
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = base.get_gameObject();
				this.OnBindingPress(true);
				UICamera.currentTouch.current = null;
			}
			if (this.mPress && keyUp)
			{
				UICamera.currentTouch = UICamera.controller;
				UICamera.currentScheme = UICamera.ControlScheme.Controller;
				UICamera.currentTouch.current = base.get_gameObject();
				this.OnBindingPress(false);
				this.OnBindingClick();
				UICamera.currentTouch.current = null;
			}
		}
		if ((this.action == UIKeyBinding.Action.Select || this.action == UIKeyBinding.Action.All) && keyUp)
		{
			if (this.mIsInput)
			{
				if (!this.mIgnoreUp && !UICamera.inputHasFocus && this.mPress)
				{
					UICamera.selectedObject = base.get_gameObject();
				}
				this.mIgnoreUp = false;
			}
			else if (this.mPress)
			{
				UICamera.selectedObject = base.get_gameObject();
			}
		}
		if (keyUp)
		{
			this.mPress = false;
		}
	}

	protected virtual void OnBindingPress(bool pressed)
	{
		UICamera.Notify(base.get_gameObject(), "OnPress", pressed);
	}

	protected virtual void OnBindingClick()
	{
		UICamera.Notify(base.get_gameObject(), "OnClick", null);
	}
}
