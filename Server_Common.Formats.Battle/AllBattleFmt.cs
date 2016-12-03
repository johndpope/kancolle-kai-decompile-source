using Common.Enum;
using System;

namespace Server_Common.Formats.Battle
{
	public class AllBattleFmt
	{
		public BattleFormationKinds1[] Formation;

		public BattleFormationKinds2 BattleFormation;

		public DayBattleFmt DayBattle;

		public NightBattleFmt NightBattle;

		private AllBattleFmt()
		{
			this.Formation = new BattleFormationKinds1[2];
		}

		public AllBattleFmt(BattleFormationKinds1 fFormation, BattleFormationKinds1 eFormation, BattleFormationKinds2 battleFormation) : this()
		{
			this.Formation[0] = fFormation;
			this.Formation[1] = eFormation;
			this.BattleFormation = battleFormation;
		}
	}
}
