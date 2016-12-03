using Common.Enum;
using System;

namespace Server_Common.Formats.Battle
{
	public class Support_HouRai
	{
		private readonly int capacity;

		public BattleHitStatus[] Clitical;

		public int[] Damage;

		public BattleDamageKinds[] DamageType;

		public Support_HouRai()
		{
			this.capacity = 6;
			this.Clitical = new BattleHitStatus[this.capacity];
			this.Damage = new int[this.capacity];
			this.DamageType = new BattleDamageKinds[this.capacity];
			for (int i = 0; i < this.capacity; i++)
			{
				this.Clitical[i] = BattleHitStatus.Miss;
				this.Damage[i] = 0;
				this.DamageType[i] = BattleDamageKinds.Normal;
			}
		}
	}
}
