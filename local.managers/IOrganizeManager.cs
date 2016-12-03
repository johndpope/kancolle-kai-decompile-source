using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public interface IOrganizeManager
	{
		MapAreaModel MapArea
		{
			get;
		}

		int ShipsCount
		{
			get;
		}

		SortKey NowSortKey
		{
			get;
		}

		int GetMamiyaCount();

		int GetIrakoCount();

		bool ChangeSortKey(SortKey new_sort_key);

		ShipModel[] GetShipList();

		ShipModel[] GetShipList(int page_no, int count_in_page);

		bool IsValidShip(int ship_mem_id);

		bool IsValidChange(int deck_id, int selected_index, int ship_mem_id);

		bool ChangeOrganize(int deck_id, int selected_index, int ship_mem_id);

		bool IsValidUnset(int ship_mem_id);

		bool UnsetOrganize(int deck_id, int selected_index);

		bool IsValidUnsetAll(int deck_id);

		bool UnsetAllOrganize(int deck_id);

		string ChangeDeckName(int deck_id, string new_deck_name);

		bool Lock(int ship_mem_id);

		bool IsValidUseSweets(int deck_id);

		Dictionary<SweetsType, bool> GetAvailableSweets(int deck_id);

		bool UseSweets(int deck_id, SweetsType type);
	}
}
