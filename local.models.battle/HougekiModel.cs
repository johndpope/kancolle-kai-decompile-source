using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class HougekiModel : DamageModelBase, IBattlePhase
	{
		private bool _is_night;

		private BattleAttackKind _attack_type;

		private ShipModel_Attacker _attacker;

		private List<SlotitemModel_Battle> _slots;

		public ShipModel_Attacker Attacker
		{
			get
			{
				return this._attacker;
			}
		}

		public BattleAttackKind AttackType
		{
			get
			{
				return this._attack_type;
			}
		}

		public HougekiModel(Hougeki<BattleAtackKinds_Day> data, Dictionary<int, ShipModel_BattleAll> ships) : base(ships.get_Item(data.Target.get_Item(0)))
		{
			this._attack_type = this._convertAttackTypeEnum(data.SpType);
			this._initialize<BattleAtackKinds_Day>(data, ships);
			this._SetDamageForDefender();
		}

		public HougekiModel(Hougeki<BattleAtackKinds_Night> data, Dictionary<int, ShipModel_BattleAll> ships) : base(ships.get_Item(data.Target.get_Item(0)))
		{
			this._is_night = true;
			this._attack_type = this._convertAttackTypeEnum(data.SpType);
			this._initialize<BattleAtackKinds_Night>(data, ships);
			this._SetDamageForDefender();
		}

		public int GetDamage(int index)
		{
			return this._damages.get_Item(index);
		}

		public BattleHitStatus GetHitState(int index)
		{
			return this._hitstates.get_Item(index);
		}

		public SlotitemModel_Battle[] GetSlotitems()
		{
			return this._slots.ToArray();
		}

		public SlotitemModel_Battle GetSlotitem()
		{
			return this._slots.get_Item(0);
		}

		public bool GetProtectEffect(int index)
		{
			return this._dmgkind.get_Item(index) == BattleDamageKinds.Rescue;
		}

		public override bool GetGurdEffect()
		{
			return base.GetHitState() != BattleHitStatus.Miss && base.GetDamage() == 0;
		}

		public bool GetGurdEffect(int index)
		{
			return this.GetHitState(index) != BattleHitStatus.Miss && this.GetDamage(index) == 0;
		}

		public bool GetRocketEffenct()
		{
			return this._attack_type == BattleAttackKind.Normal && this.Attacker.HasRocket() && base.Defender.IsGroundFacility();
		}

		public bool GetMihariEffect()
		{
			return this._is_night && (this._attack_type == BattleAttackKind.Syu_Syu_Syu || this._attack_type == BattleAttackKind.Syu_Syu_Fuku || this._attack_type == BattleAttackKind.Syu_Rai || this._attack_type == BattleAttackKind.Rai_Rai) && this.Attacker.HasMihari();
		}

		[Obsolete("GetProtectEffect を使用してください", false)]
		public bool IsShielded(int index)
		{
			return this._dmgkind.get_Item(index) == BattleDamageKinds.Rescue;
		}

		[Obsolete("GetProtectEffect を使用してください", false)]
		public bool IsShielded()
		{
			return this.IsShielded(0);
		}

		public bool IsNight()
		{
			return this._is_night;
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			if (is_friend == base.Defender.IsFriend())
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, DamagedStates damage_event)
		{
			if (is_friend == base.Defender.IsFriend() && base.Defender.DamageEventAfter == damage_event)
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public List<ShipModel_Defender> GetGekichinShips()
		{
			if (base.Defender.DamageEventAfter == DamagedStates.Gekichin || base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami)
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public List<ShipModel_Defender> GetGekichinShips(bool is_friend)
		{
			if ((base.Defender.DamageEventAfter == DamagedStates.Gekichin || base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami) && base.Defender.IsFriend())
			{
				List<ShipModel_Defender> list = new List<ShipModel_Defender>();
				list.Add(base.Defender);
				return list;
			}
			return new List<ShipModel_Defender>();
		}

		public bool HasChuhaEvent()
		{
			return this.HasChuhaEvent(true);
		}

		public bool HasChuhaEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && base.Defender.DamageEventAfter == DamagedStates.Tyuuha;
		}

		public bool HasTaihaEvent()
		{
			return this.HasTaihaEvent(true);
		}

		public bool HasTaihaEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && base.Defender.DamageEventAfter == DamagedStates.Taiha;
		}

		public bool HasGekichinEvent()
		{
			return this.HasGekichinEvent(true);
		}

		public bool HasGekichinEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && (base.Defender.DamageEventAfter == DamagedStates.Gekichin || base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami);
		}

		public bool HasRecoveryEvent()
		{
			return this.HasRecoveryEvent(true);
		}

		public bool HasRecoveryEvent(bool is_friend)
		{
			return is_friend == base.Defender.IsFriend() && (base.Defender.DamageEventAfter == DamagedStates.Youin || base.Defender.DamageEventAfter == DamagedStates.Megami);
		}

		public int __GetDamage__(int index)
		{
			return this._calc_damages.get_Item(index);
		}

		private void _initialize<T>(Hougeki<T> data, Dictionary<int, ShipModel_BattleAll> ships) where T : IConvertible
		{
			this._attacker = ships.get_Item(data.Attacker).__CreateAttacker__();
			for (int i = 0; i < data.Target.get_Count(); i++)
			{
				base._AddData(data.Damage.get_Item(i), data.Clitical.get_Item(i), data.DamageKind.get_Item(i));
			}
			this._slots = new List<SlotitemModel_Battle>();
			if (data.Slot_List != null)
			{
				for (int j = 0; j < data.Slot_List.get_Count(); j++)
				{
					int num = data.Slot_List.get_Item(j);
					if (num == 0)
					{
						this._slots.Add(null);
					}
					else
					{
						this._slots.Add(new SlotitemModel_Battle(num));
					}
				}
			}
			while (this._slots.get_Count() < 3)
			{
				this._slots.Add(null);
			}
			if (this.IsNight() && this.Attacker.IsAircraftCarrier() && this.Attacker.Yomi != "グラーフ・ツェッペリン")
			{
				this._attack_type = BattleAttackKind.AirAttack;
			}
			if (this.AttackType == BattleAttackKind.Normal)
			{
				this._setAttackSubType();
			}
			else if (this.AttackType == BattleAttackKind.AirAttack)
			{
				HougekiModel.<_initialize>c__AnonStorey537<T> <_initialize>c__AnonStorey = new HougekiModel.<_initialize>c__AnonStorey537<T>();
				List<SlotitemModel_Battle> slotitemList = this.Attacker.SlotitemList;
				if (base.Defender.IsSubMarine())
				{
					HougekiModel.<_initialize>c__AnonStorey537<T> arg_1A7_0 = <_initialize>c__AnonStorey;
					List<int> list = new List<int>();
					list.Add(7);
					list.Add(8);
					list.Add(11);
					list.Add(25);
					list.Add(26);
					list.Add(41);
					arg_1A7_0.enable_type3 = list;
					this._slots.set_Item(0, slotitemList.Find((SlotitemModel_Battle slot) => <_initialize>c__AnonStorey.enable_type3.IndexOf(slot.Type3) >= 0 && slot.Taisen > 0));
				}
				else
				{
					HougekiModel.<_initialize>c__AnonStorey537<T> arg_1EB_0 = <_initialize>c__AnonStorey;
					List<int> list = new List<int>();
					list.Add(7);
					list.Add(8);
					arg_1EB_0.enable_type3 = list;
					this._slots.set_Item(0, slotitemList.Find((SlotitemModel_Battle slot) => <_initialize>c__AnonStorey.enable_type3.IndexOf(slot.Type3) >= 0 && (slot.Bakugeki > 0 || slot.Raigeki > 0)));
				}
			}
		}

		private void _setAttackSubType()
		{
			SType shipType = (SType)this.Attacker.ShipType;
			SlotitemModel_Battle slotitem = this.GetSlotitem();
			if (slotitem != null && (slotitem.Type3 == 5 || slotitem.Type3 == 32))
			{
				this._attack_type = BattleAttackKind.Gyorai;
			}
			else if (slotitem != null)
			{
				if (slotitem.Type3 != 1 && slotitem.Type3 != 2 && slotitem.Type3 != 3 && slotitem.Type3 != 4 && slotitem.Type3 != 5 && slotitem.Type3 != 32)
				{
					List<SlotitemModel_Battle> list = new List<SlotitemModel_Battle>();
					list.Add(null);
					list.Add(null);
					list.Add(null);
					this._slots = list;
				}
			}
		}

		private BattleAttackKind _convertAttackTypeEnum(BattleAtackKinds_Day kind)
		{
			return (BattleAttackKind)kind;
		}

		private BattleAttackKind _convertAttackTypeEnum(BattleAtackKinds_Night kind)
		{
			string text = kind.ToString();
			return (BattleAttackKind)((int)Enum.Parse(typeof(BattleAttackKind), text));
		}

		private void _SetDamageForDefender()
		{
			if (this.AttackType == BattleAttackKind.Renzoku)
			{
				base._InitializeRengeki();
			}
			else
			{
				base._Initialize();
			}
		}

		public override string ToString()
		{
			string text = string.Format("{0}({1})の攻撃 {2}", this.Attacker.Name, this.Attacker.Index, this.AttackType);
			if (this.AttackType == BattleAttackKind.Normal || this.AttackType == BattleAttackKind.AirAttack || this.AttackType == BattleAttackKind.Bakurai)
			{
				SlotitemModel_Battle[] slotitems = this.GetSlotitems();
				text += string.Format("{0}({1})は", base.Defender.Name, base.Defender.Index);
				text += string.Format("{0}(c:{2})のダメージ({1})", this.GetDamage(0), this.GetHitState(0), this.__GetDamage__(0));
				text += string.Format("{0}", (!this.GetRocketEffenct()) ? string.Empty : "[対地演出]");
				text += string.Format("{0}", (!this.GetProtectEffect(0)) ? string.Empty : "[かばう]");
				text += string.Format("{0}", (!this.GetGurdEffect(0)) ? string.Empty : "[ガード]");
				for (int i = 0; i < slotitems.Length; i++)
				{
					if (slotitems[i] != null)
					{
						text += string.Format("{0}", slotitems[i]);
					}
					else
					{
						text += string.Format("[--]", new object[0]);
					}
				}
			}
			else if (this.AttackType == BattleAttackKind.Renzoku)
			{
				SlotitemModel_Battle[] slotitems2 = this.GetSlotitems();
				text += string.Format("\"{0}({1})は", base.Defender.Name, base.Defender.Index);
				text += string.Format("{0},{1}で", slotitems2[0], slotitems2[1]);
				text += string.Format("{0}({2})のダメージ({1})", this.GetDamage(0), this.GetHitState(0), this.__GetDamage__(0));
				text += string.Format("{0}", (!this.GetProtectEffect(0)) ? string.Empty : "[かばう]");
				text += string.Format("{0}\" ", (!this.GetGurdEffect(0)) ? string.Empty : "[ガード]");
				text += string.Format("と{0}({2})のダメージ({1})", this.GetDamage(1), this.GetHitState(1), this.__GetDamage__(1));
				text += string.Format("{0}\" ", (!this.GetProtectEffect(1)) ? string.Empty : "[かばう]");
				text += string.Format("{0}\" ", (!this.GetGurdEffect(1)) ? string.Empty : "[ガード]");
			}
			else if (this.AttackType == BattleAttackKind.Sp1 || this.AttackType == BattleAttackKind.Sp2 || this.AttackType == BattleAttackKind.Sp3 || this.AttackType == BattleAttackKind.Sp4)
			{
				SlotitemModel_Battle[] slotitems3 = this.GetSlotitems();
				text += string.Format("\"{0}({1})は", base.Defender.Name, base.Defender.Index);
				text += string.Format("{0},{1},{2}で", slotitems3[0], slotitems3[1], slotitems3[2]);
				text += string.Format("{0}({2})のダメージ({1})", this.GetDamage(0), this.GetHitState(0), this.__GetDamage__(0));
				text += string.Format("{0}\" ", (!this.GetProtectEffect(0)) ? string.Empty : "[かばう]");
				text += string.Format("{0}\" ", (!this.GetGurdEffect(0)) ? string.Empty : "[ガード]");
			}
			else if (this.AttackType == BattleAttackKind.Syu_Rai || this.AttackType == BattleAttackKind.Rai_Rai)
			{
				SlotitemModel_Battle[] slotitems4 = this.GetSlotitems();
				text += string.Format("\"{0}({1})は", base.Defender.Name, base.Defender.Index);
				text += string.Format("{0},{1}で", slotitems4[0], slotitems4[1]);
				text += string.Format("{0}({2})のダメージ({1})", this.GetDamage(0), this.GetHitState(0), this.__GetDamage__(0));
				text += string.Format("{0}\" ", (!this.GetProtectEffect(0)) ? string.Empty : "[かばう]");
				text += string.Format("{0}\" ", (!this.GetGurdEffect(0)) ? string.Empty : "[ガード]");
			}
			else if (this.AttackType == BattleAttackKind.Syu_Syu_Syu || this.AttackType == BattleAttackKind.Syu_Syu_Fuku)
			{
				SlotitemModel_Battle[] slotitems5 = this.GetSlotitems();
				text += string.Format("\"{0}({1})は", base.Defender.Name, base.Defender.Index);
				text += string.Format("{0},{1},{2}で", slotitems5[0], slotitems5[1], slotitems5[2]);
				text += string.Format("{0}({2})のダメージ({1})", this.GetDamage(0), this.GetHitState(0), this.__GetDamage__(0));
				text += string.Format("{0}\" ", (!this.GetProtectEffect(0)) ? string.Empty : "[かばう]");
				text += string.Format("{0}\" ", (!this.GetGurdEffect(0)) ? string.Empty : "[ガード]");
			}
			text += "\n";
			text += string.Format("Attacker:{0}\n", this.Attacker);
			return text + string.Format("Defender:{0}", base.Defender);
		}
	}
}
