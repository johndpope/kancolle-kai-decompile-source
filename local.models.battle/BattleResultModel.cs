using Common.Enum;
using local.managers;
using Server_Common.Formats;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class BattleResultModel
	{
		private string _deck_name = string.Empty;

		private string _user_name = string.Empty;

		private BattleResultFmt _fmt;

		private List<int> _new_opened_area_ids;

		private List<int> _new_opened_map_ids;

		private List<ShipModel_BattleResult> _ships_f;

		private List<ShipModel_BattleResult> _ships_e;

		private int _hp_start_f;

		private int _hp_start_e;

		private int _hp_end_f;

		private int _hp_end_e;

		private bool _first_area_clear;

		private ShipModel_BattleResult _mvp_ship;

		public BattleWinRankKinds WinRank
		{
			get
			{
				return this._fmt.WinRank;
			}
		}

		public string DeckName
		{
			get
			{
				return this._deck_name;
			}
		}

		public string UserName
		{
			get
			{
				return this._user_name;
			}
		}

		public int UserLevel
		{
			get
			{
				return this._fmt.BasicLevel;
			}
		}

		public int SPoint
		{
			get
			{
				return this._fmt.GetSpoint;
			}
		}

		public ShipModel_BattleResult[] Ships_f
		{
			get
			{
				return this._ships_f.ToArray();
			}
		}

		public ShipModel_BattleResult[] Ships_e
		{
			get
			{
				return this._ships_e.ToArray();
			}
		}

		public string MapName
		{
			get
			{
				return this._fmt.QuestName;
			}
		}

		public int BaseExp
		{
			get
			{
				return this._fmt.GetBaseExp;
			}
		}

		public int HPStart_f
		{
			get
			{
				return this._hp_start_f;
			}
		}

		public int HPStart_e
		{
			get
			{
				return this._hp_start_e;
			}
		}

		public int HPEnd_f
		{
			get
			{
				return this._hp_end_f;
			}
		}

		public int HPEnd_e
		{
			get
			{
				return this._hp_end_e;
			}
		}

		public string EnemyName
		{
			get
			{
				return this._fmt.EnemyName;
			}
		}

		public bool FirstClear
		{
			get
			{
				return this._fmt.FirstClear;
			}
		}

		public bool FirstAreaClear
		{
			get
			{
				return this._first_area_clear;
			}
		}

		public int[] NewOpenAreaIDs
		{
			get
			{
				return this._new_opened_area_ids.ToArray();
			}
		}

		public int[] NewOpenMapIDs
		{
			get
			{
				return this._new_opened_map_ids.ToArray();
			}
		}

		public List<int> ReOpenMapIDs
		{
			get
			{
				return this._fmt.ReOpenMapId;
			}
		}

		public ShipModel_BattleResult MvpShip
		{
			get
			{
				return this._mvp_ship;
			}
		}

		public BattleResultModel(int deck_id, BattleManager bManager, BattleResultFmt fmt, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, MapModel map, Dictionary<int, int> exp_rates_before)
		{
			this._Init(deck_id, -1, bManager, fmt, ships_f, ships_e, exp_rates_before);
		}

		public BattleResultModel(int deck_id, int enemy_deck_id, BattleManager bManager, BattleResultFmt fmt, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Dictionary<int, int> exp_rates_before)
		{
			this._Init(deck_id, enemy_deck_id, bManager, fmt, ships_f, ships_e, exp_rates_before);
		}

		public List<IReward> GetRewardItems()
		{
			return this._ConvertItemGetFmts(this._fmt.GetItem);
		}

		public List<IReward> GetAreaRewardItems()
		{
			if (this._fmt.AreaClearRewardItem == null)
			{
				return null;
			}
			List<IReward> list = new List<IReward>();
			list.Add(this._ConvertItemGetFmt(this._fmt.AreaClearRewardItem));
			return list;
		}

		public List<MapEventItemModel> GetAirReconnaissanceItems()
		{
			if (this._fmt.GetAirReconnaissanceItems == null)
			{
				return null;
			}
			return this._fmt.GetAirReconnaissanceItems.ConvertAll<MapEventItemModel>((MapItemGetFmt i) => new MapEventItemModel(i));
		}

		private void _Init(int deck_id, int enemy_deck_id, BattleManager bManager, BattleResultFmt fmt, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Dictionary<int, int> exp_rates_before)
		{
			UserInfoModel userInfo = bManager.UserInfo;
			DeckModel deck = userInfo.GetDeck(deck_id);
			this._deck_name = deck.Name;
			this._user_name = userInfo.Name;
			this._fmt = fmt;
			this._ships_f = ships_f.ConvertAll<ShipModel_BattleResult>((ShipModel_BattleAll ship) => (ShipModel_BattleResult)ship);
			this._ships_e = ships_e.ConvertAll<ShipModel_BattleResult>((ShipModel_BattleAll ship) => (ShipModel_BattleResult)ship);
			this._mvp_ship = this._ships_f.Find((ShipModel_BattleResult ship) => ship != null && ship.TmpId == this._fmt.MvpShip);
			for (int i = 0; i < this.Ships_f.Length; i++)
			{
				ShipModel_BattleResult shipModel_BattleResult = this.Ships_f[i];
				if (shipModel_BattleResult != null)
				{
					this._SetShipExp(deck, shipModel_BattleResult, exp_rates_before);
					this._hp_start_f += shipModel_BattleResult.HpStart;
					this._hp_end_f += shipModel_BattleResult.HpEnd;
				}
			}
			DeckModel deck2 = userInfo.GetDeck(enemy_deck_id);
			for (int j = 0; j < this.Ships_e.Length; j++)
			{
				ShipModel_BattleResult shipModel_BattleResult2 = this.Ships_e[j];
				if (shipModel_BattleResult2 != null)
				{
					this._SetShipExp(deck2, shipModel_BattleResult2, exp_rates_before);
					this._hp_start_e += shipModel_BattleResult2.HpStart;
					this._hp_end_e += shipModel_BattleResult2.HpEnd;
				}
			}
			this._new_opened_map_ids = fmt.NewOpenMapId.GetRange(0, fmt.NewOpenMapId.get_Count());
			this._new_opened_map_ids.AddRange(fmt.ReOpenMapId);
			this._new_opened_area_ids = this._new_opened_map_ids.FindAll((int map_id) => map_id % 10 == 1);
			this._new_opened_area_ids = this._new_opened_area_ids.ConvertAll<int>((int map_id) => (int)Math.Floor((double)map_id / 10.0));
			this._first_area_clear = fmt.FirstAreaComplete;
		}

		private void _SetShipExp(DeckModel deck, ShipModel_BattleResult ship, Dictionary<int, int> exp_rates_before)
		{
			if (deck == null)
			{
				ship.__InitResultData__(0, null, 0, null);
				return;
			}
			ShipModel shipFromMemId = deck.GetShipFromMemId(ship.TmpId);
			int exp_rate_before = 0;
			exp_rates_before.TryGetValue(ship.TmpId, ref exp_rate_before);
			int exp = 0;
			this._fmt.GetShipExp.TryGetValue(ship.TmpId, out exp);
			List<int> levelup_info = null;
			this._fmt.LevelUpInfo.TryGetValue(ship.TmpId, out levelup_info);
			ship.__InitResultData__(exp_rate_before, shipFromMemId, exp, levelup_info);
		}

		private List<IReward> _ConvertItemGetFmts(List<ItemGetFmt> fmts)
		{
			if (fmts == null)
			{
				return new List<IReward>();
			}
			return fmts.ConvertAll<IReward>((ItemGetFmt item) => this._ConvertItemGetFmt(item));
		}

		private IReward _ConvertItemGetFmt(ItemGetFmt fmt)
		{
			if (fmt.Category == ItemGetKinds.Ship)
			{
				return new Reward_Ship(fmt.Id);
			}
			if (fmt.Category == ItemGetKinds.SlotItem)
			{
				return new Reward_Slotitem(fmt.Id, fmt.Count);
			}
			if (fmt.Category == ItemGetKinds.UseItem)
			{
				return new Reward_Useitem(fmt.Id, fmt.Count);
			}
			return null;
		}

		public string ToString(int[] values)
		{
			if (values == null)
			{
				return string.Empty;
			}
			string text = string.Empty;
			for (int i = 0; i < values.Length; i++)
			{
				text += values[i];
				if (i < values.Length - 1)
				{
					text += ",";
				}
			}
			return text;
		}

		public override string ToString()
		{
			string text = "-- BattleResultDTO --\n";
			text += string.Format("勝利ランク：{0}   提督名：{1}   戦闘後の提督レベル：{2}\n", this.WinRank, this.UserName, this.UserLevel);
			text += string.Format("海域名：{0} 味方艦隊名：{1}  敵艦隊名：{2}   海域基本経験値：{3}\n", new object[]
			{
				this.MapName,
				this.DeckName,
				this.EnemyName,
				this.BaseExp
			});
			for (int i = 0; i < this.Ships_f.Length; i++)
			{
				ShipModel_BattleResult shipModel_BattleResult = this.Ships_f[i];
				if (shipModel_BattleResult != null)
				{
					text += string.Format("[{0}] ID:({1}) {2} 状態:{3} {4} {5}\n", new object[]
					{
						i,
						shipModel_BattleResult.MstId,
						shipModel_BattleResult.Name,
						shipModel_BattleResult.DmgStateEnd,
						shipModel_BattleResult.ExpInfo,
						(shipModel_BattleResult != this.MvpShip) ? string.Empty : "[MVP]"
					});
				}
			}
			for (int j = 0; j < this.Ships_e.Length; j++)
			{
				ShipModel_BattleResult shipModel_BattleResult2 = this.Ships_e[j];
				if (shipModel_BattleResult2 != null)
				{
					text += string.Format("[{0}] ID:({1}) {2} 状態:{3}  種類(読み): {4} {5}\n", new object[]
					{
						j,
						shipModel_BattleResult2.MstId,
						shipModel_BattleResult2.Name,
						shipModel_BattleResult2.DmgStateEnd,
						shipModel_BattleResult2.Yomi,
						shipModel_BattleResult2.ExpInfo
					});
				}
			}
			text += string.Format("自分側HP {0}->{1}\n", this.HPStart_f, this.HPEnd_f);
			text += string.Format("相手側HP {0}->{1}\n", this.HPStart_e, this.HPEnd_e);
			List<IReward> list = this.GetRewardItems();
			if (list.get_Count() > 0)
			{
				text += "《ドロップあり》\n";
				for (int k = 0; k < list.get_Count(); k++)
				{
					text += string.Format("{0}\n", list.get_Item(k));
				}
			}
			else
			{
				text += "《ドロップなし》\n";
			}
			list = this.GetAreaRewardItems();
			if (list != null && list.get_Count() > 0)
			{
				text += "《海域クリア報酬あり》\n";
				for (int l = 0; l < list.get_Count(); l++)
				{
					text += string.Format("{0}\n", list.get_Item(l));
				}
			}
			else
			{
				text += "《海域クリア報酬なし》\n";
			}
			if (this.FirstClear || this.FirstAreaClear)
			{
				if (this.FirstClear)
				{
					text += string.Format("[初回マップクリア]", new object[0]);
				}
				if (this.FirstAreaClear)
				{
					text += string.Format("[初回海域クリア]", new object[0]);
				}
				text += string.Format("\n", new object[0]);
			}
			text += string.Format("-新開放海域-\n", new object[0]);
			for (int m = 0; m < this._new_opened_area_ids.get_Count(); m++)
			{
				text += string.Format("{0}\n", this._new_opened_area_ids.get_Item(m));
			}
			text += string.Format("-新開放マップ-\n", new object[0]);
			for (int n = 0; n < this._new_opened_map_ids.get_Count(); n++)
			{
				text += string.Format("{0}\n", this._new_opened_map_ids.get_Item(n));
			}
			text += string.Format("-獲得戦略ポイント:{0}\n", this.SPoint);
			return text + "---------------------";
		}
	}
}
