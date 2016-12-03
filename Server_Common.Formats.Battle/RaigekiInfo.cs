using Common.Enum;
using System;

namespace Server_Common.Formats.Battle
{
	public class RaigekiInfo
	{
		public int[] Target;

		public BattleHitStatus[] Clitical;

		public int[] Damage;

		public BattleDamageKinds[] DamageKind;

		public RaigekiInfo()
		{
			this.Target = new int[6];
			this.Damage = new int[6];
			this.Clitical = new BattleHitStatus[6];
			this.DamageKind = new BattleDamageKinds[6];
			for (int i = 0; i < 6; i++)
			{
				this.Target[i] = -1;
				this.Damage[i] = 0;
				this.Clitical[i] = BattleHitStatus.Miss;
				this.DamageKind[i] = BattleDamageKinds.Normal;
			}
		}
	}
}
