using Common.Enum;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class Hougeki<T> where T : IConvertible
	{
		public int Attacker;

		public T SpType;

		public List<int> Slot_List;

		public List<int> Target;

		public List<BattleHitStatus> Clitical;

		public List<int> Damage;

		public List<BattleDamageKinds> DamageKind;

		public Hougeki()
		{
			this.Slot_List = new List<int>();
			this.Target = new List<int>();
			this.Clitical = new List<BattleHitStatus>();
			this.Damage = new List<int>();
			this.DamageKind = new List<BattleDamageKinds>();
		}
	}
}
