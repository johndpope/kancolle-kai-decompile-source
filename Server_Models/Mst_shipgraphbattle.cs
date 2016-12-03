using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipgraphbattle : Model_Base
	{
		private int _id;

		private int _foot_x;

		private int _foot_y;

		private int _foot_d_x;

		private int _foot_d_y;

		private int _pog_x;

		private int _pog_y;

		private int _pog_d_x;

		private int _pog_d_y;

		private int _pog_sp_x;

		private int _pog_sp_y;

		private int _pog_sp_d_x;

		private int _pog_sp_d_y;

		private int _pog_sp_ensyu_x;

		private int _pog_sp_ensyu_y;

		private int _pog_sp_ensyu_d_x;

		private int _pog_sp_ensyu_d_y;

		private int _cutin_x;

		private int _cutin_y;

		private int _cutin_d_x;

		private int _cutin_d_y;

		private int _cutin_sp1_x;

		private int _cutin_sp1_y;

		private int _cutin_sp1_d_x;

		private int _cutin_sp1_d_y;

		private double _scale_mag;

		private static string _tableName = "mst_shipgraphbattle";

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

		public int Foot_x
		{
			get
			{
				return this._foot_x;
			}
			private set
			{
				this._foot_x = value;
			}
		}

		public int Foot_y
		{
			get
			{
				return this._foot_y;
			}
			private set
			{
				this._foot_y = value;
			}
		}

		public int Foot_d_x
		{
			get
			{
				return this._foot_d_x;
			}
			private set
			{
				this._foot_d_x = value;
			}
		}

		public int Foot_d_y
		{
			get
			{
				return this._foot_d_y;
			}
			private set
			{
				this._foot_d_y = value;
			}
		}

		public int Pog_x
		{
			get
			{
				return this._pog_x;
			}
			private set
			{
				this._pog_x = value;
			}
		}

		public int Pog_y
		{
			get
			{
				return this._pog_y;
			}
			private set
			{
				this._pog_y = value;
			}
		}

		public int Pog_d_x
		{
			get
			{
				return this._pog_d_x;
			}
			private set
			{
				this._pog_d_x = value;
			}
		}

		public int Pog_d_y
		{
			get
			{
				return this._pog_d_y;
			}
			private set
			{
				this._pog_d_y = value;
			}
		}

		public int Pog_sp_x
		{
			get
			{
				return this._pog_sp_x;
			}
			private set
			{
				this._pog_sp_x = value;
			}
		}

		public int Pog_sp_y
		{
			get
			{
				return this._pog_sp_y;
			}
			private set
			{
				this._pog_sp_y = value;
			}
		}

		public int Pog_sp_d_x
		{
			get
			{
				return this._pog_sp_d_x;
			}
			private set
			{
				this._pog_sp_d_x = value;
			}
		}

		public int Pog_sp_d_y
		{
			get
			{
				return this._pog_sp_d_y;
			}
			private set
			{
				this._pog_sp_d_y = value;
			}
		}

		public int Pog_sp_ensyu_x
		{
			get
			{
				return this._pog_sp_ensyu_x;
			}
			private set
			{
				this._pog_sp_ensyu_x = value;
			}
		}

		public int Pog_sp_ensyu_y
		{
			get
			{
				return this._pog_sp_ensyu_y;
			}
			private set
			{
				this._pog_sp_ensyu_y = value;
			}
		}

		public int Pog_sp_ensyu_d_x
		{
			get
			{
				return this._pog_sp_ensyu_d_x;
			}
			private set
			{
				this._pog_sp_ensyu_d_x = value;
			}
		}

		public int Pog_sp_ensyu_d_y
		{
			get
			{
				return this._pog_sp_ensyu_d_y;
			}
			private set
			{
				this._pog_sp_ensyu_d_y = value;
			}
		}

		public int Cutin_x
		{
			get
			{
				return this._cutin_x;
			}
			private set
			{
				this._cutin_x = value;
			}
		}

		public int Cutin_y
		{
			get
			{
				return this._cutin_y;
			}
			private set
			{
				this._cutin_y = value;
			}
		}

		public int Cutin_d_x
		{
			get
			{
				return this._cutin_d_x;
			}
			private set
			{
				this._cutin_d_x = value;
			}
		}

		public int Cutin_d_y
		{
			get
			{
				return this._cutin_d_y;
			}
			private set
			{
				this._cutin_d_y = value;
			}
		}

		public int Cutin_sp1_x
		{
			get
			{
				return this._cutin_sp1_x;
			}
			private set
			{
				this._cutin_sp1_x = value;
			}
		}

		public int Cutin_sp1_y
		{
			get
			{
				return this._cutin_sp1_y;
			}
			private set
			{
				this._cutin_sp1_y = value;
			}
		}

		public int Cutin_sp1_d_x
		{
			get
			{
				return this._cutin_sp1_d_x;
			}
			private set
			{
				this._cutin_sp1_d_x = value;
			}
		}

		public int Cutin_sp1_d_y
		{
			get
			{
				return this._cutin_sp1_d_y;
			}
			private set
			{
				this._cutin_sp1_d_y = value;
			}
		}

		public double Scale_mag
		{
			get
			{
				return this._scale_mag;
			}
			private set
			{
				this._scale_mag = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_shipgraphbattle._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			char c = ',';
			if (element.Element("BattlePos") == null)
			{
				return;
			}
			string[] array = element.Element("BattlePos").get_Value().Split(new char[]
			{
				c
			});
			string text = array[24];
			array[24] = "0";
			int[] array2 = Array.ConvertAll<string, int>(array, (string x) => int.Parse(x));
			this.Foot_x = array2[0];
			this.Foot_y = array2[1];
			this.Foot_d_x = array2[2];
			this.Foot_d_y = array2[3];
			this.Pog_x = array2[4];
			this.Pog_y = array2[5];
			this.Pog_d_x = array2[6];
			this.Pog_d_y = array2[7];
			this.Pog_sp_x = array2[8];
			this.Pog_sp_y = array2[9];
			this.Pog_sp_d_x = array2[10];
			this.Pog_sp_d_y = array2[11];
			this.Pog_sp_ensyu_x = array2[12];
			this.Pog_sp_ensyu_y = array2[13];
			this.Pog_sp_ensyu_d_x = array2[14];
			this.Pog_sp_ensyu_d_y = array2[15];
			this.Cutin_x = array2[16];
			this.Cutin_y = array2[17];
			this.Cutin_d_x = array2[18];
			this.Cutin_d_y = array2[19];
			this.Cutin_sp1_x = array2[20];
			this.Cutin_sp1_y = array2[21];
			this.Cutin_sp1_d_x = array2[22];
			this.Cutin_sp1_d_y = array2[23];
			if (text != string.Empty)
			{
				this.Scale_mag = double.Parse(text);
			}
		}
	}
}
