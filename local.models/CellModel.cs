using Common.Enum;
using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class CellModel
	{
		private User_MapCellInfo _data;

		public int AreaId
		{
			get
			{
				return this._data.Mst_mapcell.Maparea_id;
			}
		}

		public int MapNo
		{
			get
			{
				return this._data.Mst_mapcell.Mapinfo_no;
			}
		}

		public int CellNo
		{
			get
			{
				return this._data.Mst_mapcell.No;
			}
		}

		public int ColorNo
		{
			get
			{
				return this._data.Mst_mapcell.Color_no;
			}
		}

		public enumMapEventType EventType
		{
			get
			{
				return this._data.Mst_mapcell.Event_1;
			}
		}

		public enumMapWarType WarType
		{
			get
			{
				return this._data.Mst_mapcell.Event_2;
			}
		}

		public bool Passed
		{
			get
			{
				return this._data.Passed;
			}
		}

		public CellModel(User_MapCellInfo data)
		{
			this._data = data;
		}

		public List<int> GetLinkNo()
		{
			return this._data.Mst_mapcell.GetLinkNo();
		}

		public override string ToString()
		{
			string[] array = new string[]
			{
				"白",
				"青",
				"緑",
				"紫",
				"赤",
				"赤(BOSS)",
				"赤",
				"航空戦マス",
				"補給EO",
				"航空偵察"
			};
			string text = string.Format("#{0}-{1} セル{2}({3})", new object[]
			{
				this.AreaId,
				this.MapNo,
				this.CellNo,
				array[this.ColorNo]
			});
			text += string.Format(" {0}-{1}", this.EventType, this.WarType);
			return text + string.Format(" {0}", (!this.Passed) ? "未通過" : "通過済");
		}
	}
}
