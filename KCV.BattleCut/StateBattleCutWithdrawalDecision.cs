using System;
using UnityEngine;

namespace KCV.BattleCut
{
	public class StateBattleCutWithdrawalDecision : BaseBattleCutState
	{
		private ProdBCWithdrawalDecision _prodBCWithdrawalDecision;

		public override bool Init(object data)
		{
			if (BattleCutManager.GetBattleManager().HasNightBattle())
			{
				this._prodBCWithdrawalDecision = ProdBCWithdrawalDecision.Instantiate(BattleCutManager.GetPrefabFile().prefabProdWithdrawalDecision.GetComponent<ProdBCWithdrawalDecision>(), BattleCutManager.GetSharedPlase());
				this._prodBCWithdrawalDecision.Play(delegate(int index)
				{
					if (index == 1)
					{
						BattleCutManager.GetBattleManager().StartDayToNightBattle();
						BattleCutManager.ReqPhase(BattleCutPhase.NightBattle);
					}
					else
					{
						BattleCutManager.ReqPhase(BattleCutPhase.Judge);
					}
				});
				return false;
			}
			BattleCutManager.ReqPhase(BattleCutPhase.Judge);
			return true;
		}

		public override bool Run(object data)
		{
			if (this._prodBCWithdrawalDecision != null)
			{
				this._prodBCWithdrawalDecision.Run();
			}
			return base.IsCheckPhase(BattleCutPhase.WithdrawalDecision);
		}

		public override bool Terminate(object data)
		{
			if (this._prodBCWithdrawalDecision != null)
			{
				Object.Destroy(this._prodBCWithdrawalDecision.get_gameObject());
			}
			this._prodBCWithdrawalDecision = null;
			return false;
		}
	}
}
