using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class MapSupplyModel
	{
		private ShipModel _ship;

		private List<ShipModel> _given_ships;

		public ShipModel Ship
		{
			get
			{
				return this._ship;
			}
		}

		public List<ShipModel> GivenShips
		{
			get
			{
				return this._given_ships;
			}
		}

		public MapSupplyModel(DeckModel deck, MapSupplyFmt fmt)
		{
			this._ship = deck.GetShipFromMemId(fmt.useShip);
			this._given_ships = fmt.givenShip.ConvertAll<ShipModel>((int mem_id) => deck.GetShipFromMemId(mem_id));
		}
	}
}
