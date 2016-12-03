using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class BookSlotData : IBookData
	{
		private Mst_slotitem _mstData;

		private List<int> _enableStype;

		private string _info;

		public Mst_slotitem MstData
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

		public List<int> EnableStype
		{
			get
			{
				return this._enableStype;
			}
			private set
			{
				this._enableStype = value;
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

		public void SetBookData(int mst_id, string info, string cname, object hosoku)
		{
			this.MstData = Mst_DataManager.Instance.Mst_Slotitem.get_Item(mst_id);
			this.Info = info;
			this.EnableStype = ((Dictionary<int, List<int>>)hosoku).get_Item(this.MstData.Type3);
		}

		public int GetSortNo(int mst_id)
		{
			return Mst_DataManager.Instance.Mst_Slotitem.get_Item(mst_id).Sortno;
		}
	}
}
