using Common.Enum;
using KCV.Battle;
using KCV.Battle.Production;
using KCV.SortieBattle;
using Librarys.State;
using local.managers;
using System;
using System.Collections;
using UnityEngine;

namespace KCV.SortieMap
{
	public class TaskSortieResult : Task
	{
		private CtrlSortieResult _ctrlSortieResult;

		private ProdMapOpen _prodMapOpen;

		private ProdMapPoint _prodStrategyPoint;

		private StatementMachine _clsState;

		private BattleShutter _uiBattleShutter;

		protected override bool Init()
		{
			this._clsState = new StatementMachine();
			UIAreaMapFrame uIAreaMapFrame = SortieMapTaskManager.GetUIAreaMapFrame();
			uIAreaMapFrame.Hide().setOnComplete(delegate
			{
				this._uiBattleShutter = BattleShutter.Instantiate(SortieBattleTaskManager.GetSortieBattlePrefabFile().prefabUIBattleShutter.GetComponent<BattleShutter>(), SortieMapTaskManager.GetSharedPlace(), 20);
				this._uiBattleShutter.Init(BaseShutter.ShutterMode.Open);
				this._uiBattleShutter.ReqMode(BaseShutter.ShutterMode.Close, delegate
				{
					ProdSortieEnd prodSortieEnd = ProdSortieEnd.Instantiate(SortieMapTaskManager.GetPrefabFile().prodSortieEnd.GetComponent<ProdSortieEnd>(), SortieMapTaskManager.GetSharedPlace());
					prodSortieEnd.Play(delegate
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitResult), new StatementMachine.StatementMachineUpdate(this.UpdateResult));
					});
				});
			});
			return true;
		}

		protected override bool UnInit()
		{
			Mem.Del<CtrlSortieResult>(ref this._ctrlSortieResult);
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			Mem.Del<BattleShutter>(ref this._uiBattleShutter);
			return true;
		}

		protected override bool Update()
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.SortieMapTaskManagerMode_BEF || SortieMapTaskManager.GetMode() == SortieMapTaskManagerMode.Result;
		}

		private void OnFinished()
		{
			Hashtable hashtable = new Hashtable();
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager is SortieMapManager)
			{
				hashtable.Add("sortieMapManager", mapManager);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", ShipRecoveryType.None);
			}
			else
			{
				hashtable.Add("rebellionMapManager", mapManager);
				hashtable.Add("rootType", 1);
				hashtable.Add("shipRecoveryType", ShipRecoveryType.None);
			}
			if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
			{
				SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(true);
				SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
				SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
				{
					SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
					Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
				});
			}
			else
			{
				SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
				Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
			}
		}

		private bool InitResult(object data)
		{
			this._ctrlSortieResult = CtrlSortieResult.Instantiate(SortieMapTaskManager.GetPrefabFile().prefabCtrlSortieResult.GetComponent<CtrlSortieResult>(), SortieMapTaskManager.GetSharedPlace(), SortieBattleTaskManager.GetMapManager().Items, delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitReward), new StatementMachine.StatementMachineUpdate(this.UpdateReward));
			});
			return false;
		}

		private bool UpdateResult(object data)
		{
			return this._ctrlSortieResult != null && this._ctrlSortieResult.Run();
		}

		private bool InitReward(object data)
		{
			this._clsState.Clear();
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitAirRecSucccessOrFailure), new StatementMachine.StatementMachineUpdate(this.UpdateAirRecSuccessOrFailure));
			return false;
		}

		private bool UpdateReward(object data)
		{
			return true;
		}

		private bool InitAirRecSucccessOrFailure(object data)
		{
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitMapOpen), new StatementMachine.StatementMachineUpdate(this.UpdateMapOpen));
			return false;
		}

		private bool UpdateAirRecSuccessOrFailure(object data)
		{
			return true;
		}

		private bool InitMapOpen(object data)
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager.GetNewOpenMapIDs() != null)
			{
				this._prodMapOpen = ProdMapOpen.Instantiate(PrefabFile.Load<ProdMapOpen>(PrefabFileInfos.MapOpen), mapManager.GetNewOpenAreaIDs(), mapManager.GetNewOpenMapIDs(), SortieMapTaskManager.GetSharedPlace(), SortieBattleTaskManager.GetKeyControl(), 120);
				this._prodMapOpen.Play(new Action(this.OnMapOpenFinished));
			}
			else
			{
				this.OnMapOpenFinished();
			}
			return false;
		}

		private bool UpdateMapOpen(object data)
		{
			return this._prodMapOpen != null && this._prodMapOpen.Run();
		}

		private void OnMapOpenFinished()
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager.GetNewOpenMapIDs() != null && mapManager.GetSPoint() > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitGetSPoint), new StatementMachine.StatementMachineUpdate(this.UpdateGetSPoint));
			}
			else
			{
				this.OnFinished();
			}
		}

		private bool InitGetSPoint(object data)
		{
			MapManager mapManager = SortieBattleTaskManager.GetMapManager();
			if (mapManager.GetNewOpenMapIDs() != null && mapManager.GetSPoint() > 0)
			{
				this._prodStrategyPoint = ProdMapPoint.Instantiate(Resources.Load<ProdMapPoint>("Prefabs/Battle/Production/MapOpen/ProdMapOpenPoint"), SortieMapTaskManager.GetSharedPlace(), mapManager.GetSPoint());
				this._prodStrategyPoint.Play(new Action(this.OnFinished));
			}
			else
			{
				this.OnFinished();
			}
			return false;
		}

		private bool UpdateGetSPoint(object data)
		{
			return true;
		}
	}
}
