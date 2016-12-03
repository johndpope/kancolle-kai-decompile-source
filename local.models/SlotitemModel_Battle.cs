using Server_Controllers.BattleLogic;
using Server_Models;
using System;

namespace local.models
{
	public class SlotitemModel_Battle : SlotitemModel_Mst, ISlotitemModel
	{
		public SlotitemModel_Battle(int mst_id) : base(mst_id)
		{
		}

		public SlotitemModel_Battle(Mst_slotitem mst) : base(mst)
		{
		}

		public bool IsPlaneAtKouku()
		{
			return FighterInfo.ValidFighter(this._mst);
		}
	}
}
