using DG.Tweening;
using KCV.Scene.Port;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Duty.Reward
{
	public class UIDutyOpenDeckRewardGet : MonoBehaviour
	{
		[SerializeField]
		private UILabel mLabel_Message;

		[SerializeField]
		private UITexture mTexture_Deck;

		[SerializeField]
		private UITexture mTexture_Yousei;

		[SerializeField]
		private Texture mTexture2d_Yousei_Off;

		[SerializeField]
		private Texture mTexture2d_Yousei_On;

		public void Initialize(Reward_Deck reward)
		{
			this.Initialize(reward.DeckId);
		}

		private void Update()
		{
			if (Input.GetKeyDown(116))
			{
				this.Initialize(3);
				this.PlayAnimation();
			}
		}

		private void Start()
		{
			this.mTexture_Yousei.alpha = 1E-06f;
			this.mTexture_Yousei.get_transform().localPositionY(-90f);
			this.mTexture_Deck.alpha = 1E-06f;
			this.mTexture_Deck.get_transform().localPositionY(-10f);
		}

		public void PlayAnimation()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			Sequence sequence = DOTween.Sequence();
			TweenSettingsExtensions.SetId<Sequence>(sequence, this);
			this.mTexture_Yousei.alpha = 1E-06f;
			this.mTexture_Yousei.get_transform().localPositionY(-90f);
			this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_Off;
			this.mTexture_Deck.alpha = 1E-06f;
			this.mTexture_Deck.get_transform().localPositionY(-10f);
			Sequence sequence2 = DOTween.Sequence();
			float num = 1f;
			Tween tween = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTexture_Yousei.get_transform(), -50f, num, false), 27);
			Tween tween2 = DOVirtual.Float(0f, 1f, num, delegate(float alpha)
			{
				this.mTexture_Yousei.alpha = alpha;
			});
			TweenSettingsExtensions.Append(sequence2, tween);
			TweenSettingsExtensions.Join(sequence2, tween2);
			TweenSettingsExtensions.Append(sequence, sequence2);
			Sequence sequence3 = DOTween.Sequence();
			TweenCallback tweenCallback = delegate
			{
				this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_On;
			};
			Sequence sequence4 = DOTween.Sequence();
			Tween tween3 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTexture_Deck.get_transform(), 20f, num, false), 27);
			Tween tween4 = DOVirtual.Float(0f, 1f, num, delegate(float alpha)
			{
				this.mTexture_Deck.alpha = alpha;
			});
			TweenSettingsExtensions.OnPlay<Sequence>(sequence4, tweenCallback);
			TweenSettingsExtensions.Append(sequence4, tween3);
			TweenSettingsExtensions.Join(sequence4, tween4);
			TweenSettingsExtensions.Append(sequence, sequence4);
			Sequence sequence5 = DOTween.Sequence();
			Sequence sequence6 = DOTween.Sequence();
			TweenCallback tweenCallback2 = delegate
			{
				this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_On;
			};
			TweenCallback tweenCallback3 = delegate
			{
				this.mTexture_Yousei.mainTexture = this.mTexture2d_Yousei_Off;
			};
			TweenSettingsExtensions.AppendInterval(sequence6, 0.3f);
			TweenSettingsExtensions.AppendCallback(sequence6, tweenCallback3);
			TweenSettingsExtensions.AppendInterval(sequence6, 0.3f);
			TweenSettingsExtensions.AppendCallback(sequence6, tweenCallback2);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence6, 2147483647);
			TweenSettingsExtensions.Append(sequence5, sequence6);
			Sequence sequence7 = DOTween.Sequence();
			Tween tween5 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTexture_Deck.get_transform(), 24f, 1.5f, false), 6);
			Tween tween6 = TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveY(this.mTexture_Deck.get_transform(), 20f, 1.5f, false), 6);
			TweenSettingsExtensions.Append(sequence7, tween5);
			TweenSettingsExtensions.Append(sequence7, tween6);
			TweenSettingsExtensions.SetLoops<Sequence>(sequence7, 2147483647);
			TweenSettingsExtensions.Join(sequence5, sequence7);
			TweenSettingsExtensions.Append(sequence, sequence5);
		}

		private void Initialize(int deckId)
		{
			this.mLabel_Message.text = string.Format("第{0}艦隊\nが開放されました！", deckId);
			this.mTexture_Deck.mainTexture = (Resources.Load("Textures/Common/DeckFlag/icon_deck" + deckId) as Texture2D);
			this.PlayAnimation();
		}

		private void OnDestroy()
		{
			if (DOTween.IsTweening(this))
			{
				DOTween.Kill(this, false);
			}
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mLabel_Message);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Deck, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Yousei, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Yousei_Off, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture2d_Yousei_On, false);
		}
	}
}
