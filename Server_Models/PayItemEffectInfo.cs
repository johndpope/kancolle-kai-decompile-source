using System;

namespace Server_Models
{
	public class PayItemEffectInfo
	{
		public int Type;

		public int MstId;

		public int Count;

		public PayItemEffectInfo(int[] itemData)
		{
			this.Type = itemData[0];
			this.MstId = itemData[1];
			this.Count = itemData[2];
		}
	}
}
