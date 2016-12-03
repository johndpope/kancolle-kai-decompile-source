using Common.Struct;
using System;

namespace local.models
{
	public interface IReward_Ship : IReward
	{
		ShipModelMst Ship
		{
			get;
		}

		[Obsolete("Ship から取得してください", false)]
		string Name
		{
			get;
		}

		string GreetingText
		{
			get;
		}

		[Obsolete("Ship から取得してください", false)]
		Point Offset
		{
			get;
		}
	}
}
