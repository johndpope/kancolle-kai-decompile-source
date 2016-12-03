using System;

namespace Server_Common.Formats
{
	public class User_MapinfoFmt
	{
		public enum enumExBossType
		{
			Normal,
			Defeat,
			MapHp
		}

		public int Id;

		public bool Cleared;

		public User_MapinfoFmt.enumExBossType Boss_type;

		public EventMapInfo Eventmap;

		public int Defeat_count;

		public bool IsGo;

		public User_MapinfoFmt()
		{
			this.Boss_type = User_MapinfoFmt.enumExBossType.Normal;
			this.Eventmap = new EventMapInfo();
			this.Cleared = false;
			this.IsGo = false;
		}
	}
}
