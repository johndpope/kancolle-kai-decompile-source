using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.utils
{
	public class UseitemUtil
	{
		private Dictionary<int, Mem_useitem> _data;

		public UseitemUtil()
		{
			Api_Result<Dictionary<int, Mem_useitem>> api_Result = new Api_get_Member().UseItem();
			if (api_Result.state == Api_Result_State.Success)
			{
				this._data = api_Result.data;
			}
			else
			{
				this._data = null;
			}
		}

		public int GetCount(int useitem_id)
		{
			if (this._data != null && this._data.ContainsKey(useitem_id))
			{
				return this._data.get_Item(useitem_id).Value;
			}
			return 0;
		}
	}
}
