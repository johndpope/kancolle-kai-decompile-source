using KCV.Battle.Production;
using KCV.Battle.Utils;
using Librarys.State;
using local.models.battle;
using System;

namespace KCV.Battle
{
	public class BaseBattleTask : Task
	{
		protected StatementMachine _clsState;

		protected override void Dispose(bool isDisposing)
		{
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
		}

		protected override bool UnInit()
		{
			if (this._clsState != null)
			{
				this._clsState.Clear();
			}
			Mem.Del<StatementMachine>(ref this._clsState);
			return true;
		}

		protected virtual void PlayProdDamage(IBattlePhase iBattlePhase, Action onFinished)
		{
			new ProdDamage().Play(iBattlePhase, onFinished);
		}

		protected virtual void EndPhase(BattlePhase iPhase)
		{
			BattleTaskManager.ReqPhase(iPhase);
		}

		protected virtual bool ChkChangePhase(BattlePhase iPhase)
		{
			return BattleTaskManager.GetPhase() == BattlePhase.BattlePhase_BEF || BattleTaskManager.GetPhase() == iPhase;
		}
	}
}
