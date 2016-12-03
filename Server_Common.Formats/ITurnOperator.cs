using System;

namespace Server_Common.Formats
{
	public interface ITurnOperator
	{
		TurnWorkResult ExecTurnStateChange();
	}
}
