using DG.Tweening;
using KCV;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class UserInterfaceSlotItemAlbumManager : MonoBehaviour
{
	public enum State
	{
		None,
		SlotItemList,
		SlotItemDetail
	}

	private class Context
	{
		private IAlbumModel mAlbumModel;

		public IAlbumModel GetAlbumModel()
		{
			return this.mAlbumModel;
		}

		public void SetAlbumModel(IAlbumModel albumModel)
		{
			this.mAlbumModel = albumModel;
		}
	}

	private class StateManager<State>
	{
		private Stack<State> mStateStack;

		private State mEmptyState;

		public Action<State> OnPush
		{
			private get;
			set;
		}

		public Action<State> OnPop
		{
			private get;
			set;
		}

		public Action<State> OnResume
		{
			private get;
			set;
		}

		public Action<State> OnSwitch
		{
			private get;
			set;
		}

		public State CurrentState
		{
			get
			{
				if (0 < this.mStateStack.get_Count())
				{
					return this.mStateStack.Peek();
				}
				return this.mEmptyState;
			}
		}

		public StateManager(State emptyState)
		{
			this.mEmptyState = emptyState;
			this.mStateStack = new Stack<State>();
		}

		public void PushState(State state)
		{
			this.mStateStack.Push(state);
			this.Notify(this.OnPush, this.mStateStack.Peek());
			this.Notify(this.OnSwitch, this.mStateStack.Peek());
		}

		public void ReplaceState(State state)
		{
			if (0 < this.mStateStack.get_Count())
			{
				this.PopState();
			}
			this.mStateStack.Push(state);
			this.Notify(this.OnPush, this.mStateStack.Peek());
			this.Notify(this.OnSwitch, this.mStateStack.Peek());
		}

		public void PopState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				State state = this.mStateStack.Pop();
				this.Notify(this.OnPop, state);
			}
		}

		public void ResumeState()
		{
			if (0 < this.mStateStack.get_Count())
			{
				this.Notify(this.OnResume, this.mStateStack.Peek());
				this.Notify(this.OnSwitch, this.mStateStack.Peek());
			}
		}

		public override string ToString()
		{
			this.mStateStack.ToArray();
			string text = string.Empty;
			using (Stack<State>.Enumerator enumerator = this.mStateStack.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					State current = enumerator.get_Current();
					text = current + " > " + text;
				}
			}
			return text;
		}

		private void Notify(Action<State> target, State state)
		{
			if (target != null)
			{
				target.Invoke(state);
			}
		}
	}

	[SerializeField]
	private UISlotItemAlbumList mUISlotItemAlbumList;

	[SerializeField]
	private UISlotItemAlbumDetail mUISlotItemAlbumDetail;

	private BGMFileInfos DefaultBGM = BGMFileInfos.Port;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private UserInterfaceSlotItemAlbumManager.StateManager<UserInterfaceSlotItemAlbumManager.State> mStateManager;

	private UserInterfaceSlotItemAlbumManager.Context mContext;

	private Action mOnBackListener;

	private Action<UserInterfaceSlotItemAlbumManager.State> mOnChangeStateUserInterfaceSlotItemAlbumManager;

	public bool Initialized
	{
		get;
		private set;
	}

	public void Initialize(IAlbumModel[] albumModels)
	{
		this.mAlbumModels = albumModels;
		this.mUISlotItemAlbumList.SetOnSelectedListItemListener(new Action<IAlbumModel>(this.OnSelectedListItemListener));
		this.mUISlotItemAlbumList.SetOnBackListener(new Action(this.OnBackListListener));
		this.mUISlotItemAlbumDetail.SetActive(false);
		this.mUISlotItemAlbumDetail.SetOnBackListener(new Action<Tween>(this.OnBackAlbumDetailListener));
		this.mStateManager = new UserInterfaceSlotItemAlbumManager.StateManager<UserInterfaceSlotItemAlbumManager.State>(UserInterfaceSlotItemAlbumManager.State.None);
		this.mStateManager.OnPush = new Action<UserInterfaceSlotItemAlbumManager.State>(this.OnPushState);
		this.mStateManager.OnPop = new Action<UserInterfaceSlotItemAlbumManager.State>(this.OnPopState);
		this.mStateManager.OnResume = new Action<UserInterfaceSlotItemAlbumManager.State>(this.OnResumeState);
		this.mStateManager.OnSwitch = new Action<UserInterfaceSlotItemAlbumManager.State>(this.OnChangeState);
		this.Initialized = true;
	}

	private void OnBackAlbumDetailListener(Tween closeTween)
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceSlotItemAlbumManager.State.SlotItemDetail;
		if (flag)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUISlotItemAlbumDetail.SetKeyController(null);
			TweenSettingsExtensions.OnComplete<Tween>(closeTween, delegate
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
				this.mUISlotItemAlbumDetail.SetActive(false);
			});
		}
	}

	private void OnBackListListener()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceSlotItemAlbumManager.State.SlotItemList;
		if (flag)
		{
			this.mStateManager.PopState();
			this.OnBack();
		}
	}

	public void SetOnBackListener(Action onBack)
	{
		this.mOnBackListener = onBack;
	}

	private void OnBack()
	{
		if (this.mOnBackListener != null)
		{
			this.mOnBackListener.Invoke();
		}
	}

	private void OnSelectedListItemListener(IAlbumModel albumModel)
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceSlotItemAlbumManager.State.SlotItemList;
		if (flag && albumModel is AlbumSlotModel)
		{
			AlbumSlotModel albumModel2 = (AlbumSlotModel)albumModel;
			this.mContext.SetAlbumModel(albumModel2);
			this.mUISlotItemAlbumList.SetKeyController(null);
			this.mStateManager.PushState(UserInterfaceSlotItemAlbumManager.State.SlotItemDetail);
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void StartState()
	{
		this.mContext = new UserInterfaceSlotItemAlbumManager.Context();
		this.mStateManager.PushState(UserInterfaceSlotItemAlbumManager.State.SlotItemList);
	}

	private void OnPushState(UserInterfaceSlotItemAlbumManager.State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		if (state != UserInterfaceSlotItemAlbumManager.State.SlotItemList)
		{
			if (state == UserInterfaceSlotItemAlbumManager.State.SlotItemDetail)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnPushSlotItemDetailState();
			}
		}
		else
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			this.mUISlotItemAlbumList.SetActive(true);
			this.mUISlotItemAlbumList.Initialize(this.mAlbumModels);
			this.mUISlotItemAlbumList.SetKeyController(this.mKeyController);
			this.mUISlotItemAlbumList.StartState();
		}
	}

	private void OnPushSlotItemDetailState()
	{
		IEnumerator enumerator = this.OnPushSlotItemDetailStateCoroutine();
		base.StartCoroutine(enumerator);
	}

	[DebuggerHidden]
	private IEnumerator OnPushSlotItemDetailStateCoroutine()
	{
		UserInterfaceSlotItemAlbumManager.<OnPushSlotItemDetailStateCoroutine>c__Iterator33 <OnPushSlotItemDetailStateCoroutine>c__Iterator = new UserInterfaceSlotItemAlbumManager.<OnPushSlotItemDetailStateCoroutine>c__Iterator33();
		<OnPushSlotItemDetailStateCoroutine>c__Iterator.<>f__this = this;
		return <OnPushSlotItemDetailStateCoroutine>c__Iterator;
	}

	private void OnPopState(UserInterfaceSlotItemAlbumManager.State state)
	{
	}

	private void OnResumeState(UserInterfaceSlotItemAlbumManager.State state)
	{
		if (state == UserInterfaceSlotItemAlbumManager.State.SlotItemList)
		{
			this.mUISlotItemAlbumList.SetKeyController(this.mKeyController);
			this.mUISlotItemAlbumList.ResumeState();
		}
	}

	private void OnDestroy()
	{
		this.mUISlotItemAlbumList = null;
		this.mUISlotItemAlbumDetail = null;
		this.mKeyController = null;
		this.mAlbumModels = null;
		this.mStateManager = null;
		this.mContext = null;
		this.mOnChangeStateUserInterfaceSlotItemAlbumManager = null;
	}

	internal void SetOnChangeStateListener(Action<UserInterfaceSlotItemAlbumManager.State> onChangeStateUserInterfaceSlotItemAlbumManager)
	{
		this.mOnChangeStateUserInterfaceSlotItemAlbumManager = onChangeStateUserInterfaceSlotItemAlbumManager;
	}

	private void OnChangeState(UserInterfaceSlotItemAlbumManager.State state)
	{
		if (this.mOnChangeStateUserInterfaceSlotItemAlbumManager != null)
		{
			this.mOnChangeStateUserInterfaceSlotItemAlbumManager.Invoke(state);
		}
	}
}
