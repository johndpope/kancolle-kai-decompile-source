using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_StrategeMap
	{
		private Dictionary<int, User_StrategyMapFmt> strategyFmt;

		public Api_req_StrategeMap()
		{
			Api_get_Member api_get_Member = new Api_get_Member();
			this.strategyFmt = api_get_Member.StrategyInfo().data;
		}

		public bool IsLastAreaOpend()
		{
			List<Mem_history> list = null;
			if (!Comm_UserDatas.Instance.User_history.TryGetValue(3, ref list))
			{
				return false;
			}
			return Enumerable.Any<Mem_history>(list, (Mem_history x) => x.MapinfoId == 171);
		}

		public Api_Result<Mem_deck> MoveArea(int rid, int move_id)
		{
			Api_Result<Mem_deck> api_Result = new Api_Result<Mem_deck>();
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(rid, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			mem_deck.MoveArea(move_id);
			mem_deck.ActionEnd();
			api_Result.t_state = TurnState.OWN_END;
			return api_Result;
		}

		public List<int> GetRebellionAreaOrderByEvent()
		{
			return Enumerable.ToList<int>(Enumerable.Select<Mem_rebellion_point, int>(Enumerable.OrderByDescending<Mem_rebellion_point, int>(Enumerable.Where<Mem_rebellion_point>(Comm_UserDatas.Instance.User_rebellion_point.get_Values(), (Mem_rebellion_point item) => item.State == RebellionState.Invation), (Mem_rebellion_point item) => item.Rid), (Mem_rebellion_point item) => item.Rid));
		}

		public bool ExecuteRebellionLostArea(int maparea_id)
		{
			new RebellionUtils().LostArea(maparea_id, null);
			return true;
		}
	}
}
