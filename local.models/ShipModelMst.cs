using Common.Enum;
using local.utils;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.models
{
	public class ShipModelMst
	{
		protected Mst_ship _mst_data;

		public int MstId
		{
			get
			{
				return this._mst_data.Id;
			}
		}

		public string Name
		{
			get
			{
				return this._mst_data.Name;
			}
		}

		public string Yomi
		{
			get
			{
				return this._mst_data.Yomi;
			}
		}

		public virtual int ShipType
		{
			get
			{
				return this._mst_data.Stype;
			}
		}

		public string ShipTypeName
		{
			get
			{
				return Mst_DataManager.Instance.Mst_stype.get_Item(this.ShipType).Name;
			}
		}

		public int BuildStep
		{
			get
			{
				return Mst_DataManager.Instance.Mst_stype.get_Item(this.ShipType).Kcnt;
			}
		}

		public int ClassType
		{
			get
			{
				return this._mst_data.Ctype;
			}
		}

		public int ClassNum
		{
			get
			{
				return this._mst_data.Cnum;
			}
		}

		public int Rare
		{
			get
			{
				return this._mst_data.Backs;
			}
		}

		public virtual int Taikyu
		{
			get
			{
				return this._mst_data.Taik;
			}
		}

		public virtual int Karyoku
		{
			get
			{
				return this._mst_data.Houg;
			}
		}

		public virtual int KaryokuMax
		{
			get
			{
				return this._mst_data.Houg_max;
			}
		}

		public virtual int Raisou
		{
			get
			{
				return this._mst_data.Raig;
			}
		}

		public virtual int RaisouMax
		{
			get
			{
				return this._mst_data.Raig_max;
			}
		}

		public virtual int Soukou
		{
			get
			{
				return this._mst_data.Souk;
			}
		}

		public virtual int SoukouMax
		{
			get
			{
				return this._mst_data.Souk_max;
			}
		}

		public virtual int Taiku
		{
			get
			{
				return this._mst_data.Tyku;
			}
		}

		public virtual int TaikuMax
		{
			get
			{
				return this._mst_data.Tyku_max;
			}
		}

		public virtual int Kaihi
		{
			get
			{
				return this._mst_data.Kaih;
			}
		}

		public virtual int KaihiMax
		{
			get
			{
				return this._mst_data.Kaih_max;
			}
		}

		public virtual int Taisen
		{
			get
			{
				return this._mst_data.Tais;
			}
		}

		public virtual int TaisenMax
		{
			get
			{
				return this._mst_data.Tais_max;
			}
		}

		public virtual int Lucky
		{
			get
			{
				return this._mst_data.Luck;
			}
		}

		public virtual int LuckyMax
		{
			get
			{
				return this._mst_data.Luck_max;
			}
		}

		public int TousaiMaxAll
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.SlotCount; i++)
				{
					num += this._mst_data.Maxeq.get_Item(i);
				}
				return num;
			}
		}

		public int[] TousaiMax
		{
			get
			{
				return this._mst_data.Maxeq.GetRange(0, this.SlotCount).ToArray();
			}
		}

		public int Soku
		{
			get
			{
				return this._mst_data.Soku;
			}
		}

		public virtual int Leng
		{
			get
			{
				return this._mst_data.Leng;
			}
		}

		public virtual int SlotCount
		{
			get
			{
				return this._mst_data.Slot_num;
			}
		}

		public int FuelMax
		{
			get
			{
				return this._mst_data.Fuel_max;
			}
		}

		public int AmmoMax
		{
			get
			{
				return this._mst_data.Bull_max;
			}
		}

		public int AfterLevel
		{
			get
			{
				return this._mst_data.Afterlv;
			}
		}

		public int AfterAmmo
		{
			get
			{
				return this._mst_data.Afterbull;
			}
		}

		public int AfterSteel
		{
			get
			{
				return this._mst_data.Afterfuel;
			}
		}

		public int AfterDevkit
		{
			get
			{
				return this._mst_data.GetRemodelDevKitNum();
			}
		}

		public int AfterShipId
		{
			get
			{
				return this._mst_data.Aftershipid;
			}
		}

		public int PowUpKaryoku
		{
			get
			{
				return this._mst_data.Powup1;
			}
		}

		public int PowUpRaisou
		{
			get
			{
				return this._mst_data.Powup2;
			}
		}

		public int PowUpTaikuu
		{
			get
			{
				return this._mst_data.Powup3;
			}
		}

		public int PowUpSoukou
		{
			get
			{
				return this._mst_data.Powup4;
			}
		}

		public int PowUpLucky
		{
			get
			{
				return (this._mst_data.GetLuckUpKeisu() <= 0.0) ? 0 : 1;
			}
		}

		public ShipOffset Offsets
		{
			get
			{
				return new ShipOffset(this.GetGraphicsMstId());
			}
		}

		protected ShipModelMst()
		{
		}

		public ShipModelMst(int mst_id)
		{
			this._mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mst_id);
		}

		public ShipModelMst(Mst_ship mst)
		{
			this._mst_data = mst;
		}

		public BtpType GetBTPType()
		{
			return this._mst_data.GetBtpType();
		}

		public bool IsGroundFacility()
		{
			return this._mst_data.Soku == 0;
		}

		public List<SlotitemCategory> GetEquipCategory()
		{
			List<SlotitemCategory> list = new List<SlotitemCategory>();
			Dictionary<int, Mst_equip_category> mst_equip_category = Mst_DataManager.Instance.Mst_equip_category;
			List<int> equipList = this._mst_data.GetEquipList();
			using (List<int>.Enumerator enumerator = equipList.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					SlotitemCategory slotitem_type = mst_equip_category.get_Item(current).Slotitem_type;
					list.Add(slotitem_type);
				}
			}
			return Enumerable.ToList<SlotitemCategory>(Enumerable.Distinct<SlotitemCategory>(list));
		}

		public int GetGraphicsMstId()
		{
			return Utils.GetResourceMstId(this.MstId);
		}

		public int GetVoiceMstId(int voice_id)
		{
			return Utils.GetVoiceMstId(this.MstId, voice_id);
		}
	}
}
