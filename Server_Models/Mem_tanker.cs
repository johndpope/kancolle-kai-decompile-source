using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_tanker", Namespace = "")]
	public class Mem_tanker : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private DispositionStatus _disposition_status;

		[DataMember]
		private int _mission_deck_rid;

		[DataMember]
		private int _bling_start;

		[DataMember]
		private int _bling_end;

		private static string _tableName = "mem_tanker";

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

		public int Mission_deck_rid
		{
			get
			{
				return this._mission_deck_rid;
			}
			private set
			{
				this._mission_deck_rid = value;
			}
		}

		public int Bling_start
		{
			get
			{
				return this._bling_start;
			}
			private set
			{
				this._bling_start = value;
			}
		}

		public int Bling_end
		{
			get
			{
				return this._bling_end;
			}
			private set
			{
				this._bling_end = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_tanker._tableName;
			}
		}

		public Mem_tanker()
		{
			this.Maparea_id = 1;
			this.Disposition_status = DispositionStatus.NONE;
			this.Bling_start = 0;
			this.Bling_end = 0;
		}

		public Mem_tanker(int rid) : this()
		{
			this.Rid = rid;
		}

		public void GoArea(int area_id)
		{
			this.Maparea_id = area_id;
			this.Disposition_status = DispositionStatus.ARRIVAL;
			this.Bling_start = 0;
			this.Bling_end = 0;
		}

		public void BackTanker()
		{
			if (this.Disposition_status == DispositionStatus.NONE && !this.IsBlingShip())
			{
				return;
			}
			int maparea_id = this.Maparea_id;
			this.Maparea_id = 1;
			this.Disposition_status = DispositionStatus.NONE;
			this.Bling_start = Comm_UserDatas.Instance.User_turn.Total_turn;
			this.Bling_end = Comm_UserDatas.Instance.User_turn.Total_turn + Mst_DataManager.Instance.Mst_maparea.get_Item(maparea_id).Distance;
		}

		public bool IsBlingShip()
		{
			return this.Bling_end - this.Bling_start > 0;
		}

		public int GetBlingTurn()
		{
			if (!this.IsBlingShip())
			{
				return 0;
			}
			return this.Bling_end - Comm_UserDatas.Instance.User_turn.Total_turn;
		}

		public bool BlingTerm()
		{
			if (!this.IsBlingShip())
			{
				return false;
			}
			if (this.Bling_end > Comm_UserDatas.Instance.User_turn.Total_turn)
			{
				return false;
			}
			this.Bling_start = 0;
			this.Bling_end = 0;
			return true;
		}

		public bool MissionStart(int area_id, int deck_id)
		{
			if (this.Disposition_status != DispositionStatus.NONE)
			{
				return false;
			}
			this.Disposition_status = DispositionStatus.MISSION;
			this.Mission_deck_rid = deck_id;
			return true;
		}

		public bool MissionTerm()
		{
			if (this.Disposition_status != DispositionStatus.MISSION)
			{
				return false;
			}
			this.Disposition_status = DispositionStatus.NONE;
			this.Mission_deck_rid = 0;
			return true;
		}

		public static IEnumerable<IGrouping<int, Mem_tanker>> GetAreaEnableTanker(Dictionary<int, Mem_tanker> tankerItems)
		{
			return Enumerable.GroupBy<Mem_tanker, int, Mem_tanker>(Enumerable.Where<Mem_tanker>(tankerItems.get_Values(), (Mem_tanker item) => item.Disposition_status == DispositionStatus.ARRIVAL && !item.IsBlingShip()), (Mem_tanker item) => item.Maparea_id, (Mem_tanker item) => item);
		}

		public static List<Mem_tanker> GetAreaTanker(int mapAreaId)
		{
			Dictionary<int, Mem_tanker>.ValueCollection values = Comm_UserDatas.Instance.User_tanker.get_Values();
			return Enumerable.ToList<Mem_tanker>(Enumerable.Where<Mem_tanker>(values, (Mem_tanker x) => x.Maparea_id == mapAreaId));
		}

		public static List<Mem_tanker> GetFreeTanker(Dictionary<int, Mem_tanker> tankerItems)
		{
			return Enumerable.ToList<Mem_tanker>(Enumerable.Where<Mem_tanker>(tankerItems.get_Values(), (Mem_tanker x) => x.Disposition_status == DispositionStatus.NONE && x.Maparea_id == 1 && !x.IsBlingShip()));
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Maparea_id = int.Parse(element.Element("_maparea_id").get_Value());
			this.Disposition_status = (DispositionStatus)((int)Enum.Parse(typeof(DispositionStatus), element.Element("_disposition_status").get_Value()));
			this.Bling_start = int.Parse(element.Element("_bling_start").get_Value());
			this.Bling_end = int.Parse(element.Element("_bling_end").get_Value());
			this.Mission_deck_rid = int.Parse(element.Element("_mission_deck_rid").get_Value());
		}
	}
}
