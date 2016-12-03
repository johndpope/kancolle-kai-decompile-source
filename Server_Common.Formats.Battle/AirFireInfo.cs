using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class AirFireInfo
	{
		public int AttackerId;

		public int AirFireKind;

		public List<int> UseItems;

		public AirFireInfo()
		{
		}

		public AirFireInfo(int rid, int kind, List<int> items)
		{
			this.AttackerId = rid;
			this.AirFireKind = kind;
			this.UseItems = items;
		}
	}
}
