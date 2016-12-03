using DG.Tweening;
using KCV;
using KCV.PortTop;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[SelectionBase]
public class UserInterfaceShipAlbumManager : MonoBehaviour
{
	public enum State
	{
		None,
		ShipList,
		ShipDetail,
		ShipDetailMarriaged,
		MarriagedMovie
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

	private BGMFileInfos DefaultBGM = BGMFileInfos.Port;

	[SerializeField]
	private UIShipAlbumList mUIShipAlbumList;

	[SerializeField]
	private UIShipAlbumDetail mUIShipAlbumDetail;

	[SerializeField]
	private UIShipAlbumDetailForMarriaged mUIShipAlbumDetailForMarriaged;

	private MarriageCutManager mMarriageCutManager;

	private KeyControl mKeyController;

	private IAlbumModel[] mAlbumModels;

	private UserInterfaceShipAlbumManager.StateManager<UserInterfaceShipAlbumManager.State> mStateManager;

	private UserInterfaceShipAlbumManager.Context mContext;

	private Action mOnBackListener;

	private Action<UserInterfaceShipAlbumManager.State> mOnChangeStateUserInterfaceShipAlbumManager;

	public bool Initialized
	{
		get;
		private set;
	}

	public void Initialize(IAlbumModel[] albumModels)
	{
		this.mAlbumModels = albumModels;
		this.mUIShipAlbumList.SetOnSelectedListItemListener(new Action<IAlbumModel>(this.OnSelectedListItemListener));
		this.mUIShipAlbumList.SetOnBackListener(new Action(this.OnBackListListener));
		this.mUIShipAlbumDetail.SetActive(false);
		this.mUIShipAlbumDetail.SetOnBackListener(new Action<Tween>(this.OnBackAlbumDetailListener));
		this.mUIShipAlbumDetailForMarriaged.SetActive(false);
		this.mUIShipAlbumDetailForMarriaged.SetOnBackListener(new Action<Tween>(this.OnBackAlbumDetailForMarriagedListener));
		this.mUIShipAlbumDetailForMarriaged.SetOnRequestPlayMarriageMovieListener(new Action(this.OnRequestPlayMarriageMovie));
		this.mStateManager = new UserInterfaceShipAlbumManager.StateManager<UserInterfaceShipAlbumManager.State>(UserInterfaceShipAlbumManager.State.None);
		this.mStateManager.OnPush = new Action<UserInterfaceShipAlbumManager.State>(this.OnPushState);
		this.mStateManager.OnPop = new Action<UserInterfaceShipAlbumManager.State>(this.OnPopState);
		this.mStateManager.OnResume = new Action<UserInterfaceShipAlbumManager.State>(this.OnResumeState);
		this.mStateManager.OnSwitch = new Action<UserInterfaceShipAlbumManager.State>(this.OnChangeState);
		this.Initialized = true;
	}

	private void OnRequestPlayMarriageMovie()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceShipAlbumManager.State.ShipDetailMarriaged;
		if (flag)
		{
			this.mStateManager.PushState(UserInterfaceShipAlbumManager.State.MarriagedMovie);
		}
	}

	private void OnBackAlbumDetailListener(Tween closeTween)
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceShipAlbumManager.State.ShipDetail;
		if (flag)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIShipAlbumDetail.SetKeyController(null);
			TweenSettingsExtensions.OnComplete<Tween>(closeTween, delegate
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
				this.mUIShipAlbumDetail.SetActive(false);
			});
		}
	}

	private void OnBackAlbumDetailForMarriagedListener(Tween closeTween)
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceShipAlbumManager.State.ShipDetailMarriaged;
		if (flag)
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIShipAlbumDetailForMarriaged.SetKeyController(null);
			TweenSettingsExtensions.OnComplete<Tween>(closeTween, delegate
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
				this.mUIShipAlbumDetailForMarriaged.SetActive(false);
			});
		}
	}

	private void OnBackListListener()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceShipAlbumManager.State.ShipList;
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
		bool flag = this.mStateManager.CurrentState == UserInterfaceShipAlbumManager.State.ShipList;
		if (flag && albumModel is AlbumShipModel)
		{
			AlbumShipModel albumShipModel = (AlbumShipModel)albumModel;
			this.mContext.SetAlbumModel(albumShipModel);
			this.mUIShipAlbumList.SetKeyController(null);
			bool flag2 = UserInterfaceAlbumManager.CheckMarriaged(albumShipModel);
			if (flag2)
			{
				this.mStateManager.PushState(UserInterfaceShipAlbumManager.State.ShipDetailMarriaged);
			}
			else
			{
				this.mStateManager.PushState(UserInterfaceShipAlbumManager.State.ShipDetail);
			}
		}
	}

	public void SetKeyController(KeyControl keyController)
	{
		this.mKeyController = keyController;
	}

	public void StartState()
	{
		this.mContext = new UserInterfaceShipAlbumManager.Context();
		this.mStateManager.PushState(UserInterfaceShipAlbumManager.State.ShipList);
	}

	private void OnPushState(UserInterfaceShipAlbumManager.State state)
	{
		SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
		switch (state)
		{
		case UserInterfaceShipAlbumManager.State.ShipList:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			this.OnPushShipListState();
			break;
		case UserInterfaceShipAlbumManager.State.ShipDetail:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			this.OnPushShipDetailState();
			break;
		case UserInterfaceShipAlbumManager.State.ShipDetailMarriaged:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
			this.OnPushShipDetailForMarriagedState();
			break;
		case UserInterfaceShipAlbumManager.State.MarriagedMovie:
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			this.mUIShipAlbumDetailForMarriaged.SetKeyController(null);
			this.OnPushMarriagedMovieState();
			break;
		}
	}

	private void OnPushShipListState()
	{
		IEnumerator enumerator = this.OnPushShipListStateCoroutine();
		base.StartCoroutine(enumerator);
	}

	[DebuggerHidden]
	private IEnumerator OnPushShipListStateCoroutine()
	{
		UserInterfaceShipAlbumManager.<OnPushShipListStateCoroutine>c__Iterator2F <OnPushShipListStateCoroutine>c__Iterator2F = new UserInterfaceShipAlbumManager.<OnPushShipListStateCoroutine>c__Iterator2F();
		<OnPushShipListStateCoroutine>c__Iterator2F.<>f__this = this;
		return <OnPushShipListStateCoroutine>c__Iterator2F;
	}

	private void OnPushMarriagedMovieState()
	{
		SingletonMonoBehaviour<SoundManager>.Instance.StopVoice();
		int graphicShipId = this.mUIShipAlbumDetailForMarriaged.FocusTextureInfo().GetGraphicShipId();
		IEnumerator enumerator = this.OnPushMarriagedMovieStateCoroutine(graphicShipId);
		base.StartCoroutine(enumerator);
	}

	[DebuggerHidden]
	private IEnumerator OnPushMarriagedMovieStateCoroutine(int graphicShipId)
	{
		UserInterfaceShipAlbumManager.<OnPushMarriagedMovieStateCoroutine>c__Iterator30 <OnPushMarriagedMovieStateCoroutine>c__Iterator = new UserInterfaceShipAlbumManager.<OnPushMarriagedMovieStateCoroutine>c__Iterator30();
		<OnPushMarriagedMovieStateCoroutine>c__Iterator.graphicShipId = graphicShipId;
		<OnPushMarriagedMovieStateCoroutine>c__Iterator.<$>graphicShipId = graphicShipId;
		<OnPushMarriagedMovieStateCoroutine>c__Iterator.<>f__this = this;
		return <OnPushMarriagedMovieStateCoroutine>c__Iterator;
	}

	private void OnPushShipDetailState()
	{
		IEnumerator enumerator = this.OnPushShipDetailStateCoroutine();
		base.StartCoroutine(enumerator);
	}

	[DebuggerHidden]
	private IEnumerator OnPushShipDetailStateCoroutine()
	{
		UserInterfaceShipAlbumManager.<OnPushShipDetailStateCoroutine>c__Iterator31 <OnPushShipDetailStateCoroutine>c__Iterator = new UserInterfaceShipAlbumManager.<OnPushShipDetailStateCoroutine>c__Iterator31();
		<OnPushShipDetailStateCoroutine>c__Iterator.<>f__this = this;
		return <OnPushShipDetailStateCoroutine>c__Iterator;
	}

	private void OnPushShipDetailForMarriagedState()
	{
		IEnumerator enumerator = this.OnPushShipDetailForMarriagedStateCoroutine();
		base.StartCoroutine(enumerator);
	}

	[DebuggerHidden]
	private IEnumerator OnPushShipDetailForMarriagedStateCoroutine()
	{
		UserInterfaceShipAlbumManager.<OnPushShipDetailForMarriagedStateCoroutine>c__Iterator32 <OnPushShipDetailForMarriagedStateCoroutine>c__Iterator = new UserInterfaceShipAlbumManager.<OnPushShipDetailForMarriagedStateCoroutine>c__Iterator32();
		<OnPushShipDetailForMarriagedStateCoroutine>c__Iterator.<>f__this = this;
		return <OnPushShipDetailForMarriagedStateCoroutine>c__Iterator;
	}

	private void OnFinishedMarriageMovie()
	{
		bool flag = this.mStateManager.CurrentState == UserInterfaceShipAlbumManager.State.MarriagedMovie;
		if (flag)
		{
			TweenSettingsExtensions.OnComplete<Tweener>(DOVirtual.Float(this.mMarriageCutManager.Alpha, 0f, 0.3f, delegate(float alpha)
			{
				this.mMarriageCutManager.Alpha = alpha;
			}), delegate
			{
				SingletonMonoBehaviour<SoundManager>.Instance.SwitchBGM(this.DefaultBGM);
				this.mMarriageCutManager.SetActive(false);
				Object.Destroy(this.mMarriageCutManager.get_gameObject());
				this.mMarriageCutManager = null;
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			});
		}
	}

	private void OnPopState(UserInterfaceShipAlbumManager.State state)
	{
		if (state == UserInterfaceShipAlbumManager.State.MarriagedMovie)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
		}
	}

	private void OnResumeState(UserInterfaceShipAlbumManager.State state)
	{
		switch (state)
		{
		case UserInterfaceShipAlbumManager.State.ShipList:
			this.mUIShipAlbumList.SetKeyController(this.mKeyController);
			this.mUIShipAlbumList.ResumeState();
			break;
		case UserInterfaceShipAlbumManager.State.ShipDetailMarriaged:
			this.mUIShipAlbumDetailForMarriaged.SetKeyController(this.mKeyController);
			break;
		}
	}

	internal void SetOnChangeStateListener(Action<UserInterfaceShipAlbumManager.State> onChangeStateUserInterfaceShipAlbumManager)
	{
		this.mOnChangeStateUserInterfaceShipAlbumManager = onChangeStateUserInterfaceShipAlbumManager;
	}

	private void OnChangeState(UserInterfaceShipAlbumManager.State state)
	{
		if (this.mOnChangeStateUserInterfaceShipAlbumManager != null)
		{
			this.mOnChangeStateUserInterfaceShipAlbumManager.Invoke(state);
		}
	}

	private void OnDestroy()
	{
		this.mUIShipAlbumList = null;
		this.mUIShipAlbumDetail = null;
		this.mUIShipAlbumDetailForMarriaged = null;
		this.mMarriageCutManager = null;
		this.mKeyController = null;
		this.mAlbumModels = null;
		this.mStateManager = null;
		this.mContext = null;
		this.mOnChangeStateUserInterfaceShipAlbumManager = null;
	}
}
