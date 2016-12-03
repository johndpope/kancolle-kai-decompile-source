using Common.Enum;
using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[Serializable]
	public struct DebugProperties
	{
		[Header("[Priority Properties]")]
		public bool isDebug;

		public Generics.BattleRootType battleRootType;

		[Header("[Sortie Battle Properties]")]
		public int sortieAreaID;

		public int sortieMapID;

		public int sortieDeckID;

		[Header("[Practice Battle Properties]")]
		public BattleFormationKinds1 practiceFormation;

		[Header("Rebellion Battle Properties")]
		public int rebellionAreaID;

		public int rebellionSubDeckID;

		public int rebellionMainDeckID;

		public int rebellionSubSuportDeckID;

		public int rebellionMainSuportDeckID;
	}
}
