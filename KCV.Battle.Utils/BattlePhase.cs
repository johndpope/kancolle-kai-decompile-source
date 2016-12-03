using System;

namespace KCV.Battle.Utils
{
	public enum BattlePhase
	{
		BattlePhase_ST,
		BattlePhase_BEF = -1,
		BattlePhase_NONE = -1,
		BossInsert,
		FleetAdvent,
		Detection,
		Command,
		AerialCombat,
		AerialCombatSecond,
		SupportingFire,
		OpeningTorpedoSalvo,
		Shelling,
		TorpedoSalvo,
		WithdrawalDecision,
		NightCombat,
		Result,
		FlagshipWreck,
		EscortShipEvacuation,
		AdvancingWithdrawal,
		AdvancingWithdrawalDC,
		ClearReward,
		MapOpen,
		BattlePhase_AFT,
		BattlePhase_NUM = 19,
		BattlePhase_ED = 18
	}
}
