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
	[DataContract(Name = "mem_basic", Namespace = "")]
	public class Mem_basic : Model_Base
	{
		private const int DEF_SLOTNUM = 400;

		private const int DEF_SHIPNUM = 100;

		[DataMember]
		private DifficultKind _difficult;

		[DataMember]
		private string _nickname;

		[DataMember]
		private int _starttime;

		[DataMember]
		private string _comment;

		[DataMember]
		private int _max_chara;

		[DataMember]
		private int _max_slotitem;

		[DataMember]
		private int _max_quest;

		[DataMember]
		private int _large_dock;

		[DataMember]
		private int _fcoin;

		[DataMember]
		private int _strategy_point;

		[DataMember]
		private HashSet<int> tutorialProgressStep;

		[DataMember]
		private HashSet<int> tutorialProgressText;

		private static string _tableName = "mem_basic";

		public DifficultKind Difficult
		{
			get
			{
				return this._difficult;
			}
			private set
			{
				this._difficult = value;
			}
		}

		public string Nickname
		{
			get
			{
				return this._nickname;
			}
			private set
			{
				this._nickname = value;
			}
		}

		public int Starttime
		{
			get
			{
				return this._starttime;
			}
			private set
			{
				this._starttime = value;
			}
		}

		public string Comment
		{
			get
			{
				return this._comment;
			}
			private set
			{
				this._comment = value;
			}
		}

		public int Max_chara
		{
			get
			{
				return this._max_chara;
			}
			private set
			{
				this._max_chara = value;
			}
		}

		public int Max_slotitem
		{
			get
			{
				return this._max_slotitem;
			}
			private set
			{
				this._max_slotitem = value;
			}
		}

		public int Max_quest
		{
			get
			{
				return this._max_quest;
			}
			private set
			{
				this._max_quest = value;
			}
		}

		public int Large_dock
		{
			get
			{
				return this._large_dock;
			}
			private set
			{
				this._large_dock = value;
			}
		}

		public int Fcoin
		{
			get
			{
				return this._fcoin;
			}
			private set
			{
				this._fcoin = value;
			}
		}

		public int Strategy_point
		{
			get
			{
				return this._strategy_point;
			}
			private set
			{
				this._strategy_point = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_basic._tableName;
			}
		}

		public Mem_basic()
		{
			this.Difficult = DifficultKind.HEI;
			this.Nickname = string.Empty;
			this.Max_chara = 100;
			this.Max_slotitem = 400;
			this.Max_quest = 7;
			this.Starttime = 0;
			this.Comment = string.Empty;
			this.Large_dock = 0;
			this.Fcoin = 0;
			this.Strategy_point = 3000;
			this.tutorialProgressStep = new HashSet<int>();
			this.tutorialProgressText = new HashSet<int>();
		}

		public Mem_basic(ICreateNewUser createInstance, Mem_basic newGamePlusBase)
		{
			if (createInstance == null)
			{
				return;
			}
			this.Difficult = newGamePlusBase.Difficult;
			this.Nickname = newGamePlusBase.Nickname;
			this.Max_chara = newGamePlusBase.Max_chara;
			this.Max_slotitem = newGamePlusBase.Max_slotitem;
			this.Max_quest = 7;
			this.Starttime = 1;
			this.Comment = string.Empty;
			this.Large_dock = 0;
			this.Fcoin = 0;
			this.Strategy_point = 3000;
			this.tutorialProgressStep = new HashSet<int>();
			this.tutorialProgressText = new HashSet<int>();
		}

		public int UserLevel()
		{
			return Comm_UserDatas.Instance.User_record.Level;
		}

		public int UserRank()
		{
			return Comm_UserDatas.Instance.User_record.GetRank();
		}

		public void SetUserRecordData(User_RecordFmt out_fmt)
		{
			if (out_fmt == null)
			{
				return;
			}
			out_fmt.Comment = this.Comment;
			out_fmt.Large_dock = this.Large_dock;
			out_fmt.Nickname = this.Nickname;
			out_fmt.Ship_max = this.Max_chara;
			out_fmt.Slot_max = this.Max_slotitem;
		}

		public void UpdateComment(string comment)
		{
			comment.TrimEnd(new char[1]);
			this.Comment = comment;
		}

		public void UpdateNickName(string name)
		{
			name.TrimEnd(new char[1]);
			this.Nickname = name;
		}

		public void SetDifficult(DifficultKind difficult)
		{
			this.Difficult = difficult;
			this.Starttime = 1;
		}

		public bool IsMaxChara()
		{
			return Enumerable.Count<KeyValuePair<int, Mem_ship>>(Comm_UserDatas.Instance.User_ship) >= this.Max_chara;
		}

		public bool IsMaxSlotitem()
		{
			return Enumerable.Count<KeyValuePair<int, Mem_slotitem>>(Comm_UserDatas.Instance.User_slot) >= this.Max_slotitem;
		}

		public void AddCoin(int addNum)
		{
			this.Fcoin += addNum;
		}

		public void SubCoin(int subNum)
		{
			this.Fcoin -= subNum;
		}

		public void AddPoint(int addNum)
		{
			int int_value = Mst_DataManager.Instance.Mst_const.get_Item(MstConstDataIndex.Strategy_point_def).Int_value;
			this.Strategy_point += addNum;
			if (this.Strategy_point > int_value)
			{
				this.Strategy_point = int_value;
			}
		}

		public void SubPoint(int subNum)
		{
			this.Strategy_point -= subNum;
			if (this.Strategy_point < 0)
			{
				this.Strategy_point = 0;
			}
		}

		public void OpenLargeDock()
		{
			this.Large_dock = 1;
		}

		public void PortExtend(int type)
		{
			int num = 10;
			int num2 = 40;
			if (type != 1)
			{
				return;
			}
			if (this.GetPortMaxExtendNum() < this.Max_chara)
			{
				return;
			}
			this.Max_chara += num;
			this.Max_slotitem += num2;
		}

		public bool QuestExtend(Dictionary<MstConstDataIndex, Mst_const> mst_const)
		{
			if (this.Max_quest >= mst_const.get_Item(MstConstDataIndex.Parallel_quest_max).Int_value)
			{
				return false;
			}
			this.Max_quest++;
			return true;
		}

		public int GetPortMaxExtendNum()
		{
			if (Comm_UserDatas.Instance.User_plus.GetLapNum() >= 1)
			{
				return Mst_DataManager.Instance.Mst_const.get_Item(MstConstDataIndex.Boko_max_ships_sys).Int_value;
			}
			return Mst_DataManager.Instance.Mst_const.get_Item(MstConstDataIndex.Boko_max_ships_def).Int_value;
		}

		public int GetPortMaxSlotItemNum()
		{
			int num = (this.GetPortMaxExtendNum() - 100) / 10;
			return 400 + 40 * num;
		}

		public int GetMaterialMaxNum()
		{
			int num = 10000;
			int num2 = 1000;
			int num3 = this.UserLevel();
			return num3 * num2 + num;
		}

		public void AddTutorialProgress(int tutorialType, int addKind)
		{
			HashSet<int> hashSet = (tutorialType != 1) ? this.tutorialProgressText : this.tutorialProgressStep;
			hashSet.Add(addKind);
		}

		public bool GetTutorialState(int tutorialType, int getKind)
		{
			HashSet<int> hashSet = (tutorialType != 1) ? this.tutorialProgressText : this.tutorialProgressStep;
			return hashSet.Contains(getKind);
		}

		public int GetTutorialStepLastNo()
		{
			if (this.tutorialProgressStep.get_Count() == 0)
			{
				return 0;
			}
			return Enumerable.Max(Enumerable.Select<int, int>(this.tutorialProgressStep, (int x) => x));
		}

		public List<int> GetTutorialProgress(int tutorialType)
		{
			HashSet<int> hashSet = (tutorialType != 1) ? this.tutorialProgressText : this.tutorialProgressStep;
			return Enumerable.ToList<int>(hashSet);
		}

		protected override void setProperty(XElement element)
		{
			this.Difficult = (DifficultKind)((int)Enum.Parse(typeof(DifficultKind), element.Element("_difficult").get_Value()));
			this.Nickname = element.Element("_nickname").get_Value();
			this.Starttime = int.Parse(element.Element("_starttime").get_Value());
			this.Comment = element.Element("_comment").get_Value();
			this.Max_chara = int.Parse(element.Element("_max_chara").get_Value());
			this.Max_slotitem = int.Parse(element.Element("_max_slotitem").get_Value());
			this.Max_quest = int.Parse(element.Element("_max_quest").get_Value());
			this.Large_dock = int.Parse(element.Element("_large_dock").get_Value());
			this.Fcoin = int.Parse(element.Element("_fcoin").get_Value());
			this.Strategy_point = int.Parse(element.Element("_strategy_point").get_Value());
			using (IEnumerator<XElement> enumerator = element.Element("tutorialProgressStep").Elements().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					this.tutorialProgressStep.Add(int.Parse(current.get_Value()));
				}
			}
			using (IEnumerator<XElement> enumerator2 = element.Element("tutorialProgressText").Elements().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					XElement current2 = enumerator2.get_Current();
					this.tutorialProgressText.Add(int.Parse(current2.get_Value()));
				}
			}
		}
	}
}
