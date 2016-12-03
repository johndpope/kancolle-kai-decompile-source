using Common.Enum;
using local.models;
using local.utils;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.managers
{
	public class FurnitureStoreManager : ManagerBase
	{
		private Dictionary<FurnitureKinds, List<FurnitureModel>> _StoreItems;

		private Dictionary<int, string> _desciptions;

		public FurnitureStoreManager()
		{
			this._desciptions = Mst_DataManager.Instance.GetFurnitureText();
			ILookup<int, Mst_furniture> lookup = Enumerable.ToLookup<Mst_furniture, int>(Mst_furniture.getSaleFurnitureList(), (Mst_furniture x) => x.Type);
			this._StoreItems = new Dictionary<FurnitureKinds, List<FurnitureModel>>();
			using (IEnumerator<IGrouping<int, Mst_furniture>> enumerator = lookup.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					IGrouping<int, Mst_furniture> current = enumerator.get_Current();
					FurnitureKinds key = (FurnitureKinds)current.get_Key();
					List<Mst_furniture> list = Enumerable.ToList<Mst_furniture>(current);
					List<FurnitureModel> list2 = list.ConvertAll<FurnitureModel>((Mst_furniture mst) => new __FStoreItemModel__(mst, this._desciptions.get_Item(mst.Id)));
					this._StoreItems.set_Item(key, list2.FindAll((FurnitureModel item) => !item.IsPossession()));
				}
			}
		}

		public int GetStoreItemCount(FurnitureKinds kind)
		{
			return this._StoreItems.get_Item(kind).get_Count();
		}

		public FurnitureModel[] GetStoreItem(FurnitureKinds kind)
		{
			return this._StoreItems.get_Item(kind).ToArray();
		}

		public int GetWorkerCount()
		{
			return new UseitemUtil().GetCount(52);
		}

		public bool IsValidExchange(FurnitureModel store_item)
		{
			return store_item.IsSalled() && !store_item.IsPossession() && base.UserInfo.FCoin >= store_item.Price && (!store_item.IsNeedWorker() || this.GetWorkerCount() >= 1);
		}

		public bool Exchange(FurnitureModel model)
		{
			Api_Result<object> api_Result = new Api_req_furniture().Buy(model.MstId);
			if (api_Result.state != Api_Result_State.Success)
			{
				return false;
			}
			this._StoreItems.set_Item(model.Type, this._StoreItems.get_Item(model.Type).FindAll((FurnitureModel item) => !item.IsPossession()));
			return true;
		}

		public override string ToString()
		{
			string text = base.ToString();
			text += string.Format("\n", new object[0]);
			return text + string.Format("\"家具職人\"所有数:{0} \"家具コイン\"所有数:{1}\n", this.GetWorkerCount(), base.UserInfo.FCoin);
		}
	}
}
