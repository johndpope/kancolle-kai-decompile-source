using Common.Enum;
using Common.Struct;
using Server_Common;
using Server_Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Models
{
	public class Mem_ship : Model_Base, IReqNewGetNo
	{
		public enum enumKyoukaIdx
		{
			Houg,
			Raig,
			Tyku,
			Souk,
			Kaihi,
			Taisen,
			Taik,
			Taik_Powerup,
			Luck
		}

		public enum BlingKind
		{
			None,
			Bling,
			PortBack,
			WaitDeck = 11,
			WaitEscort
		}

		public const int C_TAIK_POWERUP_MAX = 3;

		public const int COND_MIN = 0;

		public const int COND_MAX = 255;

		public const int COND_DEFAULT = 40;

		public const int COND_RATION_MAX = 100;

		public const int LOV_DEFAULT = 50;

		public const int LOV_MIN = 0;

		public const int LOV_MAX = 999;

		public const int LOV_MAX_FRONT_TOUCH_PLUS = 5;

		public const int LOV_MAX_FRONT_TOUCH_MINUS = -10;

		public const int LOV_MAX_BACK_TOUCH_PLUS = 7;

		public const int LOV_MAX_BACK_TOUCH_MINUS = -10;

		public const int EXSLOT_NOT_OPEN = -2;

		private int _rid;

		private int _getNo;

		private int _sortno;

		private int _ship_id;

		private int _level;

		private int _exp;

		private int _nowhp;

		private int _maxhp;

		private int _leng;

		private List<int> _slot;

		private List<int> _onslot;

		private int _exslot;

		private Dictionary<Mem_ship.enumKyoukaIdx, int> _kyouka;

		private int _backs;

		private int _fuel;

		private int _bull;

		private int _slotnum;

		private int _cond;

		private int _houg;

		private int _houg_max;

		private int _raig;

		private int _raig_max;

		private int _taiku;

		private int _taiku_max;

		private int _soukou;

		private int _soukou_max;

		private int _houm;

		private int _kaihi;

		private int _kaihi_max;

		private int _taisen;

		private int _taisen_max;

		private int _sakuteki;

		private int _sakuteki_max;

		private int _luck;

		private int _luck_max;

		private int _locked;

		private int _stype;

		private bool _escape_sts;

		private Mem_ship.BlingKind _blingType;

		private int _blingWaitArea;

		private int _bling_start;

		private int _bling_end;

		private int _lov;

		private List<int> _lov_front_value;

		private Dictionary<byte, byte> _lov_front_processed;

		private List<int> _lov_back_value;

		private Dictionary<byte, byte> _lov_back_processed;

		private List<BattleCommand> battleCommand;

		private Ship_GrowValues BattleBaseParam;

		private int beforeRequireExp;

		private int nextRequireExp;

		private int requiereExp;

		private bool ememy_flag;

		private static string _tableName = "mem_ship";

		public int Rid
		{
			get
			{
				return this._rid;
			}
			private set
			{
				this._rid = value;
			}
		}

		public int GetNo
		{
			get
			{
				return this._getNo;
			}
			private set
			{
				this._getNo = value;
			}
		}

		public int Sortno
		{
			get
			{
				return this._sortno;
			}
			private set
			{
				this._sortno = value;
			}
		}

		public int Ship_id
		{
			get
			{
				return this._ship_id;
			}
			private set
			{
				this._ship_id = value;
			}
		}

		public int Level
		{
			get
			{
				return this._level;
			}
			private set
			{
				this._level = value;
			}
		}

		public int Exp
		{
			get
			{
				return this._exp;
			}
			private set
			{
				this._exp = value;
			}
		}

		public int Exp_next
		{
			get
			{
				return this.getNextExp();
			}
		}

		public int Exp_rate
		{
			get
			{
				return this.getNextExpRate();
			}
		}

		public int Nowhp
		{
			get
			{
				return this._nowhp;
			}
			private set
			{
				this._nowhp = value;
			}
		}

		public int Maxhp
		{
			get
			{
				return this._maxhp;
			}
			private set
			{
				this._maxhp = value;
			}
		}

		public int Leng
		{
			get
			{
				return this._leng;
			}
			private set
			{
				this._leng = value;
			}
		}

		public List<int> Slot
		{
			get
			{
				return this._slot;
			}
			private set
			{
				this._slot = value;
			}
		}

		public List<int> Onslot
		{
			get
			{
				return this._onslot;
			}
			private set
			{
				this._onslot = value;
			}
		}

		public int Exslot
		{
			get
			{
				return this._exslot;
			}
			private set
			{
				this._exslot = value;
			}
		}

		public Dictionary<Mem_ship.enumKyoukaIdx, int> Kyouka
		{
			get
			{
				return this._kyouka;
			}
			private set
			{
				this._kyouka = value;
			}
		}

		public int Backs
		{
			get
			{
				return this._backs;
			}
			private set
			{
				this._backs = value;
			}
		}

		public int Fuel
		{
			get
			{
				return this._fuel;
			}
			private set
			{
				this._fuel = value;
			}
		}

		public int Bull
		{
			get
			{
				return this._bull;
			}
			private set
			{
				this._bull = value;
			}
		}

		public int Slotnum
		{
			get
			{
				return this._slotnum;
			}
			private set
			{
				this._slotnum = value;
			}
		}

		public int Cond
		{
			get
			{
				return this._cond;
			}
			private set
			{
				this._cond = value;
			}
		}

		public int Houg
		{
			get
			{
				return this._houg;
			}
			private set
			{
				this._houg = value;
			}
		}

		public int Houg_max
		{
			get
			{
				return this._houg_max;
			}
			private set
			{
				this._houg_max = value;
			}
		}

		public int Raig
		{
			get
			{
				return this._raig;
			}
			private set
			{
				this._raig = value;
			}
		}

		public int Raig_max
		{
			get
			{
				return this._raig_max;
			}
			private set
			{
				this._raig_max = value;
			}
		}

		public int Taiku
		{
			get
			{
				return this._taiku;
			}
			private set
			{
				this._taiku = value;
			}
		}

		public int Taiku_max
		{
			get
			{
				return this._taiku_max;
			}
			private set
			{
				this._taiku_max = value;
			}
		}

		public int Soukou
		{
			get
			{
				return this._soukou;
			}
			private set
			{
				this._soukou = value;
			}
		}

		public int Soukou_max
		{
			get
			{
				return this._soukou_max;
			}
			private set
			{
				this._soukou_max = value;
			}
		}

		public int Houm
		{
			get
			{
				return this._houm;
			}
			private set
			{
				this._houm = value;
			}
		}

		public int Kaihi
		{
			get
			{
				return this._kaihi;
			}
			private set
			{
				this._kaihi = value;
			}
		}

		public int Kaihi_max
		{
			get
			{
				return this._kaihi_max;
			}
			private set
			{
				this._kaihi_max = value;
			}
		}

		public int Taisen
		{
			get
			{
				return this._taisen;
			}
			private set
			{
				this._taisen = value;
			}
		}

		public int Taisen_max
		{
			get
			{
				return this._taisen_max;
			}
			private set
			{
				this._taisen_max = value;
			}
		}

		public int Sakuteki
		{
			get
			{
				return this._sakuteki;
			}
			private set
			{
				this._sakuteki = value;
			}
		}

		public int Sakuteki_max
		{
			get
			{
				return this._sakuteki_max;
			}
			private set
			{
				this._sakuteki_max = value;
			}
		}

		public int Luck
		{
			get
			{
				return this._luck;
			}
			private set
			{
				this._luck = value;
			}
		}

		public int Luck_max
		{
			get
			{
				return this._luck_max;
			}
			private set
			{
				this._luck_max = value;
			}
		}

		public int Locked
		{
			get
			{
				return this._locked;
			}
			private set
			{
				this._locked = value;
			}
		}

		public int Damage_Rate
		{
			get
			{
				return (int)((float)this.Nowhp / (float)this.Maxhp * 100f);
			}
		}

		public int Srate
		{
			get
			{
				return this.GetStarValue();
			}
		}

		public int Stype
		{
			get
			{
				return this._stype;
			}
			private set
			{
				this._stype = value;
			}
		}

		public bool Escape_sts
		{
			get
			{
				return this._escape_sts;
			}
			private set
			{
				this._escape_sts = value;
			}
		}

		public Mem_ship.BlingKind BlingType
		{
			get
			{
				return this._blingType;
			}
			private set
			{
				this._blingType = value;
			}
		}

		public int BlingWaitArea
		{
			get
			{
				return this._blingWaitArea;
			}
			private set
			{
				this._blingWaitArea = value;
			}
		}

		public int Bling_start
		{
			get
			{
				return this._bling_start;
			}
			private set
			{
				this._bling_start = value;
			}
		}

		public int Bling_end
		{
			get
			{
				return this._bling_end;
			}
			private set
			{
				this._bling_end = value;
			}
		}

		public int Lov
		{
			get
			{
				return this._lov;
			}
			private set
			{
				this._lov = value;
			}
		}

		public List<int> Lov_front_value
		{
			get
			{
				return this._lov_front_value;
			}
			private set
			{
				this._lov_front_value = value;
			}
		}

		public Dictionary<byte, byte> Lov_front_processed
		{
			get
			{
				return this._lov_front_processed;
			}
			private set
			{
				this._lov_front_processed = value;
			}
		}

		public List<int> Lov_back_value
		{
			get
			{
				return this._lov_back_value;
			}
			private set
			{
				this._lov_back_value = value;
			}
		}

		public Dictionary<byte, byte> Lov_back_processed
		{
			get
			{
				return this._lov_back_processed;
			}
			private set
			{
				this._lov_back_processed = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_ship._tableName;
			}
		}

		public Mem_ship()
		{
			this.Slot = new List<int>(5);
			this.Onslot = new List<int>(5);
			this.Exslot = -2;
			this.Kyouka = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			this.BlingWaitArea = 0;
			this.BlingType = Mem_ship.BlingKind.None;
			this.Lov_front_processed = new Dictionary<byte, byte>();
			this.Lov_back_processed = new Dictionary<byte, byte>();
			this.Lov_back_value = new List<int>();
			this.Lov_front_value = new List<int>();
		}

		public int GetSortNo()
		{
			return this._getNo;
		}

		public void ChangeSortNo(int no)
		{
			this._getNo = no;
		}

		public void SetRequireExp(int level, Dictionary<int, int> mst_level)
		{
			this.beforeRequireExp = ((!mst_level.ContainsKey(level - 1)) ? 0 : mst_level.get_Item(level - 1));
			this.nextRequireExp = ((!mst_level.ContainsKey(level + 1)) ? 0 : mst_level.get_Item(level + 1));
			this.requiereExp = mst_level.get_Item(level);
			if (this.beforeRequireExp == -1)
			{
				this.beforeRequireExp = 0;
			}
			if (this.nextRequireExp == -1)
			{
				this.nextRequireExp = 0;
			}
			if (this.requiereExp == -1)
			{
				this.requiereExp = 0;
			}
		}

		public bool IsOpenExSlot()
		{
			return this.Exslot != -2;
		}

		public bool Set_New_ShipData(int rid, int getNo, int mst_id)
		{
			if (!Mst_DataManager.Instance.Mst_ship.ContainsKey(mst_id))
			{
				return false;
			}
			Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mst_id);
			Mem_shipBase baseData = new Mem_shipBase(rid, getNo, mst_data);
			this.Set_ShipParam(baseData, mst_data, false);
			return true;
		}

		public void Set_ShipParam(Mem_shipBase baseData, Mst_ship mst_data, bool enemy_flag)
		{
			this.Rid = baseData.Rid;
			this.GetNo = baseData.GetNo;
			this.Sortno = mst_data.Sortno;
			this.Ship_id = mst_data.Id;
			this.Level = baseData.Level;
			this.Nowhp = baseData.Nowhp;
			this.Exp = baseData.Exp;
			this.Leng = mst_data.Leng;
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Houg, baseData.C_houg);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Luck, baseData.C_luck);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Raig, baseData.C_raig);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Souk, baseData.C_souk);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Kaihi, baseData.C_kaihi);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Taisen, baseData.C_taisen);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Taik, baseData.C_taik);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup, baseData.C_taik_powerup);
			this.Kyouka.set_Item(Mem_ship.enumKyoukaIdx.Tyku, baseData.C_tyku);
			this.Backs = mst_data.Backs;
			this.Fuel = baseData.Fuel;
			this.Bull = baseData.Bull;
			this.Slotnum = mst_data.Slot_num;
			this.Cond = baseData.Cond;
			Ship_GrowValues ship_GrowValues = new Ship_GrowValues(mst_data, this.Level, this.Kyouka);
			baseData.C_houg = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg);
			baseData.C_luck = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Luck);
			baseData.C_raig = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Raig);
			baseData.C_souk = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Souk);
			baseData.C_kaihi = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Kaihi);
			baseData.C_taisen = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taisen);
			baseData.C_taik = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik);
			baseData.C_taik_powerup = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taik_Powerup);
			baseData.C_tyku = this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Tyku);
			this.BattleBaseParam = ship_GrowValues;
			this.Maxhp = ship_GrowValues.Maxhp;
			this.Houg = ship_GrowValues.Houg;
			this.Houg_max = mst_data.Houg_max;
			this.Raig = ship_GrowValues.Raig;
			this.Raig_max = mst_data.Raig_max;
			this.Sakuteki = ship_GrowValues.Sakuteki;
			this.Sakuteki_max = mst_data.Saku_max;
			this.Soukou = ship_GrowValues.Soukou;
			this.Soukou_max = mst_data.Souk_max;
			this.Taiku = ship_GrowValues.Taiku;
			this.Taiku_max = mst_data.Tyku_max;
			this.Taisen = ship_GrowValues.Taisen;
			this.Taisen_max = mst_data.Tais_max;
			this.Luck = ship_GrowValues.Luck;
			this.Luck_max = mst_data.Luck_max;
			this.Kaihi = ship_GrowValues.Kaihi;
			this.Kaihi_max = mst_data.Kaih_max;
			this.Slot.Clear();
			this.Onslot.Clear();
			for (int i = 0; i < mst_data.Slot_num; i++)
			{
				this.Slot.Add(baseData.Slot.get_Item(i));
				this.Onslot.Add(baseData.Onslot.get_Item(i));
				if (baseData.Slot.get_Item(i) > 0)
				{
					int num = enemy_flag ? baseData.Slot.get_Item(i) : Comm_UserDatas.Instance.User_slot.get_Item(baseData.Slot.get_Item(i)).Slotitem_id;
					Mst_slotitem slotParam = Mst_DataManager.Instance.Mst_Slotitem.get_Item(num);
					this.setSlotParam(slotParam);
				}
			}
			this.Exslot = baseData.Exslot;
			if (this.Exslot > 0)
			{
				this.setSlotParam(this.GetMstSlotItemToExSlot());
			}
			this.Locked = baseData.Locked;
			this.ememy_flag = enemy_flag;
			this.Stype = mst_data.Stype;
			this.Escape_sts = baseData.Escape_sts;
			this.BlingType = baseData.BlingType;
			this.BlingWaitArea = baseData.BlingWaitArea;
			this.Bling_start = baseData.Bling_start;
			this.Bling_end = baseData.Bling_end;
			this.battleCommand = baseData.BattleCommand;
			this.Lov = baseData.Lov;
			this.Lov_back_processed = baseData.Lov_back_processed;
			this.Lov_back_value = baseData.Lov_back_value;
			this.Lov_front_processed = baseData.Lov_front_processed;
			this.Lov_front_value = baseData.Lov_front_value;
		}

		private void setSlotParam(Mst_slotitem m_slot)
		{
			this.Houg += this.getSlotParamAddValue(this.Houg, m_slot.Houg);
			this.Raig += this.getSlotParamAddValue(this.Raig, m_slot.Raig);
			this.Sakuteki += this.getSlotParamAddValue(this.Sakuteki, m_slot.Saku);
			this.Soukou += this.getSlotParamAddValue(this.Soukou, m_slot.Souk);
			this.Taiku += this.getSlotParamAddValue(this.Taiku, m_slot.Tyku);
			this.Taisen += this.getSlotParamAddValue(this.Taisen, m_slot.Tais);
			this.Houm += this.getSlotParamAddValue(this.Houm, m_slot.Houm);
			this.Kaihi += this.getSlotParamAddValue(this.Kaihi, m_slot.Houk);
			if (this.Leng < m_slot.Leng)
			{
				this.Leng = m_slot.Leng;
			}
		}

		private int getSlotParamAddValue(int nowVal, int mstVal)
		{
			int num = nowVal + mstVal;
			return (num >= 0) ? mstVal : (-nowVal);
		}

		public void Set_ShipParamPracticeShip(Mem_shipBase baseData, Mst_ship mst_data)
		{
			this.Set_ShipParam(baseData, mst_data, false);
			for (int i = 0; i < this.Slot.get_Count(); i++)
			{
				int num = this.Slot.get_Item(i);
				if (num > 0)
				{
					this.Slot.set_Item(i, Comm_UserDatas.Instance.User_slot.get_Item(num).Slotitem_id);
				}
			}
			this.ememy_flag = true;
		}

		public void Set_ShipParamNewGamePlus(ICreateNewUser instance)
		{
			if (instance == null)
			{
				return;
			}
			this.Bling_start = 0;
			this.Bling_end = 0;
			this.BlingType = Mem_ship.BlingKind.None;
			this.BlingWaitArea = 0;
			this.Nowhp = this.Maxhp;
			this.Cond = 40;
			Mst_ship mst_ship = null;
			if (Mst_DataManager.Instance.Mst_ship.TryGetValue(this.Ship_id, ref mst_ship))
			{
				this.Fuel = mst_ship.Fuel_max;
				this.Bull = mst_ship.Bull_max;
				for (int i = 0; i < mst_ship.Slot_num; i++)
				{
					this.Onslot.set_Item(i, mst_ship.Maxeq.get_Item(i));
				}
			}
		}

		public SlotSetInfo GetSlotSetParam(int slot_index, int slot_id)
		{
			SlotSetInfo result = default(SlotSetInfo);
			if (slot_index >= this.Slot.get_Count())
			{
				return result;
			}
			if (this.Slot.get_Item(slot_index) <= 0 && slot_id < 0)
			{
				return result;
			}
			List<Mst_slotitem> mstSlotItems = this.GetMstSlotItems();
			List<int> arg_7F_0;
			if (mstSlotItems.get_Count() > 0)
			{
				arg_7F_0 = Enumerable.ToList<int>(Enumerable.Select<Mst_slotitem, int>(mstSlotItems, (Mst_slotitem x) => x.Leng));
			}
			else
			{
				arg_7F_0 = new List<int>();
			}
			List<int> list = arg_7F_0;
			list.Add(Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id).Leng);
			Mst_slotitem mstSlotItemToExSlot = this.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				list.Add(mstSlotItemToExSlot.Leng);
			}
			int slotitem_id;
			Mst_slotitem mst_slotitem;
			if (this.Slot.get_Item(slot_index) <= 0)
			{
				slotitem_id = Comm_UserDatas.Instance.User_slot.get_Item(slot_id).Slotitem_id;
				mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(slotitem_id);
				list.Add(mst_slotitem.Leng);
				result.SetSlot(this.Leng, mst_slotitem);
				result.SetLeng(this.Leng, list);
				return result;
			}
			int slotitem_id2 = Comm_UserDatas.Instance.User_slot.get_Item(this.Slot.get_Item(slot_index)).Slotitem_id;
			Mst_slotitem mst_slotitem2 = Mst_DataManager.Instance.Mst_Slotitem.get_Item(slotitem_id2);
			list.RemoveAt(slot_index);
			if (slot_id < 0)
			{
				result.UnsetSlot(mst_slotitem2);
				result.SetLeng(this.Leng, list);
				return result;
			}
			slotitem_id = Comm_UserDatas.Instance.User_slot.get_Item(slot_id).Slotitem_id;
			mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(slotitem_id);
			list.Add(mst_slotitem.Leng);
			result.ChangeSlot(mst_slotitem2, mst_slotitem);
			result.SetLeng(this.Leng, list);
			return result;
		}

		public SlotSetInfo GetSlotSetParam(int slot_id)
		{
			SlotSetInfo result = default(SlotSetInfo);
			if (this.Exslot <= 0 && slot_id == -1)
			{
				return result;
			}
			Mst_slotitem mstSlotItemToExSlot = this.GetMstSlotItemToExSlot();
			List<Mst_slotitem> mstSlotItems = this.GetMstSlotItems();
			List<int> arg_70_0;
			if (mstSlotItems.get_Count() > 0)
			{
				arg_70_0 = Enumerable.ToList<int>(Enumerable.Select<Mst_slotitem, int>(mstSlotItems, (Mst_slotitem x) => x.Leng));
			}
			else
			{
				arg_70_0 = new List<int>();
			}
			List<int> list = arg_70_0;
			list.Add(Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id).Leng);
			int slotitem_id;
			Mst_slotitem mst_slotitem;
			if (mstSlotItemToExSlot == null && slot_id > 0)
			{
				slotitem_id = Comm_UserDatas.Instance.User_slot.get_Item(slot_id).Slotitem_id;
				mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(slotitem_id);
				list.Add(mst_slotitem.Leng);
				result.SetSlot(this.Leng, mst_slotitem);
				result.SetLeng(this.Leng, list);
				return result;
			}
			if (slot_id < 0)
			{
				result.UnsetSlot(mstSlotItemToExSlot);
				result.SetLeng(this.Leng, list);
				return result;
			}
			slotitem_id = Comm_UserDatas.Instance.User_slot.get_Item(slot_id).Slotitem_id;
			mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(slotitem_id);
			list.Add(mst_slotitem.Leng);
			result.ChangeSlot(mstSlotItemToExSlot, mst_slotitem);
			result.SetLeng(this.Leng, list);
			return result;
		}

		public Ship_GrowValues GetBattleBaseParam()
		{
			return this.BattleBaseParam;
		}

		public List<BattleCommand> GetBattleCommad()
		{
			if (this.IsEnemy())
			{
				return null;
			}
			int battleCommandEnableNum = this.GetBattleCommandEnableNum();
			if (this.battleCommand != null)
			{
				return Enumerable.ToList<BattleCommand>(this.battleCommand);
			}
			List<BattleCommand> list = Enumerable.ToList<BattleCommand>(Enumerable.Repeat<BattleCommand>(BattleCommand.None, 5));
			if (Mst_DataManager.Instance.Mst_stype.get_Item(this.Stype).IsKouku())
			{
				if (battleCommandEnableNum == 3)
				{
					list.set_Item(0, BattleCommand.Kouku);
					list.set_Item(1, BattleCommand.Hougeki);
					list.set_Item(2, BattleCommand.Raigeki);
				}
				else if (battleCommandEnableNum == 4)
				{
					list.set_Item(0, BattleCommand.Kouku);
					list.set_Item(1, BattleCommand.Hougeki);
					list.set_Item(2, BattleCommand.Hougeki);
					list.set_Item(3, BattleCommand.Raigeki);
				}
				else
				{
					list.set_Item(0, BattleCommand.Kouku);
					list.set_Item(1, BattleCommand.Hougeki);
					list.set_Item(2, BattleCommand.Hougeki);
					list.set_Item(3, BattleCommand.Hougeki);
					list.set_Item(4, BattleCommand.Raigeki);
				}
			}
			else if (battleCommandEnableNum == 3)
			{
				list.set_Item(0, BattleCommand.Hougeki);
				list.set_Item(1, BattleCommand.Hougeki);
				list.set_Item(2, BattleCommand.Raigeki);
			}
			else if (battleCommandEnableNum == 4)
			{
				list.set_Item(0, BattleCommand.Hougeki);
				list.set_Item(1, BattleCommand.Hougeki);
				list.set_Item(2, BattleCommand.Hougeki);
				list.set_Item(3, BattleCommand.Raigeki);
			}
			else
			{
				list.set_Item(0, BattleCommand.Hougeki);
				list.set_Item(1, BattleCommand.Hougeki);
				list.set_Item(2, BattleCommand.Hougeki);
				list.set_Item(3, BattleCommand.Hougeki);
				list.set_Item(4, BattleCommand.Raigeki);
			}
			this.battleCommand = list;
			return Enumerable.ToList<BattleCommand>(this.battleCommand);
		}

		public void GetBattleCommand(out List<BattleCommand> command)
		{
			command = this.battleCommand;
		}

		public int GetBattleCommandEnableNum()
		{
			if (this.Level >= 1 && this.Level <= 34)
			{
				return 3;
			}
			if (this.Level >= 35 && this.Level <= 69)
			{
				return 4;
			}
			return 5;
		}

		public void SetBattleCommand(List<BattleCommand> command)
		{
			if (this.battleCommand == null)
			{
				this.GetBattleCommad();
			}
			for (int i = 0; i < command.get_Count(); i++)
			{
				this.battleCommand.set_Item(i, command.get_Item(i));
			}
		}

		public void PurgeBattleCommand()
		{
			this.battleCommand.Clear();
			this.battleCommand = null;
		}

		public static DamageState Get_DamageState(int nowhp, int maxhp)
		{
			if ((float)nowhp <= (float)maxhp * 0.25f)
			{
				return DamageState.Taiha;
			}
			if ((float)nowhp <= (float)maxhp * 0.5f)
			{
				return DamageState.Tyuuha;
			}
			if ((float)nowhp <= (float)maxhp * 0.75f)
			{
				return DamageState.Shouha;
			}
			return DamageState.Normal;
		}

		public DamageState Get_DamageState()
		{
			return Mem_ship.Get_DamageState(this.Nowhp, this.Maxhp);
		}

		public static FatigueState Get_FatitgueState(int cond)
		{
			if (cond >= 50)
			{
				return FatigueState.Exaltation;
			}
			if (cond >= 30 && cond <= 49)
			{
				return FatigueState.Normal;
			}
			if (cond >= 20 && cond <= 29)
			{
				return FatigueState.Light;
			}
			return FatigueState.Distress;
		}

		public FatigueState Get_FatigueState()
		{
			return Mem_ship.Get_FatitgueState(this.Cond);
		}

		public bool IsEnemy()
		{
			return this.ememy_flag;
		}

		public void Set_ChargeData(int bull, int fuel, List<int> onslot)
		{
			this.Bull = bull;
			this.Fuel = fuel;
			if (onslot == null)
			{
				return;
			}
			for (int i = 0; i < this.Onslot.get_Count(); i++)
			{
				this.Onslot.set_Item(i, onslot.get_Item(i));
			}
		}

		public int[] FindRecoveryItem()
		{
			int[] idx = new int[2];
			idx[0] = this.Slot.FindIndex(delegate(int x)
			{
				Mem_slotitem mem_slotitem = null;
				Comm_UserDatas.Instance.User_slot.TryGetValue(x, ref mem_slotitem);
				if (mem_slotitem != null && (mem_slotitem.Slotitem_id == 42 || mem_slotitem.Slotitem_id == 43))
				{
					idx[1] = mem_slotitem.Slotitem_id;
					return true;
				}
				return false;
			});
			Mst_slotitem mstSlotItemToExSlot = this.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot != null)
			{
				int id = mstSlotItemToExSlot.Id;
				if (id == 42 || id == 43)
				{
					idx[0] = 2147483647;
					idx[1] = mstSlotItemToExSlot.Id;
				}
			}
			return idx;
		}

		public int GetRecoveryItemUseHp(ShipRecoveryType recoveryType, bool flagShipRecovery)
		{
			if (recoveryType == ShipRecoveryType.Personnel)
			{
				double num = (!flagShipRecovery) ? Math.Floor((double)((float)this.Maxhp * 0.2f)) : Math.Floor((double)((float)this.Maxhp * 0.5f));
				return (int)num;
			}
			return this.Maxhp;
		}

		public bool UseRecoveryItem(int[] recoveryItemIdx, bool flagShipRecovery)
		{
			if (recoveryItemIdx[0] == -1)
			{
				return false;
			}
			if (recoveryItemIdx[1] == 43)
			{
				this.Nowhp = this.GetRecoveryItemUseHp(ShipRecoveryType.Goddes, false);
				this.Fuel = Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id).Fuel_max;
				this.Bull = Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id).Bull_max;
			}
			else if (recoveryItemIdx[1] == 42)
			{
				this.Nowhp = this.GetRecoveryItemUseHp(ShipRecoveryType.Personnel, flagShipRecovery);
			}
			int num;
			if (recoveryItemIdx[0] != 2147483647)
			{
				num = this.Slot.get_Item(recoveryItemIdx[0]);
				this.Slot.set_Item(recoveryItemIdx[0], -1);
				this.TrimSlot();
			}
			else
			{
				num = this.Exslot;
				this.Exslot = -1;
			}
			Comm_UserDatas arg_DB_0 = Comm_UserDatas.Instance;
			List<int> list = new List<int>();
			list.Add(num);
			arg_DB_0.Remove_Slot(list);
			Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count++;
			return true;
		}

		public bool IsFight()
		{
			return this.Nowhp > 0 && !this.Escape_sts;
		}

		public bool IsEscortDeffender()
		{
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(this.Stype).IsSubmarine();
			DamageState damageState = this.Get_DamageState();
			bool flag2 = this.IsBlingShip();
			return !flag && !flag2 && damageState < DamageState.Tyuuha;
		}

		public void SubHp(int subValue)
		{
			this.Nowhp -= subValue;
			if (this.Nowhp < 0)
			{
				this.Nowhp = 0;
			}
		}

		public int getLevelupInfo(Dictionary<int, int> mst_level, int nowLevel, int nowExp, ref int addExp, out List<int> lvupInfo)
		{
			int num = nowLevel;
			int num2 = (nowLevel <= 99) ? mst_level.get_Item(99) : Enumerable.Max(mst_level.get_Values());
			if (addExp + nowExp > num2)
			{
				addExp = num2 - nowExp;
			}
			int num3 = nowLevel + 1;
			int num4 = mst_level.get_Item(num3);
			int afterExp = addExp + nowExp;
			List<int> list = new List<int>();
			list.Add(nowExp);
			lvupInfo = list;
			if (num4 == -1)
			{
				return num;
			}
			if (afterExp < num4)
			{
				lvupInfo.Add(num4);
				return num;
			}
			IEnumerable<int> enumerable = Enumerable.Select<KeyValuePair<int, int>, int>(Enumerable.Where<KeyValuePair<int, int>>(mst_level, (KeyValuePair<int, int> data) => data.get_Key() > nowLevel && data.get_Value() <= afterExp && data.get_Value() != -1), (KeyValuePair<int, int> data) => data.get_Value());
			lvupInfo.AddRange(enumerable);
			num += Enumerable.Count<int>(enumerable);
			if (mst_level.get_Item(num + 1) != -1)
			{
				lvupInfo.Add(mst_level.get_Item(num + 1));
			}
			return num;
		}

		public Dictionary<Mem_ship.enumKyoukaIdx, int> getLevelupKyoukaValue(int mst_id, Dictionary<Mem_ship.enumKyoukaIdx, int> nowKyouka)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mst_id);
			Dictionary<Mem_ship.enumKyoukaIdx, int[]> dictionary = new Dictionary<Mem_ship.enumKyoukaIdx, int[]>();
			dictionary.Add(Mem_ship.enumKyoukaIdx.Houg, new int[]
			{
				mst_ship.Houg,
				mst_ship.Houg_max
			});
			dictionary.Add(Mem_ship.enumKyoukaIdx.Raig, new int[]
			{
				mst_ship.Raig,
				mst_ship.Raig_max
			});
			dictionary.Add(Mem_ship.enumKyoukaIdx.Tyku, new int[]
			{
				mst_ship.Tyku,
				mst_ship.Tyku_max
			});
			dictionary.Add(Mem_ship.enumKyoukaIdx.Souk, new int[]
			{
				mst_ship.Souk,
				mst_ship.Souk_max
			});
			dictionary.Add(Mem_ship.enumKyoukaIdx.Kaihi, new int[]
			{
				mst_ship.Kaih,
				mst_ship.Kaih_max
			});
			dictionary.Add(Mem_ship.enumKyoukaIdx.Taisen, new int[]
			{
				mst_ship.Tais,
				mst_ship.Tais_max
			});
			Dictionary<Mem_ship.enumKyoukaIdx, int[]> dictionary2 = dictionary;
			Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary3 = new Dictionary<Mem_ship.enumKyoukaIdx, int>();
			using (Dictionary<Mem_ship.enumKyoukaIdx, int>.Enumerator enumerator = nowKyouka.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship.enumKyoukaIdx, int> current = enumerator.get_Current();
					if (!dictionary2.ContainsKey(current.get_Key()))
					{
						dictionary3.Add(current.get_Key(), current.get_Value());
					}
					else
					{
						int num = dictionary2.get_Item(current.get_Key())[0];
						int num2 = dictionary2.get_Item(current.get_Key())[1];
						int num3 = num + current.get_Value();
						if (num3 >= num2)
						{
							dictionary3.Add(current.get_Key(), current.get_Value());
						}
						else
						{
							int num4 = num2 - num - current.get_Value();
							int num5 = (num4 < 20) ? 2 : 3;
							int num6 = (int)Utils.GetRandDouble(0.0, (double)num5, 1.0, 1);
							int num7 = num3 + num6;
							if (num7 > num2)
							{
								dictionary3.Add(current.get_Key(), num2 - num);
							}
							else
							{
								dictionary3.Add(current.get_Key(), current.get_Value() + num6);
							}
						}
					}
				}
			}
			return dictionary3;
		}

		public Dictionary<enumMaterialCategory, int> GetNdockMaterialNum()
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id);
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			int num = this.Maxhp - this.Nowhp;
			if (num <= 0)
			{
				return null;
			}
			int num2 = (int)((float)(mst_ship.Use_fuel * num) * 0.4f * 0.4f);
			dictionary.Add(enumMaterialCategory.Fuel, num2);
			int num3 = (int)((float)(mst_ship.Use_fuel * num) * 0.75f * 0.4f);
			dictionary.Add(enumMaterialCategory.Steel, num3);
			return dictionary;
		}

		public int GetNdockTimeSpan()
		{
			int num = this.Maxhp - this.Nowhp;
			if (num <= 0)
			{
				return 0;
			}
			int ndockTime = this.GetNdockTime();
			int num2 = (int)Math.Ceiling((double)ndockTime / 60.0 / 30.0);
			if (num2 > 10)
			{
				num2 = 10 + (int)((double)(num2 - 10) * 0.5 + 0.5);
			}
			return num2;
		}

		private int GetNdockTime()
		{
			int num = 30;
			int num2 = 5;
			double num3 = (double)this.Level;
			if (num3 > 11.0)
			{
				num3 = (double)num2 + num3 / 2.0 + (double)((int)Math.Sqrt(num3 - 11.0));
			}
			int num4 = (int)(num3 * (double)(this.Maxhp - this.Nowhp) * (double)Mst_DataManager.Instance.Mst_stype.get_Item(this.Stype).Scnt);
			return num + num4 * num2;
		}

		public void NdockRecovery(Mem_ndock dockInstance)
		{
			if (dockInstance.Ship_id == this.Rid)
			{
				this.Nowhp = this.Maxhp;
			}
			if (this.Cond < 40)
			{
				this.Cond = 40;
			}
		}

		public bool ExistsMission()
		{
			return Enumerable.Any<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.MissionState != MissionStates.NONE && x.Ship.Find(this.Rid) != -1);
		}

		public bool ExistsNdock()
		{
			return Enumerable.Any<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Ship_id == this.Rid);
		}

		public void ChangeLockSts()
		{
			this.Locked ^= 1;
		}

		public bool IsBlingShip()
		{
			return this.BlingType == Mem_ship.BlingKind.Bling || this.BlingType == Mem_ship.BlingKind.PortBack;
		}

		public bool IsBlingWait()
		{
			return this.IsBlingWait(this.BlingType);
		}

		private bool IsBlingWait(Mem_ship.BlingKind kind)
		{
			return kind >= (Mem_ship.BlingKind)10 && kind <= (Mem_ship.BlingKind)19;
		}

		public bool IsPortBack()
		{
			return this.BlingType == Mem_ship.BlingKind.PortBack;
		}

		public int GetBlingTurn()
		{
			if (!this.IsBlingShip())
			{
				return 0;
			}
			return this.Bling_end - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public bool BlingSet(int area_id)
		{
			if (area_id == 1)
			{
				return false;
			}
			this.BlingType = Mem_ship.BlingKind.Bling;
			this.Bling_start = Comm_UserDatas.Instance.User_turn.Total_turn;
			this.Bling_end = Comm_UserDatas.Instance.User_turn.Total_turn + Mst_DataManager.Instance.Mst_maparea.get_Item(area_id).Distance;
			return true;
		}

		public void BlingWait(int area_id, Mem_ship.BlingKind kind)
		{
			if (!this.IsBlingWait(kind))
			{
				return;
			}
			if (!this.BlingSet(area_id))
			{
				return;
			}
			this.BlingType = kind;
			this.BlingWaitArea = area_id;
		}

		public void BlingWaitToStart()
		{
			if (!this.IsBlingWait(this.BlingType))
			{
				return;
			}
			this.BlingSet(this.BlingWaitArea);
			this.BlingWaitArea = 0;
		}

		public void BlingWaitToStop()
		{
			if (!this.IsBlingWait(this.BlingType))
			{
				return;
			}
			this.BlingType = Mem_ship.BlingKind.None;
			this.BlingWaitArea = 0;
		}

		public void PortWithdraw(int area_id)
		{
			if (!this.BlingSet(area_id))
			{
				return;
			}
			this.BlingType = Mem_ship.BlingKind.PortBack;
		}

		public bool BlingTerm()
		{
			if (!this.IsBlingShip())
			{
				return false;
			}
			if (this.Bling_end > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			this.Bling_start = 0;
			this.Bling_end = 0;
			this.BlingType = Mem_ship.BlingKind.None;
			return true;
		}

		public void SetHp(Api_req_PracticeBattle instance, int startHp)
		{
			this.Nowhp = startHp;
		}

		public Dictionary<enumMaterialCategory, int> getDestroyShipMaterials()
		{
			Mem_ship.<getDestroyShipMaterials>c__AnonStorey4FB <getDestroyShipMaterials>c__AnonStorey4FB = new Mem_ship.<getDestroyShipMaterials>c__AnonStorey4FB();
			Mem_ship.<getDestroyShipMaterials>c__AnonStorey4FB arg_2E_0 = <getDestroyShipMaterials>c__AnonStorey4FB;
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 0);
			dictionary.Add(enumMaterialCategory.Bull, 0);
			dictionary.Add(enumMaterialCategory.Steel, 0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0);
			arg_2E_0.ret = dictionary;
			this.Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					Mem_slotitem mem_slotitem = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(x, ref mem_slotitem))
					{
						Mst_slotitem mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(mem_slotitem.Slotitem_id);
						Dictionary<enumMaterialCategory, int> ret8;
						Dictionary<enumMaterialCategory, int> expr_3C = ret8 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
						enumMaterialCategory enumMaterialCategory2;
						enumMaterialCategory expr_3F = enumMaterialCategory2 = enumMaterialCategory.Fuel;
						int num2 = ret8.get_Item(enumMaterialCategory2);
						expr_3C.set_Item(expr_3F, num2 + mst_slotitem.Broken1);
						Dictionary<enumMaterialCategory, int> ret9;
						Dictionary<enumMaterialCategory, int> expr_5E = ret9 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
						enumMaterialCategory expr_62 = enumMaterialCategory2 = enumMaterialCategory.Bull;
						num2 = ret9.get_Item(enumMaterialCategory2);
						expr_5E.set_Item(expr_62, num2 + mst_slotitem.Broken2);
						Dictionary<enumMaterialCategory, int> ret10;
						Dictionary<enumMaterialCategory, int> expr_82 = ret10 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
						enumMaterialCategory expr_86 = enumMaterialCategory2 = enumMaterialCategory.Steel;
						num2 = ret10.get_Item(enumMaterialCategory2);
						expr_82.set_Item(expr_86, num2 + mst_slotitem.Broken3);
						Dictionary<enumMaterialCategory, int> ret11;
						Dictionary<enumMaterialCategory, int> expr_A6 = ret11 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
						enumMaterialCategory expr_AA = enumMaterialCategory2 = enumMaterialCategory.Bauxite;
						num2 = ret11.get_Item(enumMaterialCategory2);
						expr_A6.set_Item(expr_AA, num2 + mst_slotitem.Broken4);
					}
				}
			});
			Mst_slotitem mstSlotItemToExSlot = this.GetMstSlotItemToExSlot();
			enumMaterialCategory enumMaterialCategory;
			int num;
			if (mstSlotItemToExSlot != null)
			{
				Dictionary<enumMaterialCategory, int> expr_5D = dictionary = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
				enumMaterialCategory expr_60 = enumMaterialCategory = enumMaterialCategory.Fuel;
				num = dictionary.get_Item(enumMaterialCategory);
				expr_5D.set_Item(expr_60, num + mstSlotItemToExSlot.Broken1);
				Dictionary<enumMaterialCategory, int> ret;
				Dictionary<enumMaterialCategory, int> expr_81 = ret = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
				enumMaterialCategory expr_85 = enumMaterialCategory = enumMaterialCategory.Bull;
				num = ret.get_Item(enumMaterialCategory);
				expr_81.set_Item(expr_85, num + mstSlotItemToExSlot.Broken2);
				Dictionary<enumMaterialCategory, int> ret2;
				Dictionary<enumMaterialCategory, int> expr_A7 = ret2 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
				enumMaterialCategory expr_AB = enumMaterialCategory = enumMaterialCategory.Steel;
				num = ret2.get_Item(enumMaterialCategory);
				expr_A7.set_Item(expr_AB, num + mstSlotItemToExSlot.Broken3);
				Dictionary<enumMaterialCategory, int> ret3;
				Dictionary<enumMaterialCategory, int> expr_CD = ret3 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
				enumMaterialCategory expr_D1 = enumMaterialCategory = enumMaterialCategory.Bauxite;
				num = ret3.get_Item(enumMaterialCategory);
				expr_CD.set_Item(expr_D1, num + mstSlotItemToExSlot.Broken4);
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id);
			Dictionary<enumMaterialCategory, int> ret4;
			Dictionary<enumMaterialCategory, int> expr_109 = ret4 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
			enumMaterialCategory expr_10D = enumMaterialCategory = enumMaterialCategory.Fuel;
			num = ret4.get_Item(enumMaterialCategory);
			expr_109.set_Item(expr_10D, num + mst_ship.Broken1);
			Dictionary<enumMaterialCategory, int> ret5;
			Dictionary<enumMaterialCategory, int> expr_12F = ret5 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
			enumMaterialCategory expr_133 = enumMaterialCategory = enumMaterialCategory.Bull;
			num = ret5.get_Item(enumMaterialCategory);
			expr_12F.set_Item(expr_133, num + mst_ship.Broken2);
			Dictionary<enumMaterialCategory, int> ret6;
			Dictionary<enumMaterialCategory, int> expr_155 = ret6 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
			enumMaterialCategory expr_159 = enumMaterialCategory = enumMaterialCategory.Steel;
			num = ret6.get_Item(enumMaterialCategory);
			expr_155.set_Item(expr_159, num + mst_ship.Broken3);
			Dictionary<enumMaterialCategory, int> ret7;
			Dictionary<enumMaterialCategory, int> expr_17B = ret7 = <getDestroyShipMaterials>c__AnonStorey4FB.ret;
			enumMaterialCategory expr_17F = enumMaterialCategory = enumMaterialCategory.Bauxite;
			num = ret7.get_Item(enumMaterialCategory);
			expr_17B.set_Item(expr_17F, num + mst_ship.Broken4);
			return <getDestroyShipMaterials>c__AnonStorey4FB.ret;
		}

		public List<Mst_slotitem> GetMstSlotItems()
		{
			List<Mst_slotitem> ret = new List<Mst_slotitem>();
			this.Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					Mem_slotitem mem_slotitem = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(x, ref mem_slotitem))
					{
						Mst_slotitem mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(mem_slotitem.Slotitem_id);
						ret.Add(mst_slotitem);
					}
				}
			});
			return ret;
		}

		public Mst_slotitem GetMstSlotItemToExSlot()
		{
			Mem_slotitem mem_slotitem = null;
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(this.Exslot, ref mem_slotitem))
			{
				return null;
			}
			return Mst_DataManager.Instance.Mst_Slotitem.get_Item(mem_slotitem.Slotitem_id);
		}

		public Dictionary<int, int> GetMstSlotItemNum_OrderId(HashSet<int> order_ids)
		{
			Dictionary<int, int> ret = Enumerable.ToDictionary<int, int, int>(order_ids, (int key) => key, (int value) => 0);
			this.GetMstSlotItems().ForEach(delegate(Mst_slotitem x)
			{
				if (ret.ContainsKey(x.Id))
				{
					Dictionary<int, int> ret2;
					Dictionary<int, int> expr_1C = ret2 = ret;
					int num2;
					int expr_24 = num2 = x.Id;
					num2 = ret2.get_Item(num2);
					expr_1C.set_Item(expr_24, num2 + 1);
				}
			});
			Mst_slotitem mstSlotItemToExSlot = this.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot == null)
			{
				return ret;
			}
			if (ret.ContainsKey(mstSlotItemToExSlot.Id))
			{
				Dictionary<int, int> ret3;
				Dictionary<int, int> expr_93 = ret3 = ret;
				int num;
				int expr_9B = num = mstSlotItemToExSlot.Id;
				num = ret3.get_Item(num);
				expr_93.set_Item(expr_9B, num + 1);
			}
			return ret;
		}

		public Dictionary<int, List<int>> GetSlotIndexFromId(HashSet<int> searchIds)
		{
			Dictionary<int, List<int>> dictionary = Enumerable.ToDictionary<int, int, List<int>>(searchIds, (int id) => id, (int value) => new List<int>());
			List<Mst_slotitem> mstSlotItems = this.GetMstSlotItems();
			for (int i = 0; i < mstSlotItems.get_Count(); i++)
			{
				Mst_slotitem mst_slotitem = mstSlotItems.get_Item(i);
				if (searchIds.Contains(mst_slotitem.Id))
				{
					dictionary.get_Item(mst_slotitem.Id).Add(i);
				}
			}
			Mst_slotitem mstSlotItemToExSlot = this.GetMstSlotItemToExSlot();
			if (mstSlotItemToExSlot == null)
			{
				return dictionary;
			}
			if (searchIds.Contains(mstSlotItemToExSlot.Id))
			{
				dictionary.get_Item(mstSlotItemToExSlot.Id).Add(2147483647);
			}
			return dictionary;
		}

		public bool IsActiveEnd()
		{
			Mem_deck mem_deck = Enumerable.FirstOrDefault<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.Ship.Find(this.Rid) != -1);
			return mem_deck != null && mem_deck.IsActionEnd;
		}

		public void TrimSlot()
		{
			this.Slot.RemoveAll((int x) => x == -1);
			if (this.Slot.get_Count() != this.Slotnum)
			{
				this.Slot.AddRange(Enumerable.Repeat<int>(-1, this.Slotnum - this.Slot.get_Count()));
			}
		}

		public int GetRequireChargeFuel()
		{
			int fuel_max = Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id).Fuel_max;
			if (this.Fuel >= fuel_max)
			{
				return 0;
			}
			double num = (this.Level <= 99) ? 1.0 : 0.85;
			double num2 = (double)(fuel_max - this.Fuel);
			return Math.Max((int)(num2 * num), 1);
		}

		public int GetRequireChargeBull()
		{
			int bull_max = Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id).Bull_max;
			if (this.Bull >= bull_max)
			{
				return 0;
			}
			double num = (this.Level <= 99) ? 1.0 : 0.85;
			double num2 = (double)(bull_max - this.Bull);
			return Math.Max((int)(num2 * num), 1);
		}

		private int getNextExp()
		{
			if (this.nextRequireExp <= 0)
			{
				return 0;
			}
			return this.nextRequireExp - this.Exp;
		}

		private int getNextExpRate()
		{
			if (this.nextRequireExp == 0)
			{
				return 0;
			}
			int num = (this.Level == 100) ? this.beforeRequireExp : this.requiereExp;
			float num2 = (float)(this.Exp - num);
			float num3 = (float)(this.nextRequireExp - num);
			if (num3 == 0f)
			{
				return 0;
			}
			return (int)(num2 / num3 * 100f);
		}

		private int GetStarValue()
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(this.Ship_id);
			float num = (float)(this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg) + this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Raig) + this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Souk) + this.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Tyku));
			float num2 = (float)(mst_ship.Houg_max - mst_ship.Houg + (mst_ship.Raig_max - mst_ship.Raig) + (mst_ship.Tyku_max - mst_ship.Tyku) + (mst_ship.Souk_max - mst_ship.Souk));
			if (num2 == 0f)
			{
				return 0;
			}
			int num3 = (int)(num / num2 * 100f);
			if (num3 >= 80)
			{
				return 4;
			}
			if (num3 >= 60)
			{
				return 3;
			}
			if (num3 >= 40)
			{
				return 2;
			}
			if (num3 >= 10)
			{
				return 1;
			}
			return 0;
		}

		public void SetSubFuel_ToMission(double use_fuel)
		{
			int num = (int)Math.Floor((double)this.Fuel * use_fuel);
			this.Fuel -= num;
		}

		public void SetSubBull_ToMission(double use_bull)
		{
			int num = (int)Math.Floor((double)this.Bull * use_bull);
			this.Bull -= num;
		}

		public void SetSortieEndCond(Api_req_Map instance)
		{
			if (instance == null)
			{
				return;
			}
			this.Cond -= 15;
			if (this.Cond < 0)
			{
				this.Cond = 0;
			}
		}

		public void AddTurnRecoveryCond(Api_TurnOperator instance, int upNum)
		{
			if (instance == null)
			{
				return;
			}
			if (this.Cond >= 49)
			{
				return;
			}
			this.Cond += upNum;
			if (this.Cond > 49)
			{
				this.Cond = 49;
			}
		}

		public void AddRationItemCond(int upNum)
		{
			this.Cond += upNum;
			if (this.Cond > 100)
			{
				this.Cond = 100;
			}
		}

		public HashSet<Mem_slotitem> GetLockSlotItems()
		{
			HashSet<Mem_slotitem> ret = new HashSet<Mem_slotitem>();
			this.Slot.ForEach(delegate(int x)
			{
				Mem_slotitem mem_slotitem2 = null;
				if (Comm_UserDatas.Instance.User_slot.TryGetValue(x, ref mem_slotitem2) && mem_slotitem2.Lock)
				{
					ret.Add(mem_slotitem2);
				}
			});
			if (this.Exslot > 0)
			{
				Mem_slotitem mem_slotitem = Comm_UserDatas.Instance.User_slot.get_Item(this.Exslot);
				if (mem_slotitem.Lock)
				{
					ret.Add(mem_slotitem);
				}
			}
			return ret;
		}

		public void ChangeEscapeState()
		{
			this.Escape_sts = !this.Escape_sts;
		}

		public bool SumLov(ref TouchLovInfo touchInfo)
		{
			byte b = (byte)touchInfo.VoiceType;
			byte b2 = 0;
			List<int> list;
			Dictionary<byte, byte> dictionary;
			int num;
			int num2;
			if (touchInfo.BackTouch)
			{
				list = this.Lov_back_value;
				dictionary = this.Lov_back_processed;
				num = -10;
				num2 = 7;
			}
			else
			{
				list = this.Lov_front_value;
				dictionary = this.Lov_front_processed;
				num = -10;
				num2 = 5;
			}
			bool flag = dictionary.TryGetValue(b, ref b2);
			int nowTouchNum = (int)(b2 + 1);
			int sumValue = touchInfo.GetSumValue(nowTouchNum);
			if (sumValue == 0)
			{
				return false;
			}
			if (list.get_Count() == 0)
			{
				list.Add(0);
				list.Add(0);
			}
			bool result = false;
			if (sumValue > 0)
			{
				list.set_Item(0, (list.get_Item(0) <= 0) ? 0 : 4);
				if (list.get_Item(0) < num2)
				{
					int num3 = (list.get_Item(0) + sumValue <= num2) ? sumValue : (num2 - list.get_Item(0));
					List<int> list2;
					List<int> expr_E3 = list2 = list;
					int num4;
					int expr_E7 = num4 = 0;
					num4 = list2.get_Item(num4);
					expr_E3.set_Item(expr_E7, num4 + num3);
					if (!flag)
					{
						dictionary.Add(b, 1);
					}
					else
					{
						Dictionary<byte, byte> dictionary2;
						Dictionary<byte, byte> expr_114 = dictionary2 = dictionary;
						byte b3;
						byte expr_118 = b3 = b;
						b3 = dictionary2.get_Item(b3);
						expr_114.set_Item(expr_118, b3 + 1);
					}
					result = this.addLov(num3);
				}
			}
			else if (list.get_Item(1) > num)
			{
				int num5 = (list.get_Item(1) + sumValue >= num) ? sumValue : (num - list.get_Item(1));
				List<int> list3;
				List<int> expr_172 = list3 = list;
				int num4;
				int expr_176 = num4 = 1;
				num4 = list3.get_Item(num4);
				expr_172.set_Item(expr_176, num4 + num5);
				if (!flag)
				{
					dictionary.Add(b, 1);
				}
				else
				{
					Dictionary<byte, byte> dictionary3;
					Dictionary<byte, byte> expr_1A3 = dictionary3 = dictionary;
					byte b3;
					byte expr_1A7 = b3 = b;
					b3 = dictionary3.get_Item(b3);
					expr_1A3.set_Item(expr_1A7, b3 + 1);
				}
				result = this.subLov(num5);
			}
			return result;
		}

		public void PurgeLovTouchData()
		{
			this.Lov_back_processed.Clear();
			this.Lov_back_value.Clear();
			this.Lov_front_processed.Clear();
			this.Lov_front_value.Clear();
		}

		public bool SumLovToTurnStart(bool isInNdock, bool flagShip)
		{
			int num = 0;
			int lov = this.Lov;
			FatigueState fatigueState = this.Get_FatigueState();
			if (fatigueState == FatigueState.Light)
			{
				num = -2;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num = -5;
			}
			int num2 = lov + num;
			if (isInNdock && num2 <= 97)
			{
				num += 3;
				num2 = lov + num;
			}
			if (num2 > 50)
			{
				num = ((!flagShip) ? (num - 3) : (num - 1));
				num2 = lov + num;
				if (num2 < 50)
				{
					num = 50 - num2;
				}
			}
			return this.SumLov(num);
		}

		public bool SumLovToBattle(BattleWinRankKinds rank, bool flagShip, bool mvp, int staHp, int endHp)
		{
			int num = 0;
			if (flagShip)
			{
				num = 3;
			}
			if (rank == BattleWinRankKinds.S)
			{
				num += 10;
			}
			else if (rank == BattleWinRankKinds.A)
			{
				num += 8;
			}
			else if (rank == BattleWinRankKinds.B)
			{
				num += 4;
			}
			else
			{
				num -= 5;
			}
			if (mvp)
			{
				num += 10;
			}
			DamageState damageState = Mem_ship.Get_DamageState(staHp, this.Maxhp);
			if (damageState == DamageState.Normal || damageState == DamageState.Shouha)
			{
				DamageState damageState2 = Mem_ship.Get_DamageState(endHp, this.Maxhp);
				if (damageState2 == DamageState.Tyuuha)
				{
					num -= 10;
				}
				else if (damageState2 == DamageState.Taiha)
				{
					num -= 20;
				}
			}
			return this.SumLov(num);
		}

		public bool SumLovToMission(MissionResultKinds kind)
		{
			int num = 0;
			if (kind == MissionResultKinds.SUCCESS)
			{
				num += 8;
			}
			else if (kind == MissionResultKinds.FAILE)
			{
				num -= 10;
			}
			else if (kind == MissionResultKinds.GREAT)
			{
				num += 10;
			}
			return this.SumLov(num);
		}

		public bool SumLovToKaisouPowUp(int eatNum)
		{
			return this.SumLov(eatNum);
		}

		public bool SumLovToMarriage()
		{
			return this.SumLov(100);
		}

		public bool SumLovToRemodeling()
		{
			return this.SumLov(30);
		}

		public bool SumLovToCharge()
		{
			return this.SumLov(3);
		}

		public bool SumLovToUseFoodSupplyShip(int useType)
		{
			int value = 0;
			if (useType == 1)
			{
				value = 5;
			}
			else if (useType == 2)
			{
				value = 4;
			}
			else if (useType == 3)
			{
				value = 10;
			}
			return this.SumLov(value);
		}

		private bool SumLov(int value)
		{
			if (value < 0)
			{
				return this.subLov(value);
			}
			return this.addLov(value);
		}

		private bool subLov(int value)
		{
			if (value == 0)
			{
				return false;
			}
			if (this.Lov <= 0)
			{
				return false;
			}
			this.Lov += value;
			if (this.Lov < 0)
			{
				this.Lov = 0;
			}
			return true;
		}

		private bool addLov(int value)
		{
			if (value == 0)
			{
				return false;
			}
			if (this.Lov >= 999)
			{
				return false;
			}
			this.Lov += value;
			if (this.Lov > 999)
			{
				this.Lov = 999;
			}
			return true;
		}

		protected override void setProperty(XElement element)
		{
			Mem_shipBase mem_shipBase = new Mem_shipBase();
			mem_shipBase.setProperty(element);
			this.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id), false);
		}
	}
}
