using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class __ShipModel_Attacker__ : ShipModel_Attacker
	{
		private ShipModel _baseModel;

		public override int TmpId
		{
			get
			{
				return this._baseModel.MemId;
			}
		}

		public override int Level
		{
			get
			{
				return this._baseModel.Level;
			}
		}

		public override int MaxHp
		{
			get
			{
				return this._baseModel.MaxHp;
			}
		}

		public override int ParamKaryoku
		{
			get
			{
				return 0;
			}
		}

		public override int ParamRaisou
		{
			get
			{
				return 0;
			}
		}

		public override int ParamTaiku
		{
			get
			{
				return 0;
			}
		}

		public override int ParamSoukou
		{
			get
			{
				return 0;
			}
		}

		public override int Hp
		{
			get
			{
				return this._baseModel.NowHp;
			}
		}

		public __ShipModel_Attacker__(ShipModel baseModel, int index) : base(null, null, 0, null, null)
		{
			this._mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(baseModel.MstId);
			this._baseModel = baseModel;
			this._base_data = new __ShipModel_Battle_BaseData__();
			this._base_data.Index = base.Index;
			this._base_data.IsFriend = true;
			this._base_data.IsPractice = false;
			this._slotitems = new List<SlotitemModel_Battle>();
			List<SlotitemModel> slotitemList = baseModel.SlotitemList;
			for (int i = 0; i < slotitemList.get_Count(); i++)
			{
				if (slotitemList.get_Item(i) == null)
				{
					this._slotitems.Add(null);
				}
				else
				{
					this._slotitems.Add(new SlotitemModel_Battle(slotitemList.get_Item(i).MstId));
				}
			}
			if (this._baseModel.HasExSlot() && this._baseModel.SlotitemEx != null)
			{
				int mstId = this._baseModel.SlotitemEx.MstId;
				this._slotitemex = new SlotitemModel_Battle(mstId);
			}
		}

		public override bool IsEscape()
		{
			return false;
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
			return text + base._ToString(base.SlotitemList, base.SlotitemEx);
		}
	}
}
