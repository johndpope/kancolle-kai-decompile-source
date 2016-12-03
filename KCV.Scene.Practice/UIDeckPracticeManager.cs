using Common.Enum;
using local.managers;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Scene.Practice
{
	public class UIDeckPracticeManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			DeckPracticeTypeSelect,
			Production
		}

		[SerializeField]
		private UIPracticeDeckTypeSelect mDeckPracticeTypeSelect;

		private StateManager<UIDeckPracticeManager.State> mStateManager;

		private PracticeManager mPracticeManager;

		private KeyControl mKeyController;

		private Action mOnBackListener;

		private DeckPracticeContext mDeckPracticeContext;

		private Action<UIDeckPracticeManager.State> mOnChangedStateListener;

		private Action<DeckPracticeContext> mOnCommitDeckPracticeListener;

		private void Start()
		{
			this.mDeckPracticeTypeSelect.SetActive(false);
			this.mDeckPracticeTypeSelect.SetOnBackCallBack(new Action(this.OnCancelDeckPracticeTypeSelect));
			this.mDeckPracticeTypeSelect.SetOnSelectedDeckPracticeTypeCallBack(new Action<DeckPracticeType>(this.OnSelectedDeckPracticeType));
		}

		public void Release()
		{
			this.mPracticeManager = null;
			this.mDeckPracticeContext = null;
			this.mStateManager = null;
		}

		public void Initialize(PracticeManager practiceManager)
		{
			this.mStateManager = new StateManager<UIDeckPracticeManager.State>(UIDeckPracticeManager.State.NONE);
			this.mStateManager.OnPush = new Action<UIDeckPracticeManager.State>(this.OnPushState);
			this.mStateManager.OnPop = new Action<UIDeckPracticeManager.State>(this.OnPopState);
			this.mStateManager.OnResume = new Action<UIDeckPracticeManager.State>(this.OnResumeState);
			this.mStateManager.OnSwitch = new Action<UIDeckPracticeManager.State>(this.OnChangedState);
			this.mPracticeManager = practiceManager;
			this.mDeckPracticeContext = new DeckPracticeContext();
			this.mDeckPracticeContext.SetFriendDeck(this.mPracticeManager.CurrentDeck);
		}

		public void StartState()
		{
			this.mStateManager.PushState(UIDeckPracticeManager.State.DeckPracticeTypeSelect);
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void SetOnBackListener(Action onBackListener)
		{
			this.mOnBackListener = onBackListener;
		}

		private void Back()
		{
			this.mDeckPracticeTypeSelect.Hide(new Action(this.OnBack));
		}

		private void OnBack()
		{
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
		}

		public void SetOnChangedStateListener(Action<UIDeckPracticeManager.State> onChangedStateListener)
		{
			this.mOnChangedStateListener = onChangedStateListener;
		}

		private void OnChangedState(UIDeckPracticeManager.State changedState)
		{
			if (this.mOnChangedStateListener != null)
			{
				this.mOnChangedStateListener.Invoke(changedState);
			}
		}

		private void OnPushStateDeckPracticeTypeSelect()
		{
			Dictionary<DeckPracticeType, bool> deckPracticeTypeDic = this.mPracticeManager.DeckPracticeTypeDic;
			this.mDeckPracticeTypeSelect.SetActive(true);
			this.mDeckPracticeTypeSelect.Initialize(deckPracticeTypeDic);
			this.mDeckPracticeTypeSelect.Show(null);
			this.mDeckPracticeTypeSelect.SetKeyController(this.mKeyController);
		}

		private void OnPopStateDeckPracticeTypeSelect()
		{
			this.mDeckPracticeTypeSelect.SetKeyController(null);
			this.Back();
		}

		private void OnPushState(UIDeckPracticeManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (state != UIDeckPracticeManager.State.DeckPracticeTypeSelect)
			{
				if (state == UIDeckPracticeManager.State.Production)
				{
					this.OnPushStateProduction();
				}
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnPushStateDeckPracticeTypeSelect();
			}
		}

		private void OnPushStateProduction()
		{
			this.mDeckPracticeTypeSelect.DisableButtonAll();
		}

		private void OnPopState(UIDeckPracticeManager.State state)
		{
			if (state == UIDeckPracticeManager.State.DeckPracticeTypeSelect)
			{
				this.OnPopStateDeckPracticeTypeSelect();
			}
		}

		private void OnCancelDeckPracticeTypeSelect()
		{
			bool flag = this.mStateManager.CurrentState == UIDeckPracticeManager.State.DeckPracticeTypeSelect;
			if (flag)
			{
				this.mStateManager.PopState();
			}
		}

		public DeckPracticeContext GetContext()
		{
			return this.mDeckPracticeContext;
		}

		private void OnSelectedDeckPracticeType(DeckPracticeType selectedType)
		{
			bool flag = this.mStateManager.CurrentState == UIDeckPracticeManager.State.DeckPracticeTypeSelect;
			if (flag)
			{
				bool flag2 = this.mPracticeManager.DeckPracticeTypeDic.get_Item(selectedType);
				if (flag2)
				{
					this.mDeckPracticeContext.SetPracticeType(selectedType);
					this.mDeckPracticeTypeSelect.SetKeyController(null);
					this.OnCommitDeckPractice(this.mDeckPracticeContext);
					this.mStateManager.PushState(UIDeckPracticeManager.State.Production);
				}
				else
				{
					string mes = UserInterfacePracticeManager.Util.DeckPracticeTypeToString(selectedType) + "に参加できる編成ではありません";
					CommonPopupDialog.Instance.StartPopup(mes, 0, CommonPopupDialogMessage.PlayType.Long);
				}
			}
		}

		public void SetOnCommitDeckPracticeListener(Action<DeckPracticeContext> onCommitDeckPracticeListener)
		{
			this.mOnCommitDeckPracticeListener = onCommitDeckPracticeListener;
		}

		private void OnCommitDeckPractice(DeckPracticeContext context)
		{
			if (this.mOnCommitDeckPracticeListener != null)
			{
				this.mOnCommitDeckPracticeListener.Invoke(context);
			}
		}

		public override string ToString()
		{
			return (this.mStateManager == null) ? string.Empty : this.mStateManager.ToString();
		}

		private void OnResumeState(UIDeckPracticeManager.State state)
		{
		}

		private void OnDestroy()
		{
			this.mDeckPracticeTypeSelect = null;
			this.mStateManager = null;
			this.mPracticeManager = null;
			this.mKeyController = null;
			this.mOnBackListener = null;
			this.mDeckPracticeContext = null;
		}
	}
}
