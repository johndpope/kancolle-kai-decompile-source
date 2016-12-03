using Common.Enum;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapcell2 : Model_Base
	{
		private int _id;

		private int _map_no;

		private int _maparea_id;

		private int _mapinfo_no;

		private int _no;

		private int _color_no;

		private string _link_no;

		private enumMapEventType _event_1;

		private enumMapWarType _event_2;

		private int _event_point_1;

		private int _event_point_2;

		private int _table_no1;

		private int _table_no2;

		private int _next_no_1;

		private int _next_no_2;

		private int _next_no_3;

		private int _next_no_4;

		private string _next_rate;

		private string _next_rate_req;

		private int _req_ship_count;

		private string _req_shiptype;

		private int _item_no;

		private int _item_count;

		private static string _tableName = "mst_mapcell2";

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

		public int Map_no
		{
			get
			{
				return this._map_no;
			}
			private set
			{
				this._map_no = value;
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

		public int No
		{
			get
			{
				return this._no;
			}
			private set
			{
				this._no = value;
			}
		}

		public int Color_no
		{
			get
			{
				return this._color_no;
			}
			private set
			{
				this._color_no = value;
			}
		}

		public string Link_no
		{
			get
			{
				return this._link_no;
			}
			private set
			{
				this._link_no = value;
			}
		}

		public enumMapEventType Event_1
		{
			get
			{
				return this._event_1;
			}
			private set
			{
				this._event_1 = value;
			}
		}

		public enumMapWarType Event_2
		{
			get
			{
				return this._event_2;
			}
			private set
			{
				this._event_2 = value;
			}
		}

		public int Event_point_1
		{
			get
			{
				return this._event_point_1;
			}
			private set
			{
				this._event_point_1 = value;
			}
		}

		public int Event_point_2
		{
			get
			{
				return this._event_point_2;
			}
			private set
			{
				this._event_point_2 = value;
			}
		}

		public int Table_no1
		{
			get
			{
				return this._table_no1;
			}
			private set
			{
				this._table_no1 = value;
			}
		}

		public int Table_no2
		{
			get
			{
				return this._table_no2;
			}
			private set
			{
				this._table_no2 = value;
			}
		}

		public int Next_no_1
		{
			get
			{
				return this._next_no_1;
			}
			private set
			{
				this._next_no_1 = value;
			}
		}

		public int Next_no_2
		{
			get
			{
				return this._next_no_2;
			}
			private set
			{
				this._next_no_2 = value;
			}
		}

		public int Next_no_3
		{
			get
			{
				return this._next_no_3;
			}
			private set
			{
				this._next_no_3 = value;
			}
		}

		public int Next_no_4
		{
			get
			{
				return this._next_no_4;
			}
			private set
			{
				this._next_no_4 = value;
			}
		}

		public string Next_rate
		{
			get
			{
				return this._next_rate;
			}
			private set
			{
				this._next_rate = value;
			}
		}

		public string Next_rate_req
		{
			get
			{
				return this._next_rate_req;
			}
			private set
			{
				this._next_rate_req = value;
			}
		}

		public int Req_ship_count
		{
			get
			{
				return this._req_ship_count;
			}
			private set
			{
				this._req_ship_count = value;
			}
		}

		public string Req_shiptype
		{
			get
			{
				return this._req_shiptype;
			}
			private set
			{
				this._req_shiptype = value;
			}
		}

		public int Item_no
		{
			get
			{
				return this._item_no;
			}
			private set
			{
				this._item_no = value;
			}
		}

		public int Item_count
		{
			get
			{
				return this._item_count;
			}
			private set
			{
				this._item_count = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_mapcell2._tableName;
			}
		}

		public List<int> GetLinkNo()
		{
			if (string.IsNullOrEmpty(this.Link_no))
			{
				return new List<int>();
			}
			string[] array = this.Link_no.Split(new char[]
			{
				','
			});
			return new List<int>(Array.ConvertAll<string, int>(array, (string x) => int.Parse(x)));
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Map_no = int.Parse(element.Element("Map_no").get_Value());
			this.Maparea_id = int.Parse(element.Element("Maparea_id").get_Value());
			this.Mapinfo_no = int.Parse(element.Element("Mapinfo_no").get_Value());
			this.No = int.Parse(element.Element("No").get_Value());
			this.Color_no = int.Parse(element.Element("Color_no").get_Value());
			this.Link_no = element.Element("Link_no").get_Value();
			this.Event_1 = (enumMapEventType)int.Parse(element.Element("Event_1").get_Value());
			this.Event_2 = (enumMapWarType)int.Parse(element.Element("Event_2").get_Value());
			this.Event_point_1 = int.Parse(element.Element("Event_point_1").get_Value());
			this.Event_point_2 = int.Parse(element.Element("Event_point_2").get_Value());
			this.Table_no1 = int.Parse(element.Element("Table_no1").get_Value());
			this.Table_no2 = int.Parse(element.Element("Table_no2").get_Value());
			this.Next_no_1 = int.Parse(element.Element("Next_no_1").get_Value());
			this.Next_no_2 = int.Parse(element.Element("Next_no_2").get_Value());
			this.Next_no_3 = int.Parse(element.Element("Next_no_3").get_Value());
			this.Next_no_4 = int.Parse(element.Element("Next_no_4").get_Value());
			this.Next_rate = element.Element("Next_rate").get_Value();
			this.Next_rate_req = element.Element("Next_rate_req").get_Value();
			this.Req_ship_count = int.Parse(element.Element("Req_ship_count").get_Value());
			this.Req_shiptype = element.Element("Req_shiptype").get_Value();
			this.Item_no = int.Parse(element.Element("Item_no").get_Value());
			this.Item_count = int.Parse(element.Element("Item_count").get_Value());
			this.disableEnemyToNonActiveArea();
		}

		public bool IsNext()
		{
			return this.Next_no_1 > 0 || this.Next_no_2 > 0 || this.Next_no_3 > 0 || this.Next_no_4 > 0;
		}

		private void disableEnemyToNonActiveArea()
		{
			if (this.Event_1 != enumMapEventType.War_Normal)
			{
				return;
			}
			HashSet<int> hashSet = new HashSet<int>();
			if (!hashSet.Contains(this.Maparea_id))
			{
				return;
			}
			this.Event_1 = enumMapEventType.Stupid;
			this.Event_2 = enumMapWarType.None;
		}
	}
}
