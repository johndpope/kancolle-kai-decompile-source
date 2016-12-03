using KCV.Organize;
using KCV.Strategy;
using KCV.Strategy.Deploy;
using KCV.Utils;
using local.managers;
using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

namespace KCV.EscortOrganize
{
	public class EscortOrganizeTaskManager : OrganizeTaskManager
	{
		public static int CurrentAreaID;

		private static EscortDeckManager EscortLogicManager;

		public new static TaskEscortOrganizeTop _clsTop;

		public new static TaskEscortOrganizeDetail _clsDetail;

		public new static TaskEscortOrganizeList _clsList;

		public new static TaskEscortOrganizeListDetail _clsListDetail;

		public TaskDeployTop DeployTop;

		public override IOrganizeManager GetLogicManager()
		{
			return EscortOrganizeTaskManager.EscortLogicManager;
		}

		public static EscortDeckManager GetEscortManager()
		{
			return EscortOrganizeTaskManager.EscortLogicManager;
		}

		public override TaskOrganizeTop GetTopTask()
		{
			return EscortOrganizeTaskManager._clsTop;
		}

		public override TaskOrganizeDetail GetDetailTask()
		{
			return EscortOrganizeTaskManager._clsDetail;
		}

		public override TaskOrganizeList GetListTask()
		{
			return EscortOrganizeTaskManager._clsList;
		}

		public override TaskOrganizeListDetail GetListDetailTask()
		{
			return EscortOrganizeTaskManager._clsListDetail;
		}

		protected override void Awake()
		{
			base.Awake();
		}

		[DebuggerHidden]
		private IEnumerator Start()
		{
			EscortOrganizeTaskManager.<Start>c__Iterator148 <Start>c__Iterator = new EscortOrganizeTaskManager.<Start>c__Iterator148();
			<Start>c__Iterator.<>f__this = this;
			return <Start>c__Iterator;
		}

		public static void CreateLogicManager()
		{
			EscortOrganizeTaskManager.EscortLogicManager = new EscortDeckManager(StrategyAreaManager.FocusAreaID);
		}

		public static void Init()
		{
			if (OrganizeTaskManager._clsTasks != null)
			{
				OrganizeTaskManager._iPhase = (OrganizeTaskManager._iPhaseReq = OrganizeTaskManager.OrganizePhase.Phase_ST);
			}
			StrategyTopTaskManager.Instance.UIModel.OverCamera.SetActive(false);
		}

		private void Update()
		{
			if (!this.isRun)
			{
				return;
			}
			OrganizeTaskManager._clsInputKey.Update();
			OrganizeTaskManager._clsTasks.Run();
			this.UpdateMode();
		}

		public static void ReqPhase(OrganizeTaskManager.OrganizePhase iPhase)
		{
			OrganizeTaskManager._iPhaseReq = iPhase;
		}

		public static OrganizeTaskManager.OrganizePhase GetPhase()
		{
			return OrganizeTaskManager._iPhase;
		}

		protected void UpdateMode()
		{
			if (OrganizeTaskManager._iPhaseReq == OrganizeTaskManager.OrganizePhase.Phase_BEF)
			{
				return;
			}
			Debug.Log("リクエストされたフェーズ:" + OrganizeTaskManager._iPhaseReq);
			switch (OrganizeTaskManager._iPhaseReq)
			{
			case OrganizeTaskManager.OrganizePhase.Phase_ST:
				if (OrganizeTaskManager._clsTasks.Open(EscortOrganizeTaskManager._clsTop) < 0)
				{
					return;
				}
				break;
			case OrganizeTaskManager.OrganizePhase.Detail:
				if (OrganizeTaskManager._clsTasks.Open(EscortOrganizeTaskManager._clsDetail) < 0)
				{
					return;
				}
				break;
			case OrganizeTaskManager.OrganizePhase.List:
				if (OrganizeTaskManager._clsTasks.Open(EscortOrganizeTaskManager._clsList) < 0)
				{
					return;
				}
				break;
			case OrganizeTaskManager.OrganizePhase.ListDetail:
				if (OrganizeTaskManager._clsTasks.Open(EscortOrganizeTaskManager._clsListDetail) < 0)
				{
					return;
				}
				break;
			}
			OrganizeTaskManager._iPhase = OrganizeTaskManager._iPhaseReq;
			OrganizeTaskManager._iPhaseReq = OrganizeTaskManager.OrganizePhase.Phase_BEF;
			EscortOrganizeTaskManager._clsTop.UpdateByModeChanging();
		}

		public void backToDeployTop()
		{
			TweenAlpha.Begin(base.get_transform().get_parent().get_gameObject(), 0.2f, 0f);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			EscortOrganizeTaskManager._clsTop.UnVisibleEmptyFrame();
			this.DelayAction(0.2f, delegate
			{
				StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.Deploy);
			});
		}

		public void BGTouch()
		{
			if (OrganizeTaskManager._clsTasks.ChkRun(EscortOrganizeTaskManager._clsTop) != -1 && !EscortOrganizeTaskManager._clsList.isListOpen())
			{
				this.backToDeployTop();
				OrganizeTaskManager._clsTasks.CloseAll();
			}
		}
	}
}
