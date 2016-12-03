using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_book", Namespace = "")]
	public class Mem_book : Model_Base
	{
		[DataMember]
		private int _type;

		[DataMember]
		private int _table_id;

		[DataMember]
		private int _flag1;

		[DataMember]
		private int _flag2;

		[DataMember]
		private int _flag3;

		[DataMember]
		private int _flag4;

		[DataMember]
		private int _flag5;

		private static string _tableName = "mem_book";

		public int Type
		{
			get
			{
				return this._type;
			}
			private set
			{
				this._type = value;
			}
		}

		public int Table_id
		{
			get
			{
				return this._table_id;
			}
			private set
			{
				this._table_id = value;
			}
		}

		public int Flag1
		{
			get
			{
				return this._flag1;
			}
			private set
			{
				this._flag1 = value;
			}
		}

		public int Flag2
		{
			get
			{
				return this._flag2;
			}
			private set
			{
				this._flag2 = value;
			}
		}

		public int Flag3
		{
			get
			{
				return this._flag3;
			}
			private set
			{
				this._flag3 = value;
			}
		}

		public int Flag4
		{
			get
			{
				return this._flag4;
			}
			private set
			{
				this._flag4 = value;
			}
		}

		public int Flag5
		{
			get
			{
				return this._flag5;
			}
			private set
			{
				this._flag5 = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_book._tableName;
			}
		}

		public Mem_book()
		{
			this.Flag1 = 1;
			this.Flag2 = 0;
			this.Flag3 = 0;
			this.Flag4 = 0;
			this.Flag5 = 0;
		}

		public Mem_book(int type, int mst_id) : this()
		{
			this.Type = type;
			this.Table_id = mst_id;
		}

		public void UpdateShipBook(bool damage, bool mariage)
		{
			if (this.Type != 1)
			{
				return;
			}
			if (this.Flag2 == 0 && damage)
			{
				this.Flag2 = 1;
			}
			if (this.Flag3 == 0 && mariage)
			{
				this.Flag3 = 1;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Type = int.Parse(element.Element("_type").get_Value());
			this.Table_id = int.Parse(element.Element("_table_id").get_Value());
			this.Flag1 = int.Parse(element.Element("_flag1").get_Value());
			this.Flag2 = int.Parse(element.Element("_flag2").get_Value());
			this.Flag3 = int.Parse(element.Element("_flag3").get_Value());
			this.Flag4 = int.Parse(element.Element("_flag4").get_Value());
			this.Flag5 = int.Parse(element.Element("_flag5").get_Value());
		}
	}
}
