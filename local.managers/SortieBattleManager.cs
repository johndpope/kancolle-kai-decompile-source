using local.models.battle;
using System;

namespace local.managers
{
	public class SortieBattleManager : SortieBattleManagerBase
	{
		public SortieBattleManager(string enemy_deck_name) : base(enemy_deck_name)
		{
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
