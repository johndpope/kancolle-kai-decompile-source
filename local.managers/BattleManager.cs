using Common.Enum;
using local.models;
using local.models.battle;
using local.utils;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public abstract class BattleManager : ManagerBase
	{
		protected AllBattleFmt _battleData;

		protected CombatPhase _phase;

		protected List<ShipModel_BattleAll> _ships_f;

		protected List<ShipModel_BattleAll> _ships_e;

		protected int _ship_count_f;

		protected int _ship_count_e;

		protected SakutekiModel _cache_sakuteki;

		protected RationModel _cache_ration;

		protected CommandPhaseModel _cache_command;

		protected KoukuuModel _cache_kouku;

		protected IShienModel _cache_shien;

		protected RaigekiModel _cache_kaimaku;

		protected EffectModel _cache_opening_effect;

		protected List<CmdActionPhaseModel> _cache_cmd_actions;

		protected RaigekiModel _cache_raigeki;

		protected KoukuuModel _cache_kouku2;

		protected NightCombatModel _cache_opening_n;

		protected HougekiListModel _cache_hougeki_n;

		protected BattleResultFmt _cache_result_fmt;

		protected int _enemy_deck_id;

		protected enumMapWarType _war_type;

		protected bool _is_boss;

		protected bool _last_cell;

		protected MapModel _map;

		protected List<MapModel> _maps;

		protected bool _changeable_deck;

		protected int _recovery_item_use_count_at_start;

		protected int _recovery_item_use_count_in_battle;

		public int DeckId
		{
			get
			{
				return this._GetCurrentHeader().F_DeckShip1.Deck_Id;
			}
		}

		public int EnemyDeckId
		{
			get
			{
				return this._enemy_deck_id;
			}
		}

		public virtual string EnemyDeckName
		{
			get
			{
				return string.Empty;
			}
		}

		public ShipModel_BattleAll[] Ships_f
		{
			get
			{
				return this._ships_f.ToArray();
			}
		}

		public ShipModel_BattleAll[] Ships_e
		{
			get
			{
				return this._ships_e.ToArray();
			}
		}

		public int ShipCount_f
		{
			get
			{
				return this._ship_count_f;
			}
		}

		public int ShipCount_e
		{
			get
			{
				return this._ship_count_e;
			}
		}

		public BattleFormationKinds1 FormationId_f
		{
			get
			{
				return this._battleData.Formation[0];
			}
		}

		public BattleFormationKinds1 FormationId_e
		{
			get
			{
				return this._battleData.Formation[1];
			}
		}

		public BattleFormationKinds2 CrossFormationId
		{
			get
			{
				return this._battleData.BattleFormation;
			}
		}

		public bool IsPractice
		{
			get
			{
				return this._enemy_deck_id != -1;
			}
		}

		public enumMapWarType WarType
		{
			get
			{
				return this._war_type;
			}
		}

		public bool BossBattle
		{
			get
			{
				return this._is_boss;
			}
		}

		public string AreaName
		{
			get
			{
				return ManagerBase._area.get_Item(this._map.AreaId).Name;
			}
		}

		public MapModel Map
		{
			get
			{
				return this._map;
			}
		}

		public List<MapModel> Maps
		{
			get
			{
				return this._maps;
			}
		}

		public bool ChangeableDeck
		{
			get
			{
				return this._changeable_deck;
			}
		}

		public int RecoveryItemUseCountAtStart
		{
			get
			{
				return this._recovery_item_use_count_at_start;
			}
		}

		public int RecoveryItemUseCountInBattle
		{
			get
			{
				return this._recovery_item_use_count_in_battle;
			}
		}

		public BattleManager()
		{
		}

		public int GetBgmId()
		{
			bool flag;
			return this.GetBgmId(this._phase == CombatPhase.DAY, this.BossBattle, out flag);
		}

		public int GetBgmId(bool is_day, bool is_boss, out bool master_loaded)
		{
			int num = (!is_day) ? 1 : 0;
			int num2 = (!is_boss) ? 0 : 1;
			Dictionary<int, List<int>> battleBgm = Mst_DataManager.Instance.UiBattleMaster.BattleBgm;
			master_loaded = (battleBgm != null);
			if (master_loaded)
			{
				return battleBgm.get_Item(num).get_Item(num2);
			}
			return (!is_day) ? 2 : 1;
		}

		public ShipModel_BattleAll GetShip(int tmp_id)
		{
			List<ShipModel_BattleAll> list = new List<ShipModel_BattleAll>(this.Ships_f);
			list.AddRange(this.Ships_e);
			return list.Find((ShipModel_BattleAll ship) => ship != null && ship.TmpId == tmp_id);
		}

		public ShipModel_BattleAll GetShip(int index, bool is_friend)
		{
			List<ShipModel_BattleAll> list = (!is_friend) ? this._ships_e : this._ships_f;
			if (0 <= index && index < list.get_Count())
			{
				ShipModel_BattleAll shipModel_BattleAll = list.get_Item(index);
				if (shipModel_BattleAll != null)
				{
					return shipModel_BattleAll;
				}
			}
			return null;
		}

		public BossInsertModel GetBossInsertData()
		{
			return (!this._is_boss) ? null : new BossInsertModel(this.Ships_e[0]);
		}

		public bool IsFiascoSakuteki()
		{
			if (this._phase != CombatPhase.DAY)
			{
				return false;
			}
			if (this._battleData.DayBattle.Search == null)
			{
				return false;
			}
			SearchInfo searchInfo = this._battleData.DayBattle.Search[0];
			return searchInfo.UsePlane.get_Count() == 0 && (searchInfo.SearchValue == BattleSearchValues.Lost || searchInfo.SearchValue == BattleSearchValues.Faile || searchInfo.SearchValue == BattleSearchValues.NotFound);
		}

		public bool IsNotFiascoSakuteki()
		{
			if (this._phase != CombatPhase.DAY)
			{
				return false;
			}
			if (this._battleData.DayBattle.Search == null)
			{
				return false;
			}
			SearchInfo searchInfo = this._battleData.DayBattle.Search[0];
			return searchInfo.UsePlane.get_Count() > 0 || (searchInfo.SearchValue == BattleSearchValues.Success || searchInfo.SearchValue == BattleSearchValues.Success_Lost || searchInfo.SearchValue == BattleSearchValues.Found);
		}

		public bool IsExistSakutekiData()
		{
			return this._phase == CombatPhase.DAY;
		}

		public SakutekiModel GetSakutekiData()
		{
			if (this._cache_sakuteki == null && this._phase == CombatPhase.DAY)
			{
				this._cache_sakuteki = new SakutekiModel(this._battleData.DayBattle.Search, this._ships_f, this._ships_e);
			}
			return this._cache_sakuteki;
		}

		public bool IsExistRationPhase()
		{
			return this._cache_ration != null;
		}

		public RationModel GetRationModel()
		{
			return this._cache_ration;
		}

		public bool IsExistCommandPhase()
		{
			return true;
		}

		public bool IsValidPresetCommand()
		{
			CommandPhaseModel commandPhaseModel = this.GetCommandPhaseModel();
			return commandPhaseModel != null && commandPhaseModel.IsValidCommand(commandPhaseModel.GetPresetCommand());
		}

		public bool IsTakeCommand()
		{
			return this._cache_command != null && this._cache_command.IsTakeCommand();
		}

		public abstract CommandPhaseModel GetCommandPhaseModel();

		public EffectModel GetOpeningEffectData()
		{
			return this._cache_opening_effect;
		}

		public EffectModel GetEffectData(int index)
		{
			if (this._cache_cmd_actions != null && index < this._cache_cmd_actions.get_Count())
			{
				CmdActionPhaseModel cmdActionPhaseModel = this._cache_cmd_actions.get_Item(index);
				if (cmdActionPhaseModel != null && cmdActionPhaseModel.Effect != null)
				{
					return cmdActionPhaseModel.Effect;
				}
			}
			return null;
		}

		public bool IsExistKoukuuData()
		{
			return this._cache_kouku != null;
		}

		public KoukuuModel GetKoukuuData()
		{
			return this._cache_kouku;
		}

		public bool IsExistShienData()
		{
			return this._cache_shien != null;
		}

		public IShienModel GetShienData()
		{
			return this._cache_shien;
		}

		public bool IsExistKaimakuData()
		{
			return this._cache_kaimaku != null;
		}

		public RaigekiModel GetKaimakuData()
		{
			return this._cache_kaimaku;
		}

		public bool IsExistHougekiPhase_Day()
		{
			return this._cache_cmd_actions != null;
		}

		public List<CmdActionPhaseModel> GetHougekiData_Day()
		{
			return this._cache_cmd_actions;
		}

		public CmdActionPhaseModel GetHougekiData_Day(int index)
		{
			if (this._cache_cmd_actions != null && index < this._cache_cmd_actions.get_Count())
			{
				return this._cache_cmd_actions.get_Item(index);
			}
			return null;
		}

		public bool IsExistHougekiPhase_Night()
		{
			return this._cache_hougeki_n != null;
		}

		public HougekiListModel GetHougekiList_Night()
		{
			return this._cache_hougeki_n;
		}

		public bool IsExistRaigekiData()
		{
			return this._cache_raigeki != null;
		}

		public RaigekiModel GetRaigekiData()
		{
			return this._cache_raigeki;
		}

		public bool IsExistKoukuuData2()
		{
			return this._cache_kouku2 != null;
		}

		public KoukuuModel GetKoukuuData2()
		{
			return this._cache_kouku2;
		}

		public virtual bool HasNightBattle()
		{
			return this._phase == CombatPhase.DAY && this._battleData.DayBattle.ValidMidnight;
		}

		public NightCombatModel GetNightCombatData()
		{
			return this._cache_opening_n;
		}

		public virtual void StartDayToNightBattle()
		{
		}

		public abstract BattleResultModel GetBattleResult();

		public ShipModel_BattleAll[] GetSubMarineShips_f()
		{
			if (this._ships_f == null)
			{
				return new ShipModel_BattleAll[0];
			}
			return this._ships_f.FindAll((ShipModel_BattleAll ship) => ship.IsSubMarine()).ToArray();
		}

		public ShipModel_BattleAll[] GetSubMarineShips_e()
		{
			if (this._ships_e == null)
			{
				return new ShipModel_BattleAll[0];
			}
			return this._ships_e.FindAll((ShipModel_BattleAll ship) => ship.IsSubMarine()).ToArray();
		}

		public bool IsSubMarineAllShip_f()
		{
			return this._ships_f != null && this._ships_f.get_Count() == this.GetSubMarineShips_f().Length;
		}

		public bool IsSubMarineAllShip_e()
		{
			return this._ships_e != null && this._ships_e.get_Count() == this.GetSubMarineShips_e().Length;
		}

		public bool HasPossibilityTaihaShingun()
		{
			return !(this is PracticeBattleManager) && (this._ships_f.get_Item(0).HasRecoverMegami() || this._ships_f.get_Item(0).HasRecoverYouin()) && !this._last_cell;
		}

		public ShipModel[] GetEscapeCandidate()
		{
			if (this._cache_result_fmt != null && this._cache_result_fmt.EscapeInfo != null && this._cache_result_fmt.EscapeInfo.TowShips.get_Count() > 0 && this._cache_result_fmt.EscapeInfo.EscapeShips.get_Count() > 0)
			{
				int ship_mem_id = this._cache_result_fmt.EscapeInfo.TowShips.get_Item(0);
				ShipModel ship = base.UserInfo.GetShip(ship_mem_id);
				int ship_mem_id2 = this._cache_result_fmt.EscapeInfo.EscapeShips.get_Item(0);
				ShipModel ship2 = base.UserInfo.GetShip(ship_mem_id2);
				if (ship != null && ship2 != null)
				{
					return new ShipModel[]
					{
						ship,
						ship2
					};
				}
			}
			return null;
		}

		public virtual bool SendOffEscapes()
		{
			return false;
		}

		public ShipRecoveryType IsUseRecoverySlotitem(int ship_tmp_id)
		{
			bool flag = this._battleData.DayBattle != null && this.GetCommandPhaseModel().IsTakeCommand();
			bool flag2 = this._battleData.NightBattle != null;
			if (!flag && !flag2)
			{
				return ShipRecoveryType.None;
			}
			ShipModel_BattleAll ship = this.GetShip(ship_tmp_id);
			if (ship == null)
			{
				return ShipRecoveryType.None;
			}
			return ship.IsUseRecoverySlotitem();
		}

		public ShipRecoveryType IsUseRecoverySlotitem(int ship_tmp_id, bool is_day)
		{
			if (is_day)
			{
				if (this._battleData.DayBattle == null || !this.GetCommandPhaseModel().IsTakeCommand())
				{
					return ShipRecoveryType.None;
				}
				ShipModel_BattleAll ship = this.GetShip(ship_tmp_id);
				if (ship == null)
				{
					return ShipRecoveryType.None;
				}
				return ship.IsUseRecoverySlotitemAtFirstCombat();
			}
			else
			{
				if (this._battleData.NightBattle == null)
				{
					return ShipRecoveryType.None;
				}
				ShipModel_BattleAll ship2 = this.GetShip(ship_tmp_id);
				if (ship2 == null)
				{
					return ShipRecoveryType.None;
				}
				if (this._battleData.DayBattle == null)
				{
					return ship2.IsUseRecoverySlotitemAtFirstCombat();
				}
				return ship2.IsUseRecoverySlotitemAtSecondCombat();
			}
		}

		public void IncrementRecoveryItemCountWithTrophyUnlock()
		{
			this._recovery_item_use_count_in_battle++;
			TrophyUtil.Unlock_At_Battle(this._recovery_item_use_count_at_start, this._recovery_item_use_count_in_battle);
		}

		protected Dictionary<int, ShipModel_BattleAll> _GetShipsDic()
		{
			Dictionary<int, ShipModel_BattleAll> dictionary = new Dictionary<int, ShipModel_BattleAll>();
			for (int i = 0; i < this._ships_f.get_Count(); i++)
			{
				if (this._ships_f.get_Item(i) != null)
				{
					dictionary.set_Item(this._ships_f.get_Item(i).TmpId, this._ships_f.get_Item(i));
				}
			}
			for (int j = 0; j < this._ships_e.get_Count(); j++)
			{
				if (this._ships_e.get_Item(j) != null)
				{
					dictionary.set_Item(this._ships_e.get_Item(j).TmpId, this._ships_e.get_Item(j));
				}
			}
			return dictionary;
		}

		protected List<ShipModel_BattleAll> _CreateShipData_f(BattleHeader header, bool practice)
		{
			BattleShipFmt[] ships = header.F_DeckShip1.Ships;
			return this._CreateShipData(ships, true, practice, out this._ship_count_f);
		}

		protected List<ShipModel_BattleAll> _CreateShipData_e(BattleHeader header, bool practice)
		{
			BattleShipFmt[] ships = header.E_DeckShip1.Ships;
			return this._CreateShipData(ships, false, practice, out this._ship_count_e);
		}

		protected List<ShipModel_BattleAll> _CreateShipData(BattleShipFmt[] ship_fmts, bool is_friend, bool practice, out int count)
		{
			count = 0;
			List<ShipModel_BattleAll> list = new List<ShipModel_BattleAll>();
			for (int i = 0; i < 6; i++)
			{
				BattleShipFmt battleShipFmt = ship_fmts[i];
				if (battleShipFmt == null)
				{
					list.Add(null);
				}
				else
				{
					ShipModel_BattleAll shipModel_BattleAll = new ShipModel_BattleResult(battleShipFmt, i, is_friend, practice);
					list.Add(shipModel_BattleAll);
					count++;
				}
			}
			return list;
		}

		protected virtual BattleResultFmt _GetBattleResult()
		{
			return null;
		}

		protected BattleHeader _GetCurrentHeader()
		{
			return (this._phase != CombatPhase.DAY) ? this._battleData.NightBattle.Header : this._battleData.DayBattle.Header;
		}

		public void __createCacheDataBeforeCommand__()
		{
			if (this._phase != CombatPhase.DAY)
			{
				return;
			}
			if (this.IsTakeCommand())
			{
				return;
			}
			this._cache_ration = null;
			if (this._GetRationData() != null)
			{
				this._cache_ration = new RationModel(this._ships_f, this._GetRationData());
			}
		}

		public void __createCacheDataAfterCommand__()
		{
			if (this._phase != CombatPhase.DAY)
			{
				return;
			}
			if (!this.IsTakeCommand())
			{
				return;
			}
			this._cache_opening_effect = null;
			if (this._battleData.DayBattle.OpeningProduction != null)
			{
				this._cache_opening_effect = new __EffectModel__(this._battleData.DayBattle.OpeningProduction);
			}
			this._cache_kouku = null;
			if (this._battleData.DayBattle.AirBattle != null)
			{
				int count = this._battleData.DayBattle.AirBattle.F_PlaneFrom.get_Count();
				int count2 = this._battleData.DayBattle.AirBattle.E_PlaneFrom.get_Count();
				if (count > 0 || count2 > 0)
				{
					this._cache_kouku = new KoukuuModel(this._ships_f, this._ships_e, this._battleData.DayBattle.AirBattle);
				}
			}
			this._cache_shien = null;
			if (this._battleData.DayBattle.SupportAtack != null)
			{
				SupportAtack supportAtack = this._battleData.DayBattle.SupportAtack;
				int deck_Id = supportAtack.Deck_Id;
				DeckModel deck = base.UserInfo.GetDeck(deck_Id);
				BattleSupportKinds supportType = supportAtack.SupportType;
				if (supportType == BattleSupportKinds.AirAtack)
				{
					this._cache_shien = new ShienModel_Air(deck, this._ships_f, this._ships_e, supportAtack);
				}
				else if (supportType == BattleSupportKinds.Hougeki)
				{
					this._cache_shien = new ShienModel_Hou(deck, this._ships_f, this._ships_e, supportAtack);
				}
				else if (supportType == BattleSupportKinds.Raigeki)
				{
					this._cache_shien = new ShienModel_Rai(deck, this._ships_f, this._ships_e, supportAtack);
				}
			}
			this._cache_kaimaku = null;
			if (this._battleData.DayBattle.OpeningAtack != null)
			{
				this._cache_kaimaku = new RaigekiModel(this._ships_f, this._ships_e, this._battleData.DayBattle.OpeningAtack);
				if (this._cache_kaimaku.Count_f == 0 && this._cache_kaimaku.Count_e == 0)
				{
					this._cache_kaimaku = null;
				}
			}
			this._cache_cmd_actions = null;
			if (this._battleData.DayBattle.FromMiddleDayBattle != null)
			{
				this._cache_cmd_actions = new List<CmdActionPhaseModel>();
				Dictionary<int, ShipModel_BattleAll> ships = this._GetShipsDic();
				for (int i = 0; i < this._battleData.DayBattle.FromMiddleDayBattle.get_Count(); i++)
				{
					FromMiddleBattleDayData data = this._battleData.DayBattle.FromMiddleDayBattle.get_Item(i);
					CmdActionPhaseModel cmdActionPhaseModel = new CmdActionPhaseModel(data, ships);
					this._cache_cmd_actions.Add(cmdActionPhaseModel);
				}
				if (this._cache_cmd_actions.TrueForAll((CmdActionPhaseModel model) => model == null || !model.HasAction()))
				{
					this._cache_cmd_actions = null;
				}
				else if (this._cache_cmd_actions.get_Count() == 0)
				{
					this._cache_cmd_actions = null;
				}
			}
			this._cache_raigeki = null;
			if (this._battleData.DayBattle.Raigeki != null)
			{
				this._cache_raigeki = new RaigekiModel(this._ships_f, this._ships_e, this._battleData.DayBattle.Raigeki);
				if (this._cache_raigeki.Count_f == 0 && this._cache_raigeki.Count_e == 0)
				{
					this._cache_raigeki = null;
				}
			}
			this._cache_kouku2 = null;
			if (this._battleData.DayBattle.AirBattle2 != null)
			{
				int count3 = this._battleData.DayBattle.AirBattle2.F_PlaneFrom.get_Count();
				int count4 = this._battleData.DayBattle.AirBattle2.E_PlaneFrom.get_Count();
				if (count3 > 0 || count4 > 0)
				{
					this._cache_kouku2 = new KoukuuModel(this._ships_f, this._ships_e, this._battleData.DayBattle.AirBattle2);
				}
			}
			if (this._cache_opening_effect != null)
			{
				ShipModel_Battle nextActionShip = this._GetFirstActionShip(0);
				((__EffectModel__)this._cache_opening_effect).SetNextActionShip(nextActionShip);
			}
			if (this._cache_cmd_actions != null)
			{
				for (int j = 0; j < this._cache_cmd_actions.get_Count(); j++)
				{
					CmdActionPhaseModel cmdActionPhaseModel2 = this._cache_cmd_actions.get_Item(j);
					if (cmdActionPhaseModel2 != null && cmdActionPhaseModel2.Effect != null)
					{
						ShipModel_Battle nextActionShip = this._GetFirstActionShip(j + 1);
						((__EffectModel__)cmdActionPhaseModel2.Effect).SetNextActionShip(nextActionShip);
					}
				}
			}
		}

		public void __createCacheDataNight__()
		{
			if (this._phase != CombatPhase.NIGHT)
			{
				return;
			}
			RationModel ration = null;
			if (this._GetRationData() != null)
			{
				ration = new RationModel(this._ships_f, this._GetRationData());
			}
			this._cache_opening_n = new NightCombatModel(this, this._battleData.NightBattle, ration);
			this._cache_hougeki_n = null;
			if (this._battleData.NightBattle.Hougeki != null)
			{
				List<Hougeki<BattleAtackKinds_Night>> hougeki = this._battleData.NightBattle.Hougeki;
				this._cache_hougeki_n = new HougekiListModel(hougeki, this._GetShipsDic());
			}
		}

		private ShipModel_Battle _GetFirstActionShip(int order)
		{
			if (order <= 0)
			{
				if (this.IsExistKoukuuData())
				{
					ShipModel_Battle firstActionShip = this.GetKoukuuData().GetFirstActionShip();
					if (firstActionShip != null)
					{
						return firstActionShip;
					}
				}
				if (this.IsExistShienData())
				{
					return null;
				}
				if (this.IsExistKaimakuData())
				{
					ShipModel_Battle firstActionShip2 = this.GetKaimakuData().GetFirstActionShip();
					if (firstActionShip2 != null)
					{
						return firstActionShip2;
					}
				}
				if (this.GetEffectData(0) != null)
				{
					return null;
				}
			}
			if (order <= 1)
			{
				CmdActionPhaseModel hougekiData_Day = this.GetHougekiData_Day(0);
				if (hougekiData_Day != null)
				{
					ShipModel_Battle firstActionShip3 = hougekiData_Day.GetFirstActionShip();
					if (firstActionShip3 != null)
					{
						return firstActionShip3;
					}
					if (this.GetEffectData(1) != null)
					{
						return null;
					}
				}
			}
			if (order <= 2)
			{
				CmdActionPhaseModel hougekiData_Day2 = this.GetHougekiData_Day(1);
				if (hougekiData_Day2 != null)
				{
					ShipModel_Battle firstActionShip4 = hougekiData_Day2.GetFirstActionShip();
					if (firstActionShip4 != null)
					{
						return firstActionShip4;
					}
					if (this.GetEffectData(2) != null)
					{
						return null;
					}
				}
			}
			if (order <= 3)
			{
				CmdActionPhaseModel hougekiData_Day3 = this.GetHougekiData_Day(2);
				if (hougekiData_Day3 != null)
				{
					ShipModel_Battle firstActionShip5 = hougekiData_Day3.GetFirstActionShip();
					if (firstActionShip5 != null)
					{
						return firstActionShip5;
					}
					if (this.GetEffectData(3) != null)
					{
						return null;
					}
				}
			}
			if (order <= 4)
			{
				CmdActionPhaseModel hougekiData_Day4 = this.GetHougekiData_Day(3);
				if (hougekiData_Day4 != null)
				{
					ShipModel_Battle firstActionShip6 = hougekiData_Day4.GetFirstActionShip();
					if (firstActionShip6 != null)
					{
						return firstActionShip6;
					}
					if (this.GetEffectData(4) != null)
					{
						return null;
					}
				}
			}
			if (order <= 5)
			{
				CmdActionPhaseModel hougekiData_Day5 = this.GetHougekiData_Day(4);
				if (hougekiData_Day5 != null)
				{
					ShipModel_Battle firstActionShip7 = hougekiData_Day5.GetFirstActionShip();
					if (firstActionShip7 != null)
					{
						return firstActionShip7;
					}
				}
				if (this.IsExistRaigekiData())
				{
					ShipModel_Battle firstActionShip7 = this.GetRaigekiData().GetFirstActionShip();
					if (firstActionShip7 != null)
					{
						return firstActionShip7;
					}
				}
				if (this.IsExistKoukuuData2())
				{
					ShipModel_Battle firstActionShip7 = this.GetKoukuuData2().GetFirstActionShip();
					if (firstActionShip7 != null)
					{
						return firstActionShip7;
					}
				}
			}
			return null;
		}

		private Dictionary<int, List<Mst_slotitem>> _GetRationData()
		{
			if (this._phase == CombatPhase.DAY)
			{
				return this._battleData.DayBattle.Header.UseRationShips;
			}
			return this._battleData.NightBattle.Header.UseRationShips;
		}

		public override string ToString()
		{
			string text = "\n";
			bool flag;
			int bgmId = this.GetBgmId(true, this.BossBattle, out flag);
			bool flag2;
			int bgmId2 = this.GetBgmId(false, this.BossBattle, out flag2);
			text += string.Format("[BGM - 昼] {0}{1}\t[BGM - 夜] {2}{3}\n", new object[]
			{
				bgmId,
				(!flag) ? "(マスタ未ロード)" : string.Empty,
				bgmId2,
				(!flag2) ? "(マスタ未ロード)" : string.Empty
			});
			for (int i = 0; i < this.Ships_f.Length; i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = this.Ships_f[i];
				if (shipModel_BattleAll != null)
				{
					text += string.Format("[{0}] {1}\n", i, shipModel_BattleAll);
				}
				else
				{
					text += string.Format("[{0}] -\n", i);
				}
			}
			text += string.Format("== 「{0}」 ==\n", this.EnemyDeckName);
			for (int j = 0; j < this.Ships_e.Length; j++)
			{
				ShipModel_BattleAll shipModel_BattleAll2 = this.Ships_e[j];
				if (shipModel_BattleAll2 != null)
				{
					text += string.Format("[{0}] {1}\n", j, shipModel_BattleAll2);
				}
				else
				{
					text += string.Format("[{0}] -\n", j);
				}
			}
			return text + string.Format("自側陣形:{0} 敵側陣形:{1} 交戦体系:{2}", this.FormationId_f, this.FormationId_e, this.CrossFormationId);
		}
	}
}
