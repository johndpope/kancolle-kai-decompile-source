using Common.Enum;
using Common.Struct;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public abstract class __ShipModelMem__ : ShipModelMst, IShipModel
	{
		protected Mem_ship _mem_data;

		public override int ShipType
		{
			get
			{
				return this._mem_data.Stype;
			}
		}

		public override int KaryokuMax
		{
			get
			{
				int num = this._mem_data.Houg - this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Houg);
				int num2 = this._mst_data.Houg_max - this._mst_data.Houg;
				return num + num2;
			}
		}

		public override int RaisouMax
		{
			get
			{
				int num = this._mem_data.Raig - this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Raig);
				int num2 = this._mst_data.Raig_max - this._mst_data.Raig;
				return num + num2;
			}
		}

		public override int TaikuMax
		{
			get
			{
				int num = this._mem_data.Taiku - this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Tyku);
				int num2 = this._mst_data.Tyku_max - this._mst_data.Tyku;
				return num + num2;
			}
		}

		public override int SoukouMax
		{
			get
			{
				int num = this._mem_data.Soukou - this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Souk);
				int num2 = this._mst_data.Souk_max - this._mst_data.Souk;
				return num + num2;
			}
		}

		public override int KaihiMax
		{
			get
			{
				int num = this._mem_data.Kaihi - this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Kaihi);
				int num2 = this._mst_data.Kaih_max - this._mst_data.Kaih;
				return num + num2;
			}
		}

		public override int TaisenMax
		{
			get
			{
				int num = this._mem_data.Taisen - this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Taisen);
				int num2 = this._mst_data.Tais_max - this._mst_data.Tais;
				return num + num2;
			}
		}

		public override int LuckyMax
		{
			get
			{
				int num = this._mem_data.Luck - this._mem_data.Kyouka.get_Item(Mem_ship.enumKyoukaIdx.Luck);
				int num2 = this._mst_data.Luck_max - this._mst_data.Luck;
				return num + num2;
			}
		}

		public override int Leng
		{
			get
			{
				return this._mem_data.Leng;
			}
		}

		public override int SlotCount
		{
			get
			{
				return this._mem_data.Slotnum;
			}
		}

		public int MemId
		{
			get
			{
				return this._mem_data.Rid;
			}
		}

		public int GetNo
		{
			get
			{
				return this._mem_data.GetNo;
			}
		}

		public int SortNo
		{
			get
			{
				return this._mem_data.Sortno;
			}
		}

		public int Level
		{
			get
			{
				return this._mem_data.Level;
			}
		}

		public int NowHp
		{
			get
			{
				return this._mem_data.Nowhp;
			}
		}

		public int MaxHp
		{
			get
			{
				return this._mem_data.Maxhp;
			}
		}

		public int Srate
		{
			get
			{
				return this._mem_data.Srate;
			}
		}

		public override int Karyoku
		{
			get
			{
				return this._mem_data.Houg;
			}
		}

		public override int Raisou
		{
			get
			{
				return this._mem_data.Raig;
			}
		}

		public override int Taiku
		{
			get
			{
				return this._mem_data.Taiku;
			}
		}

		public override int Soukou
		{
			get
			{
				return this._mem_data.Soukou;
			}
		}

		public override int Kaihi
		{
			get
			{
				return this._mem_data.Kaihi;
			}
		}

		public override int Taisen
		{
			get
			{
				return this._mem_data.Taisen;
			}
		}

		public int Sakuteki
		{
			get
			{
				return this._mem_data.Sakuteki;
			}
		}

		public override int Lucky
		{
			get
			{
				return this._mem_data.Luck;
			}
		}

		public int[] Tousai
		{
			get
			{
				return this._mem_data.Onslot.GetRange(0, this.SlotCount).ToArray();
			}
		}

		public int Fuel
		{
			get
			{
				return this._mem_data.Fuel;
			}
		}

		public int Ammo
		{
			get
			{
				return this._mem_data.Bull;
			}
		}

		public double FuelRate
		{
			get
			{
				return (double)this.Fuel * 100.0 / (double)base.FuelMax;
			}
		}

		public double AmmoRate
		{
			get
			{
				return (double)this.Ammo * 100.0 / (double)base.AmmoMax;
			}
		}

		public int Exp
		{
			get
			{
				return this._mem_data.Exp;
			}
		}

		public int Exp_Next
		{
			get
			{
				return this._mem_data.Exp_next;
			}
		}

		public int Exp_Percentage
		{
			get
			{
				return this._mem_data.Exp_rate;
			}
		}

		public double TaikyuRate
		{
			get
			{
				return (double)((this.MaxHp != 0) ? this._mem_data.Damage_Rate : 0);
			}
		}

		public DamageState DamageStatus
		{
			get
			{
				return this._mem_data.Get_DamageState();
			}
		}

		public int Condition
		{
			get
			{
				return this._mem_data.Cond;
			}
		}

		public FatigueState ConditionState
		{
			get
			{
				return this._mem_data.Get_FatigueState();
			}
		}

		public int Lov
		{
			get
			{
				return this._mem_data.Lov;
			}
		}

		public int BlingStartTurn
		{
			get
			{
				return this._mem_data.Bling_start;
			}
		}

		public int BlingEndTurn
		{
			get
			{
				return this._mem_data.Bling_end;
			}
		}

		public int BlingRemainingTurns
		{
			get
			{
				return this._mem_data.GetBlingTurn();
			}
		}

		public int AreaIdBeforeBlingWait
		{
			get
			{
				return (!this.IsBlingWait()) ? 0 : this._mem_data.BlingWaitArea;
			}
		}

		public string ShortName
		{
			get
			{
				return string.Format("{0}(mst:{1} mem:{2})", base.Name, base.MstId, this.MemId);
			}
		}

		public __ShipModelMem__(Mem_ship mem_ship)
		{
			this.SetMemData(mem_ship);
		}

		public __ShipModelMem__(int memID)
		{
			Api_get_Member arg_19_0 = new Api_get_Member();
			List<int> list = new List<int>();
			list.Add(memID);
			Api_Result<Dictionary<int, Mem_ship>> api_Result = arg_19_0.Ship(list);
			this._mem_data = api_Result.data.get_Item(memID);
			int ship_id = this._mem_data.Ship_id;
			this._mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(ship_id);
		}

		public bool LovAction(int type, int voiceID)
		{
			if (type == 0)
			{
				TouchLovInfo touchLovInfo = new TouchLovInfo(voiceID, false);
				return this._mem_data.SumLov(ref touchLovInfo);
			}
			if (type == 1)
			{
				TouchLovInfo touchLovInfo2 = new TouchLovInfo(voiceID, true);
				return this._mem_data.SumLov(ref touchLovInfo2);
			}
			return false;
		}

		public bool IsBling()
		{
			return this._mem_data.IsBlingShip();
		}

		public bool IsBlingWait()
		{
			return this._mem_data.IsBlingWait();
		}

		public bool IsBlingWaitFromDeck()
		{
			return this._mem_data.IsBlingWait() && this._mem_data.BlingType == Mem_ship.BlingKind.WaitDeck;
		}

		public bool IsBlingWaitFromEscortDeck()
		{
			return this._mem_data.IsBlingWait() && this._mem_data.BlingType == Mem_ship.BlingKind.WaitEscort;
		}

		public bool IsTettaiBling()
		{
			return this._mem_data.IsPortBack();
		}

		public void SetMemData(Mem_ship mem_ship)
		{
			this._mem_data = mem_ship;
			int ship_id = this._mem_data.Ship_id;
			this._mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(ship_id);
		}

		public bool IsDamaged()
		{
			DamageState damageState = this._mem_data.Get_DamageState();
			return damageState == DamageState.Tyuuha || damageState == DamageState.Taiha;
		}

		public bool IsTaiha()
		{
			return this._mem_data.Get_DamageState() == DamageState.Taiha;
		}

		public bool IsMarriage()
		{
			return this.Level >= 100;
		}

		public bool IsLocked()
		{
			return this._mem_data.Locked == 1;
		}

		public bool IsEscaped()
		{
			return this._mem_data.Escape_sts;
		}

		public string ToString(string slot_string)
		{
			string text = this.ShortName + "\n";
			text += slot_string;
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"Lv",
				this.Level,
				(!this.IsMarriage()) ? string.Empty : "(結婚済)",
				"    HP:",
				this.NowHp,
				"/",
				this.MaxHp,
				(!this.IsDamaged()) ? string.Empty : "(中破絵)",
				"    Exp",
				this.Exp,
				"/",
				this.Exp_Next,
				"(",
				this.Exp_Percentage,
				"%)    艦星数:",
				this.Srate,
				"    火力:",
				this.Karyoku,
				"    雷装:",
				this.Raisou,
				"    装甲:",
				this.Soukou,
				"    回避:",
				this.Kaihi,
				"    対潜:",
				this.Taisen,
				"    索的:",
				this.Sakuteki,
				"    運  :",
				this.Lucky,
				"    対空:",
				this.Taiku,
				"    搭載:",
				base.TousaiMaxAll,
				"    速力:",
				base.Soku,
				"    射程:",
				this.Leng,
				"    Lov値:",
				this.Lov,
				"\n"
			});
			text += "装備可能なカテゴリ:";
			using (List<SlotitemCategory>.Enumerator enumerator = base.GetEquipCategory().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SlotitemCategory current = enumerator.get_Current();
					text = text + " " + current;
				}
			}
			text += "\n";
			text += string.Format("立ち絵のID:{0}", base.GetGraphicsMstId());
			return text;
		}

		public void AddExp(int exp)
		{
			Debug_Mod arg_26_0 = new Debug_Mod();
			List<Mem_ship> list = new List<Mem_ship>();
			list.Add(this._mem_data);
			List<Mem_ship> arg_26_1 = list;
			List<int> list2 = new List<int>();
			list2.Add(exp);
			arg_26_0.ShipAddExp(arg_26_1, list2);
		}

		virtual int get_MstId()
		{
			return base.MstId;
		}

		virtual string get_ShipTypeName()
		{
			return base.ShipTypeName;
		}

		virtual string get_Name()
		{
			return base.Name;
		}
	}
}
