using Common.Enum;
using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class RebellionManager : ManagerBase
	{
		private int _area_id;

		private MapModel _map;

		private Api_req_Mission _req_mission;

		private List<DeckModel> _decks;

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._area_id);
			}
		}

		public MapModel Map
		{
			get
			{
				return this._map;
			}
		}

		public List<DeckModel> Decks
		{
			get
			{
				return this._decks;
			}
		}

		public RebellionManager(int area_id)
		{
			this._area_id = area_id;
			int num = this._area_id * 10 + 7;
			Mst_mapinfo mst_map;
			Mst_DataManager.Instance.Mst_mapinfo.TryGetValue(num, ref mst_map);
			this._map = new MapModel(mst_map, null);
			this._req_mission = new Api_req_Mission();
			this._decks = new List<DeckModel>();
			DeckModel[] decksFromArea = base.UserInfo.GetDecksFromArea(this.MapArea.Id);
			for (int i = 0; i < decksFromArea.Length; i++)
			{
				if (decksFromArea[i].Count > 0)
				{
					this._decks.Add(decksFromArea[i]);
				}
			}
		}

		public List<IsGoCondition> IsValidMissionSub(int deck_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidMission();
			HashSet<IsGoCondition> hashSet = this._req_mission.ValidStart(deck_id, -1, 0);
			List<IsGoCondition> list2 = new List<IsGoCondition>(hashSet);
			for (int i = 0; i < list2.get_Count(); i++)
			{
				if (!list.Contains(list2.get_Item(i)))
				{
					list.Add(list2.get_Item(i));
				}
			}
			list2.Remove(IsGoCondition.Deck1);
			list2.Sort();
			return list2;
		}

		public List<IsGoCondition> IsValid_MissionMain(int deck_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidMission();
			HashSet<IsGoCondition> hashSet = this._req_mission.ValidStart(deck_id, -2, 0);
			List<IsGoCondition> list2 = new List<IsGoCondition>(hashSet);
			for (int i = 0; i < list2.get_Count(); i++)
			{
				if (!list.Contains(list2.get_Item(i)))
				{
					list.Add(list2.get_Item(i));
				}
			}
			list2.Remove(IsGoCondition.Deck1);
			list2.Sort();
			return list2;
		}

		public bool IsGoRebellion(int sub_deck_id, int main_deck_id, int sub_support_deck_id, int main_support_deck_id)
		{
			if (this.MapArea.RState != RebellionState.Invation)
			{
				return false;
			}
			if (this._decks.Find((DeckModel deck) => deck.Id == main_deck_id) == null)
			{
				return false;
			}
			if (main_deck_id == sub_deck_id || main_deck_id == sub_support_deck_id || main_deck_id == main_support_deck_id)
			{
				return false;
			}
			if (sub_deck_id != -1)
			{
				if (sub_deck_id == sub_support_deck_id || sub_deck_id == main_support_deck_id)
				{
					return false;
				}
				if (this._decks.Find((DeckModel deck) => deck.Id == sub_deck_id) == null)
				{
					return false;
				}
			}
			if (sub_support_deck_id != -1)
			{
				if (sub_support_deck_id == main_support_deck_id)
				{
					return false;
				}
				if (this._decks.Find((DeckModel deck) => deck.Id == sub_support_deck_id) == null)
				{
					return false;
				}
				if (this.IsValidMissionSub(sub_support_deck_id).get_Count() > 0)
				{
					return false;
				}
			}
			if (main_support_deck_id != -1)
			{
				if (this._decks.Find((DeckModel deck) => deck.Id == main_support_deck_id) == null)
				{
					return false;
				}
				if (this.IsValid_MissionMain(main_support_deck_id).get_Count() > 0)
				{
					return false;
				}
			}
			return true;
		}

		public RebellionMapManager GoRebellion(int sub_deck_id, int main_deck_id, int sub_support_deck_id, int main_support_deck_id)
		{
			if (sub_support_deck_id != -1)
			{
				Api_Result<Mem_deck> api_Result = this._req_mission.Start(sub_support_deck_id, -1, 0);
				if (api_Result.state != Api_Result_State.Success)
				{
					return null;
				}
			}
			if (main_support_deck_id != -1)
			{
				Api_Result<Mem_deck> api_Result2 = this._req_mission.Start(main_support_deck_id, -2, 0);
				if (api_Result2.state != Api_Result_State.Success)
				{
					return null;
				}
			}
			DeckModel mainDeck = this._decks.Find((DeckModel deck) => deck.Id == main_deck_id);
			DeckModel subDeck = this._decks.Find((DeckModel deck) => deck.Id == sub_deck_id);
			return new RebellionMapManager(this._map, mainDeck, subDeck);
		}

		public void NotGoRebellion()
		{
			new Api_req_StrategeMap().ExecuteRebellionLostArea(this.MapArea.Id);
			base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
			base._CreateMapAreaModel();
		}

		public override string ToString()
		{
			return string.Empty;
		}
	}
}
