using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common.Formats
{
	public class User_QuestFmt
	{
		public int No;

		public int Category;

		public QuestState State;

		public string Title;

		public string Detail;

		public Dictionary<enumMaterialCategory, int> GetMaterial;

		public int GetSpoint;

		public QuestProgressKinds Progress;

		public bool InvalidFlag;

		public int Type;

		public List<QuestItemGetKind> RewardTypes;

		public List<int> RewardCount;

		public User_QuestFmt()
		{
			this.State = QuestState.NOT_DISP;
			this.Progress = QuestProgressKinds.NOT_DISP;
			this.GetMaterial = new Dictionary<enumMaterialCategory, int>();
			this.RewardTypes = new List<QuestItemGetKind>();
			this.RewardCount = new List<int>();
		}

		public User_QuestFmt(Mem_quest memObj, Mst_quest mstObj) : this()
		{
			this.No = memObj.Rid;
			this.Category = mstObj.Category;
			this.State = memObj.State;
			this.Title = mstObj.Name;
			this.Detail = mstObj.Details;
			this.Type = mstObj.Type;
			this.GetMaterial = mstObj.GetMaterialValues();
			this.GetSpoint = mstObj.GetSpointNum();
			this.Progress = this.setProgress();
			if (mstObj.Get_1_type > 0)
			{
				this.RewardTypes.Add((QuestItemGetKind)mstObj.Get_1_type);
				this.RewardCount.Add(mstObj.Get_1_count);
			}
			if (mstObj.Get_2_type > 0)
			{
				this.RewardTypes.Add((QuestItemGetKind)mstObj.Get_2_type);
				this.RewardCount.Add(mstObj.Get_2_count);
			}
		}

		private QuestProgressKinds setProgress()
		{
			if (this.State == QuestState.COMPLETE)
			{
				return QuestProgressKinds.NOT_DISP;
			}
			Mst_questcount mst_questcount = null;
			if (!Mst_DataManager.Instance.Mst_questcount.TryGetValue(this.No, ref mst_questcount))
			{
				return QuestProgressKinds.NOT_DISP;
			}
			double num = 0.0;
			if (mst_questcount.Counter_id.get_Count() != mst_questcount.Clear_num.get_Count())
			{
				double num2 = 0.0;
				Mem_questcount mem_questcount = null;
				using (HashSet<int>.Enumerator enumerator = mst_questcount.Counter_id.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						int current = enumerator.get_Current();
						if (Comm_UserDatas.Instance.User_questcount.TryGetValue(current, ref mem_questcount))
						{
							num2 += (double)mem_questcount.Value;
						}
					}
				}
				double num3 = (double)Enumerable.First<int>(mst_questcount.Clear_num.get_Values());
				num = num2 / num3;
				return this.getprogress(num);
			}
			using (HashSet<int>.Enumerator enumerator2 = mst_questcount.Counter_id.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					int current2 = enumerator2.get_Current();
					double num4 = 0.0;
					Mem_questcount mem_questcount2 = null;
					if (Comm_UserDatas.Instance.User_questcount.TryGetValue(current2, ref mem_questcount2))
					{
						num4 = (double)mem_questcount2.Value;
					}
					double num5 = (double)mst_questcount.Clear_num.get_Item(current2);
					double num6 = num4 / num5;
					if (num6 >= 1.0)
					{
						num6 = 1.0;
					}
					num += num6;
				}
			}
			num /= (double)mst_questcount.Counter_id.get_Count();
			return this.getprogress(num);
		}

		private QuestProgressKinds getprogress(double progressProb)
		{
			if (progressProb >= 0.8)
			{
				return QuestProgressKinds.MORE_THAN_80;
			}
			if (progressProb >= 0.5)
			{
				return QuestProgressKinds.MORE_THAN_50;
			}
			return QuestProgressKinds.NOT_DISP;
		}
	}
}
