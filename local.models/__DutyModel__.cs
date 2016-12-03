using Server_Common.Formats;
using System;

namespace local.models
{
	public class __DutyModel__ : DutyModel
	{
		public User_QuestFmt Fmt
		{
			get
			{
				return this._fmt;
			}
		}

		public __DutyModel__(User_QuestFmt fmt) : base(fmt)
		{
		}
	}
}
