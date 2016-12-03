using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_BattleStart : __ShipModel_Battle__
	{
		public ShipModel_BattleStart(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex) : base(mst_data, baseData, hp, slotitems, slotitemex)
		{
		}
	}
}
