using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_item_shop : Model_Base
	{
		private ushort _cabinet_no;

		private ushort _position_no;

		private ushort _item1_type;

		private int _item1_id;

		private ushort _item2_type;

		private int _item2_id;

		private static string _tableName = "mst_item_shop";

		public ushort Cabinet_no
		{
			get
			{
				return this._cabinet_no;
			}
			private set
			{
				this._cabinet_no = value;
			}
		}

		public ushort Position_no
		{
			get
			{
				return this._position_no;
			}
			private set
			{
				this._position_no = value;
			}
		}

		public ushort Item1_type
		{
			get
			{
				return this._item1_type;
			}
			private set
			{
				this._item1_type = value;
			}
		}

		public int Item1_id
		{
			get
			{
				return this._item1_id;
			}
			private set
			{
				this._item1_id = value;
			}
		}

		public ushort Item2_type
		{
			get
			{
				return this._item2_type;
			}
			private set
			{
				this._item2_type = value;
			}
		}

		public int Item2_id
		{
			get
			{
				return this._item2_id;
			}
			private set
			{
				this._item2_id = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_item_shop._tableName;
			}
		}

		public bool IsChildReference()
		{
			return this.Item2_type != 0;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Cabinet_no = ushort.Parse(element.Element("Cabinet_no").get_Value());
			this.Position_no = ushort.Parse(element.Element("Position_no").get_Value());
			if (element.Element("Item1") != null)
			{
				string value3 = element.Element("Item1").get_Value();
				int[] array = Array.ConvertAll<string, int>(value3.Split(new char[]
				{
					c
				}), (string value) => int.Parse(value));
				this.Item1_type = (ushort)array[0];
				this.Item1_id = array[1];
			}
			if (element.Element("Item2") != null)
			{
				string value2 = element.Element("Item2").get_Value();
				int[] array2 = Array.ConvertAll<string, int>(value2.Split(new char[]
				{
					c
				}), (string value) => int.Parse(value));
				this.Item2_type = (ushort)array2[0];
				this.Item2_id = array2[1];
			}
		}
	}
}
