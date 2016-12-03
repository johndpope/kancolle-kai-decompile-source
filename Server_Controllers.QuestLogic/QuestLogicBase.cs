using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.QuestLogic
{
	public abstract class QuestLogicBase : IQuestOperator
	{
		protected List<Mem_quest> checkData;

		protected Dictionary<int, Mst_questcount> mst_count;

		protected Dictionary<int, Mem_quest> questData;

		protected Dictionary<int, Mem_questcount> mem_count;

		private Dictionary<int, int> mst_counter_limit;

		public QuestLogicBase()
		{
			this.questData = Comm_UserDatas.Instance.User_quest;
			this.mem_count = Comm_UserDatas.Instance.User_questcount;
			this.mst_count = Mst_DataManager.Instance.Mst_questcount;
			this.mst_counter_limit = ArrayMaster.GetQuestCounterLimit();
		}

		public abstract List<int> ExecuteCheck();

		public bool CheckClearCounter(int quest_id)
		{
			Dictionary<int, int> clear_num = this.mst_count.get_Item(quest_id).Clear_num;
			if (clear_num.get_Count() != this.mst_count.get_Item(quest_id).Counter_id.get_Count())
			{
				return this.checkCalcCounter(this.mst_count.get_Item(quest_id).Counter_id, Enumerable.First<int>(clear_num.get_Values()));
			}
			using (Dictionary<int, int>.Enumerator enumerator = clear_num.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					int key = current.get_Key();
					Mem_questcount mem_questcount = null;
					if (!this.mem_count.TryGetValue(key, ref mem_questcount))
					{
						bool result = false;
						return result;
					}
					if (mem_questcount.Value < current.get_Value())
					{
						bool result = false;
						return result;
					}
				}
			}
			return true;
		}

		private bool checkCalcCounter(HashSet<int> target, int clearNum)
		{
			int num = 0;
			using (HashSet<int>.Enumerator enumerator = target.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					Mem_questcount mem_questcount = null;
					if (this.mem_count.TryGetValue(current, ref mem_questcount))
					{
						num += mem_questcount.Value;
					}
				}
			}
			return num >= clearNum;
		}

		protected List<Mem_quest> getCheckDatas(int category)
		{
			IEnumerable<Mem_quest> enumerable = Enumerable.Where<Mem_quest>(this.questData.get_Values(), (Mem_quest x) => x.State == QuestState.RUNNING && this.questData.get_Item(x.Rid).Category == category);
			List<Mem_quest> arg_69_0;
			if (enumerable != null)
			{
				arg_69_0 = Enumerable.ToList<Mem_quest>(Enumerable.OrderBy<Mem_quest, int>(enumerable, (Mem_quest x) => x.Rid));
			}
			else
			{
				arg_69_0 = new List<Mem_quest>();
			}
			return arg_69_0;
		}

		protected Dictionary<int, int> isAddCounter(int questId, List<Mem_quest> nowExecQuestList)
		{
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<int, int, int>(this.mst_count.get_Item(questId).Counter_id, (int x) => x, (int y) => 0);
			using (List<Mem_quest>.Enumerator enumerator = nowExecQuestList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_quest current = enumerator.get_Current();
					if (current.Rid == questId)
					{
						Dictionary<int, int> result = dictionary;
						return result;
					}
					if (this.mst_count.ContainsKey(current.Rid))
					{
						HashSet<int> counter_id = this.mst_count.get_Item(current.Rid).Counter_id;
						using (HashSet<int>.Enumerator enumerator2 = counter_id.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								int current2 = enumerator2.get_Current();
								if (dictionary.ContainsKey(current2))
								{
									dictionary.Remove(current2);
								}
								if (dictionary.get_Count() == 0)
								{
									Dictionary<int, int> result = dictionary;
									return result;
								}
							}
						}
					}
				}
			}
			return dictionary;
		}

		protected void addCounterIncrementAll(Dictionary<int, int> counter)
		{
			if (counter.get_Count() == 0)
			{
				return;
			}
			List<int> list = Enumerable.ToList<int>(counter.get_Keys());
			using (List<int>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					counter.set_Item(current, 1);
				}
			}
			this.addCounter(counter);
		}

		protected void addCounter(Dictionary<int, int> addValues)
		{
			using (Dictionary<int, int>.Enumerator enumerator = addValues.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, int> current = enumerator.get_Current();
					Mem_questcount mem_questcount = null;
					if (!this.mem_count.TryGetValue(current.get_Key(), ref mem_questcount))
					{
						mem_questcount = new Mem_questcount(current.get_Key(), current.get_Value());
						this.mem_count.Add(mem_questcount.Rid, mem_questcount);
					}
					else
					{
						mem_questcount.AddCount(current.get_Value());
					}
					int num = 0;
					if (this.mst_counter_limit.TryGetValue(mem_questcount.Rid, ref num) && mem_questcount.Value > num)
					{
						mem_questcount.Reset(false);
						mem_questcount.AddCount(num);
					}
				}
			}
		}

		protected string getFuncName(Mem_quest quest)
		{
			string text = quest.Rid.ToString().Substring(quest.Category.ToString().get_Length());
			return "Check_" + text;
		}
	}
}
