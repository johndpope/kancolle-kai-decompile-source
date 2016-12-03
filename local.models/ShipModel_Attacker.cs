using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_Attacker : __ShipModel_Battle__
	{
		public ShipModel_Attacker(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex) : base(mst_data, baseData, hp, slotitems, slotitemex)
		{
		}

		public bool HasRocket()
		{
			SlotitemModel_Battle slotitemModel_Battle = this._slotitems.Find((SlotitemModel_Battle item) => item != null && item.Type3 == 37);
			return slotitemModel_Battle != null;
		}

		public bool HasMihari()
		{
			SlotitemModel_Battle slotitemModel_Battle = this._slotitems.Find((SlotitemModel_Battle item) => item != null && item.MstId == 129);
			return slotitemModel_Battle != null;
		}
	}
}
