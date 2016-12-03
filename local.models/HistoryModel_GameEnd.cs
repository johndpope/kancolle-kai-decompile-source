using Common.Enum;
using Server_Common.Formats;
using System;

namespace local.models
{
	public class HistoryModel_GameEnd : HistoryModelBase
	{
		public HistoryModel_GameEnd(User_HistoryFmt fmt) : base(fmt)
		{
		}

		public bool IsBeDefeated()
		{
			return base.Type == HistoryType.GameOverLost || base.Type == HistoryType.GameOverTurn;
		}

		public bool IsLostBaseArea()
		{
			return base.Type == HistoryType.GameOverLost;
		}
	}
}
