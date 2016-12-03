using Common.Enum;
using System;

namespace Server_Common.Formats.Battle
{
	public class AirBattle1
	{
		public LostPlaneInfo F_LostInfo;

		public int F_TouchPlane;

		public LostPlaneInfo E_LostInfo;

		public int E_TouchPlane;

		public BattleSeikuKinds SeikuKind;

		public AirBattle1()
		{
			this.F_LostInfo = new LostPlaneInfo();
			this.E_LostInfo = new LostPlaneInfo();
			this.SeikuKind = BattleSeikuKinds.None;
			this.F_TouchPlane = 0;
			this.E_TouchPlane = 0;
		}
	}
}
