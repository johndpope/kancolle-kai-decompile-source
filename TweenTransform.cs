using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Transform")]
public class TweenTransform : UITweener
{
	public Transform from;

	public Transform to;

	public bool parentWhenFinished;

	private Transform mTrans;

	private Vector3 mPos;

	private Quaternion mRot;

	private Vector3 mScale;

	protected override void OnUpdate(float factor, bool isFinished)
	{
		if (this.to != null)
		{
			if (this.mTrans == null)
			{
				this.mTrans = base.get_transform();
				this.mPos = this.mTrans.get_position();
				this.mRot = this.mTrans.get_rotation();
				this.mScale = this.mTrans.get_localScale();
			}
			if (this.from != null)
			{
				this.mTrans.set_position(this.from.get_position() * (1f - factor) + this.to.get_position() * factor);
				this.mTrans.set_localScale(this.from.get_localScale() * (1f - factor) + this.to.get_localScale() * factor);
				this.mTrans.set_rotation(Quaternion.Slerp(this.from.get_rotation(), this.to.get_rotation(), factor));
			}
			else
			{
				this.mTrans.set_position(this.mPos * (1f - factor) + this.to.get_position() * factor);
				this.mTrans.set_localScale(this.mScale * (1f - factor) + this.to.get_localScale() * factor);
				this.mTrans.set_rotation(Quaternion.Slerp(this.mRot, this.to.get_rotation(), factor));
			}
			if (this.parentWhenFinished && isFinished)
			{
				this.mTrans.set_parent(this.to);
			}
		}
	}

	public static TweenTransform Begin(GameObject go, float duration, Transform to)
	{
		return TweenTransform.Begin(go, duration, null, to);
	}

	public static TweenTransform Begin(GameObject go, float duration, Transform from, Transform to)
	{
		TweenTransform tweenTransform = UITweener.Begin<TweenTransform>(go, duration);
		tweenTransform.from = from;
		tweenTransform.to = to;
		if (duration <= 0f)
		{
			tweenTransform.Sample(1f, true);
			tweenTransform.set_enabled(false);
		}
		return tweenTransform;
	}
}
