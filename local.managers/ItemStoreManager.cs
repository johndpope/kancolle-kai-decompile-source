using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.managers
{
	public class ItemStoreManager : ManagerBase
	{
		public const int CABINET_NO = 1;

		private Api_req_store _req_store;

		private Dictionary<int, List<Mst_item_shop>> _mst_cabinets;

		private List<ItemStoreModel> _items;

		public List<ItemStoreModel> Items
		{
			get
			{
				return this._items;
			}
		}

		public ItemStoreManager()
		{
		}

		public ItemStoreManager(Dictionary<int, List<Mst_item_shop>> mst_cabinets)
		{
			this._mst_cabinets = mst_cabinets;
		}

		public virtual void Init()
		{
			this._Init(false);
		}

		public bool IsValidBuy(int paymentitem_mst_id, int count)
		{
			ItemStoreModel item = this.Items.Find((ItemStoreModel i) => i != null && i.MstId == paymentitem_mst_id);
			return this.IsValidBuy(item, count);
		}

		public bool IsValidBuy(ItemStoreModel item, int count)
		{
			return item != null && item.Count != 0 && count > 0 && (item.Count <= 0 || count <= item.Count) && item.Price * count <= base.UserInfo.SPoint && this._req_store.IsBuy(item.MstId, count);
		}

		public bool BuyItem(int paymentitem_mst_id, int count)
		{
			ItemStoreModel itemStoreModel = this.Items.Find((ItemStoreModel i) => i != null && i.MstId == paymentitem_mst_id);
			if (!this.IsValidBuy(itemStoreModel, count))
			{
				return false;
			}
			Api_Result<object> api_Result = this._req_store.Buy(itemStoreModel.MstId, count);
			return api_Result.state == Api_Result_State.Success;
		}

		protected void _Init(bool all_item)
		{
			if (this._req_store == null)
			{
				this._req_store = new Api_req_store();
			}
			Dictionary<int, Mst_payitem> storeList = this._req_store.GetStoreList();
			this._items = new List<ItemStoreModel>();
			if (this._mst_cabinets == null)
			{
				this._mst_cabinets = Mst_DataManager.Instance.GetMstCabinet();
			}
			List<Mst_item_shop> list = this._mst_cabinets.get_Item(1);
			list = list.GetRange(0, list.get_Count());
			if (all_item)
			{
				using (Dictionary<int, Mst_payitem>.ValueCollection.Enumerator enumerator = storeList.get_Values().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mst_payitem current = enumerator.get_Current();
						if (!(current.Name == string.Empty))
						{
							ItemStoreModel itemStoreModel = new ItemStoreModel(current);
							this._items.Add(itemStoreModel);
						}
					}
				}
				this._items.Sort((ItemStoreModel a, ItemStoreModel b) => (a.MstId <= b.MstId) ? -1 : 1);
			}
			else
			{
				using (List<Mst_item_shop>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						Mst_item_shop current2 = enumerator2.get_Current();
						Mst_payitem mst_payitem;
						storeList.TryGetValue(current2.Item1_id, ref mst_payitem);
						if (mst_payitem == null)
						{
							this._items.Add(null);
						}
						else
						{
							ItemStoreModel itemStoreModel2 = new ItemStoreModel(mst_payitem);
							this._items.Add(itemStoreModel2);
						}
					}
				}
			}
		}

		public ItemlistManager CreateListManager()
		{
			return new ItemlistManager(this._mst_cabinets);
		}

		public override string ToString()
		{
			string text = base.ToString();
			text += "\n";
			text += "-- アイテム屋商品 --\n";
			for (int i = 0; i < this.Items.get_Count(); i++)
			{
				text += string.Format("[{0}] {1}\n", i, this.Items.get_Item(i));
			}
			return text + "\n";
		}
	}
}
