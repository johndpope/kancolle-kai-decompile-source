using DG.Tweening;
using System;
using UnityEngine;

public class UIDeckPracticeShip : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_SplayTail;

	[SerializeField]
	private UITexture mTexture_SplayTail_Delay;

	[SerializeField]
	private UITexture mTexture_SplayHead;

	private int mDefaultSplayTailWidth;

	private void Awake()
	{
		this.mDefaultSplayTailWidth = this.mTexture_SplayTail.width;
	}

	private void Start()
	{
		this.GenerateSplayHeadAnimation();
		this.GenerateLoopTailAnimation();
	}

	private Tween GenerateSplayHeadAnimation()
	{
		return TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions.DOScaleY(this.mTexture_SplayHead.get_transform(), Random.RandomRange(0.5f, 0.8f), 0.25f), 2147483647), this);
	}

	private Tween GenerateLoopTailAnimation()
	{
		Tween result = TweenSettingsExtensions.SetId<Tween>(TweenSettingsExtensions.SetLoops<Tween>(this.GenerateSplayAnimation(this.mTexture_SplayTail), 2147483647, 0), this);
		TweenSettingsExtensions.SetId<Tween>(DOVirtual.DelayedCall(1.5f, delegate
		{
			Tween tween = TweenSettingsExtensions.SetLoops<Tween>(this.GenerateSplayAnimation(this.mTexture_SplayTail_Delay), 2147483647, 0);
		}, true), this);
		return result;
	}

	private Tween GenerateSplayAnimation(UITexture texture)
	{
		Tween tween = TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(0f, 1f, 3f, delegate(float percentage)
		{
			texture.width = (int)((float)this.mDefaultSplayTailWidth + 30f * percentage);
		}), this);
		Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(0f, 1f, 1.5f, delegate(float percentage)
		{
			texture.alpha = percentage;
		}), this)), TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(1f, 0f, 1.5f, delegate(float percentage)
		{
			texture.alpha = percentage;
		}), this)), this);
		Sequence sequence2 = DOTween.Sequence();
		TweenSettingsExtensions.Join(sequence2, tween);
		TweenSettingsExtensions.Join(sequence2, sequence);
		TweenSettingsExtensions.OnPlay<Sequence>(sequence2, delegate
		{
			texture.alpha = 0f;
			texture.width = this.mDefaultSplayTailWidth;
		});
		TweenSettingsExtensions.SetId<Sequence>(sequence2, this);
		return sequence2;
	}

	private void OnDestroy()
	{
		bool flag = DOTween.IsTweening(this);
		if (flag)
		{
			DOTween.Kill(this, false);
		}
		this.mTexture_SplayTail = null;
		this.mTexture_SplayTail_Delay = null;
		this.mTexture_SplayHead = null;
	}
}
