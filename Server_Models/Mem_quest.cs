using Common.Enum;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_quest", Namespace = "")]
	public class Mem_quest : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _category;

		[DataMember]
		private QuestState _state;

		private static string _tableName = "mem_quest";

		public int Rid
		{
			get
			{
				return this._rid;
			}
			private set
			{
				this._rid = value;
			}
		}

		public int Category
		{
			get
			{
				return this._category;
			}
			private set
			{
				this._category = value;
			}
		}

		public QuestState State
		{
			get
			{
				return this._state;
			}
			private set
			{
				this._state = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_quest._tableName;
			}
		}

		public Mem_quest()
		{
		}

		public Mem_quest(int id, int category, QuestState state)
		{
			this.Rid = id;
			this.Category = category;
			this.State = state;
		}

		public static Dictionary<int, Mem_quest> GetData(List<Mst_quest> mst_quest)
		{
			Dictionary<int, Mem_quest> ret = new Dictionary<int, Mem_quest>();
			mst_quest.ForEach(delegate(Mst_quest x)
			{
				QuestState state = (x.Torigger != 0) ? QuestState.NOT_DISP : QuestState.WAITING_START;
				Mem_quest mem_quest = new Mem_quest(x.Id, x.Category, state);
				ret.Add(mem_quest.Rid, mem_quest);
			});
			return ret;
		}

		public void StateChange(IQuestOperator instance, QuestState state)
		{
			this.State = state;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Category = int.Parse(element.Element("_category").get_Value());
			this.State = (QuestState)((int)Enum.Parse(typeof(QuestState), element.Element("_state").get_Value()));
		}
	}
}
