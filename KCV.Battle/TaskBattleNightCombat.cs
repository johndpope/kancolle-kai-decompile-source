using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.Utils;
using Librarys.Cameras;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleNightCombat : BaseBattleTask
	{
		private ProdNightRadarDeployment _prodNightRadarDeployment;

		private NightCombatModel _clsNightCombat;

		private HougekiListModel _clsHougekiList;

		private ProdNightMessage _prodNightMessage;

		private ProdShellingAttack _prodShellingAttack;

		private SearchLightSceneController _ctrlSearchLight;

		private FlareBulletSceneController _ctrlFlareBullet;

		private int _nCurrentShellingCnt;

		private ProdAerialTouchPlane _prodAerialTouchPlane;

		private Vector3 _vCameraOriginPos;

		private int shellingCnt
		{
			get
			{
				if (this._clsHougekiList == null)
				{
					return -1;
				}
				return this._clsHougekiList.Count;
			}
		}

		private bool isNextAttack
		{
			get
			{
				return this.shellingCnt != this._nCurrentShellingCnt;
			}
		}

		protected override bool Init()
		{
			this._clsNightCombat = BattleTaskManager.GetBattleManager().GetNightCombatData();
			this._clsHougekiList = BattleTaskManager.GetBattleManager().GetHougekiList_Night();
			if (this._clsHougekiList == null)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.NightCombat));
				base.ImmediateTermination();
			}
			else
			{
				this._nCurrentShellingCnt = 1;
				this._clsState = new StatementMachine();
				this._prodShellingAttack = new ProdShellingAttack();
				this._vCameraOriginPos = BattleTaskManager.GetBattleCameras().fieldCameras.get_Item(0).get_transform().get_position();
				if (!BattleTaskManager.GetIsSameBGM())
				{
					KCV.Utils.SoundUtils.SwitchBGM((BGMFileInfos)BattleTaskManager.GetBattleManager().GetBgmId());
				}
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitNightMessage), new StatementMachine.StatementMachineUpdate(this.UpdateNightMessage));
				Transform transform = Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabSearchLightSceneController, Vector3.get_zero(), Quaternion.get_identity()) as Transform;
				this._ctrlSearchLight = transform.GetComponent<SearchLightSceneController>();
				Transform transform2 = Object.Instantiate(BattleTaskManager.GetPrefabFile().prefabFlareBulletSceneController, Vector3.get_zero(), Quaternion.get_identity()) as Transform;
				this._ctrlFlareBullet = transform2.GetComponent<FlareBulletSceneController>();
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.Del<NightCombatModel>(ref this._clsNightCombat);
			Mem.Del<HougekiListModel>(ref this._clsHougekiList);
			Mem.Del<ProdNightMessage>(ref this._prodNightMessage);
			if (this._prodShellingAttack != null)
			{
				this._prodShellingAttack.Dispose();
			}
			Mem.Del<ProdShellingAttack>(ref this._prodShellingAttack);
			if (this._prodAerialTouchPlane != null)
			{
				Object.Destroy(this._prodAerialTouchPlane.get_gameObject());
			}
			Mem.Del<ProdAerialTouchPlane>(ref this._prodAerialTouchPlane);
			if (this._ctrlSearchLight != null)
			{
				Object.Destroy(this._ctrlSearchLight.get_gameObject());
			}
			Mem.Del<SearchLightSceneController>(ref this._ctrlSearchLight);
			if (this._ctrlFlareBullet != null)
			{
				Object.Destroy(this._ctrlFlareBullet.get_gameObject());
			}
			Mem.Del<FlareBulletSceneController>(ref this._ctrlFlareBullet);
			Mem.Del<Vector3>(ref this._vCameraOriginPos);
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return this.ChkChangePhase(BattlePhase.NightCombat);
		}

		private bool InitNightMessage(object data)
		{
			this._prodNightRadarDeployment = ProdNightRadarDeployment.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdNightRadarDeployment.GetComponent<ProdNightRadarDeployment>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform());
			this._prodNightRadarDeployment.Play().Subscribe(delegate(bool _)
			{
				this.OnNightMessageFinished();
			});
			BattleField battleField = BattleTaskManager.GetBattleField();
			battleField.isEnemySeaLevelActive = false;
			ShipModel_Battle model = BattleTaskManager.GetBattleManager().Ships_f[0];
			KCV.Battle.Utils.ShipUtils.PlayStartNightCombatVoice(model);
			return false;
		}

		private bool UpdateNightMessage(object data)
		{
			return true;
		}

		private void OnNightMessageFinished()
		{
			BattleCutInCamera cutInCamera = BattleTaskManager.GetBattleCameras().cutInCamera;
			SlotitemModel_Battle touchPlane = this._clsNightCombat.GetTouchPlane(true);
			SlotitemModel_Battle touchPlane2 = this._clsNightCombat.GetTouchPlane(false);
			this._prodAerialTouchPlane = ((!(cutInCamera.get_transform().GetComponentInChildren<ProdAerialTouchPlane>() != null)) ? ProdAerialTouchPlane.Instantiate(Resources.Load<ProdAerialTouchPlane>("Prefabs/Battle/Production/AerialCombat/ProdAerialTouchPlane"), cutInCamera.get_transform()) : cutInCamera.get_transform().GetComponentInChildren<ProdAerialTouchPlane>());
			this._prodAerialTouchPlane.get_transform().set_localPosition(Vector3.get_zero());
			this._prodAerialTouchPlane.Init(touchPlane, touchPlane2);
			if (this._clsNightCombat.GetRationData() != null)
			{
				ProdCombatRation pcr = ProdCombatRation.Instantiate(BattleTaskManager.GetPrefabFile().prefabProdCombatRation.GetComponent<ProdCombatRation>(), BattleTaskManager.GetBattleCameras().cutInCamera.get_transform(), this._clsNightCombat.GetRationData());
				pcr.SetOnStageReady(delegate
				{
					if (this._prodNightRadarDeployment != null)
					{
						this._prodNightRadarDeployment.RadarObjectConvergence();
					}
					Mem.DelComponentSafe<ProdNightRadarDeployment>(ref this._prodNightRadarDeployment);
				}).Play(delegate
				{
					this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitSearchNFlare), new StatementMachine.StatementMachineUpdate(this.UpdateSearchNFlare));
				});
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Register(delegate
				{
					Mem.DelComponentSafe<ProdCombatRation>(ref pcr);
				});
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitSearchNFlare), new StatementMachine.StatementMachineUpdate(this.UpdateSearchNFlare));
			}
		}

		private bool InitSearchNFlare(object data)
		{
			BattleCameras battleCameras = BattleTaskManager.GetBattleCameras();
			BattleFieldCamera battleFieldCamera = battleCameras.fieldCameras.get_Item(0);
			battleFieldCamera.flareLayer.set_enabled(true);
			bool searchLightShip = this._clsNightCombat.GetSearchLightShip(true) != null;
			bool flareShip = this._clsNightCombat.GetFlareShip(true) != null;
			if (searchLightShip || flareShip)
			{
				if (this._prodNightRadarDeployment != null)
				{
					this._prodNightRadarDeployment.RadarObjectConvergence();
				}
				Mem.DelComponentSafe<ProdNightRadarDeployment>(ref this._prodNightRadarDeployment);
				ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
				observerAction.Executions();
				BattleTaskManager.GetBattleShips().SetStandingPosition(StandingPositionType.OneRow);
				battleFieldCamera.ReqViewMode(CameraActor.ViewMode.Fix);
				battleFieldCamera.get_transform().set_position(this._vCameraOriginPos);
				battleFieldCamera.get_transform().set_rotation(Quaternion.get_identity());
				ShipModel_BattleAll shipModel_BattleAll = (!searchLightShip) ? this._clsNightCombat.GetFlareShip(true) : this._clsNightCombat.GetSearchLightShip(true);
				if (shipModel_BattleAll != null)
				{
					BattleField battleField = BattleTaskManager.GetBattleField();
					UIBattleShip uIBattleShip = BattleTaskManager.GetBattleShips().dicFriendBattleShips.get_Item(shipModel_BattleAll.Index);
					float x = -uIBattleShip.get_transform().get_position().x;
					battleField.dicFleetAnchor.get_Item(FleetType.Friend).get_transform().AddPosX(x);
					battleFieldCamera.get_transform().AddPosX(x);
				}
			}
			this.SearchLight_FlareBullet_PlayAnimation().Subscribe(delegate(int _)
			{
				this.OnSearchNFlareFinished();
			});
			return false;
		}

		public IObservable<int> SearchLight_FlareBullet_PlayAnimation()
		{
			return Observable.FromCoroutine<int>((IObserver<int> observer) => this.SearchLight_FlareBullet_Coroutine(observer));
		}

		[DebuggerHidden]
		private IEnumerator SearchLight_FlareBullet_Coroutine(IObserver<int> observer)
		{
			TaskBattleNightCombat.<SearchLight_FlareBullet_Coroutine>c__IteratorFE <SearchLight_FlareBullet_Coroutine>c__IteratorFE = new TaskBattleNightCombat.<SearchLight_FlareBullet_Coroutine>c__IteratorFE();
			<SearchLight_FlareBullet_Coroutine>c__IteratorFE.observer = observer;
			<SearchLight_FlareBullet_Coroutine>c__IteratorFE.<$>observer = observer;
			<SearchLight_FlareBullet_Coroutine>c__IteratorFE.<>f__this = this;
			return <SearchLight_FlareBullet_Coroutine>c__IteratorFE;
		}

		private static Vector3? GetShipPointOfGaze(ShipModel_Battle model)
		{
			if (model != null)
			{
				return new Vector3?(BattleTaskManager.GetBattleShips().dicFriendBattleShips.get_Item(model.Index).pointOfGaze);
			}
			return default(Vector3?);
		}

		private Vector3? GetSearchLightShipPointOfGaze()
		{
			return TaskBattleNightCombat.GetShipPointOfGaze(this._clsNightCombat.GetSearchLightShip(true));
		}

		private Vector3? GetFlareBulletShipPointOfGaze()
		{
			return TaskBattleNightCombat.GetShipPointOfGaze(this._clsNightCombat.GetFlareShip(true));
		}

		private bool UpdateSearchNFlare(object data)
		{
			return true;
		}

		private void OnSearchNFlareFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitNightShelling), new StatementMachine.StatementMachineUpdate(this.UpdateNightShelling));
		}

		private bool InitNightShelling(object data)
		{
			HougekiModel nextData = this._clsHougekiList.GetNextData();
			if (nextData == null)
			{
				this.OnNightShellingFinished();
			}
			else
			{
				this._prodShellingAttack.Play(nextData, this._nCurrentShellingCnt, this.isNextAttack, null);
				if (this._prodNightRadarDeployment != null)
				{
					this._prodNightRadarDeployment.RadarObjectConvergence();
				}
				Mem.DelComponentSafe<ProdNightRadarDeployment>(ref this._prodNightRadarDeployment);
			}
			return false;
		}

		private bool UpdateNightShelling(object data)
		{
			if (this._prodShellingAttack.isFinished)
			{
				this._prodShellingAttack.Clear();
				this._nCurrentShellingCnt++;
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitNightShelling), new StatementMachine.StatementMachineUpdate(this.UpdateNightShelling));
				return true;
			}
			if (this._prodShellingAttack != null)
			{
				this._prodShellingAttack.Update();
			}
			return false;
		}

		private void OnNightShellingFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitGaugeDestruction), new StatementMachine.StatementMachineUpdate(this.UpdateGaugeDestruction));
		}

		private bool InitGaugeDestruction(object data)
		{
			return false;
		}

		private bool UpdateGaugeDestruction(object data)
		{
			this.OnGaugeDestructionFinished();
			return true;
		}

		private void OnGaugeDestructionFinished()
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitDeathCry), new StatementMachine.StatementMachineUpdate(this.UpdateDeathCry));
		}

		private bool InitDeathCry(object data)
		{
			return false;
		}

		private bool UpdateDeathCry(object data)
		{
			this.OnDeathCryFinished();
			return true;
		}

		private void OnDeathCryFinished()
		{
			ObserverActionQueue observerAction = BattleTaskManager.GetObserverAction();
			observerAction.Register(delegate
			{
				if (this._prodNightRadarDeployment != null)
				{
					this._prodNightRadarDeployment.RadarObjectConvergence();
				}
				Mem.DelComponentSafe<ProdNightRadarDeployment>(ref this._prodNightRadarDeployment);
			});
			Observable.Timer(TimeSpan.FromSeconds(1.0)).Subscribe(delegate(long _)
			{
				this.EndPhase(BattleUtils.NextPhase(BattlePhase.NightCombat));
			});
		}
	}
}
