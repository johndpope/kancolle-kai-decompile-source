using System;

namespace KCV.Strategy
{
	public class StrategyUtils
	{
		public static bool ChkStateRebellionTaskIsRun(StrategyRebellionTaskManagerMode iMode)
		{
			return StrategyTaskManager.GetStrategyRebellion().GetMode() == StrategyRebellionTaskManagerMode.StrategyRebellionTaskManager_BEF || StrategyTaskManager.GetStrategyRebellion().GetMode() == iMode;
		}
	}
}
