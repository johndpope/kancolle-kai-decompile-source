using Common.Enum;
using Common.Struct;
using local.models;
using local.utils;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace local.managers
{
	public abstract class RemodelManagerBase : ManagerBase
	{
		private Api_req_Kaisou _api;

		private int _area_id;

		private List<ShipModel> _other_ships;

		private Dictionary<int, List<SlotitemModel>> _unset_slotitems;

		private int _hokyo_zousetsu_num;

		public int AreaId
		{
			get
			{
				return this._area_id;
			}
		}

		public int HokyoZousetsuNum
		{
			get
			{
				return this._hokyo_zousetsu_num;
			}
		}

		public RemodelManagerBase(int area_id)
		{
			this._area_id = area_id;
			this._api = new Api_req_Kaisou();
			this._UpdateOtherShips();
			this._hokyo_zousetsu_num = new UseitemUtil().GetCount(64);
		}

		public DeckModel[] GetDecks()
		{
			return base.UserInfo.GetDecksFromArea(this._area_id);
		}

		public ShipModel[] GetOtherShipList()
		{
			return this._other_ships.ToArray();
		}

		public ShipModel[] GetOtherShipList(int page_no, int count_in_page)
		{
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, this._other_ships.get_Count());
			int num2 = this._other_ships.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return this._other_ships.GetRange(num, num2).ToArray();
		}

		public ShipModel[] GetOtherShipList(int page_no, int count_in_page, out int count)
		{
			count = this.GetOtherShipCount();
			return this.GetOtherShipList(page_no, count_in_page);
		}

		public int GetOtherShipCount()
		{
			return this._other_ships.get_Count();
		}

		public bool IsValidShip(ShipModel ship)
		{
			return ship != null && !ship.IsInRepair() && !ship.IsInMission() && !ship.IsBling() && !ship.IsInActionEndDeck();
		}

		public bool IsValidGradeUp(ShipModel ship)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(ship.MstId);
			return mst_ship.Aftershipid > 0 && ship.Level >= mst_ship.Afterlv;
		}

		public SlotitemModel[] GetSlotitemList(int ship_mem_id, SlotitemCategory category)
		{
			return this._GetSlotitemList(ship_mem_id, category).ToArray();
		}

		public SlotitemModel[] GetSlotitemList(int ship_mem_id, SlotitemCategory category, int page_no, int count_in_page, out int count)
		{
			List<SlotitemModel> list = this._GetSlotitemList(ship_mem_id, category);
			count = list.get_Count();
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, list.get_Count());
			int num2 = list.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return list.GetRange(num, num2).ToArray();
		}

		public SlotSetChkResult_Slot IsValidChangeSlotitem(int ship_mem_id, int slotitem_id, int slot_index)
		{
			return this._api.IsValidSlotSet(ship_mem_id, slotitem_id, slot_index);
		}

		public SlotSetChkResult_Slot ChangeSlotitem(int ship_mem_id, int slotitem_id, int slot_index)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = this._api.SlotSet(ship_mem_id, slotitem_id, slot_index);
			this._unset_slotitems = null;
			return api_Result.data;
		}

		public bool IsValidUnsetSlotitem(int ship_mem_id, int slot_index)
		{
			SlotSetChkResult_Slot slotSetChkResult_Slot = this._api.IsValidSlotSet(ship_mem_id, -1, slot_index);
			return slotSetChkResult_Slot == SlotSetChkResult_Slot.Ok || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUse || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUseHighCost;
		}

		public bool UnsetSlotitem(int ship_mem_id, int slot_index)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = this._api.SlotSet(ship_mem_id, -1, slot_index);
			if (api_Result.state != Api_Result_State.Success)
			{
				return false;
			}
			this._unset_slotitems = null;
			return true;
		}

		public bool IsValidUnsetAll(int ship_mem_id)
		{
			ShipModel ship = base.UserInfo.GetShip(ship_mem_id);
			return ship != null && this.IsValidShip(ship) && ship.GetSlotitemEquipCount() > 0;
		}

		public bool UnsetAll(int ship_mem_id)
		{
			Api_Result<Hashtable> api_Result = this._api.Unslot_all(ship_mem_id);
			if (api_Result.state != Api_Result_State.Success)
			{
				return false;
			}
			this._unset_slotitems = null;
			return true;
		}

		public SlotSetInfo GetSlotitemInfoToChange(int ship_mem_id, int slotitem_id, int slot_index)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_mem_id);
			return mem_ship.GetSlotSetParam(slot_index, slotitem_id);
		}

		public SlotSetInfo GetSlotitemInfoToUnset(int ship_mem_id, int slot_index)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_mem_id);
			return mem_ship.GetSlotSetParam(slot_index, -1);
		}

		public bool SlotLock(int slot_mem_id)
		{
			Api_Result<bool> api_Result = this._api.SlotLockChange(slot_mem_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public SlotitemModel[] GetSlotitemExList(int ship_mem_id)
		{
			return this._GetSlotitemExList(ship_mem_id).ToArray();
		}

		public SlotitemModel[] GetSlotitemExList(int ship_mem_id, int page_no, int count_in_page, out int count)
		{
			List<SlotitemModel> list = this._GetSlotitemExList(ship_mem_id);
			count = list.get_Count();
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, list.get_Count());
			int num2 = list.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return list.GetRange(num, num2).ToArray();
		}

		public SlotSetChkResult_Slot IsValidChangeSlotitemEx(int ship_mem_id, int slotitem_id)
		{
			return this._api.IsValidSlotSet(ship_mem_id, slotitem_id);
		}

		public SlotSetChkResult_Slot ChangeSlotitemEx(int ship_mem_id, int slotitem_id)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = this._api.SlotSet(ship_mem_id, slotitem_id);
			this._unset_slotitems = null;
			return api_Result.data;
		}

		public bool IsValidUnsetSlotitemEx(int ship_mem_id)
		{
			SlotSetChkResult_Slot slotSetChkResult_Slot = this._api.IsValidSlotSet(ship_mem_id, -1);
			return slotSetChkResult_Slot == SlotSetChkResult_Slot.Ok || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUse || slotSetChkResult_Slot == SlotSetChkResult_Slot.OkBauxiteUseHighCost;
		}

		public bool UnsetSlotitemEx(int ship_mem_id)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = this._api.SlotSet(ship_mem_id, -1);
			if (api_Result.state != Api_Result_State.Success)
			{
				return false;
			}
			this._unset_slotitems = null;
			return true;
		}

		public bool IsValidOpenSlotEx(int ship_mem_id)
		{
			return this.HokyoZousetsuNum != 0 && this._api.IsExpandSlotShip(ship_mem_id);
		}

		public bool OpenSlotEx(int ship_mem_id)
		{
			if (!this.IsValidOpenSlotEx(ship_mem_id))
			{
				return false;
			}
			Api_Result<Mem_ship> api_Result = this._api.ExpandSlot(ship_mem_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				this._hokyo_zousetsu_num--;
				return true;
			}
			return false;
		}

		public void ClearUnsetSlotsCache()
		{
			this._unset_slotitems = null;
		}

		protected void _UpdateOtherShips()
		{
			List<ShipModel> all_ships = base.UserInfo.__GetShipList__();
			this._other_ships = base.GetAreaShips(this.AreaId, false, true, all_ships);
			if (this._area_id == 1)
			{
				this._other_ships.AddRange(base.GetDepotShips(all_ships));
			}
			this._other_ships = DeckUtil.GetSortedList(this._other_ships, SortKey.LEVEL);
		}

		private void _UpdateUnsetSlotitems()
		{
			this._unset_slotitems = new Dictionary<int, List<SlotitemModel>>();
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success)
			{
				using (Dictionary<int, Mem_slotitem>.KeyCollection.Enumerator enumerator = api_Result.data.get_Keys().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						SlotitemModel slotitemModel = new SlotitemModel(api_Result.data.get_Item(current));
						if (!slotitemModel.IsEauiped())
						{
							if (!this._unset_slotitems.ContainsKey(slotitemModel.Type3))
							{
								this._unset_slotitems.set_Item(slotitemModel.Type3, new List<SlotitemModel>());
							}
							this._unset_slotitems.get_Item(slotitemModel.Type3).Add(slotitemModel);
						}
					}
				}
			}
		}

		private ShipModel _GetShip(int ship_mem_id)
		{
			int num = DeckUtil.__IsInDeck__(ship_mem_id, false);
			if (num == -1)
			{
				return this._other_ships.Find((ShipModel item) => item.MemId == ship_mem_id);
			}
			return base.UserInfo.GetDeck(num).GetShipFromMemId(ship_mem_id);
		}

		private List<SlotitemModel> _GetSlotitemList(int ship_id, SlotitemCategory category)
		{
			List<SlotitemModel> list = new List<SlotitemModel>();
			ShipModel shipModel = this._GetShip(ship_id);
			if (shipModel == null)
			{
				return list;
			}
			if (this._unset_slotitems == null)
			{
				this._UpdateUnsetSlotitems();
			}
			List<int> equipList = Mst_DataManager.Instance.Mst_ship.get_Item(shipModel.MstId).GetEquipList();
			List<int> list2;
			if (category == SlotitemCategory.None)
			{
				list2 = equipList;
			}
			else
			{
				list2 = Enumerable.ToList<int>(Enumerable.Select<KeyValuePair<int, Mst_equip_category>, int>(Enumerable.Where<KeyValuePair<int, Mst_equip_category>>(Mst_DataManager.Instance.Mst_equip_category, (KeyValuePair<int, Mst_equip_category> pair) => pair.get_Value().Slotitem_type == category), (KeyValuePair<int, Mst_equip_category> pair) => pair.get_Key()));
				list2 = Enumerable.ToList<int>(Enumerable.Intersect<int>(list2, equipList));
			}
			for (int i = 0; i < list2.get_Count(); i++)
			{
				int num = list2.get_Item(i);
				if (this._unset_slotitems.ContainsKey(num))
				{
					list.AddRange(this._unset_slotitems.get_Item(num));
				}
			}
			return list;
		}

		private List<SlotitemModel> _GetSlotitemExList(int ship_mem_id)
		{
			List<SlotitemModel> list = new List<SlotitemModel>();
			ShipModel shipModel = this._GetShip(ship_mem_id);
			if (shipModel == null)
			{
				return list;
			}
			if (this._unset_slotitems == null)
			{
				this._UpdateUnsetSlotitems();
			}
			List<int> list2 = new List<int>();
			list2.Add(23);
			list2.Add(43);
			list2.Add(44);
			List<int> list3 = list2;
			List<int> equipList = Mst_DataManager.Instance.Mst_ship.get_Item(shipModel.MstId).GetEquipList();
			list3 = Enumerable.ToList<int>(Enumerable.Intersect<int>(list3, equipList));
			for (int i = 0; i < list3.get_Count(); i++)
			{
				int num = list3.get_Item(i);
				if (this._unset_slotitems.ContainsKey(num))
				{
					list.AddRange(this._unset_slotitems.get_Item(num));
				}
			}
			return list;
		}
	}
}
