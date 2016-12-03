using Server_Common.Formats;
using System;

namespace local.models
{
	public class HistoryModel_AreaStart : HistoryModelBase
	{
		public int AreaId
		{
			get
			{
				return this._fmt.MapInfo.Maparea_id;
			}
		}

		public string AreaName
		{
			get
			{
				return this._fmt.AreaName;
			}
		}

		public int MapNo
		{
			get
			{
				return this._fmt.MapInfo.No;
			}
		}

		public int MapId
		{
			get
			{
				return this._fmt.MapInfo.Id;
			}
		}

		public string MapName
		{
			get
			{
				return this._fmt.MapInfo.Name;
			}
		}

		public HistoryModel_AreaStart(User_HistoryFmt fmt) : base(fmt)
		{
		}

		public override string ToString()
		{
			string text = string.Format("{0}年{1} {2}日", base.DateStruct.Year, base.DateStruct.Month, base.DateStruct.Day);
			return string.Format("{0} {1}[#{2}-{3}]({4}) 攻略開始", new object[]
			{
				text,
				this.AreaName,
				this.AreaId,
				this.MapNo,
				this.MapName
			});
		}
	}
}
