using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipExpModel
	{
		private int _level_before;

		private int _level_after;

		private int _exp_before;

		private int _exp_after;

		private int _exp_rate_before;

		private List<int> _exp_rate_after;

		private List<int> _exp_at_levelup;

		public int LevelBefore
		{
			get
			{
				return this._level_before;
			}
		}

		public int LevelAfter
		{
			get
			{
				return this._level_after;
			}
		}

		public int ExpBefore
		{
			get
			{
				return this._exp_before;
			}
		}

		public int ExpAfter
		{
			get
			{
				return this._exp_after;
			}
		}

		public int Exp
		{
			get
			{
				return this._exp_after - this._exp_before;
			}
		}

		public int ExpRateBefore
		{
			get
			{
				return this._exp_rate_before;
			}
		}

		public List<int> ExpRateAfter
		{
			get
			{
				return this._exp_rate_after;
			}
		}

		public List<int> ExpAtLevelup
		{
			get
			{
				return this._exp_at_levelup;
			}
		}

		public ShipExpModel(int exp_rate_before, ShipModel after_ship, int exp, List<int> levelup_info)
		{
			List<int> arg_22_0;
			if (levelup_info == null)
			{
				List<int> list = new List<int>();
				list.Add(0);
				arg_22_0 = list;
			}
			else
			{
				arg_22_0 = levelup_info;
			}
			levelup_info = arg_22_0;
			int num = 0;
			this._level_after = 0;
			this._exp_after = 0;
			int num2 = 0;
			if (after_ship != null)
			{
				if (after_ship.Level == 99 || after_ship.Level == 150)
				{
					num = Math.Max(levelup_info.get_Count() - 1, 0);
				}
				else
				{
					num = Math.Max(levelup_info.get_Count() - 2, 0);
				}
				this._level_after = after_ship.Level;
				this._exp_after = after_ship.Exp;
				num2 = after_ship.Exp_Percentage;
			}
			this._level_before = this._level_after - num;
			this._exp_before = 0;
			if (levelup_info.get_Count() > 0)
			{
				this._exp_before = levelup_info.get_Item(0);
			}
			this._exp_rate_before = exp_rate_before;
			this._exp_rate_after = new List<int>();
			for (int i = 0; i < num; i++)
			{
				this._exp_rate_after.Add(100);
			}
			this._exp_rate_after.Add(num2);
			this._exp_at_levelup = levelup_info.GetRange(1, levelup_info.get_Count() - 1);
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("獲得経験値:{0} 経験値:{1}=>{2}", this.ExpAfter - this.ExpBefore, this.ExpBefore, this.ExpAfter);
			text += string.Format(" (Lv:{0} 経験値:{1}({2}%))", this.LevelBefore, this.ExpBefore, this.ExpRateBefore);
			text += "=>";
			for (int i = 0; i <= this.LevelAfter - this.LevelBefore; i++)
			{
				int num = this.LevelBefore + i;
				int num2 = this.ExpRateAfter.get_Item(i);
				int num3 = (num2 != 100) ? this.ExpAfter : this.ExpAtLevelup.get_Item(i);
				text += string.Format("(Lv:{0} 経験値:{1}({2}%)))", num, num3, num2);
				if (num2 == 100)
				{
					text += "=>";
				}
			}
			return text;
		}
	}
}
