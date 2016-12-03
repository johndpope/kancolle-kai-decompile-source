using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipget2 : Model_Base
	{
		private int _id;

		private int _type;

		private int _maparea_id;

		private int _mapinfo_no;

		private int _ship_id;

		private static string _tableName = "mst_shipget2";

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

		public int Maparea_id
		{
			get
			{
				return this._maparea_id;
			}
			private set
			{
				this._maparea_id = value;
			}
		}

		public int Mapinfo_no
		{
			get
			{
				return this._mapinfo_no;
			}
			private set
			{
				this._mapinfo_no = value;
			}
		}

		public int Ship_id
		{
			get
			{
				return this._ship_id;
			}
			private set
			{
				this._ship_id = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_shipget2._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Type = int.Parse(element.Element("Type").get_Value());
			this.Maparea_id = int.Parse(element.Element("Maparea_id").get_Value());
			this.Mapinfo_no = int.Parse(element.Element("Mapinfo_no").get_Value());
			this.Ship_id = int.Parse(element.Element("Ship_id").get_Value());
		}
	}
}
