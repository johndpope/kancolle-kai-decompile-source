using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Controllers.BattleLogic;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers
{
	public abstract class BattleControllerBase : IDisposable, IMakeBattleData
	{
		protected bool practiceFlag;

		protected Api_req_Map mapInstance;

		protected BattleBaseData userData;

		protected Dictionary<int, BattleShipSubInfo> userSubInfo;

		protected BattleBaseData enemyData;

		protected Dictionary<int, BattleShipSubInfo> enemySubInfo;

		protected FormationDatas formationParams;

		protected BattleCommandParams battleCommandParams;

		protected ExecBattleKinds battleKinds;

		protected int[] seikuValue;

		protected AllBattleFmt allBattleFmt;

		protected HashSet<BattleCommand> enableBattleCommand;

		protected List<MiddleBattleCallInfo> callInfo = new List<MiddleBattleCallInfo>();

		protected HashSet<int> airSubstituteAttackRid;

		private bool enforceMotherFlag;

		protected abstract void init();

		public Dictionary<int, int> GetSlotExpBattleData()
		{
			if (this.userData.SlotExperience.get_Count() == 0)
			{
				return new Dictionary<int, int>();
			}
			return Enumerable.ToDictionary<KeyValuePair<int, int[]>, int, int>(this.userData.SlotExperience, (KeyValuePair<int, int[]> key) => key.get_Key(), (KeyValuePair<int, int[]> val) => val.get_Value()[1]);
		}

		public virtual void Dispose()
		{
			this.userData.Dispose();
			this.enemyData.Dispose();
			this.formationParams.Dispose();
			this.userSubInfo.Clear();
			this.enemySubInfo.Clear();
			this.battleCommandParams.Dispose();
			this.enableBattleCommand.Clear();
			this.callInfo.Clear();
			this.airSubstituteAttackRid.Clear();
		}

		public int GetBattleCommand(out List<BattleCommand> command)
		{
			command = null;
			if (this.userData == null)
			{
				return 0;
			}
			Mem_ship mem_ship = this.userData.ShipData.get_Item(0);
			command = mem_ship.GetBattleCommad();
			if (!this.validBattleCommand(command))
			{
				mem_ship.PurgeBattleCommand();
				command = mem_ship.GetBattleCommad();
			}
			bool flag = Enumerable.Any<Mem_ship>(this.userData.ShipData, (Mem_ship x) => Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsMother() && x.IsFight());
			this.enforceMotherFlag = false;
			if (flag)
			{
				Mem_ship arg_8F_0 = mem_ship;
				List<BattleCommand> list = new List<BattleCommand>();
				list.Add(BattleCommand.Kouku);
				arg_8F_0.SetBattleCommand(list);
				command = mem_ship.GetBattleCommad();
				this.enforceMotherFlag = true;
			}
			return mem_ship.GetBattleCommandEnableNum();
		}

		public HashSet<BattleCommand> GetEnableBattleCommand()
		{
			if (this.enableBattleCommand != null)
			{
				return this.enableBattleCommand;
			}
			Mem_ship mem_ship = this.userData.ShipData.get_Item(0);
			HashSet<BattleCommand> hashSet = new HashSet<BattleCommand>();
			hashSet.Add(BattleCommand.Sekkin);
			hashSet.Add(BattleCommand.Hougeki);
			hashSet.Add(BattleCommand.Raigeki);
			hashSet.Add(BattleCommand.Ridatu);
			hashSet.Add(BattleCommand.Taisen);
			hashSet.Add(BattleCommand.Kaihi);
			HashSet<BattleCommand> hashSet2 = hashSet;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			List<bool> list = new List<bool>();
			List<Mem_ship> list2 = Enumerable.ToList<Mem_ship>(Enumerable.Take<Mem_ship>(Enumerable.Where<Mem_ship>(this.userData.ShipData, (Mem_ship x) => !x.Escape_sts), 3));
			for (int i = 0; i < this.userData.ShipData.get_Count(); i++)
			{
				Mem_ship mem_ship2 = this.userData.ShipData.get_Item(i);
				if (mem_ship2.Escape_sts)
				{
					num3++;
				}
				else
				{
					if (mem_ship2.Level >= 10)
					{
						num++;
					}
					if (mem_ship2.Level >= 25)
					{
						num2++;
					}
					if (list2.Contains(mem_ship2))
					{
						List<Mst_slotitem> list3 = this.userData.SlotData.get_Item(i);
						if (Enumerable.Any<Mst_slotitem>(list3, (Mst_slotitem x) => x.IsDentan()))
						{
							list.Add(true);
						}
					}
					if (Mst_DataManager.Instance.Mst_stype.get_Item(mem_ship2.Stype).IsKouku())
					{
						hashSet2.Add(BattleCommand.Kouku);
					}
				}
			}
			int num4 = this.userData.ShipData.get_Count() - num3;
			if (mem_ship.Level >= 20 && num4 == num)
			{
				hashSet2.Add(BattleCommand.Totugeki);
			}
			if (mem_ship.Level >= 50 && num4 == num2 && list.get_Count() == 3)
			{
				hashSet2.Add(BattleCommand.Tousha);
			}
			this.enableBattleCommand = hashSet2;
			return hashSet2;
		}

		public bool ValidBattleCommand(List<BattleCommand> command)
		{
			if (command == null || this.userData == null)
			{
				return false;
			}
			HashSet<BattleCommand> items = this.GetEnableBattleCommand();
			int battleCommandEnableNum = this.userData.ShipData.get_Item(0).GetBattleCommandEnableNum();
			IEnumerable<BattleCommand> enumerable = Enumerable.Skip<BattleCommand>(command, battleCommandEnableNum);
			if (!Enumerable.All<BattleCommand>(enumerable, (BattleCommand x) => x == BattleCommand.None))
			{
				return false;
			}
			List<BattleCommand> list = Enumerable.ToList<BattleCommand>(Enumerable.Take<BattleCommand>(command, battleCommandEnableNum));
			if (this.enforceMotherFlag)
			{
				list.RemoveAt(0);
			}
			return Enumerable.All<BattleCommand>(list, (BattleCommand x) => items.Contains(x));
		}

		private bool validBattleCommand(List<BattleCommand> command)
		{
			HashSet<BattleCommand> e_items = this.GetEnableBattleCommand();
			IEnumerable<BattleCommand> enumerable = Enumerable.TakeWhile<BattleCommand>(command, (BattleCommand x) => x != BattleCommand.None);
			return Enumerable.All<BattleCommand>(enumerable, (BattleCommand x) => e_items.Contains(x));
		}

		public void SetBattleCommand(List<BattleCommand> command)
		{
			if (!this.ValidBattleCommand(command))
			{
				return;
			}
			this.userData.ShipData.get_Item(0).SetBattleCommand(command);
		}

		public virtual Api_Result<AllBattleFmt> GetDayPreBattleInfo(BattleFormationKinds1 formationKind)
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (this.userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			this.airSubstituteAttackRid = new HashSet<int>();
			if (this.battleKinds != ExecBattleKinds.None)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.userData.ShipData.get_Item(0).Get_DamageState() == DamageState.Taiha)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			this.initFormation(formationKind);
			AllBattleFmt allBattleFmt = new AllBattleFmt(this.userData.Formation, this.enemyData.Formation, this.userData.BattleFormation);
			allBattleFmt.DayBattle = new DayBattleFmt(this.userData.Deck.Rid, this.userData.ShipData, this.enemyData.ShipData);
			using (Exec_Search exec_Search = new Exec_Search(this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, this.practiceFlag))
			{
				allBattleFmt.DayBattle.Search = exec_Search.GetResultData(this.formationParams, this.battleCommandParams);
			}
			api_Result.data = allBattleFmt;
			this.allBattleFmt = allBattleFmt;
			return api_Result;
		}

		public virtual Api_Result<AllBattleFmt> DayBattle()
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (this.allBattleFmt == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.battleKinds != ExecBattleKinds.None)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			AllBattleFmt allBattleFmt = this.allBattleFmt;
			allBattleFmt.DayBattle.OpeningProduction = this.battleCommandParams.GetProductionData(this.userData.GetDeckBattleCommand().get_Item(0));
			if (this.battleCommandParams.IsEscape)
			{
				allBattleFmt.DayBattle.ValidMidnight = this.isGoMidnight();
				this.battleKinds = ExecBattleKinds.DayOnly;
				api_Result.data = allBattleFmt;
				return api_Result;
			}
			using (Exec_AirBattle exec_AirBattle = new Exec_AirBattle(this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, allBattleFmt.DayBattle.Search, this.practiceFlag))
			{
				allBattleFmt.DayBattle.AirBattle = exec_AirBattle.GetResultData(this.formationParams, this.battleCommandParams);
				this.seikuValue = exec_AirBattle.getSeikuValue();
			}
			allBattleFmt.BattleFormation = (this.userData.BattleFormation = (this.enemyData.BattleFormation = this.formationParams.AfterAirBattle_RewriteBattleFormation2(this.userData)));
			if (!this.practiceFlag)
			{
				Mst_mapcell2 nowCell = this.mapInstance.GetNowCell();
				using (Exec_SupportAttack exec_SupportAttack = new Exec_SupportAttack(this.enemyData, this.enemySubInfo, allBattleFmt.DayBattle.Search, this.mapInstance.Mst_SupportData))
				{
					if (this.mapInstance.IsRebbelion)
					{
						exec_SupportAttack.SelectSupportDeck(this.mapInstance.Support_decks, false);
					}
					else
					{
						exec_SupportAttack.SelectSupportDeck(this.mapInstance.Support_decks);
					}
					allBattleFmt.DayBattle.SupportAtack = exec_SupportAttack.GetResultData(this.formationParams, new BattleCommandParams(this.userData));
				}
			}
			using (Exec_OpeningAtack exec_OpeningAtack = new Exec_OpeningAtack(this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, this.practiceFlag))
			{
				allBattleFmt.DayBattle.OpeningAtack = exec_OpeningAtack.GetResultData(this.formationParams, this.battleCommandParams);
				if (exec_OpeningAtack.CanRaigType() == 1 && exec_OpeningAtack.getUserAttackShipNum() == 0)
				{
					allBattleFmt.DayBattle.OpeningProduction = this.battleCommandParams.GetProductionData(0, BattleCommand.Raigeki);
				}
			}
			List<BattleCommand> deckBattleCommand = this.userData.GetDeckBattleCommand();
			List<MiddleBattleCallInfo> list = this.getMiddleBattleCallType(deckBattleCommand);
			if (deckBattleCommand.get_Item(0) == BattleCommand.Kouku || deckBattleCommand.get_Item(0) == BattleCommand.Raigeki)
			{
				MiddleBattleCallInfo middleBattleCallInfo = list.get_Item(0);
				if (middleBattleCallInfo.BattleType == MiddleBattleCallInfo.CallType.None && middleBattleCallInfo.CommandPos != -1)
				{
					MiddleBattleCallInfo middleBattleCallInfo2 = Enumerable.FirstOrDefault<MiddleBattleCallInfo>(list, (MiddleBattleCallInfo x) => x.BattleType == MiddleBattleCallInfo.CallType.LastRaig);
					IEnumerable<MiddleBattleCallInfo> enumerable = Enumerable.TakeWhile<MiddleBattleCallInfo>(list, (MiddleBattleCallInfo x) => x.BattleType != MiddleBattleCallInfo.CallType.LastRaig);
					List<MiddleBattleCallInfo> list2 = Enumerable.ToList<MiddleBattleCallInfo>(Enumerable.Where<MiddleBattleCallInfo>(enumerable, (MiddleBattleCallInfo x) => x.CommandPos != -1 && x.BattleType != MiddleBattleCallInfo.CallType.None));
					MiddleBattleCallInfo middleBattleCallInfo3 = list2.get_Item(0);
					if (middleBattleCallInfo3.BattleType == MiddleBattleCallInfo.CallType.Houg && middleBattleCallInfo3.AttackType == 3)
					{
						middleBattleCallInfo3.AttackType = 1;
						MiddleBattleCallInfo middleBattleCallInfo4 = new MiddleBattleCallInfo(middleBattleCallInfo3.CommandPos, middleBattleCallInfo3.UseCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						list2.Insert(1, middleBattleCallInfo4);
					}
					List<MiddleBattleCallInfo> list3 = Enumerable.ToList<MiddleBattleCallInfo>(Enumerable.Where<MiddleBattleCallInfo>(enumerable, (MiddleBattleCallInfo x) => x.CommandPos == -1));
					List<MiddleBattleCallInfo> list4 = new List<MiddleBattleCallInfo>();
					int num = (list2.get_Count() <= list3.get_Count()) ? list3.get_Count() : list2.get_Count();
					for (int i = 0; i < num; i++)
					{
						MiddleBattleCallInfo middleBattleCallInfo5;
						if (i < list2.get_Count())
						{
							middleBattleCallInfo5 = list2.get_Item(i);
						}
						else
						{
							middleBattleCallInfo5 = new MiddleBattleCallInfo(100, BattleCommand.None, MiddleBattleCallInfo.CallType.None, 0);
						}
						MiddleBattleCallInfo middleBattleCallInfo6;
						if (i < list3.get_Count())
						{
							middleBattleCallInfo6 = list3.get_Item(i);
						}
						else
						{
							middleBattleCallInfo6 = new MiddleBattleCallInfo(-1, BattleCommand.None, MiddleBattleCallInfo.CallType.None, 0);
						}
						list4.Add(middleBattleCallInfo5);
						list4.Add(middleBattleCallInfo6);
					}
					list = list4;
					if (middleBattleCallInfo2 != null)
					{
						list.Add(middleBattleCallInfo2);
					}
				}
			}
			bool flag = false;
			allBattleFmt.DayBattle.FromMiddleDayBattle = new List<FromMiddleBattleDayData>();
			bool flag2 = false;
			bool flag3 = false;
			FromMiddleBattleDayData fromMiddleBattleDayData = null;
			using (List<MiddleBattleCallInfo>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					MiddleBattleCallInfo current = enumerator.get_Current();
					if (!flag2 && !flag3)
					{
						fromMiddleBattleDayData = new FromMiddleBattleDayData();
					}
					IBattleType battleType = null;
					if (current.BattleType == MiddleBattleCallInfo.CallType.EffectOnly && this.isGoMidnight())
					{
						fromMiddleBattleDayData.Production = this.battleCommandParams.GetProductionData(current.CommandPos, current.UseCommand);
						if (this.battleCommandParams.IsEscape)
						{
							allBattleFmt.DayBattle.ValidMidnight = this.isGoMidnight();
							this.battleKinds = ExecBattleKinds.DayOnly;
							api_Result.data = allBattleFmt;
							allBattleFmt.DayBattle.FromMiddleDayBattle.Add(fromMiddleBattleDayData);
							return api_Result;
						}
					}
					else if (current.BattleType == MiddleBattleCallInfo.CallType.Houg)
					{
						if ((current.UseCommand == BattleCommand.Tousha || current.UseCommand == BattleCommand.Totugeki) && this.isGoMidnight())
						{
							fromMiddleBattleDayData.Production = this.battleCommandParams.GetProductionData(current.CommandPos, current.UseCommand);
						}
						using (Exec_Hougeki exec_Hougeki = new Exec_Hougeki(current, this.seikuValue, this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, this.practiceFlag))
						{
							exec_Hougeki.SetAirSubstituteAttacker(this.airSubstituteAttackRid);
							HougekiDayBattleFmt resultData = exec_Hougeki.GetResultData(this.formationParams, this.battleCommandParams);
							battleType = resultData;
						}
					}
					else if (current.BattleType == MiddleBattleCallInfo.CallType.Raig)
					{
						using (Exec_Raigeki exec_Raigeki = new Exec_Raigeki(1, this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, this.practiceFlag))
						{
							Raigeki resultData2 = exec_Raigeki.GetResultData(this.formationParams, this.battleCommandParams);
							battleType = resultData2;
						}
					}
					else if (current.BattleType == MiddleBattleCallInfo.CallType.LastRaig)
					{
						flag = true;
						break;
					}
					if (current.CommandPos != -1)
					{
						fromMiddleBattleDayData.F_BattleData = battleType;
						flag2 = true;
					}
					else
					{
						fromMiddleBattleDayData.E_BattleData = battleType;
						flag3 = true;
					}
					if (flag2 && flag3)
					{
						flag2 = false;
						flag3 = false;
						allBattleFmt.DayBattle.FromMiddleDayBattle.Add(fromMiddleBattleDayData);
					}
				}
			}
			if (allBattleFmt.DayBattle.FromMiddleDayBattle.get_Count() == 0)
			{
				allBattleFmt.DayBattle.FromMiddleDayBattle = null;
			}
			int atkType = 2;
			if (flag)
			{
				atkType = 3;
			}
			using (Exec_Raigeki exec_Raigeki2 = new Exec_Raigeki(atkType, this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, this.practiceFlag))
			{
				allBattleFmt.DayBattle.Raigeki = exec_Raigeki2.GetResultData(this.formationParams, this.battleCommandParams);
			}
			allBattleFmt.DayBattle.ValidMidnight = this.isGoMidnight();
			this.battleKinds = ExecBattleKinds.DayOnly;
			api_Result.data = allBattleFmt;
			return api_Result;
		}

		protected bool isGoMidnight()
		{
			Mem_ship mem_ship = Enumerable.FirstOrDefault<Mem_ship>(this.userData.ShipData, (Mem_ship x) => x.IsFight());
			Mem_ship mem_ship2 = Enumerable.FirstOrDefault<Mem_ship>(this.enemyData.ShipData, (Mem_ship y) => y.IsFight());
			return mem_ship != null && mem_ship2 != null;
		}

		public virtual Api_Result<AllBattleFmt> NightBattle()
		{
			Api_Result<AllBattleFmt> api_Result = new Api_Result<AllBattleFmt>();
			if (this.userData == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			if (this.battleKinds != ExecBattleKinds.DayOnly)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			Mem_ship mem_ship = Enumerable.FirstOrDefault<Mem_ship>(this.userData.ShipData, (Mem_ship x) => x.IsFight());
			Mem_ship mem_ship2 = Enumerable.FirstOrDefault<Mem_ship>(this.enemyData.ShipData, (Mem_ship y) => y.IsFight());
			if (mem_ship == null && mem_ship2 == null)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
				return api_Result;
			}
			AllBattleFmt allBattleFmt = new AllBattleFmt(this.userData.Formation, this.enemyData.Formation, this.userData.BattleFormation);
			using (Exec_Midnight exec_Midnight = new Exec_Midnight(1, this.seikuValue, this.userData, this.userSubInfo, this.enemyData, this.enemySubInfo, this.practiceFlag))
			{
				allBattleFmt.NightBattle = exec_Midnight.GetResultData(this.formationParams, this.battleCommandParams);
			}
			this.battleKinds = ExecBattleKinds.DayToNight;
			api_Result.data = allBattleFmt;
			this.allBattleFmt = allBattleFmt;
			return api_Result;
		}

		public virtual Api_Result<BattleResultFmt> BattleResult()
		{
			Api_Result<BattleResultFmt> api_Result = new Api_Result<BattleResultFmt>();
			if (this.battleKinds == ExecBattleKinds.None)
			{
				api_Result.state = Api_Result_State.Parameter_Error;
			}
			using (Exec_BattleResult exec_BattleResult = new Exec_BattleResult(new BattleResultBase(this)))
			{
				api_Result.data = exec_BattleResult.GetResultData(this.formationParams, this.battleCommandParams);
			}
			this.battleKinds = ExecBattleKinds.None;
			return api_Result;
		}

		public virtual void GetBattleResultBase(BattleResultBase out_data)
		{
			out_data.MyData = this.userData;
			out_data.EnemyData = this.enemyData;
			out_data.F_SubInfo = this.userSubInfo;
			out_data.E_SubInfo = this.enemySubInfo;
			out_data.ExecKinds = this.battleKinds;
			out_data.PracticeFlag = this.practiceFlag;
		}

		protected void setBattleSubInfo(BattleBaseData baseData, out Dictionary<int, BattleShipSubInfo> subInfo)
		{
			subInfo = new Dictionary<int, BattleShipSubInfo>();
			List<Mem_ship> shipData = baseData.ShipData;
			var enumerable = Enumerable.Select(shipData, (Mem_ship obj, int idx) => new
			{
				obj,
				idx
			});
			var lookup = Enumerable.ToLookup(Enumerable.OrderByDescending(enumerable, x => x.obj.Leng), gkey => gkey.obj.Leng);
			int num = 0;
			using (var enumerator = lookup.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					var orderedEnumerable = Enumerable.OrderBy(current, x => x.idx);
					using (var enumerator2 = orderedEnumerable.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							var current2 = enumerator2.get_Current();
							BattleShipSubInfo battleShipSubInfo = new BattleShipSubInfo(current2.idx, current2.obj, num);
							subInfo.Add(current2.obj.Rid, battleShipSubInfo);
							num++;
						}
					}
				}
			}
		}

		protected void initFormation(BattleFormationKinds1 userFormation)
		{
			this.userData.Formation = userFormation;
			this.formationParams = new FormationDatas(this.userData, this.enemyData, this.practiceFlag);
			this.userData.BattleFormation = this.formationParams.BattleFormation;
			this.enemyData.Formation = this.formationParams.E_Formation;
			this.enemyData.BattleFormation = this.formationParams.BattleFormation;
		}

		protected List<MiddleBattleCallInfo> getMiddleBattleCallType(List<BattleCommand> useCommands)
		{
			List<MiddleBattleCallInfo> list = new List<MiddleBattleCallInfo>();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			if (useCommands.get_Item(0) == BattleCommand.Sekkin)
			{
				num = 1;
			}
			if (useCommands.get_Item(0) == BattleCommand.Raigeki)
			{
				num3 = 1;
			}
			BattleCommand useCommand = BattleCommand.None;
			if (useCommands.get_Count() == 3)
			{
				BattleCommand battleCommand = useCommands.get_Item(0);
				MiddleBattleCallInfo middleBattleCallInfo;
				if (Exec_Hougeki.isHougCommand(battleCommand) && battleCommand != BattleCommand.Kouku)
				{
					middleBattleCallInfo = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					num2 = 1;
				}
				else
				{
					middleBattleCallInfo = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.None, 0);
				}
				list.Add(middleBattleCallInfo);
				MiddleBattleCallInfo middleBattleCallInfo2 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1);
				list.Add(middleBattleCallInfo2);
				MiddleBattleCallInfo middleBattleCallInfo3 = null;
				battleCommand = useCommands.get_Item(1);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (useCommands.get_Item(2) == BattleCommand.Kouku)
					{
						middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else if (useCommands.get_Item(2) != BattleCommand.Kouku)
					{
						middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 1)
					{
						middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num2 = 2;
					}
					else
					{
						middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num2 = 2;
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num == 1 && (useCommands.get_Item(2) == BattleCommand.Ridatu || useCommands.get_Item(2) == BattleCommand.Kaihi))
					{
						middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
						num3 = 2;
					}
					else
					{
						num++;
						middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo3 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo3);
				MiddleBattleCallInfo middleBattleCallInfo4 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2);
				list.Add(middleBattleCallInfo4);
				MiddleBattleCallInfo middleBattleCallInfo5 = null;
				battleCommand = useCommands.get_Item(2);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (num4 == 1)
					{
						middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else
					{
						middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 2)
					{
						middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					}
					else if (num2 == 1)
					{
						middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else
					{
						middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num3 <= 1)
					{
						middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.LastRaig, 3);
					}
					else
					{
						middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo5 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo5);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.None, 0));
			}
			else if (useCommands.get_Count() == 4)
			{
				BattleCommand battleCommand = useCommands.get_Item(0);
				MiddleBattleCallInfo middleBattleCallInfo6;
				if (Exec_Hougeki.isHougCommand(battleCommand) && battleCommand != BattleCommand.Kouku)
				{
					middleBattleCallInfo6 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					num2 = 1;
				}
				else
				{
					middleBattleCallInfo6 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.None, 0);
				}
				list.Add(middleBattleCallInfo6);
				MiddleBattleCallInfo middleBattleCallInfo7 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1);
				list.Add(middleBattleCallInfo7);
				MiddleBattleCallInfo middleBattleCallInfo8 = null;
				battleCommand = useCommands.get_Item(1);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (useCommands.get_Item(2) == BattleCommand.Kouku)
					{
						middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					if (useCommands.get_Item(2) != BattleCommand.Kouku)
					{
						middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 1)
					{
						middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num2 = 2;
					}
					else
					{
						middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num2 = 2;
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num == 1 && (useCommands.get_Item(2) == BattleCommand.Ridatu || useCommands.get_Item(2) == BattleCommand.Kaihi))
					{
						middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
						num3 = 2;
					}
					else
					{
						num++;
						middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo8 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo8);
				MiddleBattleCallInfo middleBattleCallInfo9 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2);
				list.Add(middleBattleCallInfo9);
				MiddleBattleCallInfo middleBattleCallInfo10 = null;
				battleCommand = useCommands.get_Item(2);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (num4 == 1 && useCommands.get_Item(3) == BattleCommand.Kouku)
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands.get_Item(3) != BattleCommand.Kouku)
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 0 && useCommands.get_Item(3) == BattleCommand.Kouku)
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else if (num4 == 0 && useCommands.get_Item(3) != BattleCommand.Kouku)
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 2)
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num2 = 3;
					}
					else if (num2 == 1)
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num2 = 2;
					}
					else
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num2 = 2;
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num >= 1 && num3 <= 1)
					{
						if (useCommands.get_Item(3) == BattleCommand.Kaihi || useCommands.get_Item(3) == BattleCommand.Ridatu)
						{
							middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
							num3 = 3;
						}
						else
						{
							middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							num++;
						}
					}
					else
					{
						middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						num++;
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo10 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo10);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1));
				MiddleBattleCallInfo middleBattleCallInfo11 = null;
				battleCommand = useCommands.get_Item(3);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (num4 == 2)
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
					else if (num4 == 1)
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 3)
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else if (num2 == 2)
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					}
					else if (num2 == 1)
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num3 <= 1)
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.LastRaig, 3);
					}
					else
					{
						middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo11 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo11);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.None, 0));
			}
			else if (useCommands.get_Count() == 5)
			{
				BattleCommand battleCommand = useCommands.get_Item(0);
				MiddleBattleCallInfo middleBattleCallInfo12;
				if (Exec_Hougeki.isHougCommand(battleCommand) && battleCommand != BattleCommand.Kouku)
				{
					middleBattleCallInfo12 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					num2 = 1;
				}
				else
				{
					middleBattleCallInfo12 = new MiddleBattleCallInfo(0, battleCommand, MiddleBattleCallInfo.CallType.None, 0);
				}
				list.Add(middleBattleCallInfo12);
				MiddleBattleCallInfo middleBattleCallInfo13 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1);
				list.Add(middleBattleCallInfo13);
				MiddleBattleCallInfo middleBattleCallInfo14 = null;
				battleCommand = useCommands.get_Item(1);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (useCommands.get_Item(2) == BattleCommand.Kouku)
					{
						middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else
					{
						middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 1)
					{
						middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num2 = 2;
					}
					else
					{
						middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num2 = 2;
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num == 1 && (useCommands.get_Item(2) == BattleCommand.Ridatu || useCommands.get_Item(2) == BattleCommand.Kaihi))
					{
						middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
						num3 = 2;
					}
					else
					{
						num++;
						middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo14 = new MiddleBattleCallInfo(1, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo14);
				MiddleBattleCallInfo middleBattleCallInfo15 = new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2);
				list.Add(middleBattleCallInfo15);
				MiddleBattleCallInfo middleBattleCallInfo16 = null;
				battleCommand = useCommands.get_Item(2);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (num4 == 1 && useCommands.get_Item(3) == BattleCommand.Kouku)
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands.get_Item(3) != BattleCommand.Kouku)
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 0 && useCommands.get_Item(3) == BattleCommand.Kouku)
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 2)
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num2 = 3;
					}
					else if (num2 == 1)
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num2 = 2;
					}
					else
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num2 = 2;
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num >= 1 && num3 <= 1)
					{
						if (useCommands.get_Item(3) == BattleCommand.Kaihi || useCommands.get_Item(3) == BattleCommand.Ridatu)
						{
							middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
							num3 = 3;
						}
						else
						{
							middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							num++;
						}
					}
					else
					{
						middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						num++;
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo16 = new MiddleBattleCallInfo(2, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo16);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 1));
				MiddleBattleCallInfo middleBattleCallInfo17 = null;
				battleCommand = useCommands.get_Item(3);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (num4 == 0 && useCommands.get_Item(4) == BattleCommand.Kouku)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 1;
					}
					else if (num4 == 0 && useCommands.get_Item(4) != BattleCommand.Kouku)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands.get_Item(4) == BattleCommand.Kouku)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 1 && useCommands.get_Item(4) != BattleCommand.Kouku)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num4 = 2;
					}
					else if (num4 == 2 && useCommands.get_Item(4) == BattleCommand.Kouku)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num4 = 3;
					}
					else if (num4 == 2 && useCommands.get_Item(4) != BattleCommand.Kouku)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num4 = 4;
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 3)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num2 = 4;
					}
					else if (num2 == 2)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
						num2 = 3;
					}
					else if (num2 == 1)
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
						num2 = 2;
					}
					else
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
						num2 = 4;
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num >= 1 && num3 <= 1)
					{
						if (useCommands.get_Item(4) == BattleCommand.Kaihi || useCommands.get_Item(4) == BattleCommand.Ridatu)
						{
							middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.Raig, 3);
							num3 = 4;
						}
						else
						{
							middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
							num++;
						}
					}
					else
					{
						middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
						num++;
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo17 = new MiddleBattleCallInfo(3, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo17);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.Houg, 2));
				MiddleBattleCallInfo middleBattleCallInfo18 = null;
				battleCommand = useCommands.get_Item(4);
				if (battleCommand == BattleCommand.Sekkin)
				{
					num++;
					middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				else if (battleCommand == BattleCommand.Kouku)
				{
					if (num4 == 0)
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
					else if (num4 == 1)
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else if (num4 == 2)
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
					else if (num4 == 3)
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					}
				}
				else if (Exec_Hougeki.isHougCommand(battleCommand))
				{
					if (num2 == 2 || num2 == 4)
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 1);
					}
					else if (num2 == 1 || num2 == 3)
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 2);
					}
					else
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.Houg, 3);
					}
				}
				else if (battleCommand == BattleCommand.Raigeki)
				{
					if (num3 <= 1)
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.LastRaig, 3);
					}
					else
					{
						middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
					}
				}
				else if (battleCommand == BattleCommand.Ridatu || battleCommand == BattleCommand.Kaihi)
				{
					middleBattleCallInfo18 = new MiddleBattleCallInfo(4, battleCommand, MiddleBattleCallInfo.CallType.EffectOnly, 0);
				}
				list.Add(middleBattleCallInfo18);
				list.Add(new MiddleBattleCallInfo(-1, useCommand, MiddleBattleCallInfo.CallType.None, 0));
			}
			return list;
		}
	}
}
