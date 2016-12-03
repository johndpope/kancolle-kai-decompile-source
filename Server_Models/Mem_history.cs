using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_history", Namespace = "")]
	public class Mem_history : Model_Base
	{
		public const int MAPCLEAR_MAX_RECORD = 3;

		[DataMember]
		private int _rid;

		[DataMember]
		private int _type;

		[DataMember]
		private int _map_clear_num;

		[DataMember]
		private int _turn;

		[DataMember]
		private int _mapinfo_id;

		[DataMember]
		private int _flagship_id;

		[DataMember]
		private bool _tanker_lost_all;

		[DataMember]
		private int _game_end_type;

		private static string _tableName = "mem_history";

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

		public int Type
		{
			get
			{
				return this._type;
			}
			private set
			{
				this._type = value;
			}
		}

		public int MapClearNum
		{
			get
			{
				return this._map_clear_num;
			}
			private set
			{
				this._map_clear_num = value;
			}
		}

		public int Turn
		{
			get
			{
				return this._turn;
			}
			private set
			{
				this._turn = value;
			}
		}

		public int MapinfoId
		{
			get
			{
				return this._mapinfo_id;
			}
			private set
			{
				this._mapinfo_id = value;
			}
		}

		public int FlagShipId
		{
			get
			{
				return this._flagship_id;
			}
			private set
			{
				this._flagship_id = value;
			}
		}

		public bool TankerLostAll
		{
			get
			{
				return this._tanker_lost_all;
			}
			private set
			{
				this._tanker_lost_all = value;
			}
		}

		public int GameEndType
		{
			get
			{
				return this._game_end_type;
			}
			private set
			{
				this._game_end_type = value;
			}
		}

		public static string tableName
		{
			get
			{
				return Mem_history._tableName;
			}
		}

		public static int GetMapClearNum(int mapinfo_id)
		{
			List<Mem_history> list = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(1, ref list))
			{
				return 1;
			}
			int num = Enumerable.Count<Mem_history>(list, (Mem_history x) => x.MapinfoId == mapinfo_id);
			return num + 1;
		}

		public static bool IsFirstOpenArea(int mapinfo_id)
		{
			if (Mst_DataManager.Instance.Mst_mapinfo.get_Item(mapinfo_id).No != 1)
			{
				return false;
			}
			List<Mem_history> list = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(3, ref list))
			{
				return true;
			}
			bool flag = Enumerable.Any<Mem_history>(list, (Mem_history x) => x.MapinfoId == mapinfo_id);
			return !flag;
		}

		public void SetMapClear(int turn, int mapinfo_id, int clearNum, int flagship_id)
		{
			this.setNewRid();
			this.Type = 1;
			this.Turn = turn;
			this.MapClearNum = clearNum;
			this.MapinfoId = mapinfo_id;
			this.FlagShipId = flagship_id;
		}

		public void SetTanker(int turn, int mapinfo_id, bool tanker_destroy)
		{
			this.setNewRid();
			this.Type = 2;
			this.Turn = turn;
			this.MapinfoId = mapinfo_id;
			this.TankerLostAll = tanker_destroy;
		}

		public void SetAreaOpen(int turn, int mapinfo_id)
		{
			this.setNewRid();
			this.Type = 3;
			this.Turn = turn;
			this.MapinfoId = mapinfo_id;
		}

		public void SetGameClear(int turn)
		{
			this.setNewRid();
			this.Type = 4;
			this.Turn = turn;
			this.GameEndType = 1;
		}

		public void SetGameOverToLost(int turn)
		{
			this.setNewRid();
			this.Type = 4;
			this.Turn = turn;
			this.GameEndType = 2;
		}

		public void SetGameOverToTurn(int turn)
		{
			this.setNewRid();
			this.Type = 4;
			this.Turn = turn;
			this.GameEndType = 3;
		}

		public void SetAreaComplete(int mapinfo)
		{
			this.setNewRid();
			this.Type = 999;
			this.MapinfoId = mapinfo;
		}

		private void setNewRid()
		{
			int num = 0;
			using (Dictionary<int, List<Mem_history>>.ValueCollection.Enumerator enumerator = Comm_UserDatas.Instance.User_history.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<Mem_history> current = enumerator.get_Current();
					num += current.get_Count();
				}
			}
			this.Rid = num + 1;
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Type = int.Parse(element.Element("_type").get_Value());
			this.MapClearNum = int.Parse(element.Element("_map_clear_num").get_Value());
			this.Turn = int.Parse(element.Element("_turn").get_Value());
			this.MapinfoId = int.Parse(element.Element("_mapinfo_id").get_Value());
			this.MapClearNum = int.Parse(element.Element("_map_clear_num").get_Value());
			this.FlagShipId = int.Parse(element.Element("_flagship_id").get_Value());
			this.TankerLostAll = bool.Parse(element.Element("_tanker_lost_all").get_Value());
			this.GameEndType = int.Parse(element.Element("_game_end_type").get_Value());
		}
	}
}
