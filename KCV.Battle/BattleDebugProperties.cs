using Common.Enum;
using KCV.Battle.Utils;
using System;
using UnityEngine;

namespace KCV.Battle
{
	[Serializable]
	public struct BattleDebugProperties
	{
		[Header("[Priority Properties]"), Tooltip("カットインチェック状態")]
		public bool isCutInCheck;

		[Tooltip("戦闘ルート種別")]
		public Generics.BattleRootType rootType;

		[Tooltip("旗艦以外大破にするか否か")]
		public bool isDebugWreckThanTheFlagship;

		[Tooltip("戦闘スキップを使用するか否か")]
		public bool isSkipBattle;

		[Tooltip("艦隊出現後のフェーズ")]
		public BattlePhase fleetAdventLaterSkip;

		[Header("[Sortie Battle Properties]")]
		public int sortieAreaID;

		[Range(1f, 6f)]
		public int sortieMapID;

		[Range(1f, 8f)]
		public int sortieDeckID;

		public BattleFormationKinds1 sortieFormation;

		[Header("[Practice Battle Properties]"), Range(1f, 8f)]
		public int practiceDeckID;

		[Range(1f, 5f)]
		public int practiceEnemyID;

		public BattleFormationKinds1 practiceFormation;

		[Header("[Rebellion Battle Properties]")]
		public int rebellionAreaID;

		[Range(1f, 8f)]
		public int rebellionSubDeckID;

		[Range(1f, 8f)]
		public int rebellionMainDeckID;

		[Range(-1f, 8f)]
		public int rebellionSubSuportDeckID;

		[Range(-1f, 8f)]
		public int rebellionMainSupportDeckID;

		public BattleFormationKinds1 rebellionFormation;
	}
}
