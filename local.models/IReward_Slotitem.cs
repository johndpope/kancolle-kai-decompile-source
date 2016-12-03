using System;

namespace local.models
{
	public interface IReward_Slotitem : IReward
	{
		int Id
		{
			get;
		}

		string Name
		{
			get;
		}

		int Rare
		{
			get;
		}

		string Type3Name
		{
			get;
		}

		int Count
		{
			get;
		}
	}
}
