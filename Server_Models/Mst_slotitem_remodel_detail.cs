using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_slotitem_remodel_detail : Model_Base
	{
		private int _id;

		private int _level_from;

		private int _level_to;

		private int _change_flag;

		private int _success_rate1;

		private int _success_rate2;

		private int _req_material5_1;

		private int _req_material6_1;

		private int _req_material5_2;

		private int _req_material6_2;

		private int _req_slotitem_id;

		private int _req_slotitems;

		private int _new_slotitem_id;

		private int _new_slotitem_level;

		private static string _tableName = "mst_slotitem_remodel_detail";

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

		public int Level_from
		{
			get
			{
				return this._level_from;
			}
			private set
			{
				this._level_from = value;
			}
		}

		public int Level_to
		{
			get
			{
				return this._level_to;
			}
			private set
			{
				this._level_to = value;
			}
		}

		public int Change_flag
		{
			get
			{
				return this._change_flag;
			}
			private set
			{
				this._change_flag = value;
			}
		}

		public int Success_rate1
		{
			get
			{
				return this._success_rate1;
			}
			private set
			{
				this._success_rate1 = value;
			}
		}

		public int Success_rate2
		{
			get
			{
				return this._success_rate2;
			}
			private set
			{
				this._success_rate2 = value;
			}
		}

		public int Req_material5_1
		{
			get
			{
				return this._req_material5_1;
			}
			private set
			{
				this._req_material5_1 = value;
			}
		}

		public int Req_material6_1
		{
			get
			{
				return this._req_material6_1;
			}
			private set
			{
				this._req_material6_1 = value;
			}
		}

		public int Req_material5_2
		{
			get
			{
				return this._req_material5_2;
			}
			private set
			{
				this._req_material5_2 = value;
			}
		}

		public int Req_material6_2
		{
			get
			{
				return this._req_material6_2;
			}
			private set
			{
				this._req_material6_2 = value;
			}
		}

		public int Req_slotitem_id
		{
			get
			{
				return this._req_slotitem_id;
			}
			private set
			{
				this._req_slotitem_id = value;
			}
		}

		public int Req_slotitems
		{
			get
			{
				return this._req_slotitems;
			}
			private set
			{
				this._req_slotitems = value;
			}
		}

		public int New_slotitem_id
		{
			get
			{
				return this._new_slotitem_id;
			}
			private set
			{
				this._new_slotitem_id = value;
			}
		}

		public int New_slotitem_level
		{
			get
			{
				return this._new_slotitem_level;
			}
			private set
			{
				this._new_slotitem_level = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_slotitem_remodel_detail._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			char c = ',';
			string[] array = element.Element("RemodelData").get_Value().Split(new char[]
			{
				c
			});
			this.Level_from = int.Parse(array[0]);
			this.Level_to = int.Parse(array[1]);
			this.Change_flag = int.Parse(array[2]);
			this.Success_rate1 = int.Parse(array[3]);
			this.Success_rate2 = int.Parse(array[4]);
			this.Req_material5_1 = int.Parse(array[5]);
			this.Req_material6_1 = int.Parse(array[6]);
			this.Req_material5_2 = int.Parse(array[7]);
			this.Req_material6_2 = int.Parse(array[8]);
			this.Req_slotitem_id = int.Parse(array[9]);
			this.Req_slotitems = int.Parse(array[10]);
			this.New_slotitem_id = int.Parse(array[11]);
			this.New_slotitem_level = int.Parse(array[12]);
		}
	}
}
