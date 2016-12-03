using Common.Enum;
using Server_Common.Formats;
using System;

namespace local.models
{
	public class HistoryModel_TransportCraft : HistoryModel_AreaStart
	{
		public HistoryModel_TransportCraft(User_HistoryFmt fmt) : base(fmt)
		{
		}

		public bool IsBeAnnihilated()
		{
			return base.Type == HistoryType.TankerLostAll;
		}

		public bool IsBeReducedByHalf()
		{
			return base.Type == HistoryType.TankerLostHalf;
		}
	}
}
