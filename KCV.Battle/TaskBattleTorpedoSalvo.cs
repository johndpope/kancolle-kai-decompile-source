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
	public class TaskBattleTorpedoSalvo : BaseBattleTask
	{
		private Transform prefabProdTorpedoStraight;

		private RaigekiModel _clsRaigeki;

		private ProdTorpedoCutIn _prodTorpedoCutIn;

		private GameObject _prodTorpedoCutInTexture;

		private ProdTorpedoSalvoPhase2 _prodTorpedoSalvoPhase2;

		private ProdTorpedoSalvoPhase3 _prodTorpedoSalvoPhase3;

		private PSTorpedoWake TorpedoParticle;

		private UITexture CenterLine;

		protected override bool Init()
		{
			base.Init();
			this._clsRaigeki = BattleTaskManager.GetBattleManager().GetRaigekiData();
			if (this._clsRaigeki == null)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.TorpedoSalvo));
			}
			else
			{
				this._clsState = new StatementMachine();
				BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
				this.TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initTorpedoCutInNInjection), new StatementMachine.StatementMachineUpdate(this._updateTorpedoCutInNInjection));
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
			if (this.prefabProdTorpedoStraight != null)
			{
				Object.Destroy(this.prefabProdTorpedoStraight.get_gameObject());
			}
			this.prefabProdTorpedoStraight = null;
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
			return this.ChkChangePhase(BattlePhase.TorpedoSalvo);
		}

		private bool _initTorpedoCutInNInjection(object data)
		{
			BattleTaskManager.GetBattleShips().SetShipDrawType(FleetType.Enemy, ShipDrawType.Normal);
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateTorpedoCutIn(observer)).Subscribe(delegate(bool _)
			{
				this._prodTorpedoCutIn.Play(delegate
				{
					this._onTorpedoCutInNInjectionFinished();
				});
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateTorpedoCutIn(IObserver<bool> observer)
		{
			TaskBattleTorpedoSalvo.<CreateTorpedoCutIn>c__Iterator107 <CreateTorpedoCutIn>c__Iterator = new TaskBattleTorpedoSalvo.<CreateTorpedoCutIn>c__Iterator107();
			<CreateTorpedoCutIn>c__Iterator.observer = observer;
			<CreateTorpedoCutIn>c__Iterator.<$>observer = observer;
			<CreateTorpedoCutIn>c__Iterator.<>f__this = this;
			return <CreateTorpedoCutIn>c__Iterator;
		}

		private bool _updateTorpedoCutInNInjection(object data)
		{
			return true;
		}

		private void _onTorpedoCutInNInjectionFinished()
		{
			BattleCutInEffectCamera cutInEffectCamera = BattleTaskManager.GetBattleCameras().cutInEffectCamera;
			this.CenterLine = cutInEffectCamera.get_transform().FindChild("TorpedoLine/OverlayLine").GetComponent<UITexture>();
			this.CenterLine.alpha = 1f;
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
		private IEnumerator CreateTorpedoSalvoPhase2(IObserver<bool> observer)
		{
			TaskBattleTorpedoSalvo.<CreateTorpedoSalvoPhase2>c__Iterator108 <CreateTorpedoSalvoPhase2>c__Iterator = new TaskBattleTorpedoSalvo.<CreateTorpedoSalvoPhase2>c__Iterator108();
			<CreateTorpedoSalvoPhase2>c__Iterator.observer = observer;
			<CreateTorpedoSalvoPhase2>c__Iterator.<$>observer = observer;
			<CreateTorpedoSalvoPhase2>c__Iterator.<>f__this = this;
			return <CreateTorpedoSalvoPhase2>c__Iterator;
		}

		private bool _updateTorpedoCloseUp(object data)
		{
			return this._prodTorpedoSalvoPhase2 != null && this._prodTorpedoSalvoPhase2.Run();
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
				this._prodTorpedoSalvoPhase2.OnSetDestroy();
			}
			this._prodTorpedoSalvoPhase2 = null;
			if (this.prefabProdTorpedoStraight != null)
			{
				Object.Destroy(this.prefabProdTorpedoStraight.get_gameObject());
			}
			this.prefabProdTorpedoStraight = null;
			battleCameras.SetVerticalSplitCameras(true);
			battleCameras.friendFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			battleCameras.enemyFieldCamera.ReqViewMode(CameraActor.ViewMode.NotViewModeCtrl);
			Vector3 position = battleField.dicCameraAnchors.get_Item(CameraAnchorType.OneRowAnchor).get_Item(FleetType.Friend).get_position();
			battleCameras.friendFieldCamera.get_transform().set_position(new Vector3(-51f, 8f, 90f));
			battleCameras.friendFieldCamera.get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 70f, 0f)));
			Vector3 position2 = battleField.dicCameraAnchors.get_Item(CameraAnchorType.OneRowAnchor).get_Item(FleetType.Enemy).get_position();
			battleCameras.enemyFieldCamera.get_transform().set_position(new Vector3(-51f, 8f, -90f));
			battleCameras.enemyFieldCamera.get_transform().set_localRotation(Quaternion.Euler(new Vector3(10.5f, 111f, 0f)));
			this._prodTorpedoSalvoPhase3.Play(new Action(this._onTorpedoExplosionFinished));
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateTorpedoSalvoPhase3(IObserver<bool> observer)
		{
			TaskBattleTorpedoSalvo.<CreateTorpedoSalvoPhase3>c__Iterator109 <CreateTorpedoSalvoPhase3>c__Iterator = new TaskBattleTorpedoSalvo.<CreateTorpedoSalvoPhase3>c__Iterator109();
			<CreateTorpedoSalvoPhase3>c__Iterator.observer = observer;
			<CreateTorpedoSalvoPhase3>c__Iterator.<$>observer = observer;
			<CreateTorpedoSalvoPhase3>c__Iterator.<>f__this = this;
			return <CreateTorpedoSalvoPhase3>c__Iterator;
		}

		private bool _updateTorpedoExplosion(object data)
		{
			return this._prodTorpedoSalvoPhase3 != null && this._prodTorpedoSalvoPhase3.Update();
		}

		private void _onTorpedoExplosionFinished()
		{
			UICircleHPGauge circleHPGauge = BattleTaskManager.GetPrefabFile().circleHPGauge;
			circleHPGauge.get_transform().localScaleZero();
			this.PlayProdDamage(this._clsRaigeki, delegate
			{
				BattleTaskManager.GetBattleCameras().cutInEffectCamera.isCulling = true;
				Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
				{
					ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
					observerAction.Register(delegate
					{
						BattleTaskManager.GetTorpedoHpGauges().Hide();
					});
					this.EndPhase(BattleUtils.NextPhase(BattlePhase.TorpedoSalvo));
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
