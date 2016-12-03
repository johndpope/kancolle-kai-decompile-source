using Common.Enum;
using System;

namespace local.models
{
	public interface IReward_Material : IReward
	{
		string Name
		{
			get;
		}

		enumMaterialCategory Type
		{
			get;
		}

		int Count
		{
			get;
		}
	}
}
