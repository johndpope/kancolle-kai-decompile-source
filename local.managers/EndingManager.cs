using Common.Enum;
using local.models;
using local.utils;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class EndingManager : ManagerBase
	{
		private Api_req_Ending _api;

		private bool _defeat_mode;

		private SortKey _selected_sort = SortKey.LEVEL_LOCK;

		private List<Mem_shipBase> _cache_takeover_ships;

		private List<Mem_slotitem> _cache_takeover_slots;

		public EndingManager()
		{
			this._defeat_mode = Server_Common.Utils.IsGameOver();
			this._api = new Api_req_Ending();
		}

		public EndingManager(bool is_defeat)
		{
			this._defeat_mode = is_defeat;
			this._api = new Api_req_Ending();
		}

		public List<User_HistoryFmt> CreateHistoryRawData()
		{
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_get_Member().HistoryList();
			if (api_Result.state == Api_Result_State.Success)
			{
				return api_Result.data;
			}
			return new List<User_HistoryFmt>();
		}

		public List<HistoryModelBase> CreateHistoryData()
		{
			List<HistoryModelBase> list = new List<HistoryModelBase>();
			Api_Result<List<User_HistoryFmt>> api_Result = new Api_get_Member().HistoryList();
			if (api_Result.state == Api_Result_State.Success)
			{
				for (int i = 0; i < api_Result.data.get_Count(); i++)
				{
					User_HistoryFmt user_HistoryFmt = api_Result.data.get_Item(i);
					if (user_HistoryFmt.Type == HistoryType.MapClear1 || user_HistoryFmt.Type == HistoryType.MapClear2 || user_HistoryFmt.Type == HistoryType.MapClear3)
					{
						list.Add(new HistoryModel_AreaClear(user_HistoryFmt));
					}
					else if (user_HistoryFmt.Type == HistoryType.NewAreaOpen)
					{
						list.Add(new HistoryModel_AreaStart(user_HistoryFmt));
					}
					else if (user_HistoryFmt.Type == HistoryType.TankerLostAll || user_HistoryFmt.Type == HistoryType.TankerLostHalf)
					{
						list.Add(new HistoryModel_TransportCraft(user_HistoryFmt));
					}
					else if (user_HistoryFmt.Type == HistoryType.GameClear || user_HistoryFmt.Type == HistoryType.GameOverLost || user_HistoryFmt.Type == HistoryType.GameOverTurn)
					{
						list.Add(new HistoryModel_GameEnd(user_HistoryFmt));
					}
				}
			}
			return list;
		}

		public bool IsGoTrueEnd()
		{
			return this._api.IsGoTrueEnd();
		}

		public List<ShipModel> CreateShipList()
		{
			return this.CreateShipList(20);
		}

		public List<ShipModel> CreateShipList(int count)
		{
			List<ShipModel> list = base.UserInfo.__GetShipList__();
			if (this._cache_takeover_ships == null || this._cache_takeover_ships.get_Count() > 0)
			{
			}
			count = Math.Min(count, list.get_Count());
			return DeckUtil.GetSortedList(list, SortKey.LEVEL).GetRange(0, count);
		}

		public int GetTakeoverShipCountMax()
		{
			if (this._defeat_mode)
			{
				return 0;
			}
			return this._api.GetTakeOverShipCount();
		}

		public int GetTakeoverSlotCountMax()
		{
			if (this._defeat_mode)
			{
				return 0;
			}
			return this._api.GetTakeOverSlotCount();
		}

		public int GetUserShipCount()
		{
			return Comm_UserDatas.Instance.User_ship.get_Count();
		}

		public int GetTakeoverShipCount()
		{
			return Math.Min(this.GetTakeoverShipCountMax(), this.GetUserShipCount());
		}

		public bool CreatePlusData(bool is_level_sort)
		{
			if (this._cache_takeover_ships != null)
			{
				return false;
			}
			this._api.CreateNewGamePlusData(is_level_sort);
			this._cache_takeover_ships = this._api.GetTakeOverShips();
			this._cache_takeover_slots = this._api.GetTakeOverSlotItems();
			this._selected_sort = ((!is_level_sort) ? SortKey.LOCK_LEVEL : SortKey.LEVEL_LOCK);
			return true;
		}

		public bool DeletePlusData()
		{
			if (this._cache_takeover_ships != null)
			{
				return false;
			}
			this._api.PurgeNewGamePlus();
			this._cache_takeover_ships = new List<Mem_shipBase>();
			this._cache_takeover_slots = new List<Mem_slotitem>();
			return true;
		}

		public uint GetLostShipCount()
		{
			return Comm_UserDatas.Instance.User_record.LostShipNum;
		}

		public void CalculateTotalRank(out OverallRank rank, out int decorationValue)
		{
			bool overallRank = this._api.GetOverallRank(out rank, out decorationValue);
			decorationValue *= ((!overallRank) ? -1 : 1);
		}

		public DifficultKind? GetOpenedDifficulty()
		{
			if (!Server_Common.Utils.IsGameClear())
			{
				return default(DifficultKind?);
			}
			if (this._cache_takeover_ships == null)
			{
				return default(DifficultKind?);
			}
			if (this._cache_takeover_ships.get_Count() == 0)
			{
				return default(DifficultKind?);
			}
			DifficultKind difficulty = base.UserInfo.Difficulty;
			DifficultKind difficultKind;
			if (difficulty == DifficultKind.OTU)
			{
				difficultKind = DifficultKind.KOU;
			}
			else
			{
				if (difficulty != DifficultKind.KOU)
				{
					return default(DifficultKind?);
				}
				difficultKind = DifficultKind.SHI;
			}
			int num = Comm_UserDatas.Instance.User_plus.ClearNum(difficulty);
			if (num == 1)
			{
				return new DifficultKind?(difficultKind);
			}
			return default(DifficultKind?);
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += ((!this._defeat_mode) ? "勝利" : "敗戦");
			text += "\n";
			List<HistoryModelBase> list = this.CreateHistoryData();
			if (list.get_Count() > 0)
			{
				text += " == 年表 ==\n";
			}
			for (int i = 0; i < list.get_Count(); i++)
			{
				text = text + "- " + list.get_Item(i).ToString() + "\n";
			}
			if (list.get_Count() > 0)
			{
				text += " == 年表終わり ==\n";
			}
			DifficultKind? openedDifficulty = this.GetOpenedDifficulty();
			if (openedDifficulty.get_HasValue())
			{
				text += string.Format(" [難易度] 新しい難易度{0}が開放しました。\n", openedDifficulty);
			}
			List<ShipModel> list2 = this.CreateShipList();
			if (list2.get_Count() > 0)
			{
				text += " == 上位20隻の艦 ==\n";
			}
			for (int j = 0; j < list2.get_Count(); j++)
			{
				ShipModel shipModel = list2.get_Item(j);
				text += string.Format("{0:D3}: {1}(mst:{2}, mem:{3}) Lv{4} sortNo:{5}\n", new object[]
				{
					j + 1,
					shipModel.Name,
					shipModel.MstId,
					shipModel.MemId,
					shipModel.Level,
					shipModel.SortNo
				});
			}
			if (list2.get_Count() > 0)
			{
				text += " == 上位20隻の艦終わり ==\n";
			}
			if (!this._defeat_mode)
			{
				text += string.Format("{0}作戦 勝利\n", (new string[]
				{
					string.Empty,
					"丁",
					"丙",
					"乙",
					"甲",
					"史"
				})[(int)base.UserInfo.Difficulty]);
				text += string.Format("作戦日数 : {0}日\n", base.Turn);
				text += string.Format("喪失艦娘 : {0}隻\n", this.GetLostShipCount());
				OverallRank overallRank;
				int num;
				this.CalculateTotalRank(out overallRank, out num);
				string text2 = (new string[]
				{
					string.Empty,
					"+",
					"++"
				})[num];
				text += string.Format("\t総合評価 : {0}{1}\n", overallRank, text2);
				text += "\n";
				if (this._cache_takeover_ships == null)
				{
					text += string.Format("= 引き継ぎ未選択 =\n", new object[0]);
					text += string.Format("引き継ぎ可能な艦:{0}隻, 装備:{1}個\n", this.GetTakeoverShipCount(), "?");
				}
				else
				{
					text += string.Format("= 引き継ぎ選択済 =\n", new object[0]);
					text += string.Format("引き継ぎ可能な艦:{0}隻, 装備:{1}個\n", this._cache_takeover_ships.get_Count(), this._cache_takeover_slots.get_Count());
				}
			}
			return text;
		}
	}
}
