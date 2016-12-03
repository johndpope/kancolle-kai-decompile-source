using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_ndock", Namespace = "")]
	public class Mem_ndock : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _area_id;

		[DataMember]
		private int _dock_no;

		[DataMember]
		private NdockStates _state;

		[DataMember]
		private int _ship_id;

		[DataMember]
		private int _startTime;

		[DataMember]
		private int _completeTime;

		[DataMember]
		private int _item1;

		[DataMember]
		private int _item2;

		[DataMember]
		private int _item3;

		[DataMember]
		private int _item4;

		private static string _tableName = "mem_ndock";

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

		public int Area_id
		{
			get
			{
				return this._area_id;
			}
			private set
			{
				this._area_id = value;
			}
		}

		public int Dock_no
		{
			get
			{
				return this._dock_no;
			}
			private set
			{
				this._dock_no = value;
			}
		}

		public NdockStates State
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

		public int Ship_id
		{
			get
			{
				return this._ship_id;
			}
			private set
			{
				this._ship_id = value;
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

		public int Item1
		{
			get
			{
				return this._item1;
			}
			private set
			{
				this._item1 = value;
			}
		}

		public int Item2
		{
			get
			{
				return this._item2;
			}
			private set
			{
				this._item2 = value;
			}
		}

		public int Item3
		{
			get
			{
				return this._item3;
			}
			private set
			{
				this._item3 = value;
			}
		}

		public int Item4
		{
			get
			{
				return this._item4;
			}
			private set
			{
				this._item4 = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_ndock._tableName;
			}
		}

		public Mem_ndock()
		{
		}

		public Mem_ndock(int rid, int area_id, int no)
		{
			this.Rid = rid;
			this.Area_id = area_id;
			this.Dock_no = no;
			this.State = NdockStates.EMPTY;
			this.StartTime = 0;
			this.CompleteTime = 0;
			this.Item1 = 0;
			this.Item2 = 0;
			this.Item3 = 0;
			this.Item4 = 0;
		}

		public void RecoverStart(int ship_rid, Dictionary<enumMaterialCategory, int> material, int span)
		{
			this.Ship_id = ship_rid;
			this.Item1 = material.get_Item(enumMaterialCategory.Fuel);
			this.Item3 = material.get_Item(enumMaterialCategory.Steel);
			this.State = NdockStates.RESTORE;
			this.StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			this.CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + span;
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Sub_Material(this.Item1);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel).Sub_Material(this.Item3);
		}

		public bool IsRecoverEndTime()
		{
			return this.CompleteTime <= Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public bool RecoverEnd(bool timeChk)
		{
			if (timeChk && !this.IsRecoverEndTime())
			{
				return false;
			}
			if (this.State != NdockStates.RESTORE)
			{
				return false;
			}
			Mem_ship ship = Comm_UserDatas.Instance.User_ship.get_Item(this.Ship_id);
			ship.NdockRecovery(this);
			this.Ship_id = 0;
			this.Item1 = 0;
			this.Item3 = 0;
			this.State = NdockStates.EMPTY;
			this.StartTime = 0;
			this.CompleteTime = 0;
			if (!Enumerable.Any<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.Ship.Find(ship.Rid) != -1))
			{
				if (timeChk)
				{
					ship.BlingSet(this.Area_id);
				}
				else
				{
					ship.BlingWait(this.Area_id, Mem_ship.BlingKind.WaitDeck);
				}
			}
			return true;
		}

		public int GetRequireTime()
		{
			int num = this.CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
			return (num >= 0) ? num : 0;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Area_id = int.Parse(element.Element("_area_id").get_Value());
			this.Dock_no = int.Parse(element.Element("_dock_no").get_Value());
			this.State = (NdockStates)((int)Enum.Parse(typeof(NdockStates), element.Element("_state").get_Value()));
			this.Ship_id = int.Parse(element.Element("_ship_id").get_Value());
			this.StartTime = int.Parse(element.Element("_startTime").get_Value());
			this.CompleteTime = int.Parse(element.Element("_completeTime").get_Value());
			this.Item1 = int.Parse(element.Element("_item1").get_Value());
			this.Item2 = int.Parse(element.Element("_item2").get_Value());
			this.Item3 = int.Parse(element.Element("_item3").get_Value());
			this.Item4 = int.Parse(element.Element("_item4").get_Value());
		}
	}
}
