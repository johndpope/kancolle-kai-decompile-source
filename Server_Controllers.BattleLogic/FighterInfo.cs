using Server_Models;
using System;

namespace Server_Controllers.BattleLogic
{
	public class FighterInfo
	{
		public enum FighterKinds
		{
			FIGHTER = 1,
			RAIG,
			BAKU
		}

		public int SlotIdx;

		public Mst_slotitem SlotData;

		public FighterInfo.FighterKinds Kind;

		public int AttackShipPow;

		public FighterInfo(int slotidx, Mst_slotitem mst_slot)
		{
			this.SlotIdx = slotidx;
			this.SlotData = mst_slot;
			this.setKindParam(mst_slot);
		}

		public static bool ValidFighter(Mst_slotitem mst_slot)
		{
			return mst_slot.Api_mapbattle_type3 == 6 || mst_slot.Api_mapbattle_type3 == 7 || mst_slot.Api_mapbattle_type3 == 8 || mst_slot.Api_mapbattle_type3 == 11;
		}

		public static FighterInfo.FighterKinds GetKind(Mst_slotitem mst_slot)
		{
			if (mst_slot.Api_mapbattle_type3 == 6)
			{
				return FighterInfo.FighterKinds.FIGHTER;
			}
			if (mst_slot.Api_mapbattle_type3 == 7 || mst_slot.Api_mapbattle_type3 == 11)
			{
				return FighterInfo.FighterKinds.BAKU;
			}
			if (mst_slot.Api_mapbattle_type3 == 8)
			{
				return FighterInfo.FighterKinds.RAIG;
			}
			return (FighterInfo.FighterKinds)0;
		}

		private void setKindParam(Mst_slotitem mst_slot)
		{
			FighterInfo.FighterKinds kind = FighterInfo.GetKind(mst_slot);
			if (kind == FighterInfo.FighterKinds.FIGHTER)
			{
				this.Kind = kind;
				this.AttackShipPow = 0;
				return;
			}
			if (kind == FighterInfo.FighterKinds.BAKU)
			{
				this.Kind = kind;
				this.AttackShipPow = mst_slot.Baku;
				return;
			}
			if (kind == FighterInfo.FighterKinds.RAIG)
			{
				this.Kind = kind;
				this.AttackShipPow = mst_slot.Raig;
				return;
			}
		}

		public bool ValidTaiku()
		{
			return this.Kind == FighterInfo.FighterKinds.BAKU || this.Kind == FighterInfo.FighterKinds.RAIG;
		}

		public bool ValidBakurai()
		{
			return this.Kind == FighterInfo.FighterKinds.BAKU || this.Kind == FighterInfo.FighterKinds.RAIG;
		}
	}
}
