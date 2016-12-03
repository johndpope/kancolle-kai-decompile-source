using Common.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_item_limit : Model_Base
	{
		private int _id;

		private int _material_id;

		private int _useitem_id;

		private int _slotitem_id;

		private int _max_items;

		private static string _tableName = "mst_item_limit";

		public int Id
		{
			get
			{
				return this._id;
			}
			private set
			{
				this._id = value;
			}
		}

		public int Material_id
		{
			get
			{
				return this._material_id;
			}
			private set
			{
				this._material_id = value;
			}
		}

		public int Useitem_id
		{
			get
			{
				return this._useitem_id;
			}
			private set
			{
				this._useitem_id = value;
			}
		}

		public int Slotitem_id
		{
			get
			{
				return this._slotitem_id;
			}
			private set
			{
				this._slotitem_id = value;
			}
		}

		public int Max_items
		{
			get
			{
				return this._max_items;
			}
			private set
			{
				this._max_items = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_item_limit._tableName;
			}
		}

		public int GetMaterialLimit(Dictionary<int, Mst_item_limit> mst_data, enumMaterialCategory category)
		{
			Mst_item_limit mst_item_limit = Enumerable.FirstOrDefault<Mst_item_limit>(mst_data.get_Values(), (Mst_item_limit x) => x.Material_id == (int)category);
			if (mst_item_limit == null)
			{
				return 0;
			}
			return mst_item_limit.Max_items;
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Material_id = int.Parse(element.Element("Material_id").get_Value());
			this.Useitem_id = int.Parse(element.Element("Useitem_id").get_Value());
			this.Slotitem_id = int.Parse(element.Element("Slotitem_id").get_Value());
			this.Max_items = int.Parse(element.Element("Max_items").get_Value());
		}
	}
}
