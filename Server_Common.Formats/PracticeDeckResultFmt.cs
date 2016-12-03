using Common.Struct;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class PracticeDeckResultFmt
	{
		public MissionResultFmt PracticeResult;

		public Dictionary<int, PowUpInfo> PowerUpData;

		public PracticeDeckResultFmt()
		{
			this.PracticeResult = new MissionResultFmt();
			this.PowerUpData = new Dictionary<int, PowUpInfo>();
		}
	}
}
