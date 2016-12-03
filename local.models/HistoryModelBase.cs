using Common.Enum;
using Common.Struct;
using Server_Common.Formats;
using System;

namespace local.models
{
	public class HistoryModelBase
	{
		protected User_HistoryFmt _fmt;

		public HistoryType Type
		{
			get
			{
				return this._fmt.Type;
			}
		}

		public TurnString DateStruct
		{
			get
			{
				return this._fmt.DateString;
			}
		}

		public HistoryModelBase(User_HistoryFmt fmt)
		{
			this._fmt = fmt;
		}
	}
}
