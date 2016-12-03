using Common.Enum;
using Common.Struct;
using local.managers;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel : __ShipModelMem__, IMemShip
	{
		private List<SlotitemModel> _slotitems;

		private SlotitemModel _slotitem_ex;

		public int RepairTime
		{
			get
			{
				return this._mem_data.GetNdockTimeSpan();
			}
		}

		public List<SlotitemModel> SlotitemList
		{
			get
			{
				if (!this._CheckSlotitemCache())
				{
					this._UpdateSlotitems();
				}
				return this._slotitems.GetRange(0, this._slotitems.get_Count());
			}
		}

		public SlotitemModel SlotitemEx
		{
			get
			{
				if (!this._CheckSlotitemCache())
				{
					this._UpdateSlotitems();
				}
				return this._slotitem_ex;
			}
		}

		public ShipModel(int mem_id) : base(mem_id)
		{
		}

		public ShipModel(Mem_ship mem_ship) : base(mem_ship)
		{
		}

		public MaterialInfo GetResourcesForSupply()
		{
			return new MaterialInfo
			{
				Fuel = this._mem_data.GetRequireChargeFuel(),
				Ammo = this._mem_data.GetRequireChargeBull()
			};
		}

		public MaterialInfo GetResourcesForRepair()
		{
			Dictionary<enumMaterialCategory, int> ndockMaterialNum = this._mem_data.GetNdockMaterialNum();
			return new MaterialInfo(ndockMaterialNum);
		}

		public bool IsMaxTaikyu()
		{
			int num = this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup);
			return num >= 3 || this.MaxHp >= this._mst_data.Taik_max;
		}

		public bool IsMaxKaryoku()
		{
			return this.Karyoku >= this.KaryokuMax;
		}

		public bool IsMaxRaisou()
		{
			return this.Raisou >= this.RaisouMax;
		}

		public bool IsMaxTaiku()
		{
			return this.Taiku >= this.TaikuMax;
		}

		public bool IsMaxSoukou()
		{
			return this.Soukou >= this.SoukouMax;
		}

		public bool IsMaxKaihi()
		{
			return this.Kaihi >= this.KaihiMax;
		}

		public bool IsMaxTaisen()
		{
			return this.Taisen >= this.TaisenMax;
		}

		public bool IsMaxLucky()
		{
			return this.Lucky >= this.LuckyMax;
		}

		public MaterialInfo GetResourcesForDestroy()
		{
			Dictionary<enumMaterialCategory, int> destroyShipMaterials = this._mem_data.getDestroyShipMaterials();
			return new MaterialInfo(destroyShipMaterials);
		}

		public bool HasLocked()
		{
			using (List<SlotitemModel>.Enumerator enumerator = this.SlotitemList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlotitemModel current = enumerator.get_Current();
					if (current != null && current.IsLocked())
					{
						return true;
					}
				}
			}
			return this._slotitem_ex != null && this._slotitem_ex.IsLocked();
		}

		public bool __HasLocked__(Dictionary<int, Mem_slotitem> slotitems)
		{
			if (!this._CheckSlotitemCache())
			{
				this._slotitems = new List<SlotitemModel>();
				for (int i = 0; i < this.SlotCount; i++)
				{
					Mem_slotitem data;
					if (this._mem_data.Slot.get_Item(i) == -1)
					{
						this._slotitems.Add(null);
					}
					else if (!slotitems.TryGetValue(this._mem_data.Slot.get_Item(i), ref data))
					{
						this._slotitems.Add(null);
					}
					else
					{
						this._slotitems.Add(new SlotitemModel(data));
					}
				}
				if (this._mem_data.Exslot > 0)
				{
					Mem_slotitem data2;
					if (!slotitems.TryGetValue(this._mem_data.Exslot, ref data2))
					{
						this._slotitem_ex = null;
					}
					else
					{
						this._slotitem_ex = new SlotitemModel(data2);
					}
				}
				else
				{
					this._slotitem_ex = null;
				}
			}
			return this.HasLocked();
		}

		public int GetSlotitemEquipCount()
		{
			return this._mem_data.Slot.FindAll((int i) => i != -1).get_Count();
		}

		public bool HasExSlot()
		{
			return this._mem_data.IsOpenExSlot();
		}

		public DeckModelBase getDeck()
		{
			return ManagerBase.getDeck(base.MemId);
		}

		public int IsInDeck()
		{
			return this.IsInDeck(true);
		}

		public int IsInDeck(bool search_flag_ship)
		{
			int num = DeckUtil.__IsInDeck__(base.MemId);
			if (!search_flag_ship && num == 0)
			{
				num = 1;
			}
			return num;
		}

		public bool IsInActionEndDeck()
		{
			return this._mem_data.IsActiveEnd();
		}

		public int IsInEscortDeck()
		{
			return ManagerBase.IsInEscortDeck(base.MemId);
		}

		public bool IsInRepair()
		{
			return this._mem_data.ExistsNdock();
		}

		public bool IsInMission()
		{
			return this._mem_data.ExistsMission();
		}

		private bool _CheckSlotitemCache()
		{
			if (this._slotitems == null)
			{
				return false;
			}
			if (this._slotitems.get_Count() != this._mem_data.Slot.get_Count())
			{
				return false;
			}
			for (int i = 0; i < this._slotitems.get_Count(); i++)
			{
				if (this._slotitems.get_Item(i) == null && this._mem_data.Slot.get_Item(i) != -1)
				{
					return false;
				}
				if (this._slotitems.get_Item(i) != null && this._mem_data.Slot.get_Item(i) != this._slotitems.get_Item(i).MemId)
				{
					return false;
				}
			}
			return (this._slotitem_ex != null || this._mem_data.Exslot <= 0) && (this._slotitem_ex == null || this._slotitem_ex.MemId == this._mem_data.Exslot);
		}

		private void _UpdateSlotitems()
		{
			this._slotitems = new List<SlotitemModel>();
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				for (int i = 0; i < this.SlotCount; i++)
				{
					if (this._mem_data.Slot.get_Item(i) == -1)
					{
						this._slotitems.Add(null);
					}
					else
					{
						Mem_slotitem data = api_Result.data.get_Item(this._mem_data.Slot.get_Item(i));
						this._slotitems.Add(new SlotitemModel(data));
					}
				}
				if (this._mem_data.Exslot <= 0)
				{
					this._slotitem_ex = null;
				}
				else
				{
					Mem_slotitem data2 = api_Result.data.get_Item(this._mem_data.Exslot);
					this._slotitem_ex = new SlotitemModel(data2);
				}
			}
			while (this._slotitems.get_Count() < this.SlotCount)
			{
				this._slotitems.Add(null);
			}
		}

		private int _GetMaterialCount(enumMaterialCategory material_type, Dictionary<enumMaterialCategory, int> data)
		{
			if (data == null)
			{
				return 0;
			}
			return data.get_Item(material_type);
		}

		public override string ToString()
		{
			string text = string.Empty;
			if (base.IsBling())
			{
				text += "[回航中] ";
			}
			text += "Eq (";
			for (int i = 0; i < this.SlotCount; i++)
			{
				text += this._ToString(this.SlotitemList.get_Item(i), i);
				text += ((i >= this.SlotCount - 1) ? string.Empty : ", ");
			}
			if (this.HasExSlot())
			{
				text = text + ", " + this._ToString(this.SlotitemEx, -1);
			}
			else
			{
				text += ", [X]";
			}
			text += ")\n";
			return base.ToString(text);
		}

		private string _ToString(SlotitemModel slotitem, int slot_index)
		{
			string text;
			if (slotitem == null)
			{
				text = "[-]";
			}
			else
			{
				text = slotitem.ToString();
			}
			if (slot_index >= 0 && slot_index < this.SlotCount)
			{
				text += string.Format("搭載:{0}/{1} ", base.Tousai[slot_index], base.TousaiMax[slot_index]);
			}
			return text;
		}

		virtual int get_Lov()
		{
			return base.Lov;
		}
	}
}
