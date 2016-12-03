using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Controllers.BattleLogic;
using Server_Controllers.QuestLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public class Api_req_SortieBattle : BattleControllerBase
	{
		public Api_req_SortieBattle(Api_req_Map instance)
		{
			this.mapInstance = instance;
			this.init();
		}

		protected override void init()
		{
			this.practiceFlag = false;
			this.mapInstance.GetBattleShipData(out this.userData, out this.enemyData);
			base.setBattleSubInfo(this.userData, out this.userSubInfo);
			base.setBattleSubInfo(this.enemyData, out this.enemySubInfo);
			this.battleKinds = ExecBattleKinds.None;
			this.battleCommandParams = new BattleCommandParams(this.userData);
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		public override Api_Result<AllBattleFmt> GetDayPreBattleInfo(BattleFormationKinds1 formationKind)
		{
			Mst_mapcell2 nowCell = this.mapInstance.GetNowCell();
			if (nowCell.Event_2 == enumMapWarType.Midnight)
			{
				return new Api_Result<AllBattleFmt>
				{
					state = Api_Result_State.Parameter_Error
				};
			}
			Api_Result<AllBattleFmt> dayPreBattleInfo = base.GetDayPreBattleInfo(formationKind);
			if (dayPreBattleInfo.state == Api_Result_State.Success)
			{
				Dictionary<int, List<Mst_slotitem>> useRationShips = null;
				List<int> list = null;
				if (this.getCombatRationResult(out useRationShips, out list))
				{
					dayPreBattleInfo.data.DayBattle.Header.UseRationShips = useRationShips;
				}
			}
			return dayPreBattleInfo;
		}

		public override Api_Result<AllBattleFmt> DayBattle()
		{
			Mst_mapcell2 nowCell = this.mapInstance.GetNowCell();
			if (nowCell.Event_2 != enumMapWarType.Normal)
			{
				return new Api_Result<AllBattleFmt>
				{
					state = Api_Result_State.Parameter_Error
				};
			}
			return base.DayBattle();
		}

		public Api_Result<AllBattleFmt> AirBattle()
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (this.userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mst_mapcell2 nowCell = this.mapInstance.GetNowCell();
			if (nowCell.Event_2 != enumMapWarType.AirBattle && this.battleKinds != ExecBattleKinds.None)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.allBattleFmt == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			AllBattleFmt allBattleFmt = this.allBattleFmt;
			allBattleFmt.DayBattle.OpeningProduction = this.battleCommandParams.GetProductionData(this.userData.GetDeckBattleCommand().get_Item(0));
			if (this.battleCommandParams.IsEscape)
			{
				allBattleFmt.DayBattle.ValidMidnight = base.isGoMidnight();
				this.battleKinds = ExecBattleKinds.DayOnly;
				api_Result.data = allBattleFmt;
				return api_Result;
			}
			using (Exec_AirBattle exec_AirBattle = new Exec_AirBattle(this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, allBattleFmt.DayBattle.Search, false))
			{
				allBattleFmt.DayBattle.AirBattle = exec_AirBattle.GetResultData(this.formationParams, this.battleCommandParams);
				this.seikuValue = exec_AirBattle.getSeikuValue();
			}
			allBattleFmt.BattleFormation = (this.userData.BattleFormation = (this.enemyData.BattleFormation = this.formationParams.AfterAirBattle_RewriteBattleFormation2(this.userData)));
			using (Exec_AirBattle exec_AirBattle2 = new Exec_AirBattle(this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, allBattleFmt.DayBattle.Search, false))
			{
				allBattleFmt.DayBattle.AirBattle2 = exec_AirBattle2.GetResultData(this.formationParams, this.battleCommandParams);
			}
			allBattleFmt.DayBattle.ValidMidnight = base.isGoMidnight();
			this.battleKinds = ExecBattleKinds.DayOnly;
			api_Result.data = allBattleFmt;
			return api_Result;
		}

		public override Api_Result<AllBattleFmt> NightBattle()
		{
			Mst_mapcell2 nowCell = this.mapInstance.GetNowCell();
			List<enumMapWarType> list = new List<enumMapWarType>();
			list.Add(enumMapWarType.Normal);
			list.Add(enumMapWarType.AirBattle);
			List<enumMapWarType> list2 = list;
			if (!list2.Contains(nowCell.Event_2))
			{
				return new Api_Result<AllBattleFmt>
				{
					state = Api_Result_State.Parameter_Error
				};
			}
			return base.NightBattle();
		}

		public Api_Result<AllBattleFmt> Night_Sp(BattleFormationKinds1 formationKind)
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (this.userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mst_mapcell2 nowCell = this.mapInstance.GetNowCell();
			if (nowCell.Event_2 != enumMapWarType.Midnight || this.battleKinds != ExecBattleKinds.None)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.userData.ShipData.get_Item(0).Get_DamageState() == DamageState.Taiha)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			base.initFormation(formationKind);
			AllBattleFmt allBattleFmt = new AllBattleFmt(this.userData.Formation, this.enemyData.Formation, this.userData.BattleFormation);
			Dictionary<int, List<Mst_slotitem>> useRationShips = null;
			List<int> list = null;
			if (!this.getCombatRationResult(out useRationShips, out list))
			{
				useRationShips = null;
			}
			using (Exec_Midnight exec_Midnight = new Exec_Midnight(2, this.seikuValue, this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, false))
			{
				allBattleFmt.NightBattle = exec_Midnight.GetResultData(this.formationParams, this.battleCommandParams);
				allBattleFmt.NightBattle.Header.UseRationShips = useRationShips;
			}
			this.battleKinds = ExecBattleKinds.NightOnly;
			api_Result.data = allBattleFmt;
			this.allBattleFmt = allBattleFmt;
			return api_Result;
		}

		public override Api_Result<BattleResultFmt> BattleResult()
		{
			Api_Result<BattleResultFmt> api_Result = base.BattleResult();
			if (api_Result.state == Api_Result_State.Parameter_Error)
			{
				return api_Result;
			}
			List<Mem_ship> ships = null;
			List<Mem_ship> list = null;
			this.mapInstance.GetSortieShipDatas(out ships, out list);
			EscapeInfo escapeInfo = new EscapeInfo(ships);
			api_Result.data.EscapeInfo = ((!escapeInfo.ValidEscape()) ? null : escapeInfo);
			if (api_Result.data.GetAirReconnaissanceItems != null)
			{
				this.mapInstance.updateMapitemGetData(api_Result.data.GetAirReconnaissanceItems);
			}
			this.battleKinds = ExecBattleKinds.None;
			Mst_mapcell2 nowCell = this.mapInstance.GetNowCell();
			bool boss = Mst_DataManager.Instance.Mst_mapenemy.get_Item(this.enemyData.Enemy_id).Boss == 1;
			List<int> list2 = new QuestSortie(nowCell.Maparea_id, nowCell.Mapinfo_no, api_Result.data.WinRank, this.userData.Deck.Rid, this.userData.ShipData, this.enemyData.ShipData, boss).ExecuteCheck();
			this.mapInstance.SetSlotExpChangeValues(this, base.GetSlotExpBattleData());
			return api_Result;
		}

		public override void GetBattleResultBase(BattleResultBase out_data)
		{
			base.GetBattleResultBase(out_data);
			out_data.Cleard = this.mapInstance.GetMapClearState();
			out_data.NowCell = this.mapInstance.GetNowCell();
			out_data.RebellionBattle = this.mapInstance.IsRebbelion;
			out_data.GetAirCellItems = this.mapInstance.AirReconnaissanceItems;
		}

		public bool GoBackPort(int escapeRid, int towRid)
		{
			Mem_ship mem_ship = null;
			Mem_ship mem_ship2 = null;
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(escapeRid, ref mem_ship))
			{
				return false;
			}
			if (!Comm_UserDatas.Instance.User_ship.TryGetValue(towRid, ref mem_ship2))
			{
				return false;
			}
			if (mem_ship.Escape_sts)
			{
				return false;
			}
			if (mem_ship2.Escape_sts)
			{
				return false;
			}
			mem_ship.ChangeEscapeState();
			mem_ship2.ChangeEscapeState();
			return true;
		}

		private bool getCombatRationResult(out Dictionary<int, List<Mst_slotitem>> useShipInfo, out List<int> givenShips)
		{
			int mapBattleCellPassCount = this.mapInstance.MapBattleCellPassCount;
			useShipInfo = null;
			givenShips = null;
			if (mapBattleCellPassCount < 2)
			{
				return false;
			}
			Dictionary<int, List<Mst_slotitem>> dictionary = new Dictionary<int, List<Mst_slotitem>>();
			List<int> list = new List<int>();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(145);
			hashSet.Add(150);
			HashSet<int> searchIds = hashSet;
			Dictionary<int, List<int>> dictionary2 = new Dictionary<int, List<int>>();
			Dictionary<int, int> dictionary3 = Enumerable.ToDictionary<int, int, int>(this.userSubInfo.get_Keys(), (int key) => key, (int value) => 0);
			for (int i = 0; i < this.userData.ShipData.get_Count(); i++)
			{
				Mem_ship mem_ship = this.userData.ShipData.get_Item(i);
				if (mem_ship.IsFight())
				{
					Dictionary<int, List<int>> slotIndexFromId = mem_ship.GetSlotIndexFromId(searchIds);
					if (slotIndexFromId.get_Item(145).get_Count() != 0 || slotIndexFromId.get_Item(150).get_Count() != 0)
					{
						if (this.isRationLotteryWinning(mapBattleCellPassCount, mem_ship.Luck))
						{
							List<int> rationRecoveryShips = this.getRationRecoveryShips(i);
							if (rationRecoveryShips.get_Count() != 0)
							{
								dictionary.Add(mem_ship.Rid, new List<Mst_slotitem>());
								int num = 0;
								List<int> list2 = new List<int>();
								if (slotIndexFromId.get_Item(145).get_Count() > 0)
								{
									num++;
									int num2 = slotIndexFromId.get_Item(145).get_Count() - 1;
									list2.Add(slotIndexFromId.get_Item(145).get_Item(num2));
									dictionary.get_Item(mem_ship.Rid).Add(Mst_DataManager.Instance.Mst_Slotitem.get_Item(145));
								}
								if (slotIndexFromId.get_Item(150).get_Count() > 0)
								{
									num += 2;
									int num3 = slotIndexFromId.get_Item(150).get_Count() - 1;
									list2.Add(slotIndexFromId.get_Item(150).get_Item(num3));
									dictionary.get_Item(mem_ship.Rid).Add(Mst_DataManager.Instance.Mst_Slotitem.get_Item(150));
								}
								Dictionary<int, int> dictionary4;
								Dictionary<int, int> expr_23B = dictionary4 = dictionary3;
								int num4;
								int expr_245 = num4 = mem_ship.Rid;
								num4 = dictionary4.get_Item(num4);
								expr_23B.set_Item(expr_245, num4 + this.getCombatRationCondPlus(num, false));
								dictionary2.Add(i, list2);
								rationRecoveryShips.Remove(mem_ship.Rid);
								list.AddRange(rationRecoveryShips);
								using (List<int>.Enumerator enumerator = rationRecoveryShips.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										int current = enumerator.get_Current();
										Dictionary<int, int> dictionary5;
										Dictionary<int, int> expr_29F = dictionary5 = dictionary3;
										int expr_2A4 = num4 = current;
										num4 = dictionary5.get_Item(num4);
										expr_29F.set_Item(expr_2A4, num4 + this.getCombatRationCondPlus(num, true));
									}
								}
							}
						}
					}
				}
			}
			if (dictionary2.get_Count() == 0)
			{
				return false;
			}
			List<int> list3 = new List<int>();
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			using (Dictionary<int, List<int>>.Enumerator enumerator2 = dictionary2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, List<int>> current2 = enumerator2.get_Current();
					Mem_ship mem_ship2 = this.userData.ShipData.get_Item(current2.get_Key());
					Mem_shipBase mem_shipBase = new Mem_shipBase(mem_ship2);
					List<Mst_slotitem> list4 = this.userData.SlotData.get_Item(current2.get_Key());
					using (List<int>.Enumerator enumerator3 = current2.get_Value().GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							int current3 = enumerator3.get_Current();
							int num5;
							if (current3 != 2147483647)
							{
								num5 = mem_ship2.Slot.get_Item(current3);
								mem_shipBase.Slot.set_Item(current3, -1);
								list4.set_Item(current3, null);
							}
							else
							{
								num5 = mem_ship2.Exslot;
								mem_shipBase.Exslot = -1;
							}
							list3.Add(num5);
						}
					}
					mem_ship2.Set_ShipParam(mem_shipBase, mst_ship.get_Item(mem_shipBase.Ship_id), false);
					mem_ship2.TrimSlot();
					list4.RemoveAll((Mst_slotitem x) => x == null);
				}
			}
			using (Dictionary<int, int>.Enumerator enumerator4 = dictionary3.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					KeyValuePair<int, int> current4 = enumerator4.get_Current();
					Mem_ship shipInstance = this.userSubInfo.get_Item(current4.get_Key()).ShipInstance;
					int value2 = current4.get_Value();
					shipInstance.AddRationItemCond(value2);
				}
			}
			Comm_UserDatas.Instance.Remove_Slot(list3);
			useShipInfo = dictionary;
			givenShips = Enumerable.ToList<int>(Enumerable.Distinct<int>(list));
			return true;
		}

		private bool isRationLotteryWinning(int battleCount, int luck)
		{
			int num = 10;
			int num2 = (int)(Math.Sqrt((double)luck) * 2.0);
			int num3 = battleCount * 5;
			double num4 = (double)(num + num2 + num3);
			double num5 = (double)((int)Utils.GetRandDouble(0.0, 100.0, 1.0, 1));
			return num4 >= num5;
		}

		private List<int> getRationRecoveryShips(int useShipIndex)
		{
			List<int> list = new List<int>();
			list.Add(useShipIndex);
			list.Add(useShipIndex + 1);
			list.Add(useShipIndex - 1);
			List<int> list2 = list;
			List<Mem_ship> shipData = this.userData.ShipData;
			List<int> list3 = new List<int>();
			using (List<int>.Enumerator enumerator = list2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (current >= 0 && current <= shipData.get_Count() - 1)
					{
						if (shipData.get_Item(current).IsFight() && shipData.get_Item(current).Cond < 100)
						{
							list3.Add(shipData.get_Item(current).Rid);
						}
					}
				}
			}
			return list3;
		}

		private int getCombatRationCondPlus(int type, bool givenShip)
		{
			int result = 0;
			if (!givenShip)
			{
				if (type == 1)
				{
					result = 6 + (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1);
				}
				else if (type == 2)
				{
					result = 10 + (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1);
				}
				else if (type == 3)
				{
					result = 20 + (int)Utils.GetRandDouble(0.0, 8.0, 1.0, 1);
				}
			}
			else if (type == 1)
			{
				result = 2 + (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1);
			}
			else if (type == 2)
			{
				result = 8 + (int)Utils.GetRandDouble(0.0, 2.0, 1.0, 1);
			}
			else if (type == 3)
			{
				result = 16 + (int)Utils.GetRandDouble(0.0, 4.0, 1.0, 1);
			}
			return result;
		}
	}
}
