using Common.Enum;
using local.models;
using System;

namespace KCV.Scene.Practice
{
	public class DeckPracticeContext
	{
		public DeckModel FriendDeck
		{
			get;
			private set;
		}

		public DeckPracticeType PracticeType
		{
			get;
			private set;
		}

		public void SetFriendDeck(DeckModel deckModel)
		{
			this.FriendDeck = deckModel;
		}

		public void SetPracticeType(DeckPracticeType practiceType)
		{
			this.PracticeType = practiceType;
		}
	}
}
