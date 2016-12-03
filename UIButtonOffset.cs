using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Offset")]
public class UIButtonOffset : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.get_zero();

	public Vector3 pressed = new Vector3(2f, -2f);

	public float duration = 0.2f;

	private Vector3 mPos;

	private bool mStarted;

	private void Start()
	{
		if (!this.mStarted)
		{
			this.mStarted = true;
			if (this.tweenTarget == null)
			{
				this.tweenTarget = base.get_transform();
			}
			this.mPos = this.tweenTarget.get_localPosition();
		}
	}

	private void OnEnable()
	{
		if (this.mStarted)
		{
			this.OnHover(UICamera.IsHighlighted(base.get_gameObject()));
		}
	}

	private void OnDisable()
	{
		if (this.mStarted && this.tweenTarget != null)
		{
			TweenPosition component = this.tweenTarget.GetComponent<TweenPosition>();
			if (component != null)
			{
				component.value = this.mPos;
				component.set_enabled(false);
			}
		}
	}

	private void OnPress(bool isPressed)
	{
		if (base.get_enabled())
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.get_gameObject(), this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.get_gameObject())) ? this.mPos : (this.mPos + this.hover)) : (this.mPos + this.pressed)).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnHover(bool isOver)
	{
		if (base.get_enabled())
		{
			if (!this.mStarted)
			{
				this.Start();
			}
			TweenPosition.Begin(this.tweenTarget.get_gameObject(), this.duration, (!isOver) ? this.mPos : (this.mPos + this.hover)).method = UITweener.Method.EaseInOut;
		}
	}

	private void OnSelect(bool isSelected)
	{
		if (base.get_enabled() && (!isSelected || UICamera.currentScheme == UICamera.ControlScheme.Controller))
		{
			this.OnHover(isSelected);
		}
	}
}
