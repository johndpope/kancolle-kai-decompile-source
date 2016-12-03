using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class __EscortDeckModel__ : EscortDeckModel
	{
		public __EscortDeckModel__(Mem_esccort_deck mem_escort_deck, Dictionary<int, ShipModel> ships) : base(mem_escort_deck, ships)
		{
		}

		public TemporaryEscortDeckModel GetCloneDeck(Dictionary<int, ShipModel> ships)
		{
			DeckShips deckships;
			this._mem_escort_deck.Ship.Clone(out deckships);
			return new TemporaryEscortDeckModel(this.Id, deckships, this._mem_escort_deck.Name, ships);
		}
	}
}
