using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Scale")]
public class UIButtonScale : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = new Vector3(1.1f, 1.1f, 1.1f);

	public Vector3 pressed = new Vector3(1.05f, 1.05f, 1.05f);

	public float duration = 0.2f;

	private Vector3 mScale;

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
			this.mScale = this.tweenTarget.get_localScale();
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
			TweenScale component = this.tweenTarget.GetComponent<TweenScale>();
			if (component != null)
			{
				component.value = this.mScale;
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
			TweenScale.Begin(this.tweenTarget.get_gameObject(), this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.get_gameObject())) ? this.mScale : Vector3.Scale(this.mScale, this.hover)) : Vector3.Scale(this.mScale, this.pressed)).method = UITweener.Method.EaseInOut;
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
			TweenScale.Begin(this.tweenTarget.get_gameObject(), this.duration, (!isOver) ? this.mScale : Vector3.Scale(this.mScale, this.hover)).method = UITweener.Method.EaseInOut;
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
