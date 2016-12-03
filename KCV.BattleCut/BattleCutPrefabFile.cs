using System;
using UnityEngine;

namespace KCV.BattleCut
{
	[Serializable]
	public class BattleCutPrefabFile : BasePrefabFile
	{
		[SerializeField]
		private Transform _prefabProdFormation;

		[SerializeField]
		private Transform _prefabCtrlBCCommandSelect;

		[SerializeField]
		private Transform _prefabProdBCBattle;

		[SerializeField]
		private Transform _prefabProdWithdrawalDecision;

		[SerializeField]
		private Transform _prefabProdBattleEnd;

		[SerializeField]
		private Transform _prefabProdWinRunkJudge;

		[SerializeField]
		private Transform _prefabProdResult;

		[SerializeField]
		private Transform _prefabProdBCAdvancingWithdrawal;

		[SerializeField]
		private Transform _prefabProdBCAdvancingWithdrawalDC;

		[SerializeField]
		private Transform _prefabProdFlagshipWreck;

		public Transform prefabProdFormation
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdFormation);
			}
		}

		public Transform prefabCtrlBCCommandSelect
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabCtrlBCCommandSelect);
			}
		}

		public Transform prefabProdBCBattle
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdBCBattle);
			}
		}

		public Transform prefabProdWithdrawalDecision
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdWithdrawalDecision);
			}
		}

		public Transform prefabProdBattleEnd
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdBattleEnd);
			}
		}

		public Transform prefabProdWinRunkJudge
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdWinRunkJudge);
			}
		}

		public Transform prefabProdResult
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdResult);
			}
		}

		public Transform prefabProdBCAdvancingWithdrawal
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdBCAdvancingWithdrawal);
			}
		}

		public Transform prefabProdBCAdvancingWithdrawalDC
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdBCAdvancingWithdrawalDC);
			}
		}

		public Transform prefabProdFlagshipWreck
		{
			get
			{
				return BasePrefabFile.PassesPrefab(ref this._prefabProdFlagshipWreck);
			}
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			return true;
		}

		protected override void Dispose(bool disposing)
		{
			Mem.Del<Transform>(ref this._prefabProdFormation);
			Mem.Del<Transform>(ref this._prefabCtrlBCCommandSelect);
			Mem.Del<Transform>(ref this._prefabProdBCBattle);
			Mem.Del<Transform>(ref this._prefabProdWithdrawalDecision);
			Mem.Del<Transform>(ref this._prefabProdBattleEnd);
			Mem.Del<Transform>(ref this._prefabProdWinRunkJudge);
			Mem.Del<Transform>(ref this._prefabProdResult);
			Mem.Del<Transform>(ref this._prefabProdBCAdvancingWithdrawal);
			Mem.Del<Transform>(ref this._prefabProdBCAdvancingWithdrawalDC);
			Mem.Del<Transform>(ref this._prefabProdFlagshipWreck);
			base.Dispose(disposing);
		}
	}
}
