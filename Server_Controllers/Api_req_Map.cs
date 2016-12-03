using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.BattleLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Map : IRebellionPointOperator
	{
		public class MapRequireUserShipInfo
		{
			public Mem_deck Mem_deck;

			public List<Mem_ship> Mem_ship;

			public List<List<Mst_slotitem>> Mst_slotitems;

			public List<int> Stype;

			public MapRequireUserShipInfo(int deckRid)
			{
				Comm_UserDatas.Instance.User_deck.TryGetValue(deckRid, ref this.Mem_deck);
			}

			public bool SetShips()
			{
				if (this.Mem_deck == null)
				{
					return false;
				}
				if (this.Mem_ship == null)
				{
					this.Mem_ship = new List<Mem_ship>();
					this.Stype = new List<int>();
					this.Mst_slotitems = new List<List<Mst_slotitem>>();
				}
				else
				{
					this.ClearCollection();
				}
				List<Mem_ship> memShip = this.Mem_deck.Ship.getMemShip();
				if (memShip.get_Count() == 0)
				{
					return false;
				}
				memShip.ForEach(delegate(Mem_ship addShip)
				{
					this.Mem_ship.Add(addShip);
					this.Stype.Add(addShip.Stype);
					this.Mst_slotitems.Add(addShip.GetMstSlotItems());
				});
				return true;
			}

			public Mem_ship GetFlagShip()
			{
				return this.Mem_ship.get_Item(0);
			}

			public void ClearCollection()
			{
				this.Mem_ship.Clear();
				this.Mst_slotitems.Clear();
				this.Stype.Clear();
			}
		}

		private Mst_mapcell2 _mst_cell;

		private Mst_mapcell2 _next_mst_cell;

		private int _now_Cell;

		private Mem_rebellion_point _mem_rebellion;

		private Mst_maparea _mst_maparea;

		private Dictionary<int, Dictionary<int, Mst_mapenemy2>> _map_enemy;

		private ILookup<int, Mst_mapenemylevel> _map_enemylevel;

		private Api_req_Map.MapRequireUserShipInfo userShipInfo1;

		private Api_req_Map.MapRequireUserShipInfo userShipInfo2;

		private List<Mem_deck> _support_decks;

		private bool isLeadingDeck;

		private Mem_mapcomp mapComp;

		private Mem_mapclear mapClear;

		private MapBranchResult mapBranchLogic;

		private Dictionary<int, List<int>> mstRoute;

		private Dictionary<int, List<Mst_mapincentive>> mstMapIncentive;

		private Dictionary<int, List<Mst_mapcellincentive>> mstMapCellIncentive;

		private Dictionary<int, int> slotExpChangeValues;

		private int _enemy_id;

		private Dictionary<int, User_MapCellInfo> _user_mapcell;

		private IEnumerable<XElement> _mst_stype_group;

		private ILookup<int, int> _mst_SupportData;

		private bool isRebbelion;

		private List<MapItemGetFmt> _airReconnaissanceItems;

		private int _mapBattleCellPassCount;

		public List<Mem_deck> Support_decks
		{
			get
			{
				return this._support_decks;
			}
		}

		public int Enemy_Id
		{
			get
			{
				return this._enemy_id;
			}
		}

		public Dictionary<int, User_MapCellInfo> User_mapcell
		{
			get
			{
				return this._user_mapcell;
			}
			private set
			{
				this._user_mapcell = value;
			}
		}

		public ILookup<int, int> Mst_SupportData
		{
			get
			{
				return this._mst_SupportData;
			}
			private set
			{
				this._mst_SupportData = value;
			}
		}

		public bool IsRebbelion
		{
			get
			{
				return this.isRebbelion;
			}
			private set
			{
				this.isRebbelion = value;
			}
		}

		public List<MapItemGetFmt> AirReconnaissanceItems
		{
			get
			{
				return this._airReconnaissanceItems;
			}
			private set
			{
				this._airReconnaissanceItems = value;
			}
		}

		public int MapBattleCellPassCount
		{
			get
			{
				return this._mapBattleCellPassCount;
			}
			private set
			{
				this._mapBattleCellPassCount = value;
			}
		}

		public Api_req_Map()
		{
			this._support_decks = new List<Mem_deck>();
			this.isLeadingDeck = false;
			this.MapBattleCellPassCount = 0;
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			throw new NotImplementedException();
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			this._mem_rebellion.SubPoint(this, subNum);
		}

		public void GetSortieDeckInfo(MapBranchResult instance, out List<Mem_ship> ships, out Dictionary<int, List<Mst_slotitem>> slotItems)
		{
			ships = null;
			slotItems = null;
			if (instance == null)
			{
				return;
			}
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			ships = new List<Mem_ship>();
			slotItems = new Dictionary<int, List<Mst_slotitem>>();
			for (int i = 0; i < activeShipInfo.Mem_ship.get_Count(); i++)
			{
				if (activeShipInfo.Mem_ship.get_Item(i).IsFight())
				{
					ships.Add(activeShipInfo.Mem_ship.get_Item(i));
					slotItems.Add(activeShipInfo.Mem_ship.get_Item(i).Rid, Enumerable.ToList<Mst_slotitem>(activeShipInfo.Mst_slotitems.get_Item(i)));
				}
			}
		}

		public Api_Result<Map_ResultFmt> Start(int maparea_id, int map_no, int deck_id)
		{
			this.mapBranchLogic = new MapBranchResult(this);
			this.IsRebbelion = false;
			Api_Result<Map_ResultFmt> api_Result = new Api_Result<Map_ResultFmt>();
			if (!this.initMapData(maparea_id, map_no))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			((IRebellionPointOperator)this).SubRebellionPoint(maparea_id, this.getRebellionPointSubNum(map_no));
			this.userShipInfo1 = new Api_req_Map.MapRequireUserShipInfo(deck_id);
			if (!this.userShipInfo1.SetShips())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			this._now_Cell = 0;
			api_Result.data = this.getMapResult(-1);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_deck> list = Enumerable.ToList<Mem_deck>(Enumerable.Select(Enumerable.Where(Enumerable.Select(Enumerable.Where<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck deck) => deck.SupportKind == Mem_deck.SupportKinds.WAIT), (Mem_deck deck) => new
			{
				deck = deck,
				mst_misson = Mst_DataManager.Instance.Mst_mission.get_Item(deck.Mission_id)
			}), <>__TranspIdent5 => <>__TranspIdent5.mst_misson.Maparea_id == maparea_id), <>__TranspIdent5 => <>__TranspIdent5.deck));
			if (list.get_Count() > 0)
			{
				if (!this.makeSupportData())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				this._support_decks = list;
			}
			list.ForEach(delegate(Mem_deck x)
			{
				x.ChangeSupported();
			});
			List<int> list2 = new QuestSortie(this._mst_cell.Maparea_id, this._mst_cell.Mapinfo_no, this.userShipInfo1.Mem_deck.Rid, this.userShipInfo1.Mem_ship).ExecuteCheck();
			return api_Result;
		}

		public Api_Result<Map_ResultFmt> StartResisted(int maparea_id, int firstDeck, int secondDeck)
		{
			this.mapBranchLogic = new MapBranchResult(this);
			this.IsRebbelion = true;
			Api_Result<Map_ResultFmt> api_Result = new Api_Result<Map_ResultFmt>();
			this.initMapData(maparea_id, 7);
			this.userShipInfo1 = new Api_req_Map.MapRequireUserShipInfo(firstDeck);
			if (!this.userShipInfo1.SetShips())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (secondDeck > 0)
			{
				this.userShipInfo2 = new Api_req_Map.MapRequireUserShipInfo(secondDeck);
				if (!this.userShipInfo2.SetShips())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			List<Mem_deck> list = Enumerable.ToList<Mem_deck>(Enumerable.Where<Mem_deck>(Enumerable.Where<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck deck) => deck.SupportKind == Mem_deck.SupportKinds.WAIT), (Mem_deck deck) => deck.Area_id == maparea_id));
			if (list.get_Count() > 0)
			{
				List<Mst_mission2> supportResistedData = Mst_DataManager.Instance.GetSupportResistedData(maparea_id);
				Mst_DataManager.Instance.Mst_mission.Add(supportResistedData.get_Item(0).Id, supportResistedData.get_Item(0));
				Mst_DataManager.Instance.Mst_mission.Add(supportResistedData.get_Item(1).Id, supportResistedData.get_Item(1));
				if (!this.makeSupportData())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				this._support_decks = list;
			}
			list.ForEach(delegate(Mem_deck x)
			{
				x.ChangeSupported();
			});
			this._now_Cell = 0;
			api_Result.data = this.getMapResult(-1);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		public bool ChangeLeadingDeck()
		{
			if (this.userShipInfo2 == null)
			{
				return false;
			}
			this.isLeadingDeck = true;
			return true;
		}

		public Api_Result<Map_ResultFmt> Next(ShipRecoveryType recovery_type)
		{
			Api_Result<Map_ResultFmt> api_Result = this.initNext(recovery_type);
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			api_Result.data = this.getMapResult(-1);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		public Api_Result<Map_ResultFmt> Next(ShipRecoveryType recovery_type, int selectCellNo)
		{
			Api_Result<Map_ResultFmt> api_Result = this.initNext(recovery_type);
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			api_Result.data = this.getMapResult(selectCellNo);
			if (api_Result.data == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		private Api_Result<Map_ResultFmt> initNext(ShipRecoveryType recovery_type)
		{
			Api_Result<Map_ResultFmt> api_Result = new Api_Result<Map_ResultFmt>();
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			if (!activeShipInfo.SetShips())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!this.useFlagShipRecover(activeShipInfo, recovery_type))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			return api_Result;
		}

		private bool useFlagShipRecover(Api_req_Map.MapRequireUserShipInfo shipInfo, ShipRecoveryType recovery_type)
		{
			if (recovery_type == ShipRecoveryType.None)
			{
				return true;
			}
			Mem_ship flagShip = shipInfo.GetFlagShip();
			if (flagShip == null)
			{
				return false;
			}
			int item_id = (int)recovery_type;
			int[] array = new int[]
			{
				-1,
				item_id
			};
			array[0] = flagShip.GetMstSlotItems().FindIndex((Mst_slotitem x) => x.Id == item_id);
			Mst_slotitem mstSlotItemToExSlot = flagShip.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				int id = mstSlotItemToExSlot.Id;
				if (id == item_id)
				{
					array[0] = 2147483647;
					array[1] = id;
				}
			}
			if (array[0] == -1)
			{
				return false;
			}
			if (array[0] != 2147483647)
			{
				shipInfo.Mst_slotitems.get_Item(0).RemoveAt(array[0]);
			}
			flagShip.UseRecoveryItem(array, true);
			return true;
		}

		public void GetSortieShipDatas(out List<Mem_ship> activeShips, out List<Mem_ship> inactiveShips)
		{
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			Api_req_Map.MapRequireUserShipInfo inActiveShipInfo = this.getInActiveShipInfo();
			activeShips = Enumerable.ToList<Mem_ship>(activeShipInfo.Mem_ship);
			inactiveShips = ((inActiveShipInfo != null) ? Enumerable.ToList<Mem_ship>(inActiveShipInfo.Mem_ship) : new List<Mem_ship>());
		}

		public TurnState SortieEnd()
		{
			this.mapBranchLogic = null;
			this.UpdateMapcomp(this._now_Cell);
			if (!this.isRebbelion)
			{
				using (List<Mem_deck>.Enumerator enumerator = this._support_decks.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_deck current = enumerator.get_Current();
						current.MissionEnforceEnd();
						current.ActionEnd();
					}
				}
				this._support_decks.Clear();
			}
			this.User_mapcell.Clear();
			this._map_enemy.Clear();
			this._map_enemylevel = null;
			Extensions.Remove<XElement>(this._mst_stype_group);
			this.userShipInfo1.Mem_ship.ForEach(delegate(Mem_ship x)
			{
				x.SetSortieEndCond(this);
				if (x.Escape_sts)
				{
					x.ChangeEscapeState();
				}
			});
			this.userShipInfo1.Mem_deck.ActionEnd();
			this.userShipInfo1.ClearCollection();
			this.Mst_SupportData = null;
			Mst_DataManager.Instance.Mst_mapcell.Clear();
			Mst_DataManager.Instance.Mst_mapenemy.Clear();
			Extensions.Remove<XElement>(Mst_DataManager.Instance.Mst_shipget);
			if (this.userShipInfo2 != null)
			{
				this.userShipInfo2.Mem_ship.ForEach(delegate(Mem_ship x)
				{
					x.SetSortieEndCond(this);
					if (x.Escape_sts)
					{
						x.ChangeEscapeState();
					}
				});
				this.userShipInfo2.Mem_deck.ActionEnd();
				this.userShipInfo2.ClearCollection();
			}
			this.mstMapIncentive.Clear();
			this.mstRoute.Clear();
			if (this.slotExpChangeValues != null)
			{
				using (Dictionary<int, int>.Enumerator enumerator2 = this.slotExpChangeValues.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, int> current2 = enumerator2.get_Current();
						Mem_slotitem mem_slotitem = null;
						if (Comm_UserDatas.Instance.User_slot.TryGetValue(current2.get_Key(), ref mem_slotitem))
						{
							mem_slotitem.ChangeExperience(current2.get_Value());
						}
					}
				}
				this.slotExpChangeValues.Clear();
				this.slotExpChangeValues = null;
			}
			if (this.AirReconnaissanceItems != null)
			{
				this.AirReconnaissanceItems.Clear();
			}
			return TurnState.OWN_END;
		}

		public bool RebellionEnd()
		{
			bool result = false;
			if (!this.IsRebbelion)
			{
				return result;
			}
			List<int> list = new List<int>();
			using (List<Mem_deck>.Enumerator enumerator = this._support_decks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_deck current = enumerator.get_Current();
					current.MissionEnforceEnd();
					current.ActionEnd();
					list.Add(current.Rid);
				}
			}
			this._support_decks.Clear();
			if (this._mem_rebellion.State == RebellionState.Invation)
			{
				list.Add(this.userShipInfo1.Mem_deck.Rid);
				if (this.userShipInfo2 != null)
				{
					list.Add(this.userShipInfo2.Mem_deck.Rid);
				}
				new RebellionUtils().LostArea(this._mem_rebellion.Rid, list);
				result = true;
			}
			Mst_DataManager.Instance.Mst_mission.Remove(100000);
			Mst_DataManager.Instance.Mst_mission.Remove(100001);
			return result;
		}

		public List<int> GetEnemyShipIds()
		{
			Mst_mapenemy2 mst_mapenemy = null;
			if (!Mst_DataManager.Instance.Mst_mapenemy.TryGetValue(this._enemy_id, ref mst_mapenemy))
			{
				return new List<int>();
			}
			List<int> list = new List<int>();
			list.Add(mst_mapenemy.E1_id);
			list.Add(mst_mapenemy.E2_id);
			list.Add(mst_mapenemy.E3_id);
			list.Add(mst_mapenemy.E4_id);
			list.Add(mst_mapenemy.E5_id);
			list.Add(mst_mapenemy.E6_id);
			return list;
		}

		public string GetEnemyShipNames()
		{
			Mst_mapenemy2 mst_mapenemy = null;
			if (!Mst_DataManager.Instance.Mst_mapenemy.TryGetValue(this._enemy_id, ref mst_mapenemy))
			{
				return string.Empty;
			}
			return mst_mapenemy.Deck_name;
		}

		public void GetBattleShipData(out BattleBaseData f_Instance, out BattleBaseData e_Instance)
		{
			f_Instance = null;
			e_Instance = null;
			if (Mst_DataManager.Instance.Mst_mapenemy.ContainsKey(this._enemy_id))
			{
				e_Instance = new BattleBaseData(this._enemy_id);
			}
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			f_Instance = new BattleBaseData(activeShipInfo.Mem_deck, activeShipInfo.Mem_ship, activeShipInfo.Stype, activeShipInfo.Mst_slotitems);
		}

		public void SetSlotExpChangeValues(Api_req_SortieBattle battleInstance, Dictionary<int, int> changeExpDatas)
		{
			if (this.slotExpChangeValues == null)
			{
				this.slotExpChangeValues = changeExpDatas;
				return;
			}
			using (Dictionary<int, int>.Enumerator enumerator = changeExpDatas.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					if (this.slotExpChangeValues.ContainsKey(current.get_Key()))
					{
						this.slotExpChangeValues.set_Item(current.get_Key(), this.slotExpChangeValues.get_Item(current.get_Key()) + current.get_Value());
					}
					else
					{
						this.slotExpChangeValues.Add(current.get_Key(), current.get_Value());
					}
				}
			}
		}

		public Mem_mapclear GetMapClearState()
		{
			return this.mapClear;
		}

		public Mst_mapcell2 GetPrevCell()
		{
			return this._mst_cell;
		}

		public Mst_mapcell2 GetNowCell()
		{
			return this._next_mst_cell;
		}

		private Map_ResultFmt getMapResult(int selectCellNo)
		{
			User_MapCellInfo user_MapCellInfo = null;
			if (!this._user_mapcell.TryGetValue(this._now_Cell, ref user_MapCellInfo))
			{
				return null;
			}
			this._mst_cell = user_MapCellInfo.Mst_mapcell;
			if (!this.User_mapcell.get_Item(this._now_Cell).Passed)
			{
				this.UpdateMapcomp(this._now_Cell);
			}
			if (this._mst_cell.Event_1 == enumMapEventType.War_Normal || this._mst_cell.Event_1 == enumMapEventType.War_Boss)
			{
				this.MapBattleCellPassCount++;
			}
			if (!this._mst_cell.IsNext() && selectCellNo == -1)
			{
				return null;
			}
			MapCommentKind comment = MapCommentKind.None;
			MapProductionKind production = MapProductionKind.None;
			int num = 0;
			if (selectCellNo != -1)
			{
				if (!this.User_mapcell.ContainsKey(selectCellNo))
				{
					return null;
				}
				List<int> list = null;
				if (!this.mstRoute.TryGetValue(this._mst_cell.No, ref list))
				{
				}
				this._now_Cell = selectCellNo;
			}
			else if (this._mst_cell.Next_no_2 > 0)
			{
				num = (int)Utils.GetRandDouble(1.0, 4.0, 1.0, 1);
				if (!this.mapBranchLogic.getNextCellNo(out this._now_Cell, out comment, out production) && !this.getNextCellNo(out this._now_Cell))
				{
					return null;
				}
			}
			else
			{
				this._now_Cell = this._mst_cell.Next_no_1;
			}
			CompassType rashin_id = (CompassType)num;
			user_MapCellInfo = null;
			if (!this._user_mapcell.TryGetValue(this._now_Cell, ref user_MapCellInfo))
			{
				return null;
			}
			this._next_mst_cell = user_MapCellInfo.Mst_mapcell;
			Map_ResultFmt map_ResultFmt = new Map_ResultFmt();
			MapItemGetFmt mapItemGetFmt = null;
			MapHappningFmt happning = null;
			List<MapItemGetFmt> list2 = null;
			int num2 = 0;
			List<int> list3 = null;
			AirReconnaissanceFmt airReconnaissanceFmt = null;
			int spoint = 0;
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			if (this._next_mst_cell.Event_1 == enumMapEventType.ItemGet)
			{
				int itemNo = 0;
				int itemCount = 0;
				this.getItemGetCellReward(out itemNo, out itemCount);
				mapItemGetFmt = this.mapItemGet(itemNo, itemCount);
			}
			else if (this._next_mst_cell.Event_1 == enumMapEventType.PortBackEo)
			{
				bool flag = false;
				if (this.mapClear != null)
				{
					flag = this.mapClear.Cleared;
				}
				list3 = new List<int>(Utils.GetActiveMap().get_Keys());
				num2 = this.setPortBackEoArrivalData(this._next_mst_cell);
				this.setPortBackEoCellReward(num2, out mapItemGetFmt, out list2);
				if (this.mapClear.Cleared && !flag)
				{
					spoint = Mst_DataManager.Instance.Mst_mapinfo.get_Item(this._next_mst_cell.Map_no).Clear_spoint;
				}
			}
			else if (this._next_mst_cell.Event_1 == enumMapEventType.Uzushio)
			{
				happning = this.mapHappning();
			}
			else if (this._next_mst_cell.Event_1 == enumMapEventType.AirReconnaissance)
			{
				if (this.AirReconnaissanceItems == null)
				{
					this.AirReconnaissanceItems = new List<MapItemGetFmt>();
				}
				KeyValuePair<MapAirReconnaissanceKind, double> airSearchParam = this.getAirSearchParam();
				MissionResultKinds airSearchResult = this.getAirSearchResult((double)this._next_mst_cell.Event_point_1, airSearchParam);
				airReconnaissanceFmt = new AirReconnaissanceFmt(airSearchParam.get_Key(), airSearchResult);
				this.setMapCellReward(airReconnaissanceFmt, out mapItemGetFmt);
			}
			else if (this._next_mst_cell.Event_1 == enumMapEventType.War_Normal || this._next_mst_cell.Event_1 == enumMapEventType.War_Boss)
			{
				this._enemy_id = this.selectEnemy();
			}
			List<int> selectcell = null;
			this.mstRoute.TryGetValue(this._next_mst_cell.No, ref selectcell);
			List<int> list4 = null;
			if (num2 == 2)
			{
				List<int> list5 = null;
				new RebellionUtils().MapReOpen(this.mapClear, out list5);
				List<int> list6 = new List<int>(Utils.GetActiveMap().get_Keys());
				list4 = Enumerable.ToList<int>(Enumerable.Except<int>(list6, list3));
				int mapClearNum = Mem_history.GetMapClearNum(this._next_mst_cell.Map_no);
				if (mapClearNum <= 3)
				{
					Mem_history mem_history = new Mem_history();
					mem_history.SetMapClear(total_turn, this._next_mst_cell.Map_no, mapClearNum, this.userShipInfo1.Mem_ship.get_Item(0).Ship_id);
					Comm_UserDatas.Instance.Add_History(mem_history);
				}
				using (List<int>.Enumerator enumerator = list4.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						if (Mem_history.IsFirstOpenArea(current))
						{
							Mem_history mem_history2 = new Mem_history();
							mem_history2.SetAreaOpen(total_turn, current);
							Comm_UserDatas.Instance.Add_History(mem_history2);
						}
					}
				}
			}
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			bool flag2 = this.IsOffshoreSupply(this.GetPrevCell(), this.GetNowCell(), activeShipInfo.Mem_ship);
			if (flag2)
			{
				this.executeOffshoreSupply(activeShipInfo, out map_ResultFmt.MapSupply);
			}
			map_ResultFmt.SetMember(rashin_id, this._next_mst_cell, mapItemGetFmt, list2, happning, comment, production, airReconnaissanceFmt, null, selectcell, list4, spoint);
			List<MapItemGetFmt> list7 = (list2 != null) ? new List<MapItemGetFmt>(list2) : new List<MapItemGetFmt>();
			if (mapItemGetFmt != null)
			{
				list7.Add(mapItemGetFmt);
			}
			if (this._next_mst_cell.Event_1 == enumMapEventType.AirReconnaissance)
			{
				this.AirReconnaissanceItems.AddRange(list7);
			}
			else
			{
				this.updateMapitemGetData(list7);
			}
			return map_ResultFmt;
		}

		private int setPortBackEoArrivalData(Mst_mapcell2 nowCell)
		{
			if (this.mapClear == null)
			{
				Mem_mapclear mem_mapclear = new Mem_mapclear(nowCell.Map_no, nowCell.Maparea_id, nowCell.Mapinfo_no, MapClearState.Cleard);
				mem_mapclear.Insert();
				this.mapClear = mem_mapclear;
				return 2;
			}
			if (this.mapClear.State != MapClearState.Cleard)
			{
				this.mapClear.StateChange(MapClearState.Cleard);
				return 2;
			}
			if (this.mapClear.State == MapClearState.Cleard)
			{
				return 1;
			}
			return 0;
		}

		private void setPortBackEoCellReward(int clearState, out MapItemGetFmt item, out List<MapItemGetFmt> clearItem)
		{
			item = null;
			clearItem = null;
			if (!this.mstMapIncentive.ContainsKey(0))
			{
				return;
			}
			List<Mst_mapincentive> list = new List<Mst_mapincentive>();
			List<Mst_mapincentive> list2 = this.mstMapIncentive.get_Item(0);
			List<double> rateValues = Enumerable.ToList<double>(Enumerable.Select<Mst_mapincentive, double>(list2, (Mst_mapincentive x) => x.Choose_rate));
			int randomRateIndex = Utils.GetRandomRateIndex(rateValues);
			Mst_mapincentive mst_mapincentive = list2.get_Item(randomRateIndex);
			item = new MapItemGetFmt();
			item.Id = mst_mapincentive.Get_id;
			item.Category = mst_mapincentive.GetCategory;
			item.GetCount = mst_mapincentive.Get_count;
		}

		private KeyValuePair<MapAirReconnaissanceKind, double> getAirSearchParam()
		{
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(10);
			hashSet.Add(11);
			HashSet<int> hashSet3 = hashSet;
			int num = 0;
			int num2 = 0;
			double num3 = 0.0;
			for (int i = 0; i < activeShipInfo.Mem_ship.get_Count(); i++)
			{
				List<int> onslot = activeShipInfo.Mem_ship.get_Item(i).Onslot;
				List<Mst_slotitem> list = activeShipInfo.Mst_slotitems.get_Item(i);
				for (int j = 0; j < list.get_Count(); j++)
				{
					Mst_slotitem mst_slotitem = list.get_Item(j);
					if (hashSet3.Contains(mst_slotitem.Api_mapbattle_type3))
					{
						num2++;
						num3 += (double)mst_slotitem.Saku * Math.Sqrt(Math.Sqrt((double)onslot.get_Item(j)));
					}
					else if (hashSet2.Contains(mst_slotitem.Api_mapbattle_type3))
					{
						num++;
						num3 += (double)mst_slotitem.Saku * Math.Sqrt((double)onslot.get_Item(j));
					}
				}
			}
			MapAirReconnaissanceKind mapAirReconnaissanceKind = MapAirReconnaissanceKind.Impossible;
			if (num > 0)
			{
				mapAirReconnaissanceKind = MapAirReconnaissanceKind.LargePlane;
			}
			else if (num2 > 0)
			{
				mapAirReconnaissanceKind = MapAirReconnaissanceKind.WarterPlane;
			}
			KeyValuePair<MapAirReconnaissanceKind, double> result = new KeyValuePair<MapAirReconnaissanceKind, double>(mapAirReconnaissanceKind, num3);
			return result;
		}

		private MissionResultKinds getAirSearchResult(double success_keisu, KeyValuePair<MapAirReconnaissanceKind, double> param)
		{
			if (param.get_Key() == MapAirReconnaissanceKind.Impossible)
			{
				return MissionResultKinds.FAILE;
			}
			double randDouble = Utils.GetRandDouble(0.0, 0.6, 0.1, 1);
			double num = success_keisu * (1.6 + randDouble);
			if (param.get_Value() >= num)
			{
				return MissionResultKinds.GREAT;
			}
			if (param.get_Value() >= success_keisu)
			{
				return MissionResultKinds.SUCCESS;
			}
			return MissionResultKinds.FAILE;
		}

		private void setMapCellReward(AirReconnaissanceFmt air, out MapItemGetFmt itemFmt)
		{
			itemFmt = null;
			if (air.SearchResult == MissionResultKinds.FAILE)
			{
				return;
			}
			List<Mst_mapcellincentive> list = null;
			this.mstMapCellIncentive.TryGetValue(this._next_mst_cell.No, ref list);
			if (list == null)
			{
				return;
			}
			int successFlag = (air.SearchResult != MissionResultKinds.SUCCESS) ? 1 : 0;
			List<Mst_mapcellincentive> list2 = Enumerable.ToList<Mst_mapcellincentive>(Enumerable.Where<Mst_mapcellincentive>(list, (Mst_mapcellincentive x) => x.Success_level == successFlag));
			List<double> rateValues = Enumerable.ToList<double>(Enumerable.Select<Mst_mapcellincentive, double>(list2, (Mst_mapcellincentive y) => y.Choose_rate));
			int randomRateIndex = Utils.GetRandomRateIndex(rateValues);
			Mst_mapcellincentive mst_mapcellincentive = list2.get_Item(randomRateIndex);
			itemFmt = new MapItemGetFmt();
			itemFmt.Id = mst_mapcellincentive.Get_id;
			itemFmt.Category = mst_mapcellincentive.GetCategory;
			itemFmt.GetCount = mst_mapcellincentive.Get_count;
		}

		private void getItemGetCellReward(out int itemNo, out int itemCount)
		{
			itemNo = this._next_mst_cell.Item_no;
			itemCount = this._next_mst_cell.Item_count;
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			List<Mst_mapcellincentive> list = null;
			this.mstMapCellIncentive.TryGetValue(this._next_mst_cell.No, ref list);
			if (list == null)
			{
				if (itemNo < 5)
				{
					double[] array = new double[]
					{
						default(double),
						0.5,
						1.0
					};
					var <>__AnonType = Enumerable.First(Enumerable.OrderBy(Enumerable.Select(array, (double value) => new
					{
						value
					}), x => Guid.NewGuid()));
					double num = (double)itemCount * <>__AnonType.value;
					itemCount += (int)num;
				}
				return;
			}
			Mst_mapcellincentive mst_mapcellincentive = list.get_Item(0);
			if (mst_mapcellincentive.Event_id != 2)
			{
				return;
			}
			Dictionary<int, int> countDict = Enumerable.ToDictionary<int, int, int>(mst_mapcellincentive.Req_items.get_Keys(), (int item_id) => item_id, (int item_count) => 0);
			activeShipInfo.Mst_slotitems.ForEach(delegate(List<Mst_slotitem> slotList)
			{
				slotList.ForEach(delegate(Mst_slotitem mst_slot)
				{
					Dictionary<int, int> countDict;
					if (countDict.ContainsKey(mst_slot.Id))
					{
						Dictionary<int, int> expr_1C = countDict = countDict;
						int num4;
						int expr_24 = num4 = mst_slot.Id;
						num4 = countDict.get_Item(num4);
						expr_1C.set_Item(expr_24, num4 + 1);
					}
				});
			});
			int num2 = 0;
			using (Dictionary<int, int>.Enumerator enumerator = countDict.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					num2 += mst_mapcellincentive.Req_items.get_Item(current.get_Key()) * current.get_Value();
				}
			}
			int num3 = itemCount + num2;
			if (num3 > mst_mapcellincentive.Get_count)
			{
				num3 = mst_mapcellincentive.Get_count;
			}
			itemCount = num3;
		}

		private MapItemGetFmt mapItemGet(int itemNo, int itemCount)
		{
			MapItemGetFmt mapItemGetFmt = new MapItemGetFmt();
			if (itemNo >= 1 && itemNo <= 8)
			{
				mapItemGetFmt.Category = MapItemGetFmt.enumCategory.Material;
				mapItemGetFmt.Id = itemNo;
				mapItemGetFmt.GetCount = itemCount;
			}
			else if (itemNo == 9 || itemNo == 10 || itemNo == 11)
			{
				mapItemGetFmt.Category = MapItemGetFmt.enumCategory.UseItem;
				if (this._next_mst_cell.Item_no == 9)
				{
					mapItemGetFmt.Id = 10;
				}
				else if (this._next_mst_cell.Item_no == 10)
				{
					mapItemGetFmt.Id = 11;
				}
				else if (this._next_mst_cell.Item_no == 11)
				{
					mapItemGetFmt.Id = 12;
				}
				mapItemGetFmt.GetCount = itemCount;
			}
			return mapItemGetFmt;
		}

		public void updateMapitemGetData(List<MapItemGetFmt> itemFmt)
		{
			using (List<MapItemGetFmt>.Enumerator enumerator = itemFmt.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MapItemGetFmt current = enumerator.get_Current();
					if (current.Category == MapItemGetFmt.enumCategory.Furniture)
					{
						Comm_UserDatas.Instance.Add_Furniture(current.Id);
					}
					else if (current.Category == MapItemGetFmt.enumCategory.Material)
					{
						enumMaterialCategory id = (enumMaterialCategory)current.Id;
						Comm_UserDatas.Instance.User_material.get_Item(id).Add_Material(current.GetCount);
					}
					else if (current.Category == MapItemGetFmt.enumCategory.Ship)
					{
						List<int> ship_ids = Enumerable.ToList<int>(Enumerable.Repeat<int>(current.Id, current.GetCount));
						Comm_UserDatas.Instance.Add_Ship(ship_ids);
					}
					else if (current.Category == MapItemGetFmt.enumCategory.Slotitem)
					{
						List<int> slot_ids = Enumerable.ToList<int>(Enumerable.Repeat<int>(current.Id, current.GetCount));
						Comm_UserDatas.Instance.Add_Slot(slot_ids);
					}
					else if (current.Category == MapItemGetFmt.enumCategory.UseItem)
					{
						Comm_UserDatas.Instance.Add_Useitem(current.Id, current.GetCount);
					}
				}
			}
		}

		private MapHappningFmt mapHappning()
		{
			int dentanShipCnt = 0;
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			activeShipInfo.Mst_slotitems.ForEach(delegate(List<Mst_slotitem> x)
			{
				if (x.Exists((Mst_slotitem y) => y.Api_mapbattle_type3 == 12 || y.Api_mapbattle_type3 == 13))
				{
					dentanShipCnt++;
				}
			});
			double rate = 1.0;
			if (dentanShipCnt == 1)
			{
				rate = 0.75;
			}
			else if (dentanShipCnt == 2)
			{
				rate = 0.6;
			}
			else if (dentanShipCnt >= 3)
			{
				rate = 0.5;
			}
			List<int> subValues = new List<int>();
			activeShipInfo.Mem_ship.ForEach(delegate(Mem_ship x)
			{
				int num = 0;
				if (this._next_mst_cell.Item_no == 1)
				{
					num = this.getHappenCellSubValue(rate, x.Fuel);
					int fuel = (x.Fuel - num >= 0) ? (x.Fuel - num) : 0;
					x.Set_ChargeData(x.Bull, fuel, x.Onslot);
				}
				else if (this._next_mst_cell.Item_no == 2)
				{
					num = this.getHappenCellSubValue(rate, x.Bull);
					int bull = (x.Bull - num >= 0) ? (x.Bull - num) : 0;
					x.Set_ChargeData(bull, x.Fuel, x.Onslot);
				}
				subValues.Add(num);
			});
			return new MapHappningFmt
			{
				Id = this._next_mst_cell.Item_no,
				Count = Enumerable.Max(subValues),
				Dentan = (dentanShipCnt != 0)
			};
		}

		private int getHappenCellSubValue(double sub_rate, int now_num)
		{
			int num = (int)((double)now_num * 0.4 * sub_rate);
			if (num > this._next_mst_cell.Item_count)
			{
				num = this._next_mst_cell.Item_count;
			}
			return num;
		}

		private int selectEnemy()
		{
			int table_no = this._next_mst_cell.Table_no1;
			enumMapEventType event_ = this._next_mst_cell.Event_1;
			Mst_mapenemylevel mst_mapenemylevel = this.SelectEnemy(this._map_enemylevel.get_Item(table_no), Comm_UserDatas.Instance.User_basic.Difficult, Comm_UserDatas.Instance.User_turn.Total_turn);
			int deck_id = mst_mapenemylevel.Deck_id;
			return this._map_enemy.get_Item(table_no).get_Item(deck_id).Id;
		}

		private Mst_mapenemylevel SelectEnemy(IEnumerable<Mst_mapenemylevel> enemyItems, DifficultKind difficulty, int now_turn)
		{
			List<Mst_mapenemylevel> list = Enumerable.ToList<Mst_mapenemylevel>(Enumerable.Where<Mst_mapenemylevel>(enemyItems, (Mst_mapenemylevel e) => e.Difficulty == difficulty));
			if (list.get_Count() == 0)
			{
				return null;
			}
			bool flag = Enumerable.Any<Mst_mapenemylevel>(list, (Mst_mapenemylevel x) => x.Turns > 0);
			list.Sort((Mst_mapenemylevel a, Mst_mapenemylevel b) => (a.Turns != b.Turns) ? (b.Turns - a.Turns) : (b.Choose_rate - a.Choose_rate));
			int num = (int)Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			for (int i = 0; i < list.get_Count(); i++)
			{
				Mst_mapenemylevel mst_mapenemylevel = list.get_Item(i);
				if (mst_mapenemylevel.Turns <= now_turn)
				{
					if (flag)
					{
						if (mst_mapenemylevel.Choose_rate == -1)
						{
							return mst_mapenemylevel;
						}
					}
					else if (mst_mapenemylevel.Choose_rate == -1)
					{
						return mst_mapenemylevel;
					}
					num -= mst_mapenemylevel.Choose_rate;
					if (num <= 0)
					{
						return mst_mapenemylevel;
					}
				}
			}
			return null;
		}

		private bool getNextCellNo(out int cellNo)
		{
			string[] useRateData = this.getUseRateData();
			List<int> list = new List<int>();
			int num = 0;
			string[] array = useRateData;
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i];
				list.Add(num);
				num += int.Parse(text);
			}
			list.Reverse();
			Random random = new Random();
			int num2 = random.Next(100);
			int num3 = 0;
			int num4 = Enumerable.Count<int>(list);
			for (int j = 0; j < num4; j++)
			{
				if (list.get_Item(j) <= num2)
				{
					num3 = num4 - (j + 1);
					break;
				}
			}
			List<int> list2 = new List<int>();
			list2.Add(this._mst_cell.Next_no_1);
			list2.Add(this._mst_cell.Next_no_2);
			list2.Add(this._mst_cell.Next_no_3);
			list2.Add(this._mst_cell.Next_no_4);
			List<int> list3 = list2;
			cellNo = list3.get_Item(num3);
			return true;
		}

		private string[] getUseRateData()
		{
			char[] array = new char[]
			{
				','
			};
			Api_req_Map.MapRequireUserShipInfo activeShipInfo = this.getActiveShipInfo();
			if (!string.IsNullOrEmpty(this._mst_cell.Next_rate_req))
			{
				if (Enumerable.Count<Mem_ship>(activeShipInfo.Mem_ship) < this._mst_cell.Req_ship_count)
				{
					return this._mst_cell.Next_rate.Split(array);
				}
				if (!string.IsNullOrEmpty(this._mst_cell.Req_shiptype))
				{
					string[] array2 = this._mst_cell.Req_shiptype.Split(array);
					string[] array3 = array2;
					for (int i = 0; i < array3.Length; i++)
					{
						string text = array3[i];
						int num = int.Parse(text);
						if (!activeShipInfo.Stype.Contains(num))
						{
							return this._mst_cell.Next_rate.Split(array);
						}
					}
					return this._mst_cell.Next_rate_req.Split(array);
				}
			}
			return this._mst_cell.Next_rate.Split(array);
		}

		private void UpdateMapcomp(int target_cell)
		{
			User_MapCellInfo user_MapCellInfo = this.User_mapcell.get_Item(target_cell);
			if (user_MapCellInfo.Passed)
			{
				return;
			}
			if (string.IsNullOrEmpty(user_MapCellInfo.Mst_mapcell.Link_no))
			{
				this.User_mapcell.get_Item(this._now_Cell).Passed = true;
				this.mapComp.No.Add(this._now_Cell);
			}
			else
			{
				string[] array = user_MapCellInfo.Mst_mapcell.Link_no.Split(new char[]
				{
					','
				});
				string[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					string text = array2[i];
					int num = int.Parse(text);
					if (!this.User_mapcell.get_Item(num).Passed)
					{
						this.User_mapcell.get_Item(num).Passed = true;
						this.mapComp.No.Add(num);
					}
				}
			}
		}

		private Api_req_Map.MapRequireUserShipInfo getActiveShipInfo()
		{
			return (!this.isLeadingDeck) ? this.userShipInfo1 : this.userShipInfo2;
		}

		private Api_req_Map.MapRequireUserShipInfo getInActiveShipInfo()
		{
			return (!this.isLeadingDeck) ? this.userShipInfo2 : this.userShipInfo1;
		}

		private bool initMapData(int maparea_id, int map_no)
		{
			int num = int.Parse(maparea_id.ToString() + map_no.ToString());
			if (this.mapComp == null && !Comm_UserDatas.Instance.User_mapcomp.TryGetValue(num, ref this.mapComp))
			{
				this.mapComp = new Mem_mapcomp(num, maparea_id, map_no);
				this.mapComp.Insert();
			}
			if (this.mapClear == null && Comm_UserDatas.Instance.User_mapclear.TryGetValue(num, ref this.mapClear) && this.mapClear.State == MapClearState.InvationClose)
			{
				return false;
			}
			this.mstRoute = Mst_DataManager.Instance.GetMaproute(num);
			this.mstMapIncentive = Mst_DataManager.Instance.GetMapIncentive(num);
			this.mstMapCellIncentive = Mst_DataManager.Instance.GetMapCellIncentive(num);
			if (this._mst_stype_group == null)
			{
				this._mst_stype_group = Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id");
				if (this._mst_stype_group == null)
				{
					return false;
				}
			}
			if (!this.makeMapcell(maparea_id, map_no))
			{
				return false;
			}
			if (!this.makeMapEnemy(maparea_id, map_no))
			{
				return false;
			}
			if (!this.makeMapEnemyLevel(maparea_id, map_no))
			{
				return false;
			}
			if (!this.makeMapShipget(maparea_id, map_no))
			{
				return false;
			}
			this._mst_maparea = null;
			return Mst_DataManager.Instance.Mst_maparea.TryGetValue(maparea_id, ref this._mst_maparea) && this._mst_maparea.IsOpenArea() && Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(maparea_id, ref this._mem_rebellion);
		}

		private bool makeMapcell(int maparea_id, int map_no)
		{
			Mst_DataManager.Instance.Make_MapCell(maparea_id, map_no);
			if (Mst_DataManager.Instance.Mst_mapcell == null)
			{
				return false;
			}
			if (this.User_mapcell == null)
			{
				this.User_mapcell = new Dictionary<int, User_MapCellInfo>();
			}
			this.User_mapcell.Clear();
			using (Dictionary<int, Mst_mapcell2>.Enumerator enumerator = Mst_DataManager.Instance.Mst_mapcell.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, Mst_mapcell2> current = enumerator.get_Current();
					bool passed = false;
					if (this.mapComp.No.Contains(current.get_Value().No))
					{
						passed = true;
					}
					User_MapCellInfo user_MapCellInfo = new User_MapCellInfo(current.get_Value(), passed);
					this.User_mapcell.Add(current.get_Value().No, user_MapCellInfo);
				}
			}
			return true;
		}

		private bool makeMapEnemy(int maparea_id, int map_no)
		{
			Mst_DataManager.Instance.Make_Mapenemy(maparea_id, map_no);
			if (Mst_DataManager.Instance.Mst_mapenemy == null)
			{
				return false;
			}
			if (this._map_enemy == null)
			{
				this._map_enemy = new Dictionary<int, Dictionary<int, Mst_mapenemy2>>();
			}
			this._map_enemy.Clear();
			using (Dictionary<int, Mst_mapenemy2>.ValueCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_mapenemy.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_mapenemy2 current = enumerator.get_Current();
					if (this._map_enemy.ContainsKey(current.Enemy_list_id))
					{
						this._map_enemy.get_Item(current.Enemy_list_id).Add(current.Deck_id, current);
					}
					else
					{
						Dictionary<int, Mst_mapenemy2> dictionary = new Dictionary<int, Mst_mapenemy2>();
						dictionary.Add(current.Deck_id, current);
						Dictionary<int, Mst_mapenemy2> dictionary2 = dictionary;
						this._map_enemy.Add(current.Enemy_list_id, dictionary2);
					}
				}
			}
			return true;
		}

		private bool makeMapEnemyLevel(int maparea_id, int mapinfo_no)
		{
			this._map_enemylevel = Mst_DataManager.Instance.GetMapenemyLevel(maparea_id, mapinfo_no);
			return this._map_enemylevel != null;
		}

		private bool makeMapShipget(int maparea_id, int map_no)
		{
			if (this.isRebbelion)
			{
				map_no = 4;
			}
			Mst_DataManager.Instance.Make_Mapshipget(maparea_id, map_no);
			return Mst_DataManager.Instance.Mst_shipget != null && Enumerable.Count<XElement>(Mst_DataManager.Instance.Mst_shipget) != 0;
		}

		private bool makeSupportData()
		{
			if (this.Mst_SupportData != null)
			{
				return true;
			}
			var enumerable = Enumerable.Select(this._mst_stype_group, (XElement item) => new
			{
				id = int.Parse(item.Element("Id").get_Value()),
				type = int.Parse(item.Element("Support").get_Value())
			});
			this.Mst_SupportData = Enumerable.ToLookup(enumerable, key => key.type, value => value.id);
			return true;
		}

		private int getRebellionPointSubNum(int map_no)
		{
			double num = 0.0;
			if (map_no == 1)
			{
				num = 0.07;
			}
			else if (map_no == 2)
			{
				num = 0.1;
			}
			else if (map_no == 3)
			{
				num = 0.2;
			}
			else if (map_no == 4)
			{
				num = 0.14;
			}
			else if (map_no == 5)
			{
				num = 0.1;
			}
			else if (map_no == 6)
			{
				num = 0.05;
			}
			return (int)((double)this._mem_rebellion.Point * num);
		}

		private bool IsOffshoreSupply(Mst_mapcell2 nowCell, Mst_mapcell2 nextCell, List<Mem_ship> ships)
		{
			if (Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Value <= 0 && Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Value <= 0)
			{
				return false;
			}
			if (!this.IsOffshoreSuppllyCell(nowCell, nextCell))
			{
				return false;
			}
			double num = 0.0;
			double num2 = 0.0;
			int num3 = 0;
			using (var enumerator = Enumerable.Select(ships, (Mem_ship obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					Mem_ship obj2 = current.obj;
					int idx2 = current.idx;
					if (!obj2.Escape_sts)
					{
						num += (double)obj2.Fuel / (double)Mst_DataManager.Instance.Mst_ship.get_Item(obj2.Ship_id).Fuel_max;
						num2 += (double)obj2.Bull / (double)Mst_DataManager.Instance.Mst_ship.get_Item(obj2.Ship_id).Bull_max;
						num3++;
					}
				}
			}
			if (num3 == 0)
			{
				return false;
			}
			double num4 = 0.25;
			num /= (double)num3;
			num2 /= (double)num3;
			double num5 = (num + num2) / 2.0;
			return num5 <= num4;
		}

		private bool IsOffshoreSuppllyCell(Mst_mapcell2 nowCell, Mst_mapcell2 nextCell)
		{
			return nowCell.Event_1 == enumMapEventType.War_Normal && (!nextCell.IsNext() || (nextCell.Event_1 == enumMapEventType.War_Normal || nextCell.Event_1 == enumMapEventType.War_Boss));
		}

		private void executeOffshoreSupply(Api_req_Map.MapRequireUserShipInfo ship_info, out MapSupplyFmt mapSupply)
		{
			mapSupply = null;
			int[] array = this.haveOffshoreSupplyItem(ship_info);
			if (array[0] == -1)
			{
				return;
			}
			int num = array[0];
			int num2 = array[1];
			Mem_shipBase mem_shipBase = new Mem_shipBase(ship_info.Mem_ship.get_Item(num));
			int num3;
			if (num2 == 2147483647)
			{
				num3 = ship_info.Mem_ship.get_Item(num).Exslot;
				mem_shipBase.Exslot = -1;
			}
			else
			{
				num3 = ship_info.Mem_ship.get_Item(num).Slot.get_Item(num2);
				mem_shipBase.Slot.set_Item(num2, -1);
				ship_info.Mst_slotitems.get_Item(num).RemoveAt(num2);
			}
			ship_info.Mem_ship.get_Item(num).Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id), false);
			ship_info.Mem_ship.get_Item(num).TrimSlot();
			Comm_UserDatas.Instance.User_slot.Remove(num3);
			int rid = ship_info.Mem_ship.get_Item(num).Rid;
			List<int> list = new List<int>();
			using (List<Mem_ship>.Enumerator enumerator = ship_info.Mem_ship.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					bool flag = this.setOffshoreSupply(current);
					if (flag)
					{
						list.Add(current.Rid);
					}
				}
			}
			mapSupply = new MapSupplyFmt(rid, list);
		}

		private int[] haveOffshoreSupplyItem(Api_req_Map.MapRequireUserShipInfo ship_info)
		{
			int[] array = new int[]
			{
				-1,
				-1
			};
			int num = 146;
			for (int i = ship_info.Mem_ship.get_Count() - 1; i >= 0; i--)
			{
				if (!ship_info.Mem_ship.get_Item(i).Escape_sts)
				{
					Mst_slotitem mstSlotItemToExSlot = ship_info.Mem_ship.get_Item(i).GetMstSlotItemToExSlot();
					if (mstSlotItemToExSlot != null && mstSlotItemToExSlot.Id == num)
					{
						array[0] = i;
						array[1] = 2147483647;
						return array;
					}
					List<Mst_slotitem> list = ship_info.Mst_slotitems.get_Item(i);
					for (int j = list.get_Count() - 1; j >= 0; j--)
					{
						Mst_slotitem mst_slotitem = list.get_Item(j);
						if (mst_slotitem.Id == num)
						{
							array[0] = i;
							array[1] = j;
							return array;
						}
					}
				}
			}
			return array;
		}

		private bool setOffshoreSupply(Mem_ship mem_ship)
		{
			if (mem_ship.Escape_sts)
			{
				return false;
			}
			int value = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Value;
			int value2 = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Value;
			if (value <= 0 && value2 <= 0)
			{
				return false;
			}
			int fuel_max = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id).Fuel_max;
			int bull_max = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id).Bull_max;
			if (mem_ship.Fuel >= fuel_max && mem_ship.Bull >= bull_max)
			{
				return false;
			}
			double num = 0.25;
			double num2 = 0.25;
			int num3 = mem_ship.Fuel;
			int num4 = mem_ship.Bull;
			int num5 = 0;
			if (mem_ship.Fuel < fuel_max && value > 0)
			{
				int num6 = (int)((double)fuel_max * num);
				if (num6 == 0)
				{
					num6 = 1;
				}
				if (num6 > value)
				{
					num6 = value;
				}
				num3 = mem_ship.Fuel + num6;
				if (num3 > fuel_max)
				{
					num3 = fuel_max;
				}
				int num7 = num3 - mem_ship.Fuel;
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Sub_Material(num7);
				num5++;
			}
			if (mem_ship.Bull < bull_max && value2 > 0)
			{
				int num8 = (int)((double)bull_max * num2);
				if (num8 == 0)
				{
					num8 = 1;
				}
				if (num8 > value2)
				{
					num8 = value2;
				}
				num4 = mem_ship.Bull + num8;
				if (num4 > bull_max)
				{
					num4 = bull_max;
				}
				int num9 = num4 - mem_ship.Bull;
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Sub_Material(num9);
				num5++;
			}
			if (num5 == 0)
			{
				return false;
			}
			mem_ship.Set_ChargeData(num4, num3, null);
			return true;
		}
	}
}
