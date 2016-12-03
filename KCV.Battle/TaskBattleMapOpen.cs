using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using local.models.battle;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleMapOpen : BaseBattleTask
	{
		private BattleResultModel _clsResultModel;

		private MapManager _clsMapManager;

		private ProdMapOpen _prodMapOpen;

		private ProdMapPoint _prodMapPoint;

		private ProdMapClear _prodMapClear;

		private ProdThalassocracy _prodThalassocracy;

		private ProdShortRewardGet _prodShortRewardGet;

		private KeyControl _clsInput;

		protected override bool Init()
		{
			this._clsResultModel = BattleTaskManager.GetBattleManager().GetBattleResult();
			this._clsMapManager = SortieBattleTaskManager.GetMapManager();
			this._clsInput = BattleTaskManager.GetKeyControl();
			this._clsState = new StatementMachine();
			if (BattleTaskManager.GetRootType() == Generics.BattleRootType.Rebellion)
			{
				if (this._clsMapManager.IsNextFinal())
				{
					if (this._clsResultModel.WinRank == BattleWinRankKinds.B || this._clsResultModel.WinRank == BattleWinRankKinds.A || this._clsResultModel.WinRank == BattleWinRankKinds.S)
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initThalassocracyProd), new StatementMachine.StatementMachineUpdate(this._updateThalassocracyProd));
					}
					else
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
					}
				}
				else
				{
					this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
				}
			}
			else if (this._clsResultModel.FirstAreaClear && this._clsResultModel.FirstClear)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initThalassocracyProd), new StatementMachine.StatementMachineUpdate(this._updateThalassocracyProd));
			}
			else if (!this._clsResultModel.FirstAreaClear && this._clsResultModel.FirstClear)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initMapClearProd), new StatementMachine.StatementMachineUpdate(this._updateMapClearProd));
			}
			else if (!this._clsResultModel.FirstClear && this._clsResultModel.NewOpenMapIDs.Length > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initMapOpenProd), new StatementMachine.StatementMachineUpdate(this._updateMapOpenProd));
			}
			else if (this._clsResultModel.SPoint > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initStrategyPointProd), new StatementMachine.StatementMachineUpdate(this._updateStrategyPointProd));
			}
			else if (this._clsResultModel.GetAreaRewardItems() != null)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShortRewardGet), new StatementMachine.StatementMachineUpdate(this.UpdateShortRewardGet));
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
			}
			return true;
		}

		protected override bool UnInit()
		{
			if (this._prodMapOpen != null)
			{
				this._prodMapOpen.Discard();
			}
			if (this._prodMapPoint != null)
			{
				this._prodMapPoint.Discard();
			}
			if (this._prodMapClear != null)
			{
				this._prodMapClear.Discard();
			}
			if (this._prodThalassocracy != null)
			{
				this._prodThalassocracy.Discard();
			}
			this._prodMapOpen = null;
			this._prodMapClear = null;
			this._prodThalassocracy = null;
			this._clsState.Clear();
			return true;
		}

		protected override bool Update()
		{
			this._clsState.OnUpdate(Time.get_deltaTime());
			return this.ChkChangePhase(BattlePhase.MapOpen);
		}

		private bool _initThalassocracyProd(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateThalassocracy(observer)).Subscribe(delegate(bool _)
			{
				this._prodThalassocracy.Play(new Action(this._onThalassocracyProdFinished), BattleTaskManager.GetRootType(), BattleTaskManager.GetBattleManager().AreaName);
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator CreateThalassocracy(IObserver<bool> observer)
		{
			TaskBattleMapOpen.<CreateThalassocracy>c__IteratorF9 <CreateThalassocracy>c__IteratorF = new TaskBattleMapOpen.<CreateThalassocracy>c__IteratorF9();
			<CreateThalassocracy>c__IteratorF.observer = observer;
			<CreateThalassocracy>c__IteratorF.<$>observer = observer;
			<CreateThalassocracy>c__IteratorF.<>f__this = this;
			return <CreateThalassocracy>c__IteratorF;
		}

		private bool _updateThalassocracyProd(object data)
		{
			return this._prodThalassocracy.Run();
		}

		private void _onThalassocracyProdFinished()
		{
			if (this._clsResultModel.NewOpenMapIDs.Length > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initMapOpenProd), new StatementMachine.StatementMachineUpdate(this._updateMapOpenProd));
			}
			else if (this._clsResultModel.SPoint > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initStrategyPointProd), new StatementMachine.StatementMachineUpdate(this._updateStrategyPointProd));
			}
			else if (this._clsResultModel.GetAreaRewardItems() != null)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShortRewardGet), new StatementMachine.StatementMachineUpdate(this.UpdateShortRewardGet));
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
			}
			this._prodThalassocracy.Discard();
		}

		private bool _initMapClearProd(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.createMapClear(observer)).Subscribe(delegate(bool _)
			{
				this._prodMapClear.Play(new Action(this._onMapClearProdFinished));
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator createMapClear(IObserver<bool> observer)
		{
			TaskBattleMapOpen.<createMapClear>c__IteratorFA <createMapClear>c__IteratorFA = new TaskBattleMapOpen.<createMapClear>c__IteratorFA();
			<createMapClear>c__IteratorFA.observer = observer;
			<createMapClear>c__IteratorFA.<$>observer = observer;
			<createMapClear>c__IteratorFA.<>f__this = this;
			return <createMapClear>c__IteratorFA;
		}

		private bool _updateMapClearProd(object data)
		{
			return this._prodMapClear.Run();
		}

		private void _onMapClearProdFinished()
		{
			if (this._clsResultModel.NewOpenMapIDs.Length > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initMapOpenProd), new StatementMachine.StatementMachineUpdate(this._updateMapOpenProd));
			}
			else if (this._clsResultModel.SPoint > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initStrategyPointProd), new StatementMachine.StatementMachineUpdate(this._updateStrategyPointProd));
			}
			else if (this._clsResultModel.GetAreaRewardItems() != null)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShortRewardGet), new StatementMachine.StatementMachineUpdate(this.UpdateShortRewardGet));
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
			}
			this._prodMapClear.Discard();
		}

		private bool _initMapOpenProd(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.createMapOpen(observer)).Subscribe(delegate(bool _)
			{
				this._prodMapOpen.Play(new Action(this._onMapOpenProdFinished));
			});
			return false;
		}

		[DebuggerHidden]
		private IEnumerator createMapOpen(IObserver<bool> observer)
		{
			TaskBattleMapOpen.<createMapOpen>c__IteratorFB <createMapOpen>c__IteratorFB = new TaskBattleMapOpen.<createMapOpen>c__IteratorFB();
			<createMapOpen>c__IteratorFB.observer = observer;
			<createMapOpen>c__IteratorFB.<$>observer = observer;
			<createMapOpen>c__IteratorFB.<>f__this = this;
			return <createMapOpen>c__IteratorFB;
		}

		private bool _updateMapOpenProd(object data)
		{
			return this._prodMapOpen != null && this._prodMapOpen.Run();
		}

		private void _onMapOpenProdFinished()
		{
			if (this._clsResultModel.SPoint > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initStrategyPointProd), new StatementMachine.StatementMachineUpdate(this._updateStrategyPointProd));
			}
			else if (this._clsResultModel.GetAreaRewardItems() != null)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShortRewardGet), new StatementMachine.StatementMachineUpdate(this.UpdateShortRewardGet));
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
			}
		}

		private bool _initStrategyPointProd(object data)
		{
			if (this._clsResultModel.SPoint <= 0)
			{
				this._onStrategyPointProdFinished();
			}
			else
			{
				Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.createStrategyPoint(observer)).Subscribe(delegate(bool _)
				{
					this._prodMapPoint.Play(new Action(this._onStrategyPointProdFinished));
				});
			}
			return false;
		}

		[DebuggerHidden]
		private IEnumerator createStrategyPoint(IObserver<bool> observer)
		{
			TaskBattleMapOpen.<createStrategyPoint>c__IteratorFC <createStrategyPoint>c__IteratorFC = new TaskBattleMapOpen.<createStrategyPoint>c__IteratorFC();
			<createStrategyPoint>c__IteratorFC.observer = observer;
			<createStrategyPoint>c__IteratorFC.<$>observer = observer;
			<createStrategyPoint>c__IteratorFC.<>f__this = this;
			return <createStrategyPoint>c__IteratorFC;
		}

		private bool _updateStrategyPointProd(object data)
		{
			return false;
		}

		private void _onStrategyPointProdFinished()
		{
			if (this._clsResultModel.GetAreaRewardItems() != null)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShortRewardGet), new StatementMachine.StatementMachineUpdate(this.UpdateShortRewardGet));
			}
			else
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
			}
		}

		private bool InitShortRewardGet(object data)
		{
			Observable.FromCoroutine<bool>((IObserver<bool> observer) => this.CreateShortRewardGet(observer)).Subscribe(delegate(bool _)
			{
				if (!_)
				{
					this.OnShortRewardGetFinished();
				}
				else
				{
					this._prodShortRewardGet.Play(delegate
					{
						this.OnShortRewardGetFinished();
					});
				}
			});
			return false;
		}

		private bool UpdateShortRewardGet(object data)
		{
			return true;
		}

		[DebuggerHidden]
		private IEnumerator CreateShortRewardGet(IObserver<bool> observer)
		{
			TaskBattleMapOpen.<CreateShortRewardGet>c__IteratorFD <CreateShortRewardGet>c__IteratorFD = new TaskBattleMapOpen.<CreateShortRewardGet>c__IteratorFD();
			<CreateShortRewardGet>c__IteratorFD.observer = observer;
			<CreateShortRewardGet>c__IteratorFD.<$>observer = observer;
			<CreateShortRewardGet>c__IteratorFD.<>f__this = this;
			return <CreateShortRewardGet>c__IteratorFD;
		}

		private void OnShortRewardGetFinished()
		{
			Mem.DelComponentSafe<ProdShortRewardGet>(ref this._prodShortRewardGet);
			this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initChkNextCell), new StatementMachine.StatementMachineUpdate(this._updateChkNextCell));
		}

		private bool _initChkNextCell(object data)
		{
			return false;
		}

		private bool _updateChkNextCell(object data)
		{
			if (this._clsMapManager.IsNextFinal())
			{
				if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						RetentionData.SetData(BattleUtils.GetRetentionDataMapOpen(SortieBattleTaskManager.GetMapManager(), this._clsResultModel));
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					});
				}
				return true;
			}
			if (BattleTaskManager.GetRootType() == Generics.BattleRootType.Rebellion && BattleTaskManager.GetBattleManager().ChangeableDeck && BattleTaskManager.GetBattleManager().Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !BattleTaskManager.GetBattleManager().Ships_f[0].HasRecoverYouin() && !BattleTaskManager.GetBattleManager().Ships_f[0].HasRecoverMegami())
			{
				BattleTaskManager.ReqPhase(BattlePhase.AdvancingWithdrawal);
				return true;
			}
			if (BattleTaskManager.GetBattleManager().Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !ShipUtils.HasRepair(this._clsResultModel.Ships_f[0]))
			{
				BattleTaskManager.ReqPhase(BattlePhase.FlagshipWreck);
				return true;
			}
			BattleTaskManager.ReqPhase(BattlePhase.EscortShipEvacuation);
			return true;
		}
	}
}
