using System;

namespace local.models
{
	public class Reward_LargeBuild : IReward
	{
		public override string ToString()
		{
			return string.Format("大型艦建造開放", new object[0]);
		}
	}
}
