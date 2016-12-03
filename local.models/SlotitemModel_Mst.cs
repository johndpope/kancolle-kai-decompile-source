using Common.Enum;
using local.utils;
using Server_Models;
using System;

namespace local.models
{
	public class SlotitemModel_Mst
	{
		protected Mst_slotitem _mst;

		public virtual int MstId
		{
			get
			{
				return this._mst.Id;
			}
		}

		public string Name
		{
			get
			{
				return this._mst.Name;
			}
		}

		public int Type1
		{
			get
			{
				return this._mst.Type1;
			}
		}

		public int Type2
		{
			get
			{
				return this._mst.Type2;
			}
		}

		public int Type3
		{
			get
			{
				return this._mst.Type3;
			}
		}

		public int Type4
		{
			get
			{
				return this._mst.Type4;
			}
		}

		public int Soukou
		{
			get
			{
				return this._mst.Souk;
			}
		}

		public int Hougeki
		{
			get
			{
				return this._mst.Houg;
			}
		}

		public int Raigeki
		{
			get
			{
				return this._mst.Raig;
			}
		}

		public int Bakugeki
		{
			get
			{
				return this._mst.Baku;
			}
		}

		public int Taikuu
		{
			get
			{
				return this._mst.Tyku;
			}
		}

		public int Taisen
		{
			get
			{
				return this._mst.Tais;
			}
		}

		public int HouMeityu
		{
			get
			{
				return this._mst.Houm;
			}
		}

		public int Kaihi
		{
			get
			{
				return this._mst.Houk;
			}
		}

		public int Sakuteki
		{
			get
			{
				return this._mst.Saku;
			}
		}

		public int Syatei
		{
			get
			{
				return this._mst.Leng;
			}
		}

		public int Rare
		{
			get
			{
				return this._mst.Rare;
			}
		}

		public int BrokenFuel
		{
			get
			{
				return this._mst.Broken1;
			}
		}

		public int BrokenAmmo
		{
			get
			{
				return this._mst.Broken2;
			}
		}

		public int BrokenSteel
		{
			get
			{
				return this._mst.Broken3;
			}
		}

		public int BrokenBaux
		{
			get
			{
				return this._mst.Broken4;
			}
		}

		public virtual string ShortName
		{
			get
			{
				return string.Format("{0}(MstId:{1}, Cost:{2}{3})", new object[]
				{
					this.Name,
					this.MstId,
					this.GetCost(),
					(!this.IsPlane()) ? string.Empty : "[艦載機]"
				});
			}
		}

		public SlotitemModel_Mst(int mst_id)
		{
			this._mst = Mst_DataManager.Instance.Mst_Slotitem.get_Item(mst_id);
		}

		public SlotitemModel_Mst(Mst_slotitem mst)
		{
			this._mst = mst;
		}

		public int GetGraphicId()
		{
			return Utils.GetSlotitemGraphicId(this.MstId);
		}

		public int GetCost()
		{
			Mst_slotitem_cost mst_slotitem_cost;
			if (!Mst_DataManager.Instance.Mst_slotitem_cost.TryGetValue(this.MstId, ref mst_slotitem_cost))
			{
				return 0;
			}
			return mst_slotitem_cost.Cost;
		}

		public bool IsPlane()
		{
			SlotitemCategory slotitem_type = Mst_DataManager.Instance.Mst_equip_category.get_Item(this.Type3).Slotitem_type;
			return slotitem_type == SlotitemCategory.Kanjouki || slotitem_type == SlotitemCategory.Suijouki;
		}

		[Obsolete("IsPlane()引数無しを使用してください", false)]
		public bool IsPlane(bool dummy)
		{
			return this.IsPlane();
		}

		public override string ToString()
		{
			return string.Format("[{0}]", this.ShortName);
		}
	}
}
