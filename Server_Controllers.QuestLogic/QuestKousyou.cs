using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.QuestLogic
{
	public class QuestKousyou : QuestLogicBase
	{
		private enum KousyouKind
		{
			CreateShip = 1,
			CreateSlot,
			DestroyShip,
			DestroyItem,
			RemodelSlot
		}

		private QuestKousyou.KousyouKind type;

		private int createShipId;

		private Mst_ship destroyShip;

		private List<Mst_slotitem> destroyItems;

		private Mst_slotitem_remodel_detail remodelDetail;

		private Mem_slotitem remodelAfterSlot;

		private Dictionary<enumMaterialCategory, int> useMaterial;

		private bool successFlag;

		private QuestKousyou()
		{
			this.checkData = base.getCheckDatas(6);
		}

		public QuestKousyou(Dictionary<enumMaterialCategory, int> useMat, int createShip) : this()
		{
			this.type = QuestKousyou.KousyouKind.CreateShip;
			this.useMaterial = useMat;
			this.createShipId = createShip;
		}

		public QuestKousyou(Dictionary<enumMaterialCategory, int> useMat, bool success) : this()
		{
			this.type = QuestKousyou.KousyouKind.CreateSlot;
			this.useMaterial = useMat;
			this.successFlag = success;
		}

		public QuestKousyou(Mst_ship destroyShip) : this()
		{
			this.type = QuestKousyou.KousyouKind.DestroyShip;
			this.destroyShip = destroyShip;
		}

		public QuestKousyou(List<Mst_slotitem> destroySlotItem) : this()
		{
			this.type = QuestKousyou.KousyouKind.DestroyItem;
			this.destroyItems = destroySlotItem;
		}

		public QuestKousyou(Mst_slotitem_remodel_detail menuData, Mem_slotitem afterSlotItem, bool success) : this()
		{
			this.type = QuestKousyou.KousyouKind.RemodelSlot;
			this.remodelDetail = menuData;
			this.remodelAfterSlot = afterSlotItem;
			this.successFlag = success;
		}

		public override List<int> ExecuteCheck()
		{
			List<int> list = new List<int>(this.checkData.get_Count());
			using (List<Mem_quest>.Enumerator enumerator = this.checkData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_quest current = enumerator.get_Current();
					string funcName = base.getFuncName(current);
					bool flag = (bool)base.GetType().InvokeMember(funcName, 256, null, this, new object[]
					{
						current
					});
					if (flag)
					{
						current.StateChange(this, QuestState.COMPLETE);
						list.Add(current.Rid);
					}
				}
			}
			return list;
		}

		public bool Check_01(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.CreateShip)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.CreateSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyShip)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.CreateSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.CreateShip)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.CreateSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.CreateShip)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyShip)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			int count = this.destroyItems.get_Count();
			for (int i = 0; i < count; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			int count = this.destroyItems.get_Count();
			for (int i = 0; i < count; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			int count = this.destroyItems.get_Count();
			for (int i = 0; i < count; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			int count = this.destroyItems.get_Count();
			for (int i = 0; i < count; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(93);
			Dictionary<int, int> mstSlotItemNum_OrderId = arg_3D_0.GetMstSlotItemNum_OrderId(hashSet);
			if (mstSlotItemNum_OrderId.get_Item(93) == 0)
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 17);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(99);
			Dictionary<int, int> mstSlotItemNum_OrderId = arg_3D_0.GetMstSlotItemNum_OrderId(hashSet);
			if (mstSlotItemNum_OrderId.get_Item(99) == 0)
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 24);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(109);
			Dictionary<int, int> mstSlotItemNum_OrderId = arg_3D_0.GetMstSlotItemNum_OrderId(hashSet);
			if (mstSlotItemNum_OrderId.get_Item(109) == 0)
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 22);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			int count = this.destroyItems.get_Count();
			for (int i = 0; i < count; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.RemodelSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.RemodelSlot)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_20(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(16);
			Dictionary<int, List<int>> slotIndexFromId = arg_3D_0.GetSlotIndexFromId(hashSet);
			if (slotIndexFromId.get_Item(16).get_Count() == 0)
			{
				return false;
			}
			bool flag = false;
			using (List<int>.Enumerator enumerator = slotIndexFromId.get_Item(16).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (Comm_UserDatas.Instance.User_slot.get_Item(flagShip.Slot.get_Item(current)).IsMaxSkillLevel())
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 16);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_21(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(23);
			Dictionary<int, List<int>> slotIndexFromId = arg_3D_0.GetSlotIndexFromId(hashSet);
			if (slotIndexFromId.get_Item(23).get_Count() == 0)
			{
				return false;
			}
			bool flag = false;
			using (List<int>.Enumerator enumerator = slotIndexFromId.get_Item(23).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (Comm_UserDatas.Instance.User_slot.get_Item(flagShip.Slot.get_Item(current)).IsMaxSkillLevel())
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 23);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_22(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			string yomi = Mst_DataManager.Instance.Mst_ship.get_Item(flagShip.Ship_id).Yomi;
			if (!yomi.Equals("しょうかく"))
			{
				return false;
			}
			Mem_ship arg_6D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(143);
			Dictionary<int, int> mstSlotItemNum_OrderId = arg_6D_0.GetMstSlotItemNum_OrderId(hashSet);
			if (mstSlotItemNum_OrderId.get_Item(143) == 0)
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 17);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_23(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			string yomi = Mst_DataManager.Instance.Mst_ship.get_Item(flagShip.Ship_id).Yomi;
			if (!yomi.Equals("しょうかく") && !yomi.Equals("あかぎ"))
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 16);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_24(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			int count = this.destroyItems.get_Count();
			for (int i = 0; i < count; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_25(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			string yomi = Mst_DataManager.Instance.Mst_ship.get_Item(flagShip.Ship_id).Yomi;
			if (!yomi.Equals("ほうしょう"))
			{
				return false;
			}
			Mem_ship arg_6A_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(20);
			Dictionary<int, List<int>> slotIndexFromId = arg_6A_0.GetSlotIndexFromId(hashSet);
			if (slotIndexFromId.get_Item(20).get_Count() == 0)
			{
				return false;
			}
			bool flag = false;
			using (List<int>.Enumerator enumerator = slotIndexFromId.get_Item(20).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (Comm_UserDatas.Instance.User_slot.get_Item(flagShip.Slot.get_Item(current)).IsMaxSkillLevel())
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = 2;
			int num2 = 0;
			Mem_questcount mem_questcount = new Mem_questcount();
			if (Comm_UserDatas.Instance.User_questcount.TryGetValue(6806, ref mem_questcount))
			{
				num2 = mem_questcount.Value;
			}
			int num3 = 1;
			int num4 = 0;
			Mem_questcount mem_questcount2 = new Mem_questcount();
			if (Comm_UserDatas.Instance.User_questcount.TryGetValue(6807, ref mem_questcount2))
			{
				num4 = mem_questcount2.Value;
			}
			using (List<Mst_slotitem>.Enumerator enumerator2 = this.destroyItems.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mst_slotitem current2 = enumerator2.get_Current();
					if (current2.Id == 20 && num2 < num)
					{
						num2++;
					}
					if (current2.Id == 19 && num4 < num3)
					{
						num4++;
					}
				}
			}
			if (dictionary.ContainsKey(6806))
			{
				dictionary.set_Item(6806, num2);
			}
			if (dictionary.ContainsKey(6807))
			{
				dictionary.set_Item(6807, num4);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_26(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(96);
			Dictionary<int, int> mstSlotItemNum_OrderId = arg_3D_0.GetMstSlotItemNum_OrderId(hashSet);
			if (mstSlotItemNum_OrderId.get_Item(96) == 0)
			{
				return false;
			}
			int num = Enumerable.Count<Mst_slotitem>(this.destroyItems, (Mst_slotitem x) => x.Id == 21);
			for (int i = 0; i < num; i++)
			{
				base.addCounterIncrementAll(counter);
			}
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_27(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(22);
			Dictionary<int, List<int>> slotIndexFromId = arg_3D_0.GetSlotIndexFromId(hashSet);
			if (slotIndexFromId.get_Item(22).get_Count() == 0)
			{
				return false;
			}
			bool flag = false;
			using (List<int>.Enumerator enumerator = slotIndexFromId.get_Item(22).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (Comm_UserDatas.Instance.User_slot.get_Item(flagShip.Slot.get_Item(current)).IsMaxSkillLevel())
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			using (List<Mst_slotitem>.Enumerator enumerator2 = this.destroyItems.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mst_slotitem current2 = enumerator2.get_Current();
					if (current2.Id == 22)
					{
						num++;
					}
					if (current2.Id == 21)
					{
						num2++;
					}
				}
			}
			if (dictionary.ContainsKey(6809) && num > 0)
			{
				dictionary.set_Item(6809, num);
			}
			if (dictionary.ContainsKey(6810) && num2 > 0)
			{
				dictionary.set_Item(6810, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_28(Mem_quest targetQuest)
		{
			if (this.type != QuestKousyou.KousyouKind.DestroyItem)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			Mem_ship flagShip = this.getFlagShip(1);
			Mem_ship arg_3D_0 = flagShip;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(79);
			Dictionary<int, List<int>> slotIndexFromId = arg_3D_0.GetSlotIndexFromId(hashSet);
			if (slotIndexFromId.get_Item(79).get_Count() == 0)
			{
				return false;
			}
			bool flag = false;
			using (List<int>.Enumerator enumerator = slotIndexFromId.get_Item(79).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (Comm_UserDatas.Instance.User_slot.get_Item(flagShip.Slot.get_Item(current)).IsMaxSkillLevel())
					{
						flag = true;
					}
				}
			}
			if (!flag)
			{
				return false;
			}
			int num = 0;
			int num2 = 0;
			using (List<Mst_slotitem>.Enumerator enumerator2 = this.destroyItems.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Mst_slotitem current2 = enumerator2.get_Current();
					if (current2.Id == 80)
					{
						num++;
					}
					if (current2.Id == 26)
					{
						num2++;
					}
				}
			}
			if (dictionary.ContainsKey(6811))
			{
				dictionary.set_Item(6811, num);
			}
			if (dictionary.ContainsKey(6812))
			{
				dictionary.set_Item(6812, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public static List<string> GetRequireShip(int quest_id)
		{
			List<string> list = new List<string>();
			if (quest_id == 622)
			{
				list.Add("しょうかく");
			}
			if (quest_id == 625)
			{
				list.Add("ほうしょう");
			}
			return list;
		}

		private Mem_ship getFlagShip(int deckRid)
		{
			int num = Comm_UserDatas.Instance.User_deck.get_Item(deckRid).Ship[0];
			return Comm_UserDatas.Instance.User_ship.get_Item(num);
		}
	}
}
