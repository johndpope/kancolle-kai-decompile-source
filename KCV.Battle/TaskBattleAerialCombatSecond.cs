using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleAerialCombatSecond : BaseBattleTask
	{
		private KoukuuModel _clsKoukuu1;

		private KoukuuModel _clsKoukuu2;

		private ProdAerialSecondCutIn _prodAerialCutinP;

		private ProdAerialCombatP1 _prodAerialCombatP1;

		private ProdAerialCombatP2 _prodAerialCombatP2;

		private ProdAerialTouchPlane _prodAerialTouchPlane;

		private ProdAerialCombatP1 _prodAerialSecondP1;

		private ProdAerialCombatP2 _prodAerialSecondP2;

		private Dictionary<int, UIBattleShip> friendBattleship;

		private Dictionary<int, UIBattleShip> enemyBattleship;

		protected override bool Init()
		{
			this._clsKoukuu1 = BattleTaskManager.GetBattleManager().GetKoukuuData();
			this._clsKoukuu2 = BattleTaskManager.GetBattleManager().GetKoukuuData2();
			if (this._clsKoukuu1 == null)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombatSecond));
			}
			else
			{
				this._clsState = new StatementMachine();
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initAerialCombatCutIn), new StatementMachine.StatementMachineUpdate(this._updateAerialCombatCutIn));
			}
			return true;
		}

		protected override bool UnInit()
		{
			this._clsKoukuu1 = null;
			this._clsKoukuu2 = null;
			if (this._prodAerialCutinP != null)
			{
				this._prodAerialCutinP.get_gameObject().Discard();
			}
			this._prodAerialCutinP = null;
			if (this._prodAerialCombatP1 != null)
			{
				this._prodAerialCombatP1.get_gameObject().Discard();
			}
			this._prodAerialCombatP1 = null;
			if (this._prodAerialCombatP2 != null)
			{
				this._prodAerialCombatP2.get_gameObject().Discard();
			}
			this._prodAerialCombatP2 = null;
			if (this._prodAerialSecondP1 != null)
			{
				this._prodAerialSecondP1.get_gameObject().Discard();
			}
			this._prodAerialSecondP1 = null;
			if (this._prodAerialSecondP2 != null)
			{
				this._prodAerialSecondP2.get_gameObject().Discard();
			}
			this._prodAerialSecondP2 = null;
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.AerialCombatSecond);
		}

		private bool _initAerialCombatCutIn(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			this._prodAerialCutinP = ProdAerialSecondCutIn.Instantiate(PrefabFile.Load<ProdAerialSecondCutIn>(PrefabFileInfos.BattleProdAerialSecondCutIn), this._clsKoukuu1, cutInCamera.get_transform());
			this._prodAerialCombatP1 = ProdAerialCombatP1.Instantiate(PrefabFile.Load<ProdAerialCombatP1>(PrefabFileInfos.BattleProdAerialCombatP1), this._clsKoukuu1, cutInCamera.get_transform().get_parent(), this._prodAerialCutinP._chkCutInType());
			this._prodAerialCombatP1.get_gameObject().SetActive(false);
			this._prodAerialCombatP2 = ProdAerialCombatP2.Instantiate(PrefabFile.Load<ProdAerialCombatP2>(PrefabFileInfos.BattleProdAerialCombatP2), this._clsKoukuu1, cutInCamera.get_transform());
			this._prodAerialCombatP2.CreateHpGauge(FleetType.Friend);
			this._prodAerialCombatP2.get_gameObject().SetActive(false);
			this._prodAerialCutinP.Play(new Action(this._onAerialCombatCutInFinished));
			return false;
		}

		private bool _updateAerialCombatCutIn(object data)
		{
			return true;
		}

		private void _onAerialCombatCutInFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initAircraftCombat), new StatementMachine.StatementMachineUpdate(this._updateAircraftCombat));
		}

		private bool _initAircraftCombat(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			if (this._prodAerialCutinP._cutinPhaseCheck())
			{
				battleCameras.SetSplitCameras2D(true);
			}
			if (this._prodAerialCutinP._chkCutInType() == CutInType.Both)
			{
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			else if (this._prodAerialCutinP._chkCutInType() == CutInType.FriendOnly)
			{
				cutInEffectCamera.isCulling = false;
			}
			Object.Destroy(this._prodAerialCutinP.get_gameObject());
			this._prodAerialCombatP1.get_gameObject().SetActive(true);
			this._prodAerialCombatP1.Play(new Action(this._onAerialCombatPhase1Finished));
			return false;
		}

		private bool _updateAircraftCombat(object data)
		{
			return true;
		}

		private void _onAerialCombatPhase1Finished()
		{
			if (this._clsKoukuu1.existStage3())
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initAircraftCombatPhase2), new StatementMachine.StatementMachineUpdate(this._updateAircraftCombatPhase2));
			}
			else
			{
				Object.Destroy(this._prodAerialCombatP1.get_gameObject());
				this._onAircraftCombatFinished();
			}
		}

		private bool _initAircraftCombatPhase2(object data)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInCamera cutInCamera2 = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			this._prodAerialCombatP2.get_gameObject().SetActive(true);
			Object.Destroy(this._prodAerialCombatP1.get_gameObject());
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.set_enabled(true);
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(false);
			SlotitemModel_Battle touchPlane = this._clsKoukuu1.GetTouchPlane(true);
			SlotitemModel_Battle touchPlane2 = this._clsKoukuu1.GetTouchPlane(false);
			this._prodAerialTouchPlane = ProdAerialTouchPlane.Instantiate(Resources.Load<ProdAerialTouchPlane>("Prefabs/Battle/Production/AerialCombat/ProdAerialTouchPlane"), cutInCamera2.get_transform());
			this._prodAerialTouchPlane.get_transform().set_localPosition(Vector3.get_zero());
			this._prodAerialTouchPlane.Init(touchPlane, touchPlane2);
			this._prodAerialCombatP2.Play(new Action(this._onAircraftCombatFinished), dicFriendBattleShips, dicEnemyBattleShips);
			return false;
		}

		private bool _updateAircraftCombatPhase2(object data)
		{
			return true;
		}

		private void _onAircraftCombatFinished()
		{
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(false);
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.set_enabled(true);
			this.PlayProdDamage(this._clsKoukuu1, delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initAerialSecondCutIn), new StatementMachine.StatementMachineUpdate(this._updateAerialSecondCutIn));
			});
			if (this._prodAerialCombatP2 != null)
			{
				Object.Destroy(this._prodAerialCombatP2.get_gameObject());
			}
			if (this._prodAerialTouchPlane != null)
			{
				this._prodAerialTouchPlane.Hide();
			}
		}

		private bool _initAerialSecondCutIn(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			CutInType iType;
			if (this._clsKoukuu2.GetCaptainShip(true) != null && this._clsKoukuu2.GetCaptainShip(false) != null)
			{
				iType = CutInType.Both;
			}
			else if (this._clsKoukuu2.GetCaptainShip(true) != null)
			{
				iType = CutInType.FriendOnly;
			}
			else
			{
				iType = CutInType.EnemyOnly;
			}
			this._prodAerialSecondP1 = ProdAerialCombatP1.Instantiate(PrefabFile.Load<ProdAerialCombatP1>(PrefabFileInfos.BattleProdAerialCombatP1), this._clsKoukuu2, cutInCamera.get_transform().get_parent(), iType);
			this._prodAerialSecondP1.get_gameObject().SetActive(false);
			this._prodAerialSecondP2 = ProdAerialCombatP2.Instantiate(PrefabFile.Load<ProdAerialCombatP2>(PrefabFileInfos.BattleProdAerialCombatP2), this._clsKoukuu2, cutInCamera.get_transform());
			this._prodAerialSecondP2.CreateHpGauge(FleetType.Friend);
			this._prodAerialSecondP2.get_gameObject().SetActive(false);
			this._onAerialSecondCutInFinished();
			return false;
		}

		private bool _updateAerialSecondCutIn(object data)
		{
			return true;
		}

		private void _onAerialSecondCutInFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initAircraftSecond), new StatementMachine.StatementMachineUpdate(this._updateAircraftSecond));
		}

		private bool _initAircraftSecond(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleCutInCamera cutInCamera = battleCameras.cutInCamera;
			cutInCamera.isCulling = true;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			CutInType cutInType;
			if (this._clsKoukuu2.GetCaptainShip(true) != null && this._clsKoukuu2.GetCaptainShip(false) != null)
			{
				cutInType = CutInType.Both;
			}
			else if (this._clsKoukuu2.GetCaptainShip(true) != null)
			{
				cutInType = CutInType.FriendOnly;
			}
			else
			{
				cutInType = CutInType.EnemyOnly;
			}
			if (cutInType == CutInType.Both)
			{
				battleCameras.SetSplitCameras2D(true);
				cutInCamera.isCulling = true;
				cutInEffectCamera.isCulling = true;
			}
			else
			{
				if (cutInType == CutInType.FriendOnly)
				{
					cutInEffectCamera.isCulling = false;
				}
				battleCameras.SetSplitCameras2D(false);
			}
			this._prodAerialSecondP1.get_gameObject().SetActive(true);
			this._prodAerialSecondP1.Play(new Action(this._onAerialSecondPhase1Finished));
			return false;
		}

		private bool _updateAircraftSecond(object data)
		{
			return true;
		}

		private void _onAerialSecondPhase1Finished()
		{
			if (this._clsKoukuu2.existStage3())
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initAircraftSecondPhase2), new StatementMachine.StatementMachineUpdate(this._updateAircraftSecondPhase2));
			}
			else
			{
				Object.Destroy(this._prodAerialSecondP1.get_gameObject());
				this._onAircraftSecondFinished();
			}
		}

		private bool _initAircraftSecondPhase2(object data)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInCamera cutInCamera2 = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			this._prodAerialSecondP2.get_gameObject().SetActive(true);
			Object.Destroy(this._prodAerialSecondP1.get_gameObject());
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.set_enabled(true);
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(false);
			this._prodAerialSecondP2.Play(new Action(this._onAircraftSecondFinished), dicFriendBattleShips, dicEnemyBattleShips);
			return false;
		}

		private bool _updateAircraftSecondPhase2(object data)
		{
			return true;
		}

		private void _onAircraftSecondFinished()
		{
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(false);
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.set_enabled(true);
			this.PlayProdDamage(this._clsKoukuu2, delegate
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombatSecond));
			});
			if (this._prodAerialSecondP2 != null)
			{
				Object.Destroy(this._prodAerialSecondP2.get_gameObject());
			}
			if (this._prodAerialTouchPlane != null)
			{
				this._prodAerialTouchPlane.Hide();
			}
		}
	}
}
