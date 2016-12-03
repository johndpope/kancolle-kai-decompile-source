using System;

namespace local.models
{
	public class Reward_Deck : IReward
	{
		private int _deck_id;

		public int DeckId
		{
			get
			{
				return this._deck_id;
			}
		}

		public Reward_Deck(int deck_id)
		{
			this._deck_id = deck_id;
		}

		public override string ToString()
		{
			return string.Format("デッキ開放 第{0}艦隊", this.DeckId);
		}
	}
}
