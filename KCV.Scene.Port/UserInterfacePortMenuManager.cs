using DG.Tweening;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Scene.Port
{
	[RequireComponent(typeof(UIButtonManager)), RequireComponent(typeof(UIPanel))]
	public class UserInterfacePortMenuManager : MonoBehaviour
	{
		public enum State
		{
			None,
			FirstOpenMain,
			MainMenu,
			SubMenu,
			MoveToSubMenu,
			MoveToMainMenu,
			CallingNextScene,
			Wait
		}

		public class StateManager<State>
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

		private UIButtonManager mButtonManager;

		[Header("Sally"), SerializeField]
		private UIPortMenuCenterButton mUIPortMenuButton_Sally;

		[Header("Engage"), SerializeField]
		private UIPortMenuEngageButton mUIPortMenuButton_Engage;

		[Header("Main"), SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Organize;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Remodel;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Arsenal;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Duty;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Repair;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Supply;

		[Header("Sub"), SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Record;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Album;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Item;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Option;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Interior;

		[SerializeField]
		private UIPortMenuButton mUIPortMenuButton_Save;

		[SerializeField]
		private UITexture mTexture_Shaft;

		[SerializeField]
		private UIPortMenuAnimation mUIPortMenuAnimation;

		private UIPortMenuButton[] mUIPortMenuButtons_MainMenu;

		private UIPortMenuButton[] mUIPortMenuButtons_SubMenu;

		private UIPortMenuButton[] mUIPortMenuButtons_Current;

		private UIPortMenuButton mUIPortMenuButton_Current;

		private AudioClip mAudioClip_MainMenuOnMouse;

		private UIPanel mPanelThis;

		private UserInterfacePortMenuManager.StateManager<UserInterfacePortMenuManager.State> mStateManager;

		private KeyControl mKeyController;

		private PortManager mPortManager;

		private DeckModel mDeckModel;

		private Action<Generics.Scene> mOnSelectedSceneListener;

		private Action mOnFirstOpendListener;

		public float alpha
		{
			get
			{
				if (this.mPanelThis != null)
				{
					return this.mPanelThis.alpha;
				}
				return -1f;
			}
			set
			{
				if (this.mPanelThis != null)
				{
					this.mPanelThis.alpha = value;
				}
			}
		}

		public UserInterfacePortMenuManager.State GetCurrentState()
		{
			return this.mStateManager.CurrentState;
		}

		private void Awake()
		{
			this.mButtonManager = base.GetComponent<UIButtonManager>();
			this.mPanelThis = base.GetComponent<UIPanel>();
			this.mButtonManager.IndexChangeAct = delegate
			{
				UIPortMenuButton uIPortMenuButton = this.mUIPortMenuButtons_Current[this.mButtonManager.nowForcusIndex];
				bool isSelectable = this.mUIPortMenuButtons_Current[this.mButtonManager.nowForcusIndex].IsSelectable;
				bool flag = this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu;
				bool flag2 = this.mUIPortMenuButton_Current != null && this.mUIPortMenuButton_Current.Equals(uIPortMenuButton);
				bool flag3 = this.IsControllable();
				if (flag && isSelectable && !flag2 && flag3)
				{
					this.ChangeFocusButton(uIPortMenuButton);
				}
			};
			this.mAudioClip_MainMenuOnMouse = SoundFile.LoadSE(SEFIleInfos.MainMenuOnMouse);
		}

		private bool IsControllable()
		{
			return this.mKeyController != null && this.mKeyController.IsRun;
		}

		private UIPortMenuButton[] GeneratePortMenuMain()
		{
			List<UIPortMenuButton> list = new List<UIPortMenuButton>();
			list.Add(this.mUIPortMenuButton_Sally);
			list.Add(this.mUIPortMenuButton_Supply);
			list.Add(this.mUIPortMenuButton_Organize);
			list.Add(this.mUIPortMenuButton_Remodel);
			list.Add(this.mUIPortMenuButton_Arsenal);
			list.Add(this.mUIPortMenuButton_Duty);
			list.Add(this.mUIPortMenuButton_Repair);
			list.Add(this.mUIPortMenuButton_Engage);
			this.mUIPortMenuButton_Sally.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Sally.SetOnLongPressListener(new Action(this.OnLongPressEventListener));
			this.mUIPortMenuButton_Organize.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Remodel.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Arsenal.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Duty.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Repair.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Supply.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			return list.ToArray();
		}

		private UIPortMenuButton[] GeneratePortMenuSub()
		{
			List<UIPortMenuButton> list = new List<UIPortMenuButton>();
			list.Add(this.mUIPortMenuButton_Sally);
			list.Add(this.mUIPortMenuButton_Save);
			list.Add(this.mUIPortMenuButton_Record);
			list.Add(this.mUIPortMenuButton_Album);
			list.Add(this.mUIPortMenuButton_Item);
			list.Add(this.mUIPortMenuButton_Option);
			list.Add(this.mUIPortMenuButton_Interior);
			list.Add(this.mUIPortMenuButton_Engage);
			this.mUIPortMenuButton_Record.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Album.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Item.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Option.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Interior.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Save.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			this.mUIPortMenuButton_Engage.SetOnClickEventListener(new Action<UIPortMenuButton>(this.OnPortMenuButtonClickEventListener));
			return list.ToArray();
		}

		private void OnLongPressEventListener()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu;
			bool flag2 = this.IsControllable();
			if (flag && flag2)
			{
				UserInterfacePortMenuManager.State currentState = this.mStateManager.CurrentState;
				if (currentState != UserInterfacePortMenuManager.State.MainMenu)
				{
					if (currentState == UserInterfacePortMenuManager.State.SubMenu)
					{
						this.mStateManager.PopState();
						this.mStateManager.PushState(UserInterfacePortMenuManager.State.MoveToMainMenu);
						if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
						{
							SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
						}
					}
				}
				else
				{
					this.mStateManager.PopState();
					this.mStateManager.PushState(UserInterfacePortMenuManager.State.MoveToSubMenu);
					if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
					{
						SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
					}
				}
			}
		}

		private void OnPortMenuButtonClickEventListener(UIPortMenuButton calledObject)
		{
			bool flag = -1 < Array.IndexOf<UIPortMenuButton>(this.mUIPortMenuButtons_Current, calledObject);
			bool isSelectable = calledObject.IsSelectable;
			bool flag2 = this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu;
			bool flag3 = this.IsControllable();
			if (flag2 && flag && isSelectable && flag3)
			{
				this.mStateManager.PushState(UserInterfacePortMenuManager.State.CallingNextScene);
				this.ChangeFocusButton(calledObject);
				Tween tween = calledObject.GenerateTweenClick();
				TweenSettingsExtensions.OnComplete<Tween>(tween, delegate
				{
					bool flag4 = this.mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu;
					if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu && !flag4)
					{
						this.mUIPortMenuAnimation.Initialize(calledObject);
						this.mUIPortMenuAnimation.PlayCollectSubAnimation();
					}
					else
					{
						this.OnSelectedScene(calledObject.GetScene());
					}
				});
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				if (this.mKeyController.keyState.get_Item(14).down)
				{
					if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
					{
						this.OnPressKeyLeft();
					}
				}
				else if (this.mKeyController.keyState.get_Item(10).down)
				{
					if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
					{
						this.OnPressKeyRight();
					}
				}
				else if (this.mKeyController.keyState.get_Item(8).down)
				{
					if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
					{
						this.OnPressKeyUp();
					}
				}
				else if (this.mKeyController.keyState.get_Item(12).down)
				{
					if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
					{
						this.OnPressKeyDown();
					}
				}
				else if (this.mKeyController.keyState.get_Item(4).down)
				{
					if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
					{
						UserInterfacePortMenuManager.State currentState = this.mStateManager.CurrentState;
						if (currentState != UserInterfacePortMenuManager.State.MainMenu)
						{
							if (currentState == UserInterfacePortMenuManager.State.SubMenu)
							{
								this.mStateManager.PopState();
								this.mStateManager.PushState(UserInterfacePortMenuManager.State.MoveToMainMenu);
								if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
								{
									SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Show();
								}
							}
						}
						else
						{
							this.mStateManager.PopState();
							this.mStateManager.PushState(UserInterfacePortMenuManager.State.MoveToSubMenu);
							if (SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide() != null)
							{
								SingletonMonoBehaviour<PortObjectManager>.Instance.GetTutorialGuide().Hide();
							}
						}
					}
				}
				else if (this.mKeyController.keyState.get_Item(5).down)
				{
					if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
					{
						this.ChangeFocusButton(this.mUIPortMenuButton_Sally);
						this.mUIPortMenuButton_Current.ClickEvent();
					}
				}
				else if (this.mKeyController.keyState.get_Item(1).down)
				{
					bool flag = this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu || this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu;
					if (flag)
					{
						this.mUIPortMenuButton_Current.ClickEvent();
					}
				}
			}
		}

		private void OnPressKeyLeft()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu)
			{
				uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Left;
			}
			else if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
			{
				if (this.mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)
				{
					uIPortMenuButton = ((UIPortMenuButton.CompositeMenu)this.mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Left;
				}
				else
				{
					uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Left;
				}
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				this.ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnPressKeyDown()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu)
			{
				uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Down;
			}
			else if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
			{
				if (this.mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)
				{
					uIPortMenuButton = ((UIPortMenuButton.CompositeMenu)this.mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Down;
				}
				else
				{
					uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Down;
				}
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				this.ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnPressKeyRight()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu)
			{
				uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Right;
			}
			else if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
			{
				if (this.mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)
				{
					uIPortMenuButton = ((UIPortMenuButton.CompositeMenu)this.mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Right;
				}
				else
				{
					uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Right;
				}
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				this.ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnPressKeyUp()
		{
			UIPortMenuButton uIPortMenuButton = null;
			if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu)
			{
				uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Top;
			}
			else if (this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu)
			{
				if (this.mUIPortMenuButton_Current is UIPortMenuButton.CompositeMenu)
				{
					uIPortMenuButton = ((UIPortMenuButton.CompositeMenu)this.mUIPortMenuButton_Current).GetSubMenuKeyMap().mUIPortMenuButton_Top;
				}
				else
				{
					uIPortMenuButton = this.mUIPortMenuButton_Current.GetKeyMap().mUIPortMenuButton_Top;
				}
			}
			if (uIPortMenuButton != null && uIPortMenuButton.IsSelectable)
			{
				this.ChangeFocusButton(uIPortMenuButton);
			}
		}

		private void OnFinishedCollectAnimationListener()
		{
			this.OnSelectedScene(this.mUIPortMenuButton_Current.GetScene());
		}

		public void StartState()
		{
			this.mStateManager.PushState(UserInterfacePortMenuManager.State.FirstOpenMain);
		}

		public void StartWaitingState()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.MainMenu | this.mStateManager.CurrentState == UserInterfacePortMenuManager.State.SubMenu;
			if (flag)
			{
				this.mStateManager.PushState(UserInterfacePortMenuManager.State.Wait);
			}
		}

		public void ResumeState()
		{
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void Initialize(PortManager portManager, DeckModel deckModel)
		{
			this.mPortManager = portManager;
			this.mDeckModel = deckModel;
			this.mUIPortMenuButton_Sally.ChangeState(UIPortMenuCenterButton.State.MainMenu);
			this.mUIPortMenuAnimation.Initialize(null);
			if (this.mUIPortMenuButton_Current != null)
			{
				this.mUIPortMenuButton_Current.GenerateTweenRemoveHover();
				this.mUIPortMenuButton_Current.RemoveHover();
				this.mUIPortMenuButton_Current = null;
			}
			this.mUIPortMenuButtons_MainMenu = this.GeneratePortMenuMain();
			this.mUIPortMenuButtons_SubMenu = this.GeneratePortMenuSub();
			this.mTexture_Shaft.get_transform().set_localScale(Vector3.get_zero());
			UIPortMenuButton[] array = this.mUIPortMenuButtons_SubMenu;
			for (int i = 0; i < array.Length; i++)
			{
				UIPortMenuButton uIPortMenuButton = array[i];
				bool selectable = this.IsValidSelectable(this.mPortManager, this.mDeckModel, uIPortMenuButton);
				Vector3 localPosition = uIPortMenuButton.get_transform().get_localPosition();
				uIPortMenuButton.get_transform().set_localPosition(this.mUIPortMenuButton_Sally.get_transform().get_localPosition());
				uIPortMenuButton.Initialize(selectable);
				uIPortMenuButton.get_gameObject().SetActive(false);
				uIPortMenuButton.alpha = 0f;
			}
			UIPortMenuButton[] array2 = this.mUIPortMenuButtons_MainMenu;
			for (int j = 0; j < array2.Length; j++)
			{
				UIPortMenuButton uIPortMenuButton2 = array2[j];
				bool selectable2 = this.IsValidSelectable(this.mPortManager, this.mDeckModel, uIPortMenuButton2);
				Vector3 localPosition2 = uIPortMenuButton2.get_transform().get_localPosition();
				uIPortMenuButton2.get_transform().set_localPosition(this.mUIPortMenuButton_Sally.get_transform().get_localPosition());
				uIPortMenuButton2.get_transform().set_localScale(Vector3.get_zero());
				uIPortMenuButton2.Initialize(selectable2);
				uIPortMenuButton2.get_gameObject().SetActive(false);
				uIPortMenuButton2.alpha = 0f;
			}
			this.mUIPortMenuAnimation.SetOnFinishedCollectAnimationListener(new Action(this.OnFinishedCollectAnimationListener));
			UIPortMenuButton[] array3 = this.mUIPortMenuButtons_MainMenu;
			for (int k = 0; k < array3.Length; k++)
			{
				UIPortMenuButton uIPortMenuButton3 = array3[k];
				bool selectable3 = this.IsValidSelectable(this.mPortManager, this.mDeckModel, uIPortMenuButton3);
				uIPortMenuButton3.Initialize(selectable3);
			}
			UIPortMenuButton[] array4 = this.mUIPortMenuButtons_SubMenu;
			for (int l = 0; l < array4.Length; l++)
			{
				UIPortMenuButton uIPortMenuButton4 = array4[l];
				bool selectable4 = this.IsValidSelectable(this.mPortManager, this.mDeckModel, uIPortMenuButton4);
				uIPortMenuButton4.Initialize(selectable4);
			}
			this.mStateManager = new UserInterfacePortMenuManager.StateManager<UserInterfacePortMenuManager.State>(UserInterfacePortMenuManager.State.None);
			this.mStateManager.OnPush = new Action<UserInterfacePortMenuManager.State>(this.OnPushState);
			this.mStateManager.OnPop = new Action<UserInterfacePortMenuManager.State>(this.OnPopState);
			this.mStateManager.OnResume = new Action<UserInterfacePortMenuManager.State>(this.OnResumeState);
		}

		public void SetOnSelectedSceneListener(Action<Generics.Scene> onSelectedSceneListener)
		{
			this.mOnSelectedSceneListener = onSelectedSceneListener;
		}

		private void OnSelectedScene(Generics.Scene selectedScene)
		{
			if (this.mOnSelectedSceneListener != null)
			{
				this.mOnSelectedSceneListener.Invoke(selectedScene);
			}
		}

		private bool IsValidSelectable(PortManager portManager, DeckModel deckModel, UIPortMenuButton portMenuButton)
		{
			Generics.Scene scene = portMenuButton.GetScene();
			if (scene != Generics.Scene.Marriage)
			{
				return SingletonMonoBehaviour<AppInformation>.Instance.IsValidMoveToScene(portMenuButton.GetScene());
			}
			return portManager.IsValidMarriage(deckModel.GetFlagShip().MemId);
		}

		private void OnPushState(UserInterfacePortMenuManager.State state)
		{
			switch (state)
			{
			case UserInterfacePortMenuManager.State.FirstOpenMain:
				this.OnPushFirstOpenMain();
				break;
			case UserInterfacePortMenuManager.State.MainMenu:
				this.OnPushMainMenu();
				break;
			case UserInterfacePortMenuManager.State.SubMenu:
				this.OnPushSubMenu();
				break;
			case UserInterfacePortMenuManager.State.MoveToSubMenu:
				this.OnPushMoveToSubMenu();
				break;
			case UserInterfacePortMenuManager.State.MoveToMainMenu:
				this.OnPushMoveToMainMenu();
				break;
			}
		}

		private void OnPushSubMenu()
		{
			this.mUIPortMenuButtons_Current = this.mUIPortMenuButtons_SubMenu;
			this.mButtonManager.UpdateButtons(this.mUIPortMenuButtons_Current);
			this.SetKeyController(this.mKeyController);
		}

		private void OnPushMainMenu()
		{
			this.mUIPortMenuButtons_Current = this.mUIPortMenuButtons_MainMenu;
			this.mButtonManager.UpdateButtons(this.mUIPortMenuButtons_Current);
			this.SetKeyController(this.mKeyController);
		}

		private void OnPushFirstOpenMain()
		{
			base.StartCoroutine(this.OnPushFirstOpenMainCoroutine());
		}

		public void SetOnFirstOpendListener(Action onFirstOpendListener)
		{
			this.mOnFirstOpendListener = onFirstOpendListener;
		}

		[DebuggerHidden]
		private IEnumerator OnPushFirstOpenMainCoroutine()
		{
			UserInterfacePortMenuManager.<OnPushFirstOpenMainCoroutine>c__IteratorAE <OnPushFirstOpenMainCoroutine>c__IteratorAE = new UserInterfacePortMenuManager.<OnPushFirstOpenMainCoroutine>c__IteratorAE();
			<OnPushFirstOpenMainCoroutine>c__IteratorAE.<>f__this = this;
			return <OnPushFirstOpenMainCoroutine>c__IteratorAE;
		}

		private void OnPopState(UserInterfacePortMenuManager.State state)
		{
			switch (state)
			{
			case UserInterfacePortMenuManager.State.MainMenu:
				this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_MainMenu, false);
				break;
			case UserInterfacePortMenuManager.State.SubMenu:
				this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_SubMenu, false);
				break;
			}
		}

		private void OnResumeState(UserInterfacePortMenuManager.State state)
		{
			switch (state)
			{
			case UserInterfacePortMenuManager.State.FirstOpenMain:
			case UserInterfacePortMenuManager.State.MainMenu:
			case UserInterfacePortMenuManager.State.SubMenu:
				this.ChangeFocusButton(this.mUIPortMenuButton_Current);
				break;
			}
		}

		private Tween GenerateTweenCloseMain()
		{
			this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_MainMenu, false);
			return this.GenerateTweenCloseButtons(this.mUIPortMenuButtons_MainMenu);
		}

		private Tween GenerateTweenCloseSubMenu()
		{
			this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_SubMenu, false);
			return this.GenerateTweenCloseButtons(this.mUIPortMenuButtons_SubMenu);
		}

		private Tween GenerateTweenOpenMainMenu()
		{
			this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_MainMenu, true);
			return this.GenerateTweenOpenButtons(this.mUIPortMenuButtons_MainMenu);
		}

		private Tween GenerateTweenOpenSubMenu()
		{
			this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_SubMenu, true);
			return this.GenerateTweenOpenButtons(this.mUIPortMenuButtons_SubMenu);
		}

		private Tween GenerateTweenCloseButtons(UIPortMenuButton[] targetPortMenuButtons)
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			for (int i = 0; i < targetPortMenuButtons.Length; i++)
			{
				UIPortMenuButton uIPortMenuButton = targetPortMenuButtons[i];
				if (uIPortMenuButton.Equals(this.mUIPortMenuButton_Sally))
				{
					Tween tween = ShortcutExtensions.DOScale(uIPortMenuButton.get_transform(), new Vector3(0.6f, 0.6f), 0.15f);
					TweenSettingsExtensions.Join(sequence, tween);
				}
				else if (!uIPortMenuButton.Equals(this.mUIPortMenuButton_Engage))
				{
					Vector3 defaultLocalPosition = this.mUIPortMenuButton_Sally.GetDefaultLocalPosition();
					Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(uIPortMenuButton.get_transform(), defaultLocalPosition, 0.15f, false), 6), this);
					TweenSettingsExtensions.Join(sequence, tween2);
				}
			}
			return sequence;
		}

		private Tween GenerateTweenOpenButtons(UIPortMenuButton[] targetPortMenuButtons)
		{
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			for (int i = 0; i < targetPortMenuButtons.Length; i++)
			{
				UIPortMenuButton uIPortMenuButton = targetPortMenuButtons[i];
				if (uIPortMenuButton.Equals(this.mUIPortMenuButton_Sally))
				{
					Tween tween = ShortcutExtensions.DOScale(uIPortMenuButton.get_transform(), Vector3.get_one(), 0.15f);
					TweenSettingsExtensions.Append(sequence, tween);
				}
				else if (!uIPortMenuButton.Equals(this.mUIPortMenuButton_Engage))
				{
					Vector3 defaultLocalPosition = uIPortMenuButton.GetDefaultLocalPosition();
					Tween tween2 = TweenSettingsExtensions.SetId<Tweener>(TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMove(uIPortMenuButton.get_transform(), defaultLocalPosition, 0.15f, false), 6), this);
					TweenSettingsExtensions.Join(sequence, tween2);
				}
			}
			return sequence;
		}

		private void ChangeActiveMenuButtons(UIPortMenuButton[] targetButtons, bool activeState)
		{
			for (int i = 0; i < targetButtons.Length; i++)
			{
				UIPortMenuButton uIPortMenuButton = targetButtons[i];
				bool flag = !uIPortMenuButton.Equals(this.mUIPortMenuButton_Sally);
				flag &= !uIPortMenuButton.Equals(this.mUIPortMenuButton_Engage);
				if (flag)
				{
					if (activeState)
					{
						uIPortMenuButton.get_transform().set_localScale(Vector3.get_one());
						uIPortMenuButton.alpha = 1f;
						uIPortMenuButton.SetActive(true);
					}
					else
					{
						uIPortMenuButton.get_transform().set_localScale(Vector3.get_zero());
						uIPortMenuButton.alpha = 0f;
						uIPortMenuButton.SetActive(false);
					}
					uIPortMenuButton.get_gameObject().SetActive(activeState);
				}
			}
		}

		private void OnPushMoveToMainMenu()
		{
			this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_MainMenu, true);
			this.ChangeFocusableButtons(this.mUIPortMenuButtons_MainMenu);
			DOTween.Kill(this, false);
			Tween tween = this.GenerateTweenCloseSubMenu();
			TweenCallback tweenCallback = delegate
			{
				this.ChangeFocusButton(this.mUIPortMenuButton_Sally);
				this.mUIPortMenuButton_Sally.ChangeState(UIPortMenuCenterButton.State.MainMenu);
			};
			Tween tween2 = this.GenerateTweenOpenMainMenu();
			TweenCallback tweenCallback2 = delegate
			{
				this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_SubMenu, false);
				this.mStateManager.PopState();
				this.mStateManager.PushState(UserInterfacePortMenuManager.State.MainMenu);
			};
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, tweenCallback2);
		}

		private void OnPushMoveToSubMenu()
		{
			this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_SubMenu, true);
			this.ChangeFocusableButtons(this.mUIPortMenuButtons_SubMenu);
			DOTween.Kill(this, false);
			Tween tween = this.GenerateTweenCloseMain();
			TweenCallback tweenCallback = delegate
			{
				this.ChangeFocusButton(this.mUIPortMenuButton_Sally);
				this.mUIPortMenuButton_Sally.ChangeState(UIPortMenuCenterButton.State.SubMenu);
			};
			Tween tween2 = this.GenerateTweenOpenSubMenu();
			TweenCallback tweenCallback2 = delegate
			{
				this.ChangeActiveMenuButtons(this.mUIPortMenuButtons_MainMenu, false);
				this.mStateManager.PopState();
				this.mStateManager.PushState(UserInterfacePortMenuManager.State.SubMenu);
			};
			Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this);
			TweenSettingsExtensions.Append(sequence, tween);
			TweenSettingsExtensions.AppendCallback(sequence, tweenCallback);
			TweenSettingsExtensions.Append(sequence, tween2);
			TweenSettingsExtensions.OnComplete<Sequence>(sequence, tweenCallback2);
		}

		private void ChangeFocusButton(UIPortMenuButton targetButton)
		{
			if (this.mUIPortMenuButton_Current != null)
			{
				DOTween.Kill(this.mUIPortMenuButton_Current, false);
				Sequence sequence = TweenSettingsExtensions.SetId<Sequence>(TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this.mUIPortMenuButton_Current), this.mUIPortMenuButton_Current);
				Tween tween = TweenSettingsExtensions.SetId<Tween>(TweenSettingsExtensions.SetId<Tween>(this.mUIPortMenuButton_Current.GenerateTweenRemoveHover(), this.mUIPortMenuButton_Current), this.mUIPortMenuButton_Current);
				Tween tween2 = TweenSettingsExtensions.SetId<Tween>(TweenSettingsExtensions.SetId<Tween>(this.mUIPortMenuButton_Current.GenerateTweenRemoveFocus(), this.mUIPortMenuButton_Current), this.mUIPortMenuButton_Current);
				TweenSettingsExtensions.Join(sequence, tween);
				TweenSettingsExtensions.Join(sequence, tween2);
				this.mUIPortMenuButton_Current.RemoveHover();
			}
			this.mUIPortMenuButton_Current = targetButton;
			if (this.mUIPortMenuButton_Current != null)
			{
				DOTween.Kill(this.mUIPortMenuButton_Current, false);
				Sequence sequence2 = TweenSettingsExtensions.SetId<Sequence>(TweenSettingsExtensions.SetId<Sequence>(DOTween.Sequence(), this.mUIPortMenuButton_Current), this.mUIPortMenuButton_Current);
				Tween tween3 = TweenSettingsExtensions.SetId<Tween>(this.mUIPortMenuButton_Current.GenerateTweenHoverScale(), this.mUIPortMenuButton_Current);
				Tween tween4 = TweenSettingsExtensions.SetId<Tween>(this.mUIPortMenuButton_Current.GenerateTweenFocus(), this.mUIPortMenuButton_Current);
				TweenSettingsExtensions.Join(sequence2, tween3);
				TweenSettingsExtensions.Join(sequence2, tween4);
				SoundUtils.PlaySE(this.mAudioClip_MainMenuOnMouse);
				this.mUIPortMenuButton_Current.Hover();
			}
		}

		private void ChangeFocusableButtons(UIPortMenuButton[] target)
		{
			this.mUIPortMenuButtons_Current = target;
		}

		public override string ToString()
		{
			if (this.mStateManager != null)
			{
				return this.mStateManager.ToString();
			}
			return base.ToString();
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mAudioClip_MainMenuOnMouse, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mTexture_Shaft, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mPanelThis);
			this.mButtonManager = null;
			this.mUIPortMenuButton_Sally = null;
			this.mUIPortMenuButton_Engage = null;
			this.mUIPortMenuButton_Organize = null;
			this.mUIPortMenuButton_Remodel = null;
			this.mUIPortMenuButton_Arsenal = null;
			this.mUIPortMenuButton_Duty = null;
			this.mUIPortMenuButton_Repair = null;
			this.mUIPortMenuButton_Supply = null;
			this.mUIPortMenuButton_Record = null;
			this.mUIPortMenuButton_Album = null;
			this.mUIPortMenuButton_Item = null;
			this.mUIPortMenuButton_Option = null;
			this.mUIPortMenuButton_Interior = null;
			this.mUIPortMenuButton_Save = null;
			this.mUIPortMenuAnimation = null;
			this.mUIPortMenuButtons_MainMenu = null;
			this.mUIPortMenuButtons_SubMenu = null;
			this.mUIPortMenuButtons_Current = null;
			this.mUIPortMenuButton_Current = null;
			this.mStateManager = null;
			this.mKeyController = null;
			this.mPortManager = null;
		}

		public string StateToString()
		{
			return this.mStateManager.ToString();
		}
	}
}
