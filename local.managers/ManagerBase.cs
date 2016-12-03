using Common.Struct;
using local.models;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class ManagerBase
	{
		protected static TurnState _turn_state = TurnState.CONTINOUS;

		protected static Dictionary<int, MapAreaModel> _area;

		protected static MaterialModel _materialModel;

		protected static UserInfoModel _userInfoModel;

		protected static SettingModel _settingModel;

		protected _TankerManager _tanker_manager;

		public int Turn
		{
			get
			{
				return Comm_UserDatas.Instance.User_turn.Total_turn;
			}
		}

		public DateTime Datetime
		{
			get
			{
				return Comm_UserDatas.Instance.User_turn.GetDateTime();
			}
		}

		public TurnString DatetimeString
		{
			get
			{
				return Comm_UserDatas.Instance.User_turn.GetTurnString();
			}
		}

		public TurnState TurnState
		{
			get
			{
				return ManagerBase._turn_state;
			}
		}

		public MaterialModel Material
		{
			get
			{
				return ManagerBase._materialModel;
			}
		}

		public UserInfoModel UserInfo
		{
			get
			{
				return ManagerBase._userInfoModel;
			}
		}

		public SettingModel Settings
		{
			get
			{
				return ManagerBase._settingModel;
			}
		}

		public ManagerBase()
		{
			if (ManagerBase._area == null)
			{
				ManagerBase._area = new Dictionary<int, MapAreaModel>();
			}
		}

		public static bool IsInitialized()
		{
			return ManagerBase._materialModel != null && ManagerBase._userInfoModel != null && ManagerBase._settingModel != null;
		}

		public static DeckModelBase getDeck(int ship_mem_id)
		{
			DeckModelBase deckModelBase = ManagerBase._userInfoModel.GetDeckByShipMemId(ship_mem_id);
			if (deckModelBase == null)
			{
				deckModelBase = ManagerBase._userInfoModel.GetEscortDeckByShipMemId(ship_mem_id);
			}
			return deckModelBase;
		}

		public static int IsInEscortDeck(int ship_mem_id)
		{
			return ManagerBase._userInfoModel.GetEscortDeckId(ship_mem_id);
		}

		public static void initialize()
		{
			ManagerBase._materialModel = new MaterialModel();
			ManagerBase._userInfoModel = new UserInfoModel();
			ManagerBase._settingModel = new SettingModel();
			ManagerBase._materialModel.Update();
		}

		public List<ShipModel> GetAreaShips(int area_id, List<ShipModel> all_ships)
		{
			return this.GetAreaShips(area_id, true, true, all_ships);
		}

		public List<ShipModel> GetAreaShips(int area_id, bool use_deck, bool use_edeck, List<ShipModel> all_ships)
		{
			List<ShipModel> list = new List<ShipModel>();
			MapAreaModel mapAreaModel = ManagerBase._area.get_Item(area_id);
			if (use_deck)
			{
				DeckModel[] decks = mapAreaModel.GetDecks();
				DeckModel[] array = decks;
				for (int i = 0; i < array.Length; i++)
				{
					DeckModel deckModel = array[i];
					list.AddRange(deckModel.GetShips());
				}
			}
			if (use_edeck)
			{
				list.AddRange(mapAreaModel.GetEscortDeck().GetShips());
			}
			HashSet<int> hashSet = mapAreaModel.__GetRepairingShipMemIdsHash__();
			using (HashSet<int>.Enumerator enumerator = hashSet.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					ShipModel ship2 = this.UserInfo.GetShip(current);
					if (ship2.IsInDeck() == -1 && ship2.IsInEscortDeck() == -1)
					{
						list.Add(ship2);
					}
				}
			}
			if (all_ships == null)
			{
				all_ships = this.UserInfo.__GetShipList__();
			}
			List<ShipModel> list2 = all_ships.FindAll((ShipModel ship) => ship.IsBlingWait() && ship.AreaIdBeforeBlingWait == area_id);
			list.AddRange(list2);
			return list;
		}

		public List<ShipModel> GetDepotShips(List<ShipModel> all_ships)
		{
			HashSet<int> o_ship_memids = this.UserInfo.__GetShipMemIdHashInBothDeck__();
			HashSet<int> r_ship_memids = this.__GetNDockShipMemIdsHash__();
			if (all_ships == null)
			{
				all_ships = this.UserInfo.__GetShipList__();
			}
			return all_ships.FindAll((ShipModel ship) => !o_ship_memids.Contains(ship.MemId) && !r_ship_memids.Contains(ship.MemId) && !ship.IsBlingWait());
		}

		public HashSet<int> __GetNDockShipMemIdsHash__()
		{
			HashSet<int> hashSet = new HashSet<int>();
			using (Dictionary<int, MapAreaModel>.ValueCollection.Enumerator enumerator = ManagerBase._area.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MapAreaModel current = enumerator.get_Current();
					HashSet<int> hashSet2 = current.__GetRepairingShipMemIdsHash__();
					using (HashSet<int>.Enumerator enumerator2 = hashSet2.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int current2 = enumerator2.get_Current();
							hashSet.Add(current2);
						}
					}
				}
			}
			return hashSet;
		}

		protected void _UpdateTankerManager()
		{
			if (this._tanker_manager == null)
			{
				this._tanker_manager = new _TankerManager();
			}
			else
			{
				this._tanker_manager.Update();
			}
		}

		protected void _CreateMapAreaModel()
		{
			this._UpdateTankerManager();
			Dictionary<int, MapAreaModel> dictionary = new Dictionary<int, MapAreaModel>();
			using (Dictionary<int, MapAreaModel>.KeyCollection.Enumerator enumerator = ManagerBase._area.get_Keys().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					dictionary.Add(current, ManagerBase._area.get_Item(current));
				}
			}
			ManagerBase._area.Clear();
			Api_get_Member api_get_Member = new Api_get_Member();
			Api_Result<Dictionary<int, User_StrategyMapFmt>> api_Result = api_get_Member.StrategyInfo();
			Dictionary<int, List<Mem_ndock>> data = api_get_Member.AreaNdock().data;
			if (api_Result.state == Api_Result_State.Success)
			{
				Dictionary<int, User_StrategyMapFmt> data2 = api_Result.data;
				using (Dictionary<int, User_StrategyMapFmt>.KeyCollection.Enumerator enumerator2 = data2.get_Keys().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						int current2 = enumerator2.get_Current();
						if (dictionary.ContainsKey(current2))
						{
							dictionary.get_Item(current2).__Update__(this.UserInfo, data2.get_Item(current2), data, this._tanker_manager);
							ManagerBase._area.Add(current2, dictionary.get_Item(current2));
							dictionary.Remove(current2);
						}
						else
						{
							ManagerBase._area.Add(current2, new MapAreaModel(this.UserInfo, data2.get_Item(current2), data, this._tanker_manager));
						}
					}
				}
			}
		}

		public override string ToString()
		{
			string text = string.Format("{0}\n{1} 所持家具コイン:{2}\n", this.UserInfo, this.Material, this.UserInfo.FCoin);
			text += string.Format("総ターン数:{0}\t日時:{1}", this.Turn, this.Datetime);
			text += string.Format("({0}年{1} {2}日 {3})\n", new object[]
			{
				this.DatetimeString.Year,
				this.DatetimeString.Month,
				this.DatetimeString.Day,
				this.DatetimeString.DayOfWeek
			});
			return text + string.Format("{0}", this.Settings);
		}
	}
}
