using KCV.SortieMap;
using System;
using UnityEngine;

namespace KCV.SortieBattle
{
	[Serializable]
	public class SortieBattlePrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabBattleCutManager;

		[SerializeField]
		private Transform _prefabBattleTaskManager;

		[SerializeField]
		private Transform _prefabProdSortieTransitionToBattle;

		[SerializeField]
		private Transform _prefabUIBattleShutter;

		private ProdSortieTransitionToBattle _prodSortieTransitionToBattle;

		public Transform prefabBattleCutManager
		{
			get
			{
				return this._prefabBattleCutManager;
			}
		}

		public Transform prefabBattleTaskManager
		{
			get
			{
				return this._prefabBattleTaskManager;
			}
		}

		public ProdSortieTransitionToBattle prodSortieTransitionToBattle
		{
			get
			{
				if (this._prodSortieTransitionToBattle == null)
				{
					this._prodSortieTransitionToBattle = ProdSortieTransitionToBattle.Instantiate(this._prefabProdSortieTransitionToBattle.GetComponent<ProdSortieTransitionToBattle>(), SortieBattleTaskManager.GetTransitionCamera().get_transform());
				}
				return this._prodSortieTransitionToBattle;
			}
		}

		public Transform prefabUIBattleShutter
		{
			get
			{
				return this._prefabUIBattleShutter;
			}
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<Transform>(ref this._prefabBattleCutManager);
			Mem.Del<Transform>(ref this._prefabBattleTaskManager);
			Mem.Del<Transform>(ref this._prefabProdSortieTransitionToBattle);
			Mem.Del<Transform>(ref this._prefabUIBattleShutter);
			Mem.Del<ProdSortieTransitionToBattle>(ref this._prodSortieTransitionToBattle);
			base.Dispose(disposing);
		}

		public void DisposeProdSortieTransitionToBattle()
		{
			Mem.DelComponentSafe<ProdSortieTransitionToBattle>(ref this._prodSortieTransitionToBattle);
		}
	}
}
