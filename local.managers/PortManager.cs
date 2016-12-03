using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class PortManager : TurnManager
	{
		private int _area_id;

		private int _yubiwa_num;

		public MapAreaModel MapArea
		{
			get
			{
				return ManagerBase._area.get_Item(this._area_id);
			}
		}

		public int YubiwaNum
		{
			get
			{
				return this._yubiwa_num;
			}
		}

		public PortManager(int area_id)
		{
			this._area_id = area_id;
			ManagerBase._userInfoModel.__UpdateDeck__(new Api_get_Member());
			base._CreateMapAreaModel();
			this._yubiwa_num = new UseitemUtil().GetCount(55);
		}

		public Dictionary<FurnitureKinds, FurnitureModel> GetFurnitures(int deck_id)
		{
			Api_Result<List<Mst_furniture>> api_Result = new Api_get_Member().DecorateFurniture(deck_id);
			if (api_Result.state != Api_Result_State.Success)
			{
				return null;
			}
			Dictionary<FurnitureKinds, FurnitureModel> dictionary = new Dictionary<FurnitureKinds, FurnitureModel>();
			for (int i = 0; i < api_Result.data.get_Count(); i++)
			{
				FurnitureModel furnitureModel = new FurnitureModel(api_Result.data.get_Item(i), string.Empty);
				dictionary.Add(furnitureModel.Type, furnitureModel);
			}
			return dictionary;
		}

		public List<Mst_bgm_jukebox> GetJukeboxList()
		{
			return Mst_DataManager.Instance.GetJukeBoxList();
		}

		public bool PlayJukeboxBGM(int deck_id, int bgm_id)
		{
			Api_Result<bool> api_Result = new Api_req_furniture().BuyMusic(deck_id, bgm_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool SetPortBGM(int deck_id, int bgm_id)
		{
			Api_Result<bool> api_Result = new Api_req_furniture().SetPortBGMFromJukeBoxList(deck_id, bgm_id);
			return api_Result.state == Api_Result_State.Success;
		}

		public bool IsValidMarriage(int ship_mem_id)
		{
			return new Api_req_Kaisou().ValidMarriage(ship_mem_id);
		}

		public bool Marriage(int ship_mem_id)
		{
			Api_Result<Mem_ship> api_Result = new Api_req_Kaisou().Marriage(ship_mem_id);
			return api_Result.state == Api_Result_State.Success && api_Result.data != null;
		}

		public override string ToString()
		{
			string text = string.Format("[--PortManager--]\n", new object[0]);
			text += string.Format("{0}\n", base.ToString());
			text += string.Format("海域ID:{0}\n", this.MapArea.Id);
			text += string.Format("指輪の数(ケッコンカッコカリ):{0}\n", this.YubiwaNum);
			DeckModel[] decksFromArea = base.UserInfo.GetDecksFromArea(this.MapArea.Id);
			for (int i = 0; i < decksFromArea.Length; i++)
			{
				DeckModel deckModel = decksFromArea[i];
				text += string.Format("== 艦隊:{0} (母港BGMID:{1})==\n", deckModel, base.UserInfo.GetPortBGMId(deckModel.Id));
				Dictionary<FurnitureKinds, FurnitureModel> furnitures = this.GetFurnitures(deckModel.Id);
				text += string.Format("[設定家具]\n", new object[0]);
				text += string.Format("床:{0}\t壁紙:{1}\t窓:{2}\n", furnitures.get_Item(FurnitureKinds.Floor), furnitures.get_Item(FurnitureKinds.Wall), furnitures.get_Item(FurnitureKinds.Window));
				text += string.Format("壁掛け:{0}\t棚:{1}\t机:{2}\n", furnitures.get_Item(FurnitureKinds.Hangings), furnitures.get_Item(FurnitureKinds.Chest), furnitures.get_Item(FurnitureKinds.Desk));
			}
			List<Mst_bgm_jukebox> jukeboxList = this.GetJukeboxList();
			text += string.Format("[JUKE]\n", new object[0]);
			for (int j = 0; j < jukeboxList.get_Count(); j++)
			{
				Mst_bgm_jukebox mst_bgm_jukebox = jukeboxList.get_Item(j);
				text += string.Format("JUKE{0} {1} 家具コイン:{2} ループ:{3} BGM設定:{4} {5}\n", new object[]
				{
					mst_bgm_jukebox.Bgm_id,
					mst_bgm_jukebox.Name,
					mst_bgm_jukebox.R_coins,
					mst_bgm_jukebox.Loops,
					(mst_bgm_jukebox.Bgm_flag != 1) ? "不可" : "可能",
					mst_bgm_jukebox.Remarks
				});
			}
			return text + string.Format("[--PortManager--]\n", new object[0]);
		}
	}
}
