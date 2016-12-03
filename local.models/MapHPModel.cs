using Server_Common.Formats;
using Server_Models;
using System;

namespace local.models
{
	public class MapHPModel
	{
		private User_MapinfoFmt.enumExBossType _type;

		private int _mapID;

		private int _max_value;

		private int _now_value;

		public int MapID
		{
			get
			{
				return this._mapID;
			}
		}

		public int MaxValue
		{
			get
			{
				return this._max_value;
			}
		}

		public int NowValue
		{
			get
			{
				return this._now_value;
			}
		}

		[Obsolete("[デバッグ用] 表示の確認等の為に一時的にモデルを生成したい場合に使用してください。")]
		public MapHPModel(int mapID, int max, int now)
		{
			this._mapID = mapID;
			this._max_value = max;
			this._now_value = now;
		}

		public MapHPModel(Mst_mapinfo mst, User_MapinfoFmt mem)
		{
			this._mapID = mst.Id;
			if (mem != null)
			{
				this._type = mem.Boss_type;
				if (this._type == User_MapinfoFmt.enumExBossType.MapHp)
				{
					this._max_value = mem.Eventmap.Event_maxhp;
					this._now_value = mem.Eventmap.Event_hp;
				}
				else if (this._type == User_MapinfoFmt.enumExBossType.Defeat)
				{
					this._max_value = 0;
					this._now_value = Math.Max(this._max_value - mem.Defeat_count, 0);
				}
			}
			else
			{
				this._type = User_MapinfoFmt.enumExBossType.Normal;
				this._max_value = (this._now_value = 0);
			}
		}

		public void __Update__(EventMapInfo info)
		{
			if (this._type == User_MapinfoFmt.enumExBossType.MapHp)
			{
				this._max_value = info.Event_maxhp;
				this._now_value = info.Event_hp;
			}
		}

		public override string ToString()
		{
			if (this._type == User_MapinfoFmt.enumExBossType.Normal)
			{
				return string.Format("[MapHP{0}]通常ボスHP: {1}/{2}", this.MapID, this.NowValue, this.MaxValue);
			}
			if (this._type == User_MapinfoFmt.enumExBossType.Defeat)
			{
				return string.Format("[MapHP{0}]討伐系ボスHP: {1}/{2}", this.MapID, this.NowValue, this.MaxValue);
			}
			if (this._type == User_MapinfoFmt.enumExBossType.Normal)
			{
				return string.Format("[MapHP{0}]イベント系ボスHP: {1}/{2}", this.MapID, this.NowValue, this.MaxValue);
			}
			return string.Format("[MapHP{0}]ボスHP: {1}/{2}", this.MapID, this.NowValue, this.MaxValue);
		}
	}
}
