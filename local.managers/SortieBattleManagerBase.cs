using Common.Enum;
using local.models;
using local.models.battle;
using local.utils;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class SortieBattleManagerBase : BattleManager
	{
		protected Api_req_SortieBattle _reqBattle;

		private BattleResultModel _cache_result;

		private string _enemy_deck_name;

		public override string EnemyDeckName
		{
			get
			{
				return this._enemy_deck_name;
			}
		}

		public SortieBattleManagerBase(string enemy_deck_name)
		{
			this._enemy_deck_name = enemy_deck_name;
		}

		public virtual void __Init__(Api_req_SortieBattle reqBattle, enumMapWarType warType, BattleFormationKinds1 formationId, MapModel map, List<MapModel> maps, bool lastCell, bool isBoss)
		{
			this._recovery_item_use_count_at_start = Comm_UserDatas.Instance.User_trophy.Use_recovery_item_count;
			this._enemy_deck_id = -1;
			this._reqBattle = reqBattle;
			this._war_type = warType;
			this._is_boss = isBoss;
			this._last_cell = lastCell;
			this._map = map;
			this._maps = maps;
			BattleHeader header = null;
			if (warType == enumMapWarType.Normal || warType == enumMapWarType.AirBattle)
			{
				this._battleData = this._reqBattle.GetDayPreBattleInfo(formationId).data;
				this._phase = CombatPhase.DAY;
				header = this._battleData.DayBattle.Header;
			}
			else if (warType == enumMapWarType.Midnight)
			{
				this._battleData = this._reqBattle.Night_Sp(formationId).data;
				this._phase = CombatPhase.NIGHT;
				header = this._battleData.NightBattle.Header;
			}
			else if (warType != enumMapWarType.Night_To_Day)
			{
				throw new Exception("Logic Error");
			}
			this._ships_f = base._CreateShipData_f(header, false);
			this._ships_e = base._CreateShipData_e(header, false);
			if (this._phase == CombatPhase.DAY)
			{
				base.__createCacheDataBeforeCommand__();
			}
			else
			{
				base.__createCacheDataNight__();
			}
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
			DeckModel deck = base.UserInfo.GetDeck(base.DeckId);
			Dictionary<int, int> exp_rates_before = new Dictionary<int, int>();
			deck.__CreateShipExpRatesDictionary__(ref exp_rates_before);
			BattleResultFmt battleResultFmt = this._GetBattleResult();
			if (battleResultFmt == null)
			{
				return null;
			}
			this._cache_result = new BattleResultModel(base.DeckId, this, battleResultFmt, this._ships_f, this._ships_e, this._map, exp_rates_before);
			if (this._cache_result.WinRank == BattleWinRankKinds.S)
			{
				Comm_UserDatas.Instance.User_trophy.Win_S_count++;
			}
			List<int> reOpenMapIDs = this._cache_result.ReOpenMapIDs;
			if (reOpenMapIDs != null && reOpenMapIDs.get_Count() > 0)
			{
				for (int i = 0; i < reOpenMapIDs.get_Count(); i++)
				{
					int num = (int)Math.Floor((double)reOpenMapIDs.get_Item(i) / 10.0);
					int num2 = reOpenMapIDs.get_Item(i) % 10;
					if (base.Map.AreaId != num && num2 == 1)
					{
						TrophyUtil.__tmp_area_reopen__ = true;
					}
				}
			}
			return this._cache_result;
		}

		public override bool SendOffEscapes()
		{
			ShipModel[] escapeCandidate = base.GetEscapeCandidate();
			if (escapeCandidate.Length != 2)
			{
				return false;
			}
			ShipModel escapeShip = escapeCandidate[1];
			ShipModel towShip = escapeCandidate[0];
			if (escapeShip == null || towShip == null)
			{
				return false;
			}
			bool flag = this._reqBattle.GoBackPort(escapeShip.MemId, towShip.MemId);
			if (flag)
			{
				ShipModel_BattleAll shipModel_BattleAll = this._ships_f.Find((ShipModel_BattleAll item) => item != null && item.TmpId == towShip.MemId);
				if (shipModel_BattleAll != null)
				{
					shipModel_BattleAll.__UpdateEscapeStatus__(true);
				}
				ShipModel_BattleAll shipModel_BattleAll2 = this._ships_f.Find((ShipModel_BattleAll item) => item != null && item.TmpId == escapeShip.MemId);
				if (shipModel_BattleAll2 != null)
				{
					shipModel_BattleAll2.__UpdateEscapeStatus__(true);
				}
			}
			return flag;
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
