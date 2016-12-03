using Common.Enum;
using KCV.Scene.Others;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Interior
{
	public class UserInterfaceInteriorChangeManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			FurnitureKindsSelect,
			FurnitureSelect,
			FurnitureDetail,
			FurniturePreView
		}

		[SerializeField]
		private UIInteriorFurnitureKindSelector mUIInteriorChangeFurnitureSelector;

		[SerializeField]
		private UIInteriorFurnitureDetail mPrefab_UIInteriorFurnitureDetail;

		[SerializeField]
		private UIInteriorFurnitureChangeScrollListNew mPrefab_UIInteriorFurnitureChangeScrollListNew;

		private UIInteriorFurnitureDetail mUIInteriorFurnitureDetail;

		private UIInteriorFurnitureChangeScrollListNew mUIInteriorFurnitureChangeScrollList;

		[SerializeField]
		private UIInteriorFurniturePreviewWaiter mUIInteriorFurniturePreviewWaiter;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		[SerializeField]
		private Transform mTransform_MoveButton;

		private AudioClip mAudioClip_CommonEnter1;

		private AudioClip mAudioClip_CommonCancel1;

		private AudioClip mAudioClip_CommonCursolMove;

		private AudioClip mAudioClip_CommonEnter2;

		private KeyControl mKeyController;

		private UserInterfaceInteriorManager.StateManager<UserInterfaceInteriorChangeManager.State> mStateManager;

		private int mDeckid;

		private InteriorManager mInteriorManager;

		private Context mContext;

		private Action mOnRequestMoveStore;

		private Camera mCamera_SwipeEventCatch;

		public string StateToString()
		{
			return this.mStateManager.ToString();
		}

		private void Back()
		{
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}

		public void Initialize(InteriorManager interiorManager)
		{
			this.mDeckid = interiorManager.Deck.Id;
			this.mInteriorManager = interiorManager;
			this.mUserInterfacePortInteriorManager.InitializeFurnituresForConfirmation(this.mInteriorManager.Deck, interiorManager.GetRoomInfo());
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void StartState()
		{
			this.mContext = new Context();
			this.mStateManager = new UserInterfaceInteriorManager.StateManager<UserInterfaceInteriorChangeManager.State>(UserInterfaceInteriorChangeManager.State.NONE);
			this.mStateManager.OnPop = new Action<UserInterfaceInteriorChangeManager.State>(this.OnPopState);
			this.mStateManager.OnPush = new Action<UserInterfaceInteriorChangeManager.State>(this.OnPushState);
			this.mStateManager.OnResume = new Action<UserInterfaceInteriorChangeManager.State>(this.OnResumeState);
			this.mStateManager.OnSwitch = new Action<UserInterfaceInteriorChangeManager.State>(this.OnSwitchState);
			this.mStateManager.PushState(UserInterfaceInteriorChangeManager.State.FurnitureKindsSelect);
		}

		public void Release()
		{
			this.mUIInteriorFurnitureChangeScrollList.SetKeyController(null);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_CommonEnter1, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_CommonCancel1, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_CommonCursolMove, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_CommonEnter2, false);
			this.mKeyController = null;
			this.mContext = null;
			this.mStateManager = null;
			this.mUIInteriorChangeFurnitureSelector = null;
			this.mUIInteriorFurnitureDetail = null;
			this.mUIInteriorFurnitureChangeScrollList = null;
			this.mUIInteriorFurniturePreviewWaiter = null;
			this.mUserInterfacePortInteriorManager = null;
		}

		public void Clean()
		{
			this.mKeyController = null;
			this.mContext = null;
			this.mStateManager = null;
			if (this.mUIInteriorChangeFurnitureSelector != null)
			{
				this.mUIInteriorChangeFurnitureSelector.SetKeyController(null);
			}
			if (this.mUIInteriorFurnitureChangeScrollList != null)
			{
				this.mUIInteriorFurnitureChangeScrollList.SetKeyController(null);
			}
			if (this.mUIInteriorFurnitureDetail != null)
			{
				this.mUIInteriorFurnitureDetail.SetKeyController(null);
			}
			if (this.mUIInteriorFurniturePreviewWaiter != null)
			{
				this.mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
			}
		}

		private void Start()
		{
			this.mAudioClip_CommonEnter1 = SoundFile.LoadSE(SEFIleInfos.CommonEnter1);
			this.mAudioClip_CommonCancel1 = SoundFile.LoadSE(SEFIleInfos.CommonCancel1);
			this.mAudioClip_CommonCursolMove = SoundFile.LoadSE(SEFIleInfos.CommonCursolMove);
			this.mAudioClip_CommonEnter2 = SoundFile.LoadSE(SEFIleInfos.CommonEnter2);
			this.mUIInteriorChangeFurnitureSelector.SetOnSelectFurnitureKindListener(new Action<FurnitureKinds>(this.OnSelectFurnitureKindListener));
			this.mUIInteriorChangeFurnitureSelector.SetOnSelectCancelListener(new Action(this.OnSelectCancelListener));
			this.mUIInteriorFurniturePreviewWaiter.SetOnBackListener(new Action(this.OnFinishedPreview));
		}

		private void OnBackListener()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurnitureSelect;
			if (flag)
			{
				SoundUtils.PlaySE(this.mAudioClip_CommonCancel1);
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void OnChanedItemListener(UIInteriorFurnitureChangeScrollListChildNew child)
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurnitureSelect;
			if (flag)
			{
				SoundUtils.PlaySE(this.mAudioClip_CommonCursolMove);
				this.mUIInteriorFurnitureDetail.Initialize(this.mDeckid, child.GetModel().GetFurnitureModel());
			}
		}

		private void OnSelectedListener(UIInteriorFurnitureChangeScrollListChildNew child)
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurnitureSelect;
			if (flag)
			{
				this.mContext.SetSelectedFurniture(child.GetModel().GetFurnitureModel());
				this.mUIInteriorFurnitureChangeScrollList.LockControl();
				this.mStateManager.PushState(UserInterfaceInteriorChangeManager.State.FurnitureDetail);
				SoundUtils.PlaySE(this.mAudioClip_CommonEnter1);
			}
		}

		private void Update()
		{
			if (this.mKeyController != null && this.mKeyController.IsRSRightDown())
			{
				bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurnitureKindsSelect;
				if (flag)
				{
					this.RequestMoveStore();
				}
			}
		}

		private void OnSelectPreviewListener()
		{
			this.mUIInteriorFurnitureDetail.SetKeyController(null);
			this.mStateManager.PushState(UserInterfaceInteriorChangeManager.State.FurniturePreView);
			SoundUtils.PlaySE(this.mAudioClip_CommonEnter2);
		}

		private void OnSelectChangeListener()
		{
			this.ChangeFurniture(this.mContext.CurrentCategory, this.mContext.SelectedFurniture);
			FurnitureModel furniture = this.mInteriorManager.GetFurniture(this.mContext.CurrentCategory, this.mContext.SelectedFurniture.MstId);
			this.mContext.SetSelectedFurniture(furniture);
			this.mUIInteriorFurnitureDetail.Initialize(this.mDeckid, this.mContext.SelectedFurniture);
			SoundUtils.PlaySE(this.mAudioClip_CommonEnter1);
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void OnBackDetailListener()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIInteriorFurnitureDetail.QuitState();
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
			SoundUtils.PlaySE(this.mAudioClip_CommonCancel1);
		}

		private void OnSelectFurnitureKindListener(FurnitureKinds furnitureKind)
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurnitureKindsSelect;
			if (flag)
			{
				this.mContext.SetSelectFurnitureKind(furnitureKind);
				this.mUIInteriorChangeFurnitureSelector.SetKeyController(null);
				this.mStateManager.PushState(UserInterfaceInteriorChangeManager.State.FurnitureSelect);
			}
		}

		private void OnSelectCancelListener()
		{
			this.Back();
		}

		private void OnPopState(UserInterfaceInteriorChangeManager.State state)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			switch (state)
			{
			case UserInterfaceInteriorChangeManager.State.FurnitureSelect:
				this.mUIInteriorFurnitureDetail.Hide();
				this.mUIInteriorFurnitureChangeScrollList.LockControl();
				this.mUIInteriorFurnitureChangeScrollList.Hide();
				break;
			case UserInterfaceInteriorChangeManager.State.FurniturePreView:
				this.mTransform_MoveButton.SetActive(true);
				break;
			}
		}

		private void OnPushStateFurnitureSelect()
		{
			base.StartCoroutine(this.OnPushStateFurnitureSelectCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushStateFurnitureSelectCoroutine()
		{
			UserInterfaceInteriorChangeManager.<OnPushStateFurnitureSelectCoroutine>c__Iterator89 <OnPushStateFurnitureSelectCoroutine>c__Iterator = new UserInterfaceInteriorChangeManager.<OnPushStateFurnitureSelectCoroutine>c__Iterator89();
			<OnPushStateFurnitureSelectCoroutine>c__Iterator.<>f__this = this;
			return <OnPushStateFurnitureSelectCoroutine>c__Iterator;
		}

		private void OnPushState(UserInterfaceInteriorChangeManager.State state)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			switch (state)
			{
			case UserInterfaceInteriorChangeManager.State.FurnitureKindsSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mUIInteriorChangeFurnitureSelector.Initialize();
				this.mUIInteriorChangeFurnitureSelector.SetKeyController(this.mKeyController);
				this.mUIInteriorChangeFurnitureSelector.StartState();
				break;
			case UserInterfaceInteriorChangeManager.State.FurnitureSelect:
				this.OnPushStateFurnitureSelect();
				break;
			case UserInterfaceInteriorChangeManager.State.FurnitureDetail:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mUIInteriorFurnitureDetail.SetKeyController(this.mKeyController);
				this.mUIInteriorFurnitureDetail.StartState();
				break;
			case UserInterfaceInteriorChangeManager.State.FurniturePreView:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mTransform_MoveButton.SetActive(false);
				this.mUIInteriorChangeFurnitureSelector.get_transform().set_localScale(Vector3.get_zero());
				this.mUIInteriorFurnitureChangeScrollList.get_transform().set_localScale(Vector3.get_zero());
				this.mUIInteriorFurnitureDetail.get_transform().set_localScale(Vector3.get_zero());
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.mUIInteriorFurniturePreviewWaiter.SetKeyController(this.mKeyController);
				this.mUIInteriorFurniturePreviewWaiter.StartWait();
				this.PreviewFurniture(this.mContext.CurrentCategory, this.mContext.SelectedFurniture);
				break;
			}
		}

		[DebuggerHidden]
		private IEnumerator ShowFurnitureSelecter()
		{
			UserInterfaceInteriorChangeManager.<ShowFurnitureSelecter>c__Iterator8A <ShowFurnitureSelecter>c__Iterator8A = new UserInterfaceInteriorChangeManager.<ShowFurnitureSelecter>c__Iterator8A();
			<ShowFurnitureSelecter>c__Iterator8A.<>f__this = this;
			return <ShowFurnitureSelecter>c__Iterator8A;
		}

		private void OnFinishedPreview()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurniturePreView;
			if (flag)
			{
				SoundUtils.PlaySE(this.mAudioClip_CommonCursolMove);
				this.mUIInteriorChangeFurnitureSelector.get_transform().set_localScale(Vector3.get_one());
				this.mUIInteriorFurnitureChangeScrollList.get_transform().set_localScale(Vector3.get_one());
				this.mUIInteriorFurnitureDetail.get_transform().set_localScale(Vector3.get_one());
				this.mUIInteriorFurniturePreviewWaiter.SetKeyController(null);
				this.PreviewFurniture(this.mContext.CurrentCategory, this.mInteriorManager.GetRoomInfo().get_Item(this.mContext.CurrentCategory));
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void OnSwitchState(UserInterfaceInteriorChangeManager.State state)
		{
		}

		private void OnResumeState(UserInterfaceInteriorChangeManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case UserInterfaceInteriorChangeManager.State.FurnitureKindsSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mUIInteriorChangeFurnitureSelector.SetKeyController(this.mKeyController);
				this.mUIInteriorChangeFurnitureSelector.ResumeState();
				break;
			case UserInterfaceInteriorChangeManager.State.FurnitureSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mUIInteriorFurnitureChangeScrollList.ResumeControl();
				break;
			case UserInterfaceInteriorChangeManager.State.FurnitureDetail:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.mUIInteriorFurnitureDetail.SetKeyController(this.mKeyController);
				this.mUIInteriorFurnitureDetail.ResumeState();
				break;
			}
		}

		private void ChangeFurniture(FurnitureKinds furnitureKind, FurnitureModel furnitureModel)
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurnitureDetail;
			if (flag)
			{
				Dictionary<FurnitureKinds, int> dictionary = new Dictionary<FurnitureKinds, int>();
				bool flag2 = this.mInteriorManager.ChangeRoom(furnitureKind, furnitureModel.MstId);
				if (flag2)
				{
					this.mUserInterfacePortInteriorManager.UpdateFurniture(this.mInteriorManager.Deck, furnitureKind, furnitureModel);
					this.mUIInteriorFurnitureChangeScrollList.RefreshViews();
					this.mUIInteriorFurnitureDetail.QuitState();
				}
			}
		}

		private void PreviewFurniture(FurnitureKinds furnitureKind, FurnitureModel furnitureModel)
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurniturePreView;
			if (flag)
			{
				this.mUserInterfacePortInteriorManager.UpdateFurniture(this.mInteriorManager.Deck, furnitureKind, furnitureModel);
			}
		}

		public void SetOnRequestMoveToFurnitureStoreListener(Action onRequestMoveStore)
		{
			this.mOnRequestMoveStore = onRequestMoveStore;
		}

		private void RequestMoveStore()
		{
			if (this.mOnRequestMoveStore != null)
			{
				this.mOnRequestMoveStore.Invoke();
			}
		}

		[Obsolete("Inspector上に設定して使用します")]
		public void OnTouchMoveToFurnitureStore()
		{
			if (this.mStateManager == null)
			{
				return;
			}
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorChangeManager.State.FurnitureKindsSelect;
			if (flag)
			{
				this.RequestMoveStore();
			}
		}

		private void OnDestroy()
		{
			this.mUIInteriorChangeFurnitureSelector = null;
			this.mUIInteriorFurnitureDetail = null;
			this.mUIInteriorFurnitureChangeScrollList = null;
			this.mUIInteriorFurniturePreviewWaiter = null;
			this.mUserInterfacePortInteriorManager = null;
			this.mTransform_MoveButton = null;
			this.mAudioClip_CommonEnter1 = null;
			this.mAudioClip_CommonCancel1 = null;
			this.mAudioClip_CommonCursolMove = null;
			this.mAudioClip_CommonEnter2 = null;
			this.mKeyController = null;
			this.mStateManager = null;
			this.mInteriorManager = null;
			this.mContext = null;
		}

		internal void SetSwipeEventCamera(Camera swipeEventCatch)
		{
			this.mCamera_SwipeEventCatch = swipeEventCatch;
		}
	}
}
