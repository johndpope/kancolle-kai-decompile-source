using Server_Models;
using System;

namespace Server_Common.Formats
{
	public class BookShipData : IBookData
	{
		private Mst_ship _mstData;

		private string _info;

		private string _className;

		public Mst_ship MstData
		{
			get
			{
				return this._mstData;
			}
			private set
			{
				this._mstData = value;
			}
		}

		public string Info
		{
			get
			{
				return this._info;
			}
			private set
			{
				this._info = value;
			}
		}

		public string ClassName
		{
			get
			{
				return this._className;
			}
			private set
			{
				this._className = value;
			}
		}

		public void SetBookData(int mst_id, string info, string cname, object hosoku)
		{
			this.MstData = Mst_DataManager.Instance.Mst_ship.get_Item(mst_id);
			this.Info = info;
			this.ClassName = cname;
		}

		public int GetSortNo(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_ship.get_Item(mst_id).Bookno;
		}
	}
}
