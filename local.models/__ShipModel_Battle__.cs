using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class __ShipModel_Battle__ : ShipModel_Battle
	{
		protected int _hp;

		protected List<SlotitemModel_Battle> _slotitems;

		protected SlotitemModel_Battle _slotitemex;

		public virtual int Hp
		{
			get
			{
				return base._GetHp(this._hp);
			}
		}

		public DamageState_Battle DmgState
		{
			get
			{
				return base._GetDmgState(this.Hp);
			}
		}

		public bool DamagedFlg
		{
			get
			{
				return base._GetDamagedFlg(this.DmgState);
			}
		}

		public List<SlotitemModel_Battle> SlotitemList
		{
			get
			{
				return this._slotitems.GetRange(0, this._slotitems.get_Count());
			}
		}

		public SlotitemModel_Battle SlotitemEx
		{
			get
			{
				return this._slotitemex;
			}
		}

		public __ShipModel_Battle__(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex)
		{
			this._mst_data = mst_data;
			this._base_data = baseData;
			this._hp = hp;
			this._slotitems = slotitems;
			this._slotitemex = slotitemex;
		}

		public override string ToString()
		{
			string text = string.Format("{0}(mstId:{1})[{2}/{3}]", new object[]
			{
				base.Name,
				base.MstId,
				this.Hp,
				this.MaxHp
			});
			return text + base._ToString(this.SlotitemList, this.SlotitemEx);
		}
	}
}
