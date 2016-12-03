using Common.Enum;
using local.models;
using System;
using System.Collections.Generic;

namespace KCV.Scene.Practice
{
	public class BattlePracticeContext
	{
		public enum PlayType
		{
			Battle,
			ShortCutBattle
		}

		public BattlePracticeContext.PlayType BattleStartType
		{
			get;
			private set;
		}

		public List<IsGoCondition> Conditions
		{
			get;
			private set;
		}

		public DeckModel FriendDeck
		{
			get;
			private set;
		}

		public DeckModel TargetDeck
		{
			get;
			private set;
		}

		public BattleFormationKinds1 FormationType
		{
			get;
			private set;
		}

		public BattlePracticeContext()
		{
			this.BattleStartType = BattlePracticeContext.PlayType.Battle;
		}

		public void SetBattleType(BattlePracticeContext.PlayType battleType)
		{
			this.BattleStartType = battleType;
		}

		public void SetTargetDeck(DeckModel deck)
		{
			this.TargetDeck = deck;
		}

		public void SetFriendDeck(DeckModel deck)
		{
			this.FriendDeck = deck;
		}

		public void SetConditions(List<IsGoCondition> conditions)
		{
			this.Conditions = conditions;
		}

		public void SetFormationType(BattleFormationKinds1 kind)
		{
			this.FormationType = kind;
		}
	}
}
