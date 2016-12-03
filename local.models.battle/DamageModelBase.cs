using Common.Enum;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public abstract class DamageModelBase
	{
		protected bool _initialized;

		protected ShipModel_Defender _defender;

		protected List<int> _calc_damages;

		protected List<int> _damages;

		protected List<BattleHitStatus> _hitstates;

		protected List<BattleDamageKinds> _dmgkind;

		public ShipModel_Defender Defender
		{
			get
			{
				return this._defender;
			}
		}

		public DamageModelBase(ShipModel_BattleAll defender)
		{
			this._defender = defender.__CreateDefender__();
			this._calc_damages = new List<int>();
			this._damages = new List<int>();
			this._hitstates = new List<BattleHitStatus>();
			this._dmgkind = new List<BattleDamageKinds>();
		}

		public int GetDamage()
		{
			int num = 0;
			for (int i = 0; i < this._damages.get_Count(); i++)
			{
				num += this._damages.get_Item(i);
			}
			return num;
		}

		public BattleHitStatus GetHitState()
		{
			if (this._hitstates.get_Count() == 0)
			{
				return BattleHitStatus.Miss;
			}
			BattleHitStatus battleHitStatus = this._hitstates.get_Item(0);
			for (int i = 1; i < this._hitstates.get_Count(); i++)
			{
				if (this._hitstates.get_Item(i) == BattleHitStatus.Clitical)
				{
					return BattleHitStatus.Clitical;
				}
				if (this._hitstates.get_Item(i) == BattleHitStatus.Normal && battleHitStatus == BattleHitStatus.Miss)
				{
					battleHitStatus = BattleHitStatus.Normal;
				}
			}
			return battleHitStatus;
		}

		public bool GetProtectEffect()
		{
			for (int i = 0; i < this._dmgkind.get_Count(); i++)
			{
				if (this._dmgkind.get_Item(i) == BattleDamageKinds.Rescue)
				{
					return true;
				}
			}
			return false;
		}

		public virtual bool GetGurdEffect()
		{
			return false;
		}

		public int __GetDamage__()
		{
			int num = 0;
			for (int i = 0; i < this._calc_damages.get_Count(); i++)
			{
				num += this._calc_damages.get_Item(i);
			}
			return num;
		}

		public void __CalcDamage__()
		{
			this._Initialize();
		}

		protected int _AddData(int damage, BattleHitStatus hitstate, BattleDamageKinds dmgkind)
		{
			this._calc_damages.Add(damage);
			this._damages.Add(damage);
			this._hitstates.Add(hitstate);
			this._dmgkind.Add(dmgkind);
			return this._damages.get_Count();
		}

		protected void _Initialize()
		{
			if (this._initialized)
			{
				return;
			}
			if (this.Defender.IsPractice())
			{
				this._AdjustDamage();
			}
			this.Defender.SetDamage(this.GetDamage());
			this._initialized = true;
		}

		protected void _InitializeRengeki()
		{
			if (this._initialized)
			{
				return;
			}
			if (this.Defender.IsPractice())
			{
				this._AdjustDamage();
			}
			this.Defender.SetDamage(this._damages.get_Item(0), this._damages.get_Item(1));
			this._initialized = true;
		}

		private void _AdjustDamage()
		{
			int damage = this.GetDamage();
			if (this.Defender.HpBefore <= damage)
			{
				int num = damage - (this.Defender.HpBefore - 1);
				for (int i = 0; i < this._damages.get_Count(); i++)
				{
					if (this._damages.get_Item(i) > 0)
					{
						this._damages.set_Item(i, this._damages.get_Item(i) - 1);
						num--;
					}
					if (i == this._damages.get_Count() - 1)
					{
						i = -1;
					}
					if (num == 0)
					{
						break;
					}
				}
			}
		}
	}
}
