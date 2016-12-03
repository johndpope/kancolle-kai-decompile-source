using KCV.Scene.Port;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemStoreManager : MonoBehaviour
	{
		private enum State
		{
			NONE,
			ItemStoreList,
			ItemStoreBuyConfirm
		}

		[SerializeField]
		private Camera mCamera_SwipeEvent;

		[SerializeField]
		private UIButton mButton_ChangeItemList;

		[SerializeField]
		private UIItemStoreBuyConfirm mUIItemStoreBuyConfirm;

		[SerializeField]
		private UIItemStoreChildScrollView mUIItemStoreChildren;

		private ItemStoreManager mItemStoreManager;

		private KeyControl mKeyController;

		private Stack<UIItemStoreManager.State> mStateStack = new Stack<UIItemStoreManager.State>();

		private Action mOnSwitchItemListener;

		private Action mOnItemStoreBackListener;

		private UIItemStoreManager.State CurrentState
		{
			get
			{
				if (0 < this.mStateStack.get_Count())
				{
					return this.mStateStack.Peek();
				}
				return UIItemStoreManager.State.NONE;
			}
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UIItemStoreManager.<Start>c__Iterator93 <Start>c__Iterator = new UIItemStoreManager.<Start>c__Iterator93();
			<Start>c__Iterator.<>f__this = this;
			return <Start>c__Iterator;
		}

		private void OnSelectItemStoreChild(UIItemStoreChild view)
		{
			bool flag = this.mItemStoreManager.IsValidBuy(view.GetModel().MstId, 1);
			if (flag)
			{
				this.mUIItemStoreChildren.SetKeyController(null);
				this.mUIItemStoreChildren.LockControl();
				this.mUIItemStoreBuyConfirm.Initialize(view.GetModel(), this.mItemStoreManager);
				this.mUIItemStoreBuyConfirm.Show(null);
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.mUIItemStoreBuyConfirm.SetKeyController(this.mKeyController);
				this.ChangeState(UIItemStoreManager.State.ItemStoreBuyConfirm, false);
			}
			else if (this.mItemStoreManager.UserInfo.SPoint < view.GetModel().Price)
			{
				CommonPopupDialog.Instance.StartPopup("戦略ポイントが不足しています");
			}
			else
			{
				CommonPopupDialog.Instance.StartPopup("保有上限を超えています");
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
				{
					this.OnBack();
				}
				else if (this.mKeyController.IsRSLeftDown())
				{
					this.SwitchToItemList();
				}
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_ChangeItemList);
			this.mCamera_SwipeEvent = null;
			this.mUIItemStoreBuyConfirm = null;
			this.mUIItemStoreChildren = null;
			this.mItemStoreManager = null;
			this.mKeyController = null;
		}

		private void OnBuyStart(ItemStoreModel itemStoreModel, int count)
		{
			bool flag = this.CurrentState == UIItemStoreManager.State.ItemStoreBuyConfirm;
			if (flag)
			{
				bool flag2 = this.mItemStoreManager.IsValidBuy(itemStoreModel.MstId, count);
				if (flag2)
				{
					int fuel = this.mItemStoreManager.Material.Fuel;
					int ammo = this.mItemStoreManager.Material.Ammo;
					int baux = this.mItemStoreManager.Material.Baux;
					int steel = this.mItemStoreManager.Material.Steel;
					bool flag3 = this.mItemStoreManager.BuyItem(itemStoreModel.MstId, count);
					if (flag3)
					{
						int fuel2 = this.mItemStoreManager.Material.Fuel;
						int ammo2 = this.mItemStoreManager.Material.Ammo;
						int baux2 = this.mItemStoreManager.Material.Baux;
						int steel2 = this.mItemStoreManager.Material.Steel;
						bool flag4 = fuel != fuel2 || ammo != ammo2 || baux != baux2 || steel != steel2;
						if (flag4)
						{
							TrophyUtil.Unlock_Material();
						}
						TrophyUtil.Unlock_AlbumSlotNum();
					}
					this.mUIItemStoreChildren.Refresh(this.mItemStoreManager.Items.ToArray());
					if (SingletonMonoBehaviour<UIPortFrame>.exist())
					{
						SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this.mItemStoreManager);
					}
				}
				else if (this.mItemStoreManager.UserInfo.SPoint >= itemStoreModel.Price)
				{
					CommonPopupDialog.Instance.StartPopup("保有上限に達しています");
				}
				this.mUIItemStoreBuyConfirm.Close(null);
				this.mUIItemStoreBuyConfirm.SetKeyController(null);
				this.PopState();
			}
		}

		private void OnBuyCancel()
		{
			bool flag = this.CurrentState == UIItemStoreManager.State.ItemStoreBuyConfirm;
			if (flag)
			{
				this.mUIItemStoreBuyConfirm.Close(null);
				this.mUIItemStoreBuyConfirm.SetKeyController(null);
				this.PopState();
			}
		}

		public void SwitchToItemList()
		{
			bool flag = this.CurrentState == UIItemStoreManager.State.ItemStoreList;
			if (flag)
			{
				this.OnSwitchToItemList();
			}
		}

		private void OnSwitchToItemList()
		{
			if (this.mOnSwitchItemListener != null)
			{
				this.mOnSwitchItemListener.Invoke();
			}
		}

		public void Initialize(ItemStoreManager manager)
		{
			this.mItemStoreManager = manager;
			ItemStoreModel[] itemStoreModels = this.mItemStoreManager.Items.ToArray();
			this.mUIItemStoreChildren.Initialize(manager, itemStoreModels, this.mCamera_SwipeEvent);
		}

		public void StartState()
		{
			this.mStateStack.Clear();
			this.ChangeState(UIItemStoreManager.State.ItemStoreList, false);
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
			this.mUIItemStoreChildren.SetKeyController(this.mKeyController);
		}

		public void LockControl()
		{
			this.mUIItemStoreChildren.LockControl();
		}

		public void SetOnSwitchItemListListener(Action onSwitchItemListListener)
		{
			this.mOnSwitchItemListener = onSwitchItemListListener;
		}

		public void SetOnBackListener(Action onItemStoreBackListener)
		{
			this.mOnItemStoreBackListener = onItemStoreBackListener;
		}

		private void OnBack()
		{
			if (this.mOnItemStoreBackListener != null)
			{
				this.mOnItemStoreBackListener.Invoke();
			}
		}

		private void ChangeState(UIItemStoreManager.State state, bool popStack)
		{
			if (popStack && 0 < this.mStateStack.get_Count())
			{
				this.PopState();
			}
			this.mStateStack.Push(state);
			this.OnPushState(this.mStateStack.Peek());
		}

		private void PopState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				UIItemStoreManager.State state = this.mStateStack.Pop();
				this.OnPopState(state);
				if (0 < this.mStateStack.get_Count())
				{
					this.OnResumeState(this.mStateStack.Peek());
				}
			}
		}

		private void OnPushState(UIItemStoreManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (state != UIItemStoreManager.State.ItemStoreList)
			{
				if (state == UIItemStoreManager.State.ItemStoreBuyConfirm)
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				}
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				ItemStoreModel[] itemStoreModels = this.mItemStoreManager.Items.ToArray();
				this.mUIItemStoreChildren.Initialize(this.mItemStoreManager, itemStoreModels, this.mCamera_SwipeEvent);
				this.mUIItemStoreChildren.SetKeyController(this.mKeyController);
				this.mUIItemStoreChildren.StartState();
			}
		}

		private void OnPopState(UIItemStoreManager.State state)
		{
			if (state == UIItemStoreManager.State.ItemStoreList)
			{
				this.OnPopStateItemStoreList();
			}
		}

		private void OnPopStateItemStoreList()
		{
			this.mUIItemStoreChildren.SetKeyController(this.mKeyController);
		}

		private void OnResumeState(UIItemStoreManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (state != UIItemStoreManager.State.ItemStoreList)
			{
				if (state == UIItemStoreManager.State.ItemStoreBuyConfirm)
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				}
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnResumeStateItemStoreList();
			}
		}

		private void OnResumeStateItemStoreList()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIItemStoreChildren.SetKeyController(this.mKeyController);
			this.mUIItemStoreChildren.ResumeState();
		}

		public string StateToString()
		{
			this.mStateStack.ToArray();
			string text = string.Empty;
			using (Stack<UIItemStoreManager.State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIItemStoreManager.State current = enumerator.get_Current();
					text = current + " > " + text;
				}
			}
			return text;
		}

		public void Release()
		{
			this.mKeyController = null;
			this.mItemStoreManager = null;
			this.mUIItemStoreBuyConfirm.Release();
			this.mStateStack.Clear();
		}
	}
}
