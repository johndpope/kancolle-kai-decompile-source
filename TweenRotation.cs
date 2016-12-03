using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Rotation")]
public class TweenRotation : UITweener
{
	public Vector3 from;

	public Vector3 to;

	public bool quaternionLerp;

	private Transform mTrans;

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

	[Obsolete("Use 'value' instead")]
	public Quaternion rotation
	{
		get
		{
			return this.value;
		}
		set
		{
			this.value = value;
		}
	}

	public Quaternion value
	{
		get
		{
			return this.cachedTransform.get_localRotation();
		}
		set
		{
			this.cachedTransform.set_localRotation(value);
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = ((!this.quaternionLerp) ? Quaternion.Euler(new Vector3(Mathf.Lerp(this.from.x, this.to.x, factor), Mathf.Lerp(this.from.y, this.to.y, factor), Mathf.Lerp(this.from.z, this.to.z, factor))) : Quaternion.Slerp(Quaternion.Euler(this.from), Quaternion.Euler(this.to), factor));
	}

	public static TweenRotation Begin(GameObject go, float duration, Quaternion rot)
	{
		TweenRotation tweenRotation = UITweener.Begin<TweenRotation>(go, duration);
		tweenRotation.from = tweenRotation.value.get_eulerAngles();
		tweenRotation.to = rot.get_eulerAngles();
		if (duration <= 0f)
		{
			tweenRotation.Sample(1f, true);
			tweenRotation.set_enabled(false);
		}
		return tweenRotation;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value.get_eulerAngles();
	}

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value.get_eulerAngles();
	}

	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = Quaternion.Euler(this.from);
	}

	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = Quaternion.Euler(this.to);
	}
}
