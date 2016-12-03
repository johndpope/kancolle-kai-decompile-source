using Common.Enum;
using local.models;
using local.models.battle;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class PracticeBattleManager : BattleManager
	{
		private Api_req_PracticeBattle _reqBattle;

		private BattleResultModel _cache_result;

		public override string EnemyDeckName
		{
			get
			{
				return base.UserInfo.GetDeck(base.EnemyDeckId).Name;
			}
		}

		public virtual void __Init__(int deck_id, int enemy_deck_id, BattleFormationKinds1 formation_id)
		{
			this._recovery_item_use_count_at_start = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			this._enemy_deck_id = enemy_deck_id;
			this._war_type = enumMapWarType.Normal;
			this._is_boss = false;
			this._last_cell = true;
			this._reqBattle = new Api_req_PracticeBattle(deck_id, enemy_deck_id);
			this._battleData = this._reqBattle.GetDayPreBattleInfo(formation_id).data;
			this._phase = CombatPhase.DAY;
			BattleHeader header = this._battleData.DayBattle.Header;
			this._ships_f = base._CreateShipData_f(header, true);
			this._ships_e = base._CreateShipData_e(header, true);
			base.__createCacheDataBeforeCommand__();
		}

		public override CommandPhaseModel GetCommandPhaseModel()
		{
			if (this._cache_command == null && base.IsExistCommandPhase())
			{
				this._cache_command = new __CommandPhaseModel_Prac__(this, this._reqBattle);
			}
			return this._cache_command;
		}

		public override void StartDayToNightBattle()
		{
			this._battleData = this._reqBattle.NightBattle().data;
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
			base.__createCacheDataNight__();
		}

		public override BattleResultModel GetBattleResult()
		{
			if (this._cache_result != null)
			{
				return this._cache_result;
			}
			Dictionary<int, int> exp_rates_before = new Dictionary<int, int>();
			DeckModel deck = base.UserInfo.GetDeck(base.DeckId);
			deck.__CreateShipExpRatesDictionary__(ref exp_rates_before);
			deck = base.UserInfo.GetDeck(base.EnemyDeckId);
			deck.__CreateShipExpRatesDictionary__(ref exp_rates_before);
			BattleResultFmt battleResultFmt = this._GetBattleResult();
			if (battleResultFmt == null)
			{
				return null;
			}
			this._cache_result = new BattleResultModel(base.DeckId, base.EnemyDeckId, this, battleResultFmt, this._ships_f, this._ships_e, exp_rates_before);
			return this._cache_result;
		}

		protected override BattleResultFmt _GetBattleResult()
		{
			if (this._cache_result_fmt == null)
			{
				Api_Result<BattleResultFmt> api_Result = this._reqBattle.BattleResult();
				if (api_Result.state == Api_Result_State.Success)
				{
					this._cache_result_fmt = api_Result.data;
				}
			}
			return this._cache_result_fmt;
		}
	}
}
