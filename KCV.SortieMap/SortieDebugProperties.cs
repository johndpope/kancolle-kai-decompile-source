using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[Serializable]
	public struct SortieDebugProperties
	{
		[Header("[Priority Properties]")]
		public Generics.BattleRootType rootType;

		public bool isCutInChk;

		public bool isSkipBattle;

		[Header("[SortieMap(Normal) Properties]"), Range(1f, 18f)]
		public int sortieAreaID;

		[Range(1f, 7f)]
		public int sortieMapID;

		[Range(1f, 8f)]
		public int sortieDeckID;

		[Header("[SortieMap(Rebellion) Properties]"), Range(1f, 18f)]
		public int rebellionAreaID;

		[Range(1f, 8f)]
		public int rebellionSubDeckID;

		[Range(1f, 8f)]
		public int rebellionMainDeckID;

		[Range(-1f, 8f)]
		public int rebellionSubSupportDeckID;

		[Range(-1f, 8f)]
		public int rebellionMainSupportDeckID;
	}
}
