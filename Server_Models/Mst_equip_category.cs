using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_equip_category : Model_Base
	{
		private int _id;

		private SlotitemCategory _slotitem_type;

		private static string _tableName = "mst_equip_category";

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

		public SlotitemCategory Slotitem_type
		{
			get
			{
				return this._slotitem_type;
			}
			private set
			{
				this._slotitem_type = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_equip_category._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Slotitem_type = (SlotitemCategory)((int)Enum.Parse(typeof(SlotitemCategory), element.Element("Slotitem_type").get_Value()));
		}
	}
}
