using Common.Enum;
using Server_Common;
using Server_Controllers;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_rebellion_point", Namespace = "")]
	public class Mem_rebellion_point : Model_Base
	{
		private const int POINT_MAPOPEN = 40;

		private const int POINT_MAX = 200;

		private const int POINT_MIN = 0;

		[DataMember]
		private int _rid;

		[DataMember]
		private int _point;

		[DataMember]
		private RebellionState _state;

		private static string _tableName = "mem_rebellion_point";

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

		public int Point
		{
			get
			{
				return this._point;
			}
			private set
			{
				this._point = value;
			}
		}

		public RebellionState State
		{
			get
			{
				return this._state;
			}
			private set
			{
				this._state = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_rebellion_point._tableName;
			}
		}

		public Mem_rebellion_point()
		{
		}

		public Mem_rebellion_point(int maparea_id)
		{
			this.Rid = maparea_id;
			this.Point = 40;
			this.ChangeState();
		}

		public void AddPoint(IRebellionPointOperator instance, int addNum)
		{
			if (instance == null || addNum < 0)
			{
				return;
			}
			if (this.State == RebellionState.Invation)
			{
				return;
			}
			this.Point += addNum;
			if (this.Point >= 200)
			{
				this.Point = 200;
			}
			this.ChangeState();
		}

		public void SubPoint(IRebellionPointOperator instance, int subNum)
		{
			if (instance == null || subNum < 0)
			{
				return;
			}
			if (this.State == RebellionState.Invation)
			{
				return;
			}
			this.Point -= subNum;
			if (this.Point < 0)
			{
				this.Point = 0;
			}
			this.ChangeState();
		}

		public bool StartRebellion(Api_TurnOperator instance)
		{
			if (instance == null)
			{
				return false;
			}
			if (this.State != RebellionState.Alert)
			{
				return false;
			}
			double randDouble = Utils.GetRandDouble(0.0, 100.0, 1.0, 1);
			if (randDouble <= 65.0)
			{
				this.State = RebellionState.Invation;
				return true;
			}
			return false;
		}

		public int EndInvation(IRebellionPointOperator instance)
		{
			if (instance == null)
			{
				return -1;
			}
			if (this.State != RebellionState.Invation)
			{
				return -2;
			}
			this.Point = 0;
			this.State = RebellionState.Safety;
			return 0;
		}

		private void ChangeState()
		{
			if (this.State == RebellionState.Invation)
			{
				return;
			}
			if (this.Point >= 100)
			{
				this.State = RebellionState.Alert;
			}
			else if (this.Point > 90)
			{
				this.State = RebellionState.Warning;
			}
			else if (this.Point > 70)
			{
				this.State = RebellionState.Caution;
			}
			else if (this.Point > 50)
			{
				this.State = RebellionState.Attention;
			}
			else
			{
				this.State = RebellionState.Safety;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Point = int.Parse(element.Element("_point").get_Value());
			this.State = (RebellionState)((int)Enum.Parse(typeof(RebellionState), element.Element("_state").get_Value()));
		}
	}
}
