using Common.Enum;
using System;

namespace Server_Common.Formats.Battle
{
	public class SupportAtack
	{
		public int Deck_Id;

		public bool[] Undressing_Flag;

		public BattleSupportKinds SupportType;

		public AirBattle AirBattle;

		public Support_HouRai Hourai;

		public SupportAtack()
		{
			this.Undressing_Flag = new bool[6];
		}
	}
}
