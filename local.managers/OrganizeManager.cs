using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;

namespace local.managers
{
	public class OrganizeManager : ManagerBase, IOrganizeManager
	{
		private int _area_id;

		private Api_req_Hensei _api;

		private List<ShipModel> _ships;

		private SortKey _pre_sort_key;

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._area_id);
			}
		}

		public int ShipsCount
		{
			get
			{
				return this._ships.get_Count();
			}
		}

		public SortKey NowSortKey
		{
			get
			{
				return this._pre_sort_key;
			}
		}

		public OrganizeManager(int area_id)
		{
			this._area_id = area_id;
			this._api = new Api_req_Hensei();
			base._CreateMapAreaModel();
			this._UpdateShipList();
		}

		public int GetMamiyaCount()
		{
			return new UseitemUtil().GetCount(54);
		}

		public int GetIrakoCount()
		{
			return new UseitemUtil().GetCount(59);
		}

		public bool ChangeSortKey(SortKey new_sort_key)
		{
			if (this._pre_sort_key != new_sort_key)
			{
				this._pre_sort_key = new_sort_key;
				this._ships = DeckUtil.GetSortedList(this._ships, this._pre_sort_key);
				return true;
			}
			return false;
		}

		public ShipModel[] GetShipList()
		{
			return this._ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			if (page_no < 1 || this.ShipsCount / count_in_page + 1 < page_no)
			{
				return new ShipModel[0];
			}
			int num = (page_no - 1) * count_in_page;
			num = Math.Max(num, 0);
			num = Math.Min(num, this._ships.get_Count());
			int num2 = this._ships.get_Count() - num;
			num2 = Math.Max(num2, 0);
			num2 = Math.Min(num2, count_in_page);
			return this._ships.GetRange(num, num2).ToArray();
		}

		public bool IsValidShip(int ship_mem_id)
		{
			ShipModel ship = base.UserInfo.GetShip(ship_mem_id);
			if (ship.IsBling())
			{
				return false;
			}
			if (ship.IsInMission())
			{
				return false;
			}
			DeckModelBase deck = ship.getDeck();
			if (deck != null)
			{
				if (deck.IsEscortDeckMyself())
				{
					return false;
				}
				if (deck.IsActionEnd())
				{
					return false;
				}
			}
			return true;
		}

		public bool IsValidChange(int deck_id, int selected_index, int ship_mem_id)
		{
			return this._api.IsValidChange(deck_id, selected_index, ship_mem_id);
		}

		public bool ChangeOrganize(int deck_id, int selected_index, int ship_mem_id)
		{
			Api_Result<Hashtable> api_Result = this._api.Change(deck_id, selected_index, ship_mem_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				Api_get_Member api_get_mem = new Api_get_Member();
				base.UserInfo.__UpdateDeck__(api_get_mem);
				this._UpdateShipList();
				return true;
			}
			return false;
		}

		public bool IsValidUnset(int ship_mem_id)
		{
			int num = DeckUtil.__IsInDeck__(ship_mem_id, false);
			if (num == -1)
			{
				return false;
			}
			int shipIndex = base.UserInfo.GetDeck(num).GetShipIndex(ship_mem_id);
			return this._api.IsValidChange(num, shipIndex, -1);
		}

		public bool UnsetOrganize(int deck_id, int selected_index)
		{
			Api_Result<Hashtable> api_Result = this._api.Change(deck_id, selected_index, -1);
			if (api_Result.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateDeck__(new Api_get_Member());
				this._UpdateShipList();
				return true;
			}
			return false;
		}

		public bool IsValidUnsetAll(int deck_id)
		{
			return this._api.IsValidChange(deck_id, 0, -2);
		}

		public bool UnsetAllOrganize(int deck_id)
		{
			Api_Result<Hashtable> api_Result = this._api.Change(deck_id, 0, -2);
			if (api_Result.state == Api_Result_State.Success)
			{
				base.UserInfo.__UpdateDeck__(new Api_get_Member());
				this._UpdateShipList();
				return true;
			}
			return false;
		}

		public string ChangeDeckName(int deck_id, string new_deck_name)
		{
			string name = base.UserInfo.GetDeck(deck_id).Name;
			Api_Result<Hashtable> api_Result = new Api_req_Member().Update_DeckName(deck_id, new_deck_name);
			if (api_Result.state == Api_Result_State.Success)
			{
				return new_deck_name;
			}
			return name;
		}

		public bool Lock(int ship_mem_id)
		{
			Api_Result<Mem_ship> api_Result = this._api.Lock(ship_mem_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool IsValidUseSweets(int deck_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			if (deck == null)
			{
				return false;
			}
			if (deck.Count == 0)
			{
				return false;
			}
			if (deck.MissionState != MissionStates.NONE)
			{
				return false;
			}
			Dictionary<SweetsType, bool> availableSweets = this.GetAvailableSweets(deck_id);
			return availableSweets.ContainsValue(true);
		}

		public Dictionary<SweetsType, bool> GetAvailableSweets(int deck_id)
		{
			Dictionary<SweetsType, bool> dictionary = new Dictionary<SweetsType, bool>();
			bool[] array = new Api_req_Member().itemuse_cond_check(deck_id);
			dictionary.Add(SweetsType.Mamiya, array[0]);
			dictionary.Add(SweetsType.Irako, array[1]);
			dictionary.Add(SweetsType.Both, dictionary.get_Item(SweetsType.Irako) && this.GetMamiyaCount() > 0);
			return dictionary;
		}

		public bool UseSweets(int deck_id, SweetsType type)
		{
			if (!this.IsValidUseSweets(deck_id))
			{
				return false;
			}
			HashSet<int> useitem_id;
			if (type == SweetsType.Both)
			{
				HashSet<int> hashSet = new HashSet<int>();
				hashSet.Add(54);
				hashSet.Add(59);
				useitem_id = hashSet;
			}
			else
			{
				HashSet<int> hashSet = new HashSet<int>();
				hashSet.Add((int)type);
				useitem_id = hashSet;
			}
			Api_Result<bool> api_Result = new Api_req_Member().itemuse_cond(deck_id, useitem_id);
			return api_Result.state == Api_Result_State.Success;
		}

		private void _UpdateShipList()
		{
			List<ShipModel> list = base.UserInfo.__GetShipList__();
			if (this.MapArea.Id == 1)
			{
				this._ships = list;
			}
			else
			{
				this._ships = base.GetAreaShips(this.MapArea.Id, list);
				this._ships.AddRange(base.GetDepotShips(list));
			}
			this._ships = DeckUtil.GetSortedList(this._ships, this._pre_sort_key);
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("{0}\n", base.ToString());
			text += string.Format("海域:{0}\n", this.MapArea);
			text += string.Format("給糧艦(間宮)の所有数:{0}", this.GetMamiyaCount());
			return text + string.Format("\t給糧艦(伊良湖)の所有数:{0}", this.GetIrakoCount());
		}
	}
}
