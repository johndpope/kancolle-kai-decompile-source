using Common.Enum;
using KCV.Battle.Formation;
using KCV.Scene.Port;
using KCV.Strategy;
using local.managers;
using local.models;
using local.utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIWidget))]
	public class UIBattlePracticeManager : MonoBehaviour
	{
		public enum State
		{
			NONE,
			BattlePracticeTargetSelect,
			FormationSelect,
			BattlePracticeProd,
			BattlePracticeTargetConfirm,
			BattlePracticeTargetAlertConfirm
		}

		private StateManager<UIBattlePracticeManager.State> mStateManager;

		private UIWidget mWidgetThis;

		[SerializeField]
		private UIPracticeBattleList mPracticeBattleTargetSelect;

		[SerializeField]
		private UIPracticeBattleStartProduction mUIPracticeBattleStartProduction;

		[SerializeField]
		private UIPracticeBattleConfirm mPracticeBattleConfirm;

		[SerializeField]
		private CommonDialog mCommonDialog_Dialog;

		[SerializeField]
		private UIBattleFormationKindSelectManager mPrefab_UIBattleFormationKindSelectManager;

		[SerializeField]
		private UIPracticeMenu mPracticeMenu;

		private Camera mCamera_CatchTouchEvent;

		private PracticeManager mPracticeManager;

		private KeyControl mKeyController;

		private Action mOnBackListener;

		private BattlePracticeContext mBattlePracticeContext;

		private Action<UIBattlePracticeManager.State> mOnChangedStateListener;

		private Action<BattlePracticeContext> mOnCommitBattleListener;

		private void OnSwitchState(UIBattlePracticeManager.State changedState)
		{
			if (this.mOnChangedStateListener != null)
			{
				this.mOnChangedStateListener.Invoke(changedState);
			}
		}

		private void Awake()
		{
			this.mWidgetThis = base.GetComponent<UIWidget>();
		}

		private void Start()
		{
			try
			{
				this.mCamera_CatchTouchEvent = StrategyTaskManager.GetOverViewCamera();
			}
			catch (Exception)
			{
				Debug.LogError("Strategy's StaticMethod Called Exception ::[" + StackTraceUtility.ExtractStackTrace() + "]");
			}
			this.mPracticeBattleTargetSelect.SetOnBackCallBack(new Action(this.OnCancelBattleTargetSelect));
			this.mPracticeBattleTargetSelect.SetOnSelectedDeckListener(new Action<DeckModel, List<IsGoCondition>>(this.OnSelectedBattleTargetDeck));
			this.mPracticeBattleTargetSelect.SetActive(false);
			this.mUIPracticeBattleStartProduction.SetOnAnimationFinishedListener(new Action<bool>(this.OnPracticeBattleStartProductionOnFinished));
			this.mUIPracticeBattleStartProduction.SetActive(false);
			this.mPracticeBattleConfirm.SetOnCancelListener(new Action(this.OnCancelSelectedPracticeBattleConfirm));
			this.mPracticeBattleConfirm.SetOnStartListener(new Action(this.OnStartSelectedPracticeBattleConfirm));
			this.mPracticeBattleConfirm.SetActive(false);
		}

		public void SetKeyController(KeyControl keyController)
		{
			this.mKeyController = keyController;
		}

		public void SetOnBackListener(Action onBackListener)
		{
			this.mOnBackListener = onBackListener;
		}

		public void Initialize(PracticeManager practiceManager)
		{
			this.mStateManager = new StateManager<UIBattlePracticeManager.State>(UIBattlePracticeManager.State.NONE);
			base.get_transform().set_localScale(Vector3.get_one());
			this.mStateManager.OnPop = new Action<UIBattlePracticeManager.State>(this.OnPopState);
			this.mStateManager.OnPush = new Action<UIBattlePracticeManager.State>(this.OnPushState);
			this.mStateManager.OnResume = new Action<UIBattlePracticeManager.State>(this.OnResumeState);
			this.mStateManager.OnSwitch = new Action<UIBattlePracticeManager.State>(this.OnSwitchState);
			this.mPracticeManager = practiceManager;
			this.mBattlePracticeContext = new BattlePracticeContext();
		}

		public void SetOnChangedStateListener(Action<UIBattlePracticeManager.State> onChangedStateListener)
		{
			this.mOnChangedStateListener = onChangedStateListener;
		}

		public void StartState()
		{
			this.mStateManager.PushState(UIBattlePracticeManager.State.BattlePracticeTargetSelect);
			this.mPracticeBattleTargetSelect.Show(delegate
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.mPracticeBattleTargetSelect.SetKeyController(this.mKeyController);
			});
		}

		private void OnResumeStateBattlePracticeTargetSelect()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mPracticeBattleTargetSelect.SetKeyController(this.mKeyController);
		}

		private void OnPushStateBattlePracticeTargetConfirm()
		{
			base.StartCoroutine(this.OnPushStateBattlePracticeTargetConfirmCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushStateBattlePracticeTargetConfirmCoroutine()
		{
			UIBattlePracticeManager.<OnPushStateBattlePracticeTargetConfirmCoroutine>c__Iterator1C2 <OnPushStateBattlePracticeTargetConfirmCoroutine>c__Iterator1C = new UIBattlePracticeManager.<OnPushStateBattlePracticeTargetConfirmCoroutine>c__Iterator1C2();
			<OnPushStateBattlePracticeTargetConfirmCoroutine>c__Iterator1C.<>f__this = this;
			return <OnPushStateBattlePracticeTargetConfirmCoroutine>c__Iterator1C;
		}

		private void OnPushStateBattlePracticeTargetSelect()
		{
			this.mPracticeBattleTargetSelect.get_transform().SetActive(true);
			this.mBattlePracticeContext.SetFriendDeck(this.mPracticeManager.CurrentDeck);
			List<DeckModel> rivalDecks = this.mPracticeManager.RivalDecks;
			this.mPracticeBattleTargetSelect.Initialize(rivalDecks, this.mPracticeManager);
		}

		private void OnPushStateBattlePracticeTargetAlertConfirm()
		{
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mCommonDialog_Dialog.SetActive(true);
			this.mCommonDialog_Dialog.setCloseAction(new Action(this.OnClosePracticeBattleAlert));
			this.mCommonDialog_Dialog.OpenDialog(0, DialogAnimation.AnimType.POPUP);
		}

		private void OnPushStateBattlePracticeProd()
		{
			if (SingletonMonoBehaviour<Live2DModel>.exist())
			{
				SingletonMonoBehaviour<Live2DModel>.Instance.Disable();
			}
			this.mUIPracticeBattleStartProduction.SetActive(true);
			this.mUIPracticeBattleStartProduction.Initialize(this.mBattlePracticeContext.FriendDeck, this.mBattlePracticeContext.TargetDeck);
			this.mUIPracticeBattleStartProduction.SetKeyController(this.mKeyController);
			this.mUIPracticeBattleStartProduction.Play();
		}

		private void OnPushStateFormationSelect()
		{
			HashSet<BattleFormationKinds1> selectableFormations = DeckUtil.GetSelectableFormations(this.mBattlePracticeContext.FriendDeck);
			BattleFormationKinds1[] array = Enumerable.ToArray<BattleFormationKinds1>(Enumerable.Where<BattleFormationKinds1>(selectableFormations, (BattleFormationKinds1 w) => true));
			this.mPracticeBattleTargetSelect.SetActive(false);
			this.mPracticeMenu.SetActive(false);
			if (1 < Enumerable.Count<BattleFormationKinds1>(array))
			{
				GameObject gameObject = GameObject.Find("Live2DRender");
				if (gameObject != null)
				{
					UIPanel component = gameObject.GetComponent<UIPanel>();
					if (component != null)
					{
						component.depth = 6;
					}
				}
				UIBattleFormationKindSelectManager battleFormationKindSelectManager = Util.Instantiate(this.mPrefab_UIBattleFormationKindSelectManager.get_gameObject(), base.get_transform().get_gameObject(), false, false).GetComponent<UIBattleFormationKindSelectManager>();
				battleFormationKindSelectManager.Initialize(this.mCamera_CatchTouchEvent, array, true);
				battleFormationKindSelectManager.SetOnUIBattleFormationKindSelectManagerAction(delegate(UIBattleFormationKindSelectManager.ActionType actionType, UIBattleFormationKindSelectManager calledObject, UIBattleFormationKind centerView)
				{
					int width2 = StrategyTopTaskManager.Instance.UIModel.Character.GetWidth();
					int num2 = -960 - Mathf.Abs(width2) / 2;
					StrategyTopTaskManager.Instance.UIModel.Character.moveAddCharacterX((float)num2, 1f, null);
					BattleFormationKinds1 category = centerView.Category;
					this.mBattlePracticeContext.SetFormationType(category);
					this.mStateManager.PopState();
					this.mStateManager.PushState(UIBattlePracticeManager.State.BattlePracticeProd);
					Object.Destroy(battleFormationKindSelectManager.get_gameObject());
				});
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				battleFormationKindSelectManager.SetKeyController(this.mKeyController);
			}
			else
			{
				int width = StrategyTopTaskManager.Instance.UIModel.Character.GetWidth();
				int num = -960 - Mathf.Abs(width) / 2;
				StrategyTopTaskManager.Instance.UIModel.Character.moveAddCharacterX((float)num, 1f, null);
				this.mBattlePracticeContext.SetFormationType(BattleFormationKinds1.TanJuu);
				this.mStateManager.PopState();
				this.mStateManager.PushState(UIBattlePracticeManager.State.BattlePracticeProd);
			}
		}

		private void OnPopStateBattlePracticeTargetConfirm()
		{
			this.mPracticeBattleConfirm.SetKeyController(null);
			this.mKeyController.IsRun = false;
			this.mPracticeBattleConfirm.Hide(delegate
			{
				this.mKeyController.IsRun = true;
				this.mPracticeBattleConfirm.SetActive(false);
			});
		}

		private void OnPopStateBattlePracticeTargetAlertConfirm()
		{
			this.mCommonDialog_Dialog.SetActive(false);
		}

		private void OnPopStateBattlePracticeTargetSelect()
		{
			this.mPracticeBattleTargetSelect.SetKeyController(null);
			this.Back();
		}

		private void OnResumeState(UIBattlePracticeManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (state != UIBattlePracticeManager.State.BattlePracticeTargetSelect)
			{
				if (state == UIBattlePracticeManager.State.BattlePracticeTargetAlertConfirm)
				{
					SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				}
			}
			else
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnResumeStateBattlePracticeTargetSelect();
			}
		}

		private void OnBack()
		{
			if (this.mOnBackListener != null)
			{
				this.mOnBackListener.Invoke();
			}
		}

		private void OnCancelBattleTargetSelect()
		{
			bool flag = this.mStateManager.CurrentState == UIBattlePracticeManager.State.BattlePracticeTargetSelect;
			if (flag)
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void OnPopState(UIBattlePracticeManager.State state)
		{
			switch (state)
			{
			case UIBattlePracticeManager.State.BattlePracticeTargetSelect:
				this.OnPopStateBattlePracticeTargetSelect();
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetConfirm:
				this.OnPopStateBattlePracticeTargetConfirm();
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetAlertConfirm:
				this.OnPopStateBattlePracticeTargetAlertConfirm();
				break;
			}
		}

		private void OnCancelSelectedPracticeBattleConfirm()
		{
			bool flag = this.mStateManager.CurrentState == UIBattlePracticeManager.State.BattlePracticeTargetConfirm;
			if (flag)
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void OnStartSelectedPracticeBattleConfirm()
		{
			bool flag = this.mStateManager.CurrentState == UIBattlePracticeManager.State.BattlePracticeTargetConfirm;
			if (flag)
			{
				this.mPracticeBattleConfirm.SetKeyController(null);
				this.mStateManager.PopState();
				this.mStateManager.PushState(UIBattlePracticeManager.State.FormationSelect);
			}
		}

		private void OnPushState(UIBattlePracticeManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case UIBattlePracticeManager.State.BattlePracticeTargetSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnPushStateBattlePracticeTargetSelect();
				break;
			case UIBattlePracticeManager.State.FormationSelect:
				this.OnPushStateFormationSelect();
				break;
			case UIBattlePracticeManager.State.BattlePracticeProd:
				this.OnPushStateBattlePracticeProd();
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetConfirm:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnPushStateBattlePracticeTargetConfirm();
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetAlertConfirm:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnPushStateBattlePracticeTargetAlertConfirm();
				break;
			}
		}

		private void OnClosePracticeBattleAlert()
		{
			bool flag = this.mStateManager.CurrentState == UIBattlePracticeManager.State.BattlePracticeTargetAlertConfirm;
			if (flag)
			{
				this.mStateManager.PopState();
				this.mStateManager.ResumeState();
			}
		}

		private void OnPracticeBattleStartProductionOnFinished(bool isShortCutBattleStart)
		{
			this.mUIPracticeBattleStartProduction.SetKeyController(null);
			if (isShortCutBattleStart)
			{
				this.mUIPracticeBattleStartProduction.ShowCover();
				this.mBattlePracticeContext.SetBattleType(BattlePracticeContext.PlayType.ShortCutBattle);
			}
			else
			{
				this.mBattlePracticeContext.SetBattleType(BattlePracticeContext.PlayType.Battle);
			}
			this.OnCommitBattleStart(this.mBattlePracticeContext);
		}

		private void OnCommitBattleStart(BattlePracticeContext context)
		{
			if (this.mOnCommitBattleListener != null)
			{
				this.mOnCommitBattleListener.Invoke(context);
			}
		}

		private void OnSelectedBattleTargetDeck(DeckModel selectedDeck, List<IsGoCondition> conditions)
		{
			bool flag = this.mStateManager.CurrentState == UIBattlePracticeManager.State.BattlePracticeTargetSelect;
			if (flag)
			{
				bool flag2 = 0 == conditions.get_Count();
				if (flag2)
				{
					this.mPracticeBattleTargetSelect.SetKeyController(null);
					this.mBattlePracticeContext.SetTargetDeck(selectedDeck);
					this.mBattlePracticeContext.SetConditions(conditions);
					this.mStateManager.PushState(UIBattlePracticeManager.State.BattlePracticeTargetConfirm);
				}
				else
				{
					CommonPopupDialog.Instance.StartPopup(UserInterfacePracticeManager.Util.IsGoConditionToString(conditions.get_Item(0)), 0, CommonPopupDialogMessage.PlayType.Long);
				}
			}
		}

		private void Back()
		{
			this.mPracticeBattleTargetSelect.Hide(new Action(this.OnBack));
		}

		public void SetOnCommitBattleListener(Action<BattlePracticeContext> onCommitBattleStart)
		{
			this.mOnCommitBattleListener = onCommitBattleStart;
		}

		private void OnDestroy()
		{
			this.mStateManager = null;
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mWidgetThis);
			this.mPracticeBattleTargetSelect = null;
			this.mUIPracticeBattleStartProduction = null;
			this.mPracticeBattleConfirm = null;
			this.mCommonDialog_Dialog = null;
			this.mPrefab_UIBattleFormationKindSelectManager = null;
			this.mPracticeMenu = null;
			this.mCamera_CatchTouchEvent = null;
			this.mPracticeManager = null;
			this.mKeyController = null;
			this.mOnBackListener = null;
			this.mBattlePracticeContext = null;
			this.mOnChangedStateListener = null;
			this.mOnCommitBattleListener = null;
		}

		public override string ToString()
		{
			return (this.mStateManager == null) ? string.Empty : this.mStateManager.ToString();
		}
	}
}
