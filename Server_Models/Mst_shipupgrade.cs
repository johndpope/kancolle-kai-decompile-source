using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipupgrade : Model_Base
	{
		private int _id;

		private int _original_ship_id;

		private int _upgrade_type;

		private int _upgrade_level;

		private int _drawing_count;

		private static string _tableName = "mst_shipupgrade";

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

		public int Original_ship_id
		{
			get
			{
				return this._original_ship_id;
			}
			private set
			{
				this._original_ship_id = value;
			}
		}

		public int Upgrade_type
		{
			get
			{
				return this._upgrade_type;
			}
			private set
			{
				this._upgrade_type = value;
			}
		}

		public int Upgrade_level
		{
			get
			{
				return this._upgrade_level;
			}
			private set
			{
				this._upgrade_level = value;
			}
		}

		public int Drawing_count
		{
			get
			{
				return this._drawing_count;
			}
			private set
			{
				this._drawing_count = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_shipupgrade._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Original_ship_id = int.Parse(element.Element("Original_ship_id").get_Value());
			this.Upgrade_type = int.Parse(element.Element("Upgrade_type").get_Value());
			this.Upgrade_level = int.Parse(element.Element("Upgrade_level").get_Value());
			this.Drawing_count = int.Parse(element.Element("Drawing_count").get_Value());
		}
	}
}
