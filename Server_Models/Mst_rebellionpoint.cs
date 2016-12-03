using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_rebellionpoint : Model_Base
	{
		private int _id;

		private int _turn_from;

		private int _turn_to;

		private Dictionary<int, int> _area_value;

		private static string _tableName = "mst_rebellionpoint";

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

		public int Turn_from
		{
			get
			{
				return this._turn_from;
			}
			private set
			{
				this._turn_from = value;
			}
		}

		public int Turn_to
		{
			get
			{
				return this._turn_to;
			}
			private set
			{
				this._turn_to = value;
			}
		}

		public Dictionary<int, int> Area_value
		{
			get
			{
				return this._area_value;
			}
			private set
			{
				this._area_value = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_rebellionpoint._tableName;
			}
		}

		public Mst_rebellionpoint()
		{
			this.Area_value = new Dictionary<int, int>();
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Turn_from = int.Parse(element.Element("Turn_from").get_Value());
			this.Turn_to = int.Parse(element.Element("Turn_to").get_Value());
			string[] array = element.Element("Area_value").get_Value().Split(new char[]
			{
				c
			});
			int[] array2 = Array.ConvertAll<string, int>(array, (string x) => int.Parse(x));
			for (int i = 0; i < array2.Length; i++)
			{
				this.Area_value.Add(i + 1, array2[i]);
			}
		}
	}
}
