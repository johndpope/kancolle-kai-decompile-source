using Common.Enum;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common
{
	public class RebellionUtils : IRebellionPointOperator
	{
		private List<Mst_rebellionpoint> mst_rpoint;

		private readonly double rpsw;

		private Dictionary<int, bool> areaOpenState;

		public RebellionUtils()
		{
			this.mst_rpoint = Enumerable.ToList<Mst_rebellionpoint>(Mst_DataManager.Instance.Mst_RebellionPoint.get_Values());
			if (Comm_UserDatas.Instance.User_turn.Total_turn <= 99)
			{
				this.rpsw = 0.5;
				if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
				{
					this.rpsw = 0.4;
				}
				else if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
				{
					this.rpsw = 0.3;
				}
			}
			else if (Comm_UserDatas.Instance.User_turn.Total_turn <= 199)
			{
				this.rpsw = 0.6;
				if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
				{
					this.rpsw = 0.5;
				}
				else if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
				{
					this.rpsw = 0.4;
				}
			}
			else
			{
				this.rpsw = 0.7;
				if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.HEI)
				{
					this.rpsw = 0.6;
				}
				else if (Comm_UserDatas.Instance.User_basic.Difficult == DifficultKind.TEI)
				{
					this.rpsw = 0.5;
				}
			}
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point.get_Item(area_id).AddPoint(this, addNum);
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			throw new NotImplementedException();
		}

		public void LostArea(int maparea_id, List<int> sortieDeckRid)
		{
			Comm_UserDatas commInstance = Comm_UserDatas.Instance;
			if (sortieDeckRid == null)
			{
				sortieDeckRid = new List<int>();
			}
			List<int> lostArea = this.getLostArea(maparea_id);
			lostArea.ForEach(delegate(int lostAreaId)
			{
				Dictionary<int, Mem_mapclear> dictionary = Enumerable.ToDictionary<Mem_mapclear, int, Mem_mapclear>(Enumerable.Where<Mem_mapclear>(commInstance.User_mapclear.get_Values(), (Mem_mapclear x) => x.Maparea_id == lostAreaId), (Mem_mapclear x) => x.Mapinfo_no, (Mem_mapclear y) => y);
				int num = Mst_maparea.MaxMapNum(commInstance.User_basic.Difficult, lostAreaId);
				for (int i = 1; i <= num; i++)
				{
					if (!dictionary.ContainsKey(i))
					{
						int mapinfo_id = Mst_mapinfo.ConvertMapInfoId(lostAreaId, i);
						Mem_mapclear mem_mapclear = new Mem_mapclear(mapinfo_id, lostAreaId, i, MapClearState.InvationClose);
						mem_mapclear.Insert();
						dictionary.Add(i, mem_mapclear);
					}
				}
				this.lostMapClear(Enumerable.ToList<Mem_mapclear>(dictionary.get_Values()), Mst_DataManager.Instance.Mst_maparea.get_Item(lostAreaId).Neighboring_area);
				Comm_UserDatas.Instance.User_rebellion_point.Remove(lostAreaId);
				List<Mem_tanker> areaTanker = Mem_tanker.GetAreaTanker(lostAreaId);
				this.lostTanker(areaTanker);
				IEnumerable<Mem_deck> memDeck = Enumerable.Where<Mem_deck>(commInstance.User_deck.get_Values(), (Mem_deck x) => x.Area_id == lostAreaId);
				this.goBackDeck(memDeck, sortieDeckRid);
				Mem_esccort_deck escort = commInstance.User_EscortDeck.get_Item(lostAreaId);
				this.goBackEscortDeck(escort);
				List<Mem_ndock> ndock = Enumerable.ToList<Mem_ndock>(Enumerable.Where<Mem_ndock>(commInstance.User_ndock.get_Values(), (Mem_ndock x) => x.Area_id == lostAreaId));
				this.lostNdock(ndock);
				Dictionary<enumMaterialCategory, Mem_material> user_material = commInstance.User_material;
				this.lostMaterial(user_material);
				if (lostAreaId == 1)
				{
					this.lostKdock();
				}
			});
		}

		private int debugMapSetting(out Dictionary<int, bool> dict)
		{
			dict = new Dictionary<int, bool>();
			int result = 0;
			List<bool> list = null;
			for (int i = 1; i < list.get_Count(); i++)
			{
				dict.Add(i, list.get_Item(i));
			}
			return result;
		}

		public List<int> getLostArea(int ownerAreaId)
		{
			Dictionary<int, bool> sData = Enumerable.ToDictionary<KeyValuePair<int, User_StrategyMapFmt>, int, bool>(new Api_get_Member().StrategyInfo().data, (KeyValuePair<int, User_StrategyMapFmt> x) => x.get_Key(), (KeyValuePair<int, User_StrategyMapFmt> y) => y.get_Value().IsActiveArea);
			sData.set_Item(ownerAreaId, false);
			List<int> list = new List<int>();
			list.Add(ownerAreaId);
			List<int> list2 = list;
			if (ownerAreaId == 1)
			{
				return list2;
			}
			Dictionary<int, Mst_maparea> mapareaDict = Mst_DataManager.Instance.Mst_maparea;
			List<int> list3 = mapareaDict.get_Item(1).Neighboring_area.FindAll((int x) => sData.get_Item(x));
			bool flag = false;
			HashSet<int> hashSet = new HashSet<int>();
			HashSet<int> passItems = new HashSet<int>();
			hashSet.UnionWith(list3);
			while (!flag)
			{
				List<List<int>> list4 = Enumerable.ToList<List<int>>(Enumerable.Select(Enumerable.Select(hashSet, (int rootItem) => new
				{
					rootItem = rootItem,
					mstdata = mapareaDict.get_Item(rootItem).Neighboring_area.FindAll((int x) => sData.get_Item(x))
				}), <>__TranspIdent0 => <>__TranspIdent0.mstdata));
				hashSet.Clear();
				for (int i = 0; i < Enumerable.Count<List<int>>(list4); i++)
				{
					hashSet.UnionWith(list4.get_Item(i));
					hashSet.RemoveWhere((int x) => passItems.Contains(x));
				}
				if (hashSet.get_Count() == 0)
				{
					flag = true;
				}
				else
				{
					passItems.UnionWith(hashSet);
				}
			}
			HashSet<int> hashSet2 = new HashSet<int>();
			hashSet2.UnionWith(list2);
			if (passItems.get_Count() == 0)
			{
				IEnumerable<int> enumerable = Enumerable.Select<KeyValuePair<int, bool>, int>(Enumerable.Where<KeyValuePair<int, bool>>(sData, (KeyValuePair<int, bool> x) => x.get_Value() && x.get_Key() != 1), (KeyValuePair<int, bool> y) => y.get_Key());
				hashSet2.UnionWith(enumerable);
				return Enumerable.ToList<int>(hashSet2);
			}
			IEnumerable<int> enumerable2 = Enumerable.Select<KeyValuePair<int, bool>, int>(Enumerable.Where<KeyValuePair<int, bool>>(sData, (KeyValuePair<int, bool> x) => x.get_Value()), (KeyValuePair<int, bool> y) => y.get_Key());
			List<int> list5 = Enumerable.ToList<int>(Enumerable.Except<int>(enumerable2, passItems));
			if (list5.get_Count() == 0)
			{
				hashSet2.UnionWith(list2);
			}
			else
			{
				hashSet2.UnionWith(list5);
			}
			return Enumerable.ToList<int>(hashSet2);
		}

		private void lostTanker(List<Mem_tanker> tanker)
		{
			using (List<Mem_tanker>.Enumerator enumerator = tanker.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_tanker current = enumerator.get_Current();
					if (!current.IsBlingShip())
					{
						Comm_UserDatas.Instance.User_tanker.Remove(current.Rid);
					}
					else
					{
						current.BackTanker();
					}
				}
			}
		}

		private void goBackEscortDeck(Mem_esccort_deck escort)
		{
			List<Mem_ship> memShip = escort.Ship.getMemShip();
			memShip.ForEach(delegate(Mem_ship x)
			{
				if (!x.IsBlingShip())
				{
					x.SubHp(this.getShipDamage(x.Nowhp));
				}
				x.PortWithdraw(escort.Rid);
			});
			escort.EscortStop();
			escort.Ship.Clear();
		}

		private void lostMaterial(Dictionary<enumMaterialCategory, Mem_material> material)
		{
			double num = (double)material.get_Item(enumMaterialCategory.Fuel).Value * 0.15;
			material.get_Item(enumMaterialCategory.Fuel).Sub_Material((int)num);
			num = (double)material.get_Item(enumMaterialCategory.Bull).Value * 0.15;
			material.get_Item(enumMaterialCategory.Bull).Sub_Material((int)num);
			num = (double)material.get_Item(enumMaterialCategory.Steel).Value * 0.1;
			material.get_Item(enumMaterialCategory.Steel).Sub_Material((int)num);
			num = (double)material.get_Item(enumMaterialCategory.Bauxite).Value * 0.1;
			material.get_Item(enumMaterialCategory.Bauxite).Sub_Material((int)num);
		}

		private void lostNdock(List<Mem_ndock> ndock)
		{
			ndock.ForEach(delegate(Mem_ndock x)
			{
				int area_id = x.Area_id;
				if (x.Ship_id > 0)
				{
					Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(x.Ship_id);
					mem_ship.SubHp(this.getShipDamage(mem_ship.Nowhp));
					mem_ship.PortWithdraw(area_id);
				}
				Comm_UserDatas.Instance.User_ndock.Remove(x.Rid);
			});
		}

		private void lostKdock()
		{
			Comm_UserDatas.Instance.User_kdock.Clear();
		}

		private void lostMapClear(List<Mem_mapclear> ownMapClear, List<int> neighboringArea)
		{
			ownMapClear.ForEach(delegate(Mem_mapclear x)
			{
				x.StateChange(MapClearState.InvationClose);
			});
			neighboringArea.ForEach(delegate(int area)
			{
				int mapinfo_no = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, area);
				int num = Mst_mapinfo.ConvertMapInfoId(area, mapinfo_no);
				Mem_mapclear mem_mapclear;
				if (Comm_UserDatas.Instance.User_mapclear.TryGetValue(num, ref mem_mapclear) && mem_mapclear.State != MapClearState.InvationClose)
				{
					mem_mapclear.StateChange(MapClearState.InvationNeighbor);
				}
			});
		}

		private void goBackDeck(IEnumerable<Mem_deck> memDeck, List<int> sortieDeck)
		{
			RebellionUtils.<goBackDeck>c__AnonStorey489 <goBackDeck>c__AnonStorey = new RebellionUtils.<goBackDeck>c__AnonStorey489();
			<goBackDeck>c__AnonStorey.sortieDeck = sortieDeck;
			<goBackDeck>c__AnonStorey.<>f__this = this;
			using (IEnumerator<Mem_deck> enumerator = memDeck.GetEnumerator())
			{
				Mem_deck deckItem;
				while (enumerator.MoveNext())
				{
					deckItem = enumerator.get_Current();
					List<Mem_ship> memShip = deckItem.Ship.getMemShip();
					memShip.ForEach(delegate(Mem_ship ship)
					{
						ship.PortWithdraw(deckItem.Area_id);
						if (!<goBackDeck>c__AnonStorey.sortieDeck.Contains(deckItem.Rid))
						{
							ship.SubHp(this.getShipDamage(ship.Nowhp));
						}
					});
					deckItem.MissionEnforceEnd();
					deckItem.MoveArea(1);
				}
			}
		}

		private int getShipDamage(int nowHp)
		{
			double randDouble = Utils.GetRandDouble(1.0, 82.0, 1.0, 1);
			double num = (double)nowHp * (randDouble / 100.0);
			return (int)num;
		}

		public bool MapReOpen(Mem_mapclear clearData, out List<int> reOpenMap)
		{
			reOpenMap = new List<int>();
			if (clearData.State != MapClearState.Cleard)
			{
				return false;
			}
			Dictionary<int, Mem_mapclear> dictionary = Enumerable.ToDictionary<Mem_mapclear, int, Mem_mapclear>(Enumerable.Where<Mem_mapclear>(Comm_UserDatas.Instance.User_mapclear.get_Values(), (Mem_mapclear data) => data.State == MapClearState.InvationClose && data.Mapinfo_no == 1), (Mem_mapclear key) => key.Rid, (Mem_mapclear value) => value);
			if (dictionary.get_Count() == 0)
			{
				return true;
			}
			Dictionary<int, Mst_maparea> mst_maparea = Mst_DataManager.Instance.Mst_maparea;
			int maparea_id = clearData.Maparea_id;
			int num = Mst_maparea.MaxMapNum(Comm_UserDatas.Instance.User_basic.Difficult, clearData.Maparea_id);
			bool flag = clearData.Mapinfo_no == num;
			Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
			Dictionary<int, List<int>> dictionary3 = new Dictionary<int, List<int>>();
			using (Dictionary<int, Mem_mapclear>.ValueCollection.Enumerator enumerator = dictionary.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_mapclear current = enumerator.get_Current();
					Mst_mapinfo mst_mapinfo = Mst_DataManager.Instance.Mst_mapinfo.get_Item(current.Rid);
					if (mst_mapinfo.Required_ids.Contains(clearData.Rid))
					{
						dictionary3.Add(current.Maparea_id, mst_mapinfo.Required_ids);
					}
					else if (flag && mst_maparea.get_Item(current.Maparea_id).Neighboring_area.Contains(maparea_id))
					{
						if (current.Maparea_id == 7 || current.Maparea_id == 8)
						{
							Dictionary<int, List<int>> arg_1A4_0 = dictionary3;
							int arg_1A4_1 = current.Maparea_id;
							List<int> list = new List<int>();
							list.Add(Mst_mapinfo.ConvertMapInfoId(1, 4));
							arg_1A4_0.Add(arg_1A4_1, list);
						}
						else
						{
							List<int> list2 = Enumerable.ToList<int>(mst_mapinfo.Required_ids);
							list2.Add(clearData.Rid);
							dictionary3.Add(current.Maparea_id, list2);
						}
					}
				}
			}
			if (dictionary3.get_Count() == 0)
			{
				return true;
			}
			bool result = false;
			using (Dictionary<int, List<int>>.Enumerator enumerator2 = dictionary3.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, List<int>> current2 = enumerator2.get_Current();
					int areaId = current2.get_Key();
					List<int> value2 = current2.get_Value();
					if (value2.get_Count() == 0)
					{
						throw new Exception();
					}
					if (this.mapReOpenCheck(value2))
					{
						result = true;
						IEnumerable<Mem_mapclear> enumerable = Enumerable.Where<Mem_mapclear>(Comm_UserDatas.Instance.User_mapclear.get_Values(), (Mem_mapclear x) => x.Maparea_id == areaId);
						using (IEnumerator<Mem_mapclear> enumerator3 = enumerable.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Mem_mapclear current3 = enumerator3.get_Current();
								current3.StateChange(MapClearState.InvationOpen);
								reOpenMap.Add(current3.Rid);
							}
						}
					}
				}
			}
			return result;
		}

		private bool mapReOpenCheck(List<int> targetMapInfoIds)
		{
			using (List<int>.Enumerator enumerator = targetMapInfoIds.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					Mem_mapclear mem_mapclear = null;
					if (!Comm_UserDatas.Instance.User_mapclear.TryGetValue(current, ref mem_mapclear))
					{
						bool result = false;
						return result;
					}
					if (mem_mapclear.State != MapClearState.Cleard)
					{
						bool result = false;
						return result;
					}
				}
			}
			return true;
		}

		public int AddPointTo_RPTable(Mst_maparea targetArea)
		{
			if (this.areaOpenState == null)
			{
				this.initAreaOpenState();
			}
			if (!this.areaOpenState.get_Item(targetArea.Id))
			{
				return 0;
			}
			if (!Comm_UserDatas.Instance.User_rebellion_point.ContainsKey(targetArea.Id))
			{
				Mem_rebellion_point mem_rebellion_point = new Mem_rebellion_point(targetArea.Id);
				Comm_UserDatas.Instance.User_rebellion_point.Add(mem_rebellion_point.Rid, mem_rebellion_point);
			}
			Mst_rebellionpoint mstRebellionRecord = this.getMstRebellionRecord();
			if (mstRebellionRecord == null)
			{
				return 0;
			}
			double num = 0.0;
			if (!mstRebellionRecord.Area_value.ContainsKey(targetArea.Id))
			{
				return 0;
			}
			num = (double)mstRebellionRecord.Area_value.get_Item(targetArea.Id);
			List<int> neighboring_area = targetArea.Neighboring_area;
			int num2 = 0;
			using (List<int>.Enumerator enumerator = neighboring_area.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (!this.areaOpenState.get_Item(current))
					{
						num2++;
					}
				}
			}
			if (targetArea.Id == 4 && num2 == 0)
			{
				num2 = 1;
			}
			if (num2 == 0)
			{
				return 0;
			}
			double num3 = (double)neighboring_area.get_Count();
			double num4 = num * (0.5 + 0.5 * ((double)num2 + 1.0) / num3);
			int num5 = (int)(num4 * this.rpsw);
			((IRebellionPointOperator)this).AddRebellionPoint(targetArea.Id, num5);
			return num5;
		}

		private void initAreaOpenState()
		{
			this.areaOpenState = new Dictionary<int, bool>();
			using (Dictionary<int, Mst_maparea>.ValueCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_maparea.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_maparea current = enumerator.get_Current();
					this.areaOpenState.Add(current.Id, current.IsActiveArea());
				}
			}
		}

		private Mst_rebellionpoint getMstRebellionRecord()
		{
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			using (List<Mst_rebellionpoint>.Enumerator enumerator = this.mst_rpoint.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_rebellionpoint current = enumerator.get_Current();
					if (current.Turn_from <= total_turn && current.Turn_to >= total_turn)
					{
						Mst_rebellionpoint result = current;
						return result;
					}
					if (current.Turn_from <= total_turn && current.Turn_to == -1)
					{
						Mst_rebellionpoint result = current;
						return result;
					}
				}
			}
			return null;
		}
	}
}
