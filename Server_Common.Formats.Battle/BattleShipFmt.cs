using Server_Models;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Server_Common.Formats.Battle
{
	[DataContract]
	public class BattleShipFmt
	{
		[DataMember]
		public int Id;

		[DataMember]
		public int ShipId;

		[DataMember]
		public int Level;

		[DataMember]
		public int NowHp;

		[DataMember]
		public int MaxHp;

		[DataMember]
		public bool EscapeFlag;

		[DataMember]
		public Ship_GrowValues BattleParam;

		[DataMember]
		public List<int> Slot;

		public int ExSlot;

		public BattleShipFmt()
		{
		}

		public BattleShipFmt(Mem_ship ship)
		{
			this.Id = ship.Rid;
			this.ShipId = ship.Ship_id;
			this.Level = ship.Level;
			this.NowHp = ship.Nowhp;
			this.MaxHp = ship.Maxhp;
			this.BattleParam = ship.GetBattleBaseParam().Copy();
			this.EscapeFlag = ship.Escape_sts;
			this.Slot = new List<int>();
			if (!ship.IsEnemy())
			{
				ship.Slot.ForEach(delegate(int x)
				{
					int num = -1;
					if (Comm_UserDatas.Instance.User_slot.ContainsKey(x))
					{
						num = Comm_UserDatas.Instance.User_slot.get_Item(x).Slotitem_id;
					}
					this.Slot.Add(num);
				});
			}
			else
			{
				ship.Slot.ForEach(delegate(int x)
				{
					this.Slot.Add(x);
				});
			}
			Mst_slotitem mstSlotItemToExSlot = ship.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				this.ExSlot = mstSlotItemToExSlot.Id;
			}
		}
	}
}
