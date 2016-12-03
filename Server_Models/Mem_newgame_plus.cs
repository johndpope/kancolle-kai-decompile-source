using Common.Enum;
using Server_Common;
using Server_Controllers.BattleLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_newgame_plus", Namespace = "")]
	public class Mem_newgame_plus : Model_Base
	{
		[DataMember]
		private List<Mem_shipBase> _ship;

		[DataMember]
		private List<Mem_slotitem> _slotitem;

		[DataMember]
		private List<Mem_furniture> _furniture;

		[DataMember]
		private List<Mem_book> _ship_book;

		[DataMember]
		private List<Mem_book> _slot_book;

		[DataMember]
		private int _fleetLevel;

		[DataMember]
		private uint _fleetExp;

		[DataMember]
		private List<int> _clear;

		private int tempRewardShipRid;

		private static string _tableName = "mem_newgame_plus";

		public List<Mem_shipBase> Ship
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

		public List<Mem_slotitem> Slotitem
		{
			get
			{
				return this._slotitem;
			}
			private set
			{
				this._slotitem = value;
			}
		}

		public List<Mem_furniture> Furniture
		{
			get
			{
				return this._furniture;
			}
			private set
			{
				this._furniture = value;
			}
		}

		public List<Mem_book> Ship_book
		{
			get
			{
				return this._ship_book;
			}
			private set
			{
				this._ship_book = value;
			}
		}

		public List<Mem_book> Slot_book
		{
			get
			{
				return this._slot_book;
			}
			private set
			{
				this._slot_book = value;
			}
		}

		public int FleetLevel
		{
			get
			{
				return this._fleetLevel;
			}
			private set
			{
				this._fleetLevel = value;
			}
		}

		public uint FleetExp
		{
			get
			{
				return this._fleetExp;
			}
			private set
			{
				this._fleetExp = value;
			}
		}

		private int this[DifficultKind kind]
		{
			get
			{
				return this._clear.get_Item(kind - DifficultKind.TEI);
			}
			set
			{
				this._clear.set_Item(kind - DifficultKind.TEI, value);
			}
		}

		public int TempRewardShipRid
		{
			get
			{
				return this.tempRewardShipRid;
			}
			private set
			{
				this.tempRewardShipRid = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_newgame_plus._tableName;
			}
		}

		public Mem_newgame_plus()
		{
			this.TempRewardShipRid = 0;
			this.PurgeData();
		}

		public int ClearNum(DifficultKind kind)
		{
			int num = kind - DifficultKind.TEI;
			return this[kind];
		}

		public void SetRewardShipRid(Exec_BattleResult instance, int shipRid)
		{
			if (instance == null)
			{
				return;
			}
			if (this.TempRewardShipRid != 0)
			{
				return;
			}
			this.TempRewardShipRid = shipRid;
		}

		public void SetData(List<Mem_shipBase> shipBase, List<Mem_slotitem> slotItems)
		{
			this.FleetLevel = Comm_UserDatas.Instance.User_record.Level;
			this.FleetExp = Comm_UserDatas.Instance.User_record.Exp;
			shipBase.Sort((Mem_shipBase x, Mem_shipBase y) => x.GetNo.CompareTo(y.GetNo));
			slotItems.Sort((Mem_slotitem x, Mem_slotitem y) => x.GetNo.CompareTo(y.GetNo));
			for (int i = 0; i < shipBase.get_Count(); i++)
			{
				shipBase.get_Item(i).GetNo = i + 1;
			}
			for (int j = 0; j < slotItems.get_Count(); j++)
			{
				slotItems.get_Item(j).ChangeSortNo(j + 1);
			}
			this.Ship.Clear();
			this.Ship = shipBase;
			this.Slotitem.Clear();
			this.Slotitem = slotItems;
			this.Furniture.Clear();
			this.Furniture.AddRange(Comm_UserDatas.Instance.User_furniture.get_Values());
			this.Ship_book.Clear();
			this.Ship_book.AddRange(Comm_UserDatas.Instance.Ship_book.get_Values());
			this.Slot_book.Clear();
			this.Slot_book.AddRange(Comm_UserDatas.Instance.Slot_book.get_Values());
			DifficultKind difficult = Comm_UserDatas.Instance.User_basic.Difficult;
			DifficultKind kind;
			DifficultKind expr_163 = kind = difficult;
			int num = this[kind];
			this[expr_163] = num + 1;
			if (this[difficult] > 999)
			{
				this[difficult] = 999;
			}
		}

		public int GetLapNum()
		{
			return Enumerable.Sum(this._clear);
		}

		public void PurgeData()
		{
			this.Ship = new List<Mem_shipBase>();
			this.Slotitem = new List<Mem_slotitem>();
			this.Furniture = new List<Mem_furniture>();
			this.Ship_book = new List<Mem_book>();
			this.Slot_book = new List<Mem_book>();
			List<int> list = new List<int>();
			list.Add(0);
			list.Add(0);
			list.Add(0);
			list.Add(0);
			list.Add(0);
			this._clear = list;
			this.FleetLevel = 0;
			this.FleetExp = 0u;
		}

		protected override void setProperty(XElement element)
		{
			using (IEnumerator<XElement> enumerator = element.Element("_ship").Elements().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					XElement current = enumerator.get_Current();
					Mem_shipBase mem_shipBase = new Mem_shipBase();
					mem_shipBase.setProperty(current);
					this.Ship.Add(mem_shipBase);
				}
			}
			using (IEnumerator<XElement> enumerator2 = element.Element("_slotitem").Elements().GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					XElement current2 = enumerator2.get_Current();
					this.Slotitem.Add(Model_Base.SetUserData<Mem_slotitem>(current2));
				}
			}
			using (IEnumerator<XElement> enumerator3 = element.Element("_furniture").Elements().GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					XElement current3 = enumerator3.get_Current();
					this.Furniture.Add(Model_Base.SetUserData<Mem_furniture>(current3));
				}
			}
			using (IEnumerator<XElement> enumerator4 = element.Element("_ship_book").Elements().GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					XElement current4 = enumerator4.get_Current();
					this.Ship_book.Add(Model_Base.SetUserData<Mem_book>(current4));
				}
			}
			using (IEnumerator<XElement> enumerator5 = element.Element("_slot_book").Elements().GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					XElement current5 = enumerator5.get_Current();
					this.Slot_book.Add(Model_Base.SetUserData<Mem_book>(current5));
				}
			}
			using (var enumerator6 = Enumerable.Select(element.Element("_clear").Elements(), (XElement obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator6.MoveNext())
				{
					var current6 = enumerator6.get_Current();
					this._clear.set_Item(current6.idx, int.Parse(current6.obj.get_Value()));
				}
			}
			this.FleetLevel = int.Parse(element.Element("_fleetLevel").get_Value());
			this.FleetExp = uint.Parse(element.Element("_fleetExp").get_Value());
		}
	}
}
