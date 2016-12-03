using Common.Enum;
using DG.Tweening;
using KCV.BattleCut;
using KCV.Scene.Port;
using KCV.Strategy;
using local.managers;
using local.models;
using Server_Controllers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace KCV.Scene.Practice
{
	[RequireComponent(typeof(UIPanel))]
	public class UserInterfacePracticeManager : MonoBehaviour
	{
		public static class DebugUtils
		{
			public static void Debug_OpenAllDeckPractice()
			{
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Hou, true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Kouku, true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Normal, true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Rai, true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Sougou, true);
				Debug_Mod.DeckPracticeMenu_StateChange(DeckPracticeType.Taisen, true);
			}
		}

		public static class Util
		{
			public static string IsGoConditionToString(IsGoCondition condition)
			{
				switch (condition)
				{
				case IsGoCondition.Invalid:
					return "戦う相手がいません";
				case IsGoCondition.ActionEndDeck:
					return "行動終了しています";
				case IsGoCondition.Mission:
					return "遠征中です";
				case IsGoCondition.HasRepair:
					return "修復中の艦娘がいます";
				case IsGoCondition.FlagShipTaiha:
					return "旗艦が大破しています";
				case IsGoCondition.NeedSupply:
					return "補給が必要な艦娘がいます";
				case IsGoCondition.ConditionRed:
					return "疲労しています";
				}
				return string.Empty;
			}

			public static string DeckPracticeTypeToString(DeckPracticeType deckPracticeType)
			{
				switch (deckPracticeType)
				{
				case DeckPracticeType.Normal:
					return "艦隊行動";
				case DeckPracticeType.Hou:
					return "砲戦";
				case DeckPracticeType.Rai:
					return "雷撃戦";
				case DeckPracticeType.Taisen:
					return "対潜戦";
				case DeckPracticeType.Kouku:
					return "航空戦";
				case DeckPracticeType.Sougou:
					return "総合";
				default:
					return string.Empty;
				}
			}
		}

		public enum State
		{
			NONE,
			PracticeTypeSelect,
			BattlePractice,
			DeckPractice,
			DeckPracticeProd,
			BattlePracticeProd
		}

		private UIPanel mPanelThis;

		[SerializeField]
		private Texture[] mTextures_Preload;

		[SerializeField]
		private UIPracticeMenu mPracticeMenu;

		[SerializeField]
		private UIPracticeHeader mPracticeHeader;

		[SerializeField]
		private Transform mTransform_PracticeDeckPlayer;

		[SerializeField]
		private BattleCutManager mPrefab_BattleCutManager;

		[SerializeField]
		private UIBattlePracticeManager mUIBattlePracticeManager;

		[SerializeField]
		private UIDeckPracticeManager mUIDeckPracticeManager;

		[SerializeField]
		private UIDeckPracticeProductionManager mUIDeckPracticeProductionManager;

		private KeyControl mKeyController;

		private PracticeManager mPracticeManager;

		private Action mOnBackCallBack;

		private StateManager<UserInterfacePracticeManager.State> mStateManager;

		private void Awake()
		{
			this.mPanelThis = base.GetComponent<UIPanel>();
		}

		private void OnPushState(UserInterfacePracticeManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			switch (state)
			{
			case UserInterfacePracticeManager.State.PracticeTypeSelect:
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnPushStatePracticeTypeSelect();
				break;
			case UserInterfacePracticeManager.State.BattlePractice:
				this.OnPushStateBattlePractice();
				break;
			case UserInterfacePracticeManager.State.DeckPractice:
				this.OnPushStateDeckPractice();
				break;
			}
		}

		private void OnPushStateDeckPractice()
		{
			this.mUIDeckPracticeManager.SetActive(true);
			this.mUIDeckPracticeManager.Release();
			this.mUIDeckPracticeManager.Initialize(this.mPracticeManager);
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIDeckPracticeManager.SetKeyController(this.mKeyController);
			this.mUIDeckPracticeManager.StartState();
			this.mPracticeMenu.MoveToButtonCenterFocus(delegate
			{
			});
		}

		private void OnPushStateBattlePractice()
		{
			this.mUIBattlePracticeManager.SetActive(true);
			this.mUIBattlePracticeManager.Initialize(this.mPracticeManager);
			this.mKeyController.ClearKeyAll();
			this.mKeyController.firstUpdate = true;
			this.mUIBattlePracticeManager.SetKeyController(this.mKeyController);
			this.mUIBattlePracticeManager.StartState();
			this.mPracticeMenu.MoveToButtonCenterFocus(delegate
			{
			});
		}

		private void OnPopState(UserInterfacePracticeManager.State state)
		{
			if (state == UserInterfacePracticeManager.State.PracticeTypeSelect)
			{
				this.OnPopStatePracticeTypeSelect();
			}
		}

		private void OnResumeState(UserInterfacePracticeManager.State state)
		{
			SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = false;
			if (state == UserInterfacePracticeManager.State.PracticeTypeSelect)
			{
				SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable = true;
				this.OnResumeStatePracticeTypeSelect();
			}
		}

		public void SetOnBackCallBack(Action action)
		{
			this.mOnBackCallBack = action;
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			UserInterfacePracticeManager.<Start>c__Iterator156 <Start>c__Iterator = new UserInterfacePracticeManager.<Start>c__Iterator156();
			<Start>c__Iterator.<>f__this = this;
			return <Start>c__Iterator;
		}

		private void Update()
		{
			if (this.mKeyController != null)
			{
				this.mKeyController.Update();
				if (SingletonMonoBehaviour<UIShortCutMenu>.Instance != null && SingletonMonoBehaviour<UIShortCutMenu>.Instance.IsInputEnable && this.mKeyController.IsRDown())
				{
					this.mKeyController.ClearKeyAll();
					this.mKeyController.firstUpdate = true;
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
					this.mKeyController = null;
				}
			}
		}

		private void BackFromBattlePracticeSetting()
		{
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void BackFromDeckPracticeSetting()
		{
			this.mStateManager.PopState();
			this.mStateManager.ResumeState();
		}

		private void OnDeckPracticeManagerOnChangedStateListener(UIDeckPracticeManager.State state)
		{
			if (state == UIDeckPracticeManager.State.DeckPracticeTypeSelect)
			{
				this.mPracticeHeader.UpdateHeaderText("どの演習を行いますか？");
			}
		}

		private void OnBattlePracticeManagerOnChangedStateListener(UIBattlePracticeManager.State state)
		{
			switch (state)
			{
			case UIBattlePracticeManager.State.BattlePracticeTargetSelect:
				this.mPracticeHeader.UpdateHeaderText("演習相手を選んでください");
				break;
			case UIBattlePracticeManager.State.FormationSelect:
				this.mPracticeHeader.UpdateHeaderText("陣形を選んでください");
				break;
			case UIBattlePracticeManager.State.BattlePracticeProd:
				ShortcutExtensions.DOKill(this.mPracticeHeader.get_transform(), false);
				ShortcutExtensions.DOLocalMoveY(this.mPracticeHeader.get_transform(), 320f, 0.3f, false);
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetConfirm:
				this.mPracticeHeader.UpdateHeaderText(string.Empty);
				break;
			case UIBattlePracticeManager.State.BattlePracticeTargetAlertConfirm:
				this.mPracticeHeader.UpdateHeaderText(string.Empty);
				break;
			}
		}

		private void OnSelectedPracticeMenu(UIPracticeMenu.SelectType selectType)
		{
			bool flag = this.mStateManager.CurrentState == UserInterfacePracticeManager.State.PracticeTypeSelect;
			if (flag)
			{
				switch (selectType)
				{
				case UIPracticeMenu.SelectType.Back:
					this.mStateManager.PopState();
					break;
				case UIPracticeMenu.SelectType.DeckPractice:
					if (this.mPracticeManager.IsValidDeckPractice())
					{
						this.mPracticeMenu.SetKeyController(null);
						this.mStateManager.PushState(UserInterfacePracticeManager.State.DeckPractice);
					}
					else
					{
						DeckModel currentDeck = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
						List<IsGoCondition> list = this.mPracticeManager.IsValidPractice(currentDeck.Id);
						string mes = UserInterfacePracticeManager.Util.IsGoConditionToString(list.get_Item(0));
						CommonPopupDialog.Instance.StartPopup(mes);
					}
					break;
				case UIPracticeMenu.SelectType.BattlePractice:
					if (this.mPracticeManager.IsValidBattlePractice())
					{
						this.mPracticeMenu.SetKeyController(null);
						this.mStateManager.PushState(UserInterfacePracticeManager.State.BattlePractice);
					}
					else
					{
						DeckModel currentDeck2 = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck;
						List<IsGoCondition> list2 = this.mPracticeManager.IsValidPractice(currentDeck2.Id);
						if (0 < Enumerable.Count<IsGoCondition>(list2))
						{
							string mes2 = UserInterfacePracticeManager.Util.IsGoConditionToString(list2.get_Item(0));
							CommonPopupDialog.Instance.StartPopup(mes2);
						}
						else
						{
							string mes3 = "演習可能な艦隊がありません";
							CommonPopupDialog.Instance.StartPopup(mes3);
						}
					}
					break;
				}
			}
		}

		private void OnResumeStatePracticeTypeSelect()
		{
			this.mPracticeHeader.UpdateHeaderText("演習選択");
			this.mPracticeMenu.MoveToButtonDefaultFocus(delegate
			{
				this.mKeyController.ClearKeyAll();
				this.mKeyController.firstUpdate = true;
				this.mPracticeMenu.SetKeyController(this.mKeyController);
			});
		}

		private void OnPushStatePracticeTypeSelect()
		{
			base.StartCoroutine(this.OnPushStatePracticeTypeSelectCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnPushStatePracticeTypeSelectCoroutine()
		{
			UserInterfacePracticeManager.<OnPushStatePracticeTypeSelectCoroutine>c__Iterator157 <OnPushStatePracticeTypeSelectCoroutine>c__Iterator = new UserInterfacePracticeManager.<OnPushStatePracticeTypeSelectCoroutine>c__Iterator157();
			<OnPushStatePracticeTypeSelectCoroutine>c__Iterator.<>f__this = this;
			return <OnPushStatePracticeTypeSelectCoroutine>c__Iterator;
		}

		private void OnPopStatePracticeTypeSelect()
		{
			this.mPracticeMenu.SetKeyController(null);
			TweenAlpha.Begin(base.get_gameObject(), 0.2f, 0f);
			StrategyTaskManager.SceneCallBack();
		}

		private void OnCommitBattleStart(BattlePracticeContext context)
		{
			base.StartCoroutine(this.OnCommitBattleStartCoroutine(context));
		}

		[DebuggerHidden]
		private IEnumerator OnCommitBattleStartCoroutine(BattlePracticeContext context)
		{
			UserInterfacePracticeManager.<OnCommitBattleStartCoroutine>c__Iterator158 <OnCommitBattleStartCoroutine>c__Iterator = new UserInterfacePracticeManager.<OnCommitBattleStartCoroutine>c__Iterator158();
			<OnCommitBattleStartCoroutine>c__Iterator.context = context;
			<OnCommitBattleStartCoroutine>c__Iterator.<$>context = context;
			<OnCommitBattleStartCoroutine>c__Iterator.<>f__this = this;
			return <OnCommitBattleStartCoroutine>c__Iterator;
		}

		private void OnDeckPracticeProductionStateChangeListener(UIDeckPracticeProductionManager.State state)
		{
			if (state == UIDeckPracticeProductionManager.State.EndOfPractice)
			{
				int width = StrategyTopTaskManager.Instance.UIModel.Character.GetWidth();
				int num = 960;
				TweenSettingsExtensions.SetEase<Tweener>(ShortcutExtensions.DOLocalMoveX(StrategyTopTaskManager.Instance.UIModel.Character.get_transform(), (float)num, 0.5f, false), 8);
				DeckPracticeContext context = this.mUIDeckPracticeManager.GetContext();
				string text = string.Empty;
				text = UserInterfacePracticeManager.Util.DeckPracticeTypeToString(context.PracticeType) + "演習-結果";
				this.mPracticeHeader.UpdateHeaderText(text);
			}
		}

		private void OnCommitDeckPracticeStart(DeckPracticeContext context)
		{
			IEnumerator enumerator = this.OnCommitDeckPracticeStartCoroutine(context);
			base.StartCoroutine(enumerator);
		}

		[DebuggerHidden]
		private IEnumerator OnCommitDeckPracticeStartCoroutine(DeckPracticeContext context)
		{
			UserInterfacePracticeManager.<OnCommitDeckPracticeStartCoroutine>c__Iterator159 <OnCommitDeckPracticeStartCoroutine>c__Iterator = new UserInterfacePracticeManager.<OnCommitDeckPracticeStartCoroutine>c__Iterator159();
			<OnCommitDeckPracticeStartCoroutine>c__Iterator.context = context;
			<OnCommitDeckPracticeStartCoroutine>c__Iterator.<$>context = context;
			<OnCommitDeckPracticeStartCoroutine>c__Iterator.<>f__this = this;
			return <OnCommitDeckPracticeStartCoroutine>c__Iterator;
		}

		private void OnFinishedPlayVoiceAndLive2DMotion()
		{
			base.StartCoroutine(this.OnFinishedPlayVoiceAndLive2DMotionCoroutine());
		}

		[DebuggerHidden]
		private IEnumerator OnFinishedPlayVoiceAndLive2DMotionCoroutine()
		{
			UserInterfacePracticeManager.<OnFinishedPlayVoiceAndLive2DMotionCoroutine>c__Iterator15A <OnFinishedPlayVoiceAndLive2DMotionCoroutine>c__Iterator15A = new UserInterfacePracticeManager.<OnFinishedPlayVoiceAndLive2DMotionCoroutine>c__Iterator15A();
			<OnFinishedPlayVoiceAndLive2DMotionCoroutine>c__Iterator15A.<>f__this = this;
			return <OnFinishedPlayVoiceAndLive2DMotionCoroutine>c__Iterator15A;
		}

		[DebuggerHidden]
		private IEnumerator GeneratePlayVoiceAndLive2DMotionCoroutine(ShipModelMst shipModelMst, int voiceId, Action onFinished)
		{
			UserInterfacePracticeManager.<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B <GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B = new UserInterfacePracticeManager.<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B();
			<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B.shipModelMst = shipModelMst;
			<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B.voiceId = voiceId;
			<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B.onFinished = onFinished;
			<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B.<$>shipModelMst = shipModelMst;
			<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B.<$>voiceId = voiceId;
			<GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B.<$>onFinished = onFinished;
			return <GeneratePlayVoiceAndLive2DMotionCoroutine>c__Iterator15B;
		}

		private void OnFinishedDeckPracticeStartProduction()
		{
			Application.LoadLevel(Generics.Scene.Strategy.ToString());
		}

		private void OnDestroy()
		{
			UserInterfacePortManager.ReleaseUtils.Releases(ref this.mTextures_Preload, false);
			UserInterfacePortManager.ReleaseUtils.Release(ref this.mPanelThis);
			this.mPracticeMenu = null;
			this.mPracticeHeader = null;
			this.mTransform_PracticeDeckPlayer = null;
			this.mPrefab_BattleCutManager = null;
			this.mUIBattlePracticeManager = null;
			this.mUIDeckPracticeManager = null;
			this.mUIDeckPracticeProductionManager = null;
			this.mKeyController = null;
			this.mPracticeManager = null;
			this.mOnBackCallBack = null;
		}
	}
}
