using Common.Enum;
using KCV.Battle.Production;
using local.managers;
using local.models.battle;
using System;
using UnityEngine;

namespace KCV.Battle
{
	public class BattleTest : BaseBattleTask
	{
		private static SortieMapManager _clsSortieMapManager;

		private static SortieBattleManager _clsSortieBattleManager;

		private static PracticeBattleManager _clsPracticeBattleManager;

		private static BattleManager _clsBattleManager;

		private KoukuuModel model;

		private ProdMapOpen mapOpnen;

		public GameObject currentDetonator;

		private int _currentExpIdx = -1;

		public GameObject[] detonatorPrefabs;

		private KeyControl keyInput;

		private void Awake()
		{
			this.keyInput = new KeyControl(0, 0, 0.4f, 0.1f);
		}

		private void Start()
		{
			StrategyMapManager strategyMapManager = new StrategyMapManager();
			SortieManager sortieManager = strategyMapManager.SelectArea(1);
			BattleTest._clsSortieMapManager = sortieManager.GoSortie(1, 11);
			SortieMapManager sortieMapManager = sortieManager.GoSortie(1, 11);
			BattleTest._clsSortieBattleManager = BattleTest._clsSortieMapManager.BattleStart(BattleFormationKinds1.FukuJuu);
			Vector3 vector = new Vector3(0f, 0f, 0f);
			GameObject gameObject = (GameObject)Object.Instantiate(this.currentDetonator, vector, Quaternion.get_identity());
		}

		private void Update()
		{
			this.keyInput.Update();
			if (this.keyInput.keyState.get_Item(1).down)
			{
				Debug.Log("WW");
				Vector3 vector = new Vector3(0f, 0f, 0f);
				GameObject gameObject = (GameObject)Object.Instantiate(this.currentDetonator, vector, Quaternion.get_identity());
			}
		}

		private void testDet()
		{
		}
	}
}
