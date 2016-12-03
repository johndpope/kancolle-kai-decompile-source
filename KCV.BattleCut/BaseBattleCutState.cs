using System;

namespace KCV.BattleCut
{
	public class BaseBattleCutState
	{
		public virtual bool Init(object data)
		{
			return false;
		}

		public virtual bool Run(object data)
		{
			return false;
		}

		public virtual bool Terminate(object data)
		{
			return false;
		}

		protected bool IsCheckPhase(BattleCutPhase iPhase)
		{
			return BattleCutManager.GetNowPhase() != iPhase;
		}
	}
}
