using Server_Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server_Common.Formats.Battle
{
	[DataContract]
	public class NightBattleFmt
	{
		[DataMember]
		public BattleHeader Header;

		[DataMember]
		public int F_TouchPlane;

		[DataMember]
		public int E_TouchPlane;

		[DataMember]
		public int F_FlareId;

		[DataMember]
		public int E_FlareId;

		[DataMember]
		public int F_SearchId;

		[DataMember]
		public int E_SearchId;

		[DataMember]
		public List<Hougeki<BattleAtackKinds_Night>> Hougeki;

		public NightBattleFmt()
		{
		}

		public NightBattleFmt(int deck_id, List<Mem_ship> myShip, List<Mem_ship> enemyShip)
		{
			BattleHeaderItem f_DeckShip = new BattleHeaderItem(deck_id, myShip);
			BattleHeaderItem e_DeckShip = new BattleHeaderItem(0, enemyShip);
			this.Header = new BattleHeader(f_DeckShip, e_DeckShip);
			this.Hougeki = new List<Hougeki<BattleAtackKinds_Night>>();
		}
	}
}
