using DG.Tweening;
using KCV.Scene.Port;
using KCV.Utils;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIPracticeMenu : MonoBehaviour
	{
		public enum SelectType
		{
			Back,
			DeckPractice,
			BattlePractice
		}

		[SerializeField]
		private UIButton mButton_BattlePractice;

		[SerializeField]
		private UIButton mButton_DeckPractice;

		[SerializeField]
		private Vector3 mVector3_SelectPosition;

		private Vector3 mVector3_DefaultPositionBattlePractice;

		private Vector3 mVector3_DefaultPositionDeckPractice;

		private UIButton mButtonTarget;

		private KeyControl mKeyController;

		private DeckModel mDeckModel;

		private int mDefaultDepth = 10000;

		private Action<UIPracticeMenu.SelectType> mMenuSelectedCallBack;

		[Obsolete("UIButtonのSerializeFieldに設定します.")]
		public void OnTouchDeckPractice()
		{
			this.ChangeFocusButton(this.mButton_DeckPractice, true);
			this.OnClickFocusTarget();
		}

		[Obsolete("UIButtonのSerializeFieldに設定します.")]
		public void OnTocuhBattlePractice()
		{
			this.ChangeFocusButton(this.mButton_BattlePractice, true);
			this.OnClickFocusTarget();
		}

		[Obsolete("UIButtonのSerializeFieldに設定します.")]
		public void OnTouchBack()
		{
			this.OnBack();
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
			if (this.mKeyController != null)
			{
				this.mButton_BattlePractice.SetEnableCollider2D(true);
				this.mButton_DeckPractice.SetEnableCollider2D(true);
				if (this.mButtonTarget != null)
				{
					this.ChangeFocusButton(this.mButtonTarget, false);
				}
			}
			else
			{
				this.mButton_BattlePractice.SetEnableCollider2D(false);
				this.mButton_DeckPractice.SetEnableCollider2D(false);
			}
		}

		public void Initialize(DeckModel currentDeckModel)
		{
			this.mDeckModel = currentDeckModel;
			this.ChangeFocusButton(this.mButton_DeckPractice, false);
		}

		public void SetOnSelectedCallBack(Action<UIPracticeMenu.SelectType> menuSelectedCallBack)
		{
			this.mMenuSelectedCallBack = menuSelectedCallBack;
		}

		private void ChangeFocusButton(UIButton focusTarget, bool needSe)
		{
			if (this.mButtonTarget != null)
			{
				this.mButtonTarget.GetSprite().depth = this.mDefaultDepth;
				this.mButtonTarget.SetState(UIButtonColor.State.Normal, true);
			}
			this.mButtonTarget = focusTarget;
			if (this.mButtonTarget != null)
			{
				this.mButtonTarget.GetSprite().depth = this.mDefaultDepth + 1;
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.mButtonTarget.SetState(UIButtonColor.State.Hover, true);
			}
		}

		private void OnClickFocusTarget()
		{
			if (this.mButtonTarget != null)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				if (this.mButtonTarget.Equals(this.mButton_BattlePractice))
				{
					this.OnSelectedMenu(UIPracticeMenu.SelectType.BattlePractice);
				}
				else if (this.mButtonTarget.Equals(this.mButton_DeckPractice))
				{
					this.OnSelectedMenu(UIPracticeMenu.SelectType.DeckPractice);
				}
			}
		}

		private void OnBack()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this.OnSelectedMenu(UIPracticeMenu.SelectType.Back);
		}

		private void OnSelectedMenu(UIPracticeMenu.SelectType selectType)
		{
			if (this.mMenuSelectedCallBack != null && this.mKeyController != null)
			{
				this.mMenuSelectedCallBack.Invoke(selectType);
			}
		}

		private void Start()
		{
			this.mVector3_DefaultPositionBattlePractice = this.mButton_BattlePractice.get_transform().get_localPosition();
			this.mVector3_DefaultPositionDeckPractice = this.mButton_DeckPractice.get_transform().get_localPosition();
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnClickFocusTarget();
				}
				else if (this.mKeyController.keyState.get_Item(8).down || this.mKeyController.keyState.get_Item(14).down)
				{
					this.ChangeFocusButton(this.mButton_DeckPractice, true);
				}
				else if (this.mKeyController.keyState.get_Item(12).down || this.mKeyController.keyState.get_Item(10).down)
				{
					this.ChangeFocusButton(this.mButton_BattlePractice, true);
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnBack();
				}
			}
		}

		public void MoveToButtonCenterFocus(Action onFinishedAnimation)
		{
			TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mButton_BattlePractice.get_transform(), this.mVector3_SelectPosition, 0.3f, false), 21);
			TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMove(this.mButton_DeckPractice.get_transform(), this.mVector3_SelectPosition, 0.3f, false), delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation.Invoke();
				}
			}), 21);
		}

		public void MoveToButtonDefaultFocus(Action onFinishedAnimation)
		{
			TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(this.mButton_BattlePractice.get_transform(), this.mVector3_DefaultPositionBattlePractice, 0.4f, false), 21);
			TweenSettingsExtensions.SetEase<Tweener>(TweenSettingsExtensions.OnComplete<Tweener>(ShortcutExtensions.DOLocalMove(this.mButton_DeckPractice.get_transform(), this.mVector3_DefaultPositionDeckPractice, 0.4f, false), delegate
			{
				if (onFinishedAnimation != null)
				{
					onFinishedAnimation.Invoke();
				}
			}), 21);
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_BattlePractice);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_DeckPractice);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButtonTarget);
			this.mKeyController = null;
			this.mDeckModel = null;
			this.mMenuSelectedCallBack = null;
		}
	}
}
