using System;

namespace Server_Common.Formats.Battle
{
	public class Raigeki : IBattleType
	{
		private int fmtType;

		public RaigekiInfo F_Rai;

		public RaigekiInfo E_Rai;

		public int FmtType
		{
			get
			{
				return this.fmtType;
			}
		}

		public Raigeki()
		{
			this.fmtType = 2;
		}
	}
}
