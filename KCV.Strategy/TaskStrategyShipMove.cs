using KCV.Utils;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Strategy
{
	public class TaskStrategyShipMove : SceneTaskMono
	{
		private StrategyTopTaskManager sttm;

		private TaskStrategySailSelect TaskSailSelect;

		private StrategyShipManager shipIconManager;

		private int moveDeckID;

		private int sailID;

		private Vector3 NextTilePos;

		private int currentAreaID;

		public AnimationCurve bound;

		private StrategyInfoManager.Mode prevMode;

		protected override void Start()
		{
			this.sttm = StrategyTaskManager.GetStrategyTop();
			this.TaskSailSelect = StrategyTopTaskManager.GetSailSelect();
			this.shipIconManager = StrategyTopTaskManager.Instance.ShipIconManager;
		}

		protected override bool Init()
		{
			Debug.Log("+++ TaskStrategyShipMove +++");
			KeyControlManager.Instance.KeyController = StrategyAreaManager.sailKeyController;
			this.currentAreaID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentAreaID;
			this.prevMode = StrategyTopTaskManager.Instance.GetInfoMng().NowInfoMode;
			StrategyTopTaskManager.Instance.GetInfoMng().SetSidePanelMode(StrategyInfoManager.Mode.AreaInfo);
			StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.3f);
			List<int> neighboringAreaIDs = StrategyTopTaskManager.GetLogicManager().Area.get_Item(this.currentAreaID).NeighboringAreaIDs;
			StrategyTopTaskManager.Instance.TileManager.ChangeTileColorMove(neighboringAreaIDs);
			return true;
		}

		protected override bool Run()
		{
			StrategyAreaManager.sailKeyController.Update();
			this.sailID = StrategyAreaManager.sailKeyController.Index;
			if (StrategyAreaManager.sailKeyController.IsChangeIndex)
			{
				StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(this.sailID, false);
				StrategyTopTaskManager.Instance.GetInfoMng().updateInfoPanel(this.sailID);
			}
			else
			{
				if (StrategyAreaManager.sailKeyController.keyState.get_Item(1).down)
				{
					return this.OnMoveDeside();
				}
				if (StrategyAreaManager.sailKeyController.keyState.get_Item(0).down)
				{
					this.OnMoveCancel();
					return false;
				}
				if (StrategyAreaManager.sailKeyController.keyState.get_Item(5).down)
				{
					SingletonMonoBehaviour<PortObjectManager>.Instance.BackToPortOrOrganize();
				}
			}
			return true;
		}

		protected override bool UnInit()
		{
			return true;
		}

		public bool OnMoveDeside()
		{
			if (StrategyTopTaskManager.GetLogicManager().Area.get_Item(this.currentAreaID).NeighboringAreaIDs.Exists((int x) => x == StrategyAreaManager.sailKeyController.Index) && !this.shipIconManager.isShipMoving)
			{
				StrategyTopTaskManager.Instance.TileManager.ChangeTileColorMove(null);
				this.MoveStart();
				SoundUtils.PlaySE(SEFIleInfos.StrategyShipMove);
				return false;
			}
			return true;
		}

		public void OnMoveCancel()
		{
			StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.AreaId, false);
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.CommandMenu);
			StrategyTopTaskManager.GetSailSelect().moveCharacterScreen(true, null);
			StrategyTopTaskManager.Instance.GetInfoMng().ExitInfoPanel();
			StrategyTopTaskManager.Instance.TileManager.ChangeTileColorMove(null);
			SoundUtils.PlaySE(SEFIleInfos.CommonCancel1);
			base.Close();
		}

		private void MoveStart()
		{
			Debug.Log("MoveStart" + Time.get_realtimeSinceStartup());
			StrategyTopTaskManager.GetSailSelect().isEnableCharacterEnter = true;
			this.moveDeckID = SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeck.Id;
			StrategyTopTaskManager.GetLogicManager().Move(this.moveDeckID, this.sailID);
			StrategyTopTaskManager.Instance.GetAreaMng().UpdateSelectArea(this.sailID, false);
			SingletonMonoBehaviour<AppInformation>.Instance.CurrentDeckAreaModel = StrategyTopTaskManager.GetLogicManager().Area.get_Item(this.sailID);
			this.shipIconManager.sortAreaShipIcon(this.currentAreaID, false, false);
			this.shipIconManager.sortAreaShipIcon(this.sailID, false, true);
			this.returnPrevInfoMode();
			StrategyTopTaskManager.ReqMode(StrategyTopTaskManager.StrategyTopTaskManagerMode.StrategyTopTaskManagerMode_ST);
			Debug.Log("MoveEnd" + Time.get_realtimeSinceStartup());
		}

		private void returnPrevInfoMode()
		{
			StrategyTopTaskManager.Instance.GetInfoMng().SetSidePanelMode(this.prevMode);
			StrategyTopTaskManager.Instance.GetInfoMng().EnterInfoPanel(0.3f);
		}
	}
}
