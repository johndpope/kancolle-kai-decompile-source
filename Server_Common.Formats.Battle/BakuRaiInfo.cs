using Common.Enum;
using System;

namespace Server_Common.Formats.Battle
{
	public class BakuRaiInfo
	{
		private readonly int capacity;

		public bool[] IsRaigeki;

		public bool[] IsBakugeki;

		public BattleHitStatus[] Clitical;

		public int[] Damage;

		public BattleDamageKinds[] DamageType;

		public BakuRaiInfo()
		{
			this.capacity = 6;
			this.IsRaigeki = new bool[this.capacity];
			this.IsBakugeki = new bool[this.capacity];
			this.Clitical = new BattleHitStatus[this.capacity];
			this.Damage = new int[this.capacity];
			this.DamageType = new BattleDamageKinds[this.capacity];
			for (int i = 0; i < this.capacity; i++)
			{
				this.IsRaigeki[i] = false;
				this.IsBakugeki[i] = false;
				this.Clitical[i] = BattleHitStatus.Normal;
				this.Damage[i] = 0;
				this.DamageType[i] = BattleDamageKinds.Normal;
			}
		}
	}
}
