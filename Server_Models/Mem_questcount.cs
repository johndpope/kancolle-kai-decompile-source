using Server_Common;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_questcount", Namespace = "")]
	public class Mem_questcount : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _value;

		private static string _tableName = "mem_questcount";

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

		public int Value
		{
			get
			{
				return this._value;
			}
			private set
			{
				this._value = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_questcount._tableName;
			}
		}

		public Mem_questcount()
		{
		}

		public Mem_questcount(int quest_id, int value)
		{
			this.Rid = quest_id;
			this.Value = value;
		}

		public void AddCount(int addValue)
		{
			this.Value += addValue;
		}

		public void Reset(bool deleteFlag)
		{
			this.Value = 0;
			if (deleteFlag)
			{
				Comm_UserDatas.Instance.User_questcount.Remove(this.Rid);
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Value = int.Parse(element.Element("_value").get_Value());
		}
	}
}
