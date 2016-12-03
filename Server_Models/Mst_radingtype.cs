using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_radingtype : Model_Base
	{
		private int _difficult;

		private int _turn_from;

		private int _turn_to;

		private int _rading_type;

		private int _rading_rate;

		private static string _tableName = "mst_radingtype";

		public int Difficult
		{
			get
			{
				return this._difficult;
			}
			private set
			{
				this._difficult = value;
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

		public int Rading_type
		{
			get
			{
				return this._rading_type;
			}
			private set
			{
				this._rading_type = value;
			}
		}

		public int Rading_rate
		{
			get
			{
				return this._rading_rate;
			}
			private set
			{
				this._rading_rate = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_radingtype._tableName;
			}
		}

		public static Mst_radingtype GetRadingRecord(List<Mst_radingtype> types, int nowTurn)
		{
			using (List<Mst_radingtype>.Enumerator enumerator = types.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_radingtype current = enumerator.get_Current();
					if (current.Turn_from <= nowTurn)
					{
						return current;
					}
				}
			}
			return null;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Difficult = int.Parse(element.Element("Difficult").get_Value());
			string value2 = element.Element("Turn_value").get_Value();
			int[] array = Array.ConvertAll<string, int>(value2.Split(new char[]
			{
				c
			}), (string value) => int.Parse(value));
			this.Turn_from = array[0];
			this.Turn_to = array[1];
			this.Rading_type = int.Parse(element.Element("Rading_type").get_Value());
			this.Rading_rate = int.Parse(element.Element("Rading_rate").get_Value());
		}
	}
}
