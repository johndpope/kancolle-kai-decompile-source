using System;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mst_shipgraph : Model_Base
	{
		private int _id;

		private int _sortno;

		private int _boko_n_x;

		private int _boko_n_y;

		private int _boko_d_x;

		private int _boko_d_y;

		private int _face_n_x;

		private int _face_n_y;

		private int _face_d_x;

		private int _face_d_y;

		private int _slotitem_category_n_x;

		private int _slotitem_category_n_y;

		private int _slotitem_category_d_x;

		private int _slotitem_category_d_y;

		private int _ship_display_center_n_x;

		private int _ship_display_center_n_y;

		private int _ship_display_center_d_x;

		private int _ship_display_center_d_y;

		private int _weda_x;

		private int _weda_y;

		private int _wedb_x;

		private int _wedb_y;

		private int _L2dSize_W;

		private int _L2dSize_H;

		private int _L2dBias_X;

		private int _L2dBias_Y;

		private static string _tableName = "mst_shipgraph";

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

		public int Sortno
		{
			get
			{
				return this._sortno;
			}
			private set
			{
				this._sortno = value;
			}
		}

		public int Boko_n_x
		{
			get
			{
				return this._boko_n_x;
			}
			private set
			{
				this._boko_n_x = value;
			}
		}

		public int Boko_n_y
		{
			get
			{
				return this._boko_n_y;
			}
			private set
			{
				this._boko_n_y = value;
			}
		}

		public int Boko_d_x
		{
			get
			{
				return this._boko_d_x;
			}
			private set
			{
				this._boko_d_x = value;
			}
		}

		public int Boko_d_y
		{
			get
			{
				return this._boko_d_y;
			}
			private set
			{
				this._boko_d_y = value;
			}
		}

		public int Face_n_x
		{
			get
			{
				return this._face_n_x;
			}
			private set
			{
				this._face_n_x = value;
			}
		}

		public int Face_n_y
		{
			get
			{
				return this._face_n_y;
			}
			private set
			{
				this._face_n_y = value;
			}
		}

		public int Face_d_x
		{
			get
			{
				return this._face_d_x;
			}
			private set
			{
				this._face_d_x = value;
			}
		}

		public int Face_d_y
		{
			get
			{
				return this._face_d_y;
			}
			private set
			{
				this._face_d_y = value;
			}
		}

		public int Slotitem_category_n_x
		{
			get
			{
				return this._slotitem_category_n_x;
			}
			private set
			{
				this._slotitem_category_n_x = value;
			}
		}

		public int Slotitem_category_n_y
		{
			get
			{
				return this._slotitem_category_n_y;
			}
			private set
			{
				this._slotitem_category_n_y = value;
			}
		}

		public int Slotitem_category_d_x
		{
			get
			{
				return this._slotitem_category_d_x;
			}
			private set
			{
				this._slotitem_category_d_x = value;
			}
		}

		public int Slotitem_category_d_y
		{
			get
			{
				return this._slotitem_category_d_y;
			}
			private set
			{
				this._slotitem_category_d_y = value;
			}
		}

		public int Ship_display_center_n_x
		{
			get
			{
				return this._ship_display_center_n_x;
			}
			private set
			{
				this._ship_display_center_n_x = value;
			}
		}

		public int Ship_display_center_n_y
		{
			get
			{
				return this._ship_display_center_n_y;
			}
			private set
			{
				this._ship_display_center_n_y = value;
			}
		}

		public int Ship_display_center_d_x
		{
			get
			{
				return this._ship_display_center_d_x;
			}
			private set
			{
				this._ship_display_center_d_x = value;
			}
		}

		public int Ship_display_center_d_y
		{
			get
			{
				return this._ship_display_center_d_y;
			}
			private set
			{
				this._ship_display_center_d_y = value;
			}
		}

		public int Weda_x
		{
			get
			{
				return this._weda_x;
			}
			private set
			{
				this._weda_x = value;
			}
		}

		public int Weda_y
		{
			get
			{
				return this._weda_y;
			}
			private set
			{
				this._weda_y = value;
			}
		}

		public int Wedb_x
		{
			get
			{
				return this._wedb_x;
			}
			private set
			{
				this._wedb_x = value;
			}
		}

		public int Wedb_y
		{
			get
			{
				return this._wedb_y;
			}
			private set
			{
				this._wedb_y = value;
			}
		}

		public int L2dSize_W
		{
			get
			{
				return this._L2dSize_W;
			}
			private set
			{
				this._L2dSize_W = value;
			}
		}

		public int L2dSize_H
		{
			get
			{
				return this._L2dSize_H;
			}
			private set
			{
				this._L2dSize_H = value;
			}
		}

		public int L2dBias_X
		{
			get
			{
				return this._L2dBias_X;
			}
			private set
			{
				this._L2dBias_X = value;
			}
		}

		public int L2dBias_Y
		{
			get
			{
				return this._L2dBias_Y;
			}
			private set
			{
				this._L2dBias_Y = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mst_shipgraph._tableName;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Id = int.Parse(element.Element("Id").get_Value());
			this.Sortno = int.Parse(element.Element("Sortno").get_Value());
			char c = ',';
			if (this.Sortno > 0)
			{
				int[] array = Array.ConvertAll<string, int>(element.Element("Boko").get_Value().Split(new char[]
				{
					c
				}), (string x) => int.Parse(x));
				this.Boko_n_x = array[0];
				this.Boko_n_y = array[1];
				this.Boko_d_x = array[2];
				this.Boko_d_y = array[3];
				int[] array2 = Array.ConvertAll<string, int>(element.Element("Face").get_Value().Split(new char[]
				{
					c
				}), (string x) => int.Parse(x));
				this.Face_n_x = array2[0];
				this.Face_n_y = array2[1];
				this.Face_d_x = array2[2];
				this.Face_d_y = array2[3];
				int[] array3 = Array.ConvertAll<string, int>(element.Element("SlotCategory").get_Value().Split(new char[]
				{
					c
				}), (string x) => int.Parse(x));
				this.Slotitem_category_n_x = array3[0];
				this.Slotitem_category_n_y = array3[1];
				this.Slotitem_category_d_x = array3[2];
				this.Slotitem_category_d_y = array3[3];
				int[] array4 = Array.ConvertAll<string, int>(element.Element("ShipDispCenter").get_Value().Split(new char[]
				{
					c
				}), (string x) => int.Parse(x));
				this.Ship_display_center_n_x = array4[0];
				this.Ship_display_center_n_y = array4[1];
				this.Ship_display_center_d_x = array4[2];
				this.Ship_display_center_d_y = array4[3];
				int[] array5 = Array.ConvertAll<string, int>(element.Element("Wed").get_Value().Split(new char[]
				{
					c
				}), (string x) => int.Parse(x));
				this.Weda_x = array5[0];
				this.Weda_y = array5[1];
				this.Wedb_x = array5[2];
				this.Wedb_y = array5[3];
				int[] array6;
				if (element.Element("L2dSize") != null)
				{
					array6 = Array.ConvertAll<string, int>(element.Element("L2dSize").get_Value().Split(new char[]
					{
						c
					}), (string x) => int.Parse(x));
				}
				else
				{
					array6 = new int[]
					{
						666,
						666
					};
				}
				this.L2dSize_W = array6[0];
				this.L2dSize_H = array6[1];
				int[] array7;
				if (element.Element("L2dBias") != null)
				{
					array7 = Array.ConvertAll<string, int>(element.Element("L2dBias").get_Value().Split(new char[]
					{
						c
					}), (string x) => int.Parse(x));
				}
				else
				{
					array7 = new int[2];
				}
				this.L2dBias_X = array7[0];
				this.L2dBias_Y = array7[1];
			}
		}
	}
}
