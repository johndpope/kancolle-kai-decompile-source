using KCV.Utils;
using KCV.View.ScrollView;
using local.managers;
using local.models;
using System;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemStoreChildScrollView : UIScrollList<ItemStoreModel, UIItemStoreChild>
	{
		[SerializeField]
		private Transform mTransform_ArrowUp;

		[SerializeField]
		private Transform mTransform_ArrowDown;

		private ItemStoreManager mItemStoreManager;

		private Action<UIItemStoreChild> mOnSelectListener;

		private KeyControl mKeyController;

		protected override void OnAwake()
		{
			base.OnAwake();
		}

		public void Initialize(ItemStoreManager itemStoreManager, ItemStoreModel[] itemStoreModels, Camera touchEventCatchCamera)
		{
			this.mTransform_ArrowDown.SetActive(false);
			this.mTransform_ArrowUp.SetActive(false);
			base.ChangeImmediateContentPosition(UIScrollList<ItemStoreModel, UIItemStoreChild>.ContentDirection.Hell);
			this.mItemStoreManager = itemStoreManager;
			base.Initialize(itemStoreModels);
			base.SetSwipeEventCamera(touchEventCatchCamera);
		}

		protected override void OnUpdate()
		{
			if (this.mKeyController != null && base.mState == UIScrollList<ItemStoreModel, UIItemStoreChild>.ListState.Waiting)
			{
				if (this.mKeyController.keyState.get_Item(8).down)
				{
					base.PrevFocus();
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					base.NextFocus();
				}
				else if (this.mKeyController.keyState.get_Item(14).down)
				{
					base.PrevPageOrHeadFocus();
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					base.NextPageOrTailFocus();
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					base.Select();
				}
				else if (this.mKeyController.keyState.get_Item(0).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
				}
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchPrev()
		{
			if (base.mState == UIScrollList<ItemStoreModel, UIItemStoreChild>.ListState.Waiting)
			{
				base.PrevFocus();
			}
		}

		[Obsolete("Inspector上で設定して使用します")]
		public void OnTouchNext()
		{
			if (base.mState == UIScrollList<ItemStoreModel, UIItemStoreChild>.ListState.Waiting)
			{
				base.NextFocus();
			}
		}

		public void StartState()
		{
			base.StartControl();
			base.HeadFocus();
		}

		public void ResumeState()
		{
			base.StartControl();
		}

		protected override void OnSelect(UIItemStoreChild view)
		{
			if (this.mOnSelectListener != null)
			{
				this.mOnSelectListener.Invoke(view);
			}
		}

		public void SetOnSelectListener(Action<UIItemStoreChild> onSelectListener)
		{
			this.mOnSelectListener = onSelectListener;
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void LockControl()
		{
			base.LockControl();
		}

		public void Refresh(ItemStoreModel[] itemStoreModels)
		{
			this.mModels = itemStoreModels;
			base.RefreshViews();
		}

		protected override bool OnSelectable(UIItemStoreChild view)
		{
			bool flag = view.GetModel().Count == 0;
			if (flag)
			{
				CommonPopupDialog.Instance.StartPopup("売り切れです");
				SoundUtils.PlaySE(SEFIleInfos.CommonCancel2);
			}
			return !flag;
		}

		protected override void OnChangedFocusView(UIItemStoreChild focusToView)
		{
			CommonPopupDialog.Instance.StartPopup(focusToView.GetRealIndex() + 1 + "/" + this.mModels.Length, 0, CommonPopupDialogMessage.PlayType.Short);
			if (this.mCurrentFocusView.GetRealIndex() == 0)
			{
				this.mTransform_ArrowUp.SetActive(false);
			}
			else
			{
				this.mTransform_ArrowUp.SetActive(true);
			}
			if (this.mCurrentFocusView.GetRealIndex() == this.mModels.Length - 1)
			{
				this.mTransform_ArrowDown.SetActive(false);
			}
			else
			{
				this.mTransform_ArrowDown.SetActive(true);
			}
			SoundUtils.PlaySE(SEFIleInfos.SE_014);
		}

		protected override void OnCallDestroy()
		{
			base.OnCallDestroy();
			this.mItemStoreManager = null;
			this.mKeyController = null;
			this.mOnSelectListener = null;
			this.mTransform_ArrowDown = null;
			this.mTransform_ArrowUp = null;
		}
	}
}
