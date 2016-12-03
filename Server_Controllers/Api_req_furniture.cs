using Common.Enum;
using Server_Common;
using Server_Models;
using System;

namespace Server_Controllers
{
	public class Api_req_furniture
	{
		public Api_Result<int> Change(int deck_rid, FurnitureKinds furnitureKind, int furnitureId)
		{
			Api_Result<int> api_Result = new Api_Result<int>();
			Mem_room mem_room = null;
			if (!Comm_UserDatas.Instance.User_room.TryGetValue(deck_rid, ref mem_room))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int season = Mst_DataManager.Instance.Mst_furniture.get_Item(furnitureId).Season;
			int num = 0;
			Mst_DataManager.Instance.Mst_bgm_season.TryGetValue(season, ref num);
			if (num == 0)
			{
				int num2 = mem_room[furnitureKind];
				int season2 = Mst_DataManager.Instance.Mst_furniture.get_Item(num2).Season;
				int num3 = 0;
				Mst_DataManager.Instance.Mst_bgm_season.TryGetValue(season2, ref num3);
				if (num3 == mem_room.Bgm_id)
				{
					mem_room.SetFurniture(furnitureKind, furnitureId, num);
				}
				else
				{
					mem_room.SetFurniture(furnitureKind, furnitureId);
				}
			}
			else
			{
				mem_room.SetFurniture(furnitureKind, furnitureId, num);
			}
			api_Result.data = 1;
			return api_Result;
		}

		public Api_Result<object> Buy(int mst_id)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			Mst_furniture mst_furniture = null;
			if (!Mst_DataManager.Instance.Mst_furniture.TryGetValue(mst_id, ref mst_furniture))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (Comm_UserDatas.Instance.User_furniture.ContainsKey(mst_id))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_basic user_basic = Comm_UserDatas.Instance.User_basic;
			if (mst_furniture.Saleflg == 0 || user_basic.Fcoin < mst_furniture.Price)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_useitem mem_useitem = null;
			if (mst_furniture.IsRequireWorker())
			{
				if (!Comm_UserDatas.Instance.User_useItem.TryGetValue(52, ref mem_useitem))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
				if (mem_useitem.Value <= 0)
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			if (!Comm_UserDatas.Instance.Add_Furniture(mst_id))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			user_basic.SubCoin(mst_furniture.Price);
			if (mem_useitem != null)
			{
				mem_useitem.Sub_UseItem(1);
			}
			return api_Result;
		}

		public Api_Result<bool> BuyMusic(int deck_rid, int music_id)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Mst_bgm_jukebox jukeBoxItem = Mst_DataManager.Instance.GetJukeBoxItem(music_id);
			if (jukeBoxItem == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (Comm_UserDatas.Instance.User_basic.Fcoin < jukeBoxItem.R_coins)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Comm_UserDatas.Instance.User_basic.SubCoin(jukeBoxItem.R_coins);
			api_Result.state = Api_Result_State.Success;
			api_Result.data = true;
			return api_Result;
		}

		public Api_Result<bool> SetPortBGMFromJukeBoxList(int deck_rid, int music_id)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Mst_bgm_jukebox jukeBoxItem = Mst_DataManager.Instance.GetJukeBoxItem(music_id);
			if (jukeBoxItem == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (jukeBoxItem.Bgm_flag == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = true;
			Mem_room mem_room = null;
			if (!Comm_UserDatas.Instance.User_room.TryGetValue(deck_rid, ref mem_room))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			mem_room.SetPortBgmFromJuke(music_id);
			api_Result.state = Api_Result_State.Success;
			api_Result.data = true;
			return api_Result;
		}
	}
}
