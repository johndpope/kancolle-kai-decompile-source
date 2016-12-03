using Server_Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server_Common.Formats.Battle
{
	[DataContract]
	public class BattleHeaderItem
	{
		[DataMember]
		public int Deck_Id;

		[DataMember]
		public BattleShipFmt[] Ships;

		public BattleHeaderItem()
		{
		}

		public BattleHeaderItem(int deck_id, List<Mem_ship> sortieShips)
		{
			this.Ships = new BattleShipFmt[6];
			this.Deck_Id = deck_id;
			for (int i = 0; i < sortieShips.get_Count(); i++)
			{
				this.Ships[i] = new BattleShipFmt(sortieShips.get_Item(i));
			}
		}
	}
}
