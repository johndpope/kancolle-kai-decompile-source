using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_esccort_deck", Namespace = "")]
	public class Mem_esccort_deck : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private string _name;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private DispositionStatus _disposition_status;

		[DataMember]
		private int _startTime;

		[DataMember]
		private int _completeTime;

		[DataMember]
		private DeckShips _ship;

		private static string _tableName = "mem_esccort_deck";

		public int Rid
		{
			get
			{
				return this._rid;
			}
			private set
			{
				this._rid = value;
			}
		}

		public string Name
		{
			get
			{
				return this._name;
			}
			private set
			{
				this._name = value;
			}
		}

		public int Maparea_id
		{
			get
			{
				return this._maparea_id;
			}
			private set
			{
				this._maparea_id = value;
			}
		}

		public DispositionStatus Disposition_status
		{
			get
			{
				return this._disposition_status;
			}
			private set
			{
				this._disposition_status = value;
			}
		}

		public int StartTime
		{
			get
			{
				return this._startTime;
			}
			private set
			{
				this._startTime = value;
			}
		}

		public int CompleteTime
		{
			get
			{
				return this._completeTime;
			}
			private set
			{
				this._completeTime = value;
			}
		}

		public DeckShips Ship
		{
			get
			{
				return this._ship;
			}
			private set
			{
				this._ship = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_esccort_deck._tableName;
			}
		}

		public Mem_esccort_deck()
		{
			this.Maparea_id = 1;
			this.Ship = new DeckShips();
		}

		public Mem_esccort_deck(int rid, int area_id) : this()
		{
			this.Rid = rid;
			this.Disposition_status = DispositionStatus.NONE;
			this.Maparea_id = area_id;
			this.Name = string.Empty;
		}

		public void SetDeckName(string name)
		{
			name.TrimEnd(new char[1]);
			this.Name = name;
		}

		public void GoArea(int area_id)
		{
			this.Disposition_status = DispositionStatus.ARRIVAL;
			this.StartTime = 0;
			this.CompleteTime = 0;
		}

		public void EscortStop()
		{
			if (this.Disposition_status == DispositionStatus.NONE)
			{
				return;
			}
			this.Disposition_status = DispositionStatus.NONE;
			this.StartTime = 0;
			this.CompleteTime = 0;
		}

		public bool IsBlingDeck()
		{
			return this.CompleteTime - this.StartTime > 0;
		}

		public int GetBlingTurn()
		{
			if (!this.IsBlingDeck())
			{
				return 0;
			}
			return this.CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public bool BlingTerm()
		{
			if (!this.IsBlingDeck())
			{
				return false;
			}
			if (this.CompleteTime > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			this.StartTime = 0;
			this.CompleteTime = 0;
			return true;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Name = element.Element("_name").get_Value();
			this.Maparea_id = int.Parse(element.Element("_maparea_id").get_Value());
			this.Disposition_status = (DispositionStatus)((int)Enum.Parse(typeof(DispositionStatus), element.Element("_disposition_status").get_Value()));
			this.StartTime = int.Parse(element.Element("_startTime").get_Value());
			this.CompleteTime = int.Parse(element.Element("_completeTime").get_Value());
			IEnumerable<XElement> enumerable = Extensions.Elements<XElement>(element.Elements("_ship"), "ships");
			using (var enumerator = Enumerable.Select(Extensions.Elements<XElement>(enumerable), (XElement obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					this.Ship[current.idx] = int.Parse(current.obj.get_Value());
				}
			}
		}
	}
}
