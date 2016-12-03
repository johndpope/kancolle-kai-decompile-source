using Common.Enum;
using System;

namespace local.models.battle
{
	public class BakuRaiDamageModel : RaigekiDamageModel
	{
		private bool _is_raigeki;

		private bool _is_bakugeki;

		public BakuRaiDamageModel(ShipModel_BattleAll defender, bool is_raigeki, bool is_bakugeki) : base(defender)
		{
			this._is_raigeki = is_raigeki;
			this._is_bakugeki = is_bakugeki;
		}

		public bool IsRaigeki()
		{
			return this._is_raigeki;
		}

		public bool IsBakugeki()
		{
			return this._is_bakugeki;
		}

		public int __AddData__(int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			this._attackers.Add(null);
			return base._AddData(damage, hitstate, dmgkind);
		}
	}
}
