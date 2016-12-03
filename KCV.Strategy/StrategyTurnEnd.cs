using KCV.Scene.Strategy;
using KCV.Strategy.Rebellion;
using KCV.Utils;
using local.managers;
using local.models;
using Server_Common;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy
{
	public class StrategyTurnEnd : SceneTaskMono
	{
		[SerializeField]
		private DayAnimation dayAnimation;

		[SerializeField]
		private GameObject ReturnMissionAnim;

		[SerializeField]
		private UserInterfaceStrategyResult mPrefab_UserInterfaceStrategyResult;

		private StrategyMapManager LogicMng;

		private EnemyActionPhaseResultModel enemyResult;

		private TurnResultModel TurnResult;

		private UserPreActionPhaseResultModel userPreAction;

		public bool isRebellion;

		private bool finished;

		public bool TurnEndFinish;

		private bool isDebug;

		private void Awake()
		{
			this.TurnEndFinish = true;
		}

		public void TurnEnd()
		{
			if (Utils.IsTurnOver())
			{
				StrategyTopTaskManager.Instance.GameOver();
				return;
			}
			TutorialModel tutorial = StrategyTopTaskManager.GetLogicManager().UserInfo.Tutorial;
			if (tutorial.GetStep() == 7 && !tutorial.GetStepTutorialFlg(8))
			{
				tutorial.SetStepTutorialFlg(8);
				CommonPopupDialog.Instance.StartPopup("「ターン終了」 達成");
				SoundUtils.PlaySE(SEFIleInfos.SE_012);
			}
			this.TurnEndFinish = false;
			this.isRebellion = false;
			StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.3f);
			base.StartCoroutine(this.TurnEndCoroutine());
			StrategyTopTaskManager.Instance.UIModel.Character.ResetTouchCount();
		}

		public void DebugTurnEnd()
		{
			this.TurnEndFinish = true;
			this.isDebug = true;
			this.isRebellion = false;
			StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			StrategyTopTaskManager.GetLogicManager().GetResult_UserActionPhase();
			StrategyTopTaskManager.GetLogicManager().GetResult_EnemyPreActionPhase();
			EnemyActionPhaseResultModel result_EnemyActionPhase = StrategyTopTaskManager.GetLogicManager().GetResult_EnemyActionPhase();
			this.TurnResult = StrategyTopTaskManager.GetLogicManager().GetResult_Turn();
			StrategyTopTaskManager.GetLogicManager().GetResult_UserPreActionPhase();
		}

		public void DebugTurnEndAuto()
		{
			this.TurnEndFinish = true;
			this.isDebug = true;
			this.isRebellion = false;
			StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId);
			StrategyTopTaskManager.GetLogicManager().GetResult_UserActionPhase();
			StrategyTopTaskManager.GetLogicManager().GetResult_EnemyPreActionPhase();
			EnemyActionPhaseResultModel result_EnemyActionPhase = StrategyTopTaskManager.GetLogicManager().GetResult_EnemyActionPhase();
			this.TurnResult = StrategyTopTaskManager.GetLogicManager().GetResult_Turn();
			StrategyTopTaskManager.GetLogicManager().GetResult_UserPreActionPhase();
		}

		[DebuggerHidden]
		public IEnumerator TurnEndCoroutine()
		{
			StrategyTurnEnd.<TurnEndCoroutine>c__Iterator185 <TurnEndCoroutine>c__Iterator = new StrategyTurnEnd.<TurnEndCoroutine>c__Iterator185();
			<TurnEndCoroutine>c__Iterator.<>f__this = this;
			return <TurnEndCoroutine>c__Iterator;
		}

		private void EnemyResult(EnemyActionPhaseResultModel enemyResult)
		{
			if (StrategyRebellionTaskManager.RebellionForceDebug)
			{
				this.finished = true;
				return;
			}
			if (StrategyTopTaskManager.GetLogicManager().GetRebellionAreaList().get_Count() != 0)
			{
				this.isRebellion = true;
				StrategyRebellionTaskManager.checkRebellionArea();
			}
		}

		[DebuggerHidden]
		private IEnumerator UserPreAction()
		{
			StrategyTurnEnd.<UserPreAction>c__Iterator186 <UserPreAction>c__Iterator = new StrategyTurnEnd.<UserPreAction>c__Iterator186();
			<UserPreAction>c__Iterator.<>f__this = this;
			return <UserPreAction>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator RebellionTutorialGuide(TutorialModel model)
		{
			StrategyTurnEnd.<RebellionTutorialGuide>c__Iterator187 <RebellionTutorialGuide>c__Iterator = new StrategyTurnEnd.<RebellionTutorialGuide>c__Iterator187();
			<RebellionTutorialGuide>c__Iterator.model = model;
			<RebellionTutorialGuide>c__Iterator.<$>model = model;
			return <RebellionTutorialGuide>c__Iterator;
		}

		private void OnDestroy()
		{
			this.ReturnMissionAnim = null;
			this.mPrefab_UserInterfaceStrategyResult = null;
			this.dayAnimation = null;
			this.ReturnMissionAnim = null;
			this.LogicMng = null;
			this.enemyResult = null;
			this.TurnResult = null;
			this.userPreAction = null;
		}
	}
}
