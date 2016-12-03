using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_record", Namespace = "")]
	public class Mem_record : Model_Base
	{
		[DataMember]
		private int _level;

		[DataMember]
		private uint _exp;

		[DataMember]
		private uint _st_win;

		[DataMember]
		private uint _st_lose;

		[DataMember]
		private uint _rebellion_win;

		[DataMember]
		private uint _pt_win;

		[DataMember]
		private uint _pt_lose;

		[DataMember]
		private uint _deck_practice_num;

		[DataMember]
		private double _rate;

		[DataMember]
		private List<DifficultKind> _clearDifficult;

		[DataMember]
		private uint _lostShipNum;

		private static string _tableName = "mem_record";

		public int Level
		{
			get
			{
				return this._level;
			}
			private set
			{
				this._level = value;
			}
		}

		public uint Exp
		{
			get
			{
				return this._exp;
			}
			private set
			{
				this._exp = value;
			}
		}

		public uint St_win
		{
			get
			{
				return this._st_win;
			}
			private set
			{
				this._st_win = value;
			}
		}

		public uint St_lose
		{
			get
			{
				return this._st_lose;
			}
			private set
			{
				this._st_lose = value;
			}
		}

		public uint Rebellion_win
		{
			get
			{
				return this._rebellion_win;
			}
			private set
			{
				this._rebellion_win = value;
			}
		}

		public uint Pt_win
		{
			get
			{
				return this._pt_win;
			}
			private set
			{
				this._pt_win = value;
			}
		}

		public uint Pt_lose
		{
			get
			{
				return this._pt_lose;
			}
			private set
			{
				this._pt_lose = value;
			}
		}

		public uint Deck_practice_num
		{
			get
			{
				return this._deck_practice_num;
			}
			private set
			{
				this._deck_practice_num = value;
			}
		}

		public double Rate
		{
			get
			{
				return this._rate;
			}
			private set
			{
				this._rate = value;
			}
		}

		public List<DifficultKind> ClearDifficult
		{
			get
			{
				return this._clearDifficult;
			}
			private set
			{
				this._clearDifficult = value;
			}
		}

		public uint LostShipNum
		{
			get
			{
				return this._lostShipNum;
			}
			private set
			{
				this._lostShipNum = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_record._tableName;
			}
		}

		public Mem_record()
		{
			this.Level = 1;
			this.Exp = 0u;
			this.St_win = 0u;
			this.Pt_win = 0u;
			this.ClearDifficult = new List<DifficultKind>();
			this.Rebellion_win = 0u;
			this.Deck_practice_num = 0u;
			this.LostShipNum = 0u;
		}

		public Mem_record(ICreateNewUser createInstance, Mem_newgame_plus newGamePlusBase, List<DifficultKind> kind)
		{
			if (createInstance == null)
			{
				return;
			}
			this.Level = newGamePlusBase.FleetLevel;
			this.Exp = newGamePlusBase.FleetExp;
			this.St_win = 0u;
			this.Pt_win = 0u;
			this.ClearDifficult = kind;
			this.Rebellion_win = 0u;
			this.Deck_practice_num = 0u;
		}

		public void SetUserRecordData(User_RecordFmt out_fmt)
		{
			if (out_fmt == null)
			{
				return;
			}
			out_fmt.Exp = this.Exp;
			Dictionary<int, int> dictionary = Mst_DataManager.Instance.Get_MstLevel(false);
			int num = 0;
			dictionary.TryGetValue(this.Level + 1, ref num);
			if (num == -1)
			{
				num = 0;
			}
			out_fmt.Exp_next = (uint)num;
			out_fmt.Level = this.Level;
			out_fmt.War_total = this.St_win + this.St_lose + this.Rebellion_win;
			out_fmt.War_win = this.St_win;
			out_fmt.War_lose = this.St_lose;
			out_fmt.War_RebellionWin = this.Rebellion_win;
			out_fmt.War_rate = this.GetSortieRate();
			out_fmt.Practice_win = this.Pt_win;
			out_fmt.Practice_lose = this.Pt_lose;
			out_fmt.DeckPracticeNum = this.Deck_practice_num;
			out_fmt.Rank = this.GetRank();
		}

		public double GetSortieRate()
		{
			double num = this.St_win + this.St_lose + this.Rebellion_win;
			if (num == 0.0)
			{
				return 0.0;
			}
			return this.St_win / num * 100.0;
		}

		public int UpdateExp(int addValue, Dictionary<int, int> mst_level)
		{
			int num = Enumerable.Max(mst_level.get_Values());
			this.Exp += (uint)addValue;
			if ((ulong)this.Exp > (ulong)((long)num))
			{
				this.Exp = (uint)num;
			}
			int num2 = mst_level.get_Item(this.Level + 1);
			if (num2 == -1 || (ulong)this.Exp < (ulong)((long)num2))
			{
				return this.Level;
			}
			Comm_UserDatas.Instance.User_trophy.IsFleetLevelUp = true;
			int num3 = this.Level + 1;
			while ((ulong)this.Exp >= (ulong)((long)num2))
			{
				if (num2 == -1)
				{
					return this.Level;
				}
				this.Level = num3;
				num3++;
				num2 = mst_level.get_Item(num3);
			}
			return this.Level;
		}

		public void UpdateSortieCount(BattleWinRankKinds kinds, bool rebellionBoss)
		{
			if (Utils.IsBattleWin(kinds))
			{
				if (rebellionBoss)
				{
					if (this.Rebellion_win != 4294967295u)
					{
						this.Rebellion_win += 1u;
					}
				}
				else if (this.St_win != 4294967295u)
				{
					this.St_win += 1u;
				}
			}
			else if (this.St_lose != 4294967295u)
			{
				this.St_lose += 1u;
			}
		}

		public void UpdatePracticeCount(BattleWinRankKinds kinds, bool practiceBattle)
		{
			if (practiceBattle)
			{
				if (Utils.IsBattleWin(kinds))
				{
					if (this.Pt_win != 4294967295u)
					{
						this.Pt_win += 1u;
					}
				}
				else if (this.Pt_lose != 4294967295u)
				{
					this.Pt_lose += 1u;
				}
			}
			else if (this.Deck_practice_num != 4294967295u)
			{
				this.Deck_practice_num += 1u;
			}
		}

		public void UpdateMissionCount(MissionResultKinds ms_kind)
		{
		}

		public void AddClearDifficult(DifficultKind difficult)
		{
			if (this.ClearDifficult.Contains(difficult))
			{
				return;
			}
			this.ClearDifficult.Add(difficult);
		}

		public int GetRank()
		{
			if (this.Level < 10)
			{
				return 10;
			}
			if (this.Level >= 10 && this.Level < 20)
			{
				return 9;
			}
			if (this.Level >= 20 && this.Level < 30)
			{
				return 8;
			}
			if (this.Level >= 30 && this.Level < 40)
			{
				return 7;
			}
			if (this.Level >= 40 && this.Level < 50)
			{
				return 6;
			}
			if (this.Level >= 50 && this.Level < 60)
			{
				return 5;
			}
			if (this.Level >= 60 && this.Level < 70)
			{
				return 4;
			}
			if (this.Level >= 70 && this.Level < 80)
			{
				return 3;
			}
			if (this.Level >= 80 && this.Level < 90)
			{
				return 2;
			}
			return 1;
		}

		public void AddLostShipCount()
		{
			if (this.LostShipNum == 4294967295u)
			{
				return;
			}
			this.LostShipNum += 1u;
		}

		protected override void setProperty(XElement element)
		{
			this.Level = int.Parse(element.Element("_level").get_Value());
			this.Exp = uint.Parse(element.Element("_exp").get_Value());
			this.St_win = uint.Parse(element.Element("_st_win").get_Value());
			this.St_lose = uint.Parse(element.Element("_st_lose").get_Value());
			this.Pt_win = uint.Parse(element.Element("_pt_win").get_Value());
			this.Pt_lose = uint.Parse(element.Element("_pt_lose").get_Value());
			this.Rate = double.Parse(element.Element("_rate").get_Value());
			using (IEnumerator<XElement> enumerator = element.Element("_clearDifficult").Elements().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					DifficultKind difficultKind = (DifficultKind)((int)Enum.Parse(typeof(DifficultKind), current.get_Value()));
					this.ClearDifficult.Add(difficultKind);
				}
			}
			if (element.Element("_rebellion_win") != null)
			{
				this.Rebellion_win = uint.Parse(element.Element("_rebellion_win").get_Value());
			}
			if (element.Element("_deck_practice_num") != null)
			{
				this.Deck_practice_num = uint.Parse(element.Element("_deck_practice_num").get_Value());
			}
			if (element.Element("_lostShipNum") != null)
			{
				this.LostShipNum = uint.Parse(element.Element("_lostShipNum").get_Value());
			}
		}
	}
}
