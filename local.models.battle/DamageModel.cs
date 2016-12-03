using Common.Enum;
using System;

namespace local.models.battle
{
	public class DamageModel : DamageModelBase
	{
		public DamageModel(ShipModel_BattleAll defender) : base(defender)
		{
		}

		public void __AddData__(int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			base._AddData(damage, hitstate, dmgkind);
		}

		public override string ToString()
		{
			return string.Format("{0}({1})へダメージ:{2}(r:{5}) {3}{4}\n", new object[]
			{
				base.Defender.Name,
				base.Defender.Index,
				base.GetDamage(),
				base.GetHitState(),
				(!base.GetProtectEffect()) ? string.Empty : "[かばう]",
				base.__GetDamage__()
			});
		}
	}
}
