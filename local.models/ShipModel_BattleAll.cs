using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_BattleAll : ShipModel_Battle
	{
		private List<ShipModel_Battle> _record;

		public int HpStart
		{
			get
			{
				return base._GetHp(this._base_data.Fmt.NowHp);
			}
		}

		public int HpPhaseStart
		{
			get
			{
				return ((ShipModel_BattleStart)this._record.FindLast((ShipModel_Battle s) => s is ShipModel_BattleStart)).Hp;
			}
		}

		public int HpEnd
		{
			get
			{
				List<ShipModel_Defender> list = this._record.FindAll((ShipModel_Battle s) => s is ShipModel_Defender).ConvertAll<ShipModel_Defender>((ShipModel_Battle s) => (ShipModel_Defender)s);
				if (list.get_Count() == 0)
				{
					return this.HpStart;
				}
				return list.get_Item(list.get_Count() - 1).HpAfterRecovery;
			}
		}

		public DamageState_Battle DmgStateStart
		{
			get
			{
				return base._GetDmgState(this.HpStart);
			}
		}

		public DamageState_Battle DmgStatePhaseStart
		{
			get
			{
				return base._GetDmgState(this.HpPhaseStart);
			}
		}

		public DamageState_Battle DmgStateEnd
		{
			get
			{
				return base._GetDmgState(this.HpEnd);
			}
		}

		public bool DamagedFlgStart
		{
			get
			{
				return base._GetDamagedFlg(this.DmgStateStart);
			}
		}

		public bool DamagedFlgPhaseStart
		{
			get
			{
				return base._GetDamagedFlg(this.DmgStatePhaseStart);
			}
		}

		public bool DamagedFlgEnd
		{
			get
			{
				return base._GetDamagedFlg(this.DmgStateEnd);
			}
		}

		public List<SlotitemModel_Battle> SlotitemListStart
		{
			get
			{
				return ((ShipModel_BattleStart)this._record.Find((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemList;
			}
		}

		public SlotitemModel_Battle SlotitemExStart
		{
			get
			{
				return ((ShipModel_BattleStart)this._record.Find((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemEx;
			}
		}

		public List<SlotitemModel_Battle> SlotitemListPhaseStart
		{
			get
			{
				return ((ShipModel_BattleStart)this._record.FindLast((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemList;
			}
		}

		public SlotitemModel_Battle SlotitemExPhaseStart
		{
			get
			{
				return ((ShipModel_BattleStart)this._record.FindLast((ShipModel_Battle s) => s is ShipModel_BattleStart)).SlotitemEx;
			}
		}

		public List<SlotitemModel_Battle> SlotitemListEnd
		{
			get
			{
				ShipModel_Battle shipModel_Battle = this._record.get_Item(this._record.get_Count() - 1);
				if (shipModel_Battle is ShipModel_BattleStart || shipModel_Battle is ShipModel_Attacker)
				{
					return ((__ShipModel_Battle__)shipModel_Battle).SlotitemList;
				}
				if (shipModel_Battle is ShipModel_Defender)
				{
					return ((ShipModel_Defender)shipModel_Battle).SlotitemListAfterRecovery;
				}
				if (shipModel_Battle is ShipModel_Eater)
				{
					return ((ShipModel_Eater)shipModel_Battle).SlotitemListAfterRation;
				}
				return null;
			}
		}

		public SlotitemModel_Battle SlotitemExEnd
		{
			get
			{
				ShipModel_Battle shipModel_Battle = this._record.get_Item(this._record.get_Count() - 1);
				if (shipModel_Battle is ShipModel_BattleStart || shipModel_Battle is ShipModel_Attacker)
				{
					return ((__ShipModel_Battle__)shipModel_Battle).SlotitemEx;
				}
				if (shipModel_Battle is ShipModel_Defender)
				{
					return ((ShipModel_Defender)shipModel_Battle).SlotitemExAfterRecovery;
				}
				if (shipModel_Battle is ShipModel_Eater)
				{
					return ((ShipModel_Eater)shipModel_Battle).SlotitemExAfterRation;
				}
				return null;
			}
		}

		public ShipModel_BattleAll(BattleShipFmt fmt, int index, bool is_friend, bool practice)
		{
			Mst_DataManager.Instance.Mst_ship.TryGetValue(fmt.ShipId, ref this._mst_data);
			this._base_data = new __ShipModel_Battle_BaseData__();
			this._base_data.IsPractice = practice;
			this._base_data.IsFriend = is_friend;
			this._base_data.Index = index;
			this._base_data.Fmt = fmt;
			this._Init();
		}

		public ShipModel_BattleAll(Mst_ship mst_ship, __ShipModel_Battle_BaseData__ baseData)
		{
			this._mst_data = mst_ship;
			this._base_data = baseData;
			this._Init();
		}

		public bool HasRecoverYouin()
		{
			if (this.SlotitemExEnd != null && this.SlotitemExEnd.MstId == 42)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = this.SlotitemListEnd.Find((SlotitemModel_Battle item) => item != null && item.MstId == 42);
			return slotitemModel_Battle != null;
		}

		public bool HasRecoverMegami()
		{
			if (this.SlotitemExEnd != null && this.SlotitemExEnd.MstId == 43)
			{
				return true;
			}
			SlotitemModel_Battle slotitemModel_Battle = this.SlotitemListEnd.Find((SlotitemModel_Battle item) => item != null && item.MstId == 43);
			return slotitemModel_Battle != null;
		}

		public ShipRecoveryType IsUseRecoverySlotitemAtFirstCombat()
		{
			int num = this._record.FindLastIndex((ShipModel_Battle s) => s is ShipModel_BattleStart);
			if (num > 0)
			{
				List<ShipModel_Battle> range = this._record.GetRange(0, num);
				return this._IsUseRecoverySlotitem(range);
			}
			return this._IsUseRecoverySlotitem(this._record);
		}

		public ShipRecoveryType IsUseRecoverySlotitemAtSecondCombat()
		{
			int num = this._record.FindLastIndex((ShipModel_Battle s) => s is ShipModel_BattleStart);
			if (num > 0)
			{
				List<ShipModel_Battle> range = this._record.GetRange(num, this._record.get_Count() - num);
				return this._IsUseRecoverySlotitem(range);
			}
			return ShipRecoveryType.None;
		}

		public ShipRecoveryType IsUseRecoverySlotitem()
		{
			return this._IsUseRecoverySlotitem(this._record);
		}

		public void __CreateStarter__()
		{
			ShipModel_BattleStart shipModel_BattleStart = new ShipModel_BattleStart(this._mst_data, this._base_data, this.HpEnd, this.SlotitemListEnd, this.SlotitemExEnd);
			this._record.Add(shipModel_BattleStart);
		}

		public ShipModel_Attacker __CreateAttacker__()
		{
			ShipModel_Attacker shipModel_Attacker = new ShipModel_Attacker(this._mst_data, this._base_data, this.HpEnd, this.SlotitemListEnd, this.SlotitemExEnd);
			this._record.Add(shipModel_Attacker);
			return shipModel_Attacker;
		}

		public ShipModel_Defender __CreateDefender__()
		{
			ShipModel_Defender shipModel_Defender = new ShipModel_Defender(this._mst_data, this._base_data, this.HpEnd, this.SlotitemListEnd, this.SlotitemExEnd);
			this._record.Add(shipModel_Defender);
			return shipModel_Defender;
		}

		public ShipModel_Eater __CreateEater__()
		{
			ShipModel_Eater shipModel_Eater = new ShipModel_Eater(this._mst_data, this._base_data, this.HpEnd, this.SlotitemListEnd, this.SlotitemExEnd);
			this._record.Add(shipModel_Eater);
			return shipModel_Eater;
		}

		public void __UpdateEscapeStatus__(bool value)
		{
			this._base_data.Fmt.EscapeFlag = value;
		}

		private void _Init()
		{
			this._record = new List<ShipModel_Battle>();
			List<SlotitemModel_Battle> list = new List<SlotitemModel_Battle>();
			for (int i = 0; i < this._base_data.Fmt.Slot.get_Count(); i++)
			{
				if (this._base_data.Fmt.Slot.get_Item(i) > 0)
				{
					list.Add(new SlotitemModel_Battle(this._base_data.Fmt.Slot.get_Item(i)));
				}
				else
				{
					list.Add(null);
				}
			}
			while (list.get_Count() < this.SlotCount)
			{
				list.Add(null);
			}
			SlotitemModel_Battle slotitemex = null;
			if (base.HasSlotEx())
			{
				int exSlot = this._base_data.Fmt.ExSlot;
				Mst_slotitem mst;
				if (Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(exSlot, ref mst))
				{
					slotitemex = new SlotitemModel_Battle(mst);
				}
			}
			ShipModel_BattleStart shipModel_BattleStart = new ShipModel_BattleStart(this._mst_data, this._base_data, this.HpEnd, list, slotitemex);
			this._record.Add(shipModel_BattleStart);
		}

		private ShipRecoveryType _IsUseRecoverySlotitem(List<ShipModel_Battle> record)
		{
			for (int i = 0; i < record.get_Count(); i++)
			{
				if (this._record.get_Item(i) is ShipModel_Defender)
				{
					ShipModel_Defender shipModel_Defender = (ShipModel_Defender)record.get_Item(i);
					if (shipModel_Defender.DamageEventAfter == DamagedStates.Youin)
					{
						return ShipRecoveryType.Personnel;
					}
					if (shipModel_Defender.DamageEventAfter == DamagedStates.Megami)
					{
						return ShipRecoveryType.Goddes;
					}
				}
			}
			return ShipRecoveryType.None;
		}

		public override string ToString()
		{
			return string.Format("{0}({6})(mstId:{1})[{2}/{3} => {4}/{3}({5}/{3})]", new object[]
			{
				base.Name,
				base.MstId,
				this.HpPhaseStart,
				this.MaxHp,
				this.HpEnd,
				this.HpStart,
				base.Index
			});
		}
	}
}
