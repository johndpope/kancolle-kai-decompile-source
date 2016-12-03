using Common.Enum;
using KCV.SortieBattle;
using local.managers;
using local.models;
using LT.Tweening;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace KCV.BattleCut
{
	public class BattleCutManager : MonoBehaviour
	{
		private static BattleCutManager instance;

		[SerializeField]
		private BattleCutTitle titleText;

		[SerializeField]
		private UIPanel bgPanel;

		[SerializeField]
		private BtlCut_Live2D _btlCutLive2D;

		[SerializeField]
		private Transform _sharedPlace;

		[SerializeField]
		private BaseCamera _camera;

		[SerializeField]
		private UIBattleCutNavigation _uiNavigation;

		[SerializeField]
		private BattleCutPrefabFile _clsBattleCutPrefabFile;

		private KeyControl _clsInput;

		private StatementMachine _clsState;

		private static MapManager _clsMapManger;

		private static BattleManager _clsBattleManager;

		private static BattleCutPhase _iNowPhase;

		private static BattleData _clsBattleData;

		private static List<BaseBattleCutState> _listBattleCutState;

		private static Action _actOnFinished;

		private static Action _actOnStartFadeOut;

		private static Action<ShipRecoveryType> _actOnFinishedRecoveryType;

		private LTDescr test;

		private static BattleCutManager Instance
		{
			get
			{
				if (BattleCutManager.instance == null)
				{
					BattleCutManager.instance = Object.FindObjectOfType<BattleCutManager>();
				}
				return BattleCutManager.instance;
			}
			set
			{
				BattleCutManager.instance = value;
			}
		}

		public static BattleCutManager Instantiate(BattleCutManager prefab, Vector3 worldPosition)
		{
			BattleCutManager battleCutManager = Object.Instantiate<BattleCutManager>(prefab);
			battleCutManager.get_transform().localScaleOne();
			battleCutManager.get_transform().set_position(worldPosition);
			return battleCutManager;
		}

		private void Awake()
		{
			App.TimeScale(1f);
			this._clsInput = ((SortieBattleTaskManager.GetKeyControl() == null) ? new KeyControl(0, 0, 0.4f, 0.1f) : SortieBattleTaskManager.GetKeyControl());
			this._clsState = new StatementMachine();
			Util.SetRootContentSize(base.GetComponent<UIRoot>(), App.SCREEN_RESOLUTION);
			this.bgPanel.widgetsAreStatic = true;
			this._btlCutLive2D.panel.alpha = 0f;
			BattleCutManager._listBattleCutState = new List<BaseBattleCutState>();
			BattleCutManager._listBattleCutState.Add(new StateBattleCutFormationSelect());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutCommand());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutBattle());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutWithdrawalDecision());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutJudge());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutResult());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutAdvancingWithdrawal());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutAdvancingWithdrawalDC());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutClearReward());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutMapOpen());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutFlagshipWreck());
			BattleCutManager._listBattleCutState.Add(new StateBattleCutEscortShipEvacuation());
			this._uiNavigation.Startup(new SettingModel());
			this._uiNavigation.panel.depth = 100;
		}

		private void OnDestroy()
		{
			Mem.Del<BattleCutTitle>(ref this.titleText);
			Mem.Del<UIPanel>(ref this.bgPanel);
			Mem.Del<BtlCut_Live2D>(ref this._btlCutLive2D);
			Mem.Del<Transform>(ref this._sharedPlace);
			Mem.Del<BaseCamera>(ref this._camera);
			this._clsBattleCutPrefabFile.Dispose();
			this._clsBattleCutPrefabFile.UnInit();
			Mem.Del<BattleCutPrefabFile>(ref this._clsBattleCutPrefabFile);
			Mem.Del<KeyControl>(ref this._clsInput);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			Mem.Del<MapManager>(ref BattleCutManager._clsMapManger);
			Mem.Del<BattleManager>(ref BattleCutManager._clsBattleManager);
			if (BattleCutManager._clsBattleData != null)
			{
				BattleCutManager._clsBattleData.UnInit();
			}
			Mem.Del<BattleData>(ref BattleCutManager._clsBattleData);
			if (BattleCutManager._listBattleCutState != null)
			{
				BattleCutManager._listBattleCutState.Clear();
			}
			Mem.DelListSafe<BaseBattleCutState>(ref BattleCutManager._listBattleCutState);
			Mem.Del<Action>(ref BattleCutManager._actOnFinished);
			Mem.Del<Action>(ref BattleCutManager._actOnStartFadeOut);
			Mem.Del<Action<ShipRecoveryType>>(ref BattleCutManager._actOnFinishedRecoveryType);
			Mem.Del<BattleCutManager>(ref BattleCutManager.instance);
		}

		private void Update()
		{
			if (SortieBattleTaskManager.GetKeyControl() == null)
			{
				this._clsInput.Update();
			}
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
		}

		public void StartBattleCut(MapManager manager, Action onStartFadeOut, Action<ShipRecoveryType> onFinished)
		{
			BattleCutManager._actOnStartFadeOut = onStartFadeOut;
			BattleCutManager._actOnFinishedRecoveryType = onFinished;
			BattleCutManager._clsMapManger = manager;
			BattleCutManager.ReqPhase(BattleCutPhase.BattleCutPhase_ST);
			this._btlCutLive2D.shipCharacter.ChangeCharacter(manager.Deck.GetFlagShip());
			this._btlCutLive2D.shipCharacter.get_transform().get_parent().localPositionX(this._btlCutLive2D.shipCharacter.getEnterPosition2().x);
			this._btlCutLive2D.shipCharacter.get_transform().get_parent().set_localScale(Vector3.get_one() * 1.1f);
		}

		public BattleCutManager StartBattleCut(PracticeManager manager, int enemyDeckID, BattleFormationKinds1 iKind, Action onFinished)
		{
			BattleCutManager._actOnFinished = onFinished;
			BattleCutManager._clsBattleManager = manager.StartBattlePractice(enemyDeckID, iKind);
			BattleCutManager.StartBattle(iKind);
			this._btlCutLive2D.shipCharacter.ChangeCharacter(manager.CurrentDeck.GetFlagShip());
			return this;
		}

		public static void StartBattle(BattleFormationKinds1 formationKind)
		{
			if (!(BattleCutManager._clsBattleManager is PracticeBattleManager))
			{
				if (BattleCutManager._clsMapManger is SortieMapManager)
				{
					BattleCutManager._clsBattleManager = ((SortieMapManager)BattleCutManager._clsMapManger).BattleStart(formationKind);
				}
				else
				{
					BattleCutManager._clsBattleManager = ((RebellionMapManager)BattleCutManager._clsMapManger).BattleStart(formationKind);
				}
			}
			switch (BattleCutManager._clsBattleManager.WarType)
			{
			case enumMapWarType.None:
				Dlg.Call(ref BattleCutManager._actOnFinished);
				break;
			case enumMapWarType.Normal:
				BattleCutManager.ReqPhase(BattleCutPhase.Command);
				break;
			case enumMapWarType.Midnight:
				BattleCutManager.ReqPhase(BattleCutPhase.NightBattle);
				break;
			case enumMapWarType.Night_To_Day:
				BattleCutManager.ReqPhase(BattleCutPhase.NightBattle);
				break;
			case enumMapWarType.AirBattle:
				BattleCutManager.ReqPhase(BattleCutPhase.Command);
				break;
			}
		}

		public static void EndBattleCut()
		{
			List<UIPanel> panels = new List<UIPanel>(Enumerable.Where<UIPanel>(BattleCutManager.Instance.GetComponentsInChildren<UIPanel>(), (UIPanel x) => x.alpha != 0f));
			BattleCutManager.Instance.get_transform().LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panels.ForEach(delegate(UIPanel y)
				{
					y.alpha = x;
				});
			}).setOnComplete(delegate
			{
				BattleCutManager.Instance.SetActive(false);
				Dlg.Call(ref BattleCutManager._actOnFinished);
			});
		}

		public static void EndBattleCut(ShipRecoveryType iType)
		{
			Dlg.Call(ref BattleCutManager._actOnStartFadeOut);
			List<UIPanel> panels = new List<UIPanel>(Enumerable.Where<UIPanel>(BattleCutManager.Instance.GetComponentsInChildren<UIPanel>(), (UIPanel x) => x.alpha != 0f));
			BattleCutManager.Instance.get_transform().LTValue(1f, 0f, Defines.PHASE_FADE_TIME).setEase(LeanTweenType.linear).setOnUpdate(delegate(float x)
			{
				panels.ForEach(delegate(UIPanel y)
				{
					y.alpha = x;
				});
			}).setOnComplete(delegate
			{
				BattleCutManager.Instance.SetActive(false);
				Dlg.Call<ShipRecoveryType>(ref BattleCutManager._actOnFinishedRecoveryType, iType);
			});
		}

		public static void ReqPhase(BattleCutPhase NextPhase)
		{
			BattleCutManager.CheckNextBattleState(NextPhase);
			BattleCutManager._iNowPhase = ((NextPhase != BattleCutPhase.NightBattle) ? NextPhase : BattleCutPhase.DayBattle);
			BattleCutManager.Instance._clsState.AddState(new StatementMachine.StatementMachineInitialize(BattleCutManager._listBattleCutState.get_Item((int)BattleCutManager._iNowPhase).Init), new StatementMachine.StatementMachineUpdate(BattleCutManager._listBattleCutState.get_Item((int)BattleCutManager._iNowPhase).Run), new StatementMachine.StatementMachineTerminate(BattleCutManager._listBattleCutState.get_Item((int)BattleCutManager._iNowPhase).Terminate));
			BattleCutManager.SetTitleText(NextPhase);
		}

		private static void CheckNextBattleState(BattleCutPhase iPhase)
		{
			if (iPhase != BattleCutPhase.DayBattle)
			{
				if (iPhase != BattleCutPhase.NightBattle)
				{
					return;
				}
				StateBattleCutBattle stateBattleCutBattle = BattleCutManager._listBattleCutState.get_Item(2) as StateBattleCutBattle;
				stateBattleCutBattle.isNightCombat = true;
			}
			else
			{
				StateBattleCutBattle stateBattleCutBattle2 = BattleCutManager._listBattleCutState.get_Item(2) as StateBattleCutBattle;
				stateBattleCutBattle2.isNightCombat = false;
			}
		}

		public static KeyControl GetKeyControl()
		{
			return BattleCutManager.Instance._clsInput;
		}

		public static StateBattleCutBattle GetStateBattle()
		{
			return (StateBattleCutBattle)BattleCutManager._listBattleCutState.get_Item(2);
		}

		public static BattleCutPhase GetNowPhase()
		{
			return BattleCutManager._iNowPhase;
		}

		public static Generics.BattleRootType GetBattleType()
		{
			if (BattleCutManager._clsBattleManager is SortieBattleManager)
			{
				return Generics.BattleRootType.SortieMap;
			}
			if (BattleCutManager._clsBattleManager is RebellionBattleManager)
			{
				return Generics.BattleRootType.Rebellion;
			}
			return Generics.BattleRootType.Practice;
		}

		public static BaseCamera GetCamera()
		{
			return BattleCutManager.Instance._camera;
		}

		public static MapManager GetMapManager()
		{
			return BattleCutManager._clsMapManger;
		}

		public static BattleManager GetBattleManager()
		{
			return BattleCutManager._clsBattleManager;
		}

		public static BattleCutPrefabFile GetPrefabFile()
		{
			return BattleCutManager.Instance._clsBattleCutPrefabFile;
		}

		public static BattleData GetBattleData()
		{
			if (BattleCutManager._clsBattleData == null)
			{
				BattleCutManager._clsBattleData = new BattleData();
			}
			return BattleCutManager._clsBattleData;
		}

		public static Transform GetSharedPlase()
		{
			return BattleCutManager.Instance._sharedPlace;
		}

		public static BtlCut_Live2D GetLive2D()
		{
			return BattleCutManager.Instance._btlCutLive2D;
		}

		public static UIBattleCutNavigation GetNavigation()
		{
			return BattleCutManager.Instance._uiNavigation;
		}

		public static void SetTitleText(BattleCutPhase phase)
		{
			BattleCutManager.Instance.titleText.SetPhaseText(phase);
		}

		public static void Discard()
		{
			Object.Destroy(BattleCutManager.Instance.get_gameObject());
		}
	}
}
