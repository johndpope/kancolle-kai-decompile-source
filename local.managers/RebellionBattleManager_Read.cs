using Common.Enum;
using local.models;
using Server_Common.Formats.Battle;
using System;

namespace local.managers
{
	public class RebellionBattleManager_Read : RebellionBattleManager, IBattleManager_Read
	{
		private AllBattleFmt _tmp_day;

		private AllBattleFmt _tmp_night;

		private BattleResultFmt _tmp_result;

		public RebellionBattleManager_Read(bool is_boss, bool last_cell, MapModel map) : base(string.Empty)
		{
			this._enemy_deck_id = -1;
			this._is_boss = is_boss;
			this._last_cell = last_cell;
			this._map = map;
			DebugBattleMaker.LoadBattleData(out this._tmp_day, out this._tmp_night, out this._tmp_result);
			BattleHeader header;
			if (this._tmp_day == null)
			{
				this._war_type = enumMapWarType.Midnight;
				this._phase = CombatPhase.NIGHT;
				this._battleData = this._tmp_night;
				header = this._battleData.NightBattle.Header;
			}
			else
			{
				this._war_type = enumMapWarType.Normal;
				this._phase = CombatPhase.DAY;
				this._battleData = this._tmp_day;
				header = this._battleData.DayBattle.Header;
				if (this._battleData.DayBattle.AirBattle2 != null)
				{
					this._war_type = enumMapWarType.AirBattle;
				}
			}
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
