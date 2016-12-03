using DG.Tweening;
using System;
using UnityEngine;

public class UINumberCounter
{
	public enum AnimationType
	{
		Random,
		RandomRange,
		Count
	}

	private Action mOnFinishedCallBack;

	private UILabel mLabel_Target;

	private int mFrom;

	private int mTo;

	private float mDurationSeconds;

	private UINumberCounter.AnimationType mAnimationType = UINumberCounter.AnimationType.Count;

	public UINumberCounter(UILabel targetLabel)
	{
		this.SetFrom(0);
		this.SetTo(0);
		this.SetOnFinishedCallBack(null);
		this.SetDuration(0f);
		this.SetLabel(targetLabel);
	}

	public UINumberCounter SetLabel(UILabel targetLabel)
	{
		this.mLabel_Target = targetLabel;
		return this;
	}

	public UINumberCounter SetFrom(int from)
	{
		this.mFrom = from;
		return this;
	}

	public UINumberCounter SetTo(int to)
	{
		this.mTo = to;
		return this;
	}

	public UINumberCounter SetDuration(float duration)
	{
		this.mDurationSeconds = duration;
		return this;
	}

	public UINumberCounter SetAnimationType(UINumberCounter.AnimationType type)
	{
		this.mAnimationType = type;
		return this;
	}

	public UINumberCounter SetOnFinishedCallBack(Action onFinishedCallBack)
	{
		this.mOnFinishedCallBack = onFinishedCallBack;
		return this;
	}

	private void OnFinishedCallBack()
	{
		if (this.mOnFinishedCallBack != null)
		{
			this.mOnFinishedCallBack.Invoke();
		}
	}

	public Tween Buld()
	{
		Tween tween = null;
		switch (this.mAnimationType)
		{
		case UINumberCounter.AnimationType.Random:
			tween = this.PlayWithRandom();
			TweenSettingsExtensions.SetId<Tween>(tween, this);
			return tween;
		case UINumberCounter.AnimationType.RandomRange:
			tween = this.PlayWithRandomRange();
			TweenSettingsExtensions.SetId<Tween>(tween, this);
			return tween;
		case UINumberCounter.AnimationType.Count:
			tween = this.PlayCount();
			TweenSettingsExtensions.SetId<Tween>(tween, this);
			return tween;
		default:
			return tween;
		}
	}

	public Tween PlayWithRandom()
	{
		int digit = this.mTo.ToString().get_Length();
		return TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float((float)this.mFrom, (float)this.mTo, this.mDurationSeconds, delegate(float currentValue)
		{
			string text = string.Empty;
			for (int i = 0; i < digit; i++)
			{
				text += Random.Range(0, 9).ToString();
			}
			this.mLabel_Target.text = text;
		}), delegate
		{
			this.mLabel_Target.text = this.mTo.ToString();
			this.OnFinishedCallBack();
		}), 1);
	}

	public Tween PlayWithRandomRange()
	{
		return TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float((float)this.mFrom, (float)this.mTo, this.mDurationSeconds, delegate(float currentValue)
		{
			this.mLabel_Target.text = Random.Range(this.mFrom, this.mTo).ToString();
		}), delegate
		{
			this.mLabel_Target.text = this.mTo.ToString();
			this.OnFinishedCallBack();
		}), 1);
	}

	public Tween PlayCount()
	{
		return TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float((float)this.mFrom, (float)this.mTo, this.mDurationSeconds, delegate(float currentValue)
		{
			this.mLabel_Target.text = ((int)currentValue).ToString();
		}), delegate
		{
			this.OnFinishedCallBack();
		}), 1);
	}

	private void OnDestroy()
	{
		if (DOTween.IsTweening(this))
		{
			DOTween.Kill(this, false);
		}
	}
}
