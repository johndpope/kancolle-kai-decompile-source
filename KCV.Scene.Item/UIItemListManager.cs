using Common.Enum;
using KCV.Scene.Duty;
using KCV.Scene.Port;
using KCV.UseItem;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Item
{
	public class UIItemListManager : MonoBehaviour
	{
		private enum State
		{
			ItemSelect,
			UseCheck,
			GetReward,
			ExchangeSelect,
			UseProd,
			NONE
		}

		private const int ITEM_LIST_MODELS_SPLIT = 7;

		[SerializeField]
		private Transform mTransform_DialogArea;

		[SerializeField]
		private UIItemList mItemList;

		[SerializeField]
		private UIItemListChildDetail mItemListChildDetai;

		[SerializeField]
		private UIButton mButton_ChangeItemStore;

		[SerializeField]
		private UIItemExchangeMedalConfirm mItemExchangeMedalConfirm;

		[SerializeField]
		private UIItemUseLimitOverConfirm mItemUseLimitOverConfirm;

		[SerializeField]
		private UIItemUseConfirm mItemUseConfirm;

		[SerializeField]
		private UIGetRewardDialog mPrefab_UIGetRewardDialog;

		[SerializeField]
		private UIUseItemReceiveFurnitureBox mPrefab_UIUseItemReceiveFurnitureBox;

		private Stack<UIItemListManager.State> mStateStack = new Stack<UIItemListManager.State>();

		private KeyControl mKeyController;

		private ItemlistManager mItemListManager;

		private Action mOnBack;

		private Action mOnSwitchItemStore;

		private UIItemListManager.State CurrentState
		{
			get
			{
				if (0 < this.mStateStack.get_Count())
				{
					return this.mStateStack.Peek();
				}
				return UIItemListManager.State.NONE;
			}
		}

		private void Start()
		{
			this.mItemList.SetOnFocusChangeListener(new Action<ItemlistModel>(this.OnItemListFocusChangeListener));
			this.mItemList.SetOnSelectListener(new Action<ItemlistModel>(this.OnItemListInItemSelectedListener));
			this.mItemListChildDetai.SetOnUseCallBack(new Action<ItemlistModel>(this.OnSelectUseItemCallBack));
			this.mItemListChildDetai.SetOnCancelCallBack(new Action(this.OnCancelUseItem));
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.IsRDown() && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable)
				{
					this.OnBack();
				}
				else if (this.mKeyController.IsRSRightDown())
				{
					this.SwitchItemStore();
				}
			}
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mButton_ChangeItemStore);
			this.mTransform_DialogArea = null;
			this.mItemList = null;
			this.mItemListChildDetai = null;
			this.mItemExchangeMedalConfirm = null;
			this.mItemUseLimitOverConfirm = null;
			this.mItemUseConfirm = null;
			this.mPrefab_UIGetRewardDialog = null;
			this.mPrefab_UIUseItemReceiveFurnitureBox = null;
		}

		public void OnTouchSwitchItemStore()
		{
			this.SwitchItemStore();
		}

		private void SwitchItemStore()
		{
			bool flag = this.CurrentState == UIItemListManager.State.ItemSelect;
			if (flag)
			{
				this.OnSwitchItemStore();
			}
		}

		public void Initialize(ItemlistManager manager)
		{
			this.mItemListManager = manager;
			ItemlistModel[] models = this.mItemListManager.HaveItems.ToArray();
			this.mItemList.Initialize(models);
		}

		public void StartState()
		{
			this.mStateStack.Clear();
			this.mItemList.FirstFocus();
			this.mItemList.SetKeyController(this.mKeyController);
			this.PushState(UIItemListManager.State.ItemSelect);
		}

		public void Clean()
		{
			this.mItemListManager = null;
			this.mKeyController = null;
			this.mItemList.Clean();
			this.mItemListChildDetai.Clean();
			this.mStateStack.Clear();
		}

		private void OnSelectUseItemCallBack(ItemlistModel model)
		{
			bool activeState = this.CurrentState == UIItemListManager.State.ItemSelect || this.CurrentState == UIItemListManager.State.UseCheck;
			if (activeState)
			{
				if (this.CurrentState == UIItemListManager.State.ItemSelect)
				{
					this.mItemList.SetKeyController(null);
					this.PushState(UIItemListManager.State.UseCheck);
				}
				this.mItemListChildDetai.SetKeyController(null);
				bool flag = this.IsExchangeItemlistModel(model);
				if (flag)
				{
					int mstId = model.MstId;
					if (mstId == 57)
					{
						this.mItemExchangeMedalConfirm.Initialize(model, this.mItemListManager);
						this.mItemExchangeMedalConfirm.SetKeyController(this.mKeyController);
						this.mItemExchangeMedalConfirm.SetOnExchangeItemSelectedCallBack(delegate(ItemExchangeKinds selectedItemExchangeKind)
						{
							this.mItemExchangeMedalConfirm.SetKeyController(null);
							ItemlistManager.Result result = this.mItemListManager.UseItem(model.MstId, false, selectedItemExchangeKind);
							if (result.IsLimitOver())
							{
								this.mItemExchangeMedalConfirm.Close(null);
								this.mItemUseLimitOverConfirm.SetOnNegativeCallBack(delegate
								{
									bool flag2 = this.CurrentState == UIItemListManager.State.ExchangeSelect;
									if (flag2)
									{
										this.PopState();
										this.mItemUseLimitOverConfirm.Close(null);
										this.mItemUseLimitOverConfirm.SetKeyController(null);
										this.ResumeState();
									}
								});
								this.mItemUseLimitOverConfirm.SetOnPositiveCallBack(delegate
								{
									bool flag2 = this.CurrentState == UIItemListManager.State.ExchangeSelect;
									if (flag2)
									{
										this.PopState();
										this.mItemUseLimitOverConfirm.Close(null);
										this.mItemUseLimitOverConfirm.SetKeyController(null);
										ItemlistManager.Result result2 = this.mItemListManager.UseItem(model.MstId, true, selectedItemExchangeKind);
										this.StartCoroutine(this.OnGetRewards(model, result2.Rewards, delegate
										{
											this.mItemList.Refresh(this.mItemListManager.HaveItems.ToArray());
											this.ResumeState();
										}));
									}
								});
								this.mItemUseLimitOverConfirm.Initialize();
								this.mItemUseLimitOverConfirm.SetKeyController(this.mKeyController);
								this.mItemUseLimitOverConfirm.Show(null);
								this.ReplaceState(UIItemListManager.State.ExchangeSelect);
							}
							else
							{
								this.mItemExchangeMedalConfirm.Close(null);
								this.ReplaceState(UIItemListManager.State.GetReward);
								this.StartCoroutine(this.OnGetRewards(model, result.Rewards, delegate
								{
									this.mItemList.Refresh(this.mItemListManager.HaveItems.ToArray());
									this.PopState();
									this.ResumeState();
								}));
							}
						});
						this.mItemExchangeMedalConfirm.SetOnCancelCallBack(delegate
						{
							this.mItemExchangeMedalConfirm.Close(null);
							this.mItemExchangeMedalConfirm.SetKeyController(null);
							this.PopState();
							this.ResumeState();
						});
						this.mItemExchangeMedalConfirm.Show(null);
						this.ReplaceState(UIItemListManager.State.ExchangeSelect);
					}
				}
				else
				{
					this.mItemUseConfirm.SetOnNegativeCallBack(delegate
					{
						bool flag2 = this.CurrentState == UIItemListManager.State.ExchangeSelect;
						if (activeState)
						{
							this.PopState();
							this.mItemUseConfirm.SetKeyController(null);
							this.mItemUseConfirm.Close(null);
							this.ResumeState();
						}
					});
					this.mItemUseConfirm.SetOnPositiveCallBack(delegate
					{
						bool flag2 = this.CurrentState == UIItemListManager.State.ExchangeSelect;
						if (activeState)
						{
							this.mItemUseConfirm.SetKeyController(null);
							this.mItemUseConfirm.Close(null);
							ItemlistManager.Result result = this.mItemListManager.UseItem(model.MstId, false, ItemExchangeKinds.NONE);
							if (result == null)
							{
								if (model.MstId == 53)
								{
									CommonPopupDialog.Instance.StartPopup("これ以上拡張できません");
								}
								this.PopState();
								this.ResumeState();
							}
							else if (result.IsLimitOver())
							{
								this.ReplaceState(UIItemListManager.State.ExchangeSelect);
								this.mItemUseLimitOverConfirm.SetOnNegativeCallBack(delegate
								{
									this.PopState();
									this.mItemUseLimitOverConfirm.SetKeyController(null);
									this.mItemUseLimitOverConfirm.Close(null);
									this.ResumeState();
								});
								this.mItemUseLimitOverConfirm.SetOnPositiveCallBack(delegate
								{
									this.mItemUseLimitOverConfirm.SetKeyController(null);
									this.mItemUseLimitOverConfirm.Close(null);
									ItemlistManager.Result result2 = this.mItemListManager.UseItem(model.MstId, true, ItemExchangeKinds.NONE);
									this.StartCoroutine(this.OnGetRewards(model, result2.Rewards, delegate
									{
										this.mItemList.Refresh(this.mItemListManager.HaveItems.ToArray());
										this.PopState();
										this.ResumeState();
									}));
								});
								this.mItemUseLimitOverConfirm.Initialize();
								this.mItemUseLimitOverConfirm.SetKeyController(this.mKeyController);
								this.mItemUseLimitOverConfirm.Show(null);
							}
							else
							{
								this.ReplaceState(UIItemListManager.State.GetReward);
								IReward[] rewards = result.Rewards;
								this.StartCoroutine(this.OnGetRewards(model, rewards, delegate
								{
									this.mItemList.Refresh(this.mItemListManager.HaveItems.ToArray());
									this.PopState();
									this.ResumeState();
								}));
							}
						}
					});
					this.mItemListChildDetai.SetKeyController(null);
					this.ReplaceState(UIItemListManager.State.ExchangeSelect);
					this.mItemUseConfirm.Initialize();
					this.mItemUseConfirm.Show(null);
					this.mItemUseConfirm.SetKeyController(this.mKeyController);
				}
			}
		}

		private bool IsExchangeItemlistModel(ItemlistModel model)
		{
			int mstId = model.MstId;
			return mstId == 57;
		}

		[DebuggerHidden]
		private IEnumerator OnGetUseItemReward(ItemlistModel usedModel, IReward_Useitem reward)
		{
			UIItemListManager.<OnGetUseItemReward>c__Iterator90 <OnGetUseItemReward>c__Iterator = new UIItemListManager.<OnGetUseItemReward>c__Iterator90();
			<OnGetUseItemReward>c__Iterator.usedModel = usedModel;
			<OnGetUseItemReward>c__Iterator.reward = reward;
			<OnGetUseItemReward>c__Iterator.<$>usedModel = usedModel;
			<OnGetUseItemReward>c__Iterator.<$>reward = reward;
			<OnGetUseItemReward>c__Iterator.<>f__this = this;
			return <OnGetUseItemReward>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnGetMaterialsReward(ItemlistModel usedModel, IReward_Materials reward)
		{
			UIItemListManager.<OnGetMaterialsReward>c__Iterator91 <OnGetMaterialsReward>c__Iterator = new UIItemListManager.<OnGetMaterialsReward>c__Iterator91();
			<OnGetMaterialsReward>c__Iterator.reward = reward;
			<OnGetMaterialsReward>c__Iterator.<$>reward = reward;
			<OnGetMaterialsReward>c__Iterator.<>f__this = this;
			return <OnGetMaterialsReward>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator OnGetRewards(ItemlistModel usedModel, IReward[] rewards, Action onAllReceived)
		{
			UIItemListManager.<OnGetRewards>c__Iterator92 <OnGetRewards>c__Iterator = new UIItemListManager.<OnGetRewards>c__Iterator92();
			<OnGetRewards>c__Iterator.rewards = rewards;
			<OnGetRewards>c__Iterator.usedModel = usedModel;
			<OnGetRewards>c__Iterator.onAllReceived = onAllReceived;
			<OnGetRewards>c__Iterator.<$>rewards = rewards;
			<OnGetRewards>c__Iterator.<$>usedModel = usedModel;
			<OnGetRewards>c__Iterator.<$>onAllReceived = onAllReceived;
			<OnGetRewards>c__Iterator.<>f__this = this;
			return <OnGetRewards>c__Iterator;
		}

		private void OnCancelUseItem()
		{
			bool flag = this.CurrentState == UIItemListManager.State.UseCheck;
			if (flag)
			{
				this.PopState();
				this.ResumeState();
			}
		}

		private void OnItemListFocusChangeListener(ItemlistModel model)
		{
			this.mItemListChildDetai.UpdateInfo(model);
		}

		private void OnItemListInItemSelectedListener(ItemlistModel model)
		{
			this.mItemList.SetKeyController(null);
			this.PushState(UIItemListManager.State.UseCheck);
		}

		private void OnSelectListInChild(UIItemListChild itemListChild)
		{
			if (itemListChild != null)
			{
				bool flag = this.mItemListChildDetai.Usable();
				if (flag)
				{
					this.mItemList.SetKeyController(null);
					this.PushState(UIItemListManager.State.UseCheck);
				}
			}
		}

		public void SetKeyController(KeyControl keyControl)
		{
			this.mKeyController = keyControl;
			this.mItemList.SetKeyController(this.mKeyController);
		}

		public void SetOnBackListener(Action onBack)
		{
			this.mOnBack = onBack;
		}

		private void OnBack()
		{
			if (this.mOnBack != null)
			{
				this.mOnBack.Invoke();
			}
		}

		public void SetOnSwitchItemStoreListener(Action onSwitchItemStore)
		{
			this.mOnSwitchItemStore = onSwitchItemStore;
		}

		private void OnSwitchItemStore()
		{
			if (this.mOnSwitchItemStore != null)
			{
				this.mOnSwitchItemStore.Invoke();
			}
		}

		private void PushState(UIItemListManager.State state)
		{
			this.mStateStack.Push(state);
			this.OnPushState(this.mStateStack.Peek());
		}

		private void ReplaceState(UIItemListManager.State state)
		{
			if (0 < this.mStateStack.get_Count())
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
				UIItemListManager.State state = this.mStateStack.Pop();
				this.OnPopState(state);
			}
		}

		private void ResumeState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				this.OnResumeState(this.mStateStack.Peek());
			}
		}

		private void OnPushState(UIItemListManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case UIItemListManager.State.ItemSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			case UIItemListManager.State.UseCheck:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnPushStateUseCheck();
				break;
			case UIItemListManager.State.ExchangeSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			}
		}

		private void OnPopState(UIItemListManager.State state)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			if (state != UIItemListManager.State.ItemSelect)
			{
				if (state == UIItemListManager.State.UseCheck)
				{
					this.mItemListChildDetai.SetKeyController(null);
				}
			}
			else
			{
				this.OnPopStateItemSelect();
			}
		}

		private void OnResumeState(UIItemListManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case UIItemListManager.State.ItemSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnResumeStateItemSelect();
				break;
			case UIItemListManager.State.UseCheck:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			case UIItemListManager.State.ExchangeSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				break;
			}
		}

		private void OnPushStateUseCheck()
		{
			this.mItemListChildDetai.SetKeyController(this.mKeyController);
		}

		private void OnPopStateItemSelect()
		{
			this.mItemList.SetKeyController(null);
		}

		private void OnResumeStateItemSelect()
		{
			this.mItemList.SetKeyController(this.mKeyController);
			SingletonMonoBehaviour<UIPortFrame>.Instance.UpdateHeaderInfo(this.mItemListManager);
		}

		public string StateToString()
		{
			this.mStateStack.ToArray();
			string text = string.Empty;
			using (Stack<UIItemListManager.State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UIItemListManager.State current = enumerator.get_Current();
					text = current + " > " + text;
				}
			}
			return text;
		}
	}
}
