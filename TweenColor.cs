using System;
using UnityEngine;

[AddComponentMenu("NGUI/Tween/Tween Color")]
public class TweenColor : UITweener
{
	public Color from = Color.get_white();

	public Color to = Color.get_white();

	private bool mCached;

	private UIWidget mWidget;

	private Material mMat;

	private Light mLight;

	private SpriteRenderer mSr;

	[Obsolete("Use 'value' instead")]
	public Color color
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

	public Color value
	{
		get
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				return this.mWidget.color;
			}
			if (this.mMat != null)
			{
				return this.mMat.get_color();
			}
			if (this.mSr != null)
			{
				return this.mSr.get_color();
			}
			if (this.mLight != null)
			{
				return this.mLight.get_color();
			}
			return Color.get_black();
		}
		set
		{
			if (!this.mCached)
			{
				this.Cache();
			}
			if (this.mWidget != null)
			{
				this.mWidget.color = value;
			}
			else if (this.mMat != null)
			{
				this.mMat.set_color(value);
			}
			else if (this.mSr != null)
			{
				this.mSr.set_color(value);
			}
			else if (this.mLight != null)
			{
				this.mLight.set_color(value);
				this.mLight.set_enabled(value.r + value.g + value.b > 0.01f);
			}
		}
	}

	private void Cache()
	{
		this.mCached = true;
		this.mWidget = base.GetComponent<UIWidget>();
		if (this.mWidget != null)
		{
			return;
		}
		this.mSr = base.GetComponent<SpriteRenderer>();
		if (this.mSr != null)
		{
			return;
		}
		Renderer component = base.GetComponent<Renderer>();
		if (component != null)
		{
			this.mMat = component.get_material();
			return;
		}
		this.mLight = base.GetComponent<Light>();
		if (this.mLight == null)
		{
			this.mWidget = base.GetComponentInChildren<UIWidget>();
		}
	}

	protected override void OnUpdate(float factor, bool isFinished)
	{
		this.value = Color.Lerp(this.from, this.to, factor);
	}

	public static TweenColor Begin(GameObject go, float duration, Color color)
	{
		TweenColor tweenColor = UITweener.Begin<TweenColor>(go, duration);
		tweenColor.from = tweenColor.value;
		tweenColor.to = color;
		if (duration <= 0f)
		{
			tweenColor.Sample(1f, true);
			tweenColor.set_enabled(false);
		}
		return tweenColor;
	}

	[ContextMenu("Set 'From' to current value")]
	public override void SetStartToCurrentValue()
	{
		this.from = this.value;
	}

	[ContextMenu("Set 'To' to current value")]
	public override void SetEndToCurrentValue()
	{
		this.to = this.value;
	}

	[ContextMenu("Assume value of 'From'")]
	private void SetCurrentValueToStart()
	{
		this.value = this.from;
	}

	[ContextMenu("Assume value of 'To'")]
	private void SetCurrentValueToEnd()
	{
		this.value = this.to;
	}
}
