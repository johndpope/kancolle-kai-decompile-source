using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class BattleBGMModel
	{
		private int _area_id;

		private int _map_no;

		private Dictionary<int, List<int>> _mst;

		public int BGM_Day
		{
			get
			{
				return this.getBGMID(true, false);
			}
		}

		public int BGM_Night
		{
			get
			{
				return this.getBGMID(false, false);
			}
		}

		public int BGM_Day_Boss
		{
			get
			{
				return this.getBGMID(true, true);
			}
		}

		public int BGM_Night_Boss
		{
			get
			{
				return this.getBGMID(false, true);
			}
		}

		public BattleBGMModel(int area_id, int map_no)
		{
			this._area_id = area_id;
			this._map_no = map_no;
		}

		public int getBGMID(bool is_day, bool is_boss)
		{
			int num = (!is_day) ? 1 : 0;
			int num2 = (!is_boss) ? 0 : 1;
			return this._mst.get_Item(num).get_Item(num2);
		}

		public override string ToString()
		{
			string text = string.Format("戦闘BGM {0}-{1}", this._area_id, this._map_no);
			text += " ";
			text += string.Format("ザコ戦(昼:{0} 夜:{1})", this.BGM_Day, this.BGM_Night);
			text += " ";
			return text + string.Format("ボス戦(昼:{0} 夜:{1})", this.BGM_Day_Boss, this.BGM_Night_Boss);
		}
	}
}
