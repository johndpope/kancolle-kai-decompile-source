using Common.Enum;
using local.managers;
using local.utils;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.models
{
	public class MapAreaModel
	{
		private UserInfoModel _userInfo;

		private User_StrategyMapFmt _strategy_fmt;

		private List<Mem_ndock> _ndocks;

		private _TankerManager _tship_manager;

		public int Id
		{
			get
			{
				return this._strategy_fmt.Maparea.Id;
			}
		}

		public string Name
		{
			get
			{
				return this._strategy_fmt.Maparea.Name;
			}
		}

		public int NDockMax
		{
			get
			{
				return this._strategy_fmt.Maparea.Ndocks_max;
			}
		}

		public int NDockCount
		{
			get
			{
				return this._ndocks.get_Count();
			}
		}

		public int NDockCountEmpty
		{
			get
			{
				return this._ndocks.FindAll((Mem_ndock state) => state.State == NdockStates.EMPTY).get_Count();
			}
		}

		public RebellionState RState
		{
			get
			{
				return this._strategy_fmt.RebellionState;
			}
		}

		public string Description
		{
			get
			{
				return string.Format("{0}{0}{0}{0}{0}{0}{0}", "海域詳細情報");
			}
		}

		public List<int> NeighboringAreaIDs
		{
			get
			{
				return this._strategy_fmt.Maparea.Neighboring_area.GetRange(0, this._strategy_fmt.Maparea.Neighboring_area.get_Count());
			}
		}

		public MapAreaModel(UserInfoModel user_info, User_StrategyMapFmt strategy_fmt, Dictionary<int, List<Mem_ndock>> ndock_dic, _TankerManager tship_manager)
		{
			this.__Update__(user_info, strategy_fmt, ndock_dic, tship_manager);
		}

		public bool IsOpen()
		{
			return this._strategy_fmt.IsActiveArea;
		}

		public DeckModel[] GetDecks()
		{
			return this._userInfo.GetDecksFromArea(this.Id);
		}

		public EscortDeckModel GetEscortDeck()
		{
			return this._userInfo.GetEscortDeck(this.Id);
		}

		public List<ShipModel> GetRepairingShips()
		{
			List<ShipModel> list = new List<ShipModel>();
			for (int i = 0; i < this._ndocks.get_Count(); i++)
			{
				if (this._ndocks.get_Item(i).State == NdockStates.RESTORE)
				{
					list.Add(this._userInfo.GetShip(this._ndocks.get_Item(i).Ship_id));
				}
			}
			return list;
		}

		public AreaTankerModel GetTankerCount()
		{
			return this._tship_manager.GetCounts(this.Id);
		}

		public List<NdockStates> GetNDockStateList()
		{
			List<NdockStates> list = Enumerable.ToList<NdockStates>(Enumerable.Select<Mem_ndock, NdockStates>(this._ndocks, (Mem_ndock dock) => dock.State));
			while (list.get_Count() < this.NDockMax)
			{
				list.Add(NdockStates.NOTUSE);
			}
			return list;
		}

		[Obsolete("local.utils.Utils.GetAreaResource(int area_id, int tanker_count, EscortDeckManager eManager) を使用してください", false)]
		public Dictionary<enumMaterialCategory, int> GetResources(int tanker_count)
		{
			return local.utils.Utils.GetAreaResource(this.Id, tanker_count);
		}

		public void __Update__(UserInfoModel user_info, User_StrategyMapFmt strategy_fmt, Dictionary<int, List<Mem_ndock>> ndock_dic, _TankerManager tship_manager)
		{
			this._userInfo = user_info;
			this._strategy_fmt = strategy_fmt;
			if (ndock_dic.ContainsKey(this.Id))
			{
				this._ndocks = ndock_dic.get_Item(this.Id);
			}
			else
			{
				this._ndocks = new List<Mem_ndock>();
			}
			this._tship_manager = tship_manager;
		}

		public void __UpdateNdockData__(List<Mem_ndock> ndocks)
		{
			this._ndocks = ndocks;
		}

		public HashSet<int> __GetRepairingShipMemIdsHash__()
		{
			HashSet<int> hashSet = new HashSet<int>();
			for (int i = 0; i < this._ndocks.get_Count(); i++)
			{
				if (this._ndocks.get_Item(i).State == NdockStates.RESTORE)
				{
					hashSet.Add(this._ndocks.get_Item(i).Ship_id);
				}
			}
			return hashSet;
		}

		public override string ToString()
		{
			string text = string.Format("[海域{0}:{1}]", this.Id, this.Name);
			Mem_rebellion_point mem_rebellion_point;
			Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(this.Id, ref mem_rebellion_point);
			text += string.Format(" RP:{0}({1})", this.RState, (mem_rebellion_point != null) ? mem_rebellion_point.Point : 0);
			if (!this.IsOpen())
			{
				text += string.Format(" [未開放] ", new object[0]);
			}
			AreaTankerModel tankerCount = this.GetTankerCount();
			text += string.Format(" 輸送船:{0}/{1}(移動中:{2},遠征中:{3}) ", new object[]
			{
				tankerCount.GetCountNoMove(),
				tankerCount.GetMaxCount(),
				tankerCount.GetCountMove(),
				tankerCount.GetCountInMission()
			});
			text += string.Format("入渠ドック: {0} ", this.ToString(this.GetNDockStateList()));
			DeckModel[] decks = this.GetDecks();
			if (decks.Length == 0)
			{
				text += string.Format("海域に艦隊無し", new object[0]);
			}
			else
			{
				for (int i = 0; i < decks.Length; i++)
				{
					DeckModel deckModel = decks[i];
					text += string.Format("{0}", deckModel);
				}
			}
			EscortDeckModel escortDeck = this.GetEscortDeck();
			if (escortDeck == null)
			{
				text += string.Format(" 護衛艦隊無し", new object[0]);
			}
			else
			{
				text += string.Format(" {0}", escortDeck);
			}
			return text;
		}

		public string ToString(List<NdockStates> stateList)
		{
			string text = "[";
			for (int i = 0; i < stateList.get_Count(); i++)
			{
				text += stateList.get_Item(i);
				if (i < stateList.get_Count() - 1)
				{
					text += ", ";
				}
			}
			return text + "]";
		}
	}
}
