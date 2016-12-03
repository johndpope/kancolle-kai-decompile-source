using DG.Tweening;
using KCV.Base;
using KCV.Scene.Port;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.UseItem
{
	public class UIUseItemReceiveFurnitureBox : PageDialog
	{
		public delegate void UIUseItemReceiveFurnitureBoxCallBack(UIUseItemReceiveFurnitureBox calledObject);

		private const int SPARK_VALUE = 10;

		[SerializeField]
		private UITexture mTexture_Box;

		[SerializeField]
		private UITexture mTexture_Lid;

		[SerializeField]
		private UITexture mTexture_Coin;

		[SerializeField]
		private UILabel mLabel_RewardText;

		[SerializeField]
		private UITexture mTexture_Flare;

		[SerializeField]
		private UITexture mTexture_Template_Sparkle;

		private UIUseItemReceiveFurnitureBox.UIUseItemReceiveFurnitureBoxCallBack mActionCallBack;

		private KeyControl mKeyController;

		private UITexture[] mTexture_Sparkles;

		private float[] mSparkleStart;

		private float[] mSparkleSc;

		private float[] mSparkleSp;

		private bool mIsFinishedAnimation;

		private float mStartTime;

		private float mFlareStart;

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnClick();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnClick();
				}
			}
		}

		public void OnClick()
		{
			this.mIsFinishedAnimation = true;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		[DebuggerHidden]
		public IEnumerator Show(int typ, int coins, Action onFinished)
		{
			UIUseItemReceiveFurnitureBox.<Show>c__Iterator97 <Show>c__Iterator = new UIUseItemReceiveFurnitureBox.<Show>c__Iterator97();
			<Show>c__Iterator.coins = coins;
			<Show>c__Iterator.typ = typ;
			<Show>c__Iterator.onFinished = onFinished;
			<Show>c__Iterator.<$>coins = coins;
			<Show>c__Iterator.<$>typ = typ;
			<Show>c__Iterator.<$>onFinished = onFinished;
			<Show>c__Iterator.<>f__this = this;
			return <Show>c__Iterator;
		}

		private void ShowCoin()
		{
			TweenSettingsExtensions.Append(TweenSettingsExtensions.Append(DOTween.Sequence(), TweenSettingsExtensions.OnStart<Tweener>(TweenSettingsExtensions.SetDelay<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTexture_Coin.get_transform(), 160f, 0.3f, false), 0.625f), delegate
			{
				this.mTexture_Coin.get_gameObject().SetActive(true);
				this.mTexture_Coin.alpha = 1f;
			})), TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.SetLoops<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTexture_Coin.get_transform(), 120f, 1.5f, false), 2147483647, 1), 4));
		}

		[DebuggerHidden]
		private IEnumerator StartOpenLid()
		{
			UIUseItemReceiveFurnitureBox.<StartOpenLid>c__Iterator98 <StartOpenLid>c__Iterator = new UIUseItemReceiveFurnitureBox.<StartOpenLid>c__Iterator98();
			<StartOpenLid>c__Iterator.<>f__this = this;
			return <StartOpenLid>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnAnimation(Action onFinished)
		{
			UIUseItemReceiveFurnitureBox.<OnAnimation>c__Iterator99 <OnAnimation>c__Iterator = new UIUseItemReceiveFurnitureBox.<OnAnimation>c__Iterator99();
			<OnAnimation>c__Iterator.onFinished = onFinished;
			<OnAnimation>c__Iterator.<$>onFinished = onFinished;
			<OnAnimation>c__Iterator.<>f__this = this;
			return <OnAnimation>c__Iterator;
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Box, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Lid, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Coin, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Flare, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Template_Sparkle, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_RewardText);
			if (this.mTexture_Sparkles != null)
			{
				for (int i = 0; i < this.mTexture_Sparkles.Length; i++)
				{
					UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Sparkles[i], false);
				}
			}
			this.mTexture_Sparkles = null;
		}
	}
}
