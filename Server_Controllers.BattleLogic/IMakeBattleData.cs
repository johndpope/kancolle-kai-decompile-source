using System;

namespace Server_Controllers.BattleLogic
{
	public interface IMakeBattleData
	{
		void GetBattleResultBase(BattleResultBase out_data);
	}
}
