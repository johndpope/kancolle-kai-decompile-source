using Server_Common.Formats;
using System;

namespace local.models
{
	public class UserActionPhaseResultModel : PhaseResultModel
	{
		public UserActionPhaseResultModel(TurnWorkResult data) : base(data)
		{
		}

		public override string ToString()
		{
			return string.Format("[ユーザー行動フェーズ]: ", new object[0]);
		}
	}
}
