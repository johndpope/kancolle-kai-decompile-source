using System;

namespace KCV.Strategy
{
	public interface IRebellionOrganizeSelectObject
	{
		int index
		{
			get;
		}

		UIButton button
		{
			get;
		}

		UIToggle toggle
		{
			get;
		}

		DelDicideRebellionOrganizeSelectBtn delDicideRebellionOrganizeSelectBtn
		{
			get;
		}
	}
}
