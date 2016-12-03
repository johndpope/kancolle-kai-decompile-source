using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_get_Member
	{
		private Dictionary<int, int> mst_level;

		private Dictionary<int, List<int>> type3Dict;

		private Dictionary<int, string> tempShipBookText;

		private Dictionary<int, string> tempSlotBookText;

		private Dictionary<int, string> tempShipClass;

		private Dictionary<int, int> tempMstShipBook;

		public Api_Result<Mem_basic> Basic()
		{
			return new Api_Result<Mem_basic>
			{
				data = Comm_UserDatas.Instance.User_basic
			};
		}

		public Api_Result<Dictionary<int, Mem_ship>> Ship(List<int> target_rid)
		{
			Api_Result<Dictionary<int, Mem_ship>> api_Result = new Api_Result<Dictionary<int, Mem_ship>>();
			Dictionary<int, Mem_ship> ret_ship = new Dictionary<int, Mem_ship>();
			if (target_rid != null && target_rid.get_Count() == 0)
			{
				api_Result.data = ret_ship;
				return api_Result;
			}
			if (this.mst_level == null)
			{
				this.mst_level = Mst_DataManager.Instance.Get_MstLevel(true);
			}
			if (target_rid == null)
			{
				using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_ship.get_Values().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_ship current = enumerator.get_Current();
						current.SetRequireExp(current.Level, this.mst_level);
						ret_ship.Add(current.Rid, current);
					}
				}
				api_Result.data = ret_ship;
				return api_Result;
			}
			target_rid.ForEach(delegate(int x)
			{
				Mem_ship mem_ship = null;
				if (Comm_UserDatas.Instance.User_ship.TryGetValue(x, ref mem_ship))
				{
					mem_ship.SetRequireExp(mem_ship.Level, this.mst_level);
					ret_ship.Add(x, mem_ship);
				}
			});
			api_Result.data = ret_ship;
			if (ret_ship.get_Count() == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				api_Result.data = null;
			}
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_ship>> Ship(int area_id)
		{
			List<DeckShips> list = new List<DeckShips>();
			Mem_esccort_deck mem_esccort_deck = Enumerable.FirstOrDefault<Mem_esccort_deck>(Comm_UserDatas.Instance.User_EscortDeck.get_Values(), (Mem_esccort_deck x) => x.Maparea_id == area_id);
			if (mem_esccort_deck != null)
			{
				list.Add(mem_esccort_deck.Ship);
			}
			IEnumerable<DeckShips> enumerable = Enumerable.Select<Mem_deck, DeckShips>(Enumerable.Where<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck deck) => deck.Area_id == area_id), (Mem_deck deck) => deck.Ship);
			if (Enumerable.Count<DeckShips>(enumerable) > 0)
			{
				list.AddRange(enumerable);
			}
			if (list.get_Count() == 0)
			{
				return new Api_Result<Dictionary<int, Mem_ship>>
				{
					state = Api_Result_State.Parameter_Error
				};
			}
			List<int> list2 = new List<int>();
			using (List<DeckShips>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DeckShips current = enumerator.get_Current();
					List<int> list3 = null;
					current.Clone(out list3);
					list2.AddRange(list3);
				}
			}
			return this.Ship(list2);
		}

		public Api_Result<Dictionary<int, Mem_deck>> Deck()
		{
			Api_Result<Dictionary<int, Mem_deck>> api_Result = new Api_Result<Dictionary<int, Mem_deck>>();
			Dictionary<int, Mem_deck> data = Enumerable.ToDictionary<KeyValuePair<int, Mem_deck>, int, Mem_deck>(Comm_UserDatas.Instance.User_deck, (KeyValuePair<int, Mem_deck> x) => x.get_Key(), (KeyValuePair<int, Mem_deck> y) => y.get_Value());
			api_Result.data = data;
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_deck>> Deck_Port()
		{
			Api_Result<Dictionary<int, Mem_deck>> api_Result = this.Deck();
			if (api_Result.data == null)
			{
				return api_Result;
			}
			using (Dictionary<int, Mem_deck>.ValueCollection.Enumerator enumerator = api_Result.data.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_deck current = enumerator.get_Current();
					current.MissionEnd();
				}
			}
			return api_Result;
		}

		public Api_Result<Dictionary<enumMaterialCategory, Mem_material>> Material()
		{
			Api_Result<Dictionary<enumMaterialCategory, Mem_material>> api_Result = new Api_Result<Dictionary<enumMaterialCategory, Mem_material>>();
			Dictionary<enumMaterialCategory, Mem_material> data = Enumerable.ToDictionary<KeyValuePair<enumMaterialCategory, Mem_material>, enumMaterialCategory, Mem_material>(Comm_UserDatas.Instance.User_material, (KeyValuePair<enumMaterialCategory, Mem_material> x) => x.get_Key(), (KeyValuePair<enumMaterialCategory, Mem_material> y) => y.get_Value());
			api_Result.data = data;
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_slotitem>> Slotitem()
		{
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_Result<Dictionary<int, Mem_slotitem>>();
			Dictionary<int, Mem_slotitem> data = Enumerable.ToDictionary<KeyValuePair<int, Mem_slotitem>, int, Mem_slotitem>(Comm_UserDatas.Instance.User_slot, (KeyValuePair<int, Mem_slotitem> x) => x.get_Key(), (KeyValuePair<int, Mem_slotitem> y) => y.get_Value());
			api_Result.data = data;
			return api_Result;
		}

		public Api_Result<Dictionary<int, User_StrategyMapFmt>> StrategyInfo()
		{
			Api_Result<Dictionary<int, User_StrategyMapFmt>> api_Result = new Api_Result<Dictionary<int, User_StrategyMapFmt>>();
			api_Result.data = new Dictionary<int, User_StrategyMapFmt>();
			Dictionary<int, Mst_mapinfo> dictionary = Enumerable.ToDictionary<Mst_mapinfo, int, Mst_mapinfo>(Enumerable.Where<Mst_mapinfo>(Mst_DataManager.Instance.Mst_mapinfo.get_Values(), (Mst_mapinfo x) => x.No == 1), (Mst_mapinfo y) => y.Maparea_id, (Mst_mapinfo z) => z);
			using (Dictionary<int, Mst_maparea>.ValueCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_maparea.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_maparea current = enumerator.get_Current();
					bool user_MapinfoData = dictionary.get_Item(current.Id).GetUser_MapinfoData() != null;
					User_StrategyMapFmt user_StrategyMapFmt = new User_StrategyMapFmt(current, user_MapinfoData);
					Mem_rebellion_point mem_rebellion_point = null;
					if (Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(current.Id, ref mem_rebellion_point))
					{
						user_StrategyMapFmt.RebellionState = mem_rebellion_point.State;
					}
					if (user_StrategyMapFmt.IsActiveArea)
					{
						if (!Comm_UserDatas.Instance.User_EscortDeck.ContainsKey(current.Id))
						{
							Comm_UserDatas.Instance.Add_EscortDeck(current.Id, current.Id);
						}
						if (mem_rebellion_point == null)
						{
							mem_rebellion_point = new Mem_rebellion_point(current.Id);
							Comm_UserDatas.Instance.User_rebellion_point.Add(mem_rebellion_point.Rid, mem_rebellion_point);
							user_StrategyMapFmt.RebellionState = mem_rebellion_point.State;
						}
					}
					api_Result.data.Add(user_StrategyMapFmt.Maparea.Id, user_StrategyMapFmt);
				}
			}
			return api_Result;
		}

		public Api_Result<Dictionary<int, List<Mem_tanker>>> Tanker()
		{
			Api_Result<Dictionary<int, List<Mem_tanker>>> api_Result = new Api_Result<Dictionary<int, List<Mem_tanker>>>();
			Dictionary<int, List<Mem_tanker>> dictionary = Enumerable.ToDictionary<int, int, List<Mem_tanker>>(Mst_DataManager.Instance.Mst_maparea.get_Keys(), (int n) => n, (int value) => new List<Mem_tanker>());
			dictionary.Add(0, new List<Mem_tanker>());
			IEnumerable<IGrouping<int, Mem_tanker>> enumerable = Enumerable.GroupBy(Enumerable.Select(Comm_UserDatas.Instance.User_tanker.get_Values(), (Mem_tanker tankers) => new
			{
				tankers = tankers,
				maparea_id = ((tankers.Disposition_status != DispositionStatus.NONE) ? tankers.Maparea_id : 0)
			}), <>__TranspIdent2 => <>__TranspIdent2.maparea_id, <>__TranspIdent2 => <>__TranspIdent2.tankers);
			using (IEnumerator<IGrouping<int, Mem_tanker>> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, Mem_tanker> current = enumerator.get_Current();
					int key = current.get_Key();
					dictionary.get_Item(key).AddRange(Enumerable.ToList<Mem_tanker>(current));
				}
			}
			api_Result.data = dictionary;
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_esccort_deck>> EscortDeck()
		{
			Api_Result<Dictionary<int, Mem_esccort_deck>> api_Result = new Api_Result<Dictionary<int, Mem_esccort_deck>>();
			Dictionary<int, Mem_esccort_deck> dictionary = Enumerable.ToDictionary<Mem_esccort_deck, int, Mem_esccort_deck>(Comm_UserDatas.Instance.User_EscortDeck.get_Values(), (Mem_esccort_deck value) => value.Maparea_id, (Mem_esccort_deck value) => value);
			Dictionary<int, Mem_esccort_deck> dictionary2 = new Dictionary<int, Mem_esccort_deck>();
			using (Dictionary<int, Mst_maparea>.KeyCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_maparea.get_Keys().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					dictionary2.Add(current, null);
					Mem_esccort_deck mem_esccort_deck = null;
					if (dictionary.TryGetValue(current, ref mem_esccort_deck))
					{
						dictionary2.set_Item(current, mem_esccort_deck);
					}
				}
			}
			api_Result.data = dictionary2;
			return api_Result;
		}

		public Api_Result<Dictionary<int, User_MapinfoFmt>> Mapinfo()
		{
			Api_Result<Dictionary<int, User_MapinfoFmt>> api_Result = new Api_Result<Dictionary<int, User_MapinfoFmt>>();
			api_Result.data = new Dictionary<int, User_MapinfoFmt>();
			using (Dictionary<int, Mst_mapinfo>.Enumerator enumerator = Mst_DataManager.Instance.Mst_mapinfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, Mst_mapinfo> current = enumerator.get_Current();
					User_MapinfoFmt user_MapinfoFmt = current.get_Value().GetUser_MapinfoData();
					if (user_MapinfoFmt != null)
					{
						api_Result.data.Add(user_MapinfoFmt.Id, user_MapinfoFmt);
					}
					else if (current.get_Value().IsOpenMapSys())
					{
						user_MapinfoFmt = new User_MapinfoFmt();
						user_MapinfoFmt.Id = current.get_Value().Id;
						api_Result.data.Add(user_MapinfoFmt.Id, user_MapinfoFmt);
					}
				}
			}
			return api_Result;
		}

		public Api_Result<User_RecordFmt> Record()
		{
			Api_Result<User_RecordFmt> api_Result = new Api_Result<User_RecordFmt>();
			Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
			Mem_record user_record = Comm_UserDatas.Instance.User_record;
			User_RecordFmt user_RecordFmt = new User_RecordFmt();
			user_basic.SetUserRecordData(user_RecordFmt);
			user_record.SetUserRecordData(user_RecordFmt);
			user_RecordFmt.Deck_num = Enumerable.Count<KeyValuePair<int, Mem_deck>>(Comm_UserDatas.Instance.User_deck);
			user_RecordFmt.Ship_num = Enumerable.Count<KeyValuePair<int, Mem_ship>>(Comm_UserDatas.Instance.User_ship);
			user_RecordFmt.Slot_num = Enumerable.Count<KeyValuePair<int, Mem_slotitem>>(Comm_UserDatas.Instance.User_slot);
			user_RecordFmt.Ndock_num = Enumerable.Count<KeyValuePair<int, Mem_ndock>>(Comm_UserDatas.Instance.User_ndock);
			user_RecordFmt.Kdock_num = Enumerable.Count<KeyValuePair<int, Mem_kdock>>(Comm_UserDatas.Instance.User_kdock);
			int num = Enumerable.Count<Mem_furniture>(Comm_UserDatas.Instance.User_furniture.get_Values(), (Mem_furniture x) => Mst_DataManager.Instance.Mst_furniture.get_Item(x.Rid).Price == 0 && Mst_DataManager.Instance.Mst_furniture.get_Item(x.Rid).Rarity == 0 && Mst_DataManager.Instance.Mst_furniture.get_Item(x.Rid).Title.Equals("なし"));
			user_RecordFmt.Furniture_num = Comm_UserDatas.Instance.User_furniture.get_Count() - num;
			user_RecordFmt.Material_max = user_basic.GetMaterialMaxNum();
			api_Result.data = user_RecordFmt;
			return api_Result;
		}

		public Api_Result<Dictionary<int, Mem_useitem>> UseItem()
		{
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_Result<Dictionary<int, Mem_useitem>>();
			if (Comm_UserDatas.Instance.User_useItem == null || Comm_UserDatas.Instance.User_useItem.get_Count() == 0)
			{
				return api_Result;
			}
			api_Result.data = Enumerable.ToDictionary<KeyValuePair<int, Mem_useitem>, int, Mem_useitem>(Comm_UserDatas.Instance.User_useItem, (KeyValuePair<int, Mem_useitem> x) => x.get_Key(), (KeyValuePair<int, Mem_useitem> y) => y.get_Value());
			return api_Result;
		}

		public void InitBookData()
		{
			IEnumerable<XElement> enumerable = Utils.Xml_Result("mst_shiptext", "mst_shiptext", null);
			IEnumerable<XElement> enumerable2 = Utils.Xml_Result("mst_slotitemtext", "mst_slotitemtext", null);
			IEnumerable<XElement> enumerable3 = Utils.Xml_Result("mst_ship_class", "mst_ship_class", null);
			this.tempShipBookText = Enumerable.ToDictionary<XElement, int, string>(enumerable, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => value.Element("Sinfo").get_Value());
			this.tempSlotBookText = Enumerable.ToDictionary<XElement, int, string>(enumerable2, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => value.Element("Info").get_Value());
			this.tempShipClass = Enumerable.ToDictionary<XElement, int, string>(enumerable3, (XElement key) => int.Parse(key.Element("Id").get_Value()), (XElement value) => value.Element("Name").get_Value());
			this.tempMstShipBook = ArrayMaster.GetShipBookInfo();
		}

		public Api_Result<Dictionary<int, User_BookFmt<BookShipData>>> PictureShip()
		{
			Api_Result<Dictionary<int, User_BookFmt<BookShipData>>> api_Result = new Api_Result<Dictionary<int, User_BookFmt<BookShipData>>>();
			var dictionary = Enumerable.ToDictionary(Enumerable.Select(Comm_UserDatas.Instance.Ship_book.get_Values(), delegate(Mem_book item)
			{
				int arg_49_0 = item.Table_id;
				List<int> list4 = new List<int>();
				list4.Add(item.Flag1);
				list4.Add(item.Flag2);
				list4.Add(item.Flag3);
				list4.Add(item.Flag4);
				list4.Add(item.Flag5);
				return new
				{
					id = arg_49_0,
					state = list4
				};
			}), x => x.id, y => y);
			using (Dictionary<int, int>.Enumerator enumerator = this.tempMstShipBook.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					if (dictionary.ContainsKey(current.get_Key()))
					{
						bool flag = dictionary.ContainsKey(current.get_Value());
						if (dictionary.get_Item(current.get_Key()).state.get_Item(2) != 0 && flag)
						{
							dictionary.get_Item(current.get_Value()).state.set_Item(2, dictionary.get_Item(current.get_Key()).state.get_Item(2));
						}
					}
				}
			}
			int bookMax = Mst_DataManager.Instance.Mst_const.get_Item(MstConstDataIndex.Book_max_ships).Int_value;
			List<Mst_ship> list = Enumerable.ToList<Mst_ship>(Enumerable.Where<Mst_ship>(Enumerable.OrderBy<Mst_ship, int>(Mst_DataManager.Instance.Mst_ship.get_Values(), (Mst_ship obj) => obj.Bookno), (Mst_ship x) => x.Bookno > 0 && x.Bookno <= bookMax));
			Dictionary<int, User_BookFmt<BookShipData>> dictionary2 = new Dictionary<int, User_BookFmt<BookShipData>>();
			HashSet<Mst_ship> hashSet = new HashSet<Mst_ship>();
			using (List<Mst_ship>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mst_ship current2 = enumerator2.get_Current();
					if (!hashSet.Contains(current2))
					{
						List<int> list2 = new List<int>();
						List<List<int>> list3 = new List<List<int>>();
						Mst_ship mst_ship = current2;
						bool flag2 = false;
						if (dictionary.ContainsKey(mst_ship.Id))
						{
							list2.Add(current2.Id);
							list3.Add(dictionary.get_Item(current2.Id).state);
							flag2 = true;
						}
						hashSet.Add(mst_ship);
						for (int append_ship_id = current2.Append_ship_id; append_ship_id != 0; append_ship_id = Mst_DataManager.Instance.Mst_ship.get_Item(append_ship_id).Append_ship_id)
						{
							hashSet.Add(Mst_DataManager.Instance.Mst_ship.get_Item(append_ship_id));
							if (dictionary.ContainsKey(append_ship_id))
							{
								list2.Add(append_ship_id);
								list3.Add(dictionary.get_Item(append_ship_id).state);
							}
						}
						if (flag2)
						{
							string info = this.tempShipBookText.get_Item(list2.get_Item(0));
							int ctype = Mst_DataManager.Instance.Mst_ship.get_Item(list2.get_Item(0)).Ctype;
							string cname = this.tempShipClass.get_Item(ctype);
							User_BookFmt<BookShipData> user_BookFmt = new User_BookFmt<BookShipData>(list2.get_Item(0), list2, list3, info, cname, null);
							if (Enumerable.Any<List<int>>(user_BookFmt.State, (List<int> x) => x.get_Item(2) != 0))
							{
								user_BookFmt.State.ForEach(delegate(List<int> flags)
								{
									flags.set_Item(2, 1);
								});
							}
							dictionary2.Add(user_BookFmt.IndexNo, user_BookFmt);
						}
						else
						{
							dictionary2.Add(current2.Bookno, null);
						}
					}
				}
			}
			api_Result.data = dictionary2;
			this.tempShipBookText.Clear();
			this.tempShipBookText = null;
			this.tempShipClass.Clear();
			this.tempShipClass = null;
			return api_Result;
		}

		public Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>> PictureSlot()
		{
			Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>> api_Result = new Api_Result<Dictionary<int, User_BookFmt<BookSlotData>>>();
			Dictionary<int, <>__AnonType6<int, List<int>>> bookData = Enumerable.ToDictionary(Enumerable.Select(Comm_UserDatas.Instance.Slot_book.get_Values(), delegate(Mem_book item)
			{
				int arg_49_0 = item.Table_id;
				List<int> list3 = new List<int>();
				list3.Add(item.Flag1);
				list3.Add(item.Flag2);
				list3.Add(item.Flag3);
				list3.Add(item.Flag4);
				list3.Add(item.Flag5);
				return new
				{
					id = arg_49_0,
					state = list3
				};
			}), x => x.id, y => y);
			if (this.type3Dict == null)
			{
				IEnumerable<int> enumerable = Enumerable.Distinct<int>(Enumerable.Select<KeyValuePair<int, Mst_slotitem>, int>(Mst_DataManager.Instance.Mst_Slotitem, (KeyValuePair<int, Mst_slotitem> x) => x.get_Value().Type3));
				this.type3Dict = new Dictionary<int, List<int>>();
				using (IEnumerator<int> enumerator = enumerable.GetEnumerator())
				{
					int type3;
					while (enumerator.MoveNext())
					{
						type3 = enumerator.get_Current();
						List<int> list = Enumerable.ToList<int>(Enumerable.Select<Mst_stype, int>(Enumerable.Where<Mst_stype>(Mst_DataManager.Instance.Mst_stype.get_Values(), (Mst_stype stypes) => stypes.Equip.Contains(type3)), (Mst_stype stypes) => stypes.Id));
						this.type3Dict.Add(type3, list);
					}
				}
			}
			Dictionary<int, User_BookFmt<BookSlotData>> fmt = new Dictionary<int, User_BookFmt<BookSlotData>>();
			List<Mst_slotitem> list2 = Enumerable.ToList<Mst_slotitem>(Enumerable.Where<Mst_slotitem>(Mst_DataManager.Instance.Mst_Slotitem.get_Values(), (Mst_slotitem x) => x.Sortno > 0));
			list2.ForEach(delegate(Mst_slotitem dispItem)
			{
				if (bookData.ContainsKey(dispItem.Id))
				{
					var <>__AnonType = bookData.get_Item(dispItem.Id);
					string info = null;
					if (!this.tempSlotBookText.TryGetValue(<>__AnonType.id, ref info))
					{
						info = string.Empty;
					}
					List<int> list3 = new List<int>();
					list3.Add(<>__AnonType.id);
					List<int> ids = list3;
					List<List<int>> list4 = new List<List<int>>();
					list4.Add(<>__AnonType.state);
					List<List<int>> state = list4;
					User_BookFmt<BookSlotData> user_BookFmt = new User_BookFmt<BookSlotData>(<>__AnonType.id, ids, state, info, null, this.type3Dict);
					fmt.Add(user_BookFmt.IndexNo, user_BookFmt);
				}
				else
				{
					fmt.Add(dispItem.Sortno, null);
				}
			});
			api_Result.data = fmt;
			this.tempSlotBookText.Clear();
			this.tempSlotBookText = null;
			return api_Result;
		}

		public Api_Result<List<Mem_ndock>> ndock()
		{
			Api_Result<List<Mem_ndock>> api_Result = new Api_Result<List<Mem_ndock>>();
			List<Mem_ndock> list = Enumerable.ToList<Mem_ndock>(Enumerable.OrderBy<Mem_ndock, int>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Rid));
			using (List<Mem_ndock>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ndock current = enumerator.get_Current();
					current.RecoverEnd(true);
				}
			}
			api_Result.data = list;
			return api_Result;
		}

		public Api_Result<Dictionary<int, List<Mem_ndock>>> AreaNdock()
		{
			Api_Result<Dictionary<int, List<Mem_ndock>>> api_Result = new Api_Result<Dictionary<int, List<Mem_ndock>>>();
			Dictionary<int, User_StrategyMapFmt> data2 = this.StrategyInfo().data;
			IEnumerable<User_StrategyMapFmt> enumerable = Enumerable.Where<User_StrategyMapFmt>(data2.get_Values(), (User_StrategyMapFmt x) => x.IsActiveArea);
			List<int> list = Enumerable.ToList<int>(Enumerable.Select<Mem_ndock, int>(Enumerable.Where<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Dock_no == 1), (Mem_ndock y) => y.Area_id));
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			using (IEnumerator<User_StrategyMapFmt> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					User_StrategyMapFmt current = enumerator.get_Current();
					dictionary.Add(current.Maparea.Id, current.Maparea.Ndocks_max);
					if (!list.Contains(current.Maparea.Id))
					{
						for (int i = 0; i < current.Maparea.Ndocks_init; i++)
						{
							Comm_UserDatas.Instance.Add_Ndock(current.Maparea.Id);
						}
					}
				}
			}
			IEnumerable<IGrouping<int, Mem_ndock>> enumerable2 = Enumerable.GroupBy<Mem_ndock, int, Mem_ndock>(Enumerable.OrderBy<Mem_ndock, int>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock data) => data.Dock_no), (Mem_ndock data) => data.Area_id, (Mem_ndock data) => data);
			Dictionary<int, List<Mem_ndock>> dictionary2 = new Dictionary<int, List<Mem_ndock>>();
			using (IEnumerator<IGrouping<int, Mem_ndock>> enumerator2 = enumerable2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IGrouping<int, Mem_ndock> current2 = enumerator2.get_Current();
					List<Mem_ndock> list2 = Enumerable.ToList<Mem_ndock>(current2);
					if (dictionary.ContainsKey(list2.get_Item(0).Area_id))
					{
						list2.set_Capacity(dictionary.get_Item(list2.get_Item(0).Area_id));
						dictionary2.Add(current2.get_Key(), list2);
						list2.ForEach(delegate(Mem_ndock x)
						{
							x.RecoverEnd(true);
						});
					}
				}
			}
			api_Result.data = dictionary2;
			return api_Result;
		}

		public Api_Result<List<Mem_kdock>> kdock()
		{
			Api_Result<List<Mem_kdock>> api_Result = new Api_Result<List<Mem_kdock>>();
			List<Mem_kdock> list = Enumerable.ToList<Mem_kdock>(Enumerable.OrderBy<Mem_kdock, int>(Comm_UserDatas.Instance.User_kdock.get_Values(), (Mem_kdock x) => x.Rid));
			using (List<Mem_kdock>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_kdock current = enumerator.get_Current();
					current.CreateEnd(true);
				}
			}
			api_Result.data = list;
			return api_Result;
		}

		public Api_Result<List<User_MissionFmt>> Mission()
		{
			Api_Result<List<User_MissionFmt>> api_Result = new Api_Result<List<User_MissionFmt>>();
			Mem_missioncomp mem_missioncomp = new Mem_missioncomp();
			api_Result.data = mem_missioncomp.GetActiveMission();
			return api_Result;
		}

		public Api_Result<List<Mst_furniture>> DecorateFurniture(int deck_rid)
		{
			Api_Result<List<Mst_furniture>> api_Result = new Api_Result<List<Mst_furniture>>();
			Mem_room mem_room = null;
			if (!Comm_UserDatas.Instance.User_room.TryGetValue(deck_rid, ref mem_room))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = mem_room.getFurnitureDatas();
			return api_Result;
		}

		public Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> FurnitureList()
		{
			Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> api_Result = new Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>>();
			IEnumerable<IGrouping<int, Mst_furniture>> enumerable = Enumerable.GroupBy(Enumerable.OrderBy(Enumerable.Select(Comm_UserDatas.Instance.User_furniture.get_Values(), (Mem_furniture data) => new
			{
				data = data,
				master = Mst_DataManager.Instance.Mst_furniture.get_Item(data.Rid)
			}), <>__TranspIdent3 => <>__TranspIdent3.data.Rid), <>__TranspIdent3 => <>__TranspIdent3.master.Type, <>__TranspIdent3 => <>__TranspIdent3.master);
			Dictionary<FurnitureKinds, List<Mst_furniture>> dictionary = new Dictionary<FurnitureKinds, List<Mst_furniture>>();
			using (IEnumerator<IGrouping<int, Mst_furniture>> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, Mst_furniture> current = enumerator.get_Current();
					dictionary.Add((FurnitureKinds)current.get_Key(), Enumerable.ToList<Mst_furniture>(current));
				}
			}
			api_Result.data = dictionary;
			return api_Result;
		}

		public Api_Result<List<User_HistoryFmt>> HistoryList()
		{
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_Result<List<User_HistoryFmt>>();
			List<User_HistoryFmt> list = new List<User_HistoryFmt>();
			api_Result.data = list;
			List<Mem_history> list2 = new List<Mem_history>();
			using (Dictionary<int, List<Mem_history>>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_history.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_history> current = enumerator.get_Current();
					list2.AddRange(current);
				}
			}
			if (list2.get_Count() == 0)
			{
				return api_Result;
			}
			IOrderedEnumerable<Mem_history> orderedEnumerable = Enumerable.OrderBy<Mem_history, int>(list2, (Mem_history x) => x.Rid);
			using (IEnumerator<Mem_history> enumerator2 = orderedEnumerable.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mem_history current2 = enumerator2.get_Current();
					if (current2.Type != 999)
					{
						User_HistoryFmt user_HistoryFmt = new User_HistoryFmt(current2);
						list.Add(user_HistoryFmt);
					}
				}
			}
			if (Comm_UserDatas.Instance.User_kdock.get_Count() == 0)
			{
				Mem_history mem_history = new Mem_history();
				mem_history.SetGameOverToLost(Comm_UserDatas.Instance.User_turn.Total_turn);
				User_HistoryFmt user_HistoryFmt2 = new User_HistoryFmt(mem_history);
				list.Add(user_HistoryFmt2);
			}
			api_Result.data = list;
			return api_Result;
		}

		public Mem_option Option()
		{
			return new Mem_option();
		}

		public Api_Result<Mem_deckpractice> DeckPractice()
		{
			return new Api_Result<Mem_deckpractice>
			{
				data = Comm_UserDatas.Instance.User_deckpractice
			};
		}
	}
}
