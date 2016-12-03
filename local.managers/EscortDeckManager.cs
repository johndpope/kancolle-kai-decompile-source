using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class EscortDeckManager : ManagerBase, IOrganizeManager
	{
		private int _area_id;

		private Api_req_Transport _api;

		private TemporaryEscortDeckModel _init_deck;

		private TemporaryEscortDeckModel _edit_deck;

		private List<ShipModel> _ships;

		private SortKey _pre_sort_key;

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._area_id);
			}
		}

		public EscortDeckModel EditDeck
		{
			get
			{
				return this._edit_deck;
			}
		}

		public int ShipsCount
		{
			get
			{
				if (this._ships == null)
				{
					this._UpdateShipList();
				}
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

		public EscortDeckManager(int area_id)
		{
			this._area_id = area_id;
			this._api = new Api_req_Transport();
			base._CreateMapAreaModel();
			this._init_deck = base.UserInfo.__CreateEscortDeckClone__(area_id);
			this._edit_deck = base.UserInfo.__CreateEscortDeckClone__(area_id);
		}

		public int GetMamiyaCount()
		{
			return new UseitemUtil().GetCount(54);
		}

		public int GetIrakoCount()
		{
			return new UseitemUtil().GetCount(59);
		}

		public void InitEscortOrganizer()
		{
			this._api.initEscortGroup();
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
			if (this._ships == null)
			{
				this._UpdateShipList();
			}
			return this._ships.ToArray();
		}

		public ShipModel[] GetShipList(int page_no, int count_in_page)
		{
			if (page_no < 1 || this.ShipsCount / count_in_page + 1 < page_no)
			{
				return new ShipModel[0];
			}
			if (this._ships == null)
			{
				this._UpdateShipList();
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
			DeckModelBase deck = ship.getDeck();
			if (deck != null)
			{
				if (deck.AreaId != this.MapArea.Id)
				{
					return false;
				}
				if (!deck.IsEscortDeckMyself())
				{
					return false;
				}
			}
			return this._edit_deck.HasShipMemId(ship_mem_id);
		}

		public bool IsValidChange(int deck_id, int selected_index, int ship_mem_id)
		{
			return deck_id == this._edit_deck.Id && this.IsValidChange(selected_index, ship_mem_id);
		}

		public bool IsValidChange(int selected_index, int ship_mem_id)
		{
			return this._api.IsValidChange(this._edit_deck.Id, selected_index, ship_mem_id, this._edit_deck.DeckShips);
		}

		public bool ChangeOrganize(int deck_id, int selected_index, int ship_mem_id)
		{
			return deck_id == this._edit_deck.Id && this.ChangeOrganize(selected_index, ship_mem_id);
		}

		public bool ChangeOrganize(int selected_index, int ship_mem_id)
		{
			if (!this._api.IsValidChange(this._edit_deck.Id, selected_index, ship_mem_id, this._edit_deck.DeckShips))
			{
				return false;
			}
			DeckShips deckShips = this._edit_deck.DeckShips;
			bool flag = this._api.Change_TempDeck(selected_index, ship_mem_id, ref deckShips);
			if (flag)
			{
				base.UserInfo.__UpdateTemporaryEscortDeck__(this._edit_deck);
				return true;
			}
			return true;
		}

		public bool IsValidUnset(int ship_mem_id)
		{
			int deck_targetIdx = this._edit_deck.DeckShips.Find(ship_mem_id);
			return this._api.IsValidChange(this._edit_deck.Id, deck_targetIdx, -1, this._edit_deck.DeckShips);
		}

		public bool UnsetOrganize(int deck_id, int selected_index)
		{
			return deck_id == this._edit_deck.Id && this.UnsetOrganize(selected_index);
		}

		public bool UnsetOrganize(int selected_index)
		{
			if (!this._api.IsValidChange(this._edit_deck.Id, selected_index, -1, this._edit_deck.DeckShips))
			{
				return false;
			}
			DeckShips deckShips = this._edit_deck.DeckShips;
			bool flag = this._api.Change_TempDeck(selected_index, -1, ref deckShips);
			if (flag)
			{
				base.UserInfo.__UpdateTemporaryEscortDeck__(this._edit_deck);
				return true;
			}
			return false;
		}

		public bool IsValidUnsetAll(int deck_id)
		{
			return deck_id == this._edit_deck.Id && this.IsValidUnsetAll();
		}

		public bool IsValidUnsetAll()
		{
			return this._api.IsValidChange(this._edit_deck.Id, 0, -2, this._edit_deck.DeckShips);
		}

		public bool UnsetAllOrganize(int deck_id)
		{
			return deck_id == this._edit_deck.Id && this.UnsetAllOrganize();
		}

		public bool UnsetAllOrganize()
		{
			DeckShips deckShips = this._edit_deck.DeckShips;
			if (deckShips.Count() <= 1)
			{
				return false;
			}
			bool flag = this._api.Change_TempDeck(0, -2, ref deckShips);
			if (flag)
			{
				base.UserInfo.__UpdateTemporaryEscortDeck__(this._edit_deck);
				return true;
			}
			return false;
		}

		public string ChangeDeckName(int deck_id, string new_deck_name)
		{
			if (deck_id != this._edit_deck.Id)
			{
				return this._edit_deck.Name;
			}
			return this.ChangeDeckName(new_deck_name);
		}

		public string ChangeDeckName(string new_deck_name)
		{
			this._edit_deck.ChangeName(new_deck_name);
			return this._edit_deck.Name;
		}

		public bool Lock(int ship_mem_id)
		{
			Api_Result<Mem_ship> api_Result = new Api_req_Hensei().Lock(ship_mem_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool IsValidUseSweets(int deck_id)
		{
			return deck_id == this._edit_deck.Id && this.IsValidUseSweets();
		}

		public bool IsValidUseSweets()
		{
			return false;
		}

		public Dictionary<SweetsType, bool> GetAvailableSweets(int deck_id)
		{
			if (deck_id != this._edit_deck.Id)
			{
				return null;
			}
			return this.GetAvailableSweets();
		}

		public Dictionary<SweetsType, bool> GetAvailableSweets()
		{
			Dictionary<SweetsType, bool> dictionary = new Dictionary<SweetsType, bool>();
			dictionary.Add(SweetsType.Mamiya, false);
			dictionary.Add(SweetsType.Irako, false);
			dictionary.Add(SweetsType.Both, false);
			return dictionary;
		}

		public bool UseSweets(int deck_id, SweetsType type)
		{
			return deck_id == this._edit_deck.Id && this.UseSweets(type);
		}

		public bool UseSweets(SweetsType type)
		{
			return !this.IsValidUseSweets() && false;
		}

		public bool HasChanged()
		{
			if (this._init_deck.DeckShips.Count() != this._edit_deck.DeckShips.Count())
			{
				return true;
			}
			for (int i = 0; i < this._init_deck.DeckShips.Count(); i++)
			{
				if (this._init_deck.DeckShips[i] != this._edit_deck.DeckShips[i])
				{
					return true;
				}
			}
			return this._init_deck.__Name__ != this._edit_deck.__Name__;
		}

		public bool __Commit__()
		{
			Api_Result<Mem_esccort_deck> api_Result = this._api.Change(this._edit_deck.Id, this._edit_deck.DeckShips);
			if (api_Result.state == Api_Result_State.Success)
			{
				if (this._init_deck.__Name__ != this._edit_deck.__Name__)
				{
					this._api.Update_DeckName(this._edit_deck.Id, this._edit_deck.__Name__);
				}
				base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
				this._init_deck = base.UserInfo.__CreateEscortDeckClone__(this._edit_deck.AreaId);
				return true;
			}
			return false;
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
			Dictionary<int, int> tmp = this._api.Mst_escort_group;
			if (tmp != null)
			{
				this._ships = this._ships.FindAll((ShipModel ship) => tmp.get_Item(ship.ShipType) == 1);
			}
			this._ships = DeckUtil.GetSortedList(this._ships, this._pre_sort_key);
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("{0}\n", base.ToString());
			text += string.Format("海域:{0}\n", this.MapArea);
			text += string.Format("初期状態:{0}\n", this._init_deck);
			return text + string.Format("暫定編成:{0}\n", this._edit_deck);
		}
	}
}
