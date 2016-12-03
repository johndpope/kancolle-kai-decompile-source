using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Controllers.QuestLogic
{
	public class QuestSupply : QuestLogicBase
	{
		private int supplyShipCount;

		private int type;

		public QuestSupply()
		{
			this.checkData = base.getCheckDatas(5);
			this.type = 1;
		}

		public QuestSupply(int shipCount)
		{
			this.checkData = base.getCheckDatas(5);
			this.supplyShipCount = shipCount;
			this.type = 2;
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
			if (this.type != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_02(Mem_quest targetQuest)
		{
			if (this.type != 1)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_03(Mem_quest targetQuest)
		{
			if (this.type != 1)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}

		public bool Check_04(Mem_quest targetQuest)
		{
			if (this.type != 2)
			{
				return false;
			}
			Dictionary<int, int> counter = base.isAddCounter(targetQuest.Rid, this.checkData);
			base.addCounterIncrementAll(counter);
			return base.CheckClearCounter(targetQuest.Rid);
		}
	}
}
