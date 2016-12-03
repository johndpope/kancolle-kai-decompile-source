using Common.Enum;
using KCV.Utils;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

namespace KCV.Strategy.Rebellion
{
	public class TaskRebellionOrganize : SceneTaskMono
	{
		[SerializeField]
		private Transform prefabRebellionOrganizeCtrl;

		private CtrlRebellionOrganize _ctrlRebellionOrganize;

		[SerializeField]
		private CommonDialog strategyDialog;

		[SerializeField]
		private YesNoButton CancelDialogButton;

		public CtrlRebellionOrganize ctrlRebellionOrganize
		{
			get
			{
				return this._ctrlRebellionOrganize;
			}
		}

		private void OnDestroy()
		{
			this.prefabRebellionOrganizeCtrl = null;
			this._ctrlRebellionOrganize = null;
			this.strategyDialog = null;
			this.CancelDialogButton = null;
		}

		protected override bool Init()
		{
			DebugUtils.Log("TaskRebellionOrganize", string.Empty);
			this._ctrlRebellionOrganize = CtrlRebellionOrganize.Instantiate(this.prefabRebellionOrganizeCtrl.GetComponent<CtrlRebellionOrganize>(), StrategyTaskManager.GetOverView(), new Action(this.DecideSortieStart), new Action(this.DecideCancel));
			return true;
		}

		protected override bool UnInit()
		{
			DebugUtils.Log("TaskRebellionOrganize", string.Empty);
			if (this._ctrlRebellionOrganize != null && this._ctrlRebellionOrganize.get_gameObject() != null)
			{
				this._ctrlRebellionOrganize.get_gameObject().Discard();
			}
			this._ctrlRebellionOrganize = null;
			return true;
		}

		protected override bool Run()
		{
			this._ctrlRebellionOrganize.Run();
			return StrategyUtils.ChkStateRebellionTaskIsRun(StrategyRebellionTaskManagerMode.Organize);
		}

		private void DecideSortieStart()
		{
			DebugUtils.Log("TaskRebellionOrganize", string.Empty);
			RebellionManager rebellionManager = StrategyTaskManager.GetStrategyRebellion().GetRebellionManager();
			List<UIRebellionParticipatingFleetInfo> participatingFleetList = this._ctrlRebellionOrganize.participatingFleetSelector.participatingFleetList;
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.VanguardFleet);
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo2 = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.DecisiveBattlePrimaryFleet);
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo3 = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.VanguardSupportFleet);
			UIRebellionParticipatingFleetInfo uIRebellionParticipatingFleetInfo4 = participatingFleetList.Find((UIRebellionParticipatingFleetInfo x) => x.type == RebellionFleetType.DecisiveBattleSupportFleet);
			int[] array = new int[]
			{
				(!(uIRebellionParticipatingFleetInfo == null)) ? uIRebellionParticipatingFleetInfo.deckModel.Id : -1,
				(!(uIRebellionParticipatingFleetInfo2 == null)) ? uIRebellionParticipatingFleetInfo2.deckModel.Id : -1,
				(!(uIRebellionParticipatingFleetInfo3 == null)) ? uIRebellionParticipatingFleetInfo3.deckModel.Id : -1,
				(!(uIRebellionParticipatingFleetInfo4 == null)) ? uIRebellionParticipatingFleetInfo4.deckModel.Id : -1
			};
			bool flag = rebellionManager.IsGoRebellion(array[0], array[1], array[2], array[3]);
			List<IsGoCondition> list = null;
			List<IsGoCondition> list2 = null;
			if (array[2] != -1)
			{
				list = rebellionManager.IsValidMissionSub(array[2]);
			}
			if (array[3] != -1)
			{
				list2 = rebellionManager.IsValid_MissionMain(array[3]);
			}
			bool flag2 = list == null || (list != null && list.get_Count() == 0);
			bool flag3 = list2 == null || (list2 != null && list2.get_Count() == 0);
			if (flag && flag2 && flag3)
			{
				RebellionMapManager rebellionMapManager = rebellionManager.GoRebellion(array[0], array[1], array[2], array[3]);
				MapModel map = rebellionMapManager.Map;
				Hashtable hashtable = new Hashtable();
				hashtable.Add("rebellionMapManager", rebellionMapManager);
				hashtable.Add("rootType", 0);
				hashtable.Add("shipRecoveryType", ShipRecoveryType.None);
				hashtable.Add("escape", false);
				RetentionData.SetData(hashtable);
				Object.Destroy(SingletonMonoBehaviour<PortObjectManager>.Instance.get_gameObject());
				SingletonMonoBehaviour<AppInformation>.Instance.prevStrategyDecks = StrategyTopTaskManager.GetLogicManager().UserInfo.GetDecks();
				base.StartCoroutine(this.PlayTransition(map, uIRebellionParticipatingFleetInfo2.deckModel));
			}
			SoundUtils.PlaySE(SEFIleInfos.CommonEnter1);
		}

		private void DecideCancel()
		{
			this.strategyDialog.isUseDefaultKeyController = false;
			this.strategyDialog.OpenDialog(3, DialogAnimation.AnimType.POPUP);
			this.CancelDialogButton.SetOnSelectPositiveListener(new Action(this.CancelOrganize));
			this.CancelDialogButton.SetOnSelectNegativeListener(delegate
			{
				this.strategyDialog.CloseDialog();
			});
			this.CancelDialogButton.SetKeyController(new KeyControl(0, 0, 0.4f, 0.1f), true);
		}

		private void CancelOrganize()
		{
			this.strategyDialog.CloseDialog();
			this.strategyDialog.setCloseAction(delegate
			{
				DebugUtils.Log("TaskRebellionOrganize", string.Empty);
				base.Close();
				base.StartCoroutine(StrategyTaskManager.GetStrategyRebellion().taskRebellionEvent.NonDeckLose());
			});
		}

		[DebuggerHidden]
		private IEnumerator PlayTransition(MapModel mapModel, DeckModel deck)
		{
			TaskRebellionOrganize.<PlayTransition>c__Iterator16B <PlayTransition>c__Iterator16B = new TaskRebellionOrganize.<PlayTransition>c__Iterator16B();
			<PlayTransition>c__Iterator16B.mapModel = mapModel;
			<PlayTransition>c__Iterator16B.deck = deck;
			<PlayTransition>c__Iterator16B.<$>mapModel = mapModel;
			<PlayTransition>c__Iterator16B.<$>deck = deck;
			<PlayTransition>c__Iterator16B.<>f__this = this;
			return <PlayTransition>c__Iterator16B;
		}
	}
}
