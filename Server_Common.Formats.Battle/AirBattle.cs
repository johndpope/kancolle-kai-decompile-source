using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class AirBattle
	{
		public bool[] StageFlag;

		public List<int> F_PlaneFrom;

		public List<int> E_PlaneFrom;

		public AirBattle1 Air1;

		public AirBattle2 Air2;

		public AirBattle3 Air3;

		public AirBattle()
		{
			this.StageFlag = new bool[3];
			this.F_PlaneFrom = new List<int>();
			this.E_PlaneFrom = new List<int>();
		}

		public void SetStageFlag()
		{
			this.StageFlag[0] = ((this.Air1 != null) ? true : false);
			this.StageFlag[1] = ((this.Air2 != null) ? true : false);
			this.StageFlag[2] = ((this.Air3 != null) ? true : false);
		}
	}
}
