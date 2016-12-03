using Common.Enum;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class RadingResultData
	{
		public int AreaId;

		public RadingKind AttackKind;

		public int DeckAttackPow;

		public int FlagShipMstId;

		public DamageState FlagShipDamageState;

		public List<RadingDamageData> RadingDamage;

		public int BeforeNum;

		public int BreakNum;

		public override string ToString()
		{
			return string.Format("[通商破壊結果]\n海域{0} 攻撃種別:{1} 輸送船{2}隻(移動中の船を除く)から{3}隻ロスト", new object[]
			{
				this.AreaId,
				this.AttackKind,
				this.BeforeNum,
				this.BreakNum
			});
		}
	}
}
