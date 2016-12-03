using System;
using UnityEngine;

[AddComponentMenu("NGUI/Interaction/Button Rotation")]
public class UIButtonRotation : MonoBehaviour
{
	public Transform tweenTarget;

	public Vector3 hover = Vector3.get_zero();

	public Vector3 pressed = Vector3.get_zero();

	public float duration = 0.2f;

	private Quaternion mRot;

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
			this.mRot = this.tweenTarget.get_localRotation();
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
			TweenRotation component = this.tweenTarget.GetComponent<TweenRotation>();
			if (component != null)
			{
				component.value = this.mRot;
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
			TweenRotation.Begin(this.tweenTarget.get_gameObject(), this.duration, (!isPressed) ? ((!UICamera.IsHighlighted(base.get_gameObject())) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))) : (this.mRot * Quaternion.Euler(this.pressed))).method = UITweener.Method.EaseInOut;
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
			TweenRotation.Begin(this.tweenTarget.get_gameObject(), this.duration, (!isOver) ? this.mRot : (this.mRot * Quaternion.Euler(this.hover))).method = UITweener.Method.EaseInOut;
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
