using KCV.Battle.Production;
using KCV.Battle.Utils;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleAerialCombat : BaseBattleTask
	{
		private KoukuuModel _clsKoukuu;

		private ProdAerialCombatCutinP _prodAerialCutinP;

		private ProdAerialCombatP1 _prodAerialCombatP1;

		private ProdAerialCombatP2 _prodAerialCombatP2;

		private ProdAerialTouchPlane _prodAerialTouchPlane;

		protected override void Dispose(bool isDisposing)
		{
			Mem.Del<KoukuuModel>(ref this._clsKoukuu);
			Mem.Del<ProdAerialCombatCutinP>(ref this._prodAerialCutinP);
			Mem.Del<ProdAerialCombatP1>(ref this._prodAerialCombatP1);
			Mem.Del<ProdAerialCombatP2>(ref this._prodAerialCombatP2);
			Mem.Del<ProdAerialTouchPlane>(ref this._prodAerialTouchPlane);
			base.Dispose(isDisposing);
		}

		protected override bool Init()
		{
			this._clsKoukuu = BattleTaskManager.GetBattleManager().GetKoukuuData();
			if (this._clsKoukuu == null)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombat));
			}
			else if (BattleTaskManager.GetBattleManager().GetKoukuuData2() != null)
			{
				this.EndPhase(BattlePhase.AerialCombatSecond);
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
			this._clsKoukuu = null;
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
			return this.ChkChangePhase(BattlePhase.AerialCombat);
		}

		private bool _initAerialCombatCutIn(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateAerialCombatCutIn(observer)).Subscribe(delegate(bool _)
			{
				this._prodAerialCutinP.Play(new Action(this._onAerialCombatCutInFinished));
			});
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

		[DebuggerHidden]
		private IEnumerator CreateAerialCombatCutIn(IObserver<bool> observer)
		{
			TaskBattleAerialCombat.<CreateAerialCombatCutIn>c__IteratorF5 <CreateAerialCombatCutIn>c__IteratorF = new TaskBattleAerialCombat.<CreateAerialCombatCutIn>c__IteratorF5();
			<CreateAerialCombatCutIn>c__IteratorF.observer = observer;
			<CreateAerialCombatCutIn>c__IteratorF.<$>observer = observer;
			<CreateAerialCombatCutIn>c__IteratorF.<>f__this = this;
			return <CreateAerialCombatCutIn>c__IteratorF;
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
			if (this._clsKoukuu.existStage3())
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
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.set_enabled(true);
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(false);
			SlotitemModel_Battle touchPlane = this._clsKoukuu.GetTouchPlane(true);
			SlotitemModel_Battle touchPlane2 = this._clsKoukuu.GetTouchPlane(false);
			this._prodAerialTouchPlane.SetActive(true);
			this._prodAerialTouchPlane.Init(touchPlane, touchPlane2);
			this._prodAerialCombatP2.Play(new Action(this._onAircraftCombatFinished), dicFriendBattleShips, dicEnemyBattleShips);
			Object.Destroy(this._prodAerialCombatP1.get_gameObject());
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
			cutInEffectCamera.glowEffect.set_enabled(false);
			this.PlayProdDamage(this._clsKoukuu, delegate
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.AerialCombat));
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
	}
}
