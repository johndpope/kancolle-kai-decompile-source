using KCV.Utils;
using local.models;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel)), RequireComponent(typeof(UIButtonManager))]
	public class UIPracticeBattleConfirm : MonoBehaviour
	{
		private UIPanel mPanelThis;

		private UIButtonManager mButtonManager;

		[SerializeField]
		private DialogAnimation mDialogAnimation;

		[SerializeField]
		private UITexture mTexture_FriendDeckFlag;

		[SerializeField]
		private UITexture mTexture_TargetDeckFlag;

		[SerializeField]
		private UIPracticeBattleConfirmShipSlot[] mFriendUIPracticeBattleConfirmShipSlot;

		[SerializeField]
		private UIPracticeBattleConfirmShipSlot[] mTargetUIPracticeBattleConfirmShipSlot;

		[SerializeField]
		private UIButton mButton_Cancel;

		[SerializeField]
		private UIButton mButton_Start;

		[SerializeField]
		private UILabel mLabel_FriendDeckName;

		[SerializeField]
		private UILabel mLabel_TargetDeckName;

		private KeyControl mKeyController;

		private DeckModel mFriendDeck;

		private DeckModel mTargetDeck;

		private bool mMatchValid;

		private UIButton[] mButtonsFocasable;

		private Action mOnStartListener;

		private Action mOnCancelListener;

		private UIButton mFocusButton;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0f;
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				int num = Array.IndexOf<UIButton>(this.mButtonsFocasable, this.mButtonManager.nowForcusButton);
				if (-1 < num)
				{
					this.ChangeFocusButton(this.mButtonManager.nowForcusButton, false);
				}
			};
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					int num = Array.IndexOf<UIButton>(this.mButtonsFocasable, this.mFocusButton);
					int num2 = num + 1;
					if (num2 < this.mButtonsFocasable.Length)
					{
						this.ChangeFocusButton(this.mButtonsFocasable[num2], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					int num3 = Array.IndexOf<UIButton>(this.mButtonsFocasable, this.mFocusButton);
					int num4 = num3 - 1;
					if (0 <= num4)
					{
						this.ChangeFocusButton(this.mButtonsFocasable[num4], true);
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					this.OnSelectFocus();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.OnCancel();
				}
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void SetOnStartListener(Action onStartListener)
		{
			this.mOnStartListener = onStartListener;
		}

		public void SetOnCancelListener(Action onCancelListener)
		{
			this.mOnCancelListener = onCancelListener;
		}

		public void OnTouchCancel()
		{
			this.ChangeFocusButton(this.mButton_Cancel, false);
			this.OnSelectFocus();
		}

		public void OnTouchStart()
		{
			this.ChangeFocusButton(this.mButton_Start, false);
			this.OnSelectFocus();
		}

		private void OnSelectFocus()
		{
			if (this.mFocusButton != null)
			{
				if (this.mFocusButton.Equals(this.mButton_Start))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
					this.OnStartPractice();
				}
				else if (this.mFocusButton.Equals(this.mButton_Cancel))
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
					this.OnCancel();
				}
			}
		}

		private void OnStartPractice()
		{
			if (this.mOnStartListener != null)
			{
				this.mOnStartListener.Invoke();
			}
		}

		private void OnCancel()
		{
			if (this.mOnCancelListener != null)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
				this.mOnCancelListener.Invoke();
			}
		}

		public void Initialize(DeckModel friendDeckModel, DeckModel targetDeckModel, bool matchValid)
		{
			this.mFriendDeck = friendDeckModel;
			this.mTargetDeck = targetDeckModel;
			this.mMatchValid = matchValid;
			this.mLabel_FriendDeckName.text = this.mFriendDeck.Name;
			this.mLabel_TargetDeckName.text = this.mTargetDeck.Name;
			this.mLabel_FriendDeckName.supportEncoding = false;
			this.mLabel_TargetDeckName.supportEncoding = false;
			this.InitializeFriendBanners(this.mFriendDeck);
			this.InitializeTargetBanners(this.mTargetDeck);
			Texture2D mainTexture = Resources.Load(string.Format("Textures/Common/DeckFlag/icon_deck{0}", friendDeckModel.Id)) as Texture2D;
			Texture2D mainTexture2 = Resources.Load(string.Format("Textures/Common/DeckFlag/icon_deck{0}", targetDeckModel.Id)) as Texture2D;
			this.mTexture_FriendDeckFlag.mainTexture = mainTexture;
			this.mTexture_TargetDeckFlag.mainTexture = mainTexture2;
			List<UIButton> list = new List<UIButton>();
			list.Add(this.mButton_Cancel);
			if (this.mMatchValid)
			{
				this.mButton_Start.isEnabled = true;
				list.Add(this.mButton_Start);
			}
			else
			{
				this.mButton_Start.isEnabled = false;
			}
			this.mButtonsFocasable = list.ToArray();
			this.ChangeFocusButton(this.mButtonsFocasable[1], false);
		}

		private void ChangeFocusButton(UIButton targetButton, bool needSe)
		{
			if (this.mFocusButton != null)
			{
				bool flag = this.mFocusButton.Equals(targetButton);
				if (flag)
				{
					this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
					return;
				}
				this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
			}
			this.mFocusButton = targetButton;
			if (this.mFocusButton != null)
			{
				if (needSe)
				{
					SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				}
				this.mFocusButton.SetState(UIButtonColor.State.Hover, true);
			}
		}

		private void InitializeFriendBanners(DeckModel deckModel)
		{
			this.InitializeBanners(deckModel, this.mFriendUIPracticeBattleConfirmShipSlot);
		}

		private void InitializeTargetBanners(DeckModel deckModel)
		{
			this.InitializeBanners(deckModel, this.mTargetUIPracticeBattleConfirmShipSlot);
		}

		private void InitializeBanners(DeckModel deckModel, UIPracticeBattleConfirmShipSlot[] banners)
		{
			for (int i = 0; i < banners.Length; i++)
			{
				bool flag = i < deckModel.GetShipCount();
				if (flag)
				{
					banners[i].Initialize(i + 1, deckModel.GetShip(i));
				}
				else
				{
					banners[i].InitializeDefault();
				}
			}
		}

		public void Show(Action onFinished)
		{
			this.ChangeFocusButton(this.mFocusButton, false);
			this.mPanelThis.alpha = 1f;
			if (!this.mDialogAnimation.IsOpen)
			{
				this.mDialogAnimation.OpenAction = delegate
				{
					if (onFinished != null)
					{
						onFinished.Invoke();
					}
				};
				this.mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, true);
			}
		}

		public void Hide(Action onFinished)
		{
			if (this.mDialogAnimation.IsOpen)
			{
				this.mDialogAnimation.CloseAction = delegate
				{
					this.mPanelThis.alpha = 0f;
					if (onFinished != null)
					{
						onFinished.Invoke();
					}
				};
				this.mDialogAnimation.StartAnim(DialogAnimation.AnimType.POPUP, false);
			}
		}

		private void OnDestroy()
		{
			this.mPanelThis = null;
			this.mButtonManager = null;
			this.mDialogAnimation = null;
			this.mTexture_FriendDeckFlag = null;
			this.mTexture_TargetDeckFlag = null;
			this.mFriendUIPracticeBattleConfirmShipSlot = null;
			this.mTargetUIPracticeBattleConfirmShipSlot = null;
			this.mButton_Cancel = null;
			this.mButton_Start = null;
			this.mLabel_FriendDeckName = null;
			this.mLabel_TargetDeckName = null;
			this.mKeyController = null;
			this.mFriendDeck = null;
			this.mTargetDeck = null;
			this.mButtonsFocasable = null;
		}
	}
}
