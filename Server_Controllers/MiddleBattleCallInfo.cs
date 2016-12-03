using Common.Enum;
using System;

namespace Server_Controllers
{
	public class MiddleBattleCallInfo
	{
		public enum CallType
		{
			None,
			Houg,
			Raig,
			LastRaig,
			EffectOnly
		}

		public int CommandPos;

		public BattleCommand UseCommand;

		public MiddleBattleCallInfo.CallType BattleType;

		public int AttackType;

		public MiddleBattleCallInfo(int commandPos, BattleCommand useCommand, MiddleBattleCallInfo.CallType callType, int attackType)
		{
			this.CommandPos = commandPos;
			this.UseCommand = useCommand;
			this.BattleType = callType;
			this.AttackType = attackType;
		}
	}
}
