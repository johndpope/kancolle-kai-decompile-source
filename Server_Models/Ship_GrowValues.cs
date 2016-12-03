using System;
using System.Collections.Generic;

namespace Server_Models
{
	public class Ship_GrowValues
	{
		public int Maxhp;

		public int Houg;

		public int Raig;

		public int Taiku;

		public int Soukou;

		public int Kaihi;

		public int Taisen;

		public int Sakuteki;

		public int Luck;

		private Ship_GrowValues()
		{
		}

		public Ship_GrowValues(Mst_ship mst_data, int level, Dictionary<Mem_ship.enumKyoukaIdx, int> kyoukaValue)
		{
			this.changeLimitKyoukaValue(mst_data, kyoukaValue);
			this.Maxhp = mst_data.Taik + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Taik) + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup);
			this.Houg = mst_data.Houg + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Houg);
			this.Raig = mst_data.Raig + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Raig);
			this.Taiku = mst_data.Tyku + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Tyku);
			this.Soukou = mst_data.Souk + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Souk);
			this.Kaihi = mst_data.Kaih + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Kaihi);
			this.Taisen = mst_data.Tais + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Taisen);
			this.Sakuteki = (int)((float)mst_data.Saku + (float)((mst_data.Saku_max - mst_data.Saku) * level) / 99f);
			if (this.Sakuteki > mst_data.Saku_max)
			{
				this.Sakuteki = mst_data.Saku_max;
			}
			this.Luck = mst_data.Luck + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Luck);
		}

		public Ship_GrowValues Copy()
		{
			return new Ship_GrowValues
			{
				Maxhp = this.Maxhp,
				Houg = this.Houg,
				Raig = this.Raig,
				Taiku = this.Taiku,
				Soukou = this.Soukou,
				Kaihi = this.Kaihi,
				Taisen = this.Taisen,
				Sakuteki = this.Sakuteki,
				Luck = this.Luck
			};
		}

		private void changeLimitKyoukaValue(Mst_ship mst_data, Dictionary<Mem_ship.enumKyoukaIdx, int> kyoukaValue)
		{
			if (mst_data.Houg + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Houg) > mst_data.Houg_max)
			{
				kyoukaValue.set_Item(Mem_ship.enumKyoukaIdx.Houg, mst_data.Houg_max - mst_data.Houg);
			}
			if (mst_data.Raig + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Raig) > mst_data.Raig_max)
			{
				kyoukaValue.set_Item(Mem_ship.enumKyoukaIdx.Raig, mst_data.Raig_max - mst_data.Raig);
			}
			if (mst_data.Tyku + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Tyku) > mst_data.Tyku_max)
			{
				kyoukaValue.set_Item(Mem_ship.enumKyoukaIdx.Tyku, mst_data.Tyku_max - mst_data.Tyku);
			}
			if (mst_data.Souk + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Souk) > mst_data.Souk_max)
			{
				kyoukaValue.set_Item(Mem_ship.enumKyoukaIdx.Souk, mst_data.Souk_max - mst_data.Souk);
			}
			if (mst_data.Kaih + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Kaihi) > mst_data.Kaih_max)
			{
				kyoukaValue.set_Item(Mem_ship.enumKyoukaIdx.Kaihi, mst_data.Kaih_max - mst_data.Kaih);
			}
			if (mst_data.Tais + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Taisen) > mst_data.Tais_max)
			{
				kyoukaValue.set_Item(Mem_ship.enumKyoukaIdx.Taisen, mst_data.Tais_max - mst_data.Tais);
			}
			if (mst_data.Luck + kyoukaValue.get_Item(Mem_ship.enumKyoukaIdx.Luck) > mst_data.Luck_max)
			{
				kyoukaValue.set_Item(Mem_ship.enumKyoukaIdx.Luck, mst_data.Luck_max - mst_data.Luck);
			}
		}
	}
}
