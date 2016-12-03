using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class MapSupplyFmt
	{
		public int useShip;

		public List<int> givenShip;

		public MapSupplyFmt()
		{
		}

		public MapSupplyFmt(int use_ship, List<int> supply_ships)
		{
			this.useShip = use_ship;
			this.givenShip = supply_ships;
			this.givenShip.Remove(this.useShip);
		}
	}
}
