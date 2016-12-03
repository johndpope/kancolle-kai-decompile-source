using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class DayBattleFmt
	{
		public BattleHeader Header;

		public SearchInfo[] Search;

		public DayBattleProductionFmt OpeningProduction;

		public AirBattle AirBattle;

		public AirBattle AirBattle2;

		public SupportAtack SupportAtack;

		public Raigeki OpeningAtack;

		public List<FromMiddleBattleDayData> FromMiddleDayBattle;

		public Raigeki Raigeki;

		public bool ValidMidnight;

		public DayBattleFmt()
		{
		}

		public DayBattleFmt(int deck_id, List<Mem_ship> myShip, List<Mem_ship> enemyShip)
		{
			this.ValidMidnight = false;
			this.Search = new SearchInfo[2];
			BattleHeaderItem f_DeckShip = new BattleHeaderItem(deck_id, myShip);
			BattleHeaderItem e_DeckShip = new BattleHeaderItem(0, enemyShip);
			this.Header = new BattleHeader(f_DeckShip, e_DeckShip);
		}
	}
}
