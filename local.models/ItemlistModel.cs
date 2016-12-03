using Common.Enum;
using Server_Common;
using Server_Models;
using System;

namespace local.models
{
	public class ItemlistModel
	{
		private Mst_useitem _mst_data;

		private Mem_useitem _mem_data;

		private string _description = string.Empty;

		private int _override_count;

		public int MstId
		{
			get
			{
				return (this._mst_data != null) ? this._mst_data.Id : 0;
			}
		}

		public int Category
		{
			get
			{
				return (this._mst_data != null) ? this._mst_data.Category : 0;
			}
		}

		public string Name
		{
			get
			{
				return (this._mst_data != null) ? this._mst_data.Name : string.Empty;
			}
		}

		public string Description
		{
			get
			{
				if (this._description == null)
				{
					return string.Empty;
				}
				if (this.MstId == 53)
				{
					int portMaxExtendNum = Comm_UserDatas.Instance.User_basic.GetPortMaxExtendNum();
					int portMaxSlotItemNum = Comm_UserDatas.Instance.User_basic.GetPortMaxSlotItemNum();
					return string.Format(this._description, portMaxExtendNum, portMaxSlotItemNum);
				}
				return this._description;
			}
		}

		public string Description2
		{
			get
			{
				return (this._mst_data != null) ? this._mst_data.Description2 : string.Empty;
			}
		}

		public int Price
		{
			get
			{
				return (this._mst_data != null) ? this._mst_data.Price : 0;
			}
		}

		public int Count
		{
			get
			{
				if (this._override_count > 0)
				{
					return this._override_count;
				}
				return (this._mem_data != null) ? this._mem_data.Value : 0;
			}
		}

		public ItemlistModel(Mst_useitem mst_data, Mem_useitem mem_data, string description)
		{
			this._mst_data = mst_data;
			this._mem_data = mem_data;
			this._description = description;
		}

		public bool IsUsable()
		{
			return this._mst_data != null && this._mst_data.Usetype == 4;
		}

		public int GetSpendCountInUse(ItemExchangeKinds use_type)
		{
			if (this._mst_data == null)
			{
				return 0;
			}
			return this._mst_data.GetItemExchangeNum(use_type);
		}

		public void __SetOverrideCount__(int value)
		{
			this._override_count = value;
		}

		public override string ToString()
		{
			return string.Format("[{0}]{1} {2}個所有", this.MstId, this.Name, this.Count);
		}
	}
}
