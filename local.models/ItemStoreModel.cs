using Server_Common;
using Server_Models;
using System;

namespace local.models
{
	public class ItemStoreModel
	{
		private Mst_payitem _mst_data;

		public int MstId
		{
			get
			{
				return (this._mst_data != null) ? this._mst_data.Id : 0;
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
				if (this._mst_data == null)
				{
					return string.Empty;
				}
				if (this.MstId == 16)
				{
					int portMaxExtendNum = Comm_UserDatas.Instance.User_basic.GetPortMaxExtendNum();
					return string.Format(this._mst_data.Description, portMaxExtendNum);
				}
				return this._mst_data.Description;
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
				return (this._mst_data != null) ? this._mst_data.GetBuyNum() : 0;
			}
		}

		public ItemStoreModel(Mst_payitem mst_data)
		{
			this._mst_data = mst_data;
		}

		public override string ToString()
		{
			string result;
			if (this.Count < 0)
			{
				result = string.Format("[{0}]{1}  価格{2}", this.MstId, this.Name, this.Price);
			}
			else if (this.Count == 0)
			{
				result = string.Format("[{0}][購入不可]{1}  価格{2}", this.MstId, this.Name, this.Price);
			}
			else
			{
				result = string.Format("[{0}][残{3}]{1}  価格{2}", new object[]
				{
					this.MstId,
					this.Name,
					this.Price,
					this.Count
				});
			}
			return result;
		}
	}
}
