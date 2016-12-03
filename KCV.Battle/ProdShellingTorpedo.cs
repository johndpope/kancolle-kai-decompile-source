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
	public class ProdShellingTorpedo : IDisposable
	{
		public Transform prefabProdTorpedoCutIn;

		public Transform prefabTorpedoStraightController;

		public Transform prefabProdTorpedoResucueCutIn;

		public Transform prefabProdTorpedoStraight;

		private StatementMachine _clsState;

		private RaigekiModel _clsRaigeki;

		private ProdTorpedoCutIn _prodTorpedoCutIn;

		private GameObject _prodTorpedoCutInTexture;

		private ProdTorpedoSalvoPhase2 _prodTorpedoSalvoPhase2;

		private ProdTorpedoSalvoPhase3 _prodTorpedoSalvoPhase3;

		private PSTorpedoWake TorpedoParticle;

		private UITexture CenterLine;

		private bool _isPlaying;

		private Action _actOnFinished;

		public ProdTorpedoSalvoPhase2 prodTorpedoSalvoPhase2
		{
			get
			{
				return this._prodTorpedoSalvoPhase2;
			}
		}

		public bool isPlaying
		{
			get
			{
				return this._isPlaying;
			}
		}

		public ProdShellingTorpedo(RaigekiModel model)
		{
			this._clsState = new StatementMachine();
			this._isPlaying = false;
			this._actOnFinished = null;
			this._clsRaigeki = model;
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
		}

		public void Dispose()
		{
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
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
			if (this._clsRaigeki != null)
			{
				this._clsRaigeki = null;
			}
			this.TorpedoParticle = null;
		}

		public void Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
		}

		public void Play(Action onFinished)
		{
			this._isPlaying = true;
			this._actOnFinished = onFinished;
			BattleTaskManager.GetBattleCameras().enemyFieldCamera.cullingMask = BattleTaskManager.GetBattleCameras().GetDefaultLayers();
			this.TorpedoParticle = ParticleFile.Load<PSTorpedoWake>(ParticleFileInfos.PARTICLE_FILE_INFOS_ID_ST);
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initTorpedoCutInNInjection), new StatementMachine.StatementMachineUpdate(this._updateTorpedoCutInNInjection));
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
			ProdShellingTorpedo.<CreateTorpedoCutIn>c__IteratorEA <CreateTorpedoCutIn>c__IteratorEA = new ProdShellingTorpedo.<CreateTorpedoCutIn>c__IteratorEA();
			<CreateTorpedoCutIn>c__IteratorEA.observer = observer;
			<CreateTorpedoCutIn>c__IteratorEA.<$>observer = observer;
			<CreateTorpedoCutIn>c__IteratorEA.<>f__this = this;
			return <CreateTorpedoCutIn>c__IteratorEA;
		}

		private bool _updateTorpedoCutInNInjection(object data)
		{
			return true;
		}

		private void _onTorpedoCutInNInjectionFinished()
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
			BattleTaskManager.GetBattleShips().SetBollboardTarget(false, battleCameras.enemyFieldCamera.get_transform());
			this._prodTorpedoSalvoPhase3.Play(new Action(this._onTorpedoExplosionFinished));
			return false;
		}

		private bool _updateTorpedoExplosion(object data)
		{
			return this._prodTorpedoSalvoPhase3 != null && this._prodTorpedoSalvoPhase3.Update();
		}

		private void _onTorpedoExplosionFinished()
		{
			if (this._prodTorpedoSalvoPhase3 != null)
			{
				Object.Destroy(this._prodTorpedoSalvoPhase3.transform.get_gameObject());
			}
			this._prodTorpedoSalvoPhase3 = null;
			Dlg.Call(ref this._actOnFinished);
		}
	}
}
