using KCV.Utils;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	[RequireComponent(typeof(UIPanel))]
	public class UIItemStoreBuyConfirm : MonoBehaviour
	{
		private UIPanel mPanelThis;

		private ItemStoreModel mModel;

		private ItemStoreManager mItemStoreCheckUtils;

		private KeyControl mKeyController;

		[SerializeField]
		private UIButton mButton_Negative;

		[SerializeField]
		private UIButton mButton_Positive;

		[SerializeField]
		private UITexture mTexture_Icon;

		[SerializeField]
		private UILabel mLabel_Price;

		[SerializeField]
		private UILabel mLabel_Name;

		[SerializeField]
		private UIItemAkashi mAkashi;

		[SerializeField]
		private DialogAnimation mDialogAnimation;

		private UIButton mFocusButton;

		private Action<ItemStoreModel, int> mOnBuyCallBack;

		private Action mOnBuyCancelCallBack;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mPanelThis.alpha = 0f;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					this.ChangeFocus(this.mButton_Positive, true);
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					this.ChangeFocus(this.mButton_Negative, true);
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					if (this.mButton_Negative.Equals(this.mFocusButton))
					{
						this.BuyCancel();
					}
					else if (this.mButton_Positive.Equals(this.mFocusButton))
					{
						this.Buy();
					}
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					this.BuyCancel();
				}
			}
		}

		public void Initialize(ItemStoreModel itemStoreModel, ItemStoreManager checkUtils)
		{
			if (itemStoreModel != null)
			{
				this.mModel = itemStoreModel;
				this.mItemStoreCheckUtils = checkUtils;
				this.mLabel_Name.text = this.mModel.Name;
				this.mLabel_Price.text = this.mModel.Price.ToString();
				this.mTexture_Icon.mainTexture = UserInterfaceItemManager.RequestItemStoreIcon(this.mModel.MstId);
				this.ChangeFocus(this.mButton_Positive, false);
				this.mAkashi.SetKeyController(null);
			}
		}

		public void Show(Action onFinished)
		{
			this.mPanelThis.alpha = 1f;
			if (!this.mDialogAnimation.IsOpen)
			{
				this.mAkashi.Show();
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

		public void Close(Action onFinished)
		{
			if (this.mDialogAnimation.IsOpen)
			{
				this.mAkashi.Hide();
				this.mDialogAnimation.CloseAction = delegate
				{
					if (onFinished != null)
					{
						onFinished.Invoke();
					}
					this.mPanelThis.alpha = 0f;
				};
				this.mDialogAnimation.StartAnim(DialogAnimation.AnimType.FEAD, false);
			}
		}

		public void SetOnBuyStartCallBack(Action<ItemStoreModel, int> onBuyCallBack)
		{
			this.mOnBuyCallBack = onBuyCallBack;
		}

		private void OnBuy(ItemStoreModel itemStoreModel, int count)
		{
			if (this.mOnBuyCallBack != null)
			{
				this.mOnBuyCallBack.Invoke(itemStoreModel, count);
			}
		}

		public void SetOnBuyCancelCallBack(Action onBuyCancel)
		{
			this.mOnBuyCancelCallBack = onBuyCancel;
		}

		private void OnBuyCancel()
		{
			if (this.mOnBuyCancelCallBack != null)
			{
				this.mOnBuyCancelCallBack.Invoke();
			}
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		private void ChangeFocus(UIButton targetButton, bool needSe)
		{
			if (this.mFocusButton != null)
			{
				this.mFocusButton.SetState(UIButtonColor.State.Normal, true);
				if (this.mFocusButton == targetButton)
				{
					needSe = false;
				}
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

		public void OnClickPositive()
		{
			this.ChangeFocus(this.mButton_Positive, false);
			this.Buy();
		}

		private void BuyCancel()
		{
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this.OnBuyCancel();
		}

		private void Buy()
		{
			int count = 1;
			bool flag = this.mItemStoreCheckUtils.IsValidBuy(this.mModel.MstId, count);
			if (flag)
			{
				SoundUtils.PlaySE(SEFIleInfos.SE_004);
				this.OnBuy(this.mModel, count);
			}
		}

		public void OnClickNegative()
		{
			this.BuyCancel();
		}

		public void OnTouchOther()
		{
			this.BuyCancel();
		}

		public void Release()
		{
			this.mModel = null;
			this.mItemStoreCheckUtils = null;
			this.mFocusButton = null;
		}
	}
}
