using Common.Enum;
using local.models;
using local.utils;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class SortieManager : ManagerBase
	{
		private int _area_id;

		private List<MapModel> _maps;

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._area_id);
			}
		}

		public MapModel[] Maps
		{
			get
			{
				return this._maps.ToArray();
			}
		}

		public SortieManager(int area_id)
		{
			this._area_id = area_id;
			Dictionary<int, Mst_mapinfo> mst_mapinfo = Mst_DataManager.Instance.Mst_mapinfo;
			Api_Result<Dictionary<int, User_MapinfoFmt>> api_Result = new Api_get_Member().Mapinfo();
			Dictionary<int, User_MapinfoFmt> dictionary;
			if (api_Result.state == Api_Result_State.Success)
			{
				dictionary = api_Result.data;
			}
			else
			{
				dictionary = new Dictionary<int, User_MapinfoFmt>();
			}
			this._maps = new List<MapModel>();
			using (Dictionary<int, User_MapinfoFmt>.ValueCollection.Enumerator enumerator = dictionary.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					User_MapinfoFmt current = enumerator.get_Current();
					Mst_mapinfo mst_mapinfo2 = mst_mapinfo.get_Item(current.Id);
					if (mst_mapinfo2.Maparea_id == area_id)
					{
						MapModel mapModel = new MapModel(mst_mapinfo2, current);
						this._maps.Add(mapModel);
					}
				}
			}
			this._maps.Sort((MapModel x, MapModel y) => (x.MstId <= y.MstId) ? -1 : 1);
		}

		public List<IsGoCondition> IsGoSortie(int deck_id, int map_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidSortie();
			if (deck.AreaId != this.MapArea.Id)
			{
				list.Add(IsGoCondition.AnotherArea);
			}
			if (this._maps.Find((MapModel map) => map.MstId == map_id) == null)
			{
				list.Add(IsGoCondition.Invalid);
			}
			ShipModel[] ships = deck.GetShips();
			HashSet<SType> sortieLimit = Utils.GetSortieLimit(map_id, true);
			if (sortieLimit != null)
			{
				for (int i = 0; i < ships.Length; i++)
				{
					SType shipType = (SType)ships[i].ShipType;
					if (!sortieLimit.Contains(shipType))
					{
						list.Add(IsGoCondition.InvalidOrganization);
						break;
					}
				}
			}
			if (!list.Contains(IsGoCondition.InvalidOrganization))
			{
				HashSet<SType> sortieLimit2 = Utils.GetSortieLimit(map_id, false);
				if (sortieLimit2 != null)
				{
					for (int j = 0; j < ships.Length; j++)
					{
						SType shipType2 = (SType)ships[j].ShipType;
						if (sortieLimit2.Contains(shipType2))
						{
							list.Add(IsGoCondition.InvalidOrganization);
							break;
						}
					}
				}
			}
			return list;
		}

		public SortieMapManager GoSortie(int deck_id, int map_id)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			MapModel map = this._maps.Find((MapModel m) => m.MstId == map_id);
			return new SortieMapManager(deck, map, this._maps);
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("{0}\n", base.ToString());
			text += string.Format("\n--この海域のマップ一覧--\n", new object[0]);
			for (int i = 0; i < this.Maps.Length; i++)
			{
				text += string.Format("{0}\n", this.Maps[i]);
			}
			text += string.Format("\n--この海域の艦隊一覧--\n", new object[0]);
			for (int j = 0; j < this.MapArea.GetDecks().Length; j++)
			{
				text += string.Format("{0}\n", this.MapArea.GetDecks()[j]);
			}
			return text;
		}
	}
}
