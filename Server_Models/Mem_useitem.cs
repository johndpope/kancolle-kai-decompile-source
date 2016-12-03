using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_useitem", Namespace = "")]
	public class Mem_useitem : Model_Base
	{
		public const int DefItemMaxNum = 3000;

		[DataMember]
		private int _rid;

		[DataMember]
		private int _value;

		private static string _tableName = "mem_useitem";

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
				return Mem_useitem._tableName;
			}
		}

		public Mem_useitem()
		{
		}

		public Mem_useitem(int rid, int value)
		{
			this.Rid = rid;
			this.Value = value;
		}

		public int Add_UseItem(int num)
		{
			this.Value += num;
			if (this.Value > 3000)
			{
				this.Value = 3000;
			}
			return this.Value;
		}

		public int Sub_UseItem(int num)
		{
			this.Value -= num;
			if (this.Value < 0)
			{
				this.Value = 0;
			}
			return this.Value;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Value = int.Parse(element.Element("_value").get_Value());
		}
	}
}
