using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace Server_Models
{
	[DataContract(Name = "mem_missioncomp", Namespace = "")]
	public class Mem_missioncomp : Model_Base
	{
		[DataMember]
		private int _rid;

		[DataMember]
		private int _maparea_id;

		[DataMember]
		private MissionClearKinds _state;

		private static string _tableName = "mem_missioncomp";

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

		public MissionClearKinds State
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
				return Mem_missioncomp._tableName;
			}
		}

		public Mem_missioncomp()
		{
		}

		public Mem_missioncomp(int rid, int maparea_id, MissionClearKinds state) : this()
		{
			this.Rid = rid;
			this.Maparea_id = maparea_id;
			this.State = state;
		}

		public bool Insert()
		{
			if (!Comm_UserDatas.Instance.User_missioncomp.ContainsKey(this.Rid))
			{
				Comm_UserDatas.Instance.User_missioncomp.Add(this.Rid, this);
			}
			return true;
		}

		public bool Update()
		{
			Mem_missioncomp mem_missioncomp = null;
			if (Comm_UserDatas.Instance.User_missioncomp.TryGetValue(this.Rid, ref mem_missioncomp))
			{
				mem_missioncomp.Maparea_id = this.Maparea_id;
				mem_missioncomp.Rid = this.Rid;
				mem_missioncomp.State = this.State;
			}
			return true;
		}

		public List<User_MissionFmt> GetActiveMission()
		{
			Dictionary<int, Mst_mission2> mst_mission = Mst_DataManager.Instance.Mst_mission;
			if (Comm_UserDatas.Instance.User_missioncomp.get_Count() == 0)
			{
				return this.newUserActiveMission(mst_mission);
			}
			var enumerable = Enumerable.Select(Comm_UserDatas.Instance.User_missioncomp.get_Values(), (Mem_missioncomp element) => new
			{
				id = element.Rid,
				state = element.State
			});
			List<User_MissionFmt> list = new List<User_MissionFmt>();
			using (Dictionary<int, Mst_mission2>.ValueCollection.Enumerator enumerator = mst_mission.get_Values().GetEnumerator())
			{
				Mst_mission2 mst_item;
				while (enumerator.MoveNext())
				{
					mst_item = enumerator.get_Current();
					if (Mst_DataManager.Instance.Mst_maparea.ContainsKey(mst_item.Maparea_id))
					{
						if (Mst_DataManager.Instance.Mst_maparea.get_Item(mst_item.Maparea_id).Evt_flag == 0)
						{
							var <>__AnonType = Enumerable.FirstOrDefault(enumerable, x => x.id == mst_item.Id);
							if (<>__AnonType != null)
							{
								list.Add(new User_MissionFmt
								{
									MissionId = mst_item.Id,
									State = <>__AnonType.state
								});
							}
							else if (string.IsNullOrEmpty(mst_item.Required_ids))
							{
								list.Add(new User_MissionFmt
								{
									MissionId = mst_item.Id,
									State = MissionClearKinds.NEW
								});
							}
							else
							{
								string[] array = mst_item.Required_ids.Split(new char[]
								{
									','
								});
								bool flag = true;
								string[] array2 = array;
								for (int i = 0; i < array2.Length; i++)
								{
									string text = array2[i];
									int id = int.Parse(text);
									var <>__AnonType2 = Enumerable.FirstOrDefault(enumerable, y => y.id == id);
									if (<>__AnonType2 == null)
									{
										flag = false;
										break;
									}
									MissionClearKinds state = <>__AnonType2.state;
									if (state != MissionClearKinds.CLEARED)
									{
										flag = false;
										break;
									}
								}
								if (flag)
								{
									list.Add(new User_MissionFmt
									{
										MissionId = mst_item.Id,
										State = MissionClearKinds.NEW
									});
								}
							}
						}
					}
				}
			}
			return Enumerable.ToList<User_MissionFmt>(Enumerable.OrderBy<User_MissionFmt, int>(list, (User_MissionFmt x) => x.MissionId));
		}

		private List<User_MissionFmt> newUserActiveMission(Dictionary<int, Mst_mission2> mst_mission)
		{
			List<User_MissionFmt> list = new List<User_MissionFmt>();
			using (Dictionary<int, Mst_mission2>.ValueCollection.Enumerator enumerator = mst_mission.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_mission2 current = enumerator.get_Current();
					if (Mst_DataManager.Instance.Mst_maparea.ContainsKey(current.Maparea_id))
					{
						if (Mst_DataManager.Instance.Mst_maparea.get_Item(current.Maparea_id).Evt_flag == 0)
						{
							if (string.IsNullOrEmpty(current.Required_ids))
							{
								list.Add(new User_MissionFmt
								{
									MissionId = current.Id,
									State = MissionClearKinds.NEW
								});
							}
						}
					}
				}
			}
			return Enumerable.ToList<User_MissionFmt>(Enumerable.OrderBy<User_MissionFmt, int>(list, (User_MissionFmt x) => x.MissionId));
		}

		protected override void setProperty(XElement element)
		{
			this.Rid = int.Parse(element.Element("_rid").get_Value());
			this.Maparea_id = int.Parse(element.Element("_maparea_id").get_Value());
			this.State = (MissionClearKinds)((int)Enum.Parse(typeof(MissionClearKinds), element.Element("_state").get_Value()));
		}
	}
}
