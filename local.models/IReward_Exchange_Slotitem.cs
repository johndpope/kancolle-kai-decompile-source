using System;

namespace local.models
{
	public interface IReward_Exchange_Slotitem : IReward
	{
		IReward_Slotitem ItemFrom
		{
			get;
		}

		IReward_Slotitem ItemTo
		{
			get;
		}
	}
}
