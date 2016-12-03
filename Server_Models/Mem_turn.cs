using Common.Struct;
using Server_Controllers;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_turn", Namespace = "")]
	public class Mem_turn : Model_Base
	{
		[DataMember]
		private int _total_turn;

		[DataMember]
		private int _erc;

		[DataMember]
		private bool _reqQuestReset;

		private static string _tableName = "mem_turn";

		private readonly DateTime baseTime;

		public int Total_turn
		{
			get
			{
				return this._total_turn;
			}
			private set
			{
				this._total_turn = value;
			}
		}

		public int Erc
		{
			get
			{
				return this._erc;
			}
			private set
			{
				this._erc = value;
			}
		}

		public bool ReqQuestReset
		{
			get
			{
				return this._reqQuestReset;
			}
			private set
			{
				this._reqQuestReset = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_turn._tableName;
			}
		}

		public Mem_turn()
		{
			this.baseTime = new DateTime(1941, 12, 8);
			this.Total_turn = 0;
			this.ReqQuestReset = true;
		}

		public int GetElapsedYear()
		{
			return this.GetElapsedYear(this.GetDateTime());
		}

		public int GetElapsedYear(DateTime dt)
		{
			return dt.get_Year() - this.baseTime.get_Year();
		}

		public TurnString GetTurnString()
		{
			DateTime systemDate = this.baseTime.AddDays((double)this.Total_turn);
			int elapsed_year = systemDate.get_Year() - this.baseTime.get_Year();
			TurnString result = new TurnString(elapsed_year, systemDate);
			return result;
		}

		public TurnString GetTurnString(int reqTurn)
		{
			DateTime systemDate = this.baseTime.AddDays((double)reqTurn);
			int elapsed_year = systemDate.get_Year() - this.baseTime.get_Year();
			TurnString result = new TurnString(elapsed_year, systemDate);
			return result;
		}

		public DateTime GetDateTime()
		{
			return this.baseTime.AddDays((double)this.Total_turn);
		}

		public DateTime GetDateTime(int plusYear, int month, int day)
		{
			int num = this.baseTime.get_Year() + plusYear;
			return new DateTime(num, month, day);
		}

		public void AddTurn(Api_TurnOperator instance)
		{
			if (instance == null)
			{
				return;
			}
			this.Total_turn++;
			this.ReqQuestReset = true;
		}

		public void DisableQuestReset()
		{
			this.ReqQuestReset = false;
		}

		protected override void setProperty(XElement element)
		{
			this.Total_turn = int.Parse(element.Element("_total_turn").get_Value());
			this.Erc = int.Parse(element.Element("_erc").get_Value());
			this.ReqQuestReset = bool.Parse(element.Element("_reqQuestReset").get_Value());
		}
	}
}
