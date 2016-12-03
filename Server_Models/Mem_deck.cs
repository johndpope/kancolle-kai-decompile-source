using Common.Enum;
using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_deck", Namespace = "")]
	public class Mem_deck : Model_Base
	{
		[DataContract(Namespace = "")]
		public enum SupportKinds
		{
			[EnumMember]
			NONE,
			[EnumMember]
			WAIT,
			[EnumMember]
			SUPPORTED
		}

		[DataMember]
		private int _rid;

		[DataMember]
		private int _area_id;

		[DataMember]
		private string _name;

		[DataMember]
		private int _mission_id;

		[DataMember]
		private MissionStates _missionState;

		[DataMember]
		private Mem_deck.SupportKinds _supportKind;

		[DataMember]
		private int _startTime;

		[DataMember]
		private int _completeTime;

		[DataMember]
		private DeckShips _ship;

		[DataMember]
		private bool _isActionEnd;

		private static string _tableName = "mem_deck";

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

		public int Mission_id
		{
			get
			{
				return this._mission_id;
			}
			private set
			{
				this._mission_id = value;
			}
		}

		public MissionStates MissionState
		{
			get
			{
				return this._missionState;
			}
			private set
			{
				this._missionState = value;
			}
		}

		public Mem_deck.SupportKinds SupportKind
		{
			get
			{
				return this._supportKind;
			}
			private set
			{
				this._supportKind = value;
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

		public bool IsActionEnd
		{
			get
			{
				return this._isActionEnd;
			}
			private set
			{
				this._isActionEnd = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_deck._tableName;
			}
		}

		public Mem_deck()
		{
			this.Area_id = 1;
			this.Mission_id = 0;
			this.MissionState = MissionStates.NONE;
			this.StartTime = 0;
			this.CompleteTime = 0;
			this.Ship = new DeckShips();
			this.IsActionEnd = false;
		}

		public Mem_deck(int rid) : this()
		{
			this.Rid = rid;
			this.Name = string.Format("第{0}艦隊", this.Rid);
		}

		public int[] Search_ShipIdx(Dictionary<int, Mem_deck> target_decks, int ship_id)
		{
			int[] array = new int[]
			{
				-1,
				-1
			};
			using (Dictionary<int, Mem_deck>.Enumerator enumerator = target_decks.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, Mem_deck> current = enumerator.get_Current();
					array[1] = current.get_Value().Ship.Find(ship_id);
					if (array[1] != -1)
					{
						array[0] = current.get_Key();
						return array;
					}
				}
			}
			return array;
		}

		public int[] Search_ShipIdx(int ship_id)
		{
			int[] array = new int[]
			{
				-1,
				-1
			};
			using (Dictionary<int, Mem_deck>.Enumerator enumerator = Comm_UserDatas.Instance.User_deck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, Mem_deck> current = enumerator.get_Current();
					array[1] = current.get_Value().Ship.Find(ship_id);
					if (array[1] != -1)
					{
						array[0] = current.get_Key();
						return array;
					}
				}
			}
			return array;
		}

		public bool Contains_Yomi(int mst_id)
		{
			Mst_ship mst_ship = null;
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(mst_id, ref mst_ship))
			{
				return true;
			}
			string yomi = mst_ship.Yomi;
			for (int i = 0; i < this.Ship.Count(); i++)
			{
				Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(this.Ship[i]);
				string yomi2 = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id).Yomi;
				if (yomi.Equals(yomi2))
				{
					return true;
				}
			}
			return false;
		}

		public void SetDeckName(string name)
		{
			name.TrimEnd(new char[1]);
			this.Name = name;
		}

		public bool IsMissionComplete()
		{
			return this.MissionState == MissionStates.END || (this.MissionState == MissionStates.STOP && this.CompleteTime >= Comm_UserDatas.Instance.User_turn.Total_turn);
		}

		public bool MissionStart(Mst_mission2 mst_mission)
		{
			if (mst_mission == null || this.MissionState != MissionStates.NONE)
			{
				return false;
			}
			this.Mission_id = mst_mission.Id;
			this.StartTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			this.CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn + mst_mission.Time;
			this.MissionState = MissionStates.RUNNING;
			this.SupportKind = ((!mst_mission.IsSupportMission()) ? Mem_deck.SupportKinds.NONE : Mem_deck.SupportKinds.WAIT);
			return true;
		}

		public bool MissionEnd()
		{
			if (this.MissionState != MissionStates.RUNNING && this.MissionState != MissionStates.STOP)
			{
				return false;
			}
			if (this.CompleteTime > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			if (this.SupportKind != Mem_deck.SupportKinds.NONE && this.MissionState != MissionStates.STOP)
			{
				this.UpdateSupportShip();
				this.MissionState = MissionStates.END;
				this.MissionInit();
			}
			else if (this.SupportKind == Mem_deck.SupportKinds.NONE)
			{
				if (this.MissionState != MissionStates.STOP)
				{
					this.MissionState = MissionStates.END;
				}
				this.StartTime = 0;
				this.CompleteTime = 0;
			}
			return true;
		}

		public bool MissionStop(int newEndTime)
		{
			if (this.MissionState != MissionStates.RUNNING)
			{
				return false;
			}
			if (this.CompleteTime < newEndTime)
			{
				return false;
			}
			this.MissionState = MissionStates.STOP;
			this.CompleteTime = newEndTime;
			return true;
		}

		public bool MissionEnforceEnd()
		{
			if (this.MissionState == MissionStates.NONE)
			{
				return false;
			}
			if (this.SupportKind != Mem_deck.SupportKinds.NONE)
			{
				this.CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn;
				return this.MissionEnd();
			}
			this.CompleteTime = Comm_UserDatas.Instance.User_turn.Total_turn;
			this.MissionState = MissionStates.END;
			return this.MissionInit();
		}

		public bool MissionInit()
		{
			if (this.MissionState != MissionStates.END && this.MissionState != MissionStates.STOP)
			{
				return false;
			}
			this.Mission_id = 0;
			this.MissionState = MissionStates.NONE;
			this.SupportKind = Mem_deck.SupportKinds.NONE;
			return true;
		}

		public void ChangeSupported()
		{
			if (this.SupportKind == Mem_deck.SupportKinds.WAIT)
			{
				this.SupportKind = Mem_deck.SupportKinds.SUPPORTED;
			}
		}

		private void UpdateSupportShip()
		{
			if (this.SupportKind != Mem_deck.SupportKinds.SUPPORTED)
			{
				return;
			}
			Mst_mission2 mst_mission = Mst_DataManager.Instance.Mst_mission.get_Item(this.Mission_id);
			int num = (mst_mission.Mission_type != MissionType.SupportForward) ? 10 : 5;
			Random random = new Random();
			int subCond = random.Next(num) + 1;
			double subFuel = mst_mission.Use_fuel;
			double subBull = mst_mission.Use_bull;
			int num2 = this.Ship.Count();
			int num3 = 0;
			List<Mem_ship> list = new List<Mem_ship>();
			for (int i = 0; i < num2; i++)
			{
				if (this.Ship[i] > 0)
				{
					Mem_ship mem_ship = Comm_UserDatas.Instance.User_ship.get_Item(this.Ship[i]);
					list.Add(mem_ship);
					if (Mst_DataManager.Instance.Mst_stype.get_Item(mem_ship.Stype).IsMother())
					{
						num3++;
					}
				}
			}
			if (num3 >= 3)
			{
				subBull *= 0.5;
			}
			list.ForEach(delegate(Mem_ship x)
			{
				x.SetSubBull_ToMission(subBull);
				x.SetSubFuel_ToMission(subFuel);
				Mem_shipBase mem_shipBase = new Mem_shipBase(x);
				mem_shipBase.Cond -= subCond;
				if (mem_shipBase.Cond < 0)
				{
					mem_shipBase.Cond = 0;
				}
				x.Set_ShipParam(mem_shipBase, Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id), false);
			});
		}

		public int GetRequireMissionTime()
		{
			if (this.CompleteTime < Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return 0;
			}
			return this.CompleteTime - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public void MoveArea(int area_id)
		{
			this.Area_id = area_id;
		}

		public void ActionStart()
		{
			if (this.IsActionEnd)
			{
				this.IsActionEnd = false;
			}
		}

		public void ActionEnd()
		{
			if (!this.IsActionEnd)
			{
				this.IsActionEnd = true;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Area_id = int.Parse(element.Element("_area_id").get_Value());
			this.Name = element.Element("_name").get_Value();
			this.Mission_id = int.Parse(element.Element("_mission_id").get_Value());
			this.MissionState = (MissionStates)((int)Enum.Parse(typeof(MissionStates), element.Element("_missionState").get_Value()));
			this.SupportKind = (Mem_deck.SupportKinds)((int)Enum.Parse(typeof(Mem_deck.SupportKinds), element.Element("_supportKind").get_Value()));
			this.StartTime = int.Parse(element.Element("_startTime").get_Value());
			this.CompleteTime = int.Parse(element.Element("_completeTime").get_Value());
			this.IsActionEnd = bool.Parse(element.Element("_isActionEnd").get_Value());
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
