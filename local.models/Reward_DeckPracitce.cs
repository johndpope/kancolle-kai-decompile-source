using Common.Enum;
using System;

namespace local.models
{
	public class Reward_DeckPracitce : IReward
	{
		private DeckPracticeType _type;

		public DeckPracticeType type
		{
			get
			{
				return this._type;
			}
		}

		public Reward_DeckPracitce(int opened_deckpractice_id)
		{
			this._type = (DeckPracticeType)opened_deckpractice_id;
		}

		public override string ToString()
		{
			return string.Format("演習タイプ開放報酬: {0}", this.type);
		}
	}
}
