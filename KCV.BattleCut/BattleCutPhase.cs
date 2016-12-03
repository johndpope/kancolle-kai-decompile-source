using System;

namespace KCV.BattleCut
{
	public enum BattleCutPhase
	{
		BattleCutPhase_ST,
		BattleCutPhase_BEF = -1,
		BattleCutPhase_NONE = -1,
		Formation,
		Command,
		DayBattle,
		WithdrawalDecision,
		Judge,
		Result,
		AdvancingWithdrawal,
		AdvancingWithdrawalDC,
		ClearReward,
		MapOpen,
		FlagshipWreck,
		EscortShipEvacuation,
		NightBattle,
		Battle_End,
		BattleCutPhase_AFT,
		BattleCutPhase_NUM = 14,
		BattleCutPhase_ED = 13
	}
}
