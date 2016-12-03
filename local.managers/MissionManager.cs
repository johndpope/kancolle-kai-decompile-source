using Common.Enum;
using local.models;
using Server_Common.Formats;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class MissionManager : ManagerBase
	{
		private int _area_id;

		private Api_req_Mission _req_mission;

		private Dictionary<int, List<MissionModel>> _mission_dic;

		public int AreaId
		{
			get
			{
				return this._area_id;
			}
		}

		public int TankerCount
		{
			get
			{
				return this._tanker_manager.GetCounts().GetCountNoMove();
			}
		}

		public MissionManager(int area_id)
		{
			this._area_id = area_id;
			this._req_mission = new Api_req_Mission();
			this._tanker_manager = new _TankerManager();
		}

		public MissionModel GetMission(int mission_id)
		{
			if (this._mission_dic == null)
			{
				this._UpdateMission();
			}
			using (Dictionary<int, List<MissionModel>>.ValueCollection.Enumerator enumerator = this._mission_dic.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					List<MissionModel> current = enumerator.get_Current();
					MissionModel missionModel = current.Find((MissionModel mission) => mission.Id == mission_id);
					if (missionModel != null)
					{
						return missionModel;
					}
				}
			}
			return null;
		}

		public MissionModel[] GetMissions()
		{
			if (this._mission_dic == null)
			{
				this._UpdateMission();
			}
			if (this._mission_dic.ContainsKey(this._area_id))
			{
				return this._mission_dic.get_Item(this._area_id).ToArray();
			}
			return new MissionModel[0];
		}

		public MissionModel[] GetMissionsWithoutDeck()
		{
			if (this._mission_dic == null)
			{
				this._UpdateMission();
			}
			if (this._mission_dic.ContainsKey(this._area_id))
			{
				List<MissionModel> list = this._mission_dic.get_Item(this._area_id);
				list = list.FindAll((MissionModel mission) => mission.Deck == null);
				return list.ToArray();
			}
			return new MissionModel[0];
		}

		public List<IsGoCondition> IsValidMissionStart(int deck_id, int mission_id, int tanker_count)
		{
			DeckModel deck = base.UserInfo.GetDeck(deck_id);
			List<IsGoCondition> list = deck.IsValidMission();
			HashSet<IsGoCondition> hashSet = this._req_mission.ValidStart(deck_id, mission_id, tanker_count);
			List<IsGoCondition> list2 = new List<IsGoCondition>(hashSet);
			list2.Sort();
			for (int i = 0; i < list2.get_Count(); i++)
			{
				if (!list.Contains(list2.get_Item(i)))
				{
					list.Add(list2.get_Item(i));
				}
			}
			return list;
		}

		public bool MissionStart(int deck_id, int mission_id, int tanker_count)
		{
			if (this.IsValidMissionStart(deck_id, mission_id, tanker_count).get_Count() == 0)
			{
				Api_Result<Mem_deck> api_Result = this._req_mission.Start(deck_id, mission_id, tanker_count);
				if (api_Result.state == Api_Result_State.Success)
				{
					this._mission_dic = null;
					return true;
				}
			}
			return false;
		}

		public bool IsValidMissionStop(int deck_id)
		{
			return this._req_mission.ValidStop(deck_id);
		}

		public bool MissionStop(int deck_id)
		{
			if (!this._req_mission.ValidStop(deck_id))
			{
				return false;
			}
			Api_Result<int> api_Result = this._req_mission.Stop(deck_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				this._mission_dic = null;
				return true;
			}
			return false;
		}

		public void UpdateMissionStates()
		{
			this._UpdateMission();
		}

		private void _UpdateMission()
		{
			this._mission_dic = new Dictionary<int, List<MissionModel>>();
			Api_Result<List<User_MissionFmt>> api_Result = new Api_get_Member().Mission();
			if (api_Result.state != Api_Result_State.Success)
			{
				return;
			}
			DeckModel[] decksFromArea = base.UserInfo.GetDecksFromArea(this._area_id);
			Dictionary<int, DeckModel> dictionary = new Dictionary<int, DeckModel>();
			for (int i = 0; i < decksFromArea.Length; i++)
			{
				DeckModel deckModel = decksFromArea[i];
				if (deckModel.MissionState != MissionStates.NONE)
				{
					dictionary.Add(deckModel.MissionId, deckModel);
				}
			}
			for (int j = 0; j < api_Result.data.get_Count(); j++)
			{
				User_MissionFmt user_MissionFmt = api_Result.data.get_Item(j);
				MissionModel missionModel;
				if (dictionary.ContainsKey(user_MissionFmt.MissionId))
				{
					missionModel = new MissionModel(user_MissionFmt, dictionary.get_Item(user_MissionFmt.MissionId));
				}
				else
				{
					missionModel = new MissionModel(user_MissionFmt);
				}
				if (!this._mission_dic.ContainsKey(missionModel.AreaId))
				{
					this._mission_dic.set_Item(missionModel.AreaId, new List<MissionModel>());
				}
				this._mission_dic.get_Item(missionModel.AreaId).Add(missionModel);
			}
		}
	}
}
