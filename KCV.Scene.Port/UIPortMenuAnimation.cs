using DG.Tweening;
using System;
using UnityEngine;

namespace KCV.Scene.Port
{
	public class UIPortMenuAnimation : MonoBehaviour
	{
		[SerializeField]
		private Animation mAnim_MennuCollect;

		private Action mOnFinishedCollectAnimationListener;

		private UIPortMenuButton mTargetMenuButton;

		private bool mIsSubMenu;

		public void Initialize(UIPortMenuButton targetMenuButton)
		{
			if (this.mTargetMenuButton != null)
			{
				NGUITools.AdjustDepth(this.mTargetMenuButton.get_gameObject(), -10);
				this.mTargetMenuButton = null;
			}
			this.mTargetMenuButton = targetMenuButton;
		}

		public void PlayCollectAnimation()
		{
			this.mAnim_MennuCollect.Play("Anim_MenuCollect");
		}

		public void PlayCollectSubAnimation()
		{
			this.mAnim_MennuCollect.Play("Anim_MenuCollect_Sub");
		}

		public void OnDepthAdjust()
		{
			if (this.mTargetMenuButton != null)
			{
				NGUITools.AdjustDepth(this.mTargetMenuButton.get_gameObject(), 10);
			}
		}

		public void OnFinishedCollectAnimation()
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			Tween tween = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTargetMenuButton.get_transform(), new Vector3(1.8f, 1.8f), 0.2f), this);
			Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(ShortcutExtensions.DOScale(this.mTargetMenuButton.get_transform(), new Vector3(1.7f, 1.7f), 0.1f), this);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.Append(sequence, tween2);
			if (this.mOnFinishedCollectAnimationListener != null)
			{
				this.mOnFinishedCollectAnimationListener.Invoke();
			}
		}

		public void SetOnFinishedCollectAnimationListener(Action onFinishedCollectAnimationListener)
		{
			this.mOnFinishedCollectAnimationListener = onFinishedCollectAnimationListener;
		}

		private void OnDestroy()
		{
			this.mAnim_MennuCollect = null;
			this.mOnFinishedCollectAnimationListener = null;
			this.mTargetMenuButton = null;
		}
	}
}
