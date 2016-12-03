using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Ending
	{
		private delegate bool getRankDlgt(DifficultKind difficult, DateTime nowDt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive);

		private Mem_newgame_plus mem_newgame;

		public Api_req_Ending()
		{
			this.mem_newgame = Comm_UserDatas.Instance.User_plus;
		}

		public bool GetOverallRank(out OverallRank clearRank, out int decorationNum)
		{
			clearRank = OverallRank.F;
			decorationNum = 0;
			List<Api_req_Ending.getRankDlgt> list = new List<Api_req_Ending.getRankDlgt>();
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankEx));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankS));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankA));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankB));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankC));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankD));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankD));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankE));
			list.Add(new Api_req_Ending.getRankDlgt(this.setRankF));
			List<Api_req_Ending.getRankDlgt> list2 = list;
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			DateTime dateTime = Comm_UserDatas.Instance.User_turn.GetDateTime();
			int elapsedYear = Comm_UserDatas.Instance.User_turn.GetElapsedYear(dateTime);
			uint lostShipNum = Comm_UserDatas.Instance.User_record.LostShipNum;
			bool result = true;
			using (List<Api_req_Ending.getRankDlgt>.Enumerator enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Api_req_Ending.getRankDlgt current = enumerator.get_Current();
					if (current(difficult, dateTime, elapsedYear, lostShipNum, ref clearRank, ref decorationNum, ref result))
					{
						return result;
					}
				}
			}
			clearRank = OverallRank.F;
			return result;
		}

		public int GetTakeOverShipCount()
		{
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.SHI))
			{
				return 88;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.KOU))
			{
				return 65;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.OTU))
			{
				return 50;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.HEI))
			{
				return 35;
			}
			return 20;
		}

		public int GetTakeOverSlotCount()
		{
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.SHI))
			{
				return 50;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.KOU))
			{
				return 40;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.OTU))
			{
				return 30;
			}
			if (Comm_UserDatas.Instance.User_record.ClearDifficult.Contains(DifficultKind.HEI))
			{
				return 20;
			}
			return 10;
		}

		public bool IsGoTrueEnd()
		{
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			return Enumerable.Any<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values(), (Mem_ship x) => mst_ship.get_Item(x.Ship_id).Yomi.Equals("アイオワ"));
		}

		public void PurgeNewGamePlus()
		{
			this.mem_newgame.PurgeData();
		}

		public void CreateNewGamePlusData(bool level)
		{
			int priority = (!level) ? 1 : 0;
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			List<Mem_ship> ships = Enumerable.ToList<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values());
			Mem_ship mem_ship = null;
			Comm_UserDatas.Instance.User_ship.TryGetValue(this.mem_newgame.TempRewardShipRid, ref mem_ship);
			List<Mem_ship> sortedShipList = this.getSortedShipList(ships, priority);
			if (mem_ship != null)
			{
				sortedShipList.Remove(mem_ship);
				sortedShipList.Insert(0, mem_ship);
			}
			IEnumerable<Mem_ship> enumerable = Enumerable.Take<Mem_ship>(sortedShipList, this.GetTakeOverShipCount());
			List<Mem_shipBase> list = new List<Mem_shipBase>();
			List<Mem_slotitem> takeOverSlotItems = new List<Mem_slotitem>();
			using (IEnumerator<Mem_ship> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					Mem_shipBase mem_shipBase = new Mem_shipBase(current);
					list.Add(mem_shipBase);
					current.Slot.ForEach(delegate(int slot_rid)
					{
						if (slot_rid > 0)
						{
							takeOverSlotItems.Add(Comm_UserDatas.Instance.User_slot.get_Item(slot_rid));
						}
					});
					if (current.Exslot > 0)
					{
						takeOverSlotItems.Add(Comm_UserDatas.Instance.User_slot.get_Item(current.Exslot));
					}
				}
			}
			IEnumerable<Mem_slotitem> enumerable2 = Enumerable.Except<Mem_slotitem>(Comm_UserDatas.Instance.User_slot.get_Values(), takeOverSlotItems);
			IEnumerable<Mem_slotitem> enumerable3 = Enumerable.Where<Mem_slotitem>(enumerable2, (Mem_slotitem x) => x.Equip_flag == Mem_slotitem.enumEquipSts.Unset);
			List<Mem_slotitem> sortedSlotList = this.getSortedSlotList(Enumerable.ToList<Mem_slotitem>(enumerable3));
			IEnumerable<Mem_slotitem> enumerable4 = Enumerable.Take<Mem_slotitem>(sortedSlotList, this.GetTakeOverSlotCount());
			takeOverSlotItems.AddRange(enumerable4);
			this.mem_newgame.SetData(list, takeOverSlotItems);
		}

		public List<Mem_shipBase> GetTakeOverShips()
		{
			return Enumerable.ToList<Mem_shipBase>(this.mem_newgame.Ship);
		}

		public List<Mem_slotitem> GetTakeOverSlotItems()
		{
			return Enumerable.ToList<Mem_slotitem>(this.mem_newgame.Slotitem);
		}

		private List<Mem_ship> getSortedShipList(List<Mem_ship> ships, int priority)
		{
			List<Mem_ship> range = ships.GetRange(0, ships.get_Count());
			range.Sort((Mem_ship a, Mem_ship b) => this._compShip(a, b, priority));
			return range;
		}

		private List<Mem_slotitem> getSortedSlotList(List<Mem_slotitem> items)
		{
			List<Mem_slotitem> range = items.GetRange(0, items.get_Count());
			range.Sort((Mem_slotitem a, Mem_slotitem b) => this._compSlot(a, b));
			return range;
		}

		private int _compShip(Mem_ship a, Mem_ship b, int priority)
		{
			int num = this.__compMarriage(a, b);
			if (num != 0)
			{
				return num;
			}
			if (priority == 0)
			{
				num = this.__compLevel(a, b);
				if (num != 0)
				{
					return num;
				}
				num = this.__compLock(a, b);
				if (num != 0)
				{
					return num;
				}
			}
			else if (priority == 1)
			{
				num = this.__compLock(a, b);
				if (num != 0)
				{
					return num;
				}
				num = this.__compLevel(a, b);
				if (num != 0)
				{
					return num;
				}
			}
			num = this.__compLov(a, b);
			if (num != 0)
			{
				return num;
			}
			return this.__compGetNo(a, b);
		}

		private int _compSlot(Mem_slotitem a, Mem_slotitem b)
		{
			int num = this.__compSlotLock(a, b);
			if (num != 0)
			{
				return num;
			}
			num = this.__compSlotRare(a, b);
			if (num != 0)
			{
				return num;
			}
			num = this.__compSlotLevel(a, b);
			if (num != 0)
			{
				return num;
			}
			return this.__compSlotGetNo(a, b);
		}

		private int __compMarriage(Mem_ship a, Mem_ship b)
		{
			bool flag = a.Level >= 100;
			bool flag2 = b.Level >= 100;
			if (!flag && flag2)
			{
				return 1;
			}
			if (flag && !flag2)
			{
				return -1;
			}
			return 0;
		}

		private int __compLevel(Mem_ship a, Mem_ship b)
		{
			if (a.Level < b.Level)
			{
				return 1;
			}
			if (a.Level > b.Level)
			{
				return -1;
			}
			return 0;
		}

		private int __compSortNo(Mem_ship a, Mem_ship b)
		{
			if (a.Sortno > b.Sortno)
			{
				return 1;
			}
			if (a.Sortno < b.Sortno)
			{
				return -1;
			}
			return 0;
		}

		private int __compGetNo(Mem_ship a, Mem_ship b)
		{
			if (a.GetNo > b.GetNo)
			{
				return 1;
			}
			if (a.GetNo < b.GetNo)
			{
				return -1;
			}
			return 0;
		}

		private int __compLock(Mem_ship a, Mem_ship b)
		{
			if (a.Locked < b.Locked)
			{
				return 1;
			}
			if (a.Locked > b.Locked)
			{
				return -1;
			}
			return 0;
		}

		private int __compLov(Mem_ship a, Mem_ship b)
		{
			if (a.Lov < b.Lov)
			{
				return 1;
			}
			if (a.Lov > b.Lov)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotLock(Mem_slotitem a, Mem_slotitem b)
		{
			if (!a.Lock && b.Lock)
			{
				return 1;
			}
			if (a.Lock && !b.Lock)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotRare(Mem_slotitem a, Mem_slotitem b)
		{
			Dictionary<int, Mst_slotitem> mst_Slotitem = Mst_DataManager.Instance.Mst_Slotitem;
			if (mst_Slotitem.get_Item(a.Slotitem_id).Rare < mst_Slotitem.get_Item(b.Slotitem_id).Rare)
			{
				return 1;
			}
			if (mst_Slotitem.get_Item(a.Slotitem_id).Rare > mst_Slotitem.get_Item(b.Slotitem_id).Rare)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotLevel(Mem_slotitem a, Mem_slotitem b)
		{
			if (a.Level < b.Level)
			{
				return 1;
			}
			if (a.Level > b.Level)
			{
				return -1;
			}
			return 0;
		}

		private int __compSlotGetNo(Mem_slotitem a, Mem_slotitem b)
		{
			if (a.GetNo > b.GetNo)
			{
				return 1;
			}
			if (a.GetNo < b.GetNo)
			{
				return -1;
			}
			return 0;
		}

		private bool setRankEx(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			outPositive = true;
			outRank = OverallRank.EX;
			outDecNum = 0;
			if (difficult != DifficultKind.SHI)
			{
				return false;
			}
			if (elapsedYear <= 1 && lostNum == 0u)
			{
				outDecNum = 4;
				return true;
			}
			if (elapsedYear <= 2 && lostNum == 0u)
			{
				outDecNum = 3;
				return true;
			}
			if (elapsedYear <= 3 && lostNum == 0u)
			{
				outDecNum = 2;
				DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
				if (dt.get_Date() <= dateTime.get_Date())
				{
					return true;
				}
			}
			if (elapsedYear <= 2 && lostNum <= 3u)
			{
				outDecNum = 1;
				return true;
			}
			if (elapsedYear <= 3 && lostNum <= 3u)
			{
				outDecNum = 0;
				DateTime dateTime2 = user_turn.GetDateTime(3, 8, 15);
				if (dt.get_Date() <= dateTime2.get_Date())
				{
					return true;
				}
			}
			return false;
		}

		private bool setRankS(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			if (difficult < DifficultKind.OTU)
			{
				return false;
			}
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			outPositive = true;
			outRank = OverallRank.S;
			outDecNum = 0;
			if (elapsedYear <= 1 && lostNum == 0u)
			{
				outDecNum = 3;
				return true;
			}
			if (difficult >= DifficultKind.OTU)
			{
				outDecNum = 2;
				if (difficult == DifficultKind.SHI)
				{
					if (lostNum <= 6u)
					{
						return true;
					}
				}
				else if (elapsedYear <= 2 && lostNum == 0u)
				{
					return true;
				}
			}
			if (difficult >= DifficultKind.OTU)
			{
				outDecNum = 1;
				if (difficult == DifficultKind.SHI)
				{
					if (lostNum <= 9u)
					{
						return true;
					}
				}
				else if (elapsedYear <= 3)
				{
					DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
					if (dt.get_Date() <= dateTime.get_Date() && lostNum == 0u)
					{
						return true;
					}
				}
			}
			if (difficult >= DifficultKind.OTU)
			{
				outDecNum = 0;
				if (difficult == DifficultKind.SHI)
				{
					if (lostNum >= 10u && lostNum <= 19u)
					{
						return true;
					}
				}
				else
				{
					DateTime dateTime2 = user_turn.GetDateTime(3, 8, 15);
					if (dt.get_Date() <= dateTime2.get_Date() && lostNum <= 3u)
					{
						return true;
					}
				}
			}
			return false;
		}

		private bool setRankA(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			if (difficult > DifficultKind.KOU)
			{
				return false;
			}
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			outRank = OverallRank.A;
			outPositive = true;
			outDecNum = 0;
			if (difficult == DifficultKind.KOU && lostNum <= 6u)
			{
				DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
				outDecNum = 3;
				if (dt.get_Date() <= dateTime.get_Date())
				{
					return true;
				}
			}
			if (difficult <= DifficultKind.OTU)
			{
				outDecNum = 2;
				if (difficult != DifficultKind.OTU)
				{
					if (elapsedYear <= 1 && lostNum == 0u)
					{
						return true;
					}
				}
				else
				{
					DateTime dateTime2 = user_turn.GetDateTime(3, 8, 15);
					if (lostNum <= 6u && dt.get_Date() <= dateTime2.get_Date())
					{
						return true;
					}
				}
			}
			if (difficult <= DifficultKind.HEI)
			{
				outDecNum = 1;
				if (elapsedYear <= 2 && lostNum == 0u)
				{
					return true;
				}
			}
			outDecNum = 0;
			if (difficult <= DifficultKind.HEI)
			{
				DateTime dateTime3 = user_turn.GetDateTime(3, 8, 15);
				if (dt.get_Date() <= dateTime3.get_Date() && lostNum == 0u)
				{
					return true;
				}
			}
			return false;
		}

		private bool setRankB(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			Mem_turn user_turn = Comm_UserDatas.Instance.User_turn;
			DateTime dateTime = user_turn.GetDateTime(3, 8, 15);
			outDecNum = 0;
			outPositive = true;
			outRank = OverallRank.B;
			if (dt.get_Date() <= dateTime.get_Date())
			{
				outDecNum = 2;
				if (difficult <= DifficultKind.HEI && lostNum <= 4u)
				{
					return true;
				}
				if (difficult == DifficultKind.SHI && lostNum <= 19u)
				{
					return true;
				}
				if ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && lostNum <= 9u)
				{
					return true;
				}
			}
			outDecNum = 1;
			if (dt.get_Date() <= dateTime.get_Date())
			{
				if (difficult <= DifficultKind.HEI && lostNum <= 9u)
				{
					return true;
				}
				if ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && lostNum <= 14u)
				{
					return true;
				}
			}
			if ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && lostNum <= 9u)
			{
				return true;
			}
			outDecNum = 0;
			return (difficult <= DifficultKind.HEI && lostNum <= 9u) || ((difficult == DifficultKind.KOU || difficult == DifficultKind.OTU) && dt.get_Date() <= dateTime.get_Date() && lostNum <= 19u) || (difficult == DifficultKind.SHI && lostNum <= 19u);
		}

		private bool setRankC(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outDecNum = 0;
			outPositive = true;
			outRank = OverallRank.C;
			if (difficult <= DifficultKind.KOU && lostNum >= 10u && lostNum <= 19u)
			{
				return true;
			}
			outPositive = false;
			if (lostNum >= 20u && lostNum <= 24u)
			{
				outDecNum = 1;
				return true;
			}
			if (lostNum >= 25u && lostNum <= 29u)
			{
				outDecNum = 2;
				return true;
			}
			return false;
		}

		private bool setRankD(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outRank = OverallRank.D;
			outDecNum = 0;
			outPositive = true;
			return lostNum >= 30u && lostNum <= 34u;
		}

		private bool setRankE(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outRank = OverallRank.E;
			outDecNum = 0;
			outPositive = true;
			return lostNum >= 35u && lostNum <= 39u;
		}

		private bool setRankF(DifficultKind difficult, DateTime dt, int elapsedYear, uint lostNum, ref OverallRank outRank, ref int outDecNum, ref bool outPositive)
		{
			outRank = OverallRank.F;
			outDecNum = 0;
			outPositive = true;
			return lostNum >= 40u;
		}
	}
}
