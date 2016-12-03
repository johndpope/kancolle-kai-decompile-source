using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.Cameras;
using local.models.battle;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleOpeningTorpedoSalvo : BaseBattleTask
	{
		private RaigekiModel _clsRaigeki;

		private ProdTorpedoCutIn _prodTorpedoCutIn;

		private GameObject _prodTorpedoCutInTexture;

		private ProdTorpedoSalvoPhase2 _prodTorpedoSalvoPhase2;

		private ProdTorpedoSalvoPhase3 _prodTorpedoSalvoPhase3;

		private PSTorpedoWake TorpedoParticle;

		private UITexture CenterLine;

		protected override bool Init()
		{
			this._clsRaigeki = BattleTaskManager.GetBattleManager().GetKaimakuData();
			if (this._clsRaigeki == null)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.OpeningTorpedoSalvo));
			}
			else
			{
				this._clsState = new StatementMachine();
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initTorpedoCutInInjection), new StatementMachine.StatementMachineUpdate(this._updateTorpedoCutInInjection));
				this.TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
			}
			return true;
		}

		protected override bool UnInit()
		{
			if (this._prodTorpedoCutIn != null)
			{
				this._prodTorpedoCutIn.get_gameObject().Discard();
			}
			this._prodTorpedoCutIn = null;
			if (this._prodTorpedoSalvoPhase2 != null)
			{
				Object.Destroy(this._prodTorpedoSalvoPhase2.transform.get_gameObject());
			}
			this._prodTorpedoSalvoPhase2 = null;
			if (this._prodTorpedoSalvoPhase3 != null)
			{
				Object.Destroy(this._prodTorpedoSalvoPhase3.transform.get_gameObject());
			}
			this._prodTorpedoSalvoPhase3 = null;
			base.UnInit();
			if (this._clsRaigeki != null)
			{
				this._clsRaigeki = null;
			}
			this.TorpedoParticle = null;
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.OpeningTorpedoSalvo);
		}

		private bool _initTorpedoCutInInjection(object data)
		{
			BattleTaskManager.GetBattleShips().SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateTorpedoCutIn(observer)).Subscribe(delegate(bool _)
			{
				this._prodTorpedoCutIn.Play(delegate
				{
					this._onTorpedoCutInInjectionFinished();
				});
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateTorpedoCutIn(IObserver<bool> observer)
		{
			TaskBattleOpeningTorpedoSalvo.<CreateTorpedoCutIn>c__IteratorFF <CreateTorpedoCutIn>c__IteratorFF = new TaskBattleOpeningTorpedoSalvo.<CreateTorpedoCutIn>c__IteratorFF();
			<CreateTorpedoCutIn>c__IteratorFF.observer = observer;
			<CreateTorpedoCutIn>c__IteratorFF.<$>observer = observer;
			<CreateTorpedoCutIn>c__IteratorFF.<>f__this = this;
			return <CreateTorpedoCutIn>c__IteratorFF;
		}

		private bool _updateTorpedoCutInInjection(object data)
		{
			return true;
		}

		private void _onTorpedoCutInInjectionFinished()
		{
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			this.CenterLine = cutInEffectCamera.get_transform().FindChild("TorpedoLine/OverlayLine").GetComponent<UITexture>();
			this.CenterLine.alpha = 1f;
			cutInEffectCamera.motionBlur.set_enabled(false);
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initTorpedoCloseUp), new StatementMachine.StatementMachineUpdate(this._updateTorpedoCloseUp));
		}

		private bool _initTorpedoCloseUp(object data)
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			cutInEffectCamera.isCulling = true;
			this._prodTorpedoSalvoPhase2.Play(new Action(this._onTorpedoCloseUpFinished));
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateTorpedoPhase2(IObserver<bool> observer)
		{
			TaskBattleOpeningTorpedoSalvo.<CreateTorpedoPhase2>c__Iterator100 <CreateTorpedoPhase2>c__Iterator = new TaskBattleOpeningTorpedoSalvo.<CreateTorpedoPhase2>c__Iterator100();
			<CreateTorpedoPhase2>c__Iterator.observer = observer;
			<CreateTorpedoPhase2>c__Iterator.<$>observer = observer;
			<CreateTorpedoPhase2>c__Iterator.<>f__this = this;
			return <CreateTorpedoPhase2>c__Iterator;
		}

		private bool _updateTorpedoCloseUp(object data)
		{
			return this._prodTorpedoSalvoPhase2.Run();
		}

		private void _onTorpedoCloseUpFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initTorpedoExplosion), new StatementMachine.StatementMachineUpdate(this._updateTorpedoExplosion));
		}

		private bool _initTorpedoExplosion(object data)
		{
			this.CenterLine.alpha = 1f;
			BattleField battleField = BattleTaskManager.GetBattleField();
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			if (this._prodTorpedoSalvoPhase2 != null)
			{
				this._prodTorpedoSalvoPhase2.deleteTorpedoWake();
				Object.Destroy(this._prodTorpedoSalvoPhase2.transform.get_gameObject());
			}
			this._prodTorpedoSalvoPhase2 = null;
			battleCameras.SetVerticalSplitCameras(true);
			battleCameras.friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			battleCameras.enemyFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors.get_Item(CameraAnchorType.OneRowAnchor).get_Item(FleetType.Friend).get_position();
			battleCameras.friendFieldCamera.get_transform().set_position(new Vector3(-51f, 8f, 90f));
			battleCameras.friendFieldCamera.get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 70f, 0f)));
			Vector3 position2 = battleField.dicCameraAnchors.get_Item(CameraAnchorType.OneRowAnchor).get_Item(FleetType.Enemy).get_position();
			battleCameras.enemyFieldCamera.get_transform().set_position(new Vector3(-51f, 8f, -90f));
			battleCameras.enemyFieldCamera.get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 111f, 0f)));
			BattleTaskManager.GetBattleShips().SetBollboardTarget(false, battleCameras.enemyFieldCamera.get_transform());
			this._prodTorpedoSalvoPhase3.Play(new Action(this._onTorpedoExplosionFinished));
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateTorpedoPhase3(IObserver<bool> observer)
		{
			TaskBattleOpeningTorpedoSalvo.<CreateTorpedoPhase3>c__Iterator101 <CreateTorpedoPhase3>c__Iterator = new TaskBattleOpeningTorpedoSalvo.<CreateTorpedoPhase3>c__Iterator101();
			<CreateTorpedoPhase3>c__Iterator.observer = observer;
			<CreateTorpedoPhase3>c__Iterator.<$>observer = observer;
			<CreateTorpedoPhase3>c__Iterator.<>f__this = this;
			return <CreateTorpedoPhase3>c__Iterator;
		}

		private bool _updateTorpedoExplosion(object data)
		{
			return this._prodTorpedoSalvoPhase3.Update();
		}

		private void _onTorpedoExplosionFinished()
		{
			BattleCutInEffectCamera efcam = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
			circleHPGauge.get_transform().localScaleZero();
			this.PlayProdDamage(this._clsRaigeki, delegate
			{
				efcam.isCulling = true;
				Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
				{
					ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
					observerAction.Register(delegate
					{
						BattleTaskManager.GetTorpedoHpGauges().Hide();
					});
					this.EndPhase(BattleUtils.NextPhase(BattlePhase.OpeningTorpedoSalvo));
				});
			});
			if (this._prodTorpedoSalvoPhase3 != null)
			{
				Object.Destroy(this._prodTorpedoSalvoPhase3.transform.get_gameObject());
			}
			this._prodTorpedoSalvoPhase3 = null;
		}
	}
}
