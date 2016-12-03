using Server_Models;
using System;

namespace local.models
{
	public class SlotitemModel : SlotitemModel_Mst, ISlotitemModel
	{
		private Mem_slotitem _data;

		public int MemId
		{
			get
			{
				return this._data.Rid;
			}
		}

		public int GetNo
		{
			get
			{
				return this._data.GetNo;
			}
		}

		public override int MstId
		{
			get
			{
				return this._data.Slotitem_id;
			}
		}

		public int Level
		{
			get
			{
				return this._data.Level;
			}
		}

		public int SkillLevel
		{
			get
			{
				return this._data.GetSkillLevel();
			}
		}

		public override string ShortName
		{
			get
			{
				return string.Format("{0}(MstId:{1} MemId:{2} Lv:{3} c:{4}{5})", new object[]
				{
					base.Name,
					this.MstId,
					this.MemId,
					this.Level,
					base.GetCost(),
					(!base.IsPlane()) ? string.Empty : "[艦載機]"
				});
			}
		}

		public SlotitemModel(Mem_slotitem data) : base(data.Slotitem_id)
		{
			this._data = data;
		}

		public bool IsLocked()
		{
			return this._data.Lock;
		}

		public bool IsEauiped()
		{
			return this._data.Equip_flag == Mem_slotitem.enumEquipSts.Equip;
		}

		public void __update__()
		{
			if (this._data.Slotitem_id != this._mst.Id)
			{
				this._mst = Mst_DataManager.Instance.Mst_Slotitem.get_Item(this._data.Slotitem_id);
			}
		}

		public override string ToString()
		{
			return string.Format("[{0}{1}]", this.ShortName, (!this.IsLocked()) ? string.Empty : "(Lock)");
		}
	}
}
