using KCV.Scene.Others;
using KCV.Utils;
using local.managers;
using local.models;
using local.utils;
using System;
using UnityEngine;

namespace KCV.InteriorStore
{
	public class UIInteriorStoreManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			CategoryTabSelect,
			ListFurnitureSelect,
			PreviewFurniture,
			FurnitureDetailDialog
		}

		public enum Mode
		{
			TabSelect,
			ListSelect,
			StoreDialog,
			Preview
		}

		private Context mContext;

		[SerializeField]
		private Transform mUIInteriorStoreManagerContents;

		[SerializeField]
		private InteriorStoreTabManager tabManager;

		[SerializeField]
		private UIFurnitureStoreTabList mUIFurnitureStoreTabList;

		[SerializeField]
		private UIFurniturePurchaseDialog storeDialog;

		[SerializeField]
		private InteriorStoreFrame storeFrame;

		[SerializeField]
		private UIInteriorFurniturePreviewWaiter mUIInteriorFurniturePreviewWaiter;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		private KeyControl mKeyController;

		private StateManager<UIInteriorStoreManager.State> mStateManager;

		private InteriorManager mInteriorManager;

		private FurnitureStoreManager mFurnitureStoreManager;

		private Action mOnRequestMoveToInteriorListener;

		private void OnPushState(UIInteriorStoreManager.State state)
		{
			switch (state)
			{
			case UIInteriorStoreManager.State.CategoryTabSelect:
				this.OnPushStateCategoryTabSelect();
				break;
			case UIInteriorStoreManager.State.ListFurnitureSelect:
				this.OnPushStateListFurnitureSelect();
				break;
			case UIInteriorStoreManager.State.PreviewFurniture:
				this.OnPushStatePreviewFurniture();
				break;
			case UIInteriorStoreManager.State.FurnitureDetailDialog:
				this.OnPushStateDetailDialog();
				break;
			}
		}

		private void OnPushStatePreviewFurniture()
		{
			this.mUIInteriorStoreManagerContents.SetActive(false);
			this.mUserInterfacePortInteriorManager.SetActive(true);
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			FurnitureModel selectedFurniture = this.mContext.SelectedFurniture;
			this.mUserInterfacePortInteriorManager.UpdateFurniture(this.mInteriorManager.Deck, selectedFurniture.Type, selectedFurniture);
			this.mUIInteriorFurniturePreviewWaiter.SetKeyController(this.mKeyController);
			this.mUIInteriorFurniturePreviewWaiter.StartWait();
		}

		private void OnPushStateDetailDialog()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			FurnitureModel selectedFurniture = this.mContext.SelectedFurniture;
			bool isValidBuy = this.mFurnitureStoreManager.IsValidExchange(selectedFurniture);
			this.storeDialog.Initialize(selectedFurniture, isValidBuy);
			this.storeDialog.SetKeyController(this.mKeyController);
			this.storeDialog.Show();
		}

		private void OnPushStateListFurnitureSelect()
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIFurnitureStoreTabList.SetKeyController(this.mKeyController);
			this.mUIFurnitureStoreTabList.ResumeControl();
		}

		private void OnPushStateCategoryTabSelect()
		{
			bool flag = this.mStateManager.CurrentState == UIInteriorStoreManager.State.CategoryTabSelect;
			if (flag)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.tabManager.StartState();
			}
		}

		private void OnResumeState(UIInteriorStoreManager.State state)
		{
			switch (state)
			{
			case UIInteriorStoreManager.State.CategoryTabSelect:
				this.OnResumeStateCategoryTabSelect();
				break;
			case UIInteriorStoreManager.State.ListFurnitureSelect:
				this.OnResumeStateListFurnitureSelect();
				break;
			case UIInteriorStoreManager.State.PreviewFurniture:
				this.OnResumeStatePreviewFurniture();
				break;
			case UIInteriorStoreManager.State.FurnitureDetailDialog:
				this.OnResumeFurnitureDetailDialog();
				break;
			}
		}

		private void OnResumeStateCategoryTabSelect()
		{
			this.tabManager.ResumeState();
		}

		private void OnResumeStatePreviewFurniture()
		{
			this.mUserInterfacePortInteriorManager.UpdateFurniture(this.mInteriorManager.Deck, this.mContext.SelectedCategory, this.mInteriorManager.GetRoomInfo().get_Item(this.mContext.SelectedCategory));
		}

		private void OnResumeFurnitureDetailDialog()
		{
			this.mUIInteriorStoreManagerContents.SetActive(true);
			this.mUserInterfacePortInteriorManager.SetActive(false);
			this.storeDialog.SetKeyController(this.mKeyController);
			this.storeDialog.ResumeFocus();
		}

		private void OnResumeStateListFurnitureSelect()
		{
			this.mUIFurnitureStoreTabList.ResumeControl();
		}

		private void OnPopState(UIInteriorStoreManager.State state)
		{
			switch (state)
			{
			case UIInteriorStoreManager.State.CategoryTabSelect:
				this.tabManager.PopState();
				break;
			case UIInteriorStoreManager.State.ListFurnitureSelect:
				this.mUIFurnitureStoreTabList.LockControl();
				break;
			case UIInteriorStoreManager.State.PreviewFurniture:
				this.OnResumeStatePreviewFurniture();
				break;
			}
		}

		private void Update()
		{
			if (this.mKeyController != null && this.mStateManager != null && this.mStateManager.CurrentState == UIInteriorStoreManager.State.CategoryTabSelect)
			{
				if (this.mKeyController.IsRightDown())
				{
					this.tabManager.NextTab();
				}
				else if (this.mKeyController.IsLeftDown())
				{
					this.tabManager.PrevTab();
				}
				else if (this.mKeyController.IsMaruDown())
				{
					this.OnDesideTabListener();
				}
				else if (this.mKeyController.IsBatuDown())
				{
					this.RequestMoveToPort();
				}
				else if (this.mKeyController.IsRSLeftDown())
				{
					this.RequestMoveToInterior();
				}
			}
		}

		public void Initialize(InteriorManager interiorManager, FurnitureStoreManager furnitureStoreManager, UserInterfacePortInteriorManager uiPortInteriorManager)
		{
			this.mInteriorManager = interiorManager;
			this.mUserInterfacePortInteriorManager = uiPortInteriorManager;
			this.mFurnitureStoreManager = furnitureStoreManager;
			this.mUIFurnitureStoreTabList.Initialize(this.mFurnitureStoreManager);
			this.tabManager.InitTab();
			this.tabManager.Init(new Action(this.OnChangedTabListener), new Action(this.OnDesideTabListener));
			this.storeFrame.updateUserInfo(this.mFurnitureStoreManager);
			this.mUserInterfacePortInteriorManager.InitializeFurnituresForConfirmation(interiorManager.Deck, interiorManager.GetRoomInfo());
		}

		private void OnSelectedFurnitureListener(UIFurnitureStoreTabListChild selectedView)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIFurnitureStoreTabList.LockControl();
			FurnitureModel model = selectedView.GetModel();
			this.mContext.SetSelectedFurniture(model);
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.mStateManager.PushState(UIInteriorStoreManager.State.FurnitureDetailDialog);
		}

		private void OnBackListListener()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIFurnitureStoreTabList.LockControl();
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void StartState()
		{
			this.mContext = new Context();
			this.mStateManager = new StateManager<UIInteriorStoreManager.State>(UIInteriorStoreManager.State.NONE);
			this.mStateManager.OnPush = new Action<UIInteriorStoreManager.State>(this.OnPushState);
			this.mStateManager.OnPop = new Action<UIInteriorStoreManager.State>(this.OnPopState);
			this.mStateManager.OnResume = new Action<UIInteriorStoreManager.State>(this.OnResumeState);
			this.storeDialog.SetOnSelectNegativeListener(new Action(this.OnSelectNegative));
			this.storeDialog.SetOnSelectPositiveListener(new Action(this.OnSelectPositive));
			this.storeDialog.SetOnSelectPreviewListener(new Action(this.OnSelectPreview));
			this.mUIFurnitureStoreTabList.SetOnBackListener(new Action(this.OnBackListListener));
			this.mUIFurnitureStoreTabList.SetOnSelectedFurnitureListener(new Action<UIFurnitureStoreTabListChild>(this.OnSelectedFurnitureListener));
			this.mUIInteriorFurniturePreviewWaiter.SetOnBackListener(new Action(this.OnBackFromPreview));
			this.mStateManager.PushState(UIInteriorStoreManager.State.CategoryTabSelect);
		}

		public void SetOnRequestMoveToInteriorListener(Action onRequestMoveToInteriorListener)
		{
			this.mOnRequestMoveToInteriorListener = onRequestMoveToInteriorListener;
		}

		private void RequestMoveToInterior()
		{
			if (this.mOnRequestMoveToInteriorListener != null)
			{
				this.mOnRequestMoveToInteriorListener.Invoke();
			}
		}

		private void RequestMoveToPort()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void OnChangedTabListener()
		{
			if (this.mStateManager.CurrentState == UIInteriorStoreManager.State.ListFurnitureSelect)
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.tabManager.changeNowCategory();
				this.mContext.SetSelectedCategory(this.tabManager.GetCurrentCategory());
				this.mUIFurnitureStoreTabList.ChangeCategory(this.mContext.SelectedCategory);
				this.mUIFurnitureStoreTabList.StopFocusBlink();
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
			else if (this.mStateManager.CurrentState == UIInteriorStoreManager.State.CategoryTabSelect)
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.tabManager.changeNowCategory();
				this.mContext.SetSelectedCategory(this.tabManager.GetCurrentCategory());
				this.mUIFurnitureStoreTabList.ChangeCategory(this.mContext.SelectedCategory);
				SoundUtils.PlaySE(SEFIleInfos.CommonCursolMove);
			}
		}

		public void SetSwipeEventCamera(Camera camera)
		{
			this.mUIFurnitureStoreTabList.SetSwipeEventCamera(camera);
		}

		public void OnDesideTabListener()
		{
			if (this.mStateManager.CurrentState == UIInteriorStoreManager.State.ListFurnitureSelect)
			{
				return;
			}
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.tabManager.hideUnselectTabs();
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
			this.mStateManager.PushState(UIInteriorStoreManager.State.ListFurnitureSelect);
		}

		private void OnRequestChangeListMode()
		{
			this.OnDesideTabListener();
		}

		[Obsolete("Inspector上で設定して使用します。")]
		public void OnTouchSwitchToFurniture()
		{
			this.RequestMoveToInterior();
		}

		private void OnSelectPositive()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			FurnitureModel selectedFurniture = this.mContext.SelectedFurniture;
			bool flag = this.mFurnitureStoreManager.IsValidExchange(selectedFurniture);
			if (flag)
			{
				bool flag2 = this.mFurnitureStoreManager.Exchange(selectedFurniture);
				if (flag2)
				{
					TrophyUtil.Unlock_At_BuyFurniture();
					SoundUtils.PlaySE(SEFIleInfos.SE_004);
					this.storeFrame.updateUserInfo(this.mFurnitureStoreManager);
					this.storeDialog.Hide();
					this.storeDialog.SetKeyController(null);
					this.mUIFurnitureStoreTabList.Refresh();
					this.mStateManager.PopState();
					this.mStateManager.ResumeState();
				}
			}
		}

		private void OnSelectNegative()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			this.mContext.SetSelectedFurniture(null);
			this.storeDialog.SetKeyController(null);
			this.storeDialog.Hide();
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void OnSelectPreview()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter2);
			FurnitureModel selectedFurniture = this.mContext.SelectedFurniture;
			this.mStateManager.PushState(UIInteriorStoreManager.State.PreviewFurniture);
			this.storeDialog.SetKeyController(null);
		}

		private void OnBackFromPreview()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void OnDestroy()
		{
			this.mFurnitureStoreManager = null;
		}
	}
}
