using Common.Enum;
using local.models;
using local.models.battle;
using Server_Controllers;
using System;

namespace local.managers
{
	public class RebellionBattleManager_Write : RebellionBattleManager
	{
		public RebellionBattleManager_Write(string enemy_deck_name) : base(enemy_deck_name)
		{
		}

		public override void __Init__(Api_req_SortieBattle reqBattle, enumMapWarType warType, BattleFormationKinds1 formationId, MapModel map, bool lastCell, bool isBoss, bool changeableDeck)
		{
			this._changeable_deck = changeableDeck;
			base.__Init__(reqBattle, warType, formationId, map, null, lastCell, isBoss);
			if (warType == enumMapWarType.Normal || warType == enumMapWarType.AirBattle)
			{
				DebugBattleMaker.SerializeDayBattle(this._battleData);
			}
			else if (warType == enumMapWarType.Midnight)
			{
				DebugBattleMaker.SerializeNightBattle(this._battleData);
			}
			else if (warType == enumMapWarType.Night_To_Day)
			{
				DebugBattleMaker.SerializeNightBattle(this._battleData);
			}
		}

		public override void StartDayToNightBattle()
		{
			base.StartDayToNightBattle();
			DebugBattleMaker.SerializeNightBattle(this._battleData);
		}

		public override BattleResultModel GetBattleResult()
		{
			BattleResultModel battleResult = base.GetBattleResult();
			DebugBattleMaker.SerializeBattleResult(this._cache_result_fmt);
			return battleResult;
		}
	}
}
