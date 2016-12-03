using Common.Enum;
using Server_Common;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers
{
	public class Api_req_Kousyou : Mem_slotitem.IMemSlotIdOperator
	{
		public readonly int Stratege_Min;

		public readonly int Stratege_Max;

		private Dictionary<int, Dictionary<enumMaterialCategory, int>> mat_min;

		private Dictionary<int, Dictionary<enumMaterialCategory, int>> mat_max;

		private IEnumerable<XElement> createTable;

		private IEnumerable<XElement> createLargeTable;

		private IEnumerable<XElement> createTable_change;

		private IEnumerable<XElement> createLargeTable_change;

		private IEnumerable<XElement> createItemTable;

		private IEnumerable<XElement> createItemGroup;

		private Dictionary<int, List<Mst_slotitem_remodel>> mst_remodel;

		private Dictionary<int, Mst_slotitem_remodel> mst_remodel_list;

		private Dictionary<int, List<Mst_slotitem_remodel_detail>> mst_remodel_detail;

		private readonly int[] requireHighSpeedNum;

		private bool questDestroyItemDisable;

		public Api_req_Kousyou()
		{
			this.requireHighSpeedNum = new int[]
			{
				1,
				10,
				1
			};
			this.makeRequireMaterialNum();
			this.Stratege_Min = 50;
			this.Stratege_Max = 400;
			this.mst_remodel = null;
			this.mst_remodel_list = null;
			this.mst_remodel_detail = null;
		}

		public Dictionary<enumMaterialCategory, int> GetRequireMaterials_Min(int type)
		{
			return this.mat_min.get_Item(type);
		}

		public Dictionary<enumMaterialCategory, int> GetRequireMaterials_Max(int type)
		{
			return this.mat_max.get_Item(type);
		}

		public int GetRequireHighSpeedNum(int type)
		{
			return this.requireHighSpeedNum[type];
		}

		public bool ValidStart(int rid, bool highSpeed, bool largeDock, ref Dictionary<enumMaterialCategory, int> materials, int deck_rid)
		{
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				return false;
			}
			if (mem_kdock.State != KdockStates.EMPTY)
			{
				return false;
			}
			if (Comm_UserDatas.Instance.User_basic.IsMaxChara() || Comm_UserDatas.Instance.User_basic.IsMaxSlotitem())
			{
				return false;
			}
			if (largeDock && Comm_UserDatas.Instance.User_basic.Large_dock <= 0)
			{
				return false;
			}
			if (!Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Fuel) || !Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Bull) || !Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Steel) || !Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Bauxite) || !Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Dev_Kit))
			{
				return false;
			}
			int num = 0;
			int num2 = largeDock ? 1 : 0;
			if (highSpeed)
			{
				num = this.mat_max.get_Item(num2).get_Item(enumMaterialCategory.Build_Kit);
			}
			materials.Add(enumMaterialCategory.Build_Kit, num);
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator = materials.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current = enumerator.get_Current();
					if (current.get_Value() > Comm_UserDatas.Instance.User_material.get_Item(current.get_Key()).Value)
					{
						bool result = false;
						return result;
					}
					if (current.get_Value() < this.mat_min.get_Item(num2).get_Item(current.get_Key()))
					{
						bool result = false;
						return result;
					}
					if (current.get_Value() > this.mat_max.get_Item(num2).get_Item(current.get_Key()))
					{
						bool result = false;
						return result;
					}
				}
			}
			Mem_deck mem_deck = null;
			return Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck) && Comm_UserDatas.Instance.User_ship.ContainsKey(mem_deck.Ship[0]);
		}

		public bool ValidStartTanker(int rid, bool highSpeed, ref Dictionary<enumMaterialCategory, int> materials, int stratege_point)
		{
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				return false;
			}
			if (mem_kdock.State != KdockStates.EMPTY)
			{
				return false;
			}
			if (!Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Fuel) || !Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Bull) || !Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Steel) || !Enumerable.Contains<enumMaterialCategory>(materials.get_Keys(), enumMaterialCategory.Bauxite))
			{
				return false;
			}
			Dictionary<enumMaterialCategory, int> dictionary = this.mat_min.get_Item(2);
			Dictionary<enumMaterialCategory, int> dictionary2 = this.mat_max.get_Item(2);
			int num = 0;
			if (highSpeed)
			{
				num = dictionary2.get_Item(enumMaterialCategory.Build_Kit);
			}
			materials.Add(enumMaterialCategory.Build_Kit, num);
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator = materials.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current = enumerator.get_Current();
					if (current.get_Value() > Comm_UserDatas.Instance.User_material.get_Item(current.get_Key()).Value)
					{
						bool result = false;
						return result;
					}
					if (current.get_Value() < dictionary.get_Item(current.get_Key()))
					{
						bool result = false;
						return result;
					}
					if (current.get_Value() > dictionary2.get_Item(current.get_Key()))
					{
						bool result = false;
						return result;
					}
				}
			}
			return stratege_point <= Comm_UserDatas.Instance.User_basic.Strategy_point && stratege_point >= this.Stratege_Min && stratege_point <= this.Stratege_Max;
		}

		public bool ValidSpeedChange(int rid)
		{
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				return false;
			}
			if (mem_kdock.State != KdockStates.CREATE)
			{
				return false;
			}
			int num;
			if (mem_kdock.IsLargeDock())
			{
				num = this.requireHighSpeedNum[1];
			}
			else if (mem_kdock.IsTunkerDock())
			{
				num = this.requireHighSpeedNum[2];
			}
			else
			{
				num = this.requireHighSpeedNum[0];
			}
			return Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Build_Kit).Value >= num;
		}

		public bool ValidSpeedChangeTanker(int rid)
		{
			return this.ValidSpeedChange(rid);
		}

		public bool ValidGetShip(int rid)
		{
			Mem_kdock mem_kdock = null;
			return Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock) && mem_kdock.State == KdockStates.COMPLETE && !Comm_UserDatas.Instance.User_basic.IsMaxChara() && !Comm_UserDatas.Instance.User_basic.IsMaxSlotitem();
		}

		public bool ValidGetTanker(int rid)
		{
			Mem_kdock mem_kdock = null;
			return Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock) && mem_kdock.State == KdockStates.COMPLETE;
		}

		public Api_Result<Mem_kdock> Start(int rid, bool highSpeed, bool largeDock, Dictionary<enumMaterialCategory, int> materials, int deck_rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int create_ship = 0;
			Func<Dictionary<enumMaterialCategory, int>, int, int> func;
			if (largeDock)
			{
				func = new Func<Dictionary<enumMaterialCategory, int>, int, int>(this.getShipIdLarge);
			}
			else
			{
				func = new Func<Dictionary<enumMaterialCategory, int>, int, int>(this.getShipId);
			}
			create_ship = func.Invoke(materials, deck_rid);
			if (Enumerable.Any<Mem_ship>(Comm_UserDatas.Instance.User_ship.get_Values(), (Mem_ship x) => x.Ship_id == create_ship))
			{
				create_ship = func.Invoke(materials, deck_rid);
			}
			Mst_ship mst_ship = null;
			if (!Mst_DataManager.Instance.Mst_ship.TryGetValue(create_ship, ref mst_ship))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			TimeSpan span = new TimeSpan(0, mst_ship.Buildtime, 0);
			mem_kdock.CreateStart(create_ship, materials, span);
			if (materials.get_Item(enumMaterialCategory.Build_Kit) != 0)
			{
				mem_kdock.CreateEnd(false);
			}
			api_Result.data = mem_kdock;
			QuestKousyou questKousyou = new QuestKousyou(materials, mst_ship.Id);
			questKousyou.ExecuteCheck();
			return api_Result;
		}

		public Api_Result<Mem_kdock> StartTanker(int rid, bool highSpeed, Dictionary<enumMaterialCategory, int> materials, int strategy_point)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int createTurn = 0;
			int createTankerNum = this.getCreateTankerNum(materials, strategy_point, ref createTurn);
			mem_kdock.CreateTunker(createTankerNum, materials, strategy_point, createTurn);
			if (materials.get_Item(enumMaterialCategory.Build_Kit) != 0)
			{
				mem_kdock.CreateEnd(false);
			}
			api_Result.data = mem_kdock;
			return api_Result;
		}

		public Api_Result<Mem_kdock> SpeedChange(int rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			if (!this.ValidSpeedChange(rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			int num;
			if (mem_kdock.IsLargeDock())
			{
				num = this.requireHighSpeedNum[1];
			}
			else if (mem_kdock.IsTunkerDock())
			{
				num = this.requireHighSpeedNum[2];
			}
			else
			{
				num = this.requireHighSpeedNum[0];
			}
			if (mem_kdock.CreateEnd(false))
			{
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Build_Kit).Sub_Material(num);
				api_Result.data = mem_kdock;
				return api_Result;
			}
			api_Result.state = Api_Result_State.Parameter_Error;
			return api_Result;
		}

		public Api_Result<Mem_kdock> SpeedChangeTanker(int rid)
		{
			return this.SpeedChange(rid);
		}

		public Api_Result<Mem_kdock> GetShip(int rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!this.ValidGetShip(rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!mem_kdock.GetShip())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = mem_kdock;
			return api_Result;
		}

		public Api_Result<Mem_kdock> GetTanker(int rid)
		{
			Api_Result<Mem_kdock> api_Result = new Api_Result<Mem_kdock>();
			Mem_kdock mem_kdock = null;
			if (!Comm_UserDatas.Instance.User_kdock.TryGetValue(rid, ref mem_kdock))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!this.ValidGetTanker(rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (!mem_kdock.GetTunker())
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = mem_kdock;
			return api_Result;
		}

		private void makeRequireMaterialNum()
		{
			this.mat_min = new Dictionary<int, Dictionary<enumMaterialCategory, int>>();
			this.mat_max = new Dictionary<int, Dictionary<enumMaterialCategory, int>>();
			Dictionary<int, Dictionary<enumMaterialCategory, int>> arg_58_0 = this.mat_min;
			int arg_58_1 = 0;
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 30);
			dictionary.Add(enumMaterialCategory.Bull, 30);
			dictionary.Add(enumMaterialCategory.Steel, 30);
			dictionary.Add(enumMaterialCategory.Bauxite, 30);
			dictionary.Add(enumMaterialCategory.Dev_Kit, 1);
			dictionary.Add(enumMaterialCategory.Build_Kit, 0);
			arg_58_0.Add(arg_58_1, dictionary);
			Dictionary<int, Dictionary<enumMaterialCategory, int>> arg_B2_0 = this.mat_max;
			int arg_B2_1 = 0;
			dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 999);
			dictionary.Add(enumMaterialCategory.Bull, 999);
			dictionary.Add(enumMaterialCategory.Steel, 999);
			dictionary.Add(enumMaterialCategory.Bauxite, 999);
			dictionary.Add(enumMaterialCategory.Dev_Kit, 1);
			dictionary.Add(enumMaterialCategory.Build_Kit, this.requireHighSpeedNum[0]);
			arg_B2_0.Add(arg_B2_1, dictionary);
			Dictionary<int, Dictionary<enumMaterialCategory, int>> arg_105_0 = this.mat_min;
			int arg_105_1 = 1;
			dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 1500);
			dictionary.Add(enumMaterialCategory.Bull, 1500);
			dictionary.Add(enumMaterialCategory.Steel, 2000);
			dictionary.Add(enumMaterialCategory.Bauxite, 1000);
			dictionary.Add(enumMaterialCategory.Dev_Kit, 1);
			dictionary.Add(enumMaterialCategory.Build_Kit, 0);
			arg_105_0.Add(arg_105_1, dictionary);
			Dictionary<int, Dictionary<enumMaterialCategory, int>> arg_160_0 = this.mat_max;
			int arg_160_1 = 1;
			dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 7000);
			dictionary.Add(enumMaterialCategory.Bull, 7000);
			dictionary.Add(enumMaterialCategory.Steel, 7000);
			dictionary.Add(enumMaterialCategory.Bauxite, 7000);
			dictionary.Add(enumMaterialCategory.Dev_Kit, 100);
			dictionary.Add(enumMaterialCategory.Build_Kit, this.requireHighSpeedNum[1]);
			arg_160_0.Add(arg_160_1, dictionary);
			Dictionary<int, Dictionary<enumMaterialCategory, int>> arg_19F_0 = this.mat_min;
			int arg_19F_1 = 2;
			dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 40);
			dictionary.Add(enumMaterialCategory.Bull, 10);
			dictionary.Add(enumMaterialCategory.Steel, 40);
			dictionary.Add(enumMaterialCategory.Bauxite, 10);
			dictionary.Add(enumMaterialCategory.Build_Kit, 0);
			arg_19F_0.Add(arg_19F_1, dictionary);
			Dictionary<int, Dictionary<enumMaterialCategory, int>> arg_1F1_0 = this.mat_max;
			int arg_1F1_1 = 2;
			dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 999);
			dictionary.Add(enumMaterialCategory.Bull, 999);
			dictionary.Add(enumMaterialCategory.Steel, 999);
			dictionary.Add(enumMaterialCategory.Bauxite, 999);
			dictionary.Add(enumMaterialCategory.Build_Kit, this.requireHighSpeedNum[2]);
			arg_1F1_0.Add(arg_1F1_1, dictionary);
		}

		private int getShipId(Dictionary<enumMaterialCategory, int> material, int deck_rid)
		{
			int group_id = 0;
			if (material.get_Item(enumMaterialCategory.Steel) >= 400 && material.get_Item(enumMaterialCategory.Fuel) >= 300 && material.get_Item(enumMaterialCategory.Bauxite) >= 300)
			{
				group_id = 1;
			}
			else if (material.get_Item(enumMaterialCategory.Steel) >= 600 && material.get_Item(enumMaterialCategory.Fuel) >= 400 && material.get_Item(enumMaterialCategory.Bauxite) >= 0)
			{
				group_id = 2;
			}
			else if (material.get_Item(enumMaterialCategory.Steel) >= 200 && material.get_Item(enumMaterialCategory.Fuel) >= 250 && material.get_Item(enumMaterialCategory.Bauxite) >= 0)
			{
				group_id = 3;
			}
			else
			{
				if (material.get_Item(enumMaterialCategory.Steel) < 30 || material.get_Item(enumMaterialCategory.Fuel) < 30 || material.get_Item(enumMaterialCategory.Bauxite) < 0)
				{
					return 0;
				}
				group_id = 4;
			}
			Random random = new Random();
			int num = random.Next(100) + 1;
			int num2 = 0;
			double num3 = (double)material.get_Item(enumMaterialCategory.Bauxite);
			double num4 = (double)material.get_Item(enumMaterialCategory.Steel);
			double num5 = (double)material.get_Item(enumMaterialCategory.Fuel);
			double num6 = (double)material.get_Item(enumMaterialCategory.Bull);
			if (group_id == 1)
			{
				num2 = (int)(Math.Ceiling((num3 - 300.0) / 20.0) + Math.Ceiling((num4 - 400.0) / 25.0));
			}
			else if (group_id == 2)
			{
				num2 = (int)(Math.Ceiling((num6 - 400.0) / 25.0) + Math.Ceiling((num4 - 600.0) / 30.0));
			}
			else if (group_id == 3)
			{
				num2 = (int)(Math.Ceiling((num6 - 200.0) / 13.0) + Math.Ceiling((num4 - 200.0) / 20.0));
			}
			else if (group_id == 4)
			{
				num2 = (int)(Math.Ceiling((num5 - 100.0) / 10.0) + Math.Ceiling((num4 - 30.0) / 15.0));
			}
			num2 = Math.Abs(num2);
			if (num2 > 51)
			{
				num2 = 51;
			}
			num2 = random.Next(num2);
			int num7 = num - num2;
			if (num7 < 1)
			{
				num7 = 2 - num7;
			}
			if (this.createTable == null)
			{
				this.createTable = Utils.Xml_Result("mst_createship", "mst_createship", "Id");
			}
			var <>__AnonType = Enumerable.First(Enumerable.Skip(Enumerable.Select(Enumerable.Where<XElement>(this.createTable, (XElement data) => data.Element("Group_id").get_Value() == group_id.ToString()), (XElement data) => new
			{
				ship_id = int.Parse(data.Element("Ship_id").get_Value()),
				change_flag = int.Parse(data.Element("Change_flag").get_Value())
			}), num7));
			int result = <>__AnonType.ship_id;
			if (<>__AnonType.change_flag == 1)
			{
				int changeCreateShipId = this.getChangeCreateShipId(false, <>__AnonType.ship_id, deck_rid);
				if (changeCreateShipId != -1)
				{
					result = changeCreateShipId;
				}
			}
			return result;
		}

		private int getShipIdLarge(Dictionary<enumMaterialCategory, int> material, int deck_rid)
		{
			Api_req_Kousyou.<getShipIdLarge>c__AnonStorey4A3 <getShipIdLarge>c__AnonStorey4A = new Api_req_Kousyou.<getShipIdLarge>c__AnonStorey4A3();
			Random random = new Random();
			enumMaterialCategory enumMaterialCategory = enumMaterialCategory.Steel;
			enumMaterialCategory enumMaterialCategory2 = enumMaterialCategory.Fuel;
			enumMaterialCategory enumMaterialCategory3 = enumMaterialCategory.Bull;
			enumMaterialCategory enumMaterialCategory4 = enumMaterialCategory.Bauxite;
			enumMaterialCategory enumMaterialCategory5 = enumMaterialCategory.Dev_Kit;
			<getShipIdLarge>c__AnonStorey4A.group_id = 0;
			if (material.get_Item(enumMaterialCategory) >= 2800 + random.Next(1400) && material.get_Item(enumMaterialCategory2) >= 2400 + random.Next(1200) && material.get_Item(enumMaterialCategory3) >= 1050 + random.Next(900) && material.get_Item(enumMaterialCategory4) >= 2800 + random.Next(2400) && material.get_Item(enumMaterialCategory5) >= 1 + random.Next(0))
			{
				<getShipIdLarge>c__AnonStorey4A.group_id = 1;
			}
			else if (material.get_Item(enumMaterialCategory) >= 4400 + random.Next(2200) && material.get_Item(enumMaterialCategory2) >= 2240 + random.Next(1120) && material.get_Item(enumMaterialCategory3) >= 2940 + random.Next(2520) && material.get_Item(enumMaterialCategory4) >= 1050 + random.Next(900) && material.get_Item(enumMaterialCategory5) >= 20 + random.Next(0))
			{
				<getShipIdLarge>c__AnonStorey4A.group_id = 2;
			}
			else if (material.get_Item(enumMaterialCategory) >= 3040 + random.Next(1520) && material.get_Item(enumMaterialCategory2) >= 1920 + random.Next(960) && material.get_Item(enumMaterialCategory3) >= 2240 + random.Next(1920) && material.get_Item(enumMaterialCategory4) >= 910 + random.Next(780) && material.get_Item(enumMaterialCategory5) >= 1 + random.Next(0))
			{
				<getShipIdLarge>c__AnonStorey4A.group_id = 3;
			}
			else
			{
				<getShipIdLarge>c__AnonStorey4A.group_id = 4;
			}
			Api_req_Kousyou.<getShipIdLarge>c__AnonStorey4A3 arg_20F_0 = <getShipIdLarge>c__AnonStorey4A;
			HashSet<KdockStates> hashSet = new HashSet<KdockStates>();
			hashSet.Add(KdockStates.EMPTY);
			hashSet.Add(KdockStates.COMPLETE);
			arg_20F_0.enableState = hashSet;
			int num = Enumerable.Count<Mem_kdock>(Comm_UserDatas.Instance.User_kdock.get_Values(), (Mem_kdock dockItem) => <getShipIdLarge>c__AnonStorey4A.enableState.Contains(dockItem.State));
			int num2 = num - 1;
			Dictionary<int, int[]> dictionary = new Dictionary<int, int[]>();
			dictionary.Add(0, new int[]
			{
				3,
				100
			});
			dictionary.Add(1, new int[]
			{
				1,
				100
			});
			dictionary.Add(2, new int[]
			{
				1,
				96
			});
			dictionary.Add(3, new int[]
			{
				1,
				92
			});
			Dictionary<int, int[]> dictionary2 = dictionary;
			int num3 = random.Next(dictionary2.get_Item(num2)[0], dictionary2.get_Item(num2)[1]);
			Dictionary<enumMaterialCategory, double[]> dictionary3 = new Dictionary<enumMaterialCategory, double[]>();
			if (<getShipIdLarge>c__AnonStorey4A.group_id == 1)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(enumMaterialCategory, new double[]
				{
					4000.0,
					0.004
				});
				dictionary4.Add(enumMaterialCategory2, new double[]
				{
					3000.0,
					0.003
				});
				dictionary4.Add(enumMaterialCategory3, new double[]
				{
					2000.0,
					0.003
				});
				dictionary4.Add(enumMaterialCategory4, new double[]
				{
					5000.0,
					0.005
				});
				dictionary4.Add(enumMaterialCategory5, new double[]
				{
					50.0,
					0.1
				});
				dictionary3 = dictionary4;
			}
			else if (<getShipIdLarge>c__AnonStorey4A.group_id == 2)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(enumMaterialCategory, new double[]
				{
					5500.0,
					0.004
				});
				dictionary4.Add(enumMaterialCategory2, new double[]
				{
					3500.0,
					0.003
				});
				dictionary4.Add(enumMaterialCategory3, new double[]
				{
					4500.0,
					0.005
				});
				dictionary4.Add(enumMaterialCategory4, new double[]
				{
					2200.0,
					0.002
				});
				dictionary4.Add(enumMaterialCategory5, new double[]
				{
					60.0,
					0.2
				});
				dictionary3 = dictionary4;
			}
			else if (<getShipIdLarge>c__AnonStorey4A.group_id == 3)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(enumMaterialCategory, new double[]
				{
					4000.0,
					0.003
				});
				dictionary4.Add(enumMaterialCategory2, new double[]
				{
					2500.0,
					0.002
				});
				dictionary4.Add(enumMaterialCategory3, new double[]
				{
					3000.0,
					0.003
				});
				dictionary4.Add(enumMaterialCategory4, new double[]
				{
					1800.0,
					0.002
				});
				dictionary4.Add(enumMaterialCategory5, new double[]
				{
					40.0,
					0.2
				});
				dictionary3 = dictionary4;
			}
			else if (<getShipIdLarge>c__AnonStorey4A.group_id == 4)
			{
				Dictionary<enumMaterialCategory, double[]> dictionary4 = new Dictionary<enumMaterialCategory, double[]>();
				dictionary4.Add(enumMaterialCategory, new double[]
				{
					3000.0,
					0.002
				});
				dictionary4.Add(enumMaterialCategory2, new double[]
				{
					2000.0,
					0.002
				});
				dictionary4.Add(enumMaterialCategory3, new double[]
				{
					2500.0,
					0.002
				});
				dictionary4.Add(enumMaterialCategory4, new double[]
				{
					1500.0,
					0.002
				});
				dictionary4.Add(enumMaterialCategory5, new double[]
				{
					40.0,
					0.2
				});
				dictionary3 = dictionary4;
			}
			int num4 = (int)(((double)material.get_Item(enumMaterialCategory) - dictionary3.get_Item(enumMaterialCategory)[0]) * dictionary3.get_Item(enumMaterialCategory)[1]);
			int num5 = (int)(((double)material.get_Item(enumMaterialCategory2) - dictionary3.get_Item(enumMaterialCategory2)[0]) * dictionary3.get_Item(enumMaterialCategory2)[1]);
			int num6 = (int)(((double)material.get_Item(enumMaterialCategory3) - dictionary3.get_Item(enumMaterialCategory3)[0]) * dictionary3.get_Item(enumMaterialCategory3)[1]);
			int num7 = (int)(((double)material.get_Item(enumMaterialCategory4) - dictionary3.get_Item(enumMaterialCategory4)[0]) * dictionary3.get_Item(enumMaterialCategory4)[1]);
			int num8 = (int)(((double)material.get_Item(enumMaterialCategory5) - dictionary3.get_Item(enumMaterialCategory5)[0]) * dictionary3.get_Item(enumMaterialCategory5)[1]);
			int num9 = num4 + num5 + num6 + num7 + num8;
			int num10 = (num9 >= 0) ? random.Next(num9) : 0;
			if (num10 > 50)
			{
				num10 = 50;
			}
			int num11 = num3 - num10;
			if (num11 < 1)
			{
				num11 = 2 - num11;
			}
			if (this.createLargeTable == null)
			{
				this.createLargeTable = Utils.Xml_Result("mst_createship_large", "mst_createship_large", "Id");
			}
			var <>__AnonType = Enumerable.First(Enumerable.Skip(Enumerable.Select(Enumerable.Where<XElement>(this.createLargeTable, (XElement data) => data.Element("Group_id").get_Value() == <getShipIdLarge>c__AnonStorey4A.group_id.ToString()), (XElement data) => new
			{
				id = int.Parse(data.Element("Id").get_Value()),
				ship_id = int.Parse(data.Element("Ship_id").get_Value()),
				change_flag = int.Parse(data.Element("Change_flag").get_Value())
			}), num11));
			int result = <>__AnonType.ship_id;
			if (<>__AnonType.change_flag == 1)
			{
				int changeCreateShipId = this.getChangeCreateShipId(true, <>__AnonType.id, deck_rid);
				if (changeCreateShipId != -1)
				{
					result = changeCreateShipId;
				}
			}
			return result;
		}

		private int getChangeCreateShipId(bool largeDock, int create_id, int deck_rid)
		{
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				return -1;
			}
			List<Mem_ship> memShip = mem_deck.Ship.getMemShip();
			if (memShip.get_Count() == 0)
			{
				return -1;
			}
			int flag_shipid = memShip.get_Item(0).Ship_id;
			IEnumerable<XElement> enumerable;
			if (largeDock)
			{
				if (this.createLargeTable_change == null)
				{
					this.createLargeTable_change = Utils.Xml_Result("mst_createship_large_change", "mst_createship_large_change", "Id");
				}
				enumerable = this.createLargeTable_change;
			}
			else
			{
				if (this.createTable_change == null)
				{
					this.createTable_change = Utils.Xml_Result("mst_createship_change", "mst_createship_change", "Id");
				}
				enumerable = this.createTable_change;
			}
			var enumerable2 = Enumerable.Select(Enumerable.Where<XElement>(Enumerable.Where<XElement>(enumerable, (XElement data) => data.Element("Flag_ship_id").get_Value() == flag_shipid.ToString()), (XElement data) => data.Element("Ship_create_id").get_Value() == create_id.ToString()), (XElement data) => new
			{
				changed_ship_id = int.Parse(data.Element("Changed_ship_id").get_Value())
			});
			if (!Enumerable.Any(enumerable2))
			{
				return -1;
			}
			return Enumerable.First(enumerable2).changed_ship_id;
		}

		private int getCreateTankerNum(Dictionary<enumMaterialCategory, int> use_mat, int use_point, ref int req_turn)
		{
			int num = 10;
			Random random = new Random();
			Dictionary<enumMaterialCategory, double> dictionary = Enumerable.ToDictionary<KeyValuePair<enumMaterialCategory, int>, enumMaterialCategory, double>(use_mat, (KeyValuePair<enumMaterialCategory, int> key) => key.get_Key(), (KeyValuePair<enumMaterialCategory, int> val) => (double)val.get_Value());
			double num2 = Math.Sqrt(dictionary.get_Item(enumMaterialCategory.Steel));
			double num3 = Math.Sqrt(dictionary.get_Item(enumMaterialCategory.Fuel) / 2.0);
			double num4 = Math.Sqrt(dictionary.get_Item(enumMaterialCategory.Bull) / 40.0 + dictionary.get_Item(enumMaterialCategory.Bauxite) / 10.0);
			double num5 = num2 + num3 + num4;
			double num6 = 0.75;
			if (use_point == 100)
			{
				num6 = 1.0;
			}
			else if (use_point == 200)
			{
				num6 = 1.3;
			}
			double num7 = (double)random.Next(5);
			double num8 = num5 / 10.0 + num5 / 50.0 * num7 * num6 + 0.3;
			if (num3 < num2 / 2.0)
			{
				num8 *= 0.65;
			}
			int num9 = (int)num8;
			if (num9 >= 5 && use_point < 200)
			{
				num9 = 5;
			}
			if (num9 >= 7 && use_point < 400)
			{
				num9 = 7;
			}
			if (num9 > num)
			{
				num9 = num;
			}
			else if (num9 < 1)
			{
				num9 = 1;
			}
			if (num9 > 7)
			{
				req_turn = 3;
			}
			else if (num9 > 2)
			{
				req_turn = 2;
			}
			else
			{
				req_turn = 1;
			}
			return num9;
		}

		public Api_Result<object> DestroyShip(int ship_rid)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			Mem_ship mem_ship = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(ship_rid, ref mem_ship))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ship.Locked == 1 || mem_ship.IsBlingShip() || mem_ship.GetLockSlotItems().get_Count() > 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_ship.Rid == Comm_UserDatas.Instance.User_deck.get_Item(1).Ship[0])
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<int> slot_rids = new List<int>();
			mem_ship.Slot.ForEach(delegate(int x)
			{
				if (x > 0)
				{
					slot_rids.Add(x);
				}
			});
			if (mem_ship.Exslot > 0)
			{
				slot_rids.Add(mem_ship.Exslot);
			}
			this.questDestroyItemDisable = true;
			this.DestroyItem(slot_rids);
			this.questDestroyItemDisable = false;
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(mem_ship.Ship_id);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Fuel).Add_Material(mst_ship.Broken1);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bull).Add_Material(mst_ship.Broken2);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Steel).Add_Material(mst_ship.Broken3);
			Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Bauxite).Add_Material(mst_ship.Broken4);
			int[] array = Comm_UserDatas.Instance.User_deck.get_Item(1).Search_ShipIdx(ship_rid);
			if (array[0] != -1)
			{
				Mem_deck mem_deck = Comm_UserDatas.Instance.User_deck.get_Item(array[0]);
				mem_deck.Ship.RemoveShip(ship_rid);
			}
			Comm_UserDatas.Instance.User_ship.Remove(ship_rid);
			QuestKousyou questKousyou = new QuestKousyou(mst_ship);
			questKousyou.ExecuteCheck();
			api_Result.data = null;
			return api_Result;
		}

		public Api_Result<object> DestroyItem(List<int> slot_rids)
		{
			Api_Result<object> api_Result = new Api_Result<object>();
			if (slot_rids == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			dictionary.Add(enumMaterialCategory.Fuel, 0);
			dictionary.Add(enumMaterialCategory.Bull, 0);
			dictionary.Add(enumMaterialCategory.Steel, 0);
			dictionary.Add(enumMaterialCategory.Bauxite, 0);
			Dictionary<enumMaterialCategory, int> dictionary2 = dictionary;
			List<Mst_slotitem> list = new List<Mst_slotitem>();
			using (List<int>.Enumerator enumerator = slot_rids.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					Mem_slotitem mem_slotitem = null;
					if (Comm_UserDatas.Instance.User_slot.TryGetValue(current, ref mem_slotitem))
					{
						if (mem_slotitem.Lock)
						{
							api_Result.state = Api_Result_State.Parameter_Error;
							return api_Result;
						}
						Mst_slotitem mst_slotitem = Mst_DataManager.Instance.Mst_Slotitem.get_Item(mem_slotitem.Slotitem_id);
						Dictionary<enumMaterialCategory, int> expr_AC = dictionary = dictionary2;
						enumMaterialCategory enumMaterialCategory;
						enumMaterialCategory expr_B0 = enumMaterialCategory = enumMaterialCategory.Fuel;
						int num = dictionary.get_Item(enumMaterialCategory);
						expr_AC.set_Item(expr_B0, num + mst_slotitem.Broken1);
						Dictionary<enumMaterialCategory, int> dictionary3;
						Dictionary<enumMaterialCategory, int> expr_CE = dictionary3 = dictionary2;
						enumMaterialCategory expr_D2 = enumMaterialCategory = enumMaterialCategory.Bull;
						num = dictionary3.get_Item(enumMaterialCategory);
						expr_CE.set_Item(expr_D2, num + mst_slotitem.Broken2);
						Dictionary<enumMaterialCategory, int> dictionary4;
						Dictionary<enumMaterialCategory, int> expr_F0 = dictionary4 = dictionary2;
						enumMaterialCategory expr_F4 = enumMaterialCategory = enumMaterialCategory.Steel;
						num = dictionary4.get_Item(enumMaterialCategory);
						expr_F0.set_Item(expr_F4, num + mst_slotitem.Broken3);
						Dictionary<enumMaterialCategory, int> dictionary5;
						Dictionary<enumMaterialCategory, int> expr_112 = dictionary5 = dictionary2;
						enumMaterialCategory expr_116 = enumMaterialCategory = enumMaterialCategory.Bauxite;
						num = dictionary5.get_Item(enumMaterialCategory);
						expr_112.set_Item(expr_116, num + mst_slotitem.Broken4);
						list.Add(mst_slotitem);
					}
				}
			}
			Comm_UserDatas.Instance.Remove_Slot(slot_rids);
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator2 = dictionary2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current2 = enumerator2.get_Current();
					Comm_UserDatas.Instance.User_material.get_Item(current2.get_Key()).Add_Material(current2.get_Value());
				}
			}
			if (!this.questDestroyItemDisable)
			{
				QuestKousyou questKousyou = new QuestKousyou(list);
				questKousyou.ExecuteCheck();
			}
			api_Result.data = null;
			return api_Result;
		}

		public Api_Result<Mst_slotitem> CreateItem(Dictionary<enumMaterialCategory, int> items, int deck_rid)
		{
			Api_Result<Mst_slotitem> api_Result = new Api_Result<Mst_slotitem>();
			if (!this.validCreateItem(items, deck_rid))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.createItemGroup == null)
			{
				this.createItemGroup = Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id");
			}
			int createItemType = this.getCreateItemType(deck_rid);
			if (this.createItemTable == null)
			{
				this.createItemTable = Utils.Xml_Result("mst_slotitemget2", "mst_slotitemget2", "Id");
			}
			int createItem = this.getCreateItem(3, createItemType, items);
			Mst_slotitem mst_slotitem = null;
			Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(createItem, ref mst_slotitem);
			bool flag = true;
			if (!this.isCreateItemRareCheck(mst_slotitem) || !this.isCreateItemMatCheck(items, mst_slotitem))
			{
				flag = false;
			}
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator = items.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current = enumerator.get_Current();
					Comm_UserDatas.Instance.User_material.get_Item(current.get_Key()).Sub_Material(current.get_Value());
				}
			}
			if (flag)
			{
				Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Dev_Kit).Sub_Material(1);
				Comm_UserDatas arg_137_0 = Comm_UserDatas.Instance;
				List<int> list = new List<int>();
				list.Add(createItem);
				arg_137_0.Add_Slot(list);
				api_Result.data = mst_slotitem;
			}
			QuestKousyou questKousyou = new QuestKousyou(items, flag);
			questKousyou.ExecuteCheck();
			return api_Result;
		}

		private bool validCreateItem(Dictionary<enumMaterialCategory, int> items, int deck_rid)
		{
			if (Enumerable.Count<KeyValuePair<enumMaterialCategory, int>>(items) != 4)
			{
				return false;
			}
			Dictionary<enumMaterialCategory, int[]> dictionary = new Dictionary<enumMaterialCategory, int[]>();
			dictionary.Add(enumMaterialCategory.Fuel, new int[]
			{
				10,
				300
			});
			dictionary.Add(enumMaterialCategory.Bull, new int[]
			{
				10,
				300
			});
			dictionary.Add(enumMaterialCategory.Steel, new int[]
			{
				10,
				300
			});
			dictionary.Add(enumMaterialCategory.Bauxite, new int[]
			{
				10,
				300
			});
			Dictionary<enumMaterialCategory, int[]> dictionary2 = dictionary;
			using (Dictionary<enumMaterialCategory, int[]>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int[]> current = enumerator.get_Current();
					if (!items.ContainsKey(current.get_Key()))
					{
						bool result = false;
						return result;
					}
					if (items.get_Item(current.get_Key()) < current.get_Value()[0] || items.get_Item(current.get_Key()) > current.get_Value()[1])
					{
						bool result = false;
						return result;
					}
					if (items.get_Item(current.get_Key()) > Comm_UserDatas.Instance.User_material.get_Item(current.get_Key()).Value)
					{
						bool result = false;
						return result;
					}
				}
			}
			if (Comm_UserDatas.Instance.User_material.get_Item(enumMaterialCategory.Dev_Kit).Value < 1)
			{
				return false;
			}
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_rid, ref mem_deck))
			{
				return false;
			}
			int num = mem_deck.Ship[0];
			return Comm_UserDatas.Instance.User_ship.ContainsKey(num);
		}

		private int getCreateItemType(int deckRid)
		{
			int num = Comm_UserDatas.Instance.User_deck.get_Item(deckRid).Ship[0];
			Mem_ship ship = Comm_UserDatas.Instance.User_ship.get_Item(num);
			var <>__AnonType = Enumerable.First(Enumerable.Select(Enumerable.Where<XElement>(this.createItemGroup, (XElement item) => item.Element("Id").get_Value() == ship.Stype.ToString()), (XElement item) => new
			{
				type = item.Element("Createitem").get_Value()
			}));
			return int.Parse(<>__AnonType.type);
		}

		private int getCreateItem(int retryNum, int type, Dictionary<enumMaterialCategory, int> items)
		{
			int result = 0;
			for (int i = 0; i < retryNum; i++)
			{
				int createItemId = this.getCreateItemId(type, items);
				Mst_slotitem mst_slot = null;
				if (Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(createItemId, ref mst_slot))
				{
					if (this.isCreateItemRareCheck(mst_slot))
					{
						if (this.isCreateItemMatCheck(items, mst_slot))
						{
							result = createItemId;
							break;
						}
					}
				}
			}
			return result;
		}

		private bool isCreateItemRareCheck(Mst_slotitem mst_slot)
		{
			if (mst_slot == null)
			{
				return false;
			}
			double randDouble = Utils.GetRandDouble(0.0, 100.0, 1.0, 1);
			int num = mst_slot.Rare;
			if (randDouble <= 20.0)
			{
				num -= 2;
			}
			else if (randDouble <= 50.0)
			{
				num--;
			}
			return Comm_UserDatas.Instance.User_record.Level >= num * 10;
		}

		private bool isCreateItemMatCheck(Dictionary<enumMaterialCategory, int> items, Mst_slotitem mst_slot)
		{
			return mst_slot != null && items.get_Item(enumMaterialCategory.Fuel) >= mst_slot.Broken1 * 10 && items.get_Item(enumMaterialCategory.Bull) >= mst_slot.Broken2 * 10 && items.get_Item(enumMaterialCategory.Steel) >= mst_slot.Broken3 * 10 && items.get_Item(enumMaterialCategory.Bauxite) >= mst_slot.Broken4 * 10;
		}

		private int getCreateItemId(int typeNo, Dictionary<enumMaterialCategory, int> items)
		{
			int num = Enumerable.Max(items.get_Values());
			int layerNo = 1;
			if (items.get_Item(enumMaterialCategory.Fuel) == num)
			{
				layerNo = 1;
			}
			else if (items.get_Item(enumMaterialCategory.Steel) == num)
			{
				layerNo = 3;
			}
			else if (items.get_Item(enumMaterialCategory.Bull) == num)
			{
				layerNo = 2;
			}
			else if (items.get_Item(enumMaterialCategory.Bauxite) == num)
			{
				layerNo = 4;
			}
			Random random = new Random();
			int num2 = random.Next(50);
			var enumerable = Enumerable.Select(Enumerable.OrderBy(Enumerable.Where(Enumerable.Select(this.createItemTable, (XElement item) => new
			{
				item = item,
				table_id = int.Parse(item.Element("Id").get_Value())
			}), <>__TranspIdent4 => <>__TranspIdent4.item.Element("Type").get_Value() == typeNo.ToString() && <>__TranspIdent4.item.Element("Layer" + layerNo).get_Value() == "1"), <>__TranspIdent4 => <>__TranspIdent4.table_id), <>__TranspIdent4 => new
			{
				slot_id = <>__TranspIdent4.item.Element("Slotitem_id").get_Value()
			});
			var <>__AnonType = Enumerable.First(Enumerable.Skip(enumerable, num2));
			return int.Parse(<>__AnonType.slot_id);
		}

		public void Initialize_SlotitemRemodel()
		{
			this.mst_remodel = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
			this.mst_remodel_detail = Mst_DataManager.Instance.Get_Mst_slotitem_remodel_detail();
		}

		public Api_Result<Dictionary<int, List<Mst_slotitem_remodel>>> getSlotitemRemodelList(int deck_id)
		{
			Api_Result<Dictionary<int, List<Mst_slotitem_remodel>>> api_Result = new Api_Result<Dictionary<int, List<Mst_slotitem_remodel>>>();
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> memShip = mem_deck.Ship.getMemShip();
			if (memShip.get_Count() == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (memShip.get_Item(0).Stype != 19)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.mst_remodel == null)
			{
				this.mst_remodel = Mst_DataManager.Instance.Get_Mst_slotitem_remodel();
			}
			bool flag = false;
			if (this.mst_remodel_list == null)
			{
				this.mst_remodel_list = new Dictionary<int, Mst_slotitem_remodel>();
				flag = true;
			}
			Dictionary<int, List<Mst_slotitem_remodel>> dictionary = new Dictionary<int, List<Mst_slotitem_remodel>>();
			using (Dictionary<int, List<Mst_slotitem_remodel>>.Enumerator enumerator = this.mst_remodel.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, List<Mst_slotitem_remodel>> current = enumerator.get_Current();
					List<Mst_slotitem_remodel> list = new List<Mst_slotitem_remodel>();
					list.Add(null);
					list.Add(null);
					list.Add(null);
					list.Add(null);
					List<Mst_slotitem_remodel> list2 = list;
					List<Mst_slotitem_remodel> list3 = new List<Mst_slotitem_remodel>();
					dictionary.Add(current.get_Key(), list2);
					using (List<Mst_slotitem_remodel>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Mst_slotitem_remodel current2 = enumerator2.get_Current();
							if (flag)
							{
								this.mst_remodel_list.Add(current2.Id, current2);
							}
							if (current2.ValidShipId(memShip))
							{
								list2.set_Item(0, current2);
							}
							if (current2.ValidYomi(memShip))
							{
								list2.set_Item(1, current2);
							}
							if (current2.ValidStype(memShip))
							{
								list2.set_Item(2, current2);
							}
							if (current2.IsRemodelBase(memShip))
							{
								list2.set_Item(3, current2);
							}
						}
					}
					list2.RemoveAll((Mst_slotitem_remodel x) => x == null);
					list3.Add(list2.get_Item(0));
					dictionary.set_Item(current.get_Key(), list3);
				}
			}
			if (dictionary.get_Count() == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			api_Result.data = dictionary;
			return api_Result;
		}

		public Api_Result<Mst_slotitem_remodel_detail> getSlotitemRemodelListDetail(int menu_id, int slot_id)
		{
			Api_Result<Mst_slotitem_remodel_detail> api_Result = new Api_Result<Mst_slotitem_remodel_detail>();
			Mem_slotitem mem_slotitem;
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(slot_id, ref mem_slotitem))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.mst_remodel_detail == null)
			{
				this.mst_remodel_detail = Mst_DataManager.Instance.Get_Mst_slotitem_remodel_detail();
			}
			Mst_slotitem_remodel_detail mst_slotitem_remodel_detail = null;
			using (List<Mst_slotitem_remodel_detail>.Enumerator enumerator = this.mst_remodel_detail.get_Item(menu_id).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_slotitem_remodel_detail current = enumerator.get_Current();
					if (mem_slotitem.Level >= current.Level_from && mem_slotitem.Level <= current.Level_to)
					{
						mst_slotitem_remodel_detail = current;
						break;
					}
				}
			}
			if (mst_slotitem_remodel_detail == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			api_Result.data = mst_slotitem_remodel_detail;
			return api_Result;
		}

		public Api_Result<bool> RemodelSlot(Mst_slotitem_remodel_detail detail, int deck_id, int slot_id, bool certain_flag)
		{
			Api_Result<bool> api_Result = new Api_Result<bool>();
			Mem_slotitem mem_slotitem;
			if (!Comm_UserDatas.Instance.User_slot.TryGetValue(slot_id, ref mem_slotitem))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (mem_slotitem.Equip_flag == Mem_slotitem.enumEquipSts.Equip)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_deck mem_deck = null;
			if (!Comm_UserDatas.Instance.User_deck.TryGetValue(deck_id, ref mem_deck))
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			List<Mem_ship> memShip = mem_deck.Ship.getMemShip();
			if (memShip.get_Count() == 0)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Dictionary<enumMaterialCategory, int> remodelUseMaterials = this.GetRemodelUseMaterials(detail, certain_flag);
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator = remodelUseMaterials.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current = enumerator.get_Current();
					if (Comm_UserDatas.Instance.User_material.get_Item(current.get_Key()).Value < current.get_Value())
					{
						api_Result.state = Api_Result_State.Parameter_Error;
						return api_Result;
					}
				}
			}
			List<Mem_slotitem> list = new List<Mem_slotitem>();
			if (detail.Req_slotitems >= 1)
			{
				list = Enumerable.ToList<Mem_slotitem>(Enumerable.Take<Mem_slotitem>(Enumerable.Where<Mem_slotitem>(Enumerable.Where<Mem_slotitem>(Enumerable.Where<Mem_slotitem>(Enumerable.Where<Mem_slotitem>(Enumerable.Where<Mem_slotitem>(Comm_UserDatas.Instance.User_slot.get_Values(), (Mem_slotitem mem_slot) => mem_slot.Rid != slot_id), (Mem_slotitem mem_slot) => mem_slot.Level == 0), (Mem_slotitem mem_slot) => mem_slot.Slotitem_id == detail.Req_slotitem_id), (Mem_slotitem mem_slot) => mem_slot.Equip_flag == Mem_slotitem.enumEquipSts.Unset), (Mem_slotitem mem_slot) => !mem_slot.Lock), detail.Req_slotitems));
				if (detail.Req_slotitems < Enumerable.Count<Mem_slotitem>(list))
				{
					api_Result.state = Api_Result_State.Parameter_Error;
					return api_Result;
				}
			}
			int num = 100;
			if (!certain_flag)
			{
				if (memShip.get_Item(0).Ship_id == 182)
				{
					num = detail.Success_rate1;
				}
				else if (memShip.get_Item(0).Ship_id == 187)
				{
					num = detail.Success_rate2;
				}
				else
				{
					num = detail.Success_rate1;
				}
			}
			int num2 = new Random().Next(100);
			bool flag = num2 < num;
			int slotitem_id = mem_slotitem.Slotitem_id;
			int num3 = slotitem_id;
			int level = 0;
			if (flag)
			{
				if (detail.New_slotitem_id > 0)
				{
					num3 = detail.New_slotitem_id;
					level = detail.New_slotitem_level;
				}
				else
				{
					level = mem_slotitem.Level + 1;
				}
			}
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator2 = remodelUseMaterials.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current2 = enumerator2.get_Current();
					Comm_UserDatas.Instance.User_material.get_Item(current2.get_Key()).Sub_Material(current2.get_Value());
				}
			}
			list.ForEach(delegate(Mem_slotitem x)
			{
				Comm_UserDatas.Instance.User_slot.Remove(x.Rid);
			});
			if (flag)
			{
				if (slotitem_id != num3)
				{
					this.ChangeSlotId(mem_slotitem, num3);
					Comm_UserDatas.Instance.Add_Book(2, num3);
				}
				mem_slotitem.SetLevel(level);
			}
			api_Result.data = flag;
			QuestKousyou questKousyou = new QuestKousyou(detail, mem_slotitem, flag);
			questKousyou.ExecuteCheck();
			return api_Result;
		}

		public void ChangeSlotId(Mem_slotitem obj, int changeId)
		{
			obj.ChangeSlotId(this, changeId);
		}

		private Dictionary<enumMaterialCategory, int> GetRemodelUseMaterials(Mst_slotitem_remodel_detail detailObj, bool cetain_flag)
		{
			Dictionary<enumMaterialCategory, int> dictionary = new Dictionary<enumMaterialCategory, int>();
			Mst_slotitem_remodel mst_slotitem_remodel = this.mst_remodel_list.get_Item(detailObj.Id);
			dictionary.set_Item(enumMaterialCategory.Fuel, mst_slotitem_remodel.Req_material1);
			dictionary.set_Item(enumMaterialCategory.Bull, mst_slotitem_remodel.Req_material2);
			dictionary.set_Item(enumMaterialCategory.Steel, mst_slotitem_remodel.Req_material3);
			dictionary.set_Item(enumMaterialCategory.Bauxite, mst_slotitem_remodel.Req_material4);
			if (cetain_flag)
			{
				dictionary.set_Item(enumMaterialCategory.Dev_Kit, detailObj.Req_material5_2);
				dictionary.set_Item(enumMaterialCategory.Revamp_Kit, detailObj.Req_material6_2);
			}
			else
			{
				dictionary.set_Item(enumMaterialCategory.Dev_Kit, detailObj.Req_material5_1);
				dictionary.set_Item(enumMaterialCategory.Revamp_Kit, detailObj.Req_material6_1);
			}
			return dictionary;
		}
	}
}
