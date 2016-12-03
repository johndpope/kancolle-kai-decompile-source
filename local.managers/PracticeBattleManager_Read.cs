using Common.Enum;
using Server_Common.Formats.Battle;
using System;

namespace local.managers
{
	public class PracticeBattleManager_Read : PracticeBattleManager, IBattleManager_Read
	{
		private AllBattleFmt _tmp_day;

		private AllBattleFmt _tmp_night;

		private BattleResultFmt _tmp_result;

		public PracticeBattleManager_Read(int deck_id, int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			this._enemy_deck_id = enemy_deck_id;
			this._war_type = enumMapWarType.Normal;
			this._is_boss = false;
			DebugBattleMaker.LoadBattleData(out this._tmp_day, out this._tmp_night, out this._tmp_result);
			this._phase = CombatPhase.DAY;
			this._battleData = this._tmp_day;
			BattleHeader header = this._battleData.DayBattle.Header;
			this._ships_f = base._CreateShipData_f(header, false);
			this._ships_e = base._CreateShipData_e(header, false);
		}

		public override bool HasNightBattle()
		{
			return this._tmp_night != null && base.HasNightBattle();
		}

		public override void StartDayToNightBattle()
		{
			this._battleData = this._tmp_night;
			this._phase = CombatPhase.NIGHT;
			for (int i = 0; i < this._ships_f.get_Count(); i++)
			{
				if (this._ships_f.get_Item(i) != null)
				{
					this._ships_f.get_Item(i).__CreateStarter__();
				}
			}
			for (int j = 0; j < this._ships_e.get_Count(); j++)
			{
				if (this._ships_e.get_Item(j) != null)
				{
					this._ships_e.get_Item(j).__CreateStarter__();
				}
			}
		}

		protected override BattleResultFmt _GetBattleResult()
		{
			if (this._cache_result_fmt == null)
			{
				this._cache_result_fmt = this._tmp_result;
			}
			return this._cache_result_fmt;
		}
	}
}
