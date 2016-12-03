using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Struct
{
	public struct SlotSetInfo
	{
		public int Soukou;

		public int Karyoku;

		public int Raisou;

		public int Taiku;

		public int Taisen;

		public int Houm;

		public int Kaihi;

		public int Sakuteki;

		public int Leng;

		public void SetSlot(int now_leng, Mst_slotitem set_item)
		{
			this.Karyoku = set_item.Houg;
			this.Raisou = set_item.Raig;
			this.Sakuteki = set_item.Saku;
			this.Soukou = set_item.Souk;
			this.Taiku = set_item.Tyku;
			this.Taisen = set_item.Tais;
			this.Houm = set_item.Houm;
			this.Kaihi = set_item.Houk;
		}

		public void UnsetSlot(Mst_slotitem unset_item)
		{
			this.Karyoku -= unset_item.Houg;
			this.Raisou -= unset_item.Raig;
			this.Sakuteki -= unset_item.Saku;
			this.Soukou -= unset_item.Souk;
			this.Taiku -= unset_item.Tyku;
			this.Taisen -= unset_item.Tais;
			this.Houm -= unset_item.Houm;
			this.Kaihi -= unset_item.Houk;
		}

		public void ChangeSlot(Mst_slotitem pre_item, Mst_slotitem after_item)
		{
			this.Karyoku = after_item.Houg - pre_item.Houg;
			this.Raisou = after_item.Raig - pre_item.Raig;
			this.Sakuteki = after_item.Saku - pre_item.Saku;
			this.Soukou = after_item.Souk - pre_item.Souk;
			this.Taiku = after_item.Tyku - pre_item.Tyku;
			this.Taisen = after_item.Tais - pre_item.Tais;
			this.Houm = after_item.Houm - pre_item.Houm;
			this.Kaihi = after_item.Houk - pre_item.Houk;
		}

		public void SetLeng(int nowLeng, List<int> lengList)
		{
			int num = Enumerable.Max(lengList);
			this.Leng = num - nowLeng;
		}
	}
}
