using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common.Formats
{
	public class Map_ResultFmt
	{
		public CompassType Rashin_id;

		public int Cell_no;

		public List<int> SelectCells;

		public int Color_no;

		public enumMapEventType Event_id;

		public enumMapWarType Event_kind;

		public bool IsNext;

		public MapCommentKind Comment;

		public MapProductionKind Production;

		public AirReconnaissanceFmt AirReconnaissance;

		public MapItemGetFmt ItemGet;

		public List<MapItemGetFmt> MapClearItem;

		public MapHappningFmt Happning;

		public EventMapInfo MapHp;

		public List<int> NewOpenMapId;

		public int GetSpoint;

		public MapSupplyFmt MapSupply;

		public Map_ResultFmt()
		{
			this.Rashin_id = CompassType.None;
			this.Cell_no = 0;
			this.Color_no = 0;
			this.Event_id = enumMapEventType.None;
			this.Event_kind = enumMapWarType.None;
			this.IsNext = false;
			this.Comment = MapCommentKind.None;
			this.Production = MapProductionKind.None;
			this.GetSpoint = 0;
		}

		public void SetMember(CompassType rashin_id, Mst_mapcell2 target_cell, MapItemGetFmt item, List<MapItemGetFmt> clearItems, MapHappningFmt happning, MapCommentKind comment, MapProductionKind production, AirReconnaissanceFmt airSearch, EventMapInfo eventMap, List<int> selectcell, List<int> newOpenMap, int spoint)
		{
			this.Rashin_id = rashin_id;
			this.Cell_no = target_cell.No;
			if (selectcell != null)
			{
				this.SelectCells = Enumerable.ToList<int>(selectcell);
			}
			this.Color_no = target_cell.Color_no;
			this.Event_id = target_cell.Event_1;
			this.Event_kind = target_cell.Event_2;
			this.IsNext = target_cell.IsNext();
			this.Comment = comment;
			this.Production = production;
			this.AirReconnaissance = airSearch;
			this.ItemGet = item;
			this.MapClearItem = clearItems;
			this.Happning = happning;
			this.MapHp = eventMap;
			this.NewOpenMapId = newOpenMap;
			this.GetSpoint = spoint;
		}
	}
}
