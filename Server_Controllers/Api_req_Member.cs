using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Member : ICreateNewUser
	{
		private bool create_flag;

		public void PurgeUserData()
		{
			Comm_UserDatas.Instance.PurgeUserData(this, false);
		}

		public HashSet<DifficultKind> GetClearDifficult()
		{
			HashSet<DifficultKind> hashSet = new HashSet<DifficultKind>();
			if (Comm_UserDatas.Instance.User_record == null)
			{
				return hashSet;
			}
			hashSet.UnionWith(Enumerable.ToList<DifficultKind>(Comm_UserDatas.Instance.User_record.ClearDifficult));
			return hashSet;
		}

		public Api_Result<bool> CreateNewUser(int saveno, string nickName, int ship_id, DifficultKind difficult)
		{
			Api_Result<bool> result = new Api_Result<bool>();
			this.create_flag = true;
			this.CreateNewUser(difficult, ship_id);
			this.create_flag = false;
			this.Update_NickName(nickName);
			return result;
		}

		public bool CreateNewUser(DifficultKind difficult, int firstShip)
		{
			return this.create_flag && Comm_UserDatas.Instance.CreateNewUser(this, difficult, firstShip);
		}

		public Api_Result<bool> NewGamePlus(string nickName, DifficultKind difficult, int firstShipId)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			api_Result.state = Api_Result_State.Parameter_Error;
			if (!Utils.IsValidNewGamePlus())
			{
				return api_Result;
			}
			if (!Comm_UserDatas.Instance.NewGamePlus(this, nickName, difficult, firstShipId))
			{
				return api_Result;
			}
			api_Result.state = Api_Result_State.Success;
			return api_Result;
		}

		private Api_Result<bool> SetDifficult(DifficultKind difficult)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Comm_UserDatas.Instance.User_basic.SetDifficult(difficult);
			api_Result.data = true;
			return api_Result;
		}

		public Api_Result<Hashtable> Update_DeckName(int deck_id, string name)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			api_Result.data = null;
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			mem_deck.SetDeckName(name);
			return api_Result;
		}

		public Api_Result<Hashtable> Update_NickName(string name)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			api_Result.data = new Hashtable();
			if (Comm_UserDatas.Instance.User_basic == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Comm_UserDatas.Instance.User_basic.UpdateNickName(name);
			return api_Result;
		}

		public Api_Result<Hashtable> Update_Comment(string comment)
		{
			Api_Result<Hashtable> api_Result = new Api_Result<Hashtable>();
			api_Result.data = new Hashtable();
			if (Comm_UserDatas.Instance.User_basic == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Comm_UserDatas.Instance.User_basic.UpdateComment(comment);
			return api_Result;
		}

		public Api_Result<User_ItemUseFmt> ItemUse(int useitem_id, bool force_flag, ItemExchangeKinds exchange_type)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			Dictionary<ItemGetKinds, Dictionary<int, int>> dictionary2 = new Dictionary<ItemGetKinds, Dictionary<int, int>>();
			Dictionary<int, Dictionary<int, int>> dictionary3 = new Dictionary<int, Dictionary<int, int>>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(ItemGetKinds)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					dictionary2.Add((ItemGetKinds)((int)current), new Dictionary<int, int>());
				}
			}
			for (int i = 1; i <= 3; i++)
			{
				dictionary3.Add(i, new Dictionary<int, int>());
			}
			Api_Result<User_ItemUseFmt> api_Result = new Api_Result<User_ItemUseFmt>();
			api_Result.data = new User_ItemUseFmt();
			Mst_useitem mst_useitem = null;
			if (!Mst_DataManager.Instance.Mst_useitem.TryGetValue(useitem_id, ref mst_useitem))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mst_useitem.Usetype != 4)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_useitem mem_useitem;
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(useitem_id, ref mem_useitem))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int num = mst_useitem.GetItemExchangeNum(exchange_type);
			if (num == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_useitem.Value < num)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, Mem_material> user_material = Comm_UserDatas.Instance.User_material;
			IEnumerable<XElement> enumerable = this.get_package_item(exchange_type, mst_useitem);
			if (enumerable != null)
			{
				Mst_item_limit mst_item_limit = Mst_DataManager.Instance.Mst_item_limit.get_Item(1);
				var enumerable2 = Enumerable.Select(Enumerable.Select(enumerable, (XElement item) => new
				{
					item = item,
					category = (enumMaterialCategory)((int)Enum.Parse(typeof(enumMaterialCategory), item.Element("Material_id").get_Value()))
				}), <>__TranspIdent6 => new
				{
					material_id = <>__TranspIdent6.category,
					useitem_id = int.Parse(<>__TranspIdent6.item.Element("Useitem_id").get_Value()),
					slotitem_id = int.Parse(<>__TranspIdent6.item.Element("Slotitem_id").get_Value()),
					items = int.Parse(<>__TranspIdent6.item.Element("Items").get_Value()),
					max_items = mst_item_limit.GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, <>__TranspIdent6.category)
				});
				using (var enumerator2 = enumerable2.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						var current2 = enumerator2.get_Current();
						if (current2.material_id != (enumMaterialCategory)0)
						{
							int num2 = this.LimitGetCount(user_material.get_Item(current2.material_id).Value, current2.items, current2.max_items);
							if (!force_flag && num2 != current2.items)
							{
								api_Result.data.CautionFlag = true;
								return api_Result;
							}
							dictionary.Add(current2.material_id, current2.items);
							dictionary3.get_Item(1).Add((int)current2.material_id, num2);
						}
						else if (current2.useitem_id != 0)
						{
							int now_count = 0;
							Mem_useitem mem_useitem2;
							if (Comm_UserDatas.Instance.User_useItem.TryGetValue(current2.useitem_id, ref mem_useitem2))
							{
								now_count = mem_useitem2.Value;
							}
							int num3 = this.LimitGetCount(now_count, current2.items, current2.max_items);
							dictionary2.get_Item(ItemGetKinds.UseItem).Add(current2.useitem_id, current2.items);
							dictionary3.get_Item(2).Add(current2.useitem_id, num3);
						}
						else if (current2.slotitem_id != 0)
						{
							dictionary2.get_Item(ItemGetKinds.SlotItem).Add(current2.slotitem_id, current2.items);
							dictionary3.get_Item(3).Add(current2.slotitem_id, current2.items);
						}
					}
				}
				using (Dictionary<int, Dictionary<int, int>>.Enumerator enumerator3 = dictionary3.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						KeyValuePair<int, Dictionary<int, int>> current3 = enumerator3.get_Current();
						int key = current3.get_Key();
						using (Dictionary<int, int>.Enumerator enumerator4 = current3.get_Value().GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								KeyValuePair<int, int> current4 = enumerator4.get_Current();
								int key2 = current4.get_Key();
								int value = current4.get_Value();
								if (key == 1)
								{
									enumMaterialCategory enumMaterialCategory = (enumMaterialCategory)key2;
									user_material.get_Item(enumMaterialCategory).Add_Material(value);
								}
								else if (key == 2)
								{
									Comm_UserDatas.Instance.Add_Useitem(key2, value);
								}
								else if (key == 3)
								{
									Comm_UserDatas arg_3D7_0 = Comm_UserDatas.Instance;
									List<int> list = new List<int>();
									list.Add(key2);
									arg_3D7_0.Add_Slot(list);
								}
							}
						}
					}
				}
			}
			else if (mst_useitem.Id == 10 || mst_useitem.Id == 11 || mst_useitem.Id == 12)
			{
				Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
				int get_count = int.Parse(mst_useitem.Description2) * mem_useitem.Value;
				int max_count = 200000;
				int num4 = this.LimitGetCount(user_basic.Fcoin, get_count, max_count);
				dictionary2.get_Item(ItemGetKinds.UseItem).Add(44, num4);
				user_basic.AddCoin(num4);
				num = mem_useitem.Value;
			}
			else if (mst_useitem.Id == 53)
			{
				Mem_basic user_basic2 = Comm_UserDatas.Instance.User_basic;
				if (user_basic2.Max_chara >= user_basic2.GetPortMaxExtendNum())
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				dictionary2.get_Item(ItemGetKinds.UseItem).Add(53, 1);
				user_basic2.PortExtend(1);
			}
			mem_useitem.Sub_UseItem(num);
			api_Result.data.GetItem = this.GetItemFmt(dictionary2);
			api_Result.data.Material = dictionary;
			return api_Result;
		}

		private IEnumerable<XElement> get_package_item(ItemExchangeKinds exchange_type, Mst_useitem mst_useitem)
		{
			List<XElement> result = null;
			if (exchange_type == ItemExchangeKinds.NONE)
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("Package_type", "2");
				dictionary.Add("Package_id", mst_useitem.Id.ToString());
				Dictionary<string, string> where_dict = dictionary;
				return Utils.Xml_Result_Where("mst_item_package", "mst_item_package", where_dict);
			}
			if (exchange_type == ItemExchangeKinds.PLAN)
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new object[]
				{
					new XElement("Id", "101"),
					new XElement("Package_type", "2"),
					new XElement("Package_id", "57"),
					new XElement("Material_id", "0"),
					new XElement("Useitem_id", "58"),
					new XElement("Slotitem_id", "0"),
					new XElement("Items", "1")
				}));
				result = list;
			}
			else if (exchange_type == ItemExchangeKinds.REMODEL)
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new object[]
				{
					new XElement("Id", "102"),
					new XElement("Package_type", "2"),
					new XElement("Package_id", "57"),
					new XElement("Material_id", "8"),
					new XElement("Useitem_id", "0"),
					new XElement("Slotitem_id", "0"),
					new XElement("Items", "4")
				}));
				result = list;
			}
			else if (exchange_type == ItemExchangeKinds.PRESENT_MATERIAL)
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new object[]
				{
					new XElement("Id", "103"),
					new XElement("Package_type", "2"),
					new XElement("Package_id", "60"),
					new XElement("Material_id", "7"),
					new XElement("Useitem_id", "0"),
					new XElement("Slotitem_id", "0"),
					new XElement("Items", "3")
				}));
				list.Add(new XElement("mst_item_package", new object[]
				{
					new XElement("Id", "104"),
					new XElement("Package_type", "2"),
					new XElement("Package_id", "60"),
					new XElement("Material_id", "8"),
					new XElement("Useitem_id", "0"),
					new XElement("Slotitem_id", "0"),
					new XElement("Items", "1")
				}));
				result = list;
			}
			else if (exchange_type == ItemExchangeKinds.PRESENT_IRAKO)
			{
				List<XElement> list = new List<XElement>();
				list.Add(new XElement("mst_item_package", new object[]
				{
					new XElement("Id", "105"),
					new XElement("Package_type", "2"),
					new XElement("Package_id", "60"),
					new XElement("Material_id", "0"),
					new XElement("Useitem_id", "59"),
					new XElement("Slotitem_id", "0"),
					new XElement("Items", "1")
				}));
				result = list;
			}
			return result;
		}

		private int LimitGetCount(int now_count, int get_count, int max_count)
		{
			int result = get_count;
			int num = now_count + get_count;
			if (num > max_count)
			{
				result = max_count - now_count;
			}
			return result;
		}

		private List<ItemGetFmt> GetItemFmt(Dictionary<ItemGetKinds, Dictionary<int, int>> fmt_base)
		{
			List<ItemGetFmt> list = new List<ItemGetFmt>();
			using (Dictionary<ItemGetKinds, Dictionary<int, int>>.Enumerator enumerator = fmt_base.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<ItemGetKinds, Dictionary<int, int>> current = enumerator.get_Current();
					ItemGetKinds key = current.get_Key();
					using (Dictionary<int, int>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<int, int> current2 = enumerator2.get_Current();
							if (current2.get_Key() > 0)
							{
								list.Add(new ItemGetFmt
								{
									Category = key,
									Id = current2.get_Key(),
									Count = current2.get_Value()
								});
							}
						}
					}
				}
			}
			return list;
		}

		public bool[] itemuse_cond_check(int deck_id)
		{
			bool[] array = new bool[2];
			Dictionary<int, Mem_useitem> user_useItem = Comm_UserDatas.Instance.User_useItem;
			int[] array2 = new int[2];
			Mem_useitem mem_useitem = null;
			Mem_useitem mem_useitem2 = null;
			Comm_UserDatas.Instance.User_useItem.TryGetValue(54, ref mem_useitem);
			if (mem_useitem != null)
			{
				array2[0] = user_useItem.get_Item(54).Value;
			}
			Comm_UserDatas.Instance.User_useItem.TryGetValue(59, ref mem_useitem2);
			if (mem_useitem2 != null)
			{
				array2[1] = user_useItem.get_Item(59).Value;
			}
			Mem_deck mem_deck;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, ref mem_deck))
			{
				return array;
			}
			List<Mem_ship> memShip = mem_deck.Ship.getMemShip();
			if (memShip.get_Count() == 0)
			{
				return array;
			}
			if (array2[1] >= 1)
			{
				array[1] = true;
			}
			if (array2[0] >= 1)
			{
				using (List<Mem_ship>.Enumerator enumerator = memShip.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						if (current.Cond < 40)
						{
							array[0] = true;
						}
					}
				}
			}
			return array;
		}

		public Api_Result<bool> itemuse_cond(int deck_id, HashSet<int> useitem_id)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			api_Result.data = false;
			int num = 0;
			if (useitem_id.Contains(54) && useitem_id.Contains(59))
			{
				num = 3;
			}
			else if (useitem_id.Contains(54))
			{
				num = 1;
			}
			else
			{
				if (!useitem_id.Contains(59))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				num = 2;
			}
			Dictionary<int, Mem_useitem> user_useItem = Comm_UserDatas.Instance.User_useItem;
			List<Mem_useitem> list = new List<Mem_useitem>();
			using (HashSet<int>.Enumerator enumerator = useitem_id.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					Mem_useitem mem_useitem = null;
					user_useItem.TryGetValue(current, ref mem_useitem);
					list.Add(mem_useitem);
				}
			}
			Mem_deck mem_deck;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> memShip = mem_deck.Ship.getMemShip();
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 100;
			int num7 = 0;
			int num8 = 100;
			List<Mem_ship> list2 = new List<Mem_ship>();
			if (num == 1)
			{
				num2 = 50;
				num4 = 40;
				num8 = 100;
				Random random = new Random();
				using (List<Mem_ship>.Enumerator enumerator2 = memShip.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mem_ship current2 = enumerator2.get_Current();
						int num9 = 0;
						if (current2.Cond < 50)
						{
							if (current2.Rid == memShip.get_Item(0).Rid)
							{
								num9 = num2;
							}
							else
							{
								int num10 = random.Next(num8);
								if (current2.Stype == 2)
								{
									if (num10 < 80)
									{
										num9 = num2;
									}
								}
								else if (current2.Stype == 3)
								{
									if (num10 < 50)
									{
										num9 = num2;
									}
								}
								else if (num10 < 30)
								{
									num9 = num2;
								}
								if (current2.Cond < 40)
								{
									num9 = num4;
								}
							}
						}
						if (num9 > 0)
						{
							Mem_shipBase mem_shipBase = new Mem_shipBase(current2);
							mem_shipBase.Cond = num9;
							current2.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id), false);
							list2.Add(current2);
						}
					}
				}
			}
			else
			{
				if (num != 2 && num != 3)
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				Dictionary<int, int> dictionary = new Dictionary<int, int>();
				Dictionary<string, int> dictionary2 = new Dictionary<string, int>();
				if (num == 2)
				{
					num2 = 65;
					num3 = 25;
					num4 = 65;
					num5 = 25;
					num6 = 100;
					num7 = 81;
					num8 = 100;
					dictionary.Add(2, 20);
					dictionary.Add(3, 10);
					dictionary.Add(13, 20);
					dictionary.Add(14, 20);
					dictionary2.Add("ながと", 30);
					dictionary2.Add("かが", 15);
					dictionary2.Add("すずや", 15);
				}
				if (num == 3)
				{
					num2 = 70;
					num3 = 30;
					num4 = 65;
					num5 = 20;
					num6 = 100;
					num7 = 0;
					num8 = 11;
					dictionary.Add(2, 10);
					dictionary.Add(3, 5);
					dictionary.Add(13, 10);
					dictionary.Add(14, 10);
					dictionary2.Add("ながと", 10);
					dictionary2.Add("かが", 5);
					dictionary2.Add("すずや", 5);
				}
				Random random2 = new Random();
				using (List<Mem_ship>.Enumerator enumerator3 = memShip.GetEnumerator())
				{
					while (enumerator3.MoveNext())
					{
						Mem_ship current3 = enumerator3.get_Current();
						int num11 = 0;
						if (current3.Cond < num6)
						{
							if (current3.Rid == memShip.get_Item(0).Rid)
							{
								num11 = ((current3.Cond >= 41) ? (current3.Cond + num3) : num2);
							}
							else
							{
								int num12 = random2.Next(num8);
								int num13 = 0;
								dictionary.TryGetValue(current3.Stype, ref num13);
								if (num13 != 0)
								{
									num12 += num13;
								}
								int num14 = 0;
								dictionary2.TryGetValue(Mst_DataManager.Instance.Mst_ship.get_Item(current3.Ship_id).Yomi, ref num14);
								if (num14 != 0)
								{
									num12 += num14;
								}
								if (num12 >= num7)
								{
									if (current3.Cond < 41)
									{
										num11 = num4;
									}
									else
									{
										num11 = ((num != 2) ? (current3.Cond + num5 + num12) : (current3.Cond + num5));
									}
								}
							}
							if (num11 > num6)
							{
								num11 = num6;
							}
							if (num11 > 0)
							{
								Mem_shipBase mem_shipBase2 = new Mem_shipBase(current3);
								mem_shipBase2.Cond = num11;
								current3.Set_ShipParam(mem_shipBase2, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase2.Ship_id), false);
								list2.Add(current3);
							}
						}
					}
				}
			}
			using (List<Mem_ship>.Enumerator enumerator4 = list2.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					Mem_ship current4 = enumerator4.get_Current();
					current4.SumLovToUseFoodSupplyShip(num);
				}
			}
			using (List<Mem_useitem>.Enumerator enumerator5 = list.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					Mem_useitem current5 = enumerator5.get_Current();
					current5.Sub_UseItem(1);
				}
			}
			api_Result.data = true;
			return api_Result;
		}

		public bool IsValidKdockOpen()
		{
			Mem_useitem mem_useitem;
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(49, ref mem_useitem))
			{
				return false;
			}
			if (mem_useitem.Value == 0)
			{
				return false;
			}
			int num = 4;
			int count = Comm_UserDatas.Instance.User_kdock.get_Count();
			return count < num;
		}

		public Api_Result<Mem_kdock> KdockOpen()
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			if (!this.IsValidKdockOpen())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_kdock data = Comm_UserDatas.Instance.Add_Kdock();
			Comm_UserDatas.Instance.User_useItem.get_Item(49).Sub_UseItem(1);
			api_Result.data = data;
			return api_Result;
		}

		public bool IsValidNdockOpen(int area_id)
		{
			Mem_useitem mem_useitem;
			if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(49, ref mem_useitem))
			{
				return false;
			}
			if (mem_useitem.Value <= 0)
			{
				return false;
			}
			Mst_maparea mst_maparea;
			if (!Mst_DataManager.Instance.Mst_maparea.TryGetValue(area_id, ref mst_maparea))
			{
				return false;
			}
			int num = Enumerable.Count<Mem_ndock>(Enumerable.Where<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock data) => data.Area_id == area_id));
			return mst_maparea.IsActiveArea() && num < mst_maparea.Ndocks_max;
		}

		public Api_Result<Mem_ndock> NdockOpen(int area_id)
		{
			Api_Result<Mem_ndock> api_Result = new Api_Result<Mem_ndock>();
			if (!this.IsValidNdockOpen(area_id))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ndock data = Comm_UserDatas.Instance.Add_Ndock(area_id);
			Comm_UserDatas.Instance.User_useItem.get_Item(49).Sub_UseItem(1);
			api_Result.data = data;
			return api_Result;
		}
	}
}
