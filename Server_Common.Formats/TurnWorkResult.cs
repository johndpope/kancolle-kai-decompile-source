using Common.Enum;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class TurnWorkResult
	{
		public TurnState ChangeState;

		public List<int> BlingEndShip;

		public List<int> BlingEndEscortDeck;

		public Dictionary<int, List<Mem_tanker>> BlingEndTanker;

		public Dictionary<enumMaterialCategory, int> TransportMaterial;

		public Dictionary<enumMaterialCategory, int> BonusMaterialMonthly;

		public Dictionary<enumMaterialCategory, int> BonusMaterialWeekly;

		public List<RadingResultData> RadingResult;

		public List<Mem_deck> MissionEndDecks;

		public List<ItemGetFmt> SpecialItem;
	}
}
