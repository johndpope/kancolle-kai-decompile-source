using System;
using System.Collections.Generic;

namespace Server_Common.Formats.Battle
{
	public class ExMapRewardInfo
	{
		private int _getExMapRate;

		private List<ItemGetFmt> _getExMapItem;

		public int GetExMapRate
		{
			get
			{
				return this._getExMapRate;
			}
			set
			{
				this._getExMapRate = value;
			}
		}

		public List<ItemGetFmt> GetExMapItem
		{
			get
			{
				return this._getExMapItem;
			}
			set
			{
				this._getExMapItem = value;
			}
		}
	}
}
