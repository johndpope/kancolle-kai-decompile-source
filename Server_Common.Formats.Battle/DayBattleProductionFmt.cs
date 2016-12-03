using Common.Enum;
using System;

namespace Server_Common.Formats.Battle
{
	public class DayBattleProductionFmt
	{
		public int BoxNo;

		public BattleCommand productionKind;

		public bool Withdrawal;

		public int FSPP;

		public int RSPP;

		public int TSPP;
	}
}
