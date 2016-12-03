using System;

namespace KCV.BattleCut
{
	public class BattleData
	{
		private HPData _clsFriendFleetHP;

		private HPData _clsEnemyFleetHP;

		public HPData friendFleetHP
		{
			get
			{
				return this._clsFriendFleetHP;
			}
		}

		public HPData enemyFleetHP
		{
			get
			{
				return this._clsEnemyFleetHP;
			}
		}

		public BattleData()
		{
			this._clsFriendFleetHP = new HPData(0, 0);
			this._clsEnemyFleetHP = new HPData(0, 0);
		}

		public bool Init()
		{
			return true;
		}

		public bool UnInit()
		{
			Mem.Del<HPData>(ref this._clsFriendFleetHP);
			Mem.Del<HPData>(ref this._clsEnemyFleetHP);
			return true;
		}
	}
}
