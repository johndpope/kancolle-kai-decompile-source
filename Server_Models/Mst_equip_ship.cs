using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_equip_ship : Model_Base
	{
		private int _id;

		private List<int> _equip;

		private static string _tableName = "mst_equip_ship";

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

		public List<int> Equip
		{
			get
			{
				return this._equip;
			}
			private set
			{
				this._equip = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_equip_ship._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Ship_id").get_Value());
			this.Equip = Enumerable.ToList<int>(Array.ConvertAll<string, int>(element.Element("Equip_type").get_Value().Split(new char[]
			{
				','
			}), (string x) => int.Parse(x)));
		}
	}
}
