using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapcellincentive : Mst_mapincentiveBase
	{
		private int _mapcell_id;

		private int _event_id;

		private int _success_level;

		private Dictionary<int, int> _req_items;

		private static string _tableName = "mst_mapcellincentive";

		public int Mapcell_id
		{
			get
			{
				return this._mapcell_id;
			}
			private set
			{
				this._mapcell_id = value;
			}
		}

		public int Event_id
		{
			get
			{
				return this._event_id;
			}
			private set
			{
				this._event_id = value;
			}
		}

		public int Success_level
		{
			get
			{
				return this._success_level;
			}
			private set
			{
				this._success_level = value;
			}
		}

		public Dictionary<int, int> Req_items
		{
			get
			{
				return this._req_items;
			}
			private set
			{
				this._req_items = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_mapcellincentive._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			base.setProperty(element);
			this.Mapcell_id = int.Parse(element.Element("Mapcell_id").get_Value());
			this.Event_id = int.Parse(element.Element("Event_id").get_Value());
			this.Success_level = int.Parse(element.Element("Success_level").get_Value());
			if (element.Element("Req_item") == null)
			{
				return;
			}
			this.Req_items = new Dictionary<int, int>();
			string[] array = element.Element("Req_item").get_Value().Split(new char[]
			{
				c
			});
			string[] array2 = element.Element("Req_item_point").get_Value().Split(new char[]
			{
				c
			});
			for (int i = 0; i < array.Length; i++)
			{
				int num = int.Parse(array[i]);
				int num2 = int.Parse(array2[i]);
				this.Req_items.Add(num, num2);
			}
		}

		protected override void setIncentiveItem(XElement element)
		{
			base.setIncentiveItem(element);
		}
	}
}
