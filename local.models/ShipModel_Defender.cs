using Common.Enum;
using local.utils;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_Defender : ShipModel_Battle
	{
		private bool _set_damage;

		private bool _rengeki;

		private int _hp_before;

		private int _hp_pre_after;

		private int _hp_after;

		private int _hp_after_recovery;

		private List<SlotitemModel_Battle> _slotitems_before;

		private SlotitemModel_Battle _slotitemex_before;

		private List<SlotitemModel_Battle> _slotitems_after_recovery;

		private SlotitemModel_Battle _slotitemex_after_recovery;

		public int HpBefore
		{
			get
			{
				return base._GetHp(this._hp_before);
			}
		}

		public int HpPreAfter
		{
			get
			{
				return base._GetHp(this._hp_pre_after);
			}
		}

		public int HpAfter
		{
			get
			{
				return base._GetHp(this._hp_after);
			}
		}

		public int HpAfterRecovery
		{
			get
			{
				return base._GetHp(this._hp_after_recovery);
			}
		}

		public DamageState_Battle DmgStateBefore
		{
			get
			{
				return base._GetDmgState(this._hp_before);
			}
		}

		public DamageState_Battle DmgStatePreAfter
		{
			get
			{
				return base._GetDmgState(this._hp_pre_after);
			}
		}

		public DamageState_Battle DmgStateAfter
		{
			get
			{
				return base._GetDmgState(this._hp_after);
			}
		}

		public DamageState_Battle DmgStateAfterRecovery
		{
			get
			{
				return base._GetDmgState(this._hp_after_recovery);
			}
		}

		public DamagedStates DamageEventPreAfter
		{
			get
			{
				return this._GetDamageEvent1(this.DmgStateBefore, this.DmgStatePreAfter);
			}
		}

		public DamagedStates DamageEventAfter
		{
			get
			{
				return this._GetDamageEvent(this.DmgStateBefore, this.DmgStatePreAfter, this.DmgStateAfter);
			}
		}

		public bool DamagedFlgBefore
		{
			get
			{
				return base._GetDamagedFlg(this.DmgStateBefore);
			}
		}

		public bool DamagedFlgPreAfter
		{
			get
			{
				return base._GetDamagedFlg(this.DmgStatePreAfter);
			}
		}

		public bool DamagedFlgAfter
		{
			get
			{
				return base._GetDamagedFlg(this.DmgStateAfter);
			}
		}

		public bool DamagedFlgAfterRecovery
		{
			get
			{
				return base._GetDamagedFlg(this.DmgStateAfterRecovery);
			}
		}

		public List<SlotitemModel_Battle> SlotitemListBefore
		{
			get
			{
				return this._slotitems_before.GetRange(0, this._slotitems_before.get_Count());
			}
		}

		public SlotitemModel_Battle SlotitemExBefore
		{
			get
			{
				return this._slotitemex_before;
			}
		}

		public List<SlotitemModel_Battle> SlotitemListAfterRecovery
		{
			get
			{
				if (this._slotitems_after_recovery == null)
				{
					return this.SlotitemListBefore;
				}
				return this._slotitems_after_recovery.GetRange(0, this._slotitems_after_recovery.get_Count());
			}
		}

		public SlotitemModel_Battle SlotitemExAfterRecovery
		{
			get
			{
				return this._slotitemex_after_recovery;
			}
		}

		public ShipModel_Defender(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex)
		{
			this._mst_data = mst_data;
			this._base_data = baseData;
			this._hp_before = hp;
			this._slotitems_before = slotitems;
			this._slotitemex_before = slotitemex;
			this._hp_after = (this._hp_pre_after = (this._hp_after_recovery = this._hp_before));
		}

		public bool HasRecoveryEvent()
		{
			return this.DamageEventAfter == DamagedStates.Youin || this.DamageEventAfter == DamagedStates.Megami;
		}

		public bool HasRecoverYouin()
		{
			if (this.SlotitemExBefore != null && this.SlotitemExBefore.MstId == 42)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = this._slotitems_before.Find((SlotitemModel_Battle item) => item != null && item.MstId == 42);
			return slotitemModel_Battle != null;
		}

		public bool HasRecoverMegami()
		{
			if (this.SlotitemExBefore != null && this.SlotitemExBefore.MstId == 43)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = this._slotitems_before.Find((SlotitemModel_Battle item) => item != null && item.MstId == 43);
			return slotitemModel_Battle != null;
		}

		public void SetDamage(int damage)
		{
			this._set_damage = true;
			this._rengeki = false;
			this._hp_pre_after = this._hp_before;
			this._hp_after = this._hp_before - damage;
			this._SetHpAfterRecovery();
		}

		public void SetDamage(int damage1, int damage2)
		{
			this._set_damage = true;
			this._rengeki = true;
			this._hp_pre_after = this._hp_before - damage1;
			this._hp_after = this._hp_pre_after - damage2;
			this._SetHpAfterRecovery();
		}

		private void _SetHpAfterRecovery()
		{
			this._slotitems_after_recovery = null;
			this._slotitemex_after_recovery = null;
			if (this.DamageEventAfter == DamagedStates.Youin)
			{
				if (this._slotitemex_before != null && this._slotitemex_before.MstId == 42)
				{
					this._slotitemex_after_recovery = null;
					if (base.Index == 0)
					{
						this._hp_after_recovery = (int)Math.Floor((double)this.MaxHp * 0.5);
					}
					else
					{
						this._hp_after_recovery = (int)Math.Floor((double)this.MaxHp * 0.20000000298023224);
					}
				}
				else
				{
					this._slotitems_after_recovery = new List<SlotitemModel_Battle>();
					bool flag = false;
					for (int i = 0; i < this._slotitems_before.get_Count(); i++)
					{
						SlotitemModel_Battle slotitemModel_Battle = this._slotitems_before.get_Item(i);
						if (!flag && slotitemModel_Battle != null && slotitemModel_Battle.MstId == 42)
						{
							if (base.Index == 0)
							{
								this._hp_after_recovery = (int)Math.Floor((double)this.MaxHp * 0.5);
							}
							else
							{
								this._hp_after_recovery = (int)Math.Floor((double)this.MaxHp * 0.20000000298023224);
							}
							flag = true;
						}
						else
						{
							this._slotitems_after_recovery.Add(slotitemModel_Battle);
						}
					}
					while (this._slotitems_after_recovery.get_Count() < this._slotitems_before.get_Count())
					{
						this._slotitems_after_recovery.Add(null);
					}
					this._slotitemex_after_recovery = this._slotitemex_before;
				}
			}
			else if (this.DamageEventAfter == DamagedStates.Megami)
			{
				if (this._slotitemex_before != null && this._slotitemex_before.MstId == 43)
				{
					this._slotitemex_after_recovery = null;
					this._hp_after_recovery = this.MaxHp;
				}
				else
				{
					this._slotitems_after_recovery = new List<SlotitemModel_Battle>();
					bool flag2 = false;
					for (int j = 0; j < this._slotitems_before.get_Count(); j++)
					{
						SlotitemModel_Battle slotitemModel_Battle2 = this._slotitems_before.get_Item(j);
						if (!flag2 && slotitemModel_Battle2 != null && slotitemModel_Battle2.MstId == 43)
						{
							this._hp_after_recovery = this.MaxHp;
							flag2 = true;
						}
						else
						{
							this._slotitems_after_recovery.Add(slotitemModel_Battle2);
						}
					}
					while (this._slotitems_after_recovery.get_Count() < this._slotitems_before.get_Count())
					{
						this._slotitems_after_recovery.Add(null);
					}
					this._slotitemex_after_recovery = this._slotitemex_before;
				}
			}
			else
			{
				this._hp_after_recovery = this._hp_after;
				this._slotitemex_after_recovery = this._slotitemex_before;
			}
		}

		private DamagedStates _GetDamageEvent1(DamageState_Battle before, DamageState_Battle pre_after)
		{
			DamagedStates damagedStates = this.__GetDamageEvent(before, pre_after);
			if (damagedStates == DamagedStates.Shouha)
			{
				return damagedStates;
			}
			return DamagedStates.None;
		}

		private DamagedStates _GetDamageEvent(DamageState_Battle before, DamageState_Battle pre_after, DamageState_Battle after)
		{
			DamagedStates damagedStates = this.__GetDamageEvent(before, after);
			if (this._rengeki && damagedStates == DamagedStates.Shouha && pre_after == DamageState_Battle.Shouha)
			{
				return DamagedStates.None;
			}
			return damagedStates;
		}

		private DamagedStates __GetDamageEvent(DamageState_Battle before, DamageState_Battle after)
		{
			if (before != after)
			{
				if (after == DamageState_Battle.Shouha)
				{
					return DamagedStates.Shouha;
				}
				if (after == DamageState_Battle.Tyuuha)
				{
					return DamagedStates.Tyuuha;
				}
				if (after == DamageState_Battle.Taiha)
				{
					return DamagedStates.Taiha;
				}
				if (after == DamageState_Battle.Gekichin)
				{
					ShipRecoveryType shipRecoveryType = Utils.__HasRecoveryItem__(this.SlotitemListBefore, this.SlotitemExBefore);
					if (shipRecoveryType == ShipRecoveryType.None)
					{
						return DamagedStates.Gekichin;
					}
					if (shipRecoveryType == ShipRecoveryType.Personnel)
					{
						return DamagedStates.Youin;
					}
					if (shipRecoveryType == ShipRecoveryType.Goddes)
					{
						return DamagedStates.Megami;
					}
				}
			}
			return DamagedStates.None;
		}

		public override string ToString()
		{
			string text = string.Format("{0}(mstId:{1})[{2}/{3}({4})", new object[]
			{
				base.Name,
				base.MstId,
				this.HpBefore,
				this.MaxHp,
				this.DmgStateBefore
			});
			if (!this._set_damage)
			{
				return text + "]";
			}
			if (this._rengeki)
			{
				text += string.Format(" => {0}/{1}({2}", this.HpPreAfter, this.MaxHp, this.DmgStatePreAfter);
				DamagedStates damagedStates = this.DamageEventPreAfter;
				if (damagedStates != DamagedStates.None)
				{
					text += string.Format("・{0})", damagedStates);
				}
				else
				{
					text += ")";
				}
				text += string.Format(" => {0}/{1}({2}", this.HpAfter, this.MaxHp, this.DmgStateAfter);
				damagedStates = this.DamageEventAfter;
				if (damagedStates != DamagedStates.None)
				{
					text += string.Format("・{0})", damagedStates);
				}
				else
				{
					text += ")";
				}
			}
			else
			{
				text += string.Format(" => {0}/{1}({2}", this.HpAfter, this.MaxHp, this.DmgStateAfter);
				DamagedStates damageEventAfter = this.DamageEventAfter;
				if (damageEventAfter != DamagedStates.None)
				{
					text += string.Format("・{0})", damageEventAfter);
				}
				else
				{
					text += ")";
				}
			}
			if (this.DamageEventAfter == DamagedStates.Youin)
			{
				text += string.Format(" (Youin)=> {0}/{1}({2})]", this.HpAfterRecovery, this.MaxHp, this.DmgStateAfterRecovery);
			}
			else if (this.DamageEventAfter == DamagedStates.Megami)
			{
				text += string.Format(" (Megami)=> {0}/{1}({2})]", this.HpAfterRecovery, this.MaxHp, this.DmgStateAfterRecovery);
			}
			else
			{
				text += "]";
			}
			return text;
		}
	}
}
