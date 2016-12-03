using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Controllers.QuestLogic
{
	public class QuestMission : QuestLogicBase
	{
		private Mem_deck missionDeck;

		private int missionId;

		private MissionResultKinds missionResult;

		private Dictionary<int, HashSet<int>> commonCounterEnableMission;

		public QuestMission(int mstId, Mem_deck deck, MissionResultKinds resultKind)
		{
			this.checkData = base.getCheckDatas(4);
			this.missionId = mstId;
			this.missionDeck = deck;
			this.missionResult = resultKind;
			Dictionary<int, HashSet<int>> dictionary = new Dictionary<int, HashSet<int>>();
			Dictionary<int, HashSet<int>> arg_66_0 = dictionary;
			int arg_66_1 = 43;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(10);
			hashSet.Add(11);
			hashSet.Add(12);
			hashSet.Add(51);
			hashSet.Add(52);
			arg_66_0.Add(arg_66_1, hashSet);
			Dictionary<int, HashSet<int>> arg_7E_0 = dictionary;
			int arg_7E_1 = 45;
			hashSet = new HashSet<int>();
			hashSet.Add(60);
			arg_7E_0.Add(arg_7E_1, hashSet);
			Dictionary<int, HashSet<int>> arg_96_0 = dictionary;
			int arg_96_1 = 46;
			hashSet = new HashSet<int>();
			hashSet.Add(67);
			arg_96_0.Add(arg_96_1, hashSet);
			Dictionary<int, HashSet<int>> arg_AE_0 = dictionary;
			int arg_AE_1 = 47;
			hashSet = new HashSet<int>();
			hashSet.Add(45);
			arg_AE_0.Add(arg_AE_1, hashSet);
			Dictionary<int, HashSet<int>> arg_C6_0 = dictionary;
			int arg_C6_1 = 48;
			hashSet = new HashSet<int>();
			hashSet.Add(46);
			arg_C6_0.Add(arg_C6_1, hashSet);
			Dictionary<int, HashSet<int>> arg_DE_0 = dictionary;
			int arg_DE_1 = 49;
			hashSet = new HashSet<int>();
			hashSet.Add(52);
			arg_DE_0.Add(arg_DE_1, hashSet);
			Dictionary<int, HashSet<int>> arg_110_0 = dictionary;
			int arg_110_1 = 50;
			hashSet = new HashSet<int>();
			hashSet.Add(3);
			hashSet.Add(9);
			hashSet.Add(13);
			hashSet.Add(20);
			arg_110_0.Add(arg_110_1, hashSet);
			this.commonCounterEnableMission = dictionary;
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
			if (this.missionDeck == null)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!this.commonCounterEnableMission.get_Item(43).Contains(this.missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!this.commonCounterEnableMission.get_Item(45).Contains(this.missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			return this.Check_05(targetQuest);
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!this.commonCounterEnableMission.get_Item(50).Contains(this.missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			return this.Check_05(targetQuest);
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!this.commonCounterEnableMission.get_Item(46).Contains(this.missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (!this.commonCounterEnableMission.get_Item(47).Contains(this.missionId)) ? 0 : 1;
			int num2 = (!this.commonCounterEnableMission.get_Item(48).Contains(this.missionId)) ? 0 : 1;
			if (dictionary.ContainsKey(4007))
			{
				dictionary.set_Item(4007, num);
			}
			if (dictionary.ContainsKey(4008))
			{
				dictionary.set_Item(4008, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			return this.Check_10(targetQuest);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (!this.commonCounterEnableMission.get_Item(49).Contains(this.missionId))
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			return this.Check_12(targetQuest);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (this.missionId != 36)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (this.missionId != 107)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (this.missionId != 4)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			return this.missionDeck != null && this.missionResult != MissionResultKinds.FAILE && this.missionId == 14;
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			if (this.missionId != 5)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			return this.missionDeck != null && this.missionResult != MissionResultKinds.FAILE && this.missionId == 6;
		}

		public bool Check_20(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (this.missionId != 44) ? 0 : 1;
			int num2 = (this.missionId != 66) ? 0 : 1;
			if (dictionary.ContainsKey(4205))
			{
				dictionary.set_Item(4205, num);
			}
			if (dictionary.ContainsKey(4206))
			{
				dictionary.set_Item(4206, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_21(Mem_quest targetQuest)
		{
			return this.Check_05(targetQuest);
		}

		public bool Check_22(Mem_quest targetQuest)
		{
			return this.Check_09(targetQuest);
		}

		public bool Check_23(Mem_quest targetQuest)
		{
			if (this.missionDeck == null)
			{
				return false;
			}
			if (this.missionResult == MissionResultKinds.FAILE)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (this.missionId != 84) ? 0 : 1;
			int num2 = (this.missionId != 108) ? 0 : 1;
			if (dictionary.ContainsKey(4207))
			{
				dictionary.set_Item(4207, num);
			}
			if (dictionary.ContainsKey(4208))
			{
				dictionary.set_Item(4208, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}
	}
}
