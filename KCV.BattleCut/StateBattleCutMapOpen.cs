using Common.Enum;
using KCV.Battle.Production;
using KCV.Battle.Utils;
using KCV.SortieBattle;
using local.managers;
using local.models.battle;
using Server_Models;
using System;
using System.Collections;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutMapOpen : BaseBattleCutState
	{
		private BattleResultModel _clsResultModel;

		private MapManager _clsMapManger;

		private StatementMachine _clsState;

		private ProdMapOpen _prodMapOpen;

		private ProdMapClear _prodMapClear;

		private ProdThalassocracy _prodThalassocracy;

		private ProdShortRewardGet _prodShortRewardGet;

		private KeyControl _clsInput;

		public override bool Init(object data)
		{
			this._clsResultModel = BattleCutManager.GetBattleManager().GetBattleResult();
			this._clsMapManger = BattleCutManager.GetMapManager();
			this._clsInput = BattleCutManager.GetKeyControl();
			this._clsState = new StatementMachine();
			if (BattleCutManager.GetBattleType() == Generics.BattleRootType.Rebellion)
			{
				if (this._clsMapManger.IsNextFinal())
				{
					if (this._clsResultModel.WinRank == BattleWinRankKinds.B || this._clsResultModel.WinRank == BattleWinRankKinds.A || this._clsResultModel.WinRank == BattleWinRankKinds.S)
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initThalassocracyProd), new StatementMachine.StatementMachineUpdate(this._updateThalassocracyProd));
					}
					else
					{
						this.ChkNextCell();
					}
				}
				else
				{
					this.ChkNextCell();
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
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitMapOpen), new StatementMachine.StatementMachineUpdate(this.UpdateMapOpen));
			}
			else if (this._clsResultModel.GetAreaRewardItems() != null)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShortRewardGet), new StatementMachine.StatementMachineUpdate(this.UpdateShortRewardGet));
			}
			else
			{
				this.ChkNextCell();
			}
			return false;
		}

		public override bool Terminate(object data)
		{
			return base.Terminate(data);
		}

		public override bool Run(object data)
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return base.IsCheckPhase(BattleCutPhase.MapOpen);
		}

		private bool _initThalassocracyProd(object data)
		{
			this._prodThalassocracy = ProdThalassocracy.Instantiate(PrefabFile.Load<ProdThalassocracy>(PrefabFileInfos.Thalassocracy), BattleCutManager.GetSharedPlase(), this._clsInput, SortieBattleTaskManager.GetMapManager(), this._clsResultModel, BattleCutManager.GetBattleManager().Ships_f, 120);
			this._prodThalassocracy.Play(new Action(this._onThalassocracyProdFinished), BattleCutManager.GetBattleType(), BattleCutManager.GetBattleManager().Map.Name);
			return false;
		}

		private bool _updateThalassocracyProd(object data)
		{
			return this._prodThalassocracy.Run();
		}

		private void _onThalassocracyProdFinished()
		{
			if (this._clsResultModel.NewOpenMapIDs.Length > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitMapOpen), new StatementMachine.StatementMachineUpdate(this.UpdateMapOpen));
			}
			else
			{
				this.ChkNextCell();
			}
			this._prodThalassocracy.Discard();
		}

		private bool _initMapClearProd(object data)
		{
			this._prodMapClear = ProdMapClear.Instantiate(PrefabFile.Load<ProdMapClear>(PrefabFileInfos.MapClear), BattleCutManager.GetSharedPlase(), this._clsInput, BattleCutManager.GetBattleManager().Ships_f[0], 120);
			this._prodMapClear.Play(new Action(this._onMapClearProdFinished));
			return false;
		}

		private bool _updateMapClearProd(object data)
		{
			return this._prodMapClear.Run();
		}

		private void _onMapClearProdFinished()
		{
			if (this._clsResultModel.NewOpenMapIDs.Length > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitMapOpen), new StatementMachine.StatementMachineUpdate(this.UpdateMapOpen));
			}
			else
			{
				this.ChkNextCell();
			}
			this._prodMapClear.Discard();
		}

		private bool InitMapOpen(object data)
		{
			this._prodMapOpen = ProdMapOpen.Instantiate(PrefabFile.Load<ProdMapOpen>(PrefabFileInfos.MapOpen), this._clsResultModel, BattleCutManager.GetSharedPlase(), BattleCutManager.GetKeyControl(), BattleCutManager.GetMapManager(), 120);
			this._prodMapOpen.Play(delegate
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShortRewardGet), new StatementMachine.StatementMachineUpdate(this.UpdateShortRewardGet));
			});
			return false;
		}

		private bool UpdateMapOpen(object data)
		{
			return this._prodMapOpen.Run();
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
			StateBattleCutMapOpen.<CreateShortRewardGet>c__Iterator115 <CreateShortRewardGet>c__Iterator = new StateBattleCutMapOpen.<CreateShortRewardGet>c__Iterator115();
			<CreateShortRewardGet>c__Iterator.observer = observer;
			<CreateShortRewardGet>c__Iterator.<$>observer = observer;
			<CreateShortRewardGet>c__Iterator.<>f__this = this;
			return <CreateShortRewardGet>c__Iterator;
		}

		private void OnShortRewardGetFinished()
		{
			Mem.DelComponentSafe<ProdShortRewardGet>(ref this._prodShortRewardGet);
			this.ChkNextCell();
		}

		private void ChkNextCell()
		{
			if (this._clsMapManger.IsNextFinal())
			{
				if (SingletonMonoBehaviour<FadeCamera>.Instance != null)
				{
					SingletonMonoBehaviour<FadeCamera>.Instance.SetActive(true);
					SingletonMonoBehaviour<FadeCamera>.Instance.isDrawNowLoading = false;
					SingletonMonoBehaviour<FadeCamera>.Instance.FadeOut(0.2f, delegate
					{
						SingletonMonoBehaviour<SoundManager>.Instance.soundVolume.BGM = BattleDefines.SOUND_KEEP.BGMVolume;
						SingletonMonoBehaviour<SoundManager>.Instance.rawBGMVolume = BattleDefines.SOUND_KEEP.BGMVolume;
						Mst_DataManager.Instance.PurgeUIBattleMaster();
						RetentionData.SetData(BattleUtils.GetRetentionDataMapOpen(BattleCutManager.GetMapManager(), this._clsResultModel));
						SingletonMonoBehaviour<AppInformation>.Instance.NextLoadScene = Generics.Scene.Strategy;
						Application.LoadLevel(Generics.Scene.LoadingScene.ToString());
					});
				}
				return;
			}
			if (BattleCutManager.GetBattleType() == Generics.BattleRootType.Rebellion && BattleCutManager.GetBattleManager().ChangeableDeck && BattleCutManager.GetBattleManager().Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !BattleCutManager.GetBattleManager().Ships_f[0].HasRecoverMegami() && !BattleCutManager.GetBattleManager().Ships_f[0].HasRecoverYouin())
			{
				BattleCutManager.ReqPhase(BattleCutPhase.AdvancingWithdrawal);
				return;
			}
			if (this._clsResultModel.Ships_f[0].DmgStateEnd == DamageState_Battle.Taiha && !ShipUtils.HasRepair(this._clsResultModel.Ships_f[0]))
			{
				BattleCutManager.ReqPhase(BattleCutPhase.FlagshipWreck);
				return;
			}
			BattleCutManager.ReqPhase(BattleCutPhase.EscortShipEvacuation);
		}
	}
}
