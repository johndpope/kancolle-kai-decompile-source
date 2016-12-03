using System;

namespace Server_Common.Formats.Battle
{
	public class LostPlaneInfo
	{
		public int Count;

		public int LostCount;

		public LostPlaneInfo()
		{
			this.Count = 0;
			this.LostCount = 0;
		}
	}
}
