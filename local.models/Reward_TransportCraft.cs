using System;

namespace local.models
{
	public class Reward_TransportCraft : IReward
	{
		private int _num;

		public int Num
		{
			get
			{
				return this._num;
			}
		}

		public Reward_TransportCraft(int num)
		{
			this._num = num;
		}

		public override string ToString()
		{
			return string.Format("輸送船報酬: {0}隻", this._num);
		}
	}
}
