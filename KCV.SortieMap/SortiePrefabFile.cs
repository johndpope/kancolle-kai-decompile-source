using System;
using UnityEngine;

namespace KCV.SortieMap
{
	[Serializable]
	public class SortiePrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabUISortieShip;

		[SerializeField]
		private Transform _prefabUICompassManager;

		[SerializeField]
		private Transform _prefabBattleCutManager;

		[SerializeField]
		private Transform _prefabUIBattleFormationKindSelectManager;

		[SerializeField]
		private Transform _prefabProdSortieTransitionToBattle;

		[SerializeField]
		private Transform _prefabProdShipRipple;

		[SerializeField]
		private Transform _prefabProdMaelstrom;

		[SerializeField]
		private Transform _prefabUIAreaGauge;

		[SerializeField]
		private Transform _prefabProdSortieEnd;

		[SerializeField]
		private Transform _prefabCtrlSortieResult;

		[SerializeField]
		private Transform _prefabProdUnderwayReplenishment;

		public Transform prefabUISortieShip
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabUISortieShip);
			}
		}

		public Transform prefabUICompassManager
		{
			get
			{
				return this._prefabUICompassManager;
			}
		}

		public Transform prefabBattleCutManager
		{
			get
			{
				return this._prefabBattleCutManager;
			}
		}

		public Transform prefabUIBattleFormationKindSelectManager
		{
			get
			{
				return this._prefabUIBattleFormationKindSelectManager;
			}
		}

		public Transform prefabProdSortieTransitionToBattle
		{
			get
			{
				return this._prefabProdSortieTransitionToBattle;
			}
		}

		public Transform prefabProdShipRipple
		{
			get
			{
				return this._prefabProdShipRipple;
			}
		}

		public Transform prefabProdMaelstrom
		{
			get
			{
				return this._prefabProdMaelstrom;
			}
		}

		public Transform prefabUIAreaGauge
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabUIAreaGauge);
			}
		}

		public Transform prodSortieEnd
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdSortieEnd);
			}
		}

		public Transform prefabCtrlSortieResult
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabCtrlSortieResult);
			}
		}

		public Transform prefabProdUnderwayReplenishment
		{
			get
			{
				return this._prefabProdUnderwayReplenishment;
			}
		}

		public bool Init()
		{
			return true;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<Transform>(ref this._prefabUISortieShip);
			Mem.Del<Transform>(ref this._prefabUICompassManager);
			Mem.Del<Transform>(ref this._prefabBattleCutManager);
			Mem.Del<Transform>(ref this._prefabUIBattleFormationKindSelectManager);
			Mem.Del<Transform>(ref this._prefabProdSortieTransitionToBattle);
			Mem.Del<Transform>(ref this._prefabProdShipRipple);
			Mem.Del<Transform>(ref this._prefabProdMaelstrom);
			Mem.Del<Transform>(ref this._prefabUIAreaGauge);
			Mem.Del<Transform>(ref this._prefabCtrlSortieResult);
			Mem.Del<Transform>(ref this._prefabProdUnderwayReplenishment);
			base.Dispose(disposing);
		}
	}
}
