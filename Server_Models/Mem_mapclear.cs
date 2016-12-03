using Server_Common;
using System;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_mapclear", Namespace = "")]
	public class Mem_mapclear : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private int _mapinfo_no;

		[DataMember]
		private MapClearState _state;

		[DataMember]
		private bool _cleared;

		private static string _tableName = "mem_mapclear";

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

		public int Mapinfo_no
		{
			get
			{
				return this._mapinfo_no;
			}
			private set
			{
				this._mapinfo_no = value;
			}
		}

		public MapClearState State
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

		public bool Cleared
		{
			get
			{
				return this._cleared;
			}
			private set
			{
				this._cleared = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_mapclear._tableName;
			}
		}

		public Mem_mapclear()
		{
			this.Cleared = false;
		}

		public Mem_mapclear(int mapinfo_id, int maparea_id, int mapinfo_no, MapClearState state) : this()
		{
			this.Rid = mapinfo_id;
			this.Maparea_id = maparea_id;
			this.Mapinfo_no = mapinfo_no;
			this.State = state;
			if (state == MapClearState.Cleard)
			{
				this.Cleared = true;
			}
		}

		public void Insert()
		{
			if (Comm_UserDatas.Instance.User_mapclear.ContainsKey(this.Rid))
			{
				return;
			}
			Comm_UserDatas.Instance.User_mapclear.Add(this.Rid, this);
		}

		public void StateChange(MapClearState state)
		{
			this.State = state;
			if (state == MapClearState.Cleard)
			{
				this.Cleared = true;
			}
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Maparea_id = int.Parse(element.Element("_maparea_id").get_Value());
			this.Mapinfo_no = int.Parse(element.Element("_mapinfo_no").get_Value());
			this.State = (MapClearState)((int)Enum.Parse(typeof(MapClearState), element.Element("_state").get_Value()));
			this.Cleared = bool.Parse(element.Element("_cleared").get_Value());
		}
	}
}
