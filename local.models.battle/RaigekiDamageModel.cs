using Common.Enum;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class RaigekiDamageModel : DamageModelBase
	{
		protected List<ShipModel_Attacker> _attackers;

		public List<ShipModel_Attacker> Attackers
		{
			get
			{
				return this._attackers;
			}
		}

		public RaigekiDamageModel(ShipModel_BattleAll defender) : base(defender)
		{
			this._attackers = new List<ShipModel_Attacker>();
		}

		public int GetDamage(int attacker_tmp_id)
		{
			int num = this._GetAttackerIndex_(attacker_tmp_id);
			return this._damages.get_Item(num);
		}

		[Obsolete("GetDamage(int ship_tmp_id) を使用してください", false)]
		public int GetDamage(ShipModel_BattleAll attacker)
		{
			int num = this.__GetAttackerIndex(attacker.Index);
			return this._damages.get_Item(num);
		}

		public BattleHitStatus GetHitState(int attacker_tmp_id)
		{
			int num = this._GetAttackerIndex_(attacker_tmp_id);
			return this._hitstates.get_Item(num);
		}

		[Obsolete("GetHitState(int attacker_tmp_id) を使用してください", false)]
		public BattleHitStatus GetHitState(ShipModel_BattleAll attacker)
		{
			int num = this.__GetAttackerIndex(attacker.Index);
			return this._hitstates.get_Item(num);
		}

		public bool GetProtectEffect(int attacker_tmp_id)
		{
			int num = this._GetAttackerIndex_(attacker_tmp_id);
			return this._dmgkind.get_Item(num) == BattleDamageKinds.Rescue;
		}

		public int __AddData__(ShipModel_BattleAll attacker, int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			this._attackers.Add(attacker.__CreateAttacker__());
			return base._AddData(damage, hitstate, dmgkind);
		}

		public int __GetDamage__(int attacker_tmp_id)
		{
			int num = this._GetAttackerIndex_(attacker_tmp_id);
			return this._calc_damages.get_Item(num);
		}

		private int _GetAttackerIndex_(int ship_tmp_id)
		{
			return this._attackers.FindIndex((ShipModel_Attacker ship) => ship.TmpId == ship_tmp_id);
		}

		private int __GetAttackerIndex(int ship_index)
		{
			for (int i = 0; i < this._attackers.get_Count(); i++)
			{
				if (this._attackers.get_Item(i).Index == ship_index)
				{
					return i;
				}
			}
			return -1;
		}
	}
}
