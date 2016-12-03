using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_radingrate : Model_Base
	{
		private int _maparea_id;

		private int _rading_type;

		private int _air_rate;

		private int _air_karyoku;

		private int _submarine_rate;

		private int _submarine_karyoku;

		private static string _tableName = "mst_radingrate";

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

		public int Air_rate
		{
			get
			{
				return this._air_rate;
			}
			private set
			{
				this._air_rate = value;
			}
		}

		public int Air_karyoku
		{
			get
			{
				return this._air_karyoku;
			}
			private set
			{
				this._air_karyoku = value;
			}
		}

		public int Submarine_rate
		{
			get
			{
				return this._submarine_rate;
			}
			private set
			{
				this._submarine_rate = value;
			}
		}

		public int Submarine_karyoku
		{
			get
			{
				return this._submarine_karyoku;
			}
			private set
			{
				this._submarine_karyoku = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_radingrate._tableName;
			}
		}

		public int[] GetRadingValues(RadingKind kind)
		{
			int[] array = new int[2];
			if (kind == RadingKind.AIR_ATTACK)
			{
				array[0] = this.Air_rate;
				array[1] = this.Air_karyoku;
			}
			else
			{
				array[0] = this.Submarine_rate;
				array[1] = this.Submarine_karyoku;
			}
			return array;
		}

		public int GetRadingRate(RadingKind kind)
		{
			return (kind != RadingKind.AIR_ATTACK) ? this.Submarine_rate : this.Air_rate;
		}

		public int GetRadingPow(RadingKind kind)
		{
			return (kind != RadingKind.AIR_ATTACK) ? this.Submarine_karyoku : this.Air_karyoku;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Maparea_id = int.Parse(element.Element("Maparea_id").get_Value());
			this.Rading_type = int.Parse(element.Element("Rading_type").get_Value());
			string value3 = element.Element("Air_value").get_Value();
			int[] array = Array.ConvertAll<string, int>(value3.Split(new char[]
			{
				c
			}), (string value) => int.Parse(value));
			this.Air_rate = array[0];
			this.Air_karyoku = array[1];
			string value2 = element.Element("Submarin_value").get_Value();
			array = Array.ConvertAll<string, int>(value2.Split(new char[]
			{
				c
			}), (string value) => int.Parse(value));
			this.Submarine_rate = array[0];
			this.Submarine_karyoku = array[1];
		}
	}
}
