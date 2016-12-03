using System;

namespace local.models
{
	public class Reward_SPoint : IReward
	{
		private int _spoint;

		public int SPoint
		{
			get
			{
				return this._spoint;
			}
		}

		public Reward_SPoint(int point)
		{
			this._spoint = point;
		}

		public override string ToString()
		{
			return string.Format("戦略ポイント報酬: {0}", this.SPoint);
		}
	}
}
