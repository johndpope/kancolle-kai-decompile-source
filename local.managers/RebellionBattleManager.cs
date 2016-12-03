using Common.Enum;
using local.models;
using local.models.battle;
using Server_Controllers;
using System;

namespace local.managers
{
	public class RebellionBattleManager : SortieBattleManagerBase
	{
		public RebellionBattleManager(string enemy_deck_name) : base(enemy_deck_name)
		{
		}

		public virtual void __Init__(Api_req_SortieBattle reqBattle, enumMapWarType warType, BattleFormationKinds1 formationId, MapModel map, bool lastCell, bool isBoss, bool changeableDeck)
		{
			this._changeable_deck = changeableDeck;
			base.__Init__(reqBattle, warType, formationId, map, null, lastCell, isBoss);
		}

		public override CommandPhaseModel GetCommandPhaseModel()
		{
			if (this._cache_command == null && base.IsExistCommandPhase())
			{
				this._cache_command = new __CommandPhaseModel_Sortie__(this, this._reqBattle);
			}
			return this._cache_command;
		}
	}
}
