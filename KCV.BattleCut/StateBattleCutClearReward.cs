using KCV.Production;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutClearReward : BaseBattleCutState
	{
		private Transform _prefabProdRewardGet;

		private Transform _prefabProdReceiveShip;

		private BattleResultModel _clsResult;

		private ProdCutReceiveShip _prodReceiveShip;

		private StatementMachine _clsState;

		private List<Reward_Ship> _listRewardShips;

		private List<IReward> _listRewardModels;

		public override bool Init(object data)
		{
			this._clsState = new StatementMachine();
			this._clsResult = BattleCutManager.GetBattleManager().GetBattleResult();
			this._listRewardModels = new List<IReward>(this._clsResult.GetRewardItems());
			if (this._listRewardModels.get_Count() > 0)
			{
				this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this.InitShipGet), new StatementMachine.StatementMachineUpdate(this.UpdateShipGet));
			}
			else
			{
				BattleCutManager.ReqPhase(BattleCutPhase.MapOpen);
			}
			return false;
		}

		public override bool Terminate(object data)
		{
			this._clsResult = null;
			this._prodReceiveShip = null;
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			this._clsState = null;
			Mem.DelListSafe<Reward_Ship>(ref this._listRewardShips);
			Mem.DelListSafe<IReward>(ref this._listRewardModels);
			return false;
		}

		public override bool Run(object data)
		{
			if (this._clsState != null)
			{
				this._clsState.OnUpdate(Time.get_deltaTime());
			}
			return base.IsCheckPhase(BattleCutPhase.ClearReward);
		}

		private bool InitShipGet(object data)
		{
			Observable.FromCoroutine(new Func<IEnumerator>(this.PlayShipGet), false).Subscribe<Unit>();
			return false;
		}

		private bool UpdateShipGet(object data)
		{
			return false;
		}

		[DebuggerHidden]
		private IEnumerator PlayShipGet()
		{
			StateBattleCutClearReward.<PlayShipGet>c__Iterator112 <PlayShipGet>c__Iterator = new StateBattleCutClearReward.<PlayShipGet>c__Iterator112();
			<PlayShipGet>c__Iterator.<>f__this = this;
			return <PlayShipGet>c__Iterator;
		}

		[DebuggerHidden]
		private IEnumerator PlayReceiveShip()
		{
			StateBattleCutClearReward.<PlayReceiveShip>c__Iterator113 <PlayReceiveShip>c__Iterator = new StateBattleCutClearReward.<PlayReceiveShip>c__Iterator113();
			<PlayReceiveShip>c__Iterator.<>f__this = this;
			return <PlayReceiveShip>c__Iterator;
		}

		private bool InitSlotItemGet(object data)
		{
			return false;
		}

		private bool UpdateSlotItemGet(object data)
		{
			return false;
		}

		private bool InitUseItemGet(object data)
		{
			return false;
		}

		private bool UpdateUseItemGet(object data)
		{
			return false;
		}
	}
}
