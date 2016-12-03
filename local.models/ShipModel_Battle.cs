using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public abstract class ShipModel_Battle : ShipModelMst, IShipModel
	{
		protected __ShipModel_Battle_BaseData__ _base_data;

		public virtual int TmpId
		{
			get
			{
				return this._base_data.Fmt.Id;
			}
		}

		public int Index
		{
			get
			{
				return this._base_data.Index;
			}
		}

		public virtual int Level
		{
			get
			{
				return this._base_data.Fmt.Level;
			}
		}

		public virtual int MaxHp
		{
			get
			{
				return this._base_data.Fmt.MaxHp;
			}
		}

		public virtual int ParamKaryoku
		{
			get
			{
				return this._base_data.Fmt.BattleParam.Houg;
			}
		}

		public virtual int ParamRaisou
		{
			get
			{
				return this._base_data.Fmt.BattleParam.Raig;
			}
		}

		public virtual int ParamTaiku
		{
			get
			{
				return this._base_data.Fmt.BattleParam.Taiku;
			}
		}

		public virtual int ParamSoukou
		{
			get
			{
				return this._base_data.Fmt.BattleParam.Soukou;
			}
		}

		public ShipModel_Battle()
		{
		}

		public bool IsFriend()
		{
			return this._base_data.IsFriend;
		}

		public bool IsPractice()
		{
			return this._base_data.IsPractice;
		}

		public bool IsSubMarine()
		{
			return Mst_DataManager.Instance.Mst_stype.get_Item(this.ShipType).IsSubmarine();
		}

		public bool IsAircraftCarrier()
		{
			return Mst_DataManager.Instance.Mst_stype.get_Item(this.ShipType).IsMother();
		}

		public virtual bool IsEscape()
		{
			return this._base_data.Fmt.EscapeFlag;
		}

		public bool HasSlotEx()
		{
			return this._base_data.Fmt.ExSlot >= 0;
		}

		protected int _GetHp(int hp)
		{
			if (this._base_data.IsPractice)
			{
				return Math.Max(hp, 1);
			}
			return Math.Max(hp, 0);
		}

		protected DamageState_Battle _GetDmgState(int hp)
		{
			if (hp <= 0)
			{
				return DamageState_Battle.Gekichin;
			}
			DamageState damageState = Mem_ship.Get_DamageState(hp, this.MaxHp);
			if (damageState == DamageState.Taiha)
			{
				return DamageState_Battle.Taiha;
			}
			if (damageState == DamageState.Tyuuha)
			{
				return DamageState_Battle.Tyuuha;
			}
			if (damageState == DamageState.Shouha)
			{
				return DamageState_Battle.Shouha;
			}
			return DamageState_Battle.Normal;
		}

		protected bool _GetDamagedFlg(DamageState_Battle state)
		{
			return state == DamageState_Battle.Tyuuha || state == DamageState_Battle.Taiha || state == DamageState_Battle.Gekichin;
		}

		protected string _ToString(List<SlotitemModel_Battle> items, SlotitemModel_Battle item_ex)
		{
			string text = "(";
			for (int i = 0; i < items.get_Count(); i++)
			{
				SlotitemModel_Battle item = items.get_Item(i);
				text += this._ToString(item);
			}
			if (this.HasSlotEx())
			{
				text = text + " ex:" + this._ToString(item_ex);
			}
			else
			{
				text += " [X]";
			}
			return text + ")";
		}

		protected string _ToString(SlotitemModel_Battle item)
		{
			string result;
			if (item == null)
			{
				result = "[-]";
			}
			else
			{
				result = string.Format("[{0}({1})]", item.Name, item.MstId);
			}
			return result;
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
