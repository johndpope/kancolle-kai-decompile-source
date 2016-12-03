using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers;
using System;

namespace local.managers
{
	public class RecordManager : ManagerBase
	{
		private User_RecordFmt _record_data;

		public string Name
		{
			get
			{
				return this._record_data.Nickname;
			}
		}

		public int Level
		{
			get
			{
				return this._record_data.Level;
			}
		}

		public int Rank
		{
			get
			{
				return base.UserInfo.Rank;
			}
		}

		public uint Experience
		{
			get
			{
				return this._record_data.Exp;
			}
		}

		public uint NextExperience
		{
			get
			{
				return this._record_data.Exp_next;
			}
		}

		public uint BattleCount
		{
			get
			{
				return this._record_data.War_total;
			}
		}

		public uint SortieWin
		{
			get
			{
				return this._record_data.War_win;
			}
		}

		public uint SortieLose
		{
			get
			{
				return this._record_data.War_lose;
			}
		}

		public uint InterceptSuccess
		{
			get
			{
				return this._record_data.War_RebellionWin;
			}
		}

		public double SortieRate
		{
			get
			{
				return this._record_data.War_rate;
			}
		}

		public uint DeckPractice
		{
			get
			{
				return this._record_data.DeckPracticeNum;
			}
		}

		public uint PracticeWin
		{
			get
			{
				return this._record_data.Practice_win;
			}
		}

		public uint PracticeLose
		{
			get
			{
				return this._record_data.Practice_lose;
			}
		}

		public int DeckCount
		{
			get
			{
				return this._record_data.Deck_num;
			}
		}

		public int ShipCount
		{
			get
			{
				return this._record_data.Ship_num;
			}
		}

		public int SlotitemCount
		{
			get
			{
				return this._record_data.Slot_num;
			}
		}

		public int NDockCount
		{
			get
			{
				return this._record_data.Ndock_num;
			}
		}

		public int KDockCount
		{
			get
			{
				return this._record_data.Kdock_num;
			}
		}

		public int FurnitureCount
		{
			get
			{
				return this._record_data.Furniture_num;
			}
		}

		public int DeckCountMax
		{
			get
			{
				return 8;
			}
		}

		public int ShipCountMax
		{
			get
			{
				return this._record_data.Ship_max;
			}
		}

		public int SlotitemCountMax
		{
			get
			{
				return this._record_data.Slot_max;
			}
		}

		public int MaterialMax
		{
			get
			{
				return this._record_data.Material_max;
			}
		}

		public RecordManager()
		{
			this._Update();
		}

		public bool IsCleardOnce()
		{
			return this.GetClearCount(DifficultKind.TEI) > 0 || this.GetClearCount(DifficultKind.HEI) > 0 || this.GetClearCount(DifficultKind.OTU) > 0 || this.GetClearCount(DifficultKind.KOU) > 0 || this.GetClearCount(DifficultKind.SHI) > 0;
		}

		public int GetClearCount(DifficultKind difficulty)
		{
			return Comm_UserDatas.Instance.User_plus.ClearNum(difficulty);
		}

		private void _Update()
		{
			Api_Result<User_RecordFmt> api_Result = new Api_get_Member().Record();
			if (api_Result.state == Api_Result_State.Success)
			{
				this._record_data = api_Result.data;
			}
			else
			{
				this._record_data = new User_RecordFmt();
			}
		}

		public override string ToString()
		{
			string text = "-- 戦績画面 --\n";
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" | 提督名 / レベル / 階級 : ",
				this.Name,
				" / ",
				this.Level,
				" / ",
				this.Rank,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" | 提督経験値 / 次レベル : ",
				this.Experience,
				" / ",
				this.NextExperience,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" | [出撃] 勝利数 / 敗北数 / 勝率 : ",
				this.SortieWin,
				" / ",
				this.SortieLose,
				" / ",
				this.SortieRate,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" | 保有艦隊数 : ",
				this.DeckCount,
				"    工廠ドック数 : ",
				this.KDockCount,
				"    入渠ドック数 : ",
				this.NDockCount,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" | 艦娘保有数 : ",
				this.ShipCount,
				"    装備アイテム保有数 : ",
				this.SlotitemCount,
				"    家具保有数 : ",
				this.FurnitureCount,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" | 最大所有可能艦隊数 : ",
				this.DeckCountMax,
				"    最大所有可能艦娘数 : ",
				this.ShipCountMax,
				"\n"
			});
			text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				" | 最大所有可能装備アイテム数 : ",
				this.SlotitemCountMax,
				"    最大備蓄可能各資材量 : ",
				this.MaterialMax,
				"\n"
			});
			if (this.IsCleardOnce())
			{
				text += " | クリア実勢: ";
				text += string.Format("丁-{0}回  丙-{1}回  乙-{2}回  甲-{3}回  史-{4}回\n", new object[]
				{
					this.GetClearCount(DifficultKind.TEI),
					this.GetClearCount(DifficultKind.HEI),
					this.GetClearCount(DifficultKind.OTU),
					this.GetClearCount(DifficultKind.KOU),
					this.GetClearCount(DifficultKind.SHI)
				});
			}
			else
			{
				text += " | クリア実績: 無し\n";
			}
			return text + "-- 戦績画面 --";
		}
	}
}
