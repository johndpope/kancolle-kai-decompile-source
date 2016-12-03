using Common.Enum;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_material", Namespace = "")]
	public class Mem_material : Model_Base
	{
		[DataMember]
		private enumMaterialCategory _rid;

		[DataMember]
		private int _value;

		private static string _tableName = "mem_material";

		public enumMaterialCategory Rid
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
				return Mem_material._tableName;
			}
		}

		public Mem_material()
		{
		}

		public Mem_material(enumMaterialCategory category, int value)
		{
			this.Rid = category;
			this.Value = value;
		}

		public int Add_Material(int num)
		{
			int materialLimit = Mst_DataManager.Instance.Mst_item_limit.get_Item(1).GetMaterialLimit(Mst_DataManager.Instance.Mst_item_limit, this.Rid);
			if (materialLimit > this.Value + num)
			{
				this.Value += num;
			}
			else
			{
				this.Value = materialLimit;
			}
			return this.Value;
		}

		public int Sub_Material(int num)
		{
			if (this.Value - num >= 0)
			{
				this.Value -= num;
			}
			return this.Value;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = (enumMaterialCategory)((int)Enum.Parse(typeof(enumMaterialCategory), element.Element("_rid").get_Value()));
			this.Value = int.Parse(element.Element("_value").get_Value());
		}
	}
}
