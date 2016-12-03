using Common.Enum;
using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_mission2 : Model_Base
	{
		private int _id;

		private int _maparea_id;

		private string _name;

		private string _details;

		private MissionType _mission_type;

		private int _time;

		private int _rp_sub;

		private int _difficulty;

		private double _use_fuel;

		private double _use_bull;

		private string _required_ids;

		private int _win_exp_member;

		private int _win_exp_ship;

		private int _win_mat1;

		private int _win_mat2;

		private int _win_mat3;

		private int _win_mat4;

		private int _win_item1;

		private int _win_item1_num;

		private int _win_item2;

		private int _win_item2_num;

		private int _win_spoint1;

		private int _win_spoint2;

		private int _level;

		private int _flagship_level;

		private int _stype_num1;

		private int _stype_num2;

		private int _stype_num3;

		private int _stype_num4;

		private int _stype_num5;

		private int _stype_num6;

		private int _stype_num7;

		private int _stype_num8;

		private int _stype_num9;

		private int _deck_num;

		private int _drum_ship_num;

		private int _drum_total_num1;

		private int _drum_total_num2;

		private int _flagship_stype1;

		private int _flagship_stype2;

		private int _flagship_level_check_type;

		private int _tanker_num;

		private int _tanker_num_max;

		private static string _tableName = "mst_mission2";

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

		public string Name
		{
			get
			{
				return this._name;
			}
			private set
			{
				this._name = value;
			}
		}

		public string Details
		{
			get
			{
				return this._details;
			}
			private set
			{
				this._details = value;
			}
		}

		public MissionType Mission_type
		{
			get
			{
				return this._mission_type;
			}
			private set
			{
				this._mission_type = value;
			}
		}

		public int Time
		{
			get
			{
				return this._time;
			}
			private set
			{
				this._time = value;
			}
		}

		public int Rp_sub
		{
			get
			{
				return this._rp_sub;
			}
			private set
			{
				this._rp_sub = value;
			}
		}

		public int Difficulty
		{
			get
			{
				return this._difficulty;
			}
			private set
			{
				this._difficulty = value;
			}
		}

		public double Use_fuel
		{
			get
			{
				return this._use_fuel;
			}
			private set
			{
				this._use_fuel = value;
			}
		}

		public double Use_bull
		{
			get
			{
				return this._use_bull;
			}
			private set
			{
				this._use_bull = value;
			}
		}

		public string Required_ids
		{
			get
			{
				return this._required_ids;
			}
			private set
			{
				this._required_ids = value;
			}
		}

		public int Win_exp_member
		{
			get
			{
				return this._win_exp_member;
			}
			private set
			{
				this._win_exp_member = value;
			}
		}

		public int Win_exp_ship
		{
			get
			{
				return this._win_exp_ship;
			}
			private set
			{
				this._win_exp_ship = value;
			}
		}

		public int Win_mat1
		{
			get
			{
				return this._win_mat1;
			}
			private set
			{
				this._win_mat1 = value;
			}
		}

		public int Win_mat2
		{
			get
			{
				return this._win_mat2;
			}
			private set
			{
				this._win_mat2 = value;
			}
		}

		public int Win_mat3
		{
			get
			{
				return this._win_mat3;
			}
			private set
			{
				this._win_mat3 = value;
			}
		}

		public int Win_mat4
		{
			get
			{
				return this._win_mat4;
			}
			private set
			{
				this._win_mat4 = value;
			}
		}

		public int Win_item1
		{
			get
			{
				return this._win_item1;
			}
			private set
			{
				this._win_item1 = value;
			}
		}

		public int Win_item1_num
		{
			get
			{
				return this._win_item1_num;
			}
			private set
			{
				this._win_item1_num = value;
			}
		}

		public int Win_item2
		{
			get
			{
				return this._win_item2;
			}
			private set
			{
				this._win_item2 = value;
			}
		}

		public int Win_item2_num
		{
			get
			{
				return this._win_item2_num;
			}
			private set
			{
				this._win_item2_num = value;
			}
		}

		public int Win_spoint1
		{
			get
			{
				return this._win_spoint1;
			}
			private set
			{
				this._win_spoint1 = value;
			}
		}

		public int Win_spoint2
		{
			get
			{
				return this._win_spoint2;
			}
			private set
			{
				this._win_spoint2 = value;
			}
		}

		public int Level
		{
			get
			{
				return this._level;
			}
			private set
			{
				this._level = value;
			}
		}

		public int Flagship_level
		{
			get
			{
				return this._flagship_level;
			}
			private set
			{
				this._flagship_level = value;
			}
		}

		public int Stype_num1
		{
			get
			{
				return this._stype_num1;
			}
			private set
			{
				this._stype_num1 = value;
			}
		}

		public int Stype_num2
		{
			get
			{
				return this._stype_num2;
			}
			private set
			{
				this._stype_num2 = value;
			}
		}

		public int Stype_num3
		{
			get
			{
				return this._stype_num3;
			}
			private set
			{
				this._stype_num3 = value;
			}
		}

		public int Stype_num4
		{
			get
			{
				return this._stype_num4;
			}
			private set
			{
				this._stype_num4 = value;
			}
		}

		public int Stype_num5
		{
			get
			{
				return this._stype_num5;
			}
			private set
			{
				this._stype_num5 = value;
			}
		}

		public int Stype_num6
		{
			get
			{
				return this._stype_num6;
			}
			private set
			{
				this._stype_num6 = value;
			}
		}

		public int Stype_num7
		{
			get
			{
				return this._stype_num7;
			}
			private set
			{
				this._stype_num7 = value;
			}
		}

		public int Stype_num8
		{
			get
			{
				return this._stype_num8;
			}
			private set
			{
				this._stype_num8 = value;
			}
		}

		public int Stype_num9
		{
			get
			{
				return this._stype_num9;
			}
			private set
			{
				this._stype_num9 = value;
			}
		}

		public int Deck_num
		{
			get
			{
				return this._deck_num;
			}
			private set
			{
				this._deck_num = value;
			}
		}

		public int Drum_ship_num
		{
			get
			{
				return this._drum_ship_num;
			}
			private set
			{
				this._drum_ship_num = value;
			}
		}

		public int Drum_total_num1
		{
			get
			{
				return this._drum_total_num1;
			}
			private set
			{
				this._drum_total_num1 = value;
			}
		}

		public int Drum_total_num2
		{
			get
			{
				return this._drum_total_num2;
			}
			private set
			{
				this._drum_total_num2 = value;
			}
		}

		public int Flagship_stype1
		{
			get
			{
				return this._flagship_stype1;
			}
			private set
			{
				this._flagship_stype1 = value;
			}
		}

		public int Flagship_stype2
		{
			get
			{
				return this._flagship_stype2;
			}
			private set
			{
				this._flagship_stype2 = value;
			}
		}

		public int Flagship_level_check_type
		{
			get
			{
				return this._flagship_level_check_type;
			}
			private set
			{
				this._flagship_level_check_type = value;
			}
		}

		public int Tanker_num
		{
			get
			{
				return this._tanker_num;
			}
			private set
			{
				this._tanker_num = value;
			}
		}

		public int Tanker_num_max
		{
			get
			{
				return this._tanker_num_max;
			}
			private set
			{
				this._tanker_num_max = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_mission2._tableName;
			}
		}

		public bool IsGreatSuccessCondition()
		{
			return this.Drum_total_num2 > 0 || this.Flagship_stype2 > 0 || this.Flagship_level_check_type == 2;
		}

		public bool IsSupportMission()
		{
			return this.Mission_type == MissionType.SupportForward || this.Mission_type == MissionType.SupportBoss;
		}

		protected override void setProperty(XElement element)
		{
			char c = ',';
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Maparea_id = int.Parse(element.Element("Maparea_id").get_Value());
			this.Name = element.Element("Name").get_Value();
			this.Details = element.Element("Details").get_Value();
			this.Mission_type = (MissionType)int.Parse(element.Element("Mission_type").get_Value());
			this.Time = int.Parse(element.Element("Time").get_Value());
			this.Rp_sub = int.Parse(element.Element("Rp_sub").get_Value());
			this.Difficulty = int.Parse(element.Element("Difficulty").get_Value());
			double[] array = Array.ConvertAll<string, double>(element.Element("Use_mat").get_Value().Split(new char[]
			{
				c
			}), (string x) => double.Parse(x));
			this.Use_fuel = array[0];
			this.Use_bull = array[1];
			this.Required_ids = element.Element("Required_ids").get_Value();
			int[] array2 = Array.ConvertAll<string, int>(element.Element("Win_exp").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Win_exp_member = array2[0];
			this.Win_exp_ship = array2[1];
			int[] array3 = Array.ConvertAll<string, int>(element.Element("Win_mat").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Win_mat1 = array3[0];
			this.Win_mat2 = array3[1];
			this.Win_mat3 = array3[2];
			this.Win_mat4 = array3[3];
			int[] array4 = Array.ConvertAll<string, int>(element.Element("Win_item1").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Win_item1 = array4[0];
			this.Win_item1_num = array4[1];
			int[] array5 = Array.ConvertAll<string, int>(element.Element("Win_item2").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Win_item2 = array5[0];
			this.Win_item2_num = array5[1];
			int[] array6 = Array.ConvertAll<string, int>(element.Element("Win_spoint").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Win_spoint1 = array6[0];
			this.Win_spoint2 = array6[1];
			this.Level = int.Parse(element.Element("Level").get_Value());
			this.Flagship_level_check_type = int.Parse(element.Element("Flagship_level_check_type").get_Value());
			this.Flagship_level = int.Parse(element.Element("Flagship_level").get_Value());
			int[] array7 = Array.ConvertAll<string, int>(element.Element("Stype_num").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Stype_num1 = array7[0];
			this.Stype_num2 = array7[1];
			this.Stype_num3 = array7[2];
			this.Stype_num4 = array7[3];
			this.Stype_num5 = array7[4];
			this.Stype_num6 = array7[5];
			this.Stype_num7 = array7[6];
			this.Stype_num8 = array7[7];
			this.Stype_num9 = array7[8];
			this.Deck_num = int.Parse(element.Element("Deck_num").get_Value());
			int[] array8 = Array.ConvertAll<string, int>(element.Element("Drum_num").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Drum_ship_num = array8[0];
			this.Drum_total_num1 = array8[1];
			this.Drum_total_num2 = array8[2];
			int[] array9 = Array.ConvertAll<string, int>(element.Element("Flagship_stype").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Flagship_stype1 = array9[0];
			this.Flagship_stype2 = array9[1];
			int[] array10 = Array.ConvertAll<string, int>(element.Element("Tanker_num").get_Value().Split(new char[]
			{
				c
			}), (string x) => int.Parse(x));
			this.Tanker_num = array10[0];
			this.Tanker_num_max = 16;
		}
	}
}
