using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleSupportingFire : BaseBattleTask
	{
		private IShienModel _clsShien;

		private ShienModel_Hou _clsShelling;

		private ShienModel_Air _clsAerial;

		private ShienModel_Rai _clsTorpedo;

		private ProdSupportCutIn _prodSupportCutIn;

		private ProdSupportShelling _prodSupportShelling;

		private ProdSupportTorpedoP1 _prodSupportTorpedoP1;

		private ProdSupportTorpedoP2 _prodSupportTorpedoP2;

		private ProdSupportAerialPhase1 _prodSupportAerialPhase1;

		private ProdSupportAerialPhase2 _prodSupportAerialPhase2;

		private PSTorpedoWake TorpedoParticle;

		private ParticleSystem SplashPar;

		protected override bool Init()
		{
			this._clsShien = BattleTaskManager.GetBattleManager().GetShienData();
			this._clsState = new StatementMachine();
			if (this._clsShien == null)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initSupportFleetAdmission), new StatementMachine.StatementMachineUpdate(this._updateSupportFleetAdmission));
			}
			return true;
		}

		protected override bool UnInit()
		{
			this._clsShien = null;
			this._clsState.Clear();
			if (this._prodSupportCutIn != null)
			{
				this._prodSupportCutIn.get_gameObject().Discard();
			}
			this._prodSupportCutIn = null;
			if (this._prodSupportShelling != null)
			{
				this._prodSupportShelling.get_gameObject().Discard();
			}
			this._prodSupportShelling = null;
			if (this._prodSupportTorpedoP1 != null && this._prodSupportTorpedoP1.transform != null)
			{
				Object.Destroy(this._prodSupportTorpedoP1.transform.get_gameObject());
			}
			this._prodSupportTorpedoP1 = null;
			if (this._prodSupportTorpedoP2 != null && this._prodSupportTorpedoP2.transform != null)
			{
				Object.Destroy(this._prodSupportTorpedoP2.transform.get_gameObject());
			}
			this._prodSupportTorpedoP2 = null;
			if (this._prodSupportAerialPhase1 != null)
			{
				this._prodSupportAerialPhase1.get_gameObject().Discard();
			}
			this._prodSupportAerialPhase1 = null;
			if (this._prodSupportAerialPhase2 != null)
			{
				this._prodSupportAerialPhase2.get_gameObject().Discard();
			}
			this._prodSupportAerialPhase2 = null;
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.SupportingFire);
		}

		private bool _initSupportFleetAdmission(object data)
		{
			BattleTaskManager.GetBattleField().ResetFleetAnchorPosition();
			BattleTaskManager.GetPrefabFile().DisposeProdCommandBuffer();
			BattleShips battleShips = BattleTaskManager.GetBattleShips();
			battleShips.RadarDeployment(false);
			battleShips.SetStandingPosition(StandingPositionType.OneRow);
			battleShips.SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateCutIn(observer)).Subscribe(delegate(bool _)
			{
				this._prodSupportCutIn.Play(delegate
				{
					this._onSupportFleetAdmissionFinished();
				});
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateCutIn(IObserver<bool> observer)
		{
			TaskBattleSupportingFire.<CreateCutIn>c__Iterator102 <CreateCutIn>c__Iterator = new TaskBattleSupportingFire.<CreateCutIn>c__Iterator102();
			<CreateCutIn>c__Iterator.observer = observer;
			<CreateCutIn>c__Iterator.<$>observer = observer;
			<CreateCutIn>c__Iterator.<>f__this = this;
			return <CreateCutIn>c__Iterator;
		}

		private bool _updateSupportFleetAdmission(object data)
		{
			return true;
		}

		private void _onSupportFleetAdmissionFinished()
		{
			if (this._clsShien is ShienModel_Rai)
			{
				this._clsTorpedo = (ShienModel_Rai)this._clsShien;
				this.TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.BattlePSTorpedowakeD);
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initSupportTorpedoPhase1), new StatementMachine.StatementMachineUpdate(this._updateSupportTorpedoPhase1));
			}
			else if (this._clsShien is ShienModel_Hou)
			{
				this._clsShelling = (ShienModel_Hou)this._clsShien;
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initSupportShelling), new StatementMachine.StatementMachineUpdate(this._updateSupportShelling));
			}
			else if (this._clsShien is ShienModel_Air)
			{
				this._clsAerial = (ShienModel_Air)this._clsShien;
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initSupportAerialPhase1), new StatementMachine.StatementMachineUpdate(this._updateSupportAerialPhase1));
			}
		}

		private bool _initSupportShelling(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateShelling(observer)).Subscribe(delegate(bool _)
			{
				this._prodSupportShelling.Play(new Action(this._onSupportShellingFinished));
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateShelling(IObserver<bool> observer)
		{
			TaskBattleSupportingFire.<CreateShelling>c__Iterator103 <CreateShelling>c__Iterator = new TaskBattleSupportingFire.<CreateShelling>c__Iterator103();
			<CreateShelling>c__Iterator.observer = observer;
			<CreateShelling>c__Iterator.<$>observer = observer;
			<CreateShelling>c__Iterator.<>f__this = this;
			return <CreateShelling>c__Iterator;
		}

		private bool _updateSupportShelling(object data)
		{
			return true;
		}

		private void _onSupportShellingFinished()
		{
			this.PlayProdDamage(this._clsShelling, delegate
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			});
		}

		private bool _initSupportAerialPhase1(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateAerialCombatCutIn(observer)).Subscribe(delegate(bool _)
			{
				this._prodSupportAerialPhase1.Play(new Action(this._onSupportAerialFinishedPhase1));
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateAerialCombatCutIn(IObserver<bool> observer)
		{
			TaskBattleSupportingFire.<CreateAerialCombatCutIn>c__Iterator104 <CreateAerialCombatCutIn>c__Iterator = new TaskBattleSupportingFire.<CreateAerialCombatCutIn>c__Iterator104();
			<CreateAerialCombatCutIn>c__Iterator.observer = observer;
			<CreateAerialCombatCutIn>c__Iterator.<$>observer = observer;
			<CreateAerialCombatCutIn>c__Iterator.<>f__this = this;
			return <CreateAerialCombatCutIn>c__Iterator;
		}

		private bool _updateSupportAerialPhase1(object data)
		{
			return true;
		}

		private void _onSupportAerialFinishedPhase1()
		{
			if (this._clsAerial.existStage3())
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initSupportAerialPhase2), new StatementMachine.StatementMachineUpdate(this._updateSupportAerialPhase2));
			}
			else
			{
				Object.Destroy(this._prodSupportAerialPhase1.get_gameObject());
				this._onSupportAerialFinishedPhase2();
			}
		}

		private bool _initSupportAerialPhase2(object data)
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInCamera cutInCamera2 = BattleTaskManager.GetBattleCameras().cutInCamera;
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			Dictionary<int, UIBattleShip> dicFriendBattleShips = BattleTaskManager.GetBattleShips().dicFriendBattleShips;
			Dictionary<int, UIBattleShip> dicEnemyBattleShips = BattleTaskManager.GetBattleShips().dicEnemyBattleShips;
			Object.Destroy(this._prodSupportAerialPhase1.get_gameObject());
			cutInCamera.cullingMask = (Generics.Layers.UI2D | Generics.Layers.CutIn);
			cutInEffectCamera.cullingMask = Generics.Layers.CutIn;
			cutInCamera.depth = 5f;
			cutInEffectCamera.depth = 4f;
			cutInEffectCamera.glowEffect.set_enabled(true);
			BattleTaskManager.GetBattleCameras().SetSplitCameras2D(false);
			this._prodSupportAerialPhase2.Play(new Action(this._onSupportAerialFinishedPhase2));
			return false;
		}

		private bool _updateSupportAerialPhase2(object data)
		{
			return true;
		}

		private void _onSupportAerialFinishedPhase2()
		{
			this.PlayProdDamage(this._clsAerial, delegate
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			});
		}

		private bool _initSupportTorpedoPhase1(object data)
		{
			this.SplashPar = ParticleFile.Load<ParticleSystem>(ParticleFileInfos.BattlePSSplashTorpedo);
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateTorpedo1(observer)).Subscribe(delegate(bool _)
			{
				this._prodSupportTorpedoP1.Play(new Action(this._onSupportTorpedoPhase1Finished));
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateTorpedo1(IObserver<bool> observer)
		{
			TaskBattleSupportingFire.<CreateTorpedo1>c__Iterator105 <CreateTorpedo1>c__Iterator = new TaskBattleSupportingFire.<CreateTorpedo1>c__Iterator105();
			<CreateTorpedo1>c__Iterator.observer = observer;
			<CreateTorpedo1>c__Iterator.<$>observer = observer;
			<CreateTorpedo1>c__Iterator.<>f__this = this;
			return <CreateTorpedo1>c__Iterator;
		}

		private bool _updateSupportTorpedoPhase1(object data)
		{
			return this._prodSupportTorpedoP1.Update();
		}

		private void _onSupportTorpedoPhase1Finished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initSupportTorpedoPhase2), new StatementMachine.StatementMachineUpdate(this._updateSupportTorpedoPhase2));
		}

		private bool _initSupportTorpedoPhase2(object data)
		{
			BattleField battleField = BattleTaskManager.GetBattleField();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			battleCameras.friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors.get_Item(CameraAnchorType.OneRowAnchor).get_Item(FleetType.Friend).get_position();
			battleCameras.friendFieldCamera.get_transform().set_position(new Vector3(-38f, 8f, -74f));
			battleCameras.friendFieldCamera.get_transform().set_localRotation(Quaternion.Euler(new Vector3(9.5f, 137.5f, 0f)));
			BattleTaskManager.GetBattleShips().SetBollboardTarget(false, battleCameras.friendFieldCamera.get_transform());
			if (this._prodSupportTorpedoP1 != null)
			{
				this._prodSupportTorpedoP1.deleteTorpedoWake();
				this._prodSupportTorpedoP1.OnSetDestroy();
			}
			this._prodSupportTorpedoP1 = null;
			this._prodSupportTorpedoP2.Play(new Action(this._onSupportTorpedoPhase2Finished));
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateTorpedo2(IObserver<bool> observer)
		{
			TaskBattleSupportingFire.<CreateTorpedo2>c__Iterator106 <CreateTorpedo2>c__Iterator = new TaskBattleSupportingFire.<CreateTorpedo2>c__Iterator106();
			<CreateTorpedo2>c__Iterator.observer = observer;
			<CreateTorpedo2>c__Iterator.<$>observer = observer;
			<CreateTorpedo2>c__Iterator.<>f__this = this;
			return <CreateTorpedo2>c__Iterator;
		}

		private bool _updateSupportTorpedoPhase2(object data)
		{
			return this._prodSupportTorpedoP2 != null && this._prodSupportTorpedoP2.Update();
		}

		private void _onSupportTorpedoPhase2Finished()
		{
			this.PlayProdDamage(this._clsTorpedo, delegate
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.SupportingFire));
			});
			if (this._prodSupportTorpedoP2 != null)
			{
				Object.Destroy(this._prodSupportTorpedoP2.transform.get_gameObject());
			}
			this._prodSupportTorpedoP2 = null;
		}
	}
}
