using Common.Enum;
using Server_Common.Formats;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Controllers.QuestLogic
{
	public class QuestPractice : QuestLogicBase
	{
		private BattleResultFmt battleResult;

		private DeckPracticeType deckPrackType;

		private PracticeDeckResultFmt deckPracticeResult;

		private QuestPractice()
		{
			this.checkData = base.getCheckDatas(3);
			this.battleResult = null;
			this.deckPracticeResult = null;
		}

		public QuestPractice(BattleResultFmt battleResultFmt) : this()
		{
			this.battleResult = battleResultFmt;
		}

		public QuestPractice(DeckPracticeType pracType, PracticeDeckResultFmt pracResultFmt) : this()
		{
			this.deckPrackType = pracType;
			this.deckPracticeResult = pracResultFmt;
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
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Normal)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Hou)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Rai)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Hou)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_05(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Normal && this.deckPrackType != DeckPracticeType.Rai)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (this.deckPrackType != DeckPracticeType.Normal) ? 0 : 1;
			int num2 = (this.deckPrackType != DeckPracticeType.Rai) ? 0 : 1;
			if (dictionary.ContainsKey(3001))
			{
				dictionary.set_Item(3001, num);
			}
			if (dictionary.ContainsKey(3003))
			{
				dictionary.set_Item(3003, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_06(Mem_quest targetQuest)
		{
			if (this.battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_07(Mem_quest targetQuest)
		{
			if (this.battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_08(Mem_quest targetQuest)
		{
			if (this.battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_09(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Normal)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_10(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Hou)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_11(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Normal && this.deckPrackType != DeckPracticeType.Rai)
			{
				return false;
			}
			Dictionary<int, int> dictionary = base.isAddCounter(targetQuest.Rid, this.checkData);
			int num = (this.deckPrackType != DeckPracticeType.Normal) ? 0 : 1;
			int num2 = (this.deckPrackType != DeckPracticeType.Rai) ? 0 : 1;
			if (dictionary.ContainsKey(3001))
			{
				dictionary.set_Item(3001, num);
			}
			if (dictionary.ContainsKey(3003))
			{
				dictionary.set_Item(3003, num2);
			}
			base.addCounter(dictionary);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_12(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Taisen)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_13(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Sougou)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_14(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Kouku)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_15(Mem_quest targetQuest)
		{
			if (this.battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_16(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Taisen)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_17(Mem_quest targetQuest)
		{
			if (this.battleResult == null)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_18(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Kouku)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_19(Mem_quest targetQuest)
		{
			if (this.deckPracticeResult == null)
			{
				return false;
			}
			if (this.deckPrackType != DeckPracticeType.Sougou)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}
	}
}
