using Common.Enum;
using local.models;
using local.utils;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class StrategyMapManager : TurnManager
	{
		public Dictionary<int, MapAreaModel> Area
		{
			get
			{
				return new Dictionary<int, MapAreaModel>(ManagerBase._area);
			}
		}

		public StrategyMapManager()
		{
			base._CreateMapAreaModel();
		}

		public bool IsOpenedLastAreaAtLeastOnce()
		{
			return new Api_req_StrategeMap().IsLastAreaOpend();
		}

		public AreaTankerModel GetNonDeploymentTankerCount()
		{
			return this._tanker_manager.GetCounts();
		}

		public List<MapAreaModel> GetValidMoveToArea(int deck_id)
		{
			List<MapAreaModel> list = new List<MapAreaModel>();
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			if (deck.IsValidMove().get_Count() > 0)
			{
				return list;
			}
			int areaId = deck.AreaId;
			List<int> neighboringAreaIDs = ManagerBase._area.get_Item(areaId).NeighboringAreaIDs;
			for (int i = 0; i < neighboringAreaIDs.get_Count(); i++)
			{
				int num = neighboringAreaIDs.get_Item(i);
				MapAreaModel mapAreaModel = ManagerBase._area.get_Item(num);
				if (mapAreaModel.IsOpen())
				{
					list.Add(mapAreaModel);
				}
			}
			return list;
		}

		public bool Move(int deck_id, int move_to_area_id)
		{
			Api_Result<Mem_deck> api_Result = new Api_req_StrategeMap().MoveArea(deck_id, move_to_area_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				ManagerBase._turn_state = api_Result.t_state;
				return true;
			}
			return false;
		}

		public SortieManager SelectArea(int area_id)
		{
			return new SortieManager(area_id);
		}

		public HashSet<int> GetMissionAreaId()
		{
			HashSet<int> hashSet = new HashSet<int>();
			Api_Result<List<User_MissionFmt>> api_Result = new Api_get_Member().Mission();
			if (api_Result.state != Api_Result_State.Success)
			{
				return hashSet;
			}
			Dictionary<int, Mst_mission2> mst_mission = Mst_DataManager.Instance.Mst_mission;
			for (int i = 0; i < api_Result.data.get_Count(); i++)
			{
				User_MissionFmt user_MissionFmt = api_Result.data.get_Item(i);
				Mst_mission2 mst_mission2;
				if (mst_mission.TryGetValue(user_MissionFmt.MissionId, ref mst_mission2) && !hashSet.Contains(mst_mission2.Maparea_id))
				{
					hashSet.Add(mst_mission2.Maparea_id);
				}
			}
			return hashSet;
		}

		public List<MapAreaModel> GetRebellionAreaList()
		{
			List<int> rebellionAreaOrderByEvent = new Api_req_StrategeMap().GetRebellionAreaOrderByEvent();
			List<MapAreaModel> list = new List<MapAreaModel>();
			for (int i = 0; i < rebellionAreaOrderByEvent.get_Count(); i++)
			{
				int num = rebellionAreaOrderByEvent.get_Item(i);
				MapAreaModel mapAreaModel = ManagerBase._area.get_Item(num);
				list.Add(mapAreaModel);
			}
			return list;
		}

		public RebellionManager SelectAreaForRebellion(int area_id)
		{
			return new RebellionManager(area_id);
		}

		public bool IsValidChangeTankerCount(int area_id, int count)
		{
			AreaTankerModel tankerCount = ManagerBase._area.get_Item(area_id).GetTankerCount();
			int count2 = tankerCount.GetCount();
			if (count2 > count)
			{
				int num = count2 - count;
				int count3 = new Api_req_Transport().GetEnableBackTanker(area_id).get_Count();
				return count3 >= num;
			}
			if (count2 >= count)
			{
				return true;
			}
			int num2 = count - count2;
			if (!ManagerBase._area.get_Item(area_id).IsOpen())
			{
				return false;
			}
			if (tankerCount.GetMaxCount() < count)
			{
				return false;
			}
			AreaTankerModel nonDeploymentTankerCount = this.GetNonDeploymentTankerCount();
			return nonDeploymentTankerCount.GetCount() - nonDeploymentTankerCount.GetCountMove() >= num2;
		}

		public void GetTankerCountRange(int area_id, out int out_max, out int out_min)
		{
			AreaTankerModel tankerCount = ManagerBase._area.get_Item(area_id).GetTankerCount();
			AreaTankerModel nonDeploymentTankerCount = this.GetNonDeploymentTankerCount();
			out_max = tankerCount.GetCount() + nonDeploymentTankerCount.GetCount() - nonDeploymentTankerCount.GetCountMove();
			out_max = Math.Min(out_max, tankerCount.GetMaxCount());
			out_min = tankerCount.GetCountMove();
		}

		[Obsolete("local.utils.Utils.GetAreaResource(int area_id, int tanker_count, EscortDeckManager eManager) を使用してください", false)]
		public Dictionary<enumMaterialCategory, int> GetAreaResource(int area_id, int tanker_count)
		{
			return Utils.GetAreaResource(area_id, tanker_count);
		}

		public bool IsValidDeploy(int area_id, int tanker_count, EscortDeckManager escort)
		{
			if (!this.IsValidChangeTankerCount(area_id, tanker_count))
			{
				return false;
			}
			AreaTankerModel tankerCount = ManagerBase._area.get_Item(area_id).GetTankerCount();
			return escort.HasChanged() || tankerCount.GetCount() != tanker_count;
		}

		public bool Deploy(int area_id, int tanker_count, EscortDeckManager escort)
		{
			AreaTankerModel tankerCount = ManagerBase._area.get_Item(area_id).GetTankerCount();
			int count = tankerCount.GetCount();
			if (!this.IsValidDeploy(area_id, tanker_count, escort))
			{
				return false;
			}
			bool flag = false;
			if (escort.HasChanged())
			{
				flag = escort.__Commit__();
			}
			Api_Result<List<Mem_tanker>> api_Result = null;
			if (count != tanker_count)
			{
				if (count > tanker_count)
				{
					int num = count - tanker_count;
					api_Result = new Api_req_Transport().BackTanker(area_id, num);
				}
				else if (count < tanker_count)
				{
					int num2 = tanker_count - count;
					api_Result = new Api_req_Transport().GoTanker(area_id, num2);
				}
			}
			if (!flag && (api_Result == null || api_Result.state != Api_Result_State.Success))
			{
				return false;
			}
			if (flag)
			{
				base.UserInfo.__UpdateEscortDeck__(new Api_get_Member());
			}
			if (api_Result != null && api_Result.state == Api_Result_State.Success)
			{
				return this._tanker_manager.Update();
			}
			return flag;
		}

		public override UserPreActionPhaseResultModel GetResult_UserPreActionPhase()
		{
			UserPreActionPhaseResultModel result_UserPreActionPhase = base.GetResult_UserPreActionPhase();
			base._CreateMapAreaModel();
			return result_UserPreActionPhase;
		}

		public override UserActionPhaseResultModel GetResult_UserActionPhase()
		{
			return base.GetResult_UserActionPhase();
		}

		public override string ToString()
		{
			return this.ToString(false);
		}

		public string ToString(bool detail)
		{
			string text = string.Empty;
			text += string.Format("{0}\n", base.ToString());
			AreaTankerModel nonDeploymentTankerCount = this.GetNonDeploymentTankerCount();
			text += string.Format("海域に未配備の輸送船数:{0}(帰港中:{1})\n", nonDeploymentTankerCount.GetCount(), nonDeploymentTankerCount.GetCountMove());
			if (detail)
			{
				text += string.Format("[--海域一覧--]\n", new object[0]);
				Dictionary<int, MapAreaModel> area = this.Area;
				using (Dictionary<int, MapAreaModel>.ValueCollection.Enumerator enumerator = area.get_Values().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						MapAreaModel current = enumerator.get_Current();
						SortieManager sortieManager = this.SelectArea(current.Id);
						MapModel[] maps = sortieManager.Maps;
						text += string.Format("{0} ", current);
						for (int i = 0; i < maps.Length; i++)
						{
							MapModel mapModel = maps[i];
							if (mapModel.Map_Possible)
							{
								text += string.Format("{0}-{1}{2}{3} ", new object[]
								{
									mapModel.AreaId,
									mapModel.No,
									(!mapModel.Cleared) ? string.Empty : "[Clear]",
									(!mapModel.ClearedOnce) ? string.Empty : "!"
								});
							}
							else
							{
								text += string.Format("({0}-{1}{2}{3}) ", new object[]
								{
									mapModel.AreaId,
									mapModel.No,
									(!mapModel.Cleared) ? string.Empty : "[Clear]",
									(!mapModel.ClearedOnce) ? string.Empty : "!"
								});
							}
						}
						text += "\n";
					}
				}
				text += string.Format("[--海域一覧--]", new object[0]);
			}
			return text;
		}
	}
}
