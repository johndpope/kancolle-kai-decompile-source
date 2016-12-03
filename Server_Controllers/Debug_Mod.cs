using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Debug_Mod : IRebellionPointOperator
	{
		public Debug_Mod(List<int> firstShips)
		{
		}

		public Debug_Mod()
		{
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point.get_Item(area_id).AddPoint(this, addNum);
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			Comm_UserDatas.Instance.User_rebellion_point.get_Item(area_id).SubPoint(this, subNum);
		}

		public void Add_EscortDeck(int area_id)
		{
			Comm_UserDatas.Instance.Add_EscortDeck(area_id, area_id);
		}

		public void Add_Deck(int rid)
		{
			Comm_UserDatas.Instance.Add_Deck(rid);
			LocalManager localManager = new LocalManager();
			if (localManager.UserInfo != null)
			{
				localManager.UserInfo.__UpdateDeck__();
			}
		}

		public List<int> Add_Ship(List<int> ship_ids)
		{
			List<int> result = Comm_UserDatas.Instance.Add_Ship(ship_ids);
			LocalManager localManager = new LocalManager();
			if (localManager.UserInfo != null)
			{
				localManager.UserInfo.__UpdateShips__();
			}
			return result;
		}

		public List<int> Add_SlotItem(List<int> slot_ids)
		{
			return Comm_UserDatas.Instance.Add_Slot(slot_ids);
		}

		public void Set_ShipChargeData(int rid, int fuel, int bull, List<int> onslot)
		{
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(rid, ref mem_ship))
			{
				return;
			}
			mem_ship.Set_ChargeData(bull, fuel, onslot);
		}

		public void Add_UseItem(Dictionary<int, int> add_items)
		{
			using (Dictionary<int, int>.Enumerator enumerator = add_items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					Comm_UserDatas.Instance.Add_Useitem(current.get_Key(), current.get_Value());
				}
			}
		}

		public void Add_Port()
		{
			Comm_UserDatas.Instance.User_basic.PortExtend(1);
		}

		public void Add_Materials(enumMaterialCategory category, int count)
		{
			Comm_UserDatas.Instance.User_material.get_Item(category).Add_Material(count);
		}

		public void Add_Spoint(int count)
		{
			if (count > 0)
			{
				Comm_UserDatas.Instance.User_basic.AddPoint(count);
			}
			else if (count < 0)
			{
				int subNum = Math.Abs(count);
				Comm_UserDatas.Instance.User_basic.SubPoint(subNum);
			}
		}

		public void Add_Coin(int count)
		{
			Comm_UserDatas.Instance.User_basic.AddCoin(count);
		}

		public void ShipAddExp(List<Mem_ship> ships, List<int> addExps)
		{
			int count = addExps.get_Count();
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(true);
			for (int i = 0; i < count; i++)
			{
				Mem_shipBase mem_shipBase = new Mem_shipBase(ships.get_Item(i));
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id);
				List<int> list = null;
				int num = addExps.get_Item(i);
				int levelupInfo = ships.get_Item(i).getLevelupInfo(dictionary, ships.get_Item(i).Level, ships.get_Item(i).Exp, ref num, out list);
				mem_shipBase.Level = levelupInfo;
				mem_shipBase.Exp += num;
				int num2 = levelupInfo - ships.get_Item(i).Level;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = ships.get_Item(i).Kyouka;
				for (int j = 0; j < num2; j++)
				{
					dictionary2 = ships.get_Item(i).getLevelupKyoukaValue(ships.get_Item(i).Ship_id, dictionary2);
				}
				mem_shipBase.SetKyoukaValue(dictionary2);
				int num3 = 0;
				int num4 = 0;
				dictionary.TryGetValue(mem_shipBase.Level - 1, ref num3);
				dictionary.TryGetValue(mem_shipBase.Level + 1, ref num4);
				ships.get_Item(i).SetRequireExp(mem_shipBase.Level, dictionary);
				ships.get_Item(i).Set_ShipParam(mem_shipBase, mst_data, false);
			}
		}

		public static void ShipAddExp_To_MariageLevel(int ship_rid)
		{
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				return;
			}
			if (mem_ship.Level >= 99)
			{
				return;
			}
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(true);
			if (mem_ship.Level < 99)
			{
				int num = dictionary.get_Item(99) - mem_ship.Exp;
				Debug_Mod debug_Mod = new Debug_Mod();
				Debug_Mod arg_7A_0 = debug_Mod;
				List<Mem_ship> list = new List<Mem_ship>();
				list.Add(mem_ship);
				List<Mem_ship> arg_7A_1 = list;
				List<int> list2 = new List<int>();
				list2.Add(num);
				arg_7A_0.ShipAddExp(arg_7A_1, list2);
			}
		}

		public void AddFurniture(List<int> mst_id)
		{
			mst_id.ForEach(delegate(int x)
			{
				if (!Comm_UserDatas.Instance.User_furniture.ContainsKey(x) && Mst_DataManager.Instance.Mst_furniture.ContainsKey(x))
				{
					Comm_UserDatas.Instance.User_furniture.Add(x, new Mem_furniture(x));
				}
			});
		}

		public static void SubHp(int rid, int subvalue)
		{
			Comm_UserDatas.Instance.User_ship.get_Item(rid).SubHp(subvalue);
		}

		public static void DeckRefresh(int DeckID)
		{
			List<Mem_ship> memShip = Comm_UserDatas.Instance.User_deck.get_Item(DeckID).Ship.getMemShip();
			using (List<Mem_ship>.Enumerator enumerator = memShip.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					current.SubHp(-(current.Maxhp - current.Nowhp));
					Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id);
					current.Set_ChargeData(mst_ship.Bull_max, mst_ship.Fuel_max, Enumerable.ToList<int>(mst_ship.Maxeq));
				}
			}
		}

		public static void OpenLargeDock()
		{
			Comm_UserDatas.Instance.User_basic.OpenLargeDock();
		}

		public static void OpenMapArea(int maparea_id, int mapinfo_no)
		{
			if (Comm_UserDatas.Instance.User_basic.Starttime == 0)
			{
				return;
			}
			if (!Mst_DataManager.Instance.Mst_maparea.ContainsKey(maparea_id))
			{
				return;
			}
			int num = Mst_mapinfo.ConvertMapInfoId(maparea_id, mapinfo_no);
			if (!Mst_DataManager.Instance.Mst_mapinfo.ContainsKey(num))
			{
				return;
			}
			Mem_mapclear mem_mapclear = null;
			if (Comm_UserDatas.Instance.User_mapclear.TryGetValue(num, ref mem_mapclear))
			{
				if (mem_mapclear.State == MapClearState.InvationClose)
				{
					return;
				}
				if (mem_mapclear.State != MapClearState.Cleard)
				{
					mem_mapclear.StateChange(MapClearState.Cleard);
				}
			}
			else
			{
				mem_mapclear = new Mem_mapclear(num, maparea_id, mapinfo_no, MapClearState.Cleard);
				mem_mapclear.Insert();
			}
			if (Utils.IsGameClear() && Comm_UserDatas.Instance.User_kdock.get_Count() > 0)
			{
				Comm_UserDatas.Instance.User_record.AddClearDifficult(Comm_UserDatas.Instance.User_basic.Difficult);
			}
			if (maparea_id != 1 && mapinfo_no == 1 && Mem_history.IsFirstOpenArea(num))
			{
				Mem_history mem_history = new Mem_history();
				mem_history.SetAreaOpen(Comm_UserDatas.Instance.User_turn.Total_turn, num);
				Comm_UserDatas.Instance.Add_History(mem_history);
			}
			List<int> list = new List<int>();
			new RebellionUtils().MapReOpen(mem_mapclear, out list);
		}

		public static void OpenMapAreaAll()
		{
			using (Dictionary<int, Mst_mapinfo>.ValueCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_mapinfo.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_mapinfo current = enumerator.get_Current();
					if (!Comm_UserDatas.Instance.User_mapclear.ContainsKey(current.Id))
					{
						Mem_mapclear mem_mapclear = new Mem_mapclear(current.Id, current.Maparea_id, current.No, MapClearState.Cleard);
						mem_mapclear.Insert();
					}
					if (current.Maparea_id != 1 && current.No == 1 && Mem_history.IsFirstOpenArea(current.Id))
					{
						Mem_history mem_history = new Mem_history();
						mem_history.SetAreaOpen(Comm_UserDatas.Instance.User_turn.Total_turn, current.Id);
						Comm_UserDatas.Instance.Add_History(mem_history);
					}
				}
			}
			if (Utils.IsGameClear() && Comm_UserDatas.Instance.User_kdock.get_Count() > 0)
			{
				Comm_UserDatas.Instance.User_record.AddClearDifficult(Comm_UserDatas.Instance.User_basic.Difficult);
			}
			new Api_get_Member().StrategyInfo();
		}

		public static void Add_Tunker(int num)
		{
			Comm_UserDatas.Instance.Add_Tanker(num);
		}

		public static void Add_ShipAll()
		{
			List<int> mst_list = new List<int>();
			Enumerable.ToList<Mst_ship>(Mst_DataManager.Instance.Mst_ship.get_Values()).ForEach(delegate(Mst_ship x)
			{
				if (x.Id < 500 && x.Sortno != 0)
				{
					mst_list.Add(x.Id);
				}
			});
			Comm_UserDatas.Instance.Add_Ship(mst_list);
			LocalManager localManager = new LocalManager();
			if (localManager.UserInfo != null)
			{
				localManager.UserInfo.__UpdateShips__();
			}
		}

		public static void Add_SlotItemAll()
		{
			List<int> mst_list = new List<int>();
			Enumerable.ToList<Mst_slotitem>(Mst_DataManager.Instance.Mst_Slotitem.get_Values()).ForEach(delegate(Mst_slotitem x)
			{
				if (x.Id < 500)
				{
					mst_list.Add(x.Id);
				}
			});
			Comm_UserDatas.Instance.Add_Slot(mst_list);
		}

		public static void MissionOpenToMissionId(int missionId)
		{
			Mst_mission2 master = Mst_DataManager.Instance.Mst_mission.get_Item(missionId);
			Mem_missioncomp mem_missioncomp = new Mem_missioncomp(master.Id, master.Maparea_id, MissionClearKinds.CLEARED);
			List<User_MissionFmt> activeMission = mem_missioncomp.GetActiveMission();
			if (!Enumerable.Any<User_MissionFmt>(activeMission, (User_MissionFmt x) => x.MissionId == master.Id))
			{
				return;
			}
			Mem_missioncomp mem_missioncomp2 = null;
			if (Comm_UserDatas.Instance.User_missioncomp.TryGetValue(missionId, ref mem_missioncomp2))
			{
				mem_missioncomp.Update();
			}
			else
			{
				mem_missioncomp.Insert();
			}
			mem_missioncomp.GetActiveMission();
		}

		public static void MissionOpenToArea(int areaId)
		{
			IEnumerable<Mst_mission2> enumerable = Enumerable.Where<Mst_mission2>(Mst_DataManager.Instance.Mst_mission.get_Values(), (Mst_mission2 x) => x.Maparea_id == areaId);
			using (IEnumerator<Mst_mission2> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_mission2 current = enumerator.get_Current();
					Mem_missioncomp mem_missioncomp = new Mem_missioncomp(current.Id, areaId, MissionClearKinds.CLEARED);
					if (!Comm_UserDatas.Instance.User_missioncomp.ContainsKey(current.Id))
					{
						mem_missioncomp.Insert();
					}
					else
					{
						mem_missioncomp.Update();
					}
				}
			}
		}

		public static void ChangeSlotLevel(int slot_rid, int level)
		{
			if (level > 10)
			{
				level = 10;
			}
			if (level < 0)
			{
				level = 0;
			}
			Mem_slotitem mem_slot;
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, ref mem_slot))
			{
				return;
			}
			Dictionary<int, List<Mst_slotitem_remodel>> mst_slotitem_remodel = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
			if (!Enumerable.Any<List<Mst_slotitem_remodel>>(mst_slotitem_remodel.get_Values(), (List<Mst_slotitem_remodel> x) => Enumerable.Any<Mst_slotitem_remodel>(x, (Mst_slotitem_remodel y) => y.Slotitem_id == mem_slot.Slotitem_id)))
			{
				return;
			}
			mem_slot.SetLevel(level);
		}

		public static List<int> GetEnableSlotChangeItem()
		{
			if (Comm_UserDatas.Instance.User_slot.get_Count() == 0)
			{
				return new List<int>();
			}
			Dictionary<int, List<Mst_slotitem_remodel>> dict = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
			IEnumerable<Mem_slotitem> enumerable = Enumerable.Where<Mem_slotitem>(Comm_UserDatas.Instance.User_slot.get_Values(), (Mem_slotitem x) => Enumerable.Any<List<Mst_slotitem_remodel>>(dict.get_Values(), (List<Mst_slotitem_remodel> list) => Enumerable.Any<Mst_slotitem_remodel>(list, (Mst_slotitem_remodel item) => item.Slotitem_id == x.Slotitem_id)));
			if (Enumerable.Count<Mem_slotitem>(enumerable) == 0)
			{
				return new List<int>();
			}
			return Enumerable.ToList<int>(Enumerable.Select<Mem_slotitem, int>(enumerable, (Mem_slotitem ret_item) => ret_item.Rid));
		}

		public static void DeckPracticeMenu_StateChange(DeckPracticeType type, bool state)
		{
			Comm_UserDatas.Instance.User_deckpractice.StateChange(type, state);
		}

		public static void SetRebellionPoint(int area_id, int num)
		{
			if (Comm_UserDatas.Instance.User_rebellion_point.get_Count() == 0)
			{
				new Api_get_Member().StrategyInfo();
			}
			Mem_rebellion_point mem_rebellion_point = null;
			if (!Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(area_id, ref mem_rebellion_point))
			{
				return;
			}
			if (mem_rebellion_point.Point < num)
			{
				int addNum = num - mem_rebellion_point.Point;
				((IRebellionPointOperator)new Debug_Mod()).AddRebellionPoint(area_id, addNum);
			}
			else
			{
				int subNum = (mem_rebellion_point.Point - num > 0) ? (mem_rebellion_point.Point - num) : mem_rebellion_point.Point;
				((IRebellionPointOperator)new Debug_Mod()).SubRebellionPoint(area_id, subNum);
			}
		}

		public static void SetShipKyoukaValue(int ship_id, PowUpInfo powerUpValues)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_id);
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id);
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			mem_shipBase.C_houg = ((powerUpValues.Karyoku - mst_ship.Houg <= 0) ? mem_shipBase.C_houg : (powerUpValues.Karyoku - mst_ship.Houg));
			mem_shipBase.C_raig = ((powerUpValues.Raisou - mst_ship.Raig <= 0) ? mem_shipBase.C_raig : (powerUpValues.Raisou - mst_ship.Raig));
			mem_shipBase.C_kaihi = ((powerUpValues.Kaihi - mst_ship.Kaih <= 0) ? mem_shipBase.C_kaihi : (powerUpValues.Kaihi - mst_ship.Kaih));
			mem_shipBase.C_luck = ((powerUpValues.Lucky - mst_ship.Luck <= 0) ? mem_shipBase.C_luck : (powerUpValues.Lucky - mst_ship.Luck));
			mem_shipBase.C_souk = ((powerUpValues.Soukou - mst_ship.Souk <= 0) ? mem_shipBase.C_souk : (powerUpValues.Soukou - mst_ship.Souk));
			mem_shipBase.C_tyku = ((powerUpValues.Taiku - mst_ship.Tyku <= 0) ? mem_shipBase.C_tyku : (powerUpValues.Taiku - mst_ship.Tyku));
			mem_shipBase.C_taisen = ((powerUpValues.Taisen - mst_ship.Tais <= 0) ? mem_shipBase.C_taisen : (powerUpValues.Taisen - mst_ship.Tais));
			mem_ship.Set_ShipParam(mem_shipBase, mst_ship, false);
		}

		public static void SetFleetLevel(int fleetLevel)
		{
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			if (user_record.Level == fleetLevel)
			{
				return;
			}
			Dictionary<int, int> mstLevelUser = ArrayMaster.GetMstLevelUser();
			int num = 0;
			if (!mstLevelUser.TryGetValue(fleetLevel, ref num))
			{
				return;
			}
			uint exp = user_record.Exp;
			int num2 = (int)((long)num - (long)((ulong)exp));
			if (num2 < 0)
			{
				user_record.GetType().InvokeMember("_level", 2084, null, user_record, new object[]
				{
					1
				});
				user_record.GetType().InvokeMember("_exp", 2084, null, user_record, new object[]
				{
					0u
				});
				num2 = num;
			}
			user_record.UpdateExp(num2, mstLevelUser);
		}

		public static void SetRebellionPhase(int phase)
		{
			if (phase == 0 || phase > 6)
			{
				return;
			}
			int turn_from = Mst_DataManager.Instance.Mst_RebellionPoint.get_Item(phase).Turn_from;
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			user_turn.GetType().InvokeMember("_total_turn", 2084, null, user_turn, new object[]
			{
				turn_from - 1
			});
			user_turn.GetType().InvokeMember("_reqQuestReset", 2084, null, user_turn, new object[]
			{
				true
			});
		}

		public static void SetRadingPhase(int phase)
		{
			if (phase > 4 || phase < 1)
			{
				return;
			}
			List<Mst_radingtype> list = Mst_DataManager.Instance.Mst_RadingType.get_Item((int)Comm_UserDatas.Instance.User_basic.Difficult);
			int turn_from;
			if (phase == 3)
			{
				turn_from = list.get_Item(0).Turn_from;
			}
			else if (phase == 2)
			{
				turn_from = list.get_Item(1).Turn_from;
			}
			else
			{
				turn_from = list.get_Item(2).Turn_from;
			}
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			user_turn.GetType().InvokeMember("_total_turn", 2084, null, user_turn, new object[]
			{
				turn_from - 1
			});
			user_turn.GetType().InvokeMember("_reqQuestReset", 2084, null, user_turn, new object[]
			{
				true
			});
		}

		public static List<int> Check_CreateItemData(int typeNo)
		{
			if (typeNo > 3)
			{
				return null;
			}
			Api_req_Kousyou api_req_Kousyou = new Api_req_Kousyou();
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_slotitemget2", "mst_slotitemget2", "Id");
			api_req_Kousyou.GetType().InvokeMember("createItemTable", 2084, null, api_req_Kousyou, new object[]
			{
				enumerable
			});
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 0);
			dictionary.Add(enumMaterialCategory.Bull, 0);
			dictionary.Add(enumMaterialCategory.Steel, 0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0);
			List<int> list = new List<int>();
			dictionary.set_Item(enumMaterialCategory.Fuel, 100);
			dictionary.set_Item(enumMaterialCategory.Bull, 0);
			dictionary.set_Item(enumMaterialCategory.Steel, 0);
			dictionary.set_Item(enumMaterialCategory.Bauxite, 0);
			int num = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", 292, null, api_req_Kousyou, new object[]
			{
				typeNo,
				dictionary
			});
			list.Add(num);
			dictionary.set_Item(enumMaterialCategory.Fuel, 0);
			dictionary.set_Item(enumMaterialCategory.Bull, 100);
			dictionary.set_Item(enumMaterialCategory.Steel, 0);
			dictionary.set_Item(enumMaterialCategory.Bauxite, 0);
			int num2 = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", 292, null, api_req_Kousyou, new object[]
			{
				typeNo,
				dictionary
			});
			list.Add(num2);
			dictionary.set_Item(enumMaterialCategory.Fuel, 0);
			dictionary.set_Item(enumMaterialCategory.Bull, 0);
			dictionary.set_Item(enumMaterialCategory.Steel, 100);
			dictionary.set_Item(enumMaterialCategory.Bauxite, 0);
			int num3 = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", 292, null, api_req_Kousyou, new object[]
			{
				typeNo,
				dictionary
			});
			list.Add(num3);
			dictionary.set_Item(enumMaterialCategory.Fuel, 0);
			dictionary.set_Item(enumMaterialCategory.Bull, 0);
			dictionary.set_Item(enumMaterialCategory.Steel, 0);
			dictionary.set_Item(enumMaterialCategory.Bauxite, 100);
			int num4 = (int)api_req_Kousyou.GetType().InvokeMember("getCreateItemId", 292, null, api_req_Kousyou, new object[]
			{
				typeNo,
				dictionary
			});
			list.Add(num4);
			return list;
		}
	}
}
