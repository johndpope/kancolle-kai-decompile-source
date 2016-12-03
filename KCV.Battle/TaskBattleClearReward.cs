using KCV.Battle.Utils;
using KCV.Production;
using local.models;
using local.models.battle;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UniRx;
using UnityEngine;

namespace KCV.Battle
{
	public class TaskBattleClearReward : BaseBattleTask
	{
		private Transform _prefabProdRewardGet;

		private Transform _prefabProdReceiveShip;

		private BattleResultModel _clsBattleResult;

		private ProdBattleReceiveShip _prodReceiveShip;

		private Reward_Ship[] _clsRewardShips;

		private List<IReward> _listRewardModels;

		protected override bool Init()
		{
			this._clsState = new StatementMachine();
			this._clsBattleResult = BattleTaskManager.GetBattleManager().GetBattleResult();
			this._listRewardModels = this._clsBattleResult.GetRewardItems();
			if (this._listRewardModels.get_Count() > 0)
			{
				for (int i = 0; i < this._listRewardModels.get_Count(); i++)
				{
					if (this._listRewardModels.get_Item(i) is IReward_Ship)
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initShipGet), new StatementMachine.StatementMachineUpdate(this._updateShipGet));
					}
					if (this._listRewardModels.get_Item(i) is IReward_Slotitem)
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initSlotItemGet), new StatementMachine.StatementMachineUpdate(this._updateSlotItemGet));
					}
					if (this._listRewardModels.get_Item(i) is IReward_Useitem)
					{
						this._clsState.AddState(new StatementMachine.StatementMachineInitialize(this._initUseItemGet), new StatementMachine.StatementMachineUpdate(this._updateUseItemGet));
					}
				}
			}
			else
			{
				BattleTaskManager.ReqPhase(BattleUtils.NextPhase(BattlePhase.ClearReward));
				base.ImmediateTermination();
			}
			return true;
		}

		protected override bool UnInit()
		{
			base.UnInit();
			Mem.DelListSafe<IReward>(ref this._listRewardModels);
			this._clsBattleResult = null;
			return true;
		}

		protected override bool Update()
		{
			this._clsState.OnUpdate(Time.get_deltaTime());
			return this.ChkChangePhase(BattlePhase.ClearReward);
		}

		private bool _initShipGet(object data)
		{
			Observable.FromCoroutine(new Func<IEnumerator>(this.PlayShipGet), false).Subscribe<Unit>();
			return false;
		}

		private bool _updateShipGet(object data)
		{
			return true;
		}

		[DebuggerHidden]
		private IEnumerator PlayShipGet()
		{
			TaskBattleClearReward.<PlayShipGet>c__IteratorF6 <PlayShipGet>c__IteratorF = new TaskBattleClearReward.<PlayShipGet>c__IteratorF6();
			<PlayShipGet>c__IteratorF.<>f__this = this;
			return <PlayShipGet>c__IteratorF;
		}

		[DebuggerHidden]
		private IEnumerator PlayReceiveShip()
		{
			TaskBattleClearReward.<PlayReceiveShip>c__IteratorF7 <PlayReceiveShip>c__IteratorF = new TaskBattleClearReward.<PlayReceiveShip>c__IteratorF7();
			<PlayReceiveShip>c__IteratorF.<>f__this = this;
			return <PlayReceiveShip>c__IteratorF;
		}

		private bool _initSlotItemGet(object data)
		{
			return false;
		}

		private bool _updateSlotItemGet(object data)
		{
			return false;
		}

		private bool _initUseItemGet(object data)
		{
			return false;
		}

		private bool _updateUseItemGet(object data)
		{
			return false;
		}
	}
}
