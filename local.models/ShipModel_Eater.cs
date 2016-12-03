using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_Eater : ShipModel_Battle
	{
		private int _hp;

		private List<SlotitemModel_Battle> _slotitems_before;

		private SlotitemModel_Battle _slotitemex_before;

		private List<SlotitemModel_Battle> _slotitems_after_ration;

		private SlotitemModel_Battle _slotitemex_after_ration;

		private List<SlotitemModel_Battle> _slotitems_ration;

		public int Hp
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
				return base._GetDmgState(this._hp);
			}
		}

		public bool DamagedFlg
		{
			get
			{
				return base._GetDamagedFlg(this.DmgState);
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

		public List<SlotitemModel_Battle> SlotitemListAfterRation
		{
			get
			{
				return this._slotitems_after_ration;
			}
		}

		public SlotitemModel_Battle SlotitemExAfterRation
		{
			get
			{
				return this._slotitemex_after_ration;
			}
		}

		public List<SlotitemModel_Battle> SlotitemListRation
		{
			get
			{
				return this._slotitems_ration;
			}
		}

		public ShipModel_Eater(Mst_ship mst_data, __ShipModel_Battle_BaseData__ baseData, int hp, List<SlotitemModel_Battle> slotitems, SlotitemModel_Battle slotitemex)
		{
			this._mst_data = mst_data;
			this._base_data = baseData;
			this._hp = hp;
			this._slotitems_before = slotitems;
			this._slotitemex_before = slotitemex;
			this._init();
		}

		private void _init()
		{
			HashSet<int> hashSet = new HashSet<int>();
			this._slotitems_after_ration = new List<SlotitemModel_Battle>();
			this._slotitems_ration = new List<SlotitemModel_Battle>();
			int num = 43;
			SlotitemModel_Battle slotitemModel_Battle = this._slotitemex_before;
			if (slotitemModel_Battle != null && slotitemModel_Battle.Type3 == num && !hashSet.Contains(slotitemModel_Battle.MstId))
			{
				this._slotitems_ration.Add(slotitemModel_Battle);
				hashSet.Add(slotitemModel_Battle.MstId);
				this._slotitemex_after_ration = null;
			}
			else
			{
				this._slotitemex_after_ration = slotitemModel_Battle;
			}
			for (int i = this._slotitems_before.get_Count() - 1; i >= 0; i--)
			{
				slotitemModel_Battle = this._slotitems_before.get_Item(i);
				if (slotitemModel_Battle != null && slotitemModel_Battle.Type3 == num && !hashSet.Contains(slotitemModel_Battle.MstId))
				{
					this._slotitems_ration.Add(slotitemModel_Battle);
					hashSet.Add(slotitemModel_Battle.MstId);
				}
				else
				{
					this._slotitems_after_ration.Add(slotitemModel_Battle);
				}
			}
			this._slotitems_after_ration.Reverse();
			while (this._slotitems_after_ration.get_Count() < this.SlotCount)
			{
				this._slotitems_after_ration.Add(null);
			}
		}

		public override string ToString()
		{
			string text = string.Format("{0}(mstId:{1})[{2}/{3}({4})", new object[]
			{
				base.Name,
				base.MstId,
				this.Hp,
				this.MaxHp,
				this.DmgState
			});
			text += " ";
			text += base._ToString(this.SlotitemListBefore, this.SlotitemExBefore);
			text += " -> ";
			text += base._ToString(this.SlotitemListAfterRation, this.SlotitemExAfterRation);
			text += " ((";
			for (int i = 0; i < this.SlotitemListRation.get_Count(); i++)
			{
				SlotitemModel_Battle item = this.SlotitemListRation.get_Item(i);
				text += base._ToString(item);
			}
			return text + "))";
		}
	}
}
