using Server_Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server_Common.Formats.Battle
{
	[DataContract]
	public class BattleHeader
	{
		[DataMember]
		public BattleHeaderItem F_DeckShip1;

		[DataMember]
		public BattleHeaderItem E_DeckShip1;

		public Dictionary<int, List<Mst_slotitem>> UseRationShips;

		public BattleHeader()
		{
		}

		public BattleHeader(BattleHeaderItem F_DeckShip, BattleHeaderItem E_DeckShip)
		{
			this.F_DeckShip1 = F_DeckShip;
			this.E_DeckShip1 = E_DeckShip;
		}
	}
}
