using local.utils;
using System;

namespace local.models
{
	public class Reward_PortExtend : IReward
	{
		public int ShipMaxNum
		{
			get
			{
				return Utils.ShipCountData().MaxCount;
			}
		}

		public int SlotMaxNum
		{
			get
			{
				return Utils.SlotitemCountData().MaxCount;
			}
		}

		public override string ToString()
		{
			return string.Format("母港拡張報酬: 最大艦数:{0} 最大装備数:{1}", this.ShipMaxNum, this.SlotMaxNum);
		}
	}
}
