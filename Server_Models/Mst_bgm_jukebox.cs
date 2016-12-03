using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_bgm_jukebox : Model_Base
	{
		private int _id;

		private string _name;

		private string remarks;

		private int _bgm_id;

		private int _r_coins;

		private int _bgm_flag;

		private int _loops;

		private static string _tableName = "mst_bgm_jukebox";

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

		public string Remarks
		{
			get
			{
				return this.remarks;
			}
			private set
			{
				this.remarks = value;
			}
		}

		public int Bgm_id
		{
			get
			{
				return this._bgm_id;
			}
			private set
			{
				this._bgm_id = value;
			}
		}

		public int R_coins
		{
			get
			{
				return this._r_coins;
			}
			private set
			{
				this._r_coins = value;
			}
		}

		public int Bgm_flag
		{
			get
			{
				return this._bgm_flag;
			}
			private set
			{
				this._bgm_flag = value;
			}
		}

		public int Loops
		{
			get
			{
				return this._loops;
			}
			private set
			{
				this._loops = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_bgm_jukebox._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			string[] array = element.Element("Jukebox_record").get_Value().Split(new char[]
			{
				','
			});
			this.Id = int.Parse(array[0]);
			this.Name = array[1];
			this.Remarks = array[2];
			this.Bgm_id = int.Parse(array[3]);
			this.R_coins = int.Parse(array[4]);
			this.Bgm_flag = int.Parse(array[5]);
			this.Loops = int.Parse(array[6]);
		}
	}
}
