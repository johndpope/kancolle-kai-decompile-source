using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;

namespace Server_Controllers
{
	public class Api_req_Hokyu
	{
		public enum enumHokyuType
		{
			Fuel = 1,
			Bull,
			All
		}

		public int GetRequireUseBauxiteNum(Mem_ship ship, ref int haveBauxite, out List<int> afterOnslot)
		{
			afterOnslot = new List<int>(ship.Onslot);
			if (haveBauxite == 0)
			{
				return 0;
			}
			int num = haveBauxite;
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(ship.Ship_id);
			for (int i = 0; i < ship.Slotnum; i++)
			{
				if (haveBauxite == 0)
				{
					break;
				}
				if (afterOnslot.get_Item(i) < mst_ship.Maxeq.get_Item(i))
				{
					int num2 = ship.Slot.get_Item(i);
					int num3 = mst_ship.Maxeq.get_Item(i) - ship.Onslot.get_Item(i);
					int num4 = num3 * 5;
					if (haveBauxite >= num4)
					{
						afterOnslot.set_Item(i, afterOnslot.get_Item(i) + num3);
						haveBauxite -= num4;
					}
				}
			}
			return num - haveBauxite;
		}

		public Api_Result<bool> Charge(List<int> ship_rids, Api_req_Hokyu.enumHokyuType type)
		{
			Api_Result<bool> rslt = new Api_Result<bool>();
			rslt.data = false;
			if (ship_rids == null || ship_rids.get_Count() == 0)
			{
				rslt.state = Api_Result_State.Parameter_Error;
				return rslt;
			}
			List<Mem_ship> ships = new List<Mem_ship>();
			ship_rids.ForEach(delegate(int x)
			{
				Mem_ship mem_ship = null;
				if (!Comm_UserDatas.Instance.User_ship.TryGetValue(x, ref mem_ship))
				{
					rslt.state = Api_Result_State.Parameter_Error;
					return;
				}
				ships.Add(mem_ship);
			});
			if (rslt.state == Api_Result_State.Parameter_Error)
			{
				ships.Clear();
				return rslt;
			}
			HashSet<int> hashSet = new HashSet<int>();
			int num = 0;
			using (List<Mem_ship>.Enumerator enumerator = ships.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					if (Mst_DataManager.Instance.Mst_ship.ContainsKey(current.Ship_id))
					{
						int fuel = current.Fuel;
						int bull = current.Bull;
						if (type == Api_req_Hokyu.enumHokyuType.Fuel || type == Api_req_Hokyu.enumHokyuType.Bull)
						{
							bool flag = this.ChargeDataSet(type, current);
							if (bull < current.Bull || fuel < current.Fuel)
							{
								current.SumLovToCharge();
								num++;
							}
							HashSet<int> hashSet2 = this.ChargeDataSet_Onslot(Api_req_Hokyu.enumHokyuType.All, current);
							using (HashSet<int>.Enumerator enumerator2 = hashSet2.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									int current2 = enumerator2.get_Current();
									hashSet.Add(current2);
								}
							}
							if (!flag && hashSet2.Contains(-1))
							{
								break;
							}
						}
						else if (type == Api_req_Hokyu.enumHokyuType.All)
						{
							bool flag2 = this.ChargeDataSet(Api_req_Hokyu.enumHokyuType.Bull, current);
							bool flag3 = this.ChargeDataSet(Api_req_Hokyu.enumHokyuType.Fuel, current);
							HashSet<int> hashSet3 = this.ChargeDataSet_Onslot(Api_req_Hokyu.enumHokyuType.All, current);
							if (bull < current.Bull || fuel < current.Fuel)
							{
								current.SumLovToCharge();
								num++;
							}
							using (HashSet<int>.Enumerator enumerator3 = hashSet3.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									int current3 = enumerator3.get_Current();
									hashSet.Add(current3);
								}
							}
							if (!flag2 && !flag3 && hashSet3.Contains(-1))
							{
								break;
							}
						}
					}
				}
			}
			if (hashSet.Contains(-2))
			{
				rslt.data = false;
			}
			else
			{
				rslt.data = true;
			}
			QuestSupply questSupply = new QuestSupply(num);
			questSupply.ExecuteCheck();
			return rslt;
		}

		private bool ChargeDataSet(Api_req_Hokyu.enumHokyuType type, Mem_ship m_ship)
		{
			enumMaterialCategory enumMaterialCategory;
			int value;
			int num2;
			int fuel;
			int bull;
			if (type == Api_req_Hokyu.enumHokyuType.Fuel)
			{
				enumMaterialCategory = enumMaterialCategory.Fuel;
				int num = Mst_DataManager.Instance.Mst_ship.get_Item(m_ship.Ship_id).Fuel_max;
				value = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory).Value;
				num2 = m_ship.GetRequireChargeFuel();
				fuel = num;
				bull = m_ship.Bull;
			}
			else
			{
				if (type != Api_req_Hokyu.enumHokyuType.Bull)
				{
					return true;
				}
				enumMaterialCategory = enumMaterialCategory.Bull;
				int num = Mst_DataManager.Instance.Mst_ship.get_Item(m_ship.Ship_id).Bull_max;
				value = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory).Value;
				num2 = m_ship.GetRequireChargeBull();
				fuel = m_ship.Fuel;
				bull = num;
			}
			if (value <= 0)
			{
				return false;
			}
			if (num2 == 0)
			{
				return true;
			}
			if (num2 > value)
			{
				return true;
			}
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory).Sub_Material(num2);
			m_ship.Set_ChargeData(bull, fuel, null);
			return true;
		}

		private HashSet<int> ChargeDataSet_Onslot(Api_req_Hokyu.enumHokyuType type, Mem_ship m_ship)
		{
			enumMaterialCategory enumMaterialCategory = enumMaterialCategory.Bauxite;
			int value = Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory).Value;
			HashSet<int> hashSet = new HashSet<int>();
			List<int> onslot;
			if (value == 0)
			{
				int num = 100;
				int requireUseBauxiteNum = this.GetRequireUseBauxiteNum(m_ship, ref num, out onslot);
				if (requireUseBauxiteNum > 0)
				{
					hashSet.Add(-2);
				}
				hashSet.Add(-1);
				return hashSet;
			}
			int requireUseBauxiteNum2 = this.GetRequireUseBauxiteNum(m_ship, ref value, out onslot);
			m_ship.Set_ChargeData(m_ship.Bull, m_ship.Fuel, onslot);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory).Sub_Material(requireUseBauxiteNum2);
			List<int> maxeq = Mst_DataManager.Instance.Mst_ship.get_Item(m_ship.Ship_id).Maxeq;
			for (int i = 0; i < m_ship.Slotnum; i++)
			{
				if (maxeq.get_Item(i) > 0)
				{
					if (maxeq.get_Item(i) != m_ship.Onslot.get_Item(i))
					{
						hashSet.Add(-2);
					}
				}
			}
			return hashSet;
		}
	}
}
