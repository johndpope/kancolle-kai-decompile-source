using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class HougekiDayBattleFmt : IBattleType
	{
		private int fmtType;

		public List<Hougeki<BattleAtackKinds_Day>> AttackData;

		public int FmtType
		{
			get
			{
				return this.fmtType;
			}
		}

		public HougekiDayBattleFmt()
		{
			this.fmtType = 1;
		}
	}
}
