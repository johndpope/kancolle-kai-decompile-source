using System;

namespace KCV.BattleCut
{
	public interface ISelectedObject<Index>
	{
		Index index
		{
			get;
		}

		bool isFocus
		{
			get;
			set;
		}

		bool isValid
		{
			get;
		}

		UIToggle toggle
		{
			get;
		}
	}
}
