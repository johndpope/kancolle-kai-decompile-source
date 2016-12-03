using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using KCV.SortieMap;
using KCV.Utils;
using Librarys.State;
using local.managers;
using local.models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleTaskManager : SceneTaskMono
	{
		private static BattleTaskManager instance;

		[SerializeField]
		private Light _lWorldLight;

		[SerializeField]
		private UIPanel _uiSeparatorLine;

		private static Transform _traStage;

		private static BattleField _clsBattleField;

		private static BattleShips _clsBattleShips;

		private static BattleCameras _clsBattleCameras;

		private static BattleHPGauges _clsBattleHPGauges;

		private static KeyControl _clsInputKey;

		private static Generics.BattleRootType _iRootType;

		private static TimeZone _iTimeZone;

		private static SkyType _iSkyType;

		private static BattlePhase _iPhase;

		private static BattlePhase _iPhaseReq;

		private static Action<ShipRecoveryType> _actOnFinished;

		private static ObserverActionQueue _clsAnimationObserver;

		private static SettingModel _clsSettingModel;

		private static MapManager _clsMapManager;

		private static BattleManager _clsBattleManager;

		private static Tasks _clsTasks;

		private static TaskBattleFleetAdvent _clsTaskFleetAdvent;

		private static TaskBattleBossInsert _clsTaskBossInsert;

		private static TaskBattleDetection _clsTaskDetection;

		private static TaskBattleCommand _clsTaskCommand;

		private static TaskBattleAerialCombat _clsTaskAerialCombat;

		private static TaskBattleAerialCombatSecond _clsTaskAerialCombatSecond;

		private static TaskBattleSupportingFire _clsTaskSupportingFire;

		private static TaskBattleOpeningTorpedoSalvo _clsTaskOpeningTorpedoSalvo;

		private static TaskBattleShelling _clsTaskShelling;

		private static TaskBattleTorpedoSalvo _clsTaskTorpedoSalvo;

		private static TaskBattleWithdrawalDecision _clsTaskWithdrawalDecision;

		private static TaskBattleNightCombat _clsTaskNightCombat;

		private static TaskBattleResult _clsTaskResult;

		private static TaskBattleFlagshipWreck _clsTaskFlagshipWreck;

		private static TaskBattleEscortShipEvacuation _clsTaskEscortShipEvacuation;

		private static TaskBattleAdvancingWithdrawal _clsTaskAdvancingWithdrawal;

		private static TaskBattleAdvancingWithdrawalDC _clsTaskAdvancingWithdrawalDC;

		private static TaskBattleClearReward _clsTaskClearReward;

		private static TaskBattleMapOpen _clsTaskMapOpen;

		[SerializeField]
		private BattleParticleFile _clsBattleParticleFile;

		[SerializeField]
		private BattlePefabFile _clsBattlePrefabFile;

		private static TorpedoHpGauges _clsTorpedoHpGauges;

		private static BattleTaskManager Instance
		{
			get
			{
				if (BattleTaskManager.instance == null)
				{
					BattleTaskManager.instance = (Object.FindObjectOfType(typeof(BattleTaskManager)) as BattleTaskManager);
					if (BattleTaskManager.instance == null)
					{
						return null;
					}
				}
				return BattleTaskManager.instance;
			}
		}

		public static BattleTaskManager Instantiate(BattleTaskManager prefab, Action<ShipRecoveryType> onFinished)
		{
			BattleTaskManager battleTaskManager = Object.Instantiate<BattleTaskManager>(prefab);
			battleTaskManager.get_transform().localScaleOne();
			battleTaskManager.get_transform().localPositionZero();
			return battleTaskManager.VirtualCtor(onFinished);
		}

		private BattleTaskManager VirtualCtor(Action<ShipRecoveryType> onFinished)
		{
			BattleTaskManager._actOnFinished = onFinished;
			return this;
		}

		private void Awake()
		{
			BattleTaskManager._clsInputKey = new KeyControl(0, 0, 0.4f, 0.1f);
			BattleTaskManager._clsTasks = new Tasks();
			BattleTaskManager._clsTasks.Init(32);
			base.get_transform().GetComponentsInChildren<UIRoot>().ForEach(delegate(UIRoot x)
			{
				Util.SetRootContentSize(x, App.SCREEN_RESOLUTION);
			});
			BattleTaskManager._clsAnimationObserver = new ObserverActionQueue();
			BattleTaskManager._clsTaskBossInsert = new TaskBattleBossInsert();
			BattleTaskManager._clsTaskDetection = new TaskBattleDetection();
			BattleTaskManager._clsTaskCommand = new TaskBattleCommand();
			BattleTaskManager._clsTaskAerialCombat = new TaskBattleAerialCombat();
			BattleTaskManager._clsTaskAerialCombatSecond = new TaskBattleAerialCombatSecond();
			BattleTaskManager._clsTaskSupportingFire = new TaskBattleSupportingFire();
			BattleTaskManager._clsTaskOpeningTorpedoSalvo = new TaskBattleOpeningTorpedoSalvo();
			BattleTaskManager._clsTaskShelling = new TaskBattleShelling();
			BattleTaskManager._clsTaskTorpedoSalvo = new TaskBattleTorpedoSalvo();
			BattleTaskManager._clsTaskFlagshipWreck = new TaskBattleFlagshipWreck();
			BattleTaskManager._clsTaskEscortShipEvacuation = new TaskBattleEscortShipEvacuation();
			BattleTaskManager._clsTaskClearReward = new TaskBattleClearReward();
			BattleTaskManager._clsTaskMapOpen = new TaskBattleMapOpen();
			BattleTaskManager._clsTaskAdvancingWithdrawal = new TaskBattleAdvancingWithdrawal(new Action<ShipRecoveryType>(this.GotoSortieMap));
			BattleTaskManager._clsTaskAdvancingWithdrawalDC = new TaskBattleAdvancingWithdrawalDC(new Action<ShipRecoveryType>(this.GotoSortieMap));
			BattleTaskManager._clsTaskFleetAdvent = new TaskBattleFleetAdvent();
			BattleTaskManager._clsTaskWithdrawalDecision = new TaskBattleWithdrawalDecision();
			BattleTaskManager._clsTaskNightCombat = new TaskBattleNightCombat();
			BattleTaskManager._clsTaskResult = new TaskBattleResult();
			if (SingletonMonoBehaviour<DontDestroyObject>.Instance != null)
			{
				SingletonMonoBehaviour<DontDestroyObject>.Instance.get_transform().set_position(Vector3.get_up() * 9999f);
			}
		}

		private void Start()
		{
			this.InitBattleData();
			BattleTaskManager._clsSettingModel = new SettingModel();
			BattleTaskManager._iPhase = (BattleTaskManager._iPhaseReq = BattlePhase.BattlePhase_BEF);
			BattleTaskManager._traStage = base.get_transform().FindChild("Stage");
			BattleTaskManager._clsBattleShips = new BattleShips();
			BattleTaskManager._clsBattleCameras = new BattleCameras();
			BattleTaskManager._clsBattleHPGauges = new BattleHPGauges();
			BattleTaskManager._clsBattleField = base.get_transform().GetComponentInChildren<BattleField>();
			UICircleHPGauge circleHPGauge = this._clsBattlePrefabFile.circleHPGauge;
			UIBattleNavigation battleNavigation = this._clsBattlePrefabFile.battleNavigation;
			battleNavigation.panel.depth = 100;
			BattleTaskManager._clsTorpedoHpGauges = new TorpedoHpGauges();
			BattleTaskManager._clsBattleShips.Init(BattleTaskManager.GetBattleManager());
			BattleTaskManager._clsBattleField.ReqTimeZone(this.GetStartTimeZone(BattleTaskManager.GetBattleManager().WarType), BattleTaskManager.GetSkyType());
			KCV.Utils.SoundUtils.SwitchBGM((BGMFileInfos)BattleTaskManager.GetBattleManager().GetBgmId());
			ProdSortieTransitionToBattle psttb = (SortieBattleTaskManager.GetSortieBattlePrefabFile() != null) ? SortieBattleTaskManager.GetSortieBattlePrefabFile().prodSortieTransitionToBattle : ProdSortieTransitionToBattle.Instantiate(Resources.Load<ProdSortieTransitionToBattle>("Prefabs/SortieMap/SortieTransitionToBattle/ProdSortieTransitionToBattle"), BattleTaskManager._clsBattleCameras.cutInCamera.get_transform()).QuickFadeInInit();
			Observable.FromCoroutine<float>((IObserver<float> observer) => this.InitBattle(observer)).Subscribe(delegate(float x)
			{
				if (x == 1f)
				{
					BattleTaskManager._iPhase = (BattleTaskManager._iPhaseReq = BattlePhase.FleetAdvent);
					Observable.Timer(TimeSpan.FromSeconds(0.30000001192092896)).Subscribe(delegate(long _)
					{
						BattleTaskManager._clsBattleField.AlterWaveDirection(FleetType.Friend);
						psttb.Play(ProdSortieTransitionToBattle.AnimationName.ProdSortieTransitionToBattleFadeIn, delegate
						{
							if (SortieBattleTaskManager.GetSortieBattlePrefabFile() != null)
							{
								SortieBattleTaskManager.GetSortieBattlePrefabFile().DisposeProdSortieTransitionToBattle();
							}
							else
							{
								Object.Destroy(psttb.get_gameObject());
							}
							if (SortieBattleTaskManager.GetTransitionCamera() != null)
							{
								SortieBattleTaskManager.GetTransitionCamera().set_enabled(false);
							}
							Mem.Del<ProdSortieTransitionToBattle>(ref psttb);
						});
					});
				}
			}).AddTo(base.get_gameObject());
		}

		private void OnDestroy()
		{
			Mem.Del<Light>(ref this._lWorldLight);
			Mem.Del<BattleField>(ref BattleTaskManager._clsBattleField);
			Mem.DelIDisposableSafe<BattleParticleFile>(ref this._clsBattleParticleFile);
			Mem.DelIDisposableSafe<BattlePefabFile>(ref this._clsBattlePrefabFile);
			Mem.Del<TorpedoHpGauges>(ref BattleTaskManager._clsTorpedoHpGauges);
			Mem.DelIDisposableSafe<BattleShips>(ref BattleTaskManager._clsBattleShips);
			Mem.DelIDisposableSafe<BattleCameras>(ref BattleTaskManager._clsBattleCameras);
			Mem.DelIDisposableSafe<BattleHPGauges>(ref BattleTaskManager._clsBattleHPGauges);
			Mem.Del<KeyControl>(ref BattleTaskManager._clsInputKey);
			Mem.Del<Action<ShipRecoveryType>>(ref BattleTaskManager._actOnFinished);
			Mem.DelIDisposableSafe<ObserverActionQueue>(ref BattleTaskManager._clsAnimationObserver);
			Mem.Del<SettingModel>(ref BattleTaskManager._clsSettingModel);
			Mem.Del<MapManager>(ref BattleTaskManager._clsMapManager);
			Mem.Del<BattleManager>(ref BattleTaskManager._clsBattleManager);
			BattleTaskManager._clsTasks.UnInit();
			Mem.DelIDisposableSafe<TaskBattleFleetAdvent>(ref BattleTaskManager._clsTaskFleetAdvent);
			Mem.DelIDisposableSafe<TaskBattleBossInsert>(ref BattleTaskManager._clsTaskBossInsert);
			Mem.DelIDisposableSafe<TaskBattleDetection>(ref BattleTaskManager._clsTaskDetection);
			Mem.DelIDisposableSafe<TaskBattleCommand>(ref BattleTaskManager._clsTaskCommand);
			Mem.DelIDisposableSafe<TaskBattleAerialCombat>(ref BattleTaskManager._clsTaskAerialCombat);
			Mem.DelIDisposableSafe<TaskBattleAerialCombatSecond>(ref BattleTaskManager._clsTaskAerialCombatSecond);
			Mem.DelIDisposableSafe<TaskBattleSupportingFire>(ref BattleTaskManager._clsTaskSupportingFire);
			Mem.DelIDisposableSafe<TaskBattleOpeningTorpedoSalvo>(ref BattleTaskManager._clsTaskOpeningTorpedoSalvo);
			Mem.DelIDisposableSafe<TaskBattleShelling>(ref BattleTaskManager._clsTaskShelling);
			Mem.DelIDisposableSafe<TaskBattleTorpedoSalvo>(ref BattleTaskManager._clsTaskTorpedoSalvo);
			Mem.DelIDisposableSafe<TaskBattleWithdrawalDecision>(ref BattleTaskManager._clsTaskWithdrawalDecision);
			Mem.DelIDisposableSafe<TaskBattleNightCombat>(ref BattleTaskManager._clsTaskNightCombat);
			Mem.DelIDisposableSafe<TaskBattleResult>(ref BattleTaskManager._clsTaskResult);
			Mem.DelIDisposableSafe<TaskBattleFlagshipWreck>(ref BattleTaskManager._clsTaskFlagshipWreck);
			Mem.DelIDisposableSafe<TaskBattleEscortShipEvacuation>(ref BattleTaskManager._clsTaskEscortShipEvacuation);
			Mem.DelIDisposableSafe<TaskBattleAdvancingWithdrawal>(ref BattleTaskManager._clsTaskAdvancingWithdrawal);
			Mem.DelIDisposableSafe<TaskBattleAdvancingWithdrawalDC>(ref BattleTaskManager._clsTaskAdvancingWithdrawalDC);
			Mem.DelIDisposableSafe<TaskBattleClearReward>(ref BattleTaskManager._clsTaskClearReward);
			Mem.DelIDisposableSafe<TaskBattleMapOpen>(ref BattleTaskManager._clsTaskMapOpen);
			Mem.Del<Tasks>(ref BattleTaskManager._clsTasks);
			KCV.Battle.Utils.ShipUtils.UnInit();
			UIDrawCall.ReleaseInactive();
			App.TimeScale(1f);
			App.SetFramerate(60);
			Mem.Del<BattleTaskManager>(ref BattleTaskManager.instance);
		}

		private void Update()
		{
			if (Input.get_touchCount() == 0 && !Input.GetMouseButton(0) && BattleTaskManager._clsInputKey != null)
			{
				BattleTaskManager._clsInputKey.Update();
			}
			BattleTaskManager._clsBattleShips.Update();
			BattleTaskManager._clsTasks.Update();
			this.UpdateMode();
		}

		[DebuggerHidden]
		private IEnumerator InitBattle(IObserver<float> observer)
		{
			BattleTaskManager.<InitBattle>c__IteratorCC <InitBattle>c__IteratorCC = new BattleTaskManager.<InitBattle>c__IteratorCC();
			<InitBattle>c__IteratorCC.observer = observer;
			<InitBattle>c__IteratorCC.<$>observer = observer;
			return <InitBattle>c__IteratorCC;
		}

		private void InitBattleData()
		{
			if (RetentionData.GetData() != null)
			{
				BattleTaskManager._iRootType = (Generics.BattleRootType)((int)RetentionData.GetData().get_Item("rootType"));
				switch (BattleTaskManager._iRootType)
				{
				case Generics.BattleRootType.Practice:
					BattleTaskManager.InitPracticeBattle();
					break;
				case Generics.BattleRootType.SortieMap:
					BattleTaskManager.InitSortieBattle();
					break;
				case Generics.BattleRootType.Rebellion:
					BattleTaskManager.InitRebellionBattle();
					break;
				}
				RetentionData.Release();
			}
			this.SetSkyType(BattleTaskManager._clsBattleManager);
		}

		private static bool InitSortieBattle()
		{
			BattleFormationKinds1 formationId = (BattleFormationKinds1)((int)RetentionData.GetData().get_Item("formation"));
			BattleTaskManager._clsBattleManager = ((SortieMapManager)SortieBattleTaskManager.GetMapManager()).BattleStart(formationId);
			return true;
		}

		private static bool InitPracticeBattle()
		{
			int enemy_deck_id = (int)RetentionData.GetData().get_Item("deckID");
			BattleFormationKinds1 formation_id = (BattleFormationKinds1)((int)RetentionData.GetData().get_Item("formation"));
			PracticeManager practiceManager = RetentionData.GetData().get_Item("practiceManager") as PracticeManager;
			BattleTaskManager._clsBattleManager = practiceManager.StartBattlePractice(enemy_deck_id, formation_id);
			return true;
		}

		private static bool InitRebellionBattle()
		{
			BattleFormationKinds1 formation_id = (BattleFormationKinds1)((int)RetentionData.GetData().get_Item("formation"));
			BattleTaskManager._clsBattleManager = ((RebellionMapManager)SortieBattleTaskManager.GetMapManager()).BattleStart(formation_id);
			return true;
		}

		private void SetSkyType(BattleManager manager)
		{
			if (manager is PracticeBattleManager)
			{
				BattleTaskManager._iSkyType = SkyType.Normal;
			}
			else if (manager.Map.AreaId == 17)
			{
				switch (manager.Map.No)
				{
				case 1:
					BattleTaskManager._iSkyType = SkyType.FinalArea171;
					break;
				case 2:
					BattleTaskManager._iSkyType = SkyType.FinalArea172;
					break;
				case 3:
					BattleTaskManager._iSkyType = SkyType.FinalArea173;
					break;
				case 4:
					BattleTaskManager._iSkyType = SkyType.FinalArea174;
					break;
				default:
					BattleTaskManager._iSkyType = SkyType.Normal;
					break;
				}
			}
			else
			{
				BattleTaskManager._iSkyType = SkyType.Normal;
			}
		}

		public static void DestroyUnneccessaryObject2Result()
		{
			Mem.DelComponentSafe<Transform>(ref BattleTaskManager._traStage);
			Mem.DelComponentSafe<Light>(ref BattleTaskManager.Instance._lWorldLight);
			Mem.DelComponentSafe<UIPanel>(ref BattleTaskManager.Instance._uiSeparatorLine);
			Mem.Del<TaskBattleBossInsert>(ref BattleTaskManager._clsTaskBossInsert);
			Mem.Del<TaskBattleDetection>(ref BattleTaskManager._clsTaskDetection);
			Mem.Del<TaskBattleCommand>(ref BattleTaskManager._clsTaskCommand);
			Mem.Del<TaskBattleAerialCombat>(ref BattleTaskManager._clsTaskAerialCombat);
			Mem.Del<TaskBattleAerialCombatSecond>(ref BattleTaskManager._clsTaskAerialCombatSecond);
			Mem.Del<TaskBattleSupportingFire>(ref BattleTaskManager._clsTaskSupportingFire);
			Mem.Del<TaskBattleOpeningTorpedoSalvo>(ref BattleTaskManager._clsTaskOpeningTorpedoSalvo);
			Mem.Del<TaskBattleShelling>(ref BattleTaskManager._clsTaskShelling);
			Mem.Del<TaskBattleTorpedoSalvo>(ref BattleTaskManager._clsTaskTorpedoSalvo);
			Mem.Del<TaskBattleFleetAdvent>(ref BattleTaskManager._clsTaskFleetAdvent);
			Mem.Del<TaskBattleWithdrawalDecision>(ref BattleTaskManager._clsTaskWithdrawalDecision);
			Mem.Del<TaskBattleNightCombat>(ref BattleTaskManager._clsTaskNightCombat);
			BattleTaskManager.Instance._clsBattlePrefabFile.DisposeUnneccessaryObject2Result();
		}

		public static BattlePhase GetPhase()
		{
			return BattleTaskManager._iPhaseReq;
		}

		public static void ReqPhase(BattlePhase iPhase)
		{
			BattleTaskManager._iPhaseReq = iPhase;
		}

		protected void UpdateMode()
		{
			if (BattleTaskManager._iPhaseReq == BattlePhase.BattlePhase_BEF)
			{
				return;
			}
			switch (BattleTaskManager._iPhaseReq)
			{
			case BattlePhase.BattlePhase_ST:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskBossInsert, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.FleetAdvent:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskFleetAdvent, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Detection:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskDetection, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Command:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskCommand, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AerialCombat:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskAerialCombat, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AerialCombatSecond:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskAerialCombatSecond, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.SupportingFire:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskSupportingFire, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.OpeningTorpedoSalvo:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskOpeningTorpedoSalvo, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Shelling:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskShelling, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.TorpedoSalvo:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskTorpedoSalvo, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.WithdrawalDecision:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskWithdrawalDecision, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.NightCombat:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskNightCombat, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.Result:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskResult, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.FlagshipWreck:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskFlagshipWreck, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.EscortShipEvacuation:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskEscortShipEvacuation, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AdvancingWithdrawal:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskAdvancingWithdrawal, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.AdvancingWithdrawalDC:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskAdvancingWithdrawalDC, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.ClearReward:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskClearReward, null) < 0)
				{
					return;
				}
				break;
			case BattlePhase.MapOpen:
				if (BattleTaskManager._clsTasks.Open(BattleTaskManager._clsTaskMapOpen, null) < 0)
				{
					return;
				}
				break;
			}
			BattleTaskManager._iPhase = BattleTaskManager._iPhaseReq;
			BattleTaskManager._iPhaseReq = BattlePhase.BattlePhase_BEF;
		}

		public static MapManager GetMapManager()
		{
			return BattleTaskManager._clsMapManager;
		}

		public static BattleManager GetBattleManager()
		{
			return BattleTaskManager._clsBattleManager;
		}

		public static Transform GetStage()
		{
			return BattleTaskManager._traStage;
		}

		public static BattleShips GetBattleShips()
		{
			return BattleTaskManager._clsBattleShips;
		}

		public static BattleCameras GetBattleCameras()
		{
			return BattleTaskManager._clsBattleCameras;
		}

		public static BattleHPGauges GetBattleHPGauges()
		{
			return BattleTaskManager._clsBattleHPGauges;
		}

		public static BattleField GetBattleField()
		{
			return BattleTaskManager._clsBattleField;
		}

		public static KeyControl GetKeyControl()
		{
			return BattleTaskManager._clsInputKey;
		}

		public static Generics.BattleRootType GetRootType()
		{
			return BattleTaskManager._iRootType;
		}

		public static ObserverActionQueue GetObserverAction()
		{
			return BattleTaskManager._clsAnimationObserver;
		}

		public static SettingModel GetSettingModel()
		{
			return BattleTaskManager._clsSettingModel;
		}

		public static SkyType GetSkyType()
		{
			return BattleTaskManager._iSkyType;
		}

		public static TaskBattleCommand GetTaskCommand()
		{
			return BattleTaskManager._clsTaskCommand;
		}

		public static TaskBattleShelling GetTaskShelling()
		{
			return BattleTaskManager._clsTaskShelling;
		}

		public static TaskBattleTorpedoSalvo GetTaskTorpedoSalvo()
		{
			return BattleTaskManager._clsTaskTorpedoSalvo;
		}

		public static TaskBattleNightCombat GetTaskNightCombat()
		{
			return BattleTaskManager._clsTaskNightCombat;
		}

		public static BattleParticleFile GetParticleFile()
		{
			return BattleTaskManager.Instance._clsBattleParticleFile;
		}

		public static BattlePefabFile GetPrefabFile()
		{
			return BattleTaskManager.Instance._clsBattlePrefabFile;
		}

		public static TorpedoHpGauges GetTorpedoHpGauges()
		{
			return BattleTaskManager._clsTorpedoHpGauges;
		}

		private void GotoSortieMap(ShipRecoveryType iType)
		{
			Dlg.Call<ShipRecoveryType>(ref BattleTaskManager._actOnFinished, iType);
		}

		public static bool IsSortieBattle()
		{
			return BattleTaskManager._actOnFinished != null;
		}

		public static bool GetIsSameBGM()
		{
			BattleManager battleManager = BattleTaskManager.GetBattleManager();
			bool bossBattle = battleManager.BossBattle;
			bool flag = false;
			int bgmId = battleManager.GetBgmId(true, bossBattle, out flag);
			int bgmId2 = battleManager.GetBgmId(false, bossBattle, out flag);
			return bgmId == bgmId2;
		}

		public static bool GetIsFinalAreaBattle()
		{
			return BattleTaskManager._clsBattleManager.Map != null && BattleTaskManager._clsBattleManager.Map.AreaId == 17;
		}

		private TimeZone GetStartTimeZone(enumMapWarType iType)
		{
			if (iType == enumMapWarType.Normal || iType == enumMapWarType.AirBattle)
			{
				return TimeZone.DayTime;
			}
			if (iType == enumMapWarType.Midnight || iType == enumMapWarType.Night_To_Day)
			{
				return TimeZone.Night;
			}
			return TimeZone.None;
		}

		public static TimeZone GetTimeZone()
		{
			return (BattleTaskManager._iPhase != BattlePhase.NightCombat) ? TimeZone.DayTime : TimeZone.Night;
		}
	}
}
