using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using System;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleWithdrawalDecision : BaseBattleTask
	{
		private bool _isNightCombat;

		private Dictionary<FleetType, Vector3> _dicSplitCameraPos;

		private Dictionary<FleetType, Quaternion> _dicSplitCameraRot;

		private ProdWithdrawalDecisionSelection _prodWithdrawalDecisionSelection;

		protected override bool Init()
		{
			this._isNightCombat = BattleTaskManager.GetBattleManager().HasNightBattle();
			if (!this._isNightCombat)
			{
				this.EndPhase(BattlePhase.Result);
				return true;
			}
			this._clsState = new StatementMachine();
			float num = 72f;
			this._dicSplitCameraPos = new Dictionary<FleetType, Vector3>();
			this._dicSplitCameraPos.Add(FleetType.Friend, new Vector3(0f, 4f, num));
			this._dicSplitCameraPos.Add(FleetType.Enemy, new Vector3(0f, 4f, -num));
			this._dicSplitCameraRot = new Dictionary<FleetType, Quaternion>();
			this._dicSplitCameraRot.Add(FleetType.Friend, Quaternion.Euler(Vector3.get_zero()));
			this._dicSplitCameraRot.Add(FleetType.Enemy, Quaternion.Euler(Vector3.get_up() * 180f));
			this._prodWithdrawalDecisionSelection = ProdWithdrawalDecisionSelection.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdWithdrawalDecisionSelection.GetComponent<ProdWithdrawalDecisionSelection>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitWithdrawalSelection), new StatementMachine.StatementMachineUpdate(this.UpdateWithdrawalSelection));
			return true;
		}

		protected override bool UnInit()
		{
			this._isNightCombat = false;
			if (this._dicSplitCameraPos != null)
			{
				this._dicSplitCameraPos.Clear();
			}
			if (this._dicSplitCameraRot != null)
			{
				this._dicSplitCameraRot.Clear();
			}
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.WithdrawalDecision);
		}

		private bool InitWithdrawalSelection(object data)
		{
			this._prodWithdrawalDecisionSelection.Play(delegate
			{
				BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				BattleField battleField = BattleTaskManager.GetBattleField();
				battleField.ResetFleetAnchorPosition();
				battleField.enemySeaLevel.SetActive(true);
				battleField.ReqTimeZone(TimeZone.Night, BattleTaskManager.GetSkyType());
				battleField.AlterWaveDirection(FleetType.Friend, FleetType.Friend);
				battleField.AlterWaveDirection(FleetType.Enemy, FleetType.Enemy);
				BattleShips battleShips = BattleTaskManager.GetBattleShips();
				battleShips.RadarDeployment(false);
				battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
				battleShips.SetStandingPosition(StandingPositionType.OneRow);
				battleShips.SetLayer(Generics.Layers.ShipGirl);
				BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
				battleCameras.InitEnemyFieldCameraDefault();
				battleCameras.SetVerticalSplitCameras(false);
				if (!battleCameras.isSplit)
				{
					battleCameras.SetSplitCameras(true);
				}
				battleCameras.ResetFieldCamSettings(FleetType.Friend);
				battleCameras.ResetFieldCamSettings(FleetType.Enemy);
				battleCameras.fieldDimCameraEnabled(false);
				BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.Fix);
				battleFieldCamera.SetFixCamera(this._dicSplitCameraPos.get_Item(FleetType.Friend), this._dicSplitCameraRot.get_Item(FleetType.Friend));
				battleFieldCamera.cullingMask = battleCameras.GetDefaultLayers();
				battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(1);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.Fix);
				battleFieldCamera.SetFixCamera(this._dicSplitCameraPos.get_Item(FleetType.Enemy), this._dicSplitCameraRot.get_Item(FleetType.Enemy));
				battleFieldCamera.cullingMask = battleCameras.GetEnemyCamSplitLayers();
				BattleShips battleShips2 = BattleTaskManager.GetBattleShips();
				battleShips2.SetBollboardTarget(true, battleCameras.fieldCameras.get_Item(0).get_transform());
				battleShips2.SetBollboardTarget(false, battleCameras.fieldCameras.get_Item(1).get_transform());
				battleShips2.SetTorpedoSalvoWakeAngle(false);
				BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
				cutInEffectCamera.isCulling = false;
				UITexture component = cutInEffectCamera.get_transform().FindChild("TorpedoLine/OverlayLine").GetComponent<UITexture>();
				if (component != null)
				{
					component.alpha = 0f;
				}
				BattleTaskManager.GetTorpedoHpGauges().SetDestroy();
			}, new DelDecideHexButtonEx(this.OnDecideWithdrawalButton));
			return false;
		}

		private bool UpdateWithdrawalSelection(object data)
		{
			return this._prodWithdrawalDecisionSelection != null && this._prodWithdrawalDecisionSelection.Run();
		}

		private void OnDecideWithdrawalButton(UIHexButtonEx btn)
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				Mem.DelComponentSafe<ProdWithdrawalDecisionSelection>(ref this._prodWithdrawalDecisionSelection);
			});
			Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
			{
				if (btn.index == 0)
				{
					this.EndPhase(BattlePhase.Result);
				}
				else if (!BattleTaskManager.GetIsSameBGM())
				{
					KCV.Utils.SoundUtils.StopFadeBGM(0.2f, delegate
					{
						BattleTaskManager.GetBattleManager().StartDayToNightBattle();
						this.EndPhase(BattlePhase.NightCombat);
					});
				}
				else
				{
					BattleTaskManager.GetBattleManager().StartDayToNightBattle();
					this.EndPhase(BattlePhase.NightCombat);
				}
			});
		}
	}
}
