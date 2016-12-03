using Common.Enum;
using local.models;
using Server_Common;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class InteriorManager : ManagerBase
	{
		private int _deck_id;

		private Dictionary<FurnitureKinds, List<FurnitureModel>> _dict;

		private Dictionary<FurnitureKinds, FurnitureModel> _room_cache;

		public DeckModel Deck
		{
			get
			{
				return base.UserInfo.GetDeck(this._deck_id);
			}
		}

		public InteriorManager(int deck_id)
		{
			this._deck_id = deck_id;
			Dictionary<int, string> desciptions = Mst_DataManager.Instance.GetFurnitureText();
			this._dict = new Dictionary<FurnitureKinds, List<FurnitureModel>>();
			Api_Result<Dictionary<FurnitureKinds, List<Mst_furniture>>> api_Result = new Api_get_Member().FurnitureList();
			if (api_Result.state == Api_Result_State.Success && api_Result.data != null)
			{
				using (Dictionary<FurnitureKinds, List<Mst_furniture>>.Enumerator enumerator = api_Result.data.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<FurnitureKinds, List<Mst_furniture>> current = enumerator.get_Current();
						List<FurnitureModel> list = current.get_Value().ConvertAll<FurnitureModel>((Mst_furniture f) => new FurnitureModel(f, desciptions.get_Item(f.Id)));
						this._dict.Add(current.get_Key(), list);
					}
				}
			}
		}

		public FurnitureModel[] GetFurnitures(FurnitureKinds kind)
		{
			return this._dict.get_Item(kind).ToArray();
		}

		public FurnitureModel GetFurniture(FurnitureKinds kind, int furniture_mst_id)
		{
			return this._dict.get_Item(kind).Find((FurnitureModel f) => f.MstId == furniture_mst_id);
		}

		public Dictionary<FurnitureKinds, FurnitureModel> GetRoomInfo()
		{
			Mem_room mem_room;
			if (this._room_cache == null && Comm_UserDatas.Instance.User_room.TryGetValue(this._deck_id, ref mem_room))
			{
				this._room_cache = new Dictionary<FurnitureKinds, FurnitureModel>();
				using (Dictionary<FurnitureKinds, List<FurnitureModel>>.Enumerator enumerator = this._dict.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<FurnitureKinds, List<FurnitureModel>> current = enumerator.get_Current();
						int room_furniture = -1;
						switch (current.get_Key())
						{
						case FurnitureKinds.Floor:
							room_furniture = mem_room.Furniture1;
							break;
						case FurnitureKinds.Wall:
							room_furniture = mem_room.Furniture2;
							break;
						case FurnitureKinds.Window:
							room_furniture = mem_room.Furniture3;
							break;
						case FurnitureKinds.Hangings:
							room_furniture = mem_room.Furniture4;
							break;
						case FurnitureKinds.Chest:
							room_furniture = mem_room.Furniture5;
							break;
						case FurnitureKinds.Desk:
							room_furniture = mem_room.Furniture6;
							break;
						}
						this._room_cache.set_Item(current.get_Key(), current.get_Value().Find((FurnitureModel f) => f.MstId == room_furniture));
					}
				}
			}
			return this._room_cache;
		}

		public bool ChangeRoom(FurnitureKinds fType, int furniture_mst_id)
		{
			Api_Result<int> api_Result = new Api_req_furniture().Change(this._deck_id, fType, furniture_mst_id);
			if (api_Result.state == Api_Result_State.Success)
			{
				this._room_cache = null;
				return true;
			}
			return false;
		}
	}
}
