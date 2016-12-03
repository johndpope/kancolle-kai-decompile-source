using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Message (Legacy)")]
public class UIButtonMessage : MonoBehaviour
{
	public enum Trigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
		OnDoubleClick
	}

	public GameObject target;

	public string functionName;

	public UIButtonMessage.Trigger trigger;

	public bool includeChildren;

	private bool mStarted;

	private void Start()
	{
		this.mStarted = true;
	}

	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.get_gameObject()));
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.get_enabled() && ((isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOver) || (!isOver && this.trigger == UIButtonMessage.Trigger.OnMouseOut)))
		{
			this.Send();
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.get_enabled() && ((isPressed && this.trigger == UIButtonMessage.Trigger.OnPress) || (!isPressed && this.trigger == UIButtonMessage.Trigger.OnRelease)))
		{
			this.Send();
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.get_enabled() && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}

	private void OnClick()
	{
		if (base.get_enabled() && this.trigger == UIButtonMessage.Trigger.OnClick)
		{
			this.Send();
		}
	}

	private void OnDoubleClick()
	{
		if (base.get_enabled() && this.trigger == UIButtonMessage.Trigger.OnDoubleClick)
		{
			this.Send();
		}
	}

	private void Send()
	{
		if (string.IsNullOrEmpty(this.functionName))
		{
			return;
		}
		if (this.target == null)
		{
			this.target = base.get_gameObject();
		}
		if (this.includeChildren)
		{
			Transform[] componentsInChildren = this.target.GetComponentsInChildren<Transform>();
			int i = 0;
			int num = componentsInChildren.Length;
			while (i < num)
			{
				Transform transform = componentsInChildren[i];
				transform.get_gameObject().SendMessage(this.functionName, base.get_gameObject(), 1);
				i++;
			}
		}
		else
		{
			this.target.SendMessage(this.functionName, base.get_gameObject(), 1);
		}
	}
}
