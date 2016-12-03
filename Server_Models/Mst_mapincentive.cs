using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mapincentive : Mst_mapincentiveBase
	{
		private int _map_cleared;

		private static string _tableName = "mst_mapincentive";

		public int Map_cleared
		{
			get
			{
				return this._map_cleared;
			}
			private set
			{
				this._map_cleared = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_mapincentive._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			base.setProperty(element);
			this.Map_cleared = int.Parse(element.Element("Map_cleared").get_Value());
		}

		protected override void setIncentiveItem(XElement element)
		{
			base.setIncentiveItem(element);
		}
	}
}
