using KCV.EscortOrganize;
using KCV.Organize;
using local.models;
using System;

public class DeployOrganizeScrollListChild : OrganizeScrollListChild
{
	protected override bool IsDeckInShip(ShipModel shipModel)
	{
		DeckModelBase deck = shipModel.getDeck();
		bool flag = deck != null;
		bool flag2 = flag && !deck.IsEscortDeckMyself();
		bool flag3 = false;
		ShipModel[] ships = EscortOrganizeTaskManager.GetEscortManager().EditDeck.GetShips();
		ShipModel[] array = ships;
		for (int i = 0; i < array.Length; i++)
		{
			ShipModel shipModel2 = array[i];
			if (shipModel2.MemId == shipModel.MemId)
			{
				flag3 = true;
				break;
			}
		}
		return flag2 || flag3;
	}

	protected override DeckModelBase GetDeckFromShip(ShipModel shipModel)
	{
		DeckModelBase deck = shipModel.getDeck();
		bool flag = deck != null;
		if (flag)
		{
			return deck;
		}
		return EscortOrganizeTaskManager.GetEscortManager().EditDeck;
	}
}
