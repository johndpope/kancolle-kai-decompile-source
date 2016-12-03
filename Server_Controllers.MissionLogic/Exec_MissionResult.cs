using Common.Enum;
using Server_Common;
using Server_Common.Formats;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Server_Controllers.MissionLogic
{
	public class Exec_MissionResult : IRebellionPointOperator
	{
		private List<Mem_ship> mem_ships;

		private Mem_deck mem_deck;

		private Dictionary<int, int> mst_level;

		private Dictionary<int, int> mst_userlevel;

		private Mst_mission2 mst_mission;

		private MissionResultFmt resultInfo;

		private Random randInstance;

		private List<Mem_tanker> missionTanker;

		private int daihatuNum;

		private int drumNum;

		private int drumShipNum;

		public Exec_MissionResult(Mem_deck deck, Dictionary<int, int> mst_level, Dictionary<int, int> mst_userlevel)
		{
			this.daihatuNum = 0;
			this.drumNum = 0;
			this.drumShipNum = 0;
			this.mem_deck = deck;
			this.mst_level = mst_level;
			this.mst_userlevel = mst_userlevel;
			this.mem_ships = deck.Ship.getMemShip();
			this.mst_mission = Mst_DataManager.Instance.Mst_mission.get_Item(deck.Mission_id);
			this.missionTanker = Enumerable.ToList<Mem_tanker>(Enumerable.Where<Mem_tanker>(Comm_UserDatas.Instance.User_tanker.get_Values(), (Mem_tanker x) => x.Mission_deck_rid == deck.Rid));
			this.randInstance = new Random();
		}

		void IRebellionPointOperator.AddRebellionPoint(int area_id, int addNum)
		{
			throw new NotImplementedException();
		}

		void IRebellionPointOperator.SubRebellionPoint(int area_id, int subNum)
		{
			Mem_rebellion_point mem_rebellion_point = null;
			if (Comm_UserDatas.Instance.User_rebellion_point.TryGetValue(area_id, ref mem_rebellion_point))
			{
				mem_rebellion_point.SubPoint(this, subNum);
			}
		}

		public MissionResultFmt GetResultData()
		{
			this.resultInfo = new MissionResultFmt();
			this.resultInfo.Deck = this.mem_deck;
			this.resultInfo.MissionName = this.mst_mission.Name;
			this.setResultKind();
			Dictionary<enumMaterialCategory, int> up_mat = null;
			this.setItems(out up_mat);
			this.setBasicExp();
			Dictionary<int, int> shipExp = this.getShipExp();
			this.resultInfo.GetShipExp = shipExp;
			this.updateShip(shipExp, out this.resultInfo.LevelUpInfo);
			this.updateMaterial(up_mat);
			this.updateDeck();
			this.updateItem();
			this.updateMissionComp();
			this.updateBasic();
			this.updateRebellionPoint(this.resultInfo.MissionResult);
			this.resultInfo.MemberLevel = this.updateRecord();
			this.missionTanker.ForEach(delegate(Mem_tanker x)
			{
				x.MissionTerm();
			});
			return this.resultInfo;
		}

		private void setResultKind()
		{
			if (this.mem_deck.MissionState == MissionStates.STOP)
			{
				this.resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (this.mst_mission.Flagship_level > 0)
			{
				FatigueState fatigueState = this.mem_ships.get_Item(0).Get_FatigueState();
				if (fatigueState >= FatigueState.Light)
				{
					this.resultInfo.MissionResult = MissionResultKinds.FAILE;
					return;
				}
				if (this.mst_mission.Flagship_level > this.mem_ships.get_Item(0).Level)
				{
					this.resultInfo.MissionResult = MissionResultKinds.FAILE;
					return;
				}
			}
			if (this.mst_mission.Flagship_stype1 > 0 && this.mst_mission.Flagship_stype1 != this.mem_ships.get_Item(0).Stype)
			{
				this.resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (this.mst_mission.Tanker_num > 0 && this.missionTanker.get_Count() < this.mst_mission.Tanker_num)
			{
				this.resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			Dictionary<int, int> dictionary = Enumerable.ToDictionary(Enumerable.Select(Enumerable.Select(Utils.Xml_Result("mst_stype_group", "mst_stype_group", "Id"), (XElement data) => new
			{
				data = data,
				item = new
				{
					id = int.Parse(data.Element("Id").get_Value()),
					type = int.Parse(data.Element("Mission").get_Value())
				}
			}), <>__TranspIdent15 => <>__TranspIdent15.item), obj => obj.id, value => value.type);
			IEnumerable<int> enumerable = Enumerable.Distinct<int>(dictionary.get_Values());
			Dictionary<int, int> dictionary2 = Enumerable.ToDictionary<int, int, int>(enumerable, (int type) => type, (int count) => 0);
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			using (List<Mem_ship>.Enumerator enumerator = this.mem_ships.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					FatigueState fatigueState2 = current.Get_FatigueState();
					if (fatigueState2 == FatigueState.Distress)
					{
						this.resultInfo.MissionResult = MissionResultKinds.FAILE;
						return;
					}
					Mem_ship arg_251_0 = current;
					HashSet<int> hashSet = new HashSet<int>();
					hashSet.Add(68);
					hashSet.Add(75);
					Dictionary<int, int> mstSlotItemNum_OrderId = arg_251_0.GetMstSlotItemNum_OrderId(hashSet);
					Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(current.Ship_id);
					this.daihatuNum += mstSlotItemNum_OrderId.get_Item(68);
					if (mstSlotItemNum_OrderId.get_Item(75) > 0)
					{
						this.drumShipNum++;
					}
					this.drumNum += mstSlotItemNum_OrderId.get_Item(75);
					num7 += current.Level;
					num6 += mst_ship.Bull_max;
					num5 += mst_ship.Fuel_max;
					if (fatigueState2 == FatigueState.Light)
					{
						num2++;
					}
					else
					{
						if (fatigueState2 == FatigueState.Exaltation)
						{
							num++;
						}
						num4 += current.Bull;
						num3 += current.Fuel;
						int num8 = dictionary.get_Item(current.Stype);
						Dictionary<int, int> dictionary3;
						Dictionary<int, int> expr_326 = dictionary3 = dictionary2;
						int num9;
						int expr_32B = num9 = num8;
						num9 = dictionary3.get_Item(num9);
						expr_326.set_Item(expr_32B, num9 + 1);
					}
				}
			}
			if (dictionary2.get_Item(1) < this.mst_mission.Stype_num1 || dictionary2.get_Item(2) < this.mst_mission.Stype_num2 || dictionary2.get_Item(3) < this.mst_mission.Stype_num3 || dictionary2.get_Item(4) < this.mst_mission.Stype_num4 || dictionary2.get_Item(5) < this.mst_mission.Stype_num5 || dictionary2.get_Item(6) < this.mst_mission.Stype_num6 || dictionary2.get_Item(7) < this.mst_mission.Stype_num7 || dictionary2.get_Item(8) < this.mst_mission.Stype_num8 || dictionary2.get_Item(9) < this.mst_mission.Stype_num9)
			{
				this.resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (this.mst_mission.Deck_num > 0)
			{
				int num10 = this.mem_ships.get_Count() - num2;
				if (num10 < this.mst_mission.Deck_num)
				{
					this.resultInfo.MissionResult = MissionResultKinds.FAILE;
					return;
				}
			}
			if (num7 < this.mst_mission.Level)
			{
				this.resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (this.drumShipNum < this.mst_mission.Drum_ship_num || this.drumNum < this.mst_mission.Drum_total_num1)
			{
				this.resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			double num11 = (double)(num3 + num4);
			double num12 = (double)(num5 + num6);
			double num13 = num11 / num12 * 100.0;
			int num14 = (int)(num13 + (double)this.randInstance.Next(20));
			if (num14 < 100)
			{
				this.resultInfo.MissionResult = MissionResultKinds.FAILE;
				return;
			}
			if (num2 > 0)
			{
				this.resultInfo.MissionResult = MissionResultKinds.SUCCESS;
				return;
			}
			if (!this.mst_mission.IsGreatSuccessCondition() && num < this.mem_ships.get_Count())
			{
				this.resultInfo.MissionResult = MissionResultKinds.SUCCESS;
				return;
			}
			int drumKeisu = 0;
			if (this.mst_mission.Drum_total_num2 > 0)
			{
				drumKeisu = ((this.drumNum < this.mst_mission.Drum_total_num2) ? -15 : 20);
			}
			if (this.mst_mission.Flagship_stype2 > 0 && this.mst_mission.Flagship_stype2 != this.mem_ships.get_Item(0).Stype)
			{
				this.resultInfo.MissionResult = MissionResultKinds.SUCCESS;
				return;
			}
			int checkRate = this.getCheckRate(num, drumKeisu);
			int num15 = this.randInstance.Next(100);
			if (checkRate >= num15)
			{
				this.resultInfo.MissionResult = MissionResultKinds.GREAT;
				return;
			}
			this.resultInfo.MissionResult = MissionResultKinds.SUCCESS;
			return;
		}

		private int getCheckRate(int goodCondNum, int drumKeisu)
		{
			int num = 20 + goodCondNum * 15 + drumKeisu;
			if (this.mst_mission.Flagship_level_check_type == 2)
			{
				int flagShipLevelCheckValue = this.getFlagShipLevelCheckValue();
				num = num - 5 + flagShipLevelCheckValue;
			}
			return num;
		}

		private int getFlagShipLevelCheckValue()
		{
			int level = this.mem_ships.get_Item(0).Level;
			double num = Math.Sqrt((double)level) + (double)level / 10.0;
			return (int)num;
		}

		private void setItems(out Dictionary<enumMaterialCategory, int> up_mat)
		{
			up_mat = null;
			if (this.resultInfo.MissionResult == MissionResultKinds.FAILE)
			{
				return;
			}
			double success_keisu = (this.resultInfo.MissionResult != MissionResultKinds.GREAT) ? 1.0 : 1.5;
			up_mat = new Dictionary<enumMaterialCategory, int>();
			Array values = Enum.GetValues(typeof(enumMaterialCategory));
			using (IEnumerator enumerator = values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					up_mat.Add((enumMaterialCategory)((int)current), 0);
				}
			}
			Dictionary<enumMaterialCategory, int> getMaterials;
			Dictionary<enumMaterialCategory, int> expr_A5 = getMaterials = this.resultInfo.GetMaterials;
			enumMaterialCategory enumMaterialCategory;
			enumMaterialCategory expr_A9 = enumMaterialCategory = enumMaterialCategory.Fuel;
			int num = getMaterials.get_Item(enumMaterialCategory);
			expr_A5.set_Item(expr_A9, num + this.mst_mission.Win_mat1);
			Dictionary<enumMaterialCategory, int> getMaterials2;
			Dictionary<enumMaterialCategory, int> expr_D5 = getMaterials2 = this.resultInfo.GetMaterials;
			enumMaterialCategory expr_D9 = enumMaterialCategory = enumMaterialCategory.Bull;
			num = getMaterials2.get_Item(enumMaterialCategory);
			expr_D5.set_Item(expr_D9, num + this.mst_mission.Win_mat2);
			Dictionary<enumMaterialCategory, int> getMaterials3;
			Dictionary<enumMaterialCategory, int> expr_105 = getMaterials3 = this.resultInfo.GetMaterials;
			enumMaterialCategory expr_109 = enumMaterialCategory = enumMaterialCategory.Steel;
			num = getMaterials3.get_Item(enumMaterialCategory);
			expr_105.set_Item(expr_109, num + this.mst_mission.Win_mat3);
			Dictionary<enumMaterialCategory, int> getMaterials4;
			Dictionary<enumMaterialCategory, int> expr_135 = getMaterials4 = this.resultInfo.GetMaterials;
			enumMaterialCategory expr_139 = enumMaterialCategory = enumMaterialCategory.Bauxite;
			num = getMaterials4.get_Item(enumMaterialCategory);
			expr_135.set_Item(expr_139, num + this.mst_mission.Win_mat4);
			using (List<enumMaterialCategory>.Enumerator enumerator2 = Enumerable.ToList<enumMaterialCategory>(this.resultInfo.GetMaterials.get_Keys()).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					enumMaterialCategory current2 = enumerator2.get_Current();
					int materialBonusNum = this.getMaterialBonusNum(this.resultInfo.GetMaterials.get_Item(current2), success_keisu);
					Dictionary<enumMaterialCategory, int> getMaterials5;
					Dictionary<enumMaterialCategory, int> expr_1AA = getMaterials5 = this.resultInfo.GetMaterials;
					enumMaterialCategory expr_1AF = enumMaterialCategory = current2;
					num = getMaterials5.get_Item(enumMaterialCategory);
					expr_1AA.set_Item(expr_1AF, num + materialBonusNum);
					up_mat.set_Item(current2, this.resultInfo.GetMaterials.get_Item(current2));
				}
			}
			List<int[]> list = new List<int[]>();
			if (this.resultInfo.MissionResult == MissionResultKinds.SUCCESS)
			{
				this.resultInfo.GetSpoint += this.mst_mission.Win_spoint1;
				int num2 = this.randInstance.Next(this.mst_mission.Win_item1_num + 1);
				if (num2 == 0)
				{
					return;
				}
				list.Add(new int[]
				{
					this.mst_mission.Win_item1,
					num2
				});
			}
			else if (this.resultInfo.MissionResult == MissionResultKinds.GREAT)
			{
				this.resultInfo.GetSpoint += this.mst_mission.Win_spoint2;
				int num3 = this.randInstance.Next(this.mst_mission.Win_item1_num + 1);
				if (num3 > 0)
				{
					list.Add(new int[]
					{
						this.mst_mission.Win_item1,
						num3
					});
				}
				if (this.mst_mission.Win_item2_num > 0)
				{
					int num4 = this.randInstance.Next(this.mst_mission.Win_item2_num) + 1;
					list.Add(new int[]
					{
						this.mst_mission.Win_item2,
						num4
					});
				}
			}
			this.resultInfo.GetItems = new List<ItemGetFmt>();
			using (List<int[]>.Enumerator enumerator3 = list.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					int[] current3 = enumerator3.get_Current();
					Mst_useitem mst_useitem = Mst_DataManager.Instance.Mst_useitem.get_Item(current3[0]);
					ItemGetFmt itemGetFmt = new ItemGetFmt();
					itemGetFmt.Id = mst_useitem.Id;
					itemGetFmt.Category = ItemGetKinds.UseItem;
					itemGetFmt.Count = current3[1];
					if (itemGetFmt.Id >= 1 && itemGetFmt.Id <= 4)
					{
						int materialBonusNum2 = this.getMaterialBonusNum(itemGetFmt.Count, success_keisu);
						itemGetFmt.Count += materialBonusNum2;
						enumMaterialCategory enumMaterialCategory2;
						if (itemGetFmt.Id == 1)
						{
							enumMaterialCategory2 = enumMaterialCategory.Repair_Kit;
						}
						else if (itemGetFmt.Id == 2)
						{
							enumMaterialCategory2 = enumMaterialCategory.Build_Kit;
						}
						else if (itemGetFmt.Id == 3)
						{
							enumMaterialCategory2 = enumMaterialCategory.Dev_Kit;
						}
						else
						{
							enumMaterialCategory2 = enumMaterialCategory.Revamp_Kit;
						}
						Dictionary<enumMaterialCategory, int> dictionary;
						Dictionary<enumMaterialCategory, int> expr_419 = dictionary = up_mat;
						enumMaterialCategory expr_41E = enumMaterialCategory = enumMaterialCategory2;
						num = dictionary.get_Item(enumMaterialCategory);
						expr_419.set_Item(expr_41E, num + itemGetFmt.Count);
					}
					this.resultInfo.GetItems.Add(itemGetFmt);
				}
			}
			if (this.resultInfo.GetItems.get_Count() == 0)
			{
				this.resultInfo.GetItems = null;
			}
		}

		private int getMaterialBonusNum(int nowNum, double success_keisu)
		{
			double num = 0.05;
			int num2 = (this.daihatuNum <= 4) ? this.daihatuNum : 4;
			int num3 = (int)((double)nowNum * success_keisu);
			int num4 = (int)((double)(num3 * num2) * num);
			return num3 - nowNum + num4;
		}

		private void setBasicExp()
		{
			if (this.mem_deck.MissionState == MissionStates.STOP)
			{
				this.resultInfo.GetMemberExp = 0;
				return;
			}
			int win_exp_member = this.mst_mission.Win_exp_member;
			Dictionary<MissionResultKinds, double> dictionary = new Dictionary<MissionResultKinds, double>();
			dictionary.Add(MissionResultKinds.FAILE, 0.3);
			dictionary.Add(MissionResultKinds.SUCCESS, 1.0);
			dictionary.Add(MissionResultKinds.GREAT, 2.0);
			Dictionary<MissionResultKinds, double> dictionary2 = dictionary;
			MissionResultKinds missionResult = this.resultInfo.MissionResult;
			this.resultInfo.GetMemberExp = (int)((double)win_exp_member * dictionary2.get_Item(missionResult));
		}

		private Dictionary<int, int> getShipExp()
		{
			Exec_MissionResult.<getShipExp>c__AnonStorey4DE <getShipExp>c__AnonStorey4DE = new Exec_MissionResult.<getShipExp>c__AnonStorey4DE();
			<getShipExp>c__AnonStorey4DE.<>f__this = this;
			if (this.mem_deck.MissionState == MissionStates.STOP)
			{
				return Enumerable.ToDictionary<Mem_ship, int, int>(this.mem_ships, (Mem_ship key) => key.Rid, (Mem_ship value) => 0);
			}
			<getShipExp>c__AnonStorey4DE.base_exp = this.mst_mission.Win_exp_ship * (this.randInstance.Next(2) + 1);
			Exec_MissionResult.<getShipExp>c__AnonStorey4DE arg_BC_0 = <getShipExp>c__AnonStorey4DE;
			Dictionary<MissionResultKinds, double> dictionary = new Dictionary<MissionResultKinds, double>();
			dictionary.Add(MissionResultKinds.FAILE, 1.0);
			dictionary.Add(MissionResultKinds.SUCCESS, 1.0);
			dictionary.Add(MissionResultKinds.GREAT, 2.0);
			arg_BC_0.keisu = dictionary;
			double num = 1.5;
			<getShipExp>c__AnonStorey4DE.ret_exp = new Dictionary<int, int>(6);
			<getShipExp>c__AnonStorey4DE.flagLevelCheckValue = this.getFlagShipLevelCheckValue();
			<getShipExp>c__AnonStorey4DE.flagshipRid = this.mem_ships.get_Item(0).Rid;
			this.mem_ships.ForEach(delegate(Mem_ship x)
			{
				int num2 = (int)Math.Floor((double)<getShipExp>c__AnonStorey4DE.base_exp * <getShipExp>c__AnonStorey4DE.keisu.get_Item(<getShipExp>c__AnonStorey4DE.<>f__this.resultInfo.MissionResult));
				if (<getShipExp>c__AnonStorey4DE.<>f__this.mst_mission.Mission_type == MissionType.Practice)
				{
					<getShipExp>c__AnonStorey4DE.<>f__this.setPracticeTypeShipExp(ref num2, x.Level, <getShipExp>c__AnonStorey4DE.flagLevelCheckValue, x.Rid == <getShipExp>c__AnonStorey4DE.flagshipRid);
				}
				<getShipExp>c__AnonStorey4DE.ret_exp.Add(x.Rid, num2);
			});
			<getShipExp>c__AnonStorey4DE.ret_exp.set_Item(<getShipExp>c__AnonStorey4DE.flagshipRid, (int)Math.Floor((double)<getShipExp>c__AnonStorey4DE.ret_exp.get_Item(<getShipExp>c__AnonStorey4DE.flagshipRid) * num));
			return <getShipExp>c__AnonStorey4DE.ret_exp;
		}

		private void setPracticeTypeShipExp(ref int getExp, int nowLevel, int flagLevelCheckValue, bool flagShip)
		{
			if (!flagShip)
			{
				getExp += flagLevelCheckValue * 10;
			}
			if (nowLevel <= 9)
			{
				getExp = (int)((double)getExp * 1.5);
			}
			else if (nowLevel >= 10 && nowLevel <= 19)
			{
				getExp = (int)((double)getExp * 1.25);
			}
			else if (nowLevel >= 20 && nowLevel <= 29)
			{
				getExp = (int)((double)getExp * 1.1);
			}
			else if (nowLevel >= 70)
			{
				getExp = (int)((double)getExp * 0.9);
			}
		}

		private void updateShip(Dictionary<int, int> shipExp, out Dictionary<int, List<int>> lvupInfo)
		{
			lvupInfo = new Dictionary<int, List<int>>();
			int count = this.mem_ships.get_Count();
			for (int i = 0; i < count; i++)
			{
				this.mem_ships.get_Item(i).SetSubFuel_ToMission(this.mst_mission.Use_fuel);
				this.mem_ships.get_Item(i).SetSubBull_ToMission(this.mst_mission.Use_bull);
				Mem_shipBase mem_shipBase = new Mem_shipBase(this.mem_ships.get_Item(i));
				Mst_ship mst_data = Mst_DataManager.Instance.Mst_ship.get_Item(mem_shipBase.Ship_id);
				List<int> list = null;
				int num = shipExp.get_Item(this.mem_ships.get_Item(i).Rid);
				int levelupInfo = this.mem_ships.get_Item(i).getLevelupInfo(this.mst_level, this.mem_ships.get_Item(i).Level, this.mem_ships.get_Item(i).Exp, ref num, out list);
				lvupInfo.Add(this.mem_ships.get_Item(i).Rid, list);
				mem_shipBase.Level = levelupInfo;
				mem_shipBase.Exp += num;
				int num2 = levelupInfo - this.mem_ships.get_Item(i).Level;
				Dictionary<Mem_ship.enumKyoukaIdx, int> dictionary = this.mem_ships.get_Item(i).Kyouka;
				for (int j = 0; j < num2; j++)
				{
					dictionary = this.mem_ships.get_Item(i).getLevelupKyoukaValue(this.mem_ships.get_Item(i).Ship_id, dictionary);
				}
				mem_shipBase.SetKyoukaValue(dictionary);
				this.setCondSubValue(ref mem_shipBase.Cond);
				int num3 = 0;
				int num4 = 0;
				this.mst_level.TryGetValue(mem_shipBase.Level - 1, ref num3);
				this.mst_level.TryGetValue(mem_shipBase.Level + 1, ref num4);
				this.mem_ships.get_Item(i).SetRequireExp(mem_shipBase.Level, this.mst_level);
				this.mem_ships.get_Item(i).Set_ShipParam(mem_shipBase, mst_data, false);
				this.mem_ships.get_Item(i).SumLovToMission(this.resultInfo.MissionResult);
			}
		}

		private void setCondSubValue(ref int cond)
		{
			if (this.mem_deck.MissionState == MissionStates.STOP)
			{
				return;
			}
			int num = 3;
			if (Mem_ship.Get_FatitgueState(cond) >= FatigueState.Distress)
			{
				num += 6;
			}
			cond -= num;
			if (0 > cond)
			{
				cond = 0;
			}
		}

		private void updateMaterial(Dictionary<enumMaterialCategory, int> up_mat)
		{
			if (this.mem_deck.MissionState == MissionStates.STOP)
			{
				return;
			}
			if (up_mat == null)
			{
				return;
			}
			using (Dictionary<enumMaterialCategory, int>.Enumerator enumerator = up_mat.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<enumMaterialCategory, int> current = enumerator.get_Current();
					Comm_UserDatas.Instance.User_material.get_Item(current.get_Key()).Add_Material(current.get_Value());
				}
			}
		}

		private void updateDeck()
		{
			this.mem_deck.MissionInit();
		}

		private void updateItem()
		{
			if (this.resultInfo.GetItems == null)
			{
				return;
			}
			this.resultInfo.GetItems.ForEach(delegate(ItemGetFmt x)
			{
				if (x.Category == ItemGetKinds.UseItem && x.Id > 4)
				{
					Comm_UserDatas.Instance.Add_Useitem(x.Id, x.Count);
				}
			});
		}

		private void updateMissionComp()
		{
			if (this.resultInfo.MissionResult == MissionResultKinds.FAILE)
			{
				return;
			}
			Mem_missioncomp mem_missioncomp = new Mem_missioncomp(this.mst_mission.Id, this.mst_mission.Maparea_id, MissionClearKinds.CLEARED);
			mem_missioncomp.Update();
		}

		private int updateRecord()
		{
			int result = Comm_UserDatas.Instance.User_record.UpdateExp(this.resultInfo.GetMemberExp, this.mst_userlevel);
			Comm_UserDatas.Instance.User_record.UpdateMissionCount(this.resultInfo.MissionResult);
			return result;
		}

		private void updateBasic()
		{
			Comm_UserDatas.Instance.User_basic.AddPoint(this.resultInfo.GetSpoint);
		}

		private void updateRebellionPoint(MissionResultKinds kind)
		{
			int num = 0;
			if (kind == MissionResultKinds.SUCCESS)
			{
				num = 4;
			}
			else if (kind == MissionResultKinds.GREAT)
			{
				num = 6;
			}
			int subNum = this.mst_mission.Rp_sub * num;
			((IRebellionPointOperator)this).SubRebellionPoint(this.mst_mission.Maparea_id, subNum);
		}
	}
}
