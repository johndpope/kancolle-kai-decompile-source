using Server_Models;
using System;

namespace Server_Controllers.BattleLogic
{
	public class BattleShipSubInfo
	{
		private Mem_ship shipInstance;

		private int deckIdx;

		private int totalDamage;

		private int attackNo;

		public Mem_ship ShipInstance
		{
			get
			{
				return this.shipInstance;
			}
			private set
			{
				this.shipInstance = value;
			}
		}

		public int DeckIdx
		{
			get
			{
				return this.deckIdx;
			}
			private set
			{
				this.deckIdx = value;
			}
		}

		public int TotalDamage
		{
			get
			{
				return this.totalDamage;
			}
			set
			{
				this.totalDamage = value;
			}
		}

		public int AttackNo
		{
			get
			{
				return this.attackNo;
			}
			private set
			{
				this.attackNo = value;
			}
		}

		public BattleShipSubInfo(int deck_idx, Mem_ship ship)
		{
			this.DeckIdx = deck_idx;
			this.ShipInstance = ship;
		}

		public BattleShipSubInfo(int deck_idx, Mem_ship ship, int attackNo)
		{
			this.DeckIdx = deck_idx;
			this.ShipInstance = ship;
			this.AttackNo = attackNo;
		}
	}
}
