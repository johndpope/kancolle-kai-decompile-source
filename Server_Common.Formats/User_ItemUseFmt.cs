using Common.Enum;
using System;
using System.Collections.Generic;

namespace Server_Common.Formats
{
	public class User_ItemUseFmt
	{
		public bool CautionFlag;

		public List<ItemGetFmt> GetItem;

		public Dictionary<enumMaterialCategory, int> Material;

		public User_ItemUseFmt()
		{
			this.CautionFlag = false;
		}
	}
}
