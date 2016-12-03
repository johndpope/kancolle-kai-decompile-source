using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_store
	{
		private Dictionary<int, Mst_payitem> mst_payitem;

		public Api_req_store()
		{
			this.mst_payitem = Mst_DataManager.Instance.GetPayitem();
		}

		public Dictionary<int, Mst_payitem> GetStoreList()
		{
			return Enumerable.ToDictionary<KeyValuePair<int, Mst_payitem>, int, Mst_payitem>(this.mst_payitem, (KeyValuePair<int, Mst_payitem> x) => x.get_Key(), (KeyValuePair<int, Mst_payitem> y) => y.get_Value());
		}

		public bool IsBuy(int mst_id, int buyNum)
		{
			if (buyNum == 0)
			{
				return false;
			}
			if (Comm_UserDatas.Instance.User_basic.Strategy_point < this.mst_payitem.get_Item(mst_id).Price * buyNum)
			{
				return false;
			}
			int buyNum2 = this.mst_payitem.get_Item(mst_id).GetBuyNum();
			return buyNum2 == -1 || (buyNum2 != 0 && buyNum2 >= buyNum);
		}

		public Api_Result<object> Buy(int mst_id, int buyNum)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			Mst_payitem mst_payitem = null;
			if (!this.mst_payitem.TryGetValue(mst_id, ref mst_payitem))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			using (List<PayItemEffectInfo>.Enumerator enumerator = mst_payitem.Items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					PayItemEffectInfo current = enumerator.get_Current();
					if (current.Type == 1)
					{
						Comm_UserDatas.Instance.Add_Useitem(current.MstId, current.Count);
					}
					else if (current.Type == 2)
					{
						IEnumerable<int> enumerable = Enumerable.Repeat<int>(current.MstId, current.Count);
						Comm_UserDatas.Instance.Add_Slot(Enumerable.ToList<int>(enumerable));
					}
					else if (current.Type == 3)
					{
						enumMaterialCategory mstId = (enumMaterialCategory)current.MstId;
						Comm_UserDatas.Instance.User_material.get_Item(mstId).Add_Material(current.Count);
					}
				}
			}
			Comm_UserDatas.Instance.User_basic.SubPoint(mst_payitem.Price * buyNum);
			return api_Result;
		}
	}
}
