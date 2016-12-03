using Common.Enum;
using System;
using System.Collections.Generic;

namespace Server_Controllers
{
	public class QuestItemGetFmt
	{
		public QuestItemGetKind Category;

		public int Id;

		public int Count;

		public int FromId;

		public bool IsQuestExtend;

		public bool IsUseCrewItem;

		public List<int> createIds()
		{
			List<int> list = new List<int>();
			for (int i = 0; i < this.Count; i++)
			{
				list.Add(this.Id);
			}
			return list;
		}
	}
}
