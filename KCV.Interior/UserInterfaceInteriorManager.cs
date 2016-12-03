using Common.Enum;
using KCV.InteriorStore;
using KCV.Scene.Others;
using KCV.Scene.Port;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Interior
{
	public class UserInterfaceInteriorManager : MonoBehaviour
	{
		private enum State
		{
			NONE,
			InteriorChange,
			FurnitureStore,
			MoveToFurnitureStore,
			MoveToInteriorChange
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

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private UserInterfaceInteriorChangeManager mUserInterfaceInteriorChangeManager;

		private UIInteriorStoreManager mUIInteriorStoreManager;

		[SerializeField]
		private UserInterfaceInteriorTransitionManager mUserInterfaceInteriorTransitionManager;

		[SerializeField]
		private UserInterfacePortInteriorManager mUserInterfacePortInteriorManager;

		[SerializeField]
		private UIInteriorStoreManager mPrefab_UIInteriorStoreManager;

		[SerializeField]
		private Camera mCamera_SwipeEventCatch;

		private UserInterfaceInteriorManager.StateManager<UserInterfaceInteriorManager.State> mStateManager;

		private InteriorManager mInteriorManager;

		private KeyControl mKeyController;

		public static void Debug_AddFurniture()
		{
			List<int> mst_id = new List<int>();
			mst_id = Enumerable.ToList<int>(Enumerable.Select<Mst_furniture, int>(Enumerable.Where<Mst_furniture>(Mst_DataManager.Instance.Mst_furniture.get_Values(), (Mst_furniture x) => x.Saleflg == 1), (Mst_furniture x) => x.Id));
			new Debug_Mod().AddFurniture(mst_id);
		}

		public static void Debug_AddCoin()
		{
			new Debug_Mod().Add_Coin(80000);
		}

		public static string FurnitureKindToString(FurnitureKinds kind)
		{
			switch (kind)
			{
			case FurnitureKinds.Floor:
				return "床";
			case FurnitureKinds.Wall:
				return "壁紙";
			case FurnitureKinds.Window:
				return "窓枠＋カーテン";
			case FurnitureKinds.Hangings:
				return "装飾";
			case FurnitureKinds.Chest:
				return "家具";
			case FurnitureKinds.Desk:
				return "椅子＋机";
			default:
				return string.Empty;
			}
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UserInterfaceInteriorManager.<Start>c__Iterator8B <Start>c__Iterator8B = new UserInterfaceInteriorManager.<Start>c__Iterator8B();
			<Start>c__Iterator8B.<>f__this = this;
			return <Start>c__Iterator8B;
		}

		private void PreviewFromItemStore(FurnitureKinds furnitureKind, FurnitureModel furnitureModel, Action onFinished)
		{
		}

		private void OnRequestMoveToInterior()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorManager.State.FurnitureStore;
			if (flag)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				this.mStateManager.PopState();
				this.mStateManager.PushState(UserInterfaceInteriorManager.State.MoveToInteriorChange);
			}
		}

		private void OnRequestMoveFurnitureStore()
		{
			bool flag = this.mStateManager.CurrentState == UserInterfaceInteriorManager.State.InteriorChange;
			if (flag)
			{
				SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
				this.mStateManager.PopState();
				this.mStateManager.PushState(UserInterfaceInteriorManager.State.MoveToFurnitureStore);
			}
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
				if (SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable && this.mKeyController.IsRDown())
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToStrategy();
				}
			}
		}

		public void Release()
		{
			this.mUserInterfaceInteriorChangeManager.Release();
			this.mUserInterfaceInteriorTransitionManager.Release();
			this.mUserInterfaceInteriorChangeManager = null;
			this.mUserInterfaceInteriorTransitionManager = null;
		}

		private void OnPopState(UserInterfaceInteriorManager.State state)
		{
			if (state == UserInterfaceInteriorManager.State.InteriorChange)
			{
				this.mUserInterfaceInteriorChangeManager.Clean();
			}
		}

		private void OnPushState(UserInterfaceInteriorManager.State state)
		{
			switch (state)
			{
			case UserInterfaceInteriorManager.State.InteriorChange:
				this.OnPushStateInteriorChange();
				break;
			case UserInterfaceInteriorManager.State.FurnitureStore:
				this.OnPushStateFurnitureStore();
				break;
			case UserInterfaceInteriorManager.State.MoveToFurnitureStore:
			{
				IEnumerator enumerator = this.MoveToFurnitureStoreCoroutine();
				base.StartCoroutine(enumerator);
				break;
			}
			case UserInterfaceInteriorManager.State.MoveToInteriorChange:
			{
				IEnumerator enumerator2 = this.MoveToInteriorChangeCoroutine();
				base.StartCoroutine(enumerator2);
				break;
			}
			}
		}

		private void OnPushStateInteriorChange()
		{
			base.StartCoroutine(this.OnPushStateInteriorChangeCoroutine());
		}

		private void OnPushStateFurnitureStore()
		{
			base.StartCoroutine(this.OnPushStateFurnitureStoreCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushStateFurnitureStoreCoroutine()
		{
			UserInterfaceInteriorManager.<OnPushStateFurnitureStoreCoroutine>c__Iterator8C <OnPushStateFurnitureStoreCoroutine>c__Iterator8C = new UserInterfaceInteriorManager.<OnPushStateFurnitureStoreCoroutine>c__Iterator8C();
			<OnPushStateFurnitureStoreCoroutine>c__Iterator8C.<>f__this = this;
			return <OnPushStateFurnitureStoreCoroutine>c__Iterator8C;
		}

		[DebuggerHidden]
		private IEnumerator OnPushStateInteriorChangeCoroutine()
		{
			UserInterfaceInteriorManager.<OnPushStateInteriorChangeCoroutine>c__Iterator8D <OnPushStateInteriorChangeCoroutine>c__Iterator8D = new UserInterfaceInteriorManager.<OnPushStateInteriorChangeCoroutine>c__Iterator8D();
			<OnPushStateInteriorChangeCoroutine>c__Iterator8D.<>f__this = this;
			return <OnPushStateInteriorChangeCoroutine>c__Iterator8D;
		}

		private void OnDestroy()
		{
			if (SingletonMonoBehaviour<UIPortFrame>.exist())
			{
				SingletonMonoBehaviour<UIPortFrame>.Instance.get_gameObject().SetActive(true);
			}
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Preload, false);
			this.mUserInterfaceInteriorChangeManager = null;
			this.mUserInterfaceInteriorTransitionManager = null;
			this.mUIInteriorStoreManager = null;
			this.mStateManager = null;
			this.mInteriorManager = null;
			this.mKeyController = null;
			this.mUserInterfacePortInteriorManager = null;
		}

		[DebuggerHidden]
		private IEnumerator MoveToFurnitureStoreCoroutine()
		{
			UserInterfaceInteriorManager.<MoveToFurnitureStoreCoroutine>c__Iterator8E <MoveToFurnitureStoreCoroutine>c__Iterator8E = new UserInterfaceInteriorManager.<MoveToFurnitureStoreCoroutine>c__Iterator8E();
			<MoveToFurnitureStoreCoroutine>c__Iterator8E.<>f__this = this;
			return <MoveToFurnitureStoreCoroutine>c__Iterator8E;
		}

		[DebuggerHidden]
		private IEnumerator MoveToInteriorChangeCoroutine()
		{
			UserInterfaceInteriorManager.<MoveToInteriorChangeCoroutine>c__Iterator8F <MoveToInteriorChangeCoroutine>c__Iterator8F = new UserInterfaceInteriorManager.<MoveToInteriorChangeCoroutine>c__Iterator8F();
			<MoveToInteriorChangeCoroutine>c__Iterator8F.<>f__this = this;
			return <MoveToInteriorChangeCoroutine>c__Iterator8F;
		}

		private void OnSwitchState(UserInterfaceInteriorManager.State state)
		{
		}

		private void OnResumeState(UserInterfaceInteriorManager.State state)
		{
		}
	}
}
