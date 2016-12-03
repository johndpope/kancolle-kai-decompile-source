using System;

namespace local.models
{
	public interface IReward_Useitem : IReward
	{
		int Id
		{
			get;
		}

		string Name
		{
			get;
		}

		int Count
		{
			get;
		}
	}
}
