using Server_Common;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_slotitem", Namespace = "")]
	public class Mem_slotitem : Model_Base, IReqNewGetNo
	{
		public interface IMemSlotIdOperator
		{
			void ChangeSlotId(Mem_slotitem obj, int changeId);
		}

		[DataContract(Namespace = "")]
		public enum enumEquipSts
		{
			[EnumMember]
			Unset,
			[EnumMember]
			Equip
		}

		[DataMember]
		private int _rid;

		[DataMember]
		private int _getNO;

		[DataMember]
		private int _slotitem_id;

		[DataMember]
		private Mem_slotitem.enumEquipSts _equip_flag;

		[DataMember]
		private int _equip_rid;

		[DataMember]
		private bool _lock;

		[DataMember]
		private int _level;

		[DataMember]
		private int _experience;

		private static string _tableName = "mem_slotitem";

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
				return this._getNO;
			}
			private set
			{
				this._getNO = value;
			}
		}

		public int Slotitem_id
		{
			get
			{
				return this._slotitem_id;
			}
			private set
			{
				this._slotitem_id = value;
			}
		}

		public Mem_slotitem.enumEquipSts Equip_flag
		{
			get
			{
				return this._equip_flag;
			}
			private set
			{
				this._equip_flag = value;
			}
		}

		public int Equip_rid
		{
			get
			{
				return this._equip_rid;
			}
			private set
			{
				this._equip_rid = value;
			}
		}

		public bool Lock
		{
			get
			{
				return this._lock;
			}
			private set
			{
				this._lock = value;
			}
		}

		public int Level
		{
			get
			{
				return this._level;
			}
			set
			{
				this._level = value;
			}
		}

		public int Experience
		{
			get
			{
				return this._experience;
			}
			private set
			{
				this._experience = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_slotitem._tableName;
			}
		}

		public bool Set_New_SlotData(int rid, int getNo, int mst_id)
		{
			if (!Mst_DataManager.Instance.Mst_Slotitem.ContainsKey(mst_id))
			{
				return false;
			}
			this.Rid = rid;
			this.GetNo = getNo;
			this.Slotitem_id = mst_id;
			this.Equip_flag = Mem_slotitem.enumEquipSts.Unset;
			this.Equip_rid = 0;
			this.Lock = false;
			this.Level = 0;
			this.Experience = Mst_DataManager.Instance.Mst_Slotitem.get_Item(this.Slotitem_id).Default_exp;
			return true;
		}

		public int GetSkillLevel()
		{
			if (this.Experience >= 0 && this.Experience <= 9)
			{
				return 0;
			}
			if (this.Experience >= 10 && this.Experience <= 24)
			{
				return 1;
			}
			if (this.Experience >= 25 && this.Experience <= 39)
			{
				return 2;
			}
			if (this.Experience >= 40 && this.Experience <= 54)
			{
				return 3;
			}
			if (this.Experience >= 55 && this.Experience <= 69)
			{
				return 4;
			}
			if (this.Experience >= 70 && this.Experience <= 84)
			{
				return 5;
			}
			if (this.Experience >= 85 && this.Experience <= 99)
			{
				return 6;
			}
			return 7;
		}

		public bool IsMaxSkillLevel()
		{
			return this.GetSkillLevel() == 7;
		}

		public int GetSortNo()
		{
			return this._getNO;
		}

		public void ChangeSortNo(int no)
		{
			this._getNO = no;
		}

		public void Equip(int ship_rid)
		{
			this.Equip_flag = Mem_slotitem.enumEquipSts.Equip;
			this.Equip_rid = ship_rid;
		}

		public void UnEquip()
		{
			this.Equip_flag = Mem_slotitem.enumEquipSts.Unset;
			this.Equip_rid = 0;
		}

		public void LockChange()
		{
			this.Lock = !this.Lock;
		}

		public void SetLevel(int setVal)
		{
			this.Level = setVal;
		}

		public void ChangeExperience(int changeValue)
		{
			this.Experience += changeValue;
			if (this.Experience > 120)
			{
				this.Experience = 120;
			}
			if (this.Experience <= 0)
			{
				this.Experience = 0;
			}
		}

		public void ChangeSlotId(Mem_slotitem.IMemSlotIdOperator instance, int changeId)
		{
			if (instance == null)
			{
				return;
			}
			this.Slotitem_id = changeId;
			Comm_UserDatas.Instance.Add_Book(2, changeId);
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.GetNo = int.Parse(element.Element("_getNO").get_Value());
			this.Slotitem_id = int.Parse(element.Element("_slotitem_id").get_Value());
			this.Equip_flag = (Mem_slotitem.enumEquipSts)((int)Enum.Parse(typeof(Mem_slotitem.enumEquipSts), element.Element("_equip_flag").get_Value()));
			this.Equip_rid = int.Parse(element.Element("_equip_rid").get_Value());
			this.Lock = bool.Parse(element.Element("_lock").get_Value());
			this.Level = int.Parse(element.Element("_level").get_Value());
			if (element.Element("_experience") == null)
			{
				this.Experience = Mst_DataManager.Instance.Mst_Slotitem.get_Item(this.Slotitem_id).Default_exp;
			}
			else
			{
				this.Experience = int.Parse(element.Element("_experience").get_Value());
			}
		}
	}
}
