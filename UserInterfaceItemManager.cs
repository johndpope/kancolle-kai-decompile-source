using KCV;
using KCV.Scene.Item;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UserInterfaceItemManager : MonoBehaviour
{
	public enum StartAt
	{
		ItemList,
		ItemStore
	}

	public enum State
	{
		NONE,
		Akashi,
		ItemList,
		ItemStore,
		SwitchItemListToItemStore,
		SwitchItemStoreToItemList
	}

	public static readonly object SHARE_DATA_START_AT_KEY = "share_data_start_at_key";

	public static readonly object SHARE_DATA_START_AT_VALUE_ITEMLIST = "share_data_start_at_value_itemlist";

	public static readonly object SHARE_DATA_START_AT_VALUE_ITEMSTORE = "share_data_start_at_value_itemstore";

	[SerializeField]
	private Texture[] mTextures_Preload;

	[SerializeField]
	private UIItemListManager mUIItemListManager;

	[SerializeField]
	private UIItemStoreManager mUIItemStoreManager;

	[SerializeField]
	private UIItemAkashi mUIItemAkashi;

	[SerializeField]
	private Transform mTransform_SwitchViewRoot;

	private ItemlistManager __ItemlistManager__;

	private ItemStoreManager __ItemStoreManager__;

	private KeyControl mKeyController;

	private AudioClip mAudioClip_SE002;

	private AudioClip mAudioClip_CommonCancel1;

	private AudioClip mAudioClip_SceneBGM;

	private UserInterfaceItemManager.StartAt mStartAt;

	private Stack<UserInterfaceItemManager.State> mStateStack = new Stack<UserInterfaceItemManager.State>();

	private ItemlistManager itemListManager
	{
		get
		{
			if (this.__ItemlistManager__ == null)
			{
				if (this.__ItemStoreManager__ == null)
				{
					this.__ItemlistManager__ = new ItemlistManager();
					this.__ItemlistManager__.Init();
				}
				else
				{
					this.__ItemlistManager__ = this.__ItemStoreManager__.CreateListManager();
					this.__ItemlistManager__.Init();
				}
			}
			return this.__ItemlistManager__;
		}
	}

	private ItemStoreManager itemStoreManager
	{
		get
		{
			if (this.__ItemStoreManager__ == null)
			{
				if (this.__ItemlistManager__ == null)
				{
					this.__ItemStoreManager__ = new ItemStoreManager();
					this.__ItemStoreManager__.Init();
				}
				else
				{
					this.__ItemStoreManager__ = this.__ItemlistManager__.CreateStoreManager();
					this.__ItemStoreManager__.Init();
				}
			}
			return this.__ItemStoreManager__;
		}
	}

	public UserInterfaceItemManager.State CurrentState
	{
		get
		{
			if (0 < this.mStateStack.get_Count())
			{
				return this.mStateStack.Peek();
			}
			return UserInterfaceItemManager.State.NONE;
		}
	}

	private void OnDestroy()
	{
		UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Preload, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_SE002, false);
		UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_CommonCancel1, false);
		this.mAudioClip_SceneBGM = null;
		this.mUIItemListManager = null;
		this.mUIItemStoreManager = null;
		this.mUIItemAkashi = null;
		this.mTransform_SwitchViewRoot = null;
		this.__ItemlistManager__ = null;
		this.__ItemStoreManager__ = null;
		this.mKeyController = null;
	}

	public static Texture RequestItemStoreIcon(int masterId)
	{
		return Resources.Load<Texture>("Textures/Item/purchase_items/" + masterId.ToString());
	}

	[DebuggerHidden]
	private IEnumerator Start()
	{
		UserInterfaceItemManager.<Start>c__Iterator9A <Start>c__Iterator9A = new UserInterfaceItemManager.<Start>c__Iterator9A();
		<Start>c__Iterator9A.<>f__this = this;
		return <Start>c__Iterator9A;
	}

	private UserInterfaceItemManager.StartAt GetStartAt()
	{
		bool flag = RetentionData.GetData() != null;
		if (flag)
		{
			bool flag2 = RetentionData.GetData().Contains(UserInterfaceItemManager.SHARE_DATA_START_AT_KEY);
			if (flag2)
			{
				object obj = RetentionData.GetData().get_Item(UserInterfaceItemManager.SHARE_DATA_START_AT_KEY);
				if (obj == UserInterfaceItemManager.SHARE_DATA_START_AT_VALUE_ITEMLIST)
				{
					return UserInterfaceItemManager.StartAt.ItemList;
				}
				if (obj == UserInterfaceItemManager.SHARE_DATA_START_AT_VALUE_ITEMSTORE)
				{
					return UserInterfaceItemManager.StartAt.ItemStore;
				}
			}
		}
		return UserInterfaceItemManager.StartAt.ItemList;
	}

	private void OnAkashiHidenListener()
	{
		bool flag = this.CurrentState == UserInterfaceItemManager.State.Akashi;
		if (flag)
		{
			UserInterfaceItemManager.StartAt startAt = this.mStartAt;
			if (startAt != UserInterfaceItemManager.StartAt.ItemList)
			{
				if (startAt == UserInterfaceItemManager.StartAt.ItemStore)
				{
					this.ChangeState(UserInterfaceItemManager.State.ItemStore, true);
				}
			}
			else
			{
				this.ChangeState(UserInterfaceItemManager.State.ItemList, true);
			}
		}
	}

	private void Update()
	{
		if (this.mKeyController != null)
		{
			this.mKeyController.Update();
		}
	}

	private void SwitchView(UserInterfaceItemManager.State moveToState, Action onFinishedSwitch)
	{
		SoundUtils.PlaySE(this.mAudioClip_SE002);
		TweenPosition tweenPosition = UITweener.Begin<TweenPosition>(this.mTransform_SwitchViewRoot.get_gameObject(), 0.8f);
		if (moveToState != UserInterfaceItemManager.State.ItemList)
		{
			if (moveToState == UserInterfaceItemManager.State.ItemStore)
			{
				tweenPosition.from = this.mTransform_SwitchViewRoot.get_transform().get_localPosition();
				tweenPosition.to = new Vector3(-960f, tweenPosition.from.y, tweenPosition.from.z);
				tweenPosition.SetOnFinished(delegate
				{
					if (onFinishedSwitch != null)
					{
						onFinishedSwitch.Invoke();
					}
				});
			}
		}
		else
		{
			tweenPosition.from = this.mTransform_SwitchViewRoot.get_transform().get_localPosition();
			tweenPosition.to = new Vector3(0f, tweenPosition.from.y, tweenPosition.from.z);
			tweenPosition.SetOnFinished(delegate
			{
				if (onFinishedSwitch != null)
				{
					onFinishedSwitch.Invoke();
				}
			});
		}
	}

	private void ChangeState(UserInterfaceItemManager.State state, bool popStack)
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
			UserInterfaceItemManager.State state = this.mStateStack.Pop();
			this.OnPopState(state);
			if (0 < this.mStateStack.get_Count())
			{
				this.OnResumeState(this.mStateStack.Peek());
			}
		}
	}

	private void OnPushState(UserInterfaceItemManager.State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		if (state != UserInterfaceItemManager.State.ItemList)
		{
			if (state == UserInterfaceItemManager.State.ItemStore)
			{
				this.mUIItemStoreManager.SetKeyController(this.mKeyController);
				this.mUIItemStoreManager.StartState();
			}
		}
		else
		{
			this.mUIItemListManager.SetKeyController(this.mKeyController);
			this.mUIItemListManager.StartState();
		}
	}

	private void OnPopState(UserInterfaceItemManager.State state)
	{
		if (state != UserInterfaceItemManager.State.Akashi)
		{
			if (state == UserInterfaceItemManager.State.ItemList)
			{
				this.OnPopStateItemList();
			}
		}
		else
		{
			this.OnPopStateAkashi();
		}
	}

	private void OnResumeState(UserInterfaceItemManager.State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
	}

	private void OnPopStateAkashi()
	{
		this.mUIItemAkashi.SetOnHiddenCallBack(null);
		this.mUIItemAkashi.SetClickable(false);
	}

	private void OnItemListBack()
	{
		bool flag = this.CurrentState == UserInterfaceItemManager.State.ItemList;
		if (flag)
		{
			this.mKeyController.IsRun = false;
			SoundUtils.PlaySE(this.mAudioClip_CommonCancel1);
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
		}
	}

	private void OnSwitchToItemStore()
	{
		this.ChangeState(UserInterfaceItemManager.State.SwitchItemListToItemStore, true);
		base.StartCoroutine(this.OnSwitchToItemStoreCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator OnSwitchToItemStoreCoroutine()
	{
		UserInterfaceItemManager.<OnSwitchToItemStoreCoroutine>c__Iterator9B <OnSwitchToItemStoreCoroutine>c__Iterator9B = new UserInterfaceItemManager.<OnSwitchToItemStoreCoroutine>c__Iterator9B();
		<OnSwitchToItemStoreCoroutine>c__Iterator9B.<>f__this = this;
		return <OnSwitchToItemStoreCoroutine>c__Iterator9B;
	}

	private void OnSwitchToItemList()
	{
		this.ChangeState(UserInterfaceItemManager.State.SwitchItemStoreToItemList, true);
		base.StartCoroutine(this.OnSwitchToItemListCoroutine());
	}

	[DebuggerHidden]
	private IEnumerator OnSwitchToItemListCoroutine()
	{
		UserInterfaceItemManager.<OnSwitchToItemListCoroutine>c__Iterator9C <OnSwitchToItemListCoroutine>c__Iterator9C = new UserInterfaceItemManager.<OnSwitchToItemListCoroutine>c__Iterator9C();
		<OnSwitchToItemListCoroutine>c__Iterator9C.<>f__this = this;
		return <OnSwitchToItemListCoroutine>c__Iterator9C;
	}

	private void OnPopStateItemList()
	{
		this.mUIItemListManager.SetKeyController(null);
	}

	private void OnItemStoreBackListener()
	{
		bool flag = this.CurrentState == UserInterfaceItemManager.State.ItemStore;
		if (flag)
		{
			this.mKeyController.IsRun = false;
			SoundUtils.PlaySE(this.mAudioClip_CommonCancel1);
			SingletonMonoBehaviour<PortObjectManager>.Instance.BackToActiveScene();
		}
	}

	private void OnPopStateItemStore()
	{
		this.mUIItemStoreManager.Release();
	}

	private string StateToString()
	{
		this.mStateStack.ToArray();
		string text = string.Empty;
		using (Stack<UserInterfaceItemManager.State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UserInterfaceItemManager.State current = enumerator.get_Current();
				text = current + " > " + text;
			}
		}
		return text;
	}
}
