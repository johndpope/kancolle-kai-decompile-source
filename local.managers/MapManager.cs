using Common.Enum;
using local.models;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class MapManager : ManagerBase
	{
		protected DeckModel _deck;

		protected DeckModel _mainDeck;

		protected DeckModel _subDeck;

		protected MapModel _map;

		protected List<MapModel> _maps;

		protected Api_req_Map _req_map;

		protected List<CellModel> _cells;

		protected int _now_cell_no;

		protected Map_ResultFmt _next_cell;

		protected List<int> _passed;

		protected List<MapEventItemModel> _items;

		public DeckModel Deck
		{
			get
			{
				return this._deck;
			}
		}

		public MapModel Map
		{
			get
			{
				return this._map;
			}
		}

		public CellModel[] Cells
		{
			get
			{
				return this._cells.ToArray();
			}
		}

		public List<int> Passed
		{
			get
			{
				return this._passed.GetRange(0, this._passed.get_Count());
			}
		}

		public List<MapEventItemModel> Items
		{
			get
			{
				return this._items.GetRange(0, this._items.get_Count());
			}
		}

		public CellModel NowCell
		{
			get
			{
				return this._cells.get_Item(this._now_cell_no);
			}
		}

		public CellModel NextCell
		{
			get
			{
				return this._cells.get_Item(this._next_cell.Cell_no);
			}
		}

		public CompassType CompassId
		{
			get
			{
				return this._next_cell.Rashin_id;
			}
		}

		public MapCommentKind Comment
		{
			get
			{
				return this._next_cell.Comment;
			}
		}

		public MapProductionKind Production
		{
			get
			{
				return this._next_cell.Production;
			}
		}

		public enumMapEventType NextCategory
		{
			get
			{
				return this._next_cell.Event_id;
			}
		}

		public enumMapWarType NextEventType
		{
			get
			{
				return this._next_cell.Event_kind;
			}
		}

		public int BgmId
		{
			get
			{
				return Mst_DataManager.Instance.UiBattleMaster.MapBgm;
			}
		}

		public MapManager(DeckModel deck, MapModel map, List<MapModel> maps)
		{
			this._deck = deck;
			this._map = map;
			this._maps = maps;
			this._req_map = new Api_req_Map();
			this._cells = new List<CellModel>();
			this._items = new List<MapEventItemModel>();
			Comm_UserDatas.Instance.User_trophy.Start_map_count++;
		}

		public MapManager(DeckModel deck, MapModel map)
		{
			this._deck = deck;
			this._map = map;
			this._maps = null;
			this._req_map = new Api_req_Map();
			this._cells = new List<CellModel>();
			this._items = new List<MapEventItemModel>();
		}

		public virtual TurnState MapEnd()
		{
			TurnState result = this._req_map.SortieEnd();
			Api_get_Member api_get_mem = new Api_get_Member();
			base.UserInfo.__UpdateShips__(api_get_mem);
			return result;
		}

		public bool hasCompass()
		{
			return this.CompassId != CompassType.None;
		}

		public bool HasAirReconnaissance()
		{
			return this._req_map.AirReconnaissanceItems != null;
		}

		public List<CellModel> GetNextCellCandidate()
		{
			List<CellModel> list = new List<CellModel>();
			if (this._next_cell.SelectCells == null)
			{
				return list;
			}
			int i;
			for (i = 0; i < this._next_cell.SelectCells.get_Count(); i++)
			{
				CellModel cellModel = this._cells.Find((CellModel cell) => cell.CellNo == this._next_cell.SelectCells.get_Item(i));
				list.Add(cellModel);
			}
			return list;
		}

		public bool IsNextFinal()
		{
			return !this._next_cell.IsNext;
		}

		public bool GoNext(ShipRecoveryType recovery_type)
		{
			return this.GoNext(recovery_type, -1);
		}

		public bool GoNext(ShipRecoveryType recovery_type, int selected_next_cell_no)
		{
			Api_get_Member api_get_mem = new Api_get_Member();
			base.UserInfo.__UpdateDeck__(api_get_mem);
			Api_Result<Map_ResultFmt> api_Result = (selected_next_cell_no == -1) ? this._req_map.Next(recovery_type) : this._req_map.Next(recovery_type, selected_next_cell_no);
			if (api_Result.state == Api_Result_State.Success)
			{
				this._now_cell_no = this.NextCell.CellNo;
				this._next_cell = api_Result.data;
				this._passed.Add(this._now_cell_no);
				if (this._map.MapHP != null && api_Result.data.MapHp != null)
				{
					this._map.MapHP.__Update__(api_Result.data.MapHp);
				}
				if (this._next_cell.Event_id == enumMapEventType.ItemGet)
				{
					this._items.Add(this.GetItemEvent());
				}
				return true;
			}
			return false;
		}

		public int GetNextCellEnemyId()
		{
			if (this.NextCategory != enumMapEventType.War_Normal && this.NextCategory != enumMapEventType.War_Boss)
			{
				return 0;
			}
			return this._req_map.Enemy_Id;
		}

		public string GetNextCellEnemyFleetName()
		{
			if (this.NextCategory != enumMapEventType.War_Normal && this.NextCategory != enumMapEventType.War_Boss)
			{
				return string.Empty;
			}
			return this._req_map.GetEnemyShipNames();
		}

		public List<ShipModelMst> GetNextCellEnemys()
		{
			if (this.NextCategory != enumMapEventType.War_Normal && this.NextCategory != enumMapEventType.War_Boss)
			{
				return null;
			}
			List<int> enemyShipIds = this._req_map.GetEnemyShipIds();
			if (enemyShipIds == null)
			{
				return null;
			}
			List<ShipModelMst> list = new List<ShipModelMst>();
			for (int i = 0; i < enemyShipIds.get_Count(); i++)
			{
				int num = enemyShipIds.get_Item(i);
				if (num <= 0)
				{
					list.Add(null);
				}
				else
				{
					list.Add(new ShipModelMst(num));
				}
			}
			return list;
		}

		public MapSupplyModel GetMapSupplyInfo()
		{
			if (this._next_cell.MapSupply == null)
			{
				return null;
			}
			return new MapSupplyModel(this.Deck, this._next_cell.MapSupply);
		}

		public SortieBattleManagerBase BattleStart(BattleFormationKinds1 formation_id)
		{
			return null;
		}

		public SortieBattleManagerBase BattleStart_Write(BattleFormationKinds1 formation_id)
		{
			return null;
		}

		public SortieBattleManagerBase BattleStart_Read()
		{
			return null;
		}

		public MapEventItemModel GetItemEvent()
		{
			return new MapEventItemModel(this._next_cell.ItemGet);
		}

		public MapEventHappeningModel GetHappeningEvent()
		{
			return new MapEventHappeningModel(this._next_cell.Happning);
		}

		public MapEventAirReconnaissanceModel GetAirReconnaissanceEvent()
		{
			return new MapEventAirReconnaissanceModel(this._next_cell.ItemGet, this._next_cell.AirReconnaissance);
		}

		public List<IReward> GetMapClearItems()
		{
			if (this._next_cell.MapClearItem == null)
			{
				return null;
			}
			List<IReward> list = new List<IReward>();
			for (int i = 0; i < this._next_cell.MapClearItem.get_Count(); i++)
			{
				IReward reward = null;
				MapItemGetFmt mapItemGetFmt = this._next_cell.MapClearItem.get_Item(i);
				switch (mapItemGetFmt.Category)
				{
				case MapItemGetFmt.enumCategory.Furniture:
					reward = new Reward_Furniture(mapItemGetFmt.Id);
					break;
				case MapItemGetFmt.enumCategory.Slotitem:
					reward = new Reward_Slotitem(mapItemGetFmt.Id, mapItemGetFmt.GetCount);
					break;
				case MapItemGetFmt.enumCategory.Ship:
					reward = new Reward_Ship(mapItemGetFmt.Id);
					break;
				case MapItemGetFmt.enumCategory.Material:
					reward = new Reward_Material((enumMaterialCategory)mapItemGetFmt.Id, mapItemGetFmt.GetCount);
					break;
				case MapItemGetFmt.enumCategory.UseItem:
					reward = new Reward_Useitem(mapItemGetFmt.Id, mapItemGetFmt.GetCount);
					break;
				}
				if (reward != null)
				{
					list.Add(reward);
				}
			}
			return list;
		}

		public int[] GetNewOpenAreaIDs()
		{
			if (this._next_cell.NewOpenMapId == null)
			{
				return null;
			}
			List<int> list = this._next_cell.NewOpenMapId.FindAll((int id) => id % 10 == 1);
			if (list.get_Count() == 0)
			{
				return null;
			}
			return list.ConvertAll<int>((int val) => (int)Math.Floor((double)val / 10.0)).ToArray();
		}

		public int[] GetNewOpenMapIDs()
		{
			if (this._next_cell.NewOpenMapId == null || this._next_cell.NewOpenMapId.get_Count() == 0)
			{
				return null;
			}
			return this._next_cell.NewOpenMapId.ToArray();
		}

		public int GetSPoint()
		{
			return this._next_cell.GetSpoint;
		}

		public void ChangeCurrentDeck()
		{
			if (this._mainDeck != null && this._deck == this._subDeck && this._req_map.ChangeLeadingDeck())
			{
				this._deck = this._mainDeck;
			}
		}

		protected abstract void _Init();

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("---[マップ{0}-{1}(ID:{2}) - 「{3}」への出撃]---\n", new object[]
			{
				this.Map.AreaId,
				this.Map.No,
				this.Map.MstId,
				this.Map.Name
			});
			text += string.Format("{0}", this.Map);
			text += string.Format("出撃艦隊:{0}\n", this.Deck);
			return text + string.Format("セル総数:{0} BGM:{1}", this._cells.get_Count(), this.BgmId);
		}
	}
}
