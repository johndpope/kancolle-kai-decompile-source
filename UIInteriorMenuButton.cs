using Common.Enum;
using DG.Tweening;
using KCV.Scene.Port;
using System;
using UnityEngine;

public class UIInteriorMenuButton : MonoBehaviour
{
	[SerializeField]
	private UITexture mTexture_Area;

	[SerializeField]
	private UISprite mSprite_Menu;

	[SerializeField]
	private UIButton mButton_Menu;

	[SerializeField]
	private UISprite mSprite_Yousei;

	private Action mOnClickListener;

	public FurnitureKinds mFurnitureKind
	{
		get;
		private set;
	}

	private void Awake()
	{
		this.mSprite_Yousei.spriteName = "mini_08_a_02";
		this.mSprite_Yousei.get_transform().localPositionY(-25f);
		this.mSprite_Yousei.fillAmount = 0.5f;
	}

	public void Initialize(FurnitureKinds furnitureKind)
	{
		this.mFurnitureKind = furnitureKind;
	}

	public void RemoveFocus()
	{
		DOTween.Kill(this, false);
		this.mSprite_Yousei.spriteName = "mini_08_a_02";
		this.mButton_Menu.SetState(UIButtonColor.State.Normal, true);
		Sequence sequence = DOTween.Sequence();
		Tween tween = ShortcutExtensions.DOLocalMoveY(this.mSprite_Yousei.get_transform(), -25f, 0.3f, false);
		Tween tween2 = DOVirtual.Float(this.mSprite_Yousei.fillAmount, 0.5f, 0.3f, delegate(float percentage)
		{
			this.mSprite_Yousei.fillAmount = percentage;
		});
		Tween tween3 = DOVirtual.Float(this.mTexture_Area.alpha, 0f, 0.5f, delegate(float percentage)
		{
			this.mTexture_Area.alpha = percentage;
		});
		TweenSettingsExtensions.Append(sequence, tween);
		TweenSettingsExtensions.Join(sequence, tween2);
		TweenSettingsExtensions.Join(sequence, tween3);
		TweenSettingsExtensions.SetId<Sequence>(sequence, this);
	}

	public void SetEnableButton(bool enable)
	{
		this.mButton_Menu.isEnabled = enable;
	}

	public void Focus()
	{
		DOTween.Kill(this, false);
		this.mSprite_Yousei.spriteName = "mini_08_a_01";
		this.mButton_Menu.SetState(UIButtonColor.State.Hover, true);
		Sequence sequence = DOTween.Sequence();
		Tween tween = ShortcutExtensions.DOLocalMoveY(this.mSprite_Yousei.get_transform(), 50f, 0.3f, false);
		Tween tween2 = DOVirtual.Float(this.mSprite_Yousei.fillAmount, 1f, 0.3f, delegate(float percentage)
		{
			this.mSprite_Yousei.fillAmount = percentage;
		});
		Tween tween3 = DOVirtual.Float(0.2f, 1f, 0.3f, delegate(float percentage)
		{
			this.mTexture_Area.alpha = percentage;
		});
		this.mTexture_Area.get_transform().set_localScale(new Vector3(0.1f, 0.1f));
		Tween tween4 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOScale(this.mTexture_Area.get_transform(), Vector3.get_one(), 0.1f), 18);
		TweenSettingsExtensions.Append(sequence, tween);
		TweenSettingsExtensions.Join(sequence, tween2);
		TweenSettingsExtensions.Join(sequence, tween3);
		TweenSettingsExtensions.Join(sequence, tween4);
		TweenSettingsExtensions.OnComplete<Sequence>(sequence, delegate
		{
			Tween tween5 = TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(TweenSettingsExtensions.SetId<Tweener>(DOVirtual.Float(1f, 0.5f, 1.5f, delegate(float percentage)
			{
				this.mTexture_Area.alpha = percentage;
			}), this), 2147483647, 1), 21);
		});
		TweenSettingsExtensions.SetId<Sequence>(sequence, this);
	}

	public void Click()
	{
		this.mSprite_Yousei.spriteName = "mini_08_a_03";
		TweenSettingsExtensions.OnComplete<Tween>(TweenSettingsExtensions.SetId<Tween>(DOVirtual.DelayedCall(0.1f, delegate
		{
			this.mSprite_Yousei.spriteName = "mini_08_a_04";
		}, true), this), delegate
		{
			if (this.mOnClickListener != null)
			{
				this.mOnClickListener.Invoke();
			}
		});
	}

	public void SetOnClickListener(Action onClickListener)
	{
		this.mOnClickListener = onClickListener;
	}

	[Obsolete("Inspector上でイベントを設定する為に使用するのでスクリプトないでは使用しないでください")]
	public void OnTouchYousei()
	{
		this.Click();
	}

	private void OnDestroy()
	{
		DOTween.Kill(this, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Area, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Menu);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mSprite_Yousei);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_Menu);
		this.mOnClickListener = null;
	}
}
