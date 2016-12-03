using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_kdock", Namespace = "")]
	public class Mem_kdock : Model_Base
	{
		private const int LARGE_FUEL_VALUE = 1000;

		[DataMember]
		private int _rid;

		[DataMember]
		private KdockStates _state;

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

		[DataMember]
		private int _item5;

		[DataMember]
		private int _strategy_point;

		[DataMember]
		private int _tunker_num;

		private static string _tableName = "mem_kdock";

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

		public KdockStates State
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

		public int Item5
		{
			get
			{
				return this._item5;
			}
			private set
			{
				this._item5 = value;
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

		public int Tunker_num
		{
			get
			{
				return this._tunker_num;
			}
			private set
			{
				this._tunker_num = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_kdock._tableName;
			}
		}

		public Mem_kdock()
		{
		}

		public Mem_kdock(int rid)
		{
			this.Rid = rid;
			this.State = KdockStates.EMPTY;
			this.StartTime = 0;
			this.CompleteTime = 0;
			this.Ship_id = 0;
			this.Item1 = 0;
			this.Item2 = 0;
			this.Item3 = 0;
			this.Item4 = 0;
			this.Item5 = 0;
			this.Strategy_point = 0;
			this.Tunker_num = 0;
		}

		public void CreateStart(int ship_id, Dictionary<enumMaterialCategory, int> material, TimeSpan span)
		{
			this.Ship_id = ship_id;
			this.Item1 = material.get_Item(enumMaterialCategory.Fuel);
			this.Item2 = material.get_Item(enumMaterialCategory.Bull);
			this.Item3 = material.get_Item(enumMaterialCategory.Steel);
			this.Item4 = material.get_Item(enumMaterialCategory.Bauxite);
			this.Item5 = material.get_Item(enumMaterialCategory.Dev_Kit);
			this.State = KdockStates.CREATE;
			this.StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			this.CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + (int)span.get_TotalMinutes();
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Sub_Material(this.Item1);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Sub_Material(this.Item2);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel).Sub_Material(this.Item3);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bauxite).Sub_Material(this.Item4);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Dev_Kit).Sub_Material(this.Item5);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Build_Kit).Sub_Material(material.get_Item(enumMaterialCategory.Build_Kit));
		}

		public void CreateTunker(int createNum, Dictionary<enumMaterialCategory, int> material, int usePoint, int createTurn)
		{
			this.Item1 = material.get_Item(enumMaterialCategory.Fuel);
			this.Item2 = material.get_Item(enumMaterialCategory.Bull);
			this.Item3 = material.get_Item(enumMaterialCategory.Steel);
			this.Item4 = material.get_Item(enumMaterialCategory.Bauxite);
			this.Strategy_point = usePoint;
			this.Tunker_num = createNum;
			this.State = KdockStates.CREATE;
			this.StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			this.CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + createTurn;
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Sub_Material(this.Item1);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Sub_Material(this.Item2);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel).Sub_Material(this.Item3);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bauxite).Sub_Material(this.Item4);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Build_Kit).Sub_Material(material.get_Item(enumMaterialCategory.Build_Kit));
			Comm_UserDatas.Instance.User_basic.SubPoint(usePoint);
		}

		public bool CreateEnd(bool timeChk)
		{
			if (timeChk && this.CompleteTime > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			if (this.State != KdockStates.CREATE)
			{
				return false;
			}
			this.State = KdockStates.COMPLETE;
			this.StartTime = 0;
			this.CompleteTime = 0;
			return true;
		}

		public bool GetShip()
		{
			Comm_UserDatas arg_18_0 = Comm_UserDatas.Instance;
			List<int> list = new List<int>();
			list.Add(this.Ship_id);
			arg_18_0.Add_Ship(list);
			this.Ship_id = 0;
			this.Item1 = 0;
			this.Item2 = 0;
			this.Item3 = 0;
			this.Item4 = 0;
			this.Item5 = 0;
			this.State = KdockStates.EMPTY;
			return true;
		}

		public bool GetTunker()
		{
			Comm_UserDatas.Instance.Add_Tanker(this.Tunker_num);
			this.Item1 = 0;
			this.Item2 = 0;
			this.Item3 = 0;
			this.Item4 = 0;
			this.State = KdockStates.EMPTY;
			this.Strategy_point = 0;
			this.Tunker_num = 0;
			return true;
		}

		public bool IsLargeDock()
		{
			return this.Item1 >= 1000;
		}

		public bool IsTunkerDock()
		{
			return this.Strategy_point > 0;
		}

		public int GetRequireCreateTime()
		{
			return this.CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.State = (KdockStates)((int)Enum.Parse(typeof(KdockStates), element.Element("_state").get_Value()));
			this.Ship_id = int.Parse(element.Element("_ship_id").get_Value());
			this.StartTime = int.Parse(element.Element("_startTime").get_Value());
			this.CompleteTime = int.Parse(element.Element("_completeTime").get_Value());
			this.Item1 = int.Parse(element.Element("_item1").get_Value());
			this.Item2 = int.Parse(element.Element("_item2").get_Value());
			this.Item3 = int.Parse(element.Element("_item3").get_Value());
			this.Item4 = int.Parse(element.Element("_item4").get_Value());
			this.Item5 = int.Parse(element.Element("_item5").get_Value());
			this.Strategy_point = int.Parse(element.Element("_strategy_point").get_Value());
			this.Tunker_num = int.Parse(element.Element("_tunker_num").get_Value());
		}
	}
}
