using System;

namespace local.models
{
	public interface IReward_Materials : IReward
	{
		IReward_Material[] Rewards
		{
			get;
		}
	}
}
