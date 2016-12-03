using System;
using System.Collections.Generic;

namespace local.models
{
	public class Reward_Materials : IReward, IReward_Materials
	{
		private List<IReward_Material> _reward;

		public IReward_Material[] Rewards
		{
			get
			{
				return this._reward.ToArray();
			}
		}

		public Reward_Materials(List<IReward_Material> reward)
		{
			this._reward = reward;
		}

		public override string ToString()
		{
			string text = string.Empty;
			for (int i = 0; i < this._reward.get_Count(); i++)
			{
				text += string.Format("[{0}] ", this._reward.get_Item(i));
			}
			return text;
		}
	}
}
