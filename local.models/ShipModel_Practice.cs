using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_Practice : __ShipModelMem__
	{
		private List<ISlotitemModel> _slotitems;

		public ISlotitemModel[] Slotitems
		{
			get
			{
				return this._slotitems.ToArray();
			}
		}

		public ShipModel_Practice(Mem_ship mem_ship, List<Mst_slotitem> slotitems) : base(mem_ship)
		{
			this._slotitems = new List<ISlotitemModel>();
			for (int i = 0; i < slotitems.get_Count(); i++)
			{
				Mst_slotitem mst = slotitems.get_Item(i);
				ISlotitemModel slotitemModel = new SlotitemModel_Battle(mst);
				this._slotitems.Add(slotitemModel);
			}
			while (this._slotitems.get_Count() < this.SlotCount)
			{
				this._slotitems.Add(null);
			}
		}

		public override string ToString()
		{
			string text = "Eq (";
			for (int i = 0; i < this.SlotCount; i++)
			{
				if (this.Slotitems[i] == null)
				{
					text += "[--(MstId:- MemId:-)]";
				}
				else
				{
					text += this.Slotitems[i];
				}
				text += ((i >= this.SlotCount - 1) ? string.Empty : ", ");
			}
			text += ")\n";
			return base.ToString(text);
		}
	}
}
