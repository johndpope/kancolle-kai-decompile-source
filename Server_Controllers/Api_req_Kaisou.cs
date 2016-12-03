using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Kaisou
	{
		public enum SlotSetChkResult_Ship
		{
			Ok,
			Invalid,
			Mission,
			ActionEndDeck,
			Repair,
			BlingShip
		}

		public enum RemodelingChkResult
		{
			OK,
			Invalid,
			BlingShip,
			ActionEndDeck,
			Mission,
			Repair,
			Level,
			Drawing,
			Steel,
			Bull
		}

		public Api_req_Kaisou.SlotSetChkResult_Ship IsValidSlotSet(int ship_rid)
		{
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				return Api_req_Kaisou.SlotSetChkResult_Ship.Invalid;
			}
			if (mem_ship.IsBlingShip())
			{
				return Api_req_Kaisou.SlotSetChkResult_Ship.BlingShip;
			}
			Mem_deck mem_deck = Enumerable.FirstOrDefault<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.Ship.Find(ship_rid) != -1);
			if (mem_deck != null)
			{
				if (mem_deck.MissionState != MissionStates.NONE)
				{
					return Api_req_Kaisou.SlotSetChkResult_Ship.Mission;
				}
				if (mem_deck.IsActionEnd)
				{
					return Api_req_Kaisou.SlotSetChkResult_Ship.ActionEndDeck;
				}
			}
			if (Enumerable.Any<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Ship_id == ship_rid))
			{
				return Api_req_Kaisou.SlotSetChkResult_Ship.Repair;
			}
			return Api_req_Kaisou.SlotSetChkResult_Ship.Ok;
		}

		public SlotSetChkResult_Slot IsValidSlotSet(int ship_rid, int slot_rid, int equip_idx)
		{
			if (this.IsValidSlotSet(ship_rid) != Api_req_Kaisou.SlotSetChkResult_Ship.Ok)
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			if (mem_ship.Slotnum < equip_idx + 1)
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			int num = mem_ship.Slot.FindIndex((int x) => x == -1);
			if (num != -1 && num < equip_idx)
			{
				equip_idx = num;
			}
			Mem_slotitem mem_slotitem = null;
			if (slot_rid != -1)
			{
				Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, ref mem_slotitem);
				if (mem_slotitem == null)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
				if (mem_slotitem.Equip_flag == Mem_slotitem.enumEquipSts.Equip)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
			}
			else if (slot_rid == -1)
			{
				return SlotSetChkResult_Slot.Ok;
			}
			int num2 = 0;
			int num3 = 0;
			OnslotChangeType slotChangeCost = Mst_slotitem_cost.GetSlotChangeCost(mem_ship.Slot.get_Item(equip_idx), slot_rid, out num2, out num3);
			int onslotKeisu = mem_ship.Onslot.get_Item(equip_idx);
			if (slotChangeCost == OnslotChangeType.OtherToPlane)
			{
				onslotKeisu = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id).Maxeq.get_Item(equip_idx);
			}
			int slotChangeBauxiteNum = Mst_slotitem_cost.GetSlotChangeBauxiteNum(slotChangeCost, num2, num3, onslotKeisu);
			if (slotChangeBauxiteNum > 0)
			{
				return SlotSetChkResult_Slot.Ok;
			}
			if (Math.Abs(slotChangeBauxiteNum) > Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bauxite).Value)
			{
				if (num2 == 0 && num3 > 0)
				{
					return SlotSetChkResult_Slot.NgBauxiteShort;
				}
				if (num2 < num3)
				{
					return SlotSetChkResult_Slot.NgBausiteShortHighCost;
				}
			}
			return SlotSetChkResult_Slot.Ok;
		}

		public Api_Result<SlotSetChkResult_Slot> SlotSet(int ship_rid, int slot_rid, int equip_idx)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = new Api_Result<SlotSetChkResult_Slot>();
			SlotSetChkResult_Slot slotSetChkResult_Slot = this.IsValidSlotSet(ship_rid, slot_rid, equip_idx);
			if (slotSetChkResult_Slot != SlotSetChkResult_Slot.Ok)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				api_Result.data = slotSetChkResult_Slot;
				return api_Result;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			Mem_slotitem mem_slotitem = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, ref mem_slotitem);
			int num = mem_ship.Slot.FindIndex((int x) => x == -1);
			if (num != -1 && num < equip_idx)
			{
				equip_idx = num;
			}
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			Mem_slotitem mem_slotitem2 = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(mem_ship.Slot.get_Item(equip_idx), ref mem_slotitem2);
			int preCost = 0;
			int afterCost = 0;
			OnslotChangeType slotChangeCost = Mst_slotitem_cost.GetSlotChangeCost(mem_ship.Slot.get_Item(equip_idx), slot_rid, out preCost, out afterCost);
			int num2 = mem_ship.Onslot.get_Item(equip_idx);
			if (slotChangeCost == OnslotChangeType.OtherToPlane)
			{
				num2 = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id).Maxeq.get_Item(equip_idx);
			}
			int slotChangeBauxiteNum = Mst_slotitem_cost.GetSlotChangeBauxiteNum(slotChangeCost, preCost, afterCost, num2);
			if (slotChangeBauxiteNum < 0)
			{
				if (slotChangeCost == OnslotChangeType.PlaneToPlane)
				{
					api_Result.data = SlotSetChkResult_Slot.OkBauxiteUseHighCost;
				}
				else if (slotChangeCost == OnslotChangeType.OtherToPlane)
				{
					api_Result.data = SlotSetChkResult_Slot.OkBauxiteUse;
					mem_shipBase.Onslot.set_Item(equip_idx, num2);
				}
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bauxite).Sub_Material(Math.Abs(slotChangeBauxiteNum));
			}
			else
			{
				if (slotChangeCost == OnslotChangeType.PlaneOther)
				{
					mem_shipBase.Onslot.set_Item(equip_idx, 0);
				}
				api_Result.data = SlotSetChkResult_Slot.Ok;
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bauxite).Add_Material(slotChangeBauxiteNum);
			}
			mem_shipBase.Slot.set_Item(equip_idx, slot_rid);
			if (mem_slotitem2 != null)
			{
				mem_slotitem2.UnEquip();
			}
			if (slot_rid != -1)
			{
				mem_slotitem.Equip(ship_rid);
			}
			mem_ship.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id), false);
			if (slot_rid == -1)
			{
				mem_ship.TrimSlot();
			}
			return api_Result;
		}

		public Api_Result<Hashtable> Unslot_all(int ship_rid)
		{
			Api_Result<Hashtable> result = new Api_Result<Hashtable>();
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			for (int i = 0; i < Enumerable.Count<int>(mem_shipBase.Slot); i++)
			{
				Mem_slotitem mem_slotitem = null;
				int num = mem_shipBase.Slot.get_Item(i);
				mem_shipBase.Slot.set_Item(i, -1);
				if (num > 0 && Comm_UserDatas.Instance.User_slot.TryGetValue(num, ref mem_slotitem))
				{
					Mst_slotitem_cost mst_slotitem_cost = null;
					if (Mst_DataManager.Instance.Mst_slotitem_cost.TryGetValue(mem_slotitem.Slotitem_id, ref mst_slotitem_cost))
					{
						int addNum = mst_slotitem_cost.GetAddNum(mem_shipBase.Onslot.get_Item(i));
						Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bauxite).Add_Material(addNum);
					}
					mem_slotitem.UnEquip();
				}
			}
			mem_ship.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id), false);
			mem_ship.TrimSlot();
			return result;
		}

		public bool IsExpandSlotShip(int shipRid)
		{
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(shipRid);
			return mem_ship.Level >= 30 && !mem_ship.IsOpenExSlot();
		}

		public Api_Result<Mem_ship> ExpandSlot(int shipRid)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(shipRid);
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id);
			mem_shipBase.Exslot = -1;
			mem_ship.Set_ShipParam(mem_shipBase, mst_data, false);
			api_Result.data = mem_ship;
			Mem_useitem mem_useitem = null;
			Comm_UserDatas.Instance.User_useItem.TryGetValue(64, ref mem_useitem);
			mem_useitem.Sub_UseItem(1);
			return api_Result;
		}

		public SlotSetChkResult_Slot IsValidSlotSet(int ship_rid, int slot_rid)
		{
			if (this.IsValidSlotSet(ship_rid) != Api_req_Kaisou.SlotSetChkResult_Ship.Ok)
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			if (!mem_ship.IsOpenExSlot())
			{
				return SlotSetChkResult_Slot.NgInvalid;
			}
			Mem_slotitem mem_slotitem = null;
			if (slot_rid != -1)
			{
				Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, ref mem_slotitem);
				if (mem_slotitem == null)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
				if (mem_slotitem.Equip_flag == Mem_slotitem.enumEquipSts.Equip)
				{
					return SlotSetChkResult_Slot.NgInvalid;
				}
			}
			else if (slot_rid == -1)
			{
				return SlotSetChkResult_Slot.Ok;
			}
			return SlotSetChkResult_Slot.Ok;
		}

		public Api_Result<SlotSetChkResult_Slot> SlotSet(int ship_rid, int slot_rid)
		{
			Api_Result<SlotSetChkResult_Slot> api_Result = new Api_Result<SlotSetChkResult_Slot>();
			SlotSetChkResult_Slot slotSetChkResult_Slot = this.IsValidSlotSet(ship_rid, slot_rid);
			if (slotSetChkResult_Slot != SlotSetChkResult_Slot.Ok)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				api_Result.data = slotSetChkResult_Slot;
				return api_Result;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			Mem_slotitem mem_slotitem = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, ref mem_slotitem);
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			Mem_slotitem mem_slotitem2 = null;
			Comm_UserDatas.Instance.User_slot.TryGetValue(mem_ship.Exslot, ref mem_slotitem2);
			mem_shipBase.Exslot = slot_rid;
			if (mem_slotitem2 != null)
			{
				mem_slotitem2.UnEquip();
			}
			if (slot_rid != -1)
			{
				mem_slotitem.Equip(ship_rid);
			}
			mem_ship.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id), false);
			return api_Result;
		}

		public Api_Result<bool> SlotLockChange(int slot_rid)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Mem_slotitem mem_slotitem = null;
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(slot_rid, ref mem_slotitem))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			mem_slotitem.LockChange();
			api_Result.data = mem_slotitem.Lock;
			return api_Result;
		}

		public Api_Result<int> Powerup(int ship_rid, HashSet<int> rid_items)
		{
			Api_Result<int> api_Result = new Api_Result<int>();
			api_Result.data = 0;
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ship.IsBlingShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> list = new List<Mem_ship>();
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary.Add(Mem_ship.enumKyoukaIdx.Houg, 0);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Raig, 0);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Tyku, 0);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Souk, 0);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = dictionary;
			Dictionary<Mem_ship, int> dictionary3 = new Dictionary<Mem_ship, int>();
			Dictionary<int, Mem_deck> user_deck = Comm_UserDatas.Instance.User_deck;
			Dictionary<double, int> dictionary4 = new Dictionary<double, int>();
			using (HashSet<int>.Enumerator enumerator = rid_items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					Mem_ship mem_ship2 = null;
					if (!Comm_UserDatas.Instance.User_ship.TryGetValue(current, ref mem_ship2))
					{
						api_Result.state = Api_Result_State.Parameter_Error;
						Api_Result<int> result = api_Result;
						return result;
					}
					if (mem_ship2.Locked == 1 || mem_ship2.IsBlingShip() || mem_ship2.GetLockSlotItems().get_Count() > 0)
					{
						api_Result.state = Api_Result_State.Parameter_Error;
						Api_Result<int> result = api_Result;
						return result;
					}
					list.Add(mem_ship2);
					Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship2.Ship_id);
					List<int> powup = mst_ship.Powup;
					Dictionary<Mem_ship.enumKyoukaIdx, int> expr_135 = dictionary = dictionary2;
					Mem_ship.enumKyoukaIdx enumKyoukaIdx;
					Mem_ship.enumKyoukaIdx expr_139 = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Houg;
					int num = dictionary.get_Item(enumKyoukaIdx);
					expr_135.set_Item(expr_139, num + powup.get_Item(0));
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary5;
					Dictionary<Mem_ship.enumKyoukaIdx, int> expr_158 = dictionary5 = dictionary2;
					Mem_ship.enumKyoukaIdx expr_15C = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Raig;
					num = dictionary5.get_Item(enumKyoukaIdx);
					expr_158.set_Item(expr_15C, num + powup.get_Item(1));
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary6;
					Dictionary<Mem_ship.enumKyoukaIdx, int> expr_17B = dictionary6 = dictionary2;
					Mem_ship.enumKyoukaIdx expr_17F = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Tyku;
					num = dictionary6.get_Item(enumKyoukaIdx);
					expr_17B.set_Item(expr_17F, num + powup.get_Item(2));
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary7;
					Dictionary<Mem_ship.enumKyoukaIdx, int> expr_19E = dictionary7 = dictionary2;
					Mem_ship.enumKyoukaIdx expr_1A2 = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Souk;
					num = dictionary7.get_Item(enumKyoukaIdx);
					expr_19E.set_Item(expr_1A2, num + powup.get_Item(3));
					double luckUpKeisu = mst_ship.GetLuckUpKeisu();
					if (luckUpKeisu != 0.0)
					{
						if (dictionary4.ContainsKey(luckUpKeisu))
						{
							Dictionary<double, int> dictionary8;
							Dictionary<double, int> expr_1E9 = dictionary8 = dictionary4;
							double num2;
							double expr_1EE = num2 = luckUpKeisu;
							num = dictionary8.get_Item(num2);
							expr_1E9.set_Item(expr_1EE, num + 1);
						}
						else
						{
							dictionary4.Add(luckUpKeisu, 1);
						}
					}
					int[] array = user_deck.get_Item(1).Search_ShipIdx(mem_ship2.Rid);
					if (array[0] != -1 && array[0] == 1 && array[1] == 0)
					{
						api_Result.state = Api_Result_State.Parameter_Error;
						Api_Result<int> result = api_Result;
						return result;
					}
					dictionary3.Add(mem_ship2, array[0]);
				}
			}
			Mst_ship mst_ship2 = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary9 = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg_max);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig_max);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku_max);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk_max);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Luck, mst_ship2.Luck_max);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary10 = dictionary9;
			dictionary9 = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk);
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Luck, mst_ship2.Luck);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary11 = dictionary9;
			Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka = mem_ship.Kyouka;
			dictionary9 = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Houg, kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg));
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Raig, kyouka.get_Item(Mem_ship.enumKyoukaIdx.Raig));
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Tyku, kyouka.get_Item(Mem_ship.enumKyoukaIdx.Tyku));
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Souk, kyouka.get_Item(Mem_ship.enumKyoukaIdx.Souk));
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Kaihi, kyouka.get_Item(Mem_ship.enumKyoukaIdx.Kaihi));
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Taisen, kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taisen));
			dictionary9.Add(Mem_ship.enumKyoukaIdx.Luck, kyouka.get_Item(Mem_ship.enumKyoukaIdx.Luck));
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary12 = dictionary9;
			Random random = new Random();
			using (Dictionary<Mem_ship.enumKyoukaIdx, int>.Enumerator enumerator2 = dictionary2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<Mem_ship.enumKyoukaIdx, int> current2 = enumerator2.get_Current();
					if (current2.get_Value() > 0)
					{
						int num3 = dictionary11.get_Item(current2.get_Key()) + kyouka.get_Item(current2.get_Key());
						if (dictionary10.get_Item(current2.get_Key()) > num3)
						{
							int num4 = random.Next(2);
							int num5 = (int)Math.Floor((double)((float)current2.get_Value() * 0.6f + (float)current2.get_Value() * 0.6f * (float)num4 + 0.3f));
							if (num3 + num5 > dictionary10.get_Item(current2.get_Key()))
							{
								dictionary12.set_Item(current2.get_Key(), dictionary10.get_Item(current2.get_Key()) - dictionary11.get_Item(current2.get_Key()));
							}
							else
							{
								Dictionary<Mem_ship.enumKyoukaIdx, int> expr_4B3 = dictionary9 = dictionary12;
								Mem_ship.enumKyoukaIdx enumKyoukaIdx;
								Mem_ship.enumKyoukaIdx expr_4BD = enumKyoukaIdx = current2.get_Key();
								int num = dictionary9.get_Item(enumKyoukaIdx);
								expr_4B3.set_Item(expr_4BD, num + num5);
							}
						}
					}
				}
			}
			int num6 = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Luck) + dictionary11.get_Item(Mem_ship.enumKyoukaIdx.Luck);
			int num7 = dictionary10.get_Item(Mem_ship.enumKyoukaIdx.Luck);
			if (dictionary4.get_Count() > 0 && dictionary10.get_Item(Mem_ship.enumKyoukaIdx.Luck) > num6)
			{
				double num8 = 0.0;
				using (Dictionary<double, int>.Enumerator enumerator3 = dictionary4.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						KeyValuePair<double, int> current3 = enumerator3.get_Current();
						double num9 = current3.get_Key() * (double)current3.get_Value();
						num8 += num9;
					}
				}
				int num10 = (int)Math.Floor(num8 + Utils.GetRandDouble(0.0, 0.9, 0.1, 2));
				if (num7 < num10 + num6)
				{
					dictionary12.set_Item(Mem_ship.enumKyoukaIdx.Luck, num7 - dictionary11.get_Item(Mem_ship.enumKyoukaIdx.Luck));
				}
				else
				{
					Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary13;
					Dictionary<Mem_ship.enumKyoukaIdx, int> expr_5D7 = dictionary13 = dictionary12;
					Mem_ship.enumKyoukaIdx enumKyoukaIdx;
					Mem_ship.enumKyoukaIdx expr_5DB = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Luck;
					int num = dictionary13.get_Item(enumKyoukaIdx);
					expr_5D7.set_Item(expr_5DB, num + num10);
				}
			}
			bool flag = false;
			using (Dictionary<Mem_ship.enumKyoukaIdx, int>.Enumerator enumerator4 = dictionary12.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					KeyValuePair<Mem_ship.enumKyoukaIdx, int> current4 = enumerator4.get_Current();
					if (kyouka.get_Item(current4.get_Key()) != current4.get_Value())
					{
						flag = true;
					}
				}
			}
			List<Mem_ship> list2 = Enumerable.ToList<Mem_ship>(dictionary3.get_Keys());
			int num11 = 0;
			int num12 = 0;
			int sameShipCount = this.getSameShipCount(mem_ship, list2);
			int num13 = this.selectSamePowerupType(sameShipCount);
			dictionary12.Add(Mem_ship.enumKyoukaIdx.Taik_Powerup, mem_ship.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup));
			if (num13 == 1)
			{
				num12 = this.GetSameShipPowerupLuck(mem_ship, sameShipCount, false);
			}
			else if (num13 == 3)
			{
				num12 = this.GetSameShipPowerupLuck(mem_ship, sameShipCount, true);
			}
			else if (num13 == 2)
			{
				num11 = this.GetSameShipPowerupTaikyu(mem_ship, sameShipCount, false);
			}
			int num14 = mst_ship2.Luck + dictionary12.get_Item(Mem_ship.enumKyoukaIdx.Luck);
			int num15 = mst_ship2.Luck_max - num14;
			int num16 = mst_ship2.Taik + mem_ship.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik) + mem_ship.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup);
			int num17 = mst_ship2.Taik_max - num16;
			if (num12 > num15)
			{
				num12 = num15;
			}
			if (num11 > num17)
			{
				num11 = num17;
			}
			bool flag2 = false;
			if (num12 > 0)
			{
				flag2 = true;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary14;
				Dictionary<Mem_ship.enumKyoukaIdx, int> expr_742 = dictionary14 = dictionary12;
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_746 = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Luck;
				int num = dictionary14.get_Item(enumKyoukaIdx);
				expr_742.set_Item(expr_746, num + num12);
				flag = true;
			}
			if (num11 > 0)
			{
				flag2 = true;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary15;
				Dictionary<Mem_ship.enumKyoukaIdx, int> expr_76E = dictionary15 = dictionary12;
				Mem_ship.enumKyoukaIdx enumKyoukaIdx;
				Mem_ship.enumKyoukaIdx expr_772 = enumKyoukaIdx = Mem_ship.enumKyoukaIdx.Taik_Powerup;
				int num = dictionary15.get_Item(enumKyoukaIdx);
				expr_76E.set_Item(expr_772, num + num11);
				flag = true;
			}
			if (flag)
			{
				Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
				dictionary12.Add(Mem_ship.enumKyoukaIdx.Taik, mem_ship.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik));
				mem_shipBase.SetKyoukaValue(dictionary12);
				mem_ship.Set_ShipParam(mem_shipBase, mst_ship2, false);
				api_Result.data = ((!flag2) ? 1 : 2);
			}
			mem_ship.SumLovToKaisouPowUp(rid_items.get_Count());
			using (Dictionary<Mem_ship, int>.Enumerator enumerator5 = dictionary3.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					KeyValuePair<Mem_ship, int> current5 = enumerator5.get_Current();
					if (current5.get_Value() != -1)
					{
						Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.get_Item(current5.get_Value());
						mem_deck.Ship.RemoveShip(current5.get_Key().Rid);
					}
				}
			}
			Comm_UserDatas.Instance.Remove_Ship(list2);
			QuestKaisou questKaisou = new QuestKaisou(flag);
			questKaisou.ExecuteCheck();
			return api_Result;
		}

		public int GetSameShipPowerupTaikyu(int ship_rid, HashSet<int> rid_items)
		{
			Mem_ship owner = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			List<Mem_ship> list = new List<Mem_ship>();
			using (HashSet<int>.Enumerator enumerator = rid_items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					list.Add(Comm_UserDatas.Instance.User_ship.get_Item(current));
				}
			}
			int sameShipCount = this.getSameShipCount(owner, list);
			return this.GetSameShipPowerupTaikyu(owner, sameShipCount, true);
		}

		private int GetSameShipPowerupTaikyu(Mem_ship owner, int sameNum, bool maxFlag)
		{
			if (owner.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup) >= 3)
			{
				return 0;
			}
			if (sameNum >= 4)
			{
				return 1;
			}
			return 0;
		}

		public int GetSameShipPowerupLuck(int ship_rid, HashSet<int> rid_items)
		{
			Mem_ship owner = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			List<Mem_ship> list = new List<Mem_ship>();
			using (HashSet<int>.Enumerator enumerator = rid_items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					list.Add(Comm_UserDatas.Instance.User_ship.get_Item(current));
				}
			}
			int sameShipCount = this.getSameShipCount(owner, list);
			return this.GetSameShipPowerupLuck(owner, sameShipCount, true);
		}

		private int GetSameShipPowerupLuck(Mem_ship owner, int sameNum, bool maxFlag)
		{
			if (sameNum == 0)
			{
				return 0;
			}
			if (sameNum <= 4)
			{
				return 1;
			}
			if (sameNum >= 5 && maxFlag)
			{
				return 2;
			}
			return 1;
		}

		private int getSameShipCount(Mem_ship owner, List<Mem_ship> feedShip)
		{
			string owner_yomi = Mst_DataManager.Instance.Mst_ship.get_Item(owner.Ship_id).Yomi;
			return Enumerable.Count<Mem_ship>(feedShip, delegate(Mem_ship x)
			{
				string yomi = Mst_DataManager.Instance.Mst_ship.get_Item(x.Ship_id).Yomi;
				return yomi.Equals(owner_yomi);
			});
		}

		public int selectSamePowerupType(int sameNum)
		{
			List<double> rateValues;
			List<int> list3;
			if (sameNum == 1)
			{
				List<double> list = new List<double>();
				list.Add(3.0);
				list.Add(97.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(0);
				list3 = list2;
			}
			else if (sameNum == 2)
			{
				List<double> list = new List<double>();
				list.Add(10.0);
				list.Add(90.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(0);
				list3 = list2;
			}
			else if (sameNum == 3)
			{
				List<double> list = new List<double>();
				list.Add(20.0);
				list.Add(80.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(0);
				list3 = list2;
			}
			else if (sameNum == 4)
			{
				List<double> list = new List<double>();
				list.Add(25.0);
				list.Add(10.0);
				list.Add(65.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(2);
				list2.Add(0);
				list3 = list2;
			}
			else
			{
				if (sameNum != 5)
				{
					return 0;
				}
				List<double> list = new List<double>();
				list.Add(25.0);
				list.Add(15.0);
				list.Add(10.0);
				list.Add(50.0);
				rateValues = list;
				List<int> list2 = new List<int>();
				list2.Add(1);
				list2.Add(2);
				list2.Add(3);
				list2.Add(0);
				list3 = list2;
			}
			int randomRateIndex = Utils.GetRandomRateIndex(rateValues);
			return list3.get_Item(randomRateIndex);
		}

		public Api_req_Kaisou.RemodelingChkResult ValidRemodeling(int ship_rid, out int reqDrawingNum)
		{
			Mem_ship mem_ship = null;
			reqDrawingNum = 0;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				return Api_req_Kaisou.RemodelingChkResult.Invalid;
			}
			Mem_deck mem_deck = Enumerable.FirstOrDefault<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.Ship.Find(ship_rid) != -1);
			if (mem_deck != null)
			{
				if (mem_deck.MissionState != MissionStates.NONE)
				{
					return Api_req_Kaisou.RemodelingChkResult.Mission;
				}
				if (mem_deck.IsActionEnd)
				{
					return Api_req_Kaisou.RemodelingChkResult.ActionEndDeck;
				}
			}
			if (Enumerable.Any<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Ship_id == ship_rid))
			{
				return Api_req_Kaisou.RemodelingChkResult.Repair;
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id);
			if (mst_ship.Aftershipid <= 0)
			{
				return Api_req_Kaisou.RemodelingChkResult.Invalid;
			}
			if (mem_ship.Level < mst_ship.Afterlv)
			{
				return Api_req_Kaisou.RemodelingChkResult.Level;
			}
			if (mem_ship.IsBlingShip())
			{
				return Api_req_Kaisou.RemodelingChkResult.BlingShip;
			}
			Mst_shipupgrade mst_shipupgrade = null;
			if (Mst_DataManager.Instance.Mst_upgrade.TryGetValue(mst_ship.Aftershipid, ref mst_shipupgrade))
			{
				reqDrawingNum = mst_shipupgrade.Drawing_count;
			}
			if (mem_ship.Ship_id == 466 || mem_ship.Ship_id == 467)
			{
				reqDrawingNum = 0;
			}
			if (reqDrawingNum > 0)
			{
				Mem_useitem mem_useitem = null;
				if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(58, ref mem_useitem))
				{
					return Api_req_Kaisou.RemodelingChkResult.Drawing;
				}
				if (mem_useitem.Value < reqDrawingNum)
				{
					return Api_req_Kaisou.RemodelingChkResult.Drawing;
				}
			}
			if (Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel).Value < mst_ship.Afterfuel)
			{
				return Api_req_Kaisou.RemodelingChkResult.Steel;
			}
			if (Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Value < mst_ship.Afterbull)
			{
				return Api_req_Kaisou.RemodelingChkResult.Bull;
			}
			int remodelDevKitNum = mst_ship.GetRemodelDevKitNum();
			if (Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Dev_Kit).Value < remodelDevKitNum)
			{
				return Api_req_Kaisou.RemodelingChkResult.Invalid;
			}
			return Api_req_Kaisou.RemodelingChkResult.OK;
		}

		public Api_Result<Mem_ship> Remodeling(int ship_rid, int drawingNum)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ship.IsBlingShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id);
			int aftershipid = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id).Aftershipid;
			Mst_ship mst_ship2 = Mst_DataManager.Instance.Mst_ship.get_Item(aftershipid);
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary2 = dictionary;
			dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			dictionary.Add(Mem_ship.enumKyoukaIdx.Houg, mst_ship2.Houg_max);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Raig, mst_ship2.Raig_max);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Souk, mst_ship2.Souk_max);
			dictionary.Add(Mem_ship.enumKyoukaIdx.Tyku, mst_ship2.Tyku_max);
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary3 = dictionary;
			Dictionary<Mem_ship.enumKyoukaIdx, int> kyouka = mem_ship.Kyouka;
			Random random = new Random();
			using (Dictionary<Mem_ship.enumKyoukaIdx, int>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship.enumKyoukaIdx, int> current = enumerator.get_Current();
					int num = dictionary2.get_Item(current.get_Key());
					int num2 = dictionary3.get_Item(current.get_Key());
					int num3 = num2 - num;
					double num4 = (double)num3 * (0.4 + 0.4 * (double)random.Next(2)) * (double)mem_ship.Level / 99.0;
					int num5 = (int)Math.Ceiling(num4);
					kyouka.set_Item(current.get_Key(), num5);
					if (num2 < kyouka.get_Item(current.get_Key()) + num)
					{
						kyouka.set_Item(current.get_Key(), num2 - num);
					}
				}
			}
			if (kyouka.get_Item(Mem_ship.enumKyoukaIdx.Luck) + mst_ship2.Luck > mst_ship2.Luck_max)
			{
				kyouka.set_Item(Mem_ship.enumKyoukaIdx.Luck, mst_ship2.Luck_max - mst_ship2.Luck);
			}
			kyouka.set_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup, 0);
			mem_shipBase.C_taik_powerup = 0;
			if (mem_shipBase.Level >= 100)
			{
				int remodelingTaik = this.getRemodelingTaik(mst_ship2.Taik);
				int num6 = mst_ship2.Taik + remodelingTaik;
				if (num6 > mst_ship2.Taik_max)
				{
					num6 = mst_ship2.Taik_max;
				}
				kyouka.set_Item(Mem_ship.enumKyoukaIdx.Taik, num6 - mst_ship2.Taik);
			}
			List<int> list = Comm_UserDatas.Instance.Add_Slot(mst_ship2.Defeq);
			mem_shipBase.Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					Comm_UserDatas.Instance.User_slot.get_Item(x).UnEquip();
				}
			});
			if (mem_ship.Exslot > 0)
			{
				Comm_UserDatas.Instance.User_slot.get_Item(mem_ship.Exslot).UnEquip();
				mem_shipBase.Exslot = -1;
			}
			mem_shipBase.Slot.Clear();
			mem_shipBase.Onslot.Clear();
			for (int i = 0; i < mst_ship2.Slot_num; i++)
			{
				if (list.get_Count() > i)
				{
					mem_shipBase.Slot.Add(list.get_Item(i));
					Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot.get_Item(list.get_Item(i));
					mem_slotitem.Equip(mem_shipBase.Rid);
				}
				else
				{
					mem_shipBase.Slot.Add(mst_ship2.Defeq.get_Item(i));
				}
				mem_shipBase.Onslot.Add(mst_ship2.Maxeq.get_Item(i));
			}
			mem_shipBase.Nowhp = kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik) + mst_ship2.Taik;
			mem_shipBase.Fuel = mst_ship2.Fuel_max;
			mem_shipBase.Bull = mst_ship2.Bull_max;
			mem_shipBase.Cond = 40;
			mem_shipBase.SetKyoukaValue(kyouka);
			mem_ship.Set_ShipParam(mem_shipBase, mst_ship2, false);
			mem_ship.SumLovToRemodeling();
			if (drawingNum > 0)
			{
				Comm_UserDatas.Instance.User_useItem.get_Item(58).Sub_UseItem(drawingNum);
			}
			int remodelDevKitNum = mst_ship2.GetRemodelDevKitNum();
			if (remodelDevKitNum > 0)
			{
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Dev_Kit).Sub_Material(remodelDevKitNum);
			}
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel).Sub_Material(mst_ship.Afterfuel);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Sub_Material(mst_ship.Afterbull);
			Comm_UserDatas.Instance.Add_Book(1, mem_ship.Ship_id);
			api_Result.data = mem_ship;
			return api_Result;
		}

		private int getRemodelingTaik(int now_taik)
		{
			if (now_taik <= 29)
			{
				return 4;
			}
			if (now_taik <= 39)
			{
				return 5;
			}
			if (now_taik <= 49)
			{
				return 6;
			}
			if (now_taik <= 69)
			{
				return 7;
			}
			if (now_taik <= 89)
			{
				return 8;
			}
			return 9;
		}

		public bool ValidMarriage(int ship_rid)
		{
			Mem_ship mem_ship = null;
			return Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship) && mem_ship.Level == 99;
		}

		public Api_Result<Mem_ship> Marriage(int ship_rid)
		{
			Api_Result<Mem_ship> api_Result = new Api_Result<Mem_ship>();
			if (!this.ValidMarriage(ship_rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_useitem mem_useitem = null;
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(55, ref mem_useitem))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_useitem.Value == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(ship_rid);
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id);
			Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship);
			int num = mem_ship.Maxhp - mem_shipBase.C_taik_powerup;
			int num2 = num + this.getMariageTaik(num);
			if (num2 > mst_ship.Taik_max)
			{
				num2 = mst_ship.Taik_max;
			}
			mem_shipBase.C_taik = num2 - mst_ship.Taik;
			mem_shipBase.C_taik_powerup = ((num2 + mem_shipBase.C_taik_powerup <= mst_ship.Taik_max) ? mem_shipBase.C_taik_powerup : (mst_ship.Taik_max - num2));
			num2 += mem_shipBase.C_taik_powerup;
			mem_shipBase.Nowhp = num2;
			int luck = mem_ship.Luck;
			int num3 = (int)Utils.GetRandDouble(3.0, 6.0, 1.0, 1);
			int num4 = luck + num3;
			if (num4 > mst_ship.Luck_max)
			{
				num4 = mst_ship.Luck_max;
			}
			mem_shipBase.C_luck = num4 - mst_ship.Luck;
			mem_shipBase.Level = 100;
			mem_ship.Set_ShipParam(mem_shipBase, mst_ship, false);
			Dictionary<int, int> mst_level = Mst_DataManager.Instance.Get_MstLevel(true);
			mem_ship.SetRequireExp(mem_ship.Level, mst_level);
			mem_ship.SumLovToMarriage();
			Comm_UserDatas.Instance.Ship_book.get_Item(mem_ship.Ship_id).UpdateShipBook(false, true);
			Comm_UserDatas.Instance.User_useItem.get_Item(55).Sub_UseItem(1);
			api_Result.data = mem_ship;
			return api_Result;
		}

		private int getMariageTaik(int now_taik)
		{
			return this.getRemodelingTaik(now_taik);
		}
	}
}
