using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Controllers
{
	public class Api_req_Nyuukyo
	{
		public Api_Result<Mem_ndock> Start(int rid, int ship_rid, bool highspeed)
		{
			Api_Result<Mem_ndock> api_Result = new Api_Result<Mem_ndock>();
			Mem_ndock mem_ndock = null;
			if (!Comm_UserDatas.Instance.User_ndock.TryGetValue(rid, ref mem_ndock))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ndock.State != NdockStates.EMPTY)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ship.IsBlingShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, int> ndockMaterialNum = mem_ship.GetNdockMaterialNum();
			if (ndockMaterialNum == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int num = (!highspeed) ? 0 : 1;
			if (ndockMaterialNum.get_Item(enumMaterialCategory.Fuel) > Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Value || ndockMaterialNum.get_Item(enumMaterialCategory.Steel) > Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel).Value || num > Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Repair_Kit).Value)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			mem_ship.BlingWaitToStop();
			if (!highspeed)
			{
				int ndockTimeSpan = mem_ship.GetNdockTimeSpan();
				mem_ndock.RecoverStart(ship_rid, ndockMaterialNum, ndockTimeSpan);
			}
			else
			{
				mem_ndock.RecoverStart(ship_rid, ndockMaterialNum, 0);
				mem_ndock.RecoverEnd(false);
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Repair_Kit).Sub_Material(1);
			}
			api_Result.data = mem_ndock;
			QuestSupply questSupply = new QuestSupply();
			questSupply.ExecuteCheck();
			return api_Result;
		}

		public Api_Result<Mem_ndock> SpeedChange(int rid)
		{
			Api_Result<Mem_ndock> api_Result = new Api_Result<Mem_ndock>();
			Mem_ndock mem_ndock = null;
			if (!Comm_UserDatas.Instance.User_ndock.TryGetValue(rid, ref mem_ndock))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ndock.State != NdockStates.RESTORE)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Repair_Kit).Value < 1)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ndock.RecoverEnd(false))
			{
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Repair_Kit).Sub_Material(1);
				api_Result.data = mem_ndock;
				return api_Result;
			}
			api_Result.state = Api_Result_State.Parameter_Error;
			return api_Result;
		}
	}
}
