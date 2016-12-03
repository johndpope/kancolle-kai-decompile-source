using Common.Enum;
using local.managers;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.utils
{
	public class TrophyUtil
	{
		public static int __tmp_start_album_ship_num__;

		public static int __tmp_start_album_slot_num__;

		public static bool __tmp_area_reopen__;

		public static List<int> Unlock_At_Marriage()
		{
			return TrophyUtil.Unlock_At_Marriage(true);
		}

		public static List<int> Unlock_At_Marriage(bool unlock)
		{
			List<int> list = TrophyUtil.__convert__(1);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_AreaClear(int area_id)
		{
			return TrophyUtil.Unlock_At_AreaClear(area_id, true);
		}

		public static List<int> Unlock_At_AreaClear(int area_id, bool unlock)
		{
			int num = area_id + 8;
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(num))
			{
				return new List<int>();
			}
			List<int> list = new List<int>();
			list.Add(num);
			List<int> list2 = list;
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_At_MapStart()
		{
			return TrophyUtil.Unlock_At_MapStart(true);
		}

		public static List<int> Unlock_At_MapStart(bool unlock)
		{
			int start_map_count = Comm_UserDatas.Instance.User_trophy.Start_map_count;
			List<int> list;
			if (start_map_count >= 10)
			{
				list = TrophyUtil.__convert__(26);
			}
			else
			{
				list = TrophyUtil.__convert__(null);
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_BattleResultOnlySally()
		{
			return TrophyUtil.Unlock_At_BattleResultOnlySally(true);
		}

		public static List<int> Unlock_At_BattleResultOnlySally(bool unlock)
		{
			int win_S_count = Comm_UserDatas.Instance.User_trophy.Win_S_count;
			List<int> list = new List<int>();
			if (win_S_count >= 30)
			{
				list.Add(27);
				if (win_S_count >= 100)
				{
					list.Add(28);
				}
			}
			list = TrophyUtil.__convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_ReOpenMap()
		{
			return TrophyUtil.Unlock_ReOpenMap(true);
		}

		public static List<int> Unlock_ReOpenMap(bool unlock)
		{
			List<int> list = new List<int>();
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(29) && TrophyUtil.__tmp_area_reopen__)
			{
				list.Add(29);
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_Battle(int count_at_battlestart, int count_in_battle)
		{
			return TrophyUtil.Unlock_At_Battle(count_at_battlestart, count_in_battle, true);
		}

		public static List<int> Unlock_At_Battle(int count_at_battlestart, int count_in_battle, bool unlock)
		{
			int count = count_at_battlestart + count_in_battle;
			return TrophyUtil.__Unlock_For_RecoveryItem__(count, unlock);
		}

		public static List<int> Unlock_At_SCutBattle()
		{
			return TrophyUtil.Unlock_At_SCutBattle(true);
		}

		public static List<int> Unlock_At_SCutBattle(bool unlock)
		{
			int use_recovery_item_count = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			return TrophyUtil.__Unlock_For_RecoveryItem__(use_recovery_item_count, unlock);
		}

		public static List<int> Unlock_At_GoNext()
		{
			return TrophyUtil.Unlock_At_GoNext(true);
		}

		public static List<int> Unlock_At_GoNext(bool unlock)
		{
			int count = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count + 1;
			return TrophyUtil.__Unlock_For_RecoveryItem__(count, unlock);
		}

		public static List<int> Unlock_At_Rading()
		{
			return TrophyUtil.Unlock_At_Rading(true);
		}

		public static List<int> Unlock_At_Rading(bool unlock)
		{
			int use_recovery_item_count = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			return TrophyUtil.__Unlock_For_RecoveryItem__(use_recovery_item_count, unlock);
		}

		private static List<int> __Unlock_For_RecoveryItem__(int count, bool unlock)
		{
			List<int> list2;
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(30) && count >= 5)
			{
				List<int> list = new List<int>();
				list.Add(30);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_At_Revamp()
		{
			return TrophyUtil.Unlock_At_Revamp(true);
		}

		public static List<int> Unlock_At_Revamp(bool unlock)
		{
			int revamp_count = Comm_UserDatas.Instance.User_trophy.Revamp_count;
			List<int> list2;
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(38) && revamp_count >= 100)
			{
				List<int> list = new List<int>();
				list.Add(38);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_At_BuyFurniture()
		{
			return TrophyUtil.Unlock_At_BuyFurniture(true);
		}

		public static List<int> Unlock_At_BuyFurniture(bool unlock)
		{
			List<int> list = new List<int>();
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(39))
			{
				return list;
			}
			Dictionary<int, Mst_furniture> mst = Mst_DataManager.Instance.Mst_furniture;
			int num = Enumerable.Count<Mem_furniture>(Comm_UserDatas.Instance.User_furniture.get_Values(), (Mem_furniture f) => mst.get_Item(f.Rid).Title != "なし");
			if (num < 50)
			{
				return list;
			}
			list.Add(39);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_DockOpen()
		{
			return TrophyUtil.Unlock_At_DockOpen(true);
		}

		public static List<int> Unlock_At_DockOpen(bool unlock)
		{
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(40))
			{
				return new List<int>();
			}
			int num = 0;
			using (Dictionary<int, Mst_maparea>.ValueCollection.Enumerator enumerator = Mst_DataManager.Instance.Mst_maparea.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_maparea current = enumerator.get_Current();
					num += current.Ndocks_max;
				}
			}
			List<int> list2;
			if (Comm_UserDatas.Instance.User_ndock.get_Count() >= num)
			{
				List<int> list = new List<int>();
				list.Add(40);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_UserLevel()
		{
			return TrophyUtil.Unlock_UserLevel(true);
		}

		public static List<int> Unlock_UserLevel(bool unlock)
		{
			List<int> list = new List<int>();
			if (Comm_UserDatas.Instance.User_basic.UserLevel() >= 70)
			{
				list.Add(41);
			}
			list = TrophyUtil.__convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_GameClear()
		{
			return TrophyUtil.Unlock_At_GameClear(true);
		}

		public static List<int> Unlock_At_GameClear(bool unlock)
		{
			List<int> list = new List<int>();
			if (!Server_Common.Utils.IsGameClear())
			{
				return list;
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(44))
			{
				list.Add(44);
			}
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			if (difficult == DifficultKind.OTU)
			{
				if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(45))
				{
					list.Add(45);
				}
			}
			else if (difficult == DifficultKind.KOU)
			{
				if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(46))
				{
					list.Add(46);
				}
			}
			else if (difficult == DifficultKind.SHI && !SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(47))
			{
				list.Add(47);
			}
			list = TrophyUtil.__convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_Material()
		{
			return TrophyUtil.Unlock_Material(true);
		}

		public static List<int> Unlock_Material(bool unlock)
		{
			List<int> list = new List<int>();
			bool flag = SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(42);
			bool flag2 = SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(43);
			if (flag && flag2)
			{
				return list;
			}
			Dictionary<enumMaterialCategory, Mem_material> user_material = Comm_UserDatas.Instance.User_material;
			int value = user_material.get_Item(enumMaterialCategory.Fuel).Value;
			int value2 = user_material.get_Item(enumMaterialCategory.Bull).Value;
			int value3 = user_material.get_Item(enumMaterialCategory.Steel).Value;
			int value4 = user_material.get_Item(enumMaterialCategory.Bauxite).Value;
			int num = Math.Min(Math.Min(Math.Min(value, value2), value3), value4);
			if (!flag && num >= 20000)
			{
				list.Add(42);
			}
			if (!flag2 && num >= 100000)
			{
				list.Add(43);
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_DeckNum()
		{
			return TrophyUtil.Unlock_DeckNum(true);
		}

		public static List<int> Unlock_DeckNum(bool unlock)
		{
			List<int> list = new List<int>();
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(31))
			{
				return list;
			}
			if (Comm_UserDatas.Instance.User_deck.get_Count() < 8)
			{
				return list;
			}
			list.Add(31);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_BuildShip(int built_ship_mst_id)
		{
			return TrophyUtil.Unlock_At_BuildShip(built_ship_mst_id, true);
		}

		public static List<int> Unlock_At_BuildShip(int built_ship_mst_id, bool unlock)
		{
			Mst_ship mst_ship;
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(built_ship_mst_id, ref mst_ship))
			{
				return new List<int>();
			}
			List<int> list2;
			if (mst_ship.Yomi == "やまと" && !SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(32))
			{
				List<int> list = new List<int>();
				list.Add(32);
				list2 = list;
			}
			else
			{
				list2 = new List<int>();
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list2);
			}
			return list2;
		}

		public static List<int> Unlock_AlbumSlotNum()
		{
			return TrophyUtil.Unlock_AlbumSlotNum(true);
		}

		public static List<int> Unlock_AlbumSlotNum(bool unlock)
		{
			List<int> list = new List<int>();
			int bookRegNum = Server_Common.Utils.GetBookRegNum(2);
			if (bookRegNum <= TrophyUtil.__tmp_start_album_slot_num__)
			{
				return list;
			}
			if (bookRegNum >= 30)
			{
				list.Add(6);
				if (bookRegNum >= 70)
				{
					list.Add(7);
					if (bookRegNum >= 120)
					{
						list.Add(8);
					}
				}
			}
			list = TrophyUtil.__convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_AlbumShipNum()
		{
			return TrophyUtil.Unlock_AlbumShipNum(true);
		}

		public static List<int> Unlock_AlbumShipNum(bool unlock)
		{
			List<int> list = new List<int>();
			int bookRegNum = Server_Common.Utils.GetBookRegNum(1);
			if (bookRegNum <= TrophyUtil.__tmp_start_album_ship_num__)
			{
				return list;
			}
			if (bookRegNum >= 30)
			{
				list.Add(2);
				if (bookRegNum >= 100)
				{
					list.Add(3);
					if (bookRegNum >= 150)
					{
						list.Add(4);
						if (bookRegNum >= 200)
						{
							list.Add(5);
						}
					}
				}
			}
			list = TrophyUtil.__convert__(list);
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_GatherShips(int new_ship_mst_id)
		{
			return TrophyUtil.Unlock_GatherShips(new_ship_mst_id, true);
		}

		public static List<int> Unlock_GatherShips(int new_ship_mst_id, bool unlock)
		{
			List<int> list = new List<int>();
			HashSet<int> hashSet = new HashSet<int>();
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(33))
			{
				hashSet.Add(33);
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(35))
			{
				hashSet.Add(35);
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(36))
			{
				hashSet.Add(36);
			}
			if (!SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(37))
			{
				hashSet.Add(37);
			}
			if (hashSet.get_Count() > 0)
			{
				Mst_ship mst_ship;
				if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(new_ship_mst_id, ref mst_ship))
				{
					return new List<int>();
				}
				string yomi = mst_ship.Yomi;
				Mst_ship mst_ship2;
				if (hashSet.Contains(33))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("こんごう");
					hashSet2.Add("ひえい");
					hashSet2.Add("はるな");
					hashSet2.Add("きりしま");
					hashSet2.Add("ながと");
					hashSet2.Add("むつ");
					hashSet2.Add("ふそう");
					hashSet2.Add("やましろ");
					hashSet2.Add("いせ");
					hashSet2.Add("ひゅうが");
					hashSet2.Add("やまと");
					hashSet2.Add("むさし");
					HashSet<string> hashSet3 = hashSet2;
					if (hashSet3.Contains(yomi))
					{
						using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_ship.get_Values().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								Mem_ship current = enumerator.get_Current();
								if (Mst_DataManager.Instance.Mst_ship.TryGetValue(current.Ship_id, ref mst_ship2) && hashSet3.Contains(mst_ship2.Yomi))
								{
									hashSet3.Remove(mst_ship2.Yomi);
								}
							}
						}
						if (hashSet3.get_Count() == 0)
						{
							list.Add(33);
						}
					}
				}
				if (hashSet.Contains(35))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("あかぎ");
					hashSet2.Add("かが");
					hashSet2.Add("ひりゅう");
					hashSet2.Add("そうりゅう");
					hashSet2.Add("しょうかく");
					hashSet2.Add("ずいかく");
					HashSet<string> hashSet4 = hashSet2;
					if (hashSet4.Contains(yomi))
					{
						using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator2 = Comm_UserDatas.Instance.User_ship.get_Values().GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								Mem_ship current2 = enumerator2.get_Current();
								if (Mst_DataManager.Instance.Mst_ship.TryGetValue(current2.Ship_id, ref mst_ship2) && hashSet4.Contains(mst_ship2.Yomi))
								{
									hashSet4.Remove(mst_ship2.Yomi);
								}
							}
						}
						if (hashSet4.get_Count() == 0)
						{
							list.Add(35);
						}
					}
				}
				if (hashSet.Contains(36))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("こんごう");
					hashSet2.Add("きりしま");
					hashSet2.Add("はるな");
					hashSet2.Add("ひえい");
					HashSet<string> hashSet5 = hashSet2;
					if (hashSet5.Contains(yomi))
					{
						using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator3 = Comm_UserDatas.Instance.User_ship.get_Values().GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								Mem_ship current3 = enumerator3.get_Current();
								if (Mst_DataManager.Instance.Mst_ship.TryGetValue(current3.Ship_id, ref mst_ship2) && hashSet5.Contains(mst_ship2.Yomi))
								{
									hashSet5.Remove(mst_ship2.Yomi);
								}
							}
						}
						if (hashSet5.get_Count() == 0)
						{
							list.Add(36);
						}
					}
				}
				if (hashSet.Contains(37))
				{
					HashSet<string> hashSet2 = new HashSet<string>();
					hashSet2.Add("ふそう");
					hashSet2.Add("やましろ");
					hashSet2.Add("もがみ");
					hashSet2.Add("しぐれ");
					hashSet2.Add("みちしお");
					hashSet2.Add("あさぐも");
					hashSet2.Add("やまぐも");
					HashSet<string> hashSet6 = hashSet2;
					if (hashSet6.Contains(yomi))
					{
						using (Dictionary<int, Mem_ship>.ValueCollection.Enumerator enumerator4 = Comm_UserDatas.Instance.User_ship.get_Values().GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								Mem_ship current4 = enumerator4.get_Current();
								if (Mst_DataManager.Instance.Mst_ship.TryGetValue(current4.Ship_id, ref mst_ship2) && hashSet6.Contains(mst_ship2.Yomi))
								{
									hashSet6.Remove(mst_ship2.Yomi);
								}
							}
						}
						if (hashSet6.get_Count() == 0)
						{
							list.Add(37);
						}
					}
				}
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_At_GetIowa(int new_ship_mst_id)
		{
			return TrophyUtil.Unlock_At_GetIowa(new_ship_mst_id, true);
		}

		public static List<int> Unlock_At_GetIowa(int new_ship_mst_id, bool unlock)
		{
			List<int> list = new List<int>();
			if (SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(34))
			{
				return list;
			}
			Mst_ship mst_ship;
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(new_ship_mst_id, ref mst_ship))
			{
				return list;
			}
			if (mst_ship.Yomi == "アイオワ")
			{
				List<int> list2 = new List<int>();
				list2.Add(34);
				list = list2;
			}
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		public static List<int> Unlock_GetShip(int new_ship_mst_id)
		{
			return TrophyUtil.Unlock_GetShip(new_ship_mst_id, true);
		}

		public static List<int> Unlock_GetShip(int new_ship_mst_id, bool unlock)
		{
			List<int> list = new List<int>();
			list.AddRange(TrophyUtil.Unlock_AlbumShipNum(false));
			list.AddRange(TrophyUtil.Unlock_AlbumSlotNum(false));
			list.AddRange(TrophyUtil.Unlock_GatherShips(new_ship_mst_id, false));
			list.AddRange(TrophyUtil.Unlock_At_GetIowa(new_ship_mst_id, false));
			if (unlock)
			{
				SingletonMonoBehaviour<TrophyManager>.Instance.UnlockTrophies(list);
			}
			return list;
		}

		private static List<int> __convert__(int tmp)
		{
			List<int> list = new List<int>();
			list.Add(tmp);
			return TrophyUtil.__convert__(list);
		}

		private static List<int> __convert__(List<int> tmp)
		{
			if (tmp == null)
			{
				return new List<int>();
			}
			return tmp.FindAll((int id) => !SingletonMonoBehaviour<TrophyManager>.Instance.IsUnlocked(id));
		}
	}
}
