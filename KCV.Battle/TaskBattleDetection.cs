using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using LT.Tweening;
using System;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleDetection : BaseBattleTask
	{
		private SakutekiModel _clsSakuteki;

		private ParticleSystem _psDetectionRipple;

		private ProdDetectionCutIn _prodDetectionCutIn;

		private ProdDetectionResultCutIn _prodDetectionResultCutIn;

		private ProdDetectionResultCutIn.AnimationList _iResult;

		private Tuple<Vector3, float> _tpFocusPoint;

		protected override bool Init()
		{
			this._clsSakuteki = BattleTaskManager.GetBattleManager().GetSakutekiData();
			if (this._clsSakuteki == null || !BattleTaskManager.GetBattleManager().IsExistSakutekiData())
			{
				base.ImmediateTermination();
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.Detection));
				return true;
			}
			this._clsState = new StatementMachine();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitMoveCameraTo2D), new StatementMachine.StatementMachineUpdate(this.UpdateMoveCameraTo2D));
			Transform transform = BattleTaskManager.GetBattleCameras().cutInCamera.get_transform();
			this._prodDetectionCutIn = ProdDetectionCutIn.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdDetectionCutIn.GetComponent<ProdDetectionCutIn>(), transform, this._clsSakuteki);
			this._prodDetectionResultCutIn = ProdDetectionResultCutIn.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdDetectionResultCutIn.GetComponent<ProdDetectionResultCutIn>(), transform, this._clsSakuteki);
			this._iResult = this._prodDetectionResultCutIn.detectionResult;
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del<SakutekiModel>(ref this._clsSakuteki);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			Mem.DelComponentSafe<ParticleSystem>(ref this._psDetectionRipple);
			Mem.DelComponentSafe<ProdDetectionCutIn>(ref this._prodDetectionCutIn);
			Mem.Del<Tuple<Vector3, float>>(ref this._tpFocusPoint);
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.Detection);
		}

		private bool InitMoveCameraTo2D(object data)
		{
			BattleFieldCamera cam = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			ProdDetectionStartCutIn pdsc = ProdDetectionStartCutIn.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdDetectionStartCutIn.GetComponent<ProdDetectionStartCutIn>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
			ShipModel_Battle detectionPrimaryShip = ShipUtils.GetDetectionPrimaryShip(this._clsSakuteki.planes_f, true);
			UIBattleShip uIBattleShip = (detectionPrimaryShip == null) ? battleShips.flagShipFriend : battleShips.dicFriendBattleShips.get_Item(detectionPrimaryShip.Index);
			Vector3 vector = Mathe.NormalizeDirection(uIBattleShip.pointOfGaze, Vector3.get_zero()) * 30f;
			Vector3 fixChasingCamera = new Vector3(uIBattleShip.pointOfGaze.x, uIBattleShip.pointOfGaze.y, uIBattleShip.pointOfGaze.z + vector.z);
			cam.pointOfGaze = uIBattleShip.pointOfGaze;
			cam.ReqViewMode(CameraActor.ViewMode.FixChasing);
			cam.SetFixChasingCamera(fixChasingCamera);
			Vector3 endCamPos = new Vector3(uIBattleShip.pointOfGaze.x, 50f, uIBattleShip.pointOfGaze.z + vector.z * 6f);
			Transform transform = uIBattleShip.get_transform();
			Vector3 position = BattleTaskManager.GetBattleShips().dicFriendBattleShips.get_Item(0).get_transform().get_position();
			this._psDetectionRipple = Util.Instantiate(ParticleFile.Load(ParticleFileInfos.BattlePSDetectionRipple), null, false, false).GetComponent<ParticleSystem>();
			this._psDetectionRipple.get_transform().set_parent(transform);
			this._psDetectionRipple.get_transform().set_position(new Vector3(position.x, position.y + 0.01f, position.z));
			this._psDetectionRipple.Play();
			pdsc.Play().Subscribe(delegate(bool _)
			{
				cam.get_transform().LTMove(endCamPos, 1.95f).setEase(LeanTweenType.easeInOutCubic);
				Mem.DelComponentSafe<ProdDetectionStartCutIn>(ref pdsc);
			});
			return false;
		}

		private bool UpdateMoveCameraTo2D(object data)
		{
			ProdCloud prodCloud = BattleTaskManager.GetPrefabFile().prodCloud;
			if (!prodCloud.isPlaying)
			{
				BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
				if (battleFieldCamera.eyePosition.y >= Vector3.Lerp(Vector3.get_zero(), Vector3.get_up() * 50f, 0.15f).y)
				{
					prodCloud.Play(ProdCloud.AnimationList.ProdCloudOut, delegate
					{
						if (this._prodDetectionCutIn.isAircraft)
						{
							this._prodDetectionCutIn.Play(delegate
							{
								if (this._iResult == ProdDetectionResultCutIn.AnimationList.DetectionSucces || this._iResult == ProdDetectionResultCutIn.AnimationList.DetectionLost)
								{
									this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitDetectionResultCutIn), new StatementMachine.StatementMachineUpdate(this.UpdateDetectionResultCutIn));
								}
								else
								{
									this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitEnemyFleetFocus), new StatementMachine.StatementMachineUpdate(this.UpdateEnemyFleetFocus));
								}
							}, null);
						}
						else if (this._iResult == ProdDetectionResultCutIn.AnimationList.DetectionSucces)
						{
							this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitDetectionResultCutIn), new StatementMachine.StatementMachineUpdate(this.UpdateDetectionResultCutIn));
						}
						else
						{
							this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitEnemyFleetFocus), new StatementMachine.StatementMachineUpdate(this.UpdateEnemyFleetFocus));
							this.InitCameraSettingsForEnemyFocus();
						}
					}, delegate
					{
						if (this._prodDetectionCutIn.isAircraft || (!this._prodDetectionCutIn.isAircraft && this._iResult == ProdDetectionResultCutIn.AnimationList.DetectionSucces))
						{
							this.InitCameraSettingsForEnemyFocus();
						}
					});
					return true;
				}
			}
			return false;
		}

		private void InitCameraSettingsForEnemyFocus()
		{
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			battleFieldCamera.get_transform().LTCancel();
			Vector3 pointOfGaze = BattleTaskManager.GetBattleShips().flagShipEnemy.pointOfGaze;
			battleFieldCamera.pointOfGaze = pointOfGaze;
			battleFieldCamera.ReqViewMode(CameraActor.ViewMode.FixChasing);
			Vector3 fixChasingCamera = this.CalcCameraFleetFocusPos(this._iResult);
			battleFieldCamera.SetFixChasingCamera(fixChasingCamera);
		}

		private bool InitDetectionResultCutIn(object data)
		{
			this.SetEnemyShipsDrawType(this._iResult);
			this._prodDetectionResultCutIn.Play(new Action(this.OnDetectionResultCutInFinished), null);
			return false;
		}

		private bool UpdateDetectionResultCutIn(object data)
		{
			return true;
		}

		private void OnDetectionResultCutInFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitEnemyFleetFocus), new StatementMachine.StatementMachineUpdate(this.UpdateEnemyFleetFocus));
		}

		private bool InitEnemyFleetFocus(object data)
		{
			this.SetEnemyShipsDrawType(this._iResult);
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			Vector3 pointOfGaze = BattleTaskManager.GetBattleShips().flagShipEnemy.pointOfGaze;
			Vector3 vector = Vector3.Lerp(battleFieldCamera.eyePosition, pointOfGaze, 0.3f);
			battleFieldCamera.get_transform().LTMove(vector, 2.7f).setEase(LeanTweenType.linear);
			this._tpFocusPoint = new Tuple<Vector3, float>(vector, Vector3.Distance(Vector3.Lerp(battleFieldCamera.eyePosition, vector, 0.7f), vector));
			ProdCloud prodCloud = BattleTaskManager.GetPrefabFile().prodCloud;
			prodCloud.Play(this.GetFleetFocusAnim(this._iResult), null, null);
			return false;
		}

		private bool UpdateEnemyFleetFocus(object data)
		{
			BattleFieldCamera battleFieldCamera = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0);
			if (Vector3.Distance(battleFieldCamera.eyePosition, this._tpFocusPoint.Item1) <= this._tpFocusPoint.Item2)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.Detection));
				return true;
			}
			return false;
		}

		private Vector3 CalcCameraFleetFocusPos(ProdDetectionResultCutIn.AnimationList iList)
		{
			Vector3 result = Vector3.get_zero();
			switch (iList)
			{
			case ProdDetectionResultCutIn.AnimationList.DetectionLost:
			case ProdDetectionResultCutIn.AnimationList.DetectionNotFound:
			case ProdDetectionResultCutIn.AnimationList.DetectionSucces:
			{
				Vector3 pointOfGaze = BattleTaskManager.GetBattleShips().flagShipEnemy.pointOfGaze;
				result = BattleDefines.FLEET_ADVENT_START_CAM_POS.get_Item(1);
				result.y = pointOfGaze.y;
				break;
			}
			}
			return result;
		}

		private ProdCloud.AnimationList GetFleetFocusAnim(ProdDetectionResultCutIn.AnimationList iList)
		{
			ProdCloud.AnimationList result = ProdCloud.AnimationList.ProdCloudIn;
			switch (iList)
			{
			case ProdDetectionResultCutIn.AnimationList.DetectionLost:
			case ProdDetectionResultCutIn.AnimationList.DetectionNotFound:
				result = ProdCloud.AnimationList.ProdCloudInNotFound;
				break;
			case ProdDetectionResultCutIn.AnimationList.DetectionSucces:
				result = ProdCloud.AnimationList.ProdCloudIn;
				break;
			}
			return result;
		}

		private void SetEnemyShipsDrawType(ProdDetectionResultCutIn.AnimationList iList)
		{
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			switch (iList)
			{
			case ProdDetectionResultCutIn.AnimationList.DetectionLost:
			case ProdDetectionResultCutIn.AnimationList.DetectionNotFound:
				battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Silhouette);
				break;
			case ProdDetectionResultCutIn.AnimationList.DetectionSucces:
				battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
				break;
			}
		}
	}
}
