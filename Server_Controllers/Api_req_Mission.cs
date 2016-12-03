using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Controllers.MissionLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_Mission
	{
		private Dictionary<int, int> mst_level;

		private Dictionary<int, int> mst_userlevel;

		public HashSet<IsGoCondition> ValidStart(int deck_rid, int mission_id, int tankerNum)
		{
			HashSet<IsGoCondition> ret = new HashSet<IsGoCondition>();
			Mst_mission2 mst_mission = null;
			if (mission_id > 0)
			{
				if (!Mst_DataManager.Instance.Mst_mission.TryGetValue(mission_id, ref mst_mission))
				{
					ret.Add(IsGoCondition.Invalid);
					return ret;
				}
			}
			else
			{
				if (mission_id != -1 && mission_id != -2)
				{
					ret.Add(IsGoCondition.Invalid);
					return ret;
				}
				List<Mst_mission2> supportResistedData = Mst_DataManager.Instance.GetSupportResistedData(Comm_UserDatas.Instance.User_deck.get_Item(deck_rid).Area_id);
				mst_mission = ((mission_id != -1) ? supportResistedData.get_Item(1) : supportResistedData.get_Item(0));
			}
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				ret.Add(IsGoCondition.InvalidDeck);
				return ret;
			}
			if (mem_deck.Ship.Count() == 0)
			{
				ret.Add(IsGoCondition.InvalidDeck);
				return ret;
			}
			if (mem_deck.MissionState != MissionStates.NONE)
			{
				ret.Add(IsGoCondition.Mission);
			}
			if (mem_deck.IsActionEnd)
			{
				ret.Add(IsGoCondition.ActionEndDeck);
				return ret;
			}
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(mem_deck.Ship[0], ref mem_ship))
			{
				ret.Add(IsGoCondition.Invalid);
				return ret;
			}
			if (mem_ship.Get_DamageState() == DamageState.Taiha)
			{
				ret.Add(IsGoCondition.FlagShipTaiha);
			}
			Mem_deck mem_deck2 = Enumerable.FirstOrDefault<Mem_deck>(Comm_UserDatas.Instance.User_deck.get_Values(), (Mem_deck x) => x.Mission_id == mission_id);
			if (mem_deck2 != null)
			{
				ret.Add(IsGoCondition.OtherDeckMissionRunning);
			}
			int destroy_ship = 0;
			mem_deck.Ship.getMemShip().ForEach(delegate(Mem_ship deck_ship)
			{
				if (deck_ship.Stype == 2)
				{
					destroy_ship++;
				}
				Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(deck_ship.Ship_id);
				if (deck_ship.Fuel <= 0 || deck_ship.Bull <= 0)
				{
					ret.Add(IsGoCondition.NeedSupply);
				}
				if (mst_mission.IsSupportMission() && (deck_ship.Fuel < mst_ship.Fuel_max || deck_ship.Bull < mst_ship.Bull_max))
				{
					ret.Add(IsGoCondition.ReqFullSupply);
				}
				if (Enumerable.Any<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock ndock) => ndock.Ship_id == deck_ship.Rid))
				{
					ret.Add(IsGoCondition.HasRepair);
				}
				if (deck_ship.IsBlingShip())
				{
					ret.Add(IsGoCondition.HasBling);
				}
			});
			if (mst_mission.IsSupportMission() && destroy_ship < 2)
			{
				ret.Add(IsGoCondition.NecessaryStype);
			}
			List<Mem_tanker> freeTanker = Mem_tanker.GetFreeTanker(Comm_UserDatas.Instance.User_tanker);
			if (freeTanker.get_Count() < tankerNum)
			{
				ret.Add(IsGoCondition.Tanker);
			}
			return ret;
		}

		public Api_Result<Mem_deck> Start(int deck_rid, int mission_id, int tankerNum)
		{
			Api_Result<Mem_deck> api_Result = new Api_Result<Mem_deck>();
			Mst_mission2 mst_mission = null;
			if (mission_id > 0)
			{
				if (!Mst_DataManager.Instance.Mst_mission.TryGetValue(mission_id, ref mst_mission))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			else
			{
				List<Mst_mission2> supportResistedData = Mst_DataManager.Instance.GetSupportResistedData(Comm_UserDatas.Instance.User_deck.get_Item(deck_rid).Area_id);
				mst_mission = ((mission_id != -1) ? supportResistedData.get_Item(1) : supportResistedData.get_Item(0));
			}
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mission_id > 0 && !Comm_UserDatas.Instance.User_missioncomp.ContainsKey(mission_id))
			{
				Mem_missioncomp mem_missioncomp = new Mem_missioncomp(mission_id, mst_mission.Maparea_id, MissionClearKinds.NOTCLEAR);
				mem_missioncomp.Insert();
			}
			if (!mem_deck.MissionStart(mst_mission))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (tankerNum > 0)
			{
				IEnumerable<Mem_tanker> enumerable = Enumerable.Take<Mem_tanker>(Mem_tanker.GetFreeTanker(Comm_UserDatas.Instance.User_tanker), tankerNum);
				List<Mem_tanker> list = new List<Mem_tanker>();
				using (IEnumerator<Mem_tanker> enumerator = enumerable.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_tanker current = enumerator.get_Current();
						if (!current.MissionStart(mst_mission.Maparea_id, deck_rid))
						{
							list.ForEach(delegate(Mem_tanker x)
							{
								x.MissionTerm();
							});
							api_Result.state = Api_Result_State.Parameter_Error;
							return api_Result;
						}
						list.Add(current);
					}
				}
			}
			api_Result.data = mem_deck;
			QuestMission questMission = new QuestMission(mst_mission.Id, mem_deck, MissionResultKinds.FAILE);
			questMission.ExecuteCheck();
			return api_Result;
		}

		public bool ValidStop(int deck_rid)
		{
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				return false;
			}
			if (mem_deck.MissionState != MissionStates.RUNNING)
			{
				return false;
			}
			if (mem_deck.SupportKind != Mem_deck.SupportKinds.NONE)
			{
				return false;
			}
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			return mem_deck.CompleteTime > total_turn;
		}

		public Api_Result<int> Stop(int deck_rid)
		{
			Api_Result<int> api_Result = new Api_Result<int>();
			Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.get_Item(deck_rid);
			Mst_mission2 mst_mission = Mst_DataManager.Instance.Mst_mission.get_Item(mem_deck.Mission_id);
			int num = mst_mission.Time - (mem_deck.StartTime - mem_deck.CompleteTime);
			double num2 = Math.Ceiling((double)(mst_mission.Time + num) / 2.0);
			int total_turn = Comm_UserDatas.Instance.User_turn.Total_turn;
			int num3 = ((double)(total_turn - mem_deck.StartTime) >= num2) ? 2 : 1;
			double num4;
			if (num3 == 1)
			{
				num4 = (double)(total_turn - mem_deck.StartTime);
			}
			else
			{
				num4 = (double)(mem_deck.CompleteTime - total_turn);
			}
			num4 = Math.Ceiling(num4 / 3.0);
			int newEndTime = (int)((double)total_turn + num4);
			mem_deck.MissionStop(newEndTime);
			api_Result.data = mem_deck.StartTime - mem_deck.CompleteTime;
			return api_Result;
		}

		public Api_Result<MissionResultFmt> Result(int deck_rid)
		{
			Api_Result<MissionResultFmt> api_Result = new Api_Result<MissionResultFmt>();
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_deck.MissionState != MissionStates.END && mem_deck.MissionState != MissionStates.STOP)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_deck.Ship.Count() == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.mst_level == null)
			{
				this.mst_level = Mst_DataManager.Instance.Get_MstLevel(true);
			}
			if (this.mst_userlevel == null)
			{
				this.mst_userlevel = Mst_DataManager.Instance.Get_MstLevel(false);
			}
			int mission_id = mem_deck.Mission_id;
			bool flag = mem_deck.MissionState == MissionStates.STOP;
			Exec_MissionResult exec_MissionResult = new Exec_MissionResult(mem_deck, this.mst_level, this.mst_userlevel);
			api_Result.data = exec_MissionResult.GetResultData();
			if (!flag)
			{
				QuestMission questMission = new QuestMission(mission_id, mem_deck, api_Result.data.MissionResult);
				questMission.ExecuteCheck();
			}
			return api_Result;
		}
	}
}
