using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Hougeki : BattleLogicBase<HougekiDayBattleFmt>
	{
		protected BattleBaseData _f_Data;

		protected BattleBaseData _e_Data;

		protected Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		protected Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected MiddleBattleCallInfo callReferenceInfo;

		protected int[] seikuValue;

		protected List<int> f_AtkIdxs;

		protected List<int> e_AtkIdxs;

		protected double valance1;

		protected double valance2;

		protected double valance3;

		protected int AIR_ATACK_KEISU = 15;

		protected HashSet<BattleAtackKinds_Day> enableSpType;

		private HashSet<int> airAttackEndRid;

		public override BattleBaseData F_Data
		{
			get
			{
				return this._f_Data;
			}
		}

		public override BattleBaseData E_Data
		{
			get
			{
				return this._e_Data;
			}
		}

		public override Dictionary<int, BattleShipSubInfo> F_SubInfo
		{
			get
			{
				return this._f_SubInfo;
			}
		}

		public override Dictionary<int, BattleShipSubInfo> E_SubInfo
		{
			get
			{
				return this._e_SubInfo;
			}
		}

		public Exec_Hougeki(MiddleBattleCallInfo callInfo, int[] seikuValue, BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			this._f_Data = myData;
			this._e_Data = enemyData;
			this._f_SubInfo = mySubInfo;
			this._e_SubInfo = enemySubInfo;
			this.practiceFlag = practice;
			this.callReferenceInfo = callInfo;
			this.seikuValue = ((seikuValue != null) ? seikuValue : new int[2]);
			this.f_AtkIdxs = new List<int>();
			this.e_AtkIdxs = new List<int>();
			this.makeAttackerData(false);
			this.makeAttackerData(true);
			this.valance1 = 5.0;
			this.valance2 = 90.0;
			this.valance3 = 1.3;
			HashSet<BattleAtackKinds_Day> hashSet = new HashSet<BattleAtackKinds_Day>();
			hashSet.Add(BattleAtackKinds_Day.Renzoku);
			hashSet.Add(BattleAtackKinds_Day.Sp1);
			hashSet.Add(BattleAtackKinds_Day.Sp2);
			hashSet.Add(BattleAtackKinds_Day.Sp3);
			hashSet.Add(BattleAtackKinds_Day.Sp4);
			this.enableSpType = hashSet;
		}

		public void SetAirSubstituteAttacker(HashSet<int> airAtteker)
		{
			this.airAttackEndRid = airAtteker;
		}

		public override void Dispose()
		{
			this.randInstance = null;
			this.f_AtkIdxs.Clear();
			this.e_AtkIdxs.Clear();
			this.enableSpType.Clear();
		}

		public override HougekiDayBattleFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			this.commandParams = cParam;
			this.setTargetingKind(formation);
			this.formationData = formation;
			int num = Enumerable.Count<int>(this.f_AtkIdxs);
			int num2 = Enumerable.Count<int>(this.e_AtkIdxs);
			if (num == 0 && num2 == 0)
			{
				return null;
			}
			HougekiDayBattleFmt hougekiDayBattleFmt = new HougekiDayBattleFmt();
			List<int> fAtkIdx = new List<int>();
			List<int> eAtkIdx = new List<int>();
			if (this.callReferenceInfo.CommandPos != -1)
			{
				fAtkIdx = this.takeAttacker(this.f_AtkIdxs, this.callReferenceInfo.AttackType);
			}
			else
			{
				eAtkIdx = this.takeAttacker(this.e_AtkIdxs, this.callReferenceInfo.AttackType);
			}
			return this.GetResultData(this.callReferenceInfo.UseCommand, this.callReferenceInfo.CommandPos, fAtkIdx, eAtkIdx);
		}

		private List<int> takeAttacker(List<int> srcList, int atkType)
		{
			int count = srcList.get_Count();
			int num = count / 2 + count % 2;
			if (atkType == 1)
			{
				return Enumerable.ToList<int>(Enumerable.Take<int>(srcList, num));
			}
			if (atkType == 2)
			{
				return Enumerable.ToList<int>(Enumerable.Skip<int>(srcList, num));
			}
			if (atkType == 3)
			{
				return Enumerable.ToList<int>(srcList);
			}
			return new List<int>();
		}

		private HougekiDayBattleFmt GetResultData(BattleCommand command, int commandPos, List<int> fAtkIdx, List<int> eAtkIdx)
		{
			if (Enumerable.Any<Mem_ship>(this.F_Data.ShipData, (Mem_ship x) => x.IsFight()))
			{
				if (Enumerable.Any<Mem_ship>(this.E_Data.ShipData, (Mem_ship y) => y.IsFight()))
				{
					if (!Exec_Hougeki.isHougCommand(command))
					{
						fAtkIdx.Clear();
					}
					HougekiDayBattleFmt hougekiDayBattleFmt = new HougekiDayBattleFmt();
					List<Hougeki<BattleAtackKinds_Day>> list = new List<Hougeki<BattleAtackKinds_Day>>();
					int count = fAtkIdx.get_Count();
					int count2 = eAtkIdx.get_Count();
					int num = (count < count2) ? count2 : count;
					for (int i = 0; i < num; i++)
					{
						if (i >= count && i >= count2)
						{
							return null;
						}
						if (i < count)
						{
							Hougeki<BattleAtackKinds_Day> hougekiData = this.getHougekiData(command, fAtkIdx.get_Item(i), this.F_Data.ShipData.get_Item(fAtkIdx.get_Item(i)));
							if (hougekiData != null)
							{
								list.Add(hougekiData);
							}
						}
						if (i < count2)
						{
							Hougeki<BattleAtackKinds_Day> hougekiData2 = this.getHougekiData(eAtkIdx.get_Item(i), this.E_Data.ShipData.get_Item(eAtkIdx.get_Item(i)));
							if (hougekiData2 != null)
							{
								list.Add(hougekiData2);
							}
						}
					}
					if (list.get_Count() == 0)
					{
						return null;
					}
					hougekiDayBattleFmt.AttackData = list;
					return hougekiDayBattleFmt;
				}
			}
			return null;
		}

		public static bool isHougCommand(BattleCommand command)
		{
			HashSet<BattleCommand> hashSet = new HashSet<BattleCommand>();
			hashSet.Add(BattleCommand.Hougeki);
			hashSet.Add(BattleCommand.Taisen);
			hashSet.Add(BattleCommand.Kouku);
			hashSet.Add(BattleCommand.Tousha);
			hashSet.Add(BattleCommand.Totugeki);
			HashSet<BattleCommand> hashSet2 = hashSet;
			return hashSet2.Contains(command);
		}

		private Hougeki<BattleAtackKinds_Day> getHougekiData(BattleCommand command, int atk_idx, Mem_ship attacker)
		{
			if (!attacker.IsFight())
			{
				return null;
			}
			BattleBaseData f_Data = this.F_Data;
			Dictionary<int, BattleShipSubInfo> f_SubInfo = this.F_SubInfo;
			BattleBaseData e_Data = this.E_Data;
			Dictionary<int, BattleShipSubInfo> e_SubInfo = this.E_SubInfo;
			if (!this.isAttackerFromTargetKind(f_SubInfo.get_Item(attacker.Rid)))
			{
				return null;
			}
			Dictionary<int, Mst_stype> mst_stypes = Mst_DataManager.Instance.Mst_stype;
			Dictionary<int, Mst_ship> mst_ships = Mst_DataManager.Instance.Mst_ship;
			List<Mem_ship> list = e_Data.ShipData;
			bool submarine_flag = false;
			BattleAtackKinds_Day battleAtackKinds_Day = BattleAtackKinds_Day.Normal;
			List<int> slot_List = null;
			KeyValuePair<int, int> subMarineAtackKeisu = new KeyValuePair<int, int>(0, 0);
			bool flag = this.IsAirAttackGroup(attacker, f_Data.SlotData.get_Item(atk_idx), command);
			if (flag && !this.CanAirAtack_DamageState(attacker))
			{
				return null;
			}
			int num = 0;
			if (command != BattleCommand.Taisen)
			{
				list = Enumerable.ToList<Mem_ship>(Enumerable.Where<Mem_ship>(list, (Mem_ship x) => !mst_stypes.get_Item(x.Stype).IsSubmarine()));
				if (flag)
				{
					if (command != BattleCommand.Kouku)
					{
						return null;
					}
					if (!this.CanAirAttack(attacker, f_Data.SlotData.get_Item(atk_idx)))
					{
						return null;
					}
					battleAtackKinds_Day = BattleAtackKinds_Day.AirAttack;
					List<int> list2 = new List<int>();
					list2.Add(0);
					slot_List = list2;
					if (!this.isValidAirAtack_To_LandFaccillity(attacker, f_Data.SlotData.get_Item(atk_idx)))
					{
						list = Enumerable.ToList<Mem_ship>(Enumerable.Where<Mem_ship>(list, (Mem_ship x) => !mst_stypes.get_Item(x.Stype).IsLandFacillity(mst_ships.get_Item(x.Ship_id).Soku)));
					}
				}
				else if (!this.isValidHougeki(attacker))
				{
					return null;
				}
				if (!flag && command == BattleCommand.Kouku)
				{
					if (this.airAttackEndRid.Contains(attacker.Rid))
					{
						return null;
					}
					this.airAttackEndRid.Add(attacker.Rid);
					num = 2;
					int hougSlotData = this.getHougSlotData(f_Data.SlotData.get_Item(atk_idx));
					List<int> list2 = new List<int>();
					list2.Add(hougSlotData);
					slot_List = list2;
				}
			}
			else
			{
				list = Enumerable.ToList<Mem_ship>(Enumerable.Where<Mem_ship>(list, (Mem_ship x) => mst_stypes.get_Item(x.Stype).IsSubmarine()));
				subMarineAtackKeisu = this.getSubMarineAtackKeisu(list, attacker, f_Data.SlotData.get_Item(atk_idx), false);
				if (subMarineAtackKeisu.get_Key() == 0)
				{
					list = Enumerable.ToList<Mem_ship>(Enumerable.Where<Mem_ship>(e_Data.ShipData, (Mem_ship x) => !mst_stypes.get_Item(x.Stype).IsSubmarine()));
					submarine_flag = false;
					num = 1;
					if (flag)
					{
						if (!this.CanAirAttack(attacker, f_Data.SlotData.get_Item(atk_idx)))
						{
							return null;
						}
						battleAtackKinds_Day = BattleAtackKinds_Day.AirAttack;
						List<int> list2 = new List<int>();
						list2.Add(0);
						slot_List = list2;
						if (!this.isValidAirAtack_To_LandFaccillity(attacker, f_Data.SlotData.get_Item(atk_idx)))
						{
							list = Enumerable.ToList<Mem_ship>(Enumerable.Where<Mem_ship>(list, (Mem_ship x) => !mst_stypes.get_Item(x.Stype).IsLandFacillity(mst_ships.get_Item(x.Ship_id).Soku)));
						}
					}
					else if (!this.isValidHougeki(attacker))
					{
						return null;
					}
					if (!flag)
					{
						int hougSlotData2 = this.getHougSlotData(f_Data.SlotData.get_Item(atk_idx));
						List<int> list2 = new List<int>();
						list2.Add(hougSlotData2);
						slot_List = list2;
					}
				}
				else
				{
					battleAtackKinds_Day = ((subMarineAtackKeisu.get_Key() != 1) ? BattleAtackKinds_Day.AirAttack : BattleAtackKinds_Day.Bakurai);
					List<int> list2 = new List<int>();
					list2.Add(0);
					slot_List = list2;
					submarine_flag = true;
				}
			}
			if (list.get_Count() == 0)
			{
				return null;
			}
			if (command != BattleCommand.Taisen && num == 0)
			{
				KeyValuePair<BattleAtackKinds_Day, List<int>> spAttackKind = this.getSpAttackKind(attacker, f_Data.SlotData.get_Item(atk_idx));
				if (battleAtackKinds_Day != BattleAtackKinds_Day.AirAttack || spAttackKind.get_Key() != BattleAtackKinds_Day.Normal)
				{
					battleAtackKinds_Day = spAttackKind.get_Key();
					slot_List = spAttackKind.get_Value();
				}
			}
			Hougeki<BattleAtackKinds_Day> attackData = this.getAttackData(attacker, f_Data.SlotData.get_Item(atk_idx), f_Data.SlotLevel.get_Item(atk_idx), battleAtackKinds_Day, submarine_flag, subMarineAtackKeisu, list, e_Data.LostFlag, e_SubInfo, num);
			if (attackData != null)
			{
				attackData.Slot_List = slot_List;
			}
			return attackData;
		}

		private Hougeki<BattleAtackKinds_Day> getHougekiData(int atk_idx, Mem_ship attacker)
		{
			if (!attacker.IsFight())
			{
				return null;
			}
			BattleBaseData e_Data = this.E_Data;
			Dictionary<int, BattleShipSubInfo> e_SubInfo = this.E_SubInfo;
			Dictionary<int, BattleShipSubInfo> f_SubInfo = this.F_SubInfo;
			BattleBaseData f_Data = this.F_Data;
			if (!this.isAttackerFromTargetKind(e_SubInfo.get_Item(attacker.Rid)))
			{
				return null;
			}
			BattleAtackKinds_Day battleAtackKinds_Day = BattleAtackKinds_Day.Normal;
			List<int> slot_List = null;
			List<Mem_ship> list = f_Data.ShipData;
			KeyValuePair<int, int> subMarineAtackKeisu = this.getSubMarineAtackKeisu(list, attacker, e_Data.SlotData.get_Item(atk_idx), false);
			bool submarine_flag = false;
			bool flag = this.IsAirAttackGroup(attacker, e_Data.SlotData.get_Item(atk_idx), BattleCommand.None);
			if (flag && !this.CanAirAtack_DamageState(attacker))
			{
				return null;
			}
			if (subMarineAtackKeisu.get_Key() != 0)
			{
				battleAtackKinds_Day = ((subMarineAtackKeisu.get_Key() != 1) ? BattleAtackKinds_Day.AirAttack : BattleAtackKinds_Day.Bakurai);
				List<int> list2 = new List<int>();
				list2.Add(0);
				slot_List = list2;
				submarine_flag = true;
			}
			else
			{
				if (flag)
				{
					if (!this.CanAirAttack(attacker, e_Data.SlotData.get_Item(atk_idx)))
					{
						return null;
					}
					battleAtackKinds_Day = BattleAtackKinds_Day.AirAttack;
					List<int> list2 = new List<int>();
					list2.Add(0);
					slot_List = list2;
					if (!this.isValidAirAtack_To_LandFaccillity(attacker, e_Data.SlotData.get_Item(atk_idx)))
					{
						List<Mem_ship> list3 = Enumerable.ToList<Mem_ship>(Enumerable.Select(Enumerable.Where(Enumerable.Select(Enumerable.Select(list, (Mem_ship shipobj) => new
						{
							shipobj = shipobj,
							soku = Mst_DataManager.Instance.Mst_ship.get_Item(shipobj.Ship_id).Soku
						}), <>__TranspIdent11 => new
						{
							<>__TranspIdent11 = <>__TranspIdent11,
							land_flag = Mst_DataManager.Instance.Mst_stype.get_Item(<>__TranspIdent11.shipobj.Stype).IsLandFacillity(<>__TranspIdent11.soku)
						}), <>__TranspIdent12 => !<>__TranspIdent12.land_flag), <>__TranspIdent12 => <>__TranspIdent12.<>__TranspIdent11.shipobj));
						list = list3;
					}
				}
				else if (!this.isValidHougeki(attacker))
				{
					return null;
				}
				KeyValuePair<BattleAtackKinds_Day, List<int>> spAttackKind = this.getSpAttackKind(attacker, e_Data.SlotData.get_Item(atk_idx));
				if (battleAtackKinds_Day != BattleAtackKinds_Day.AirAttack || spAttackKind.get_Key() != BattleAtackKinds_Day.Normal)
				{
					battleAtackKinds_Day = spAttackKind.get_Key();
					slot_List = spAttackKind.get_Value();
				}
			}
			Hougeki<BattleAtackKinds_Day> attackData = this.getAttackData(attacker, e_Data.SlotData.get_Item(atk_idx), e_Data.SlotLevel.get_Item(atk_idx), battleAtackKinds_Day, submarine_flag, subMarineAtackKeisu, list, f_Data.LostFlag, f_SubInfo, 0);
			if (attackData != null)
			{
				attackData.Slot_List = slot_List;
			}
			return attackData;
		}

		private Hougeki<BattleAtackKinds_Day> getAttackData(Mem_ship attacker, List<Mst_slotitem> attackerSlot, List<int> attackerSlotLevel, BattleAtackKinds_Day attackType, bool submarine_flag, KeyValuePair<int, int> submarine_keisu, List<Mem_ship> targetShips, List<bool> targetLostFlags, Dictionary<int, BattleShipSubInfo> targetSubInfo, int powerDownType)
		{
			BattleDamageKinds battleDamageKinds = BattleDamageKinds.Normal;
			Mem_ship atackTarget = base.getAtackTarget(attacker, targetShips, false, submarine_flag, true, ref battleDamageKinds);
			if (atackTarget == null)
			{
				return null;
			}
			int deckIdx = targetSubInfo.get_Item(atackTarget.Rid).DeckIdx;
			Hougeki<BattleAtackKinds_Day> hougeki = new Hougeki<BattleAtackKinds_Day>();
			hougeki.Attacker = attacker.Rid;
			hougeki.SpType = attackType;
			int num = (attackType != BattleAtackKinds_Day.Renzoku) ? 1 : 2;
			for (int i = 0; i < num; i++)
			{
				int soukou = atackTarget.Soukou;
				hougeki.Target.Add(atackTarget.Rid);
				int num2;
				int num3;
				FormationDatas.GetFormationKinds battleState;
				if (submarine_flag)
				{
					num2 = this.getSubmarineAttackValue(submarine_keisu, attacker, attackerSlot, attackerSlotLevel);
					num3 = base.getSubmarineHitProb(attacker, attackerSlot, attackerSlotLevel);
					battleState = FormationDatas.GetFormationKinds.SUBMARINE;
				}
				else
				{
					int tekkouKind = this.getTekkouKind(atackTarget.Stype, attackerSlot);
					num2 = this.getHougAttackValue(attackType, attacker, attackerSlot, atackTarget, tekkouKind);
					num3 = this.getHougHitProb(attackType, attacker, attackerSlot, tekkouKind);
					battleState = FormationDatas.GetFormationKinds.HOUGEKI;
				}
				int battleAvo = base.getBattleAvo(battleState, atackTarget);
				if (powerDownType == 1)
				{
					num2 = (int)((double)num2 * 0.55);
					num3 = (int)((double)num3 * 0.55);
				}
				else if (powerDownType == 2)
				{
					num3 = (int)((double)num3 * 0.55);
				}
				bool airAttack = attackType == BattleAtackKinds_Day.AirAttack;
				BattleHitStatus battleHitStatus = this.getHitStatus(num3, battleAvo, attacker, atackTarget, this.valance3, airAttack);
				if (battleHitStatus == BattleHitStatus.Miss && this.enableSpType.Contains(attackType))
				{
					battleHitStatus = BattleHitStatus.Normal;
				}
				int num4 = this.setDamageValue(battleHitStatus, num2, soukou, attacker, atackTarget, targetLostFlags);
				hougeki.Damage.Add(num4);
				hougeki.Clitical.Add(battleHitStatus);
				hougeki.DamageKind.Add(battleDamageKinds);
			}
			if (attacker.IsEnemy())
			{
				base.RecoveryShip(deckIdx);
			}
			return hougeki;
		}

		protected virtual void makeAttackerData(bool enemyFlag)
		{
			List<int> list;
			Dictionary<int, BattleShipSubInfo> dictionary;
			if (!enemyFlag)
			{
				list = this.f_AtkIdxs;
				BattleBaseData battleBaseData = this.F_Data;
				dictionary = this.F_SubInfo;
			}
			else
			{
				list = this.e_AtkIdxs;
				BattleBaseData battleBaseData = this.E_Data;
				dictionary = this.E_SubInfo;
			}
			IOrderedEnumerable<BattleShipSubInfo> orderedEnumerable = Enumerable.OrderBy<BattleShipSubInfo, int>(dictionary.get_Values(), (BattleShipSubInfo x) => x.AttackNo);
			using (IEnumerator<BattleShipSubInfo> enumerator = orderedEnumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BattleShipSubInfo current = enumerator.get_Current();
					list.Add(current.DeckIdx);
				}
			}
		}

		protected virtual int getHougAttackValue(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship, int tekkouKind)
		{
			int num = 150;
			List<int> list;
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			if (atk_ship.IsEnemy())
			{
				int deckIdx = this.E_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.E_Data.SlotLevel.get_Item(deckIdx);
				formation = this.E_Data.Formation;
				battleFormation = this.E_Data.BattleFormation;
				BattleFormationKinds1 formation2 = this.F_Data.Formation;
			}
			else
			{
				int deckIdx2 = this.F_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.F_Data.SlotLevel.get_Item(deckIdx2);
				formation = this.F_Data.Formation;
				battleFormation = this.F_Data.BattleFormation;
				BattleFormationKinds1 formation2 = this.E_Data.Formation;
			}
			double num2 = 0.0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			using (var enumerator = Enumerable.Select(atk_slot, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					Mst_slotitem obj2 = current.obj;
					if (base.IsAtapSlotItem(obj2.Api_mapbattle_type3))
					{
						num5++;
					}
					num3 += obj2.Baku;
					num4 += obj2.Raig;
					num2 += this.getHougSlotPlus_Attack(obj2, list.get_Item(current.idx));
				}
			}
			double num6 = this.valance1 + (double)atk_ship.Houg + num2;
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(def_ship.Stype).IsLandFacillity(Mst_DataManager.Instance.Mst_ship.get_Item(def_ship.Ship_id).Soku);
			if (flag)
			{
				num6 *= this.getLandFacciilityKeisu(atk_slot);
				num6 += (double)base.getAtapKeisu(num5);
				num4 = 0;
			}
			if (kind == BattleAtackKinds_Day.AirAttack)
			{
				int airAtackPow = this.getAirAtackPow(num3, num4);
				num6 += (double)airAtackPow;
				num6 = 25.0 + (double)((int)(num6 * 1.5));
			}
			double formationParamBattle = this.formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.HOUGEKI, battleFormation);
			double formationParamF = this.formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.HOUGEKI, formation);
			num6 = num6 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num7 = 1.0;
			if (damageState == DamageState.Tyuuha)
			{
				num7 = 0.7;
			}
			else if (damageState == DamageState.Taiha)
			{
				num7 = 0.4;
			}
			num6 *= num7;
			num6 += this.getHougItemAtackHosei(atk_ship, atk_slot);
			if (num6 > (double)num)
			{
				num6 = (double)num + Math.Sqrt(num6 - (double)num);
			}
			Dictionary<BattleAtackKinds_Day, double> dictionary = new Dictionary<BattleAtackKinds_Day, double>();
			dictionary.Add(BattleAtackKinds_Day.Renzoku, 1.2);
			dictionary.Add(BattleAtackKinds_Day.Sp1, 1.1);
			dictionary.Add(BattleAtackKinds_Day.Sp2, 1.2);
			dictionary.Add(BattleAtackKinds_Day.Sp3, 1.3);
			dictionary.Add(BattleAtackKinds_Day.Sp4, 1.5);
			Dictionary<BattleAtackKinds_Day, double> dictionary2 = dictionary;
			if (dictionary2.ContainsKey(kind))
			{
				num6 *= dictionary2.get_Item(kind);
			}
			num6 *= this.getTekkouKeisu_Attack(tekkouKind);
			return (int)num6;
		}

		protected virtual double getHougSlotPlus_Attack(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (mstItem.Api_mapbattle_type3 == 5 || mstItem.Api_mapbattle_type3 == 22)
			{
				return result;
			}
			double num = 2.0;
			if (mstItem.Houg > 12)
			{
				num = 3.0;
			}
			if (mstItem.Api_mapbattle_type3 == 12 || mstItem.Api_mapbattle_type3 == 13 || mstItem.Api_mapbattle_type3 == 16 || mstItem.Api_mapbattle_type3 == 17 || mstItem.Api_mapbattle_type3 == 27 || mstItem.Api_mapbattle_type3 == 28)
			{
				num = 0.0;
			}
			else if (mstItem.Api_mapbattle_type3 == 14 || mstItem.Api_mapbattle_type3 == 15 || mstItem.Api_mapbattle_type3 == 40)
			{
				num = 1.5;
			}
			return num * Math.Sqrt((double)slotLevel) * 0.5;
		}

		protected virtual double getHougItemAtackHosei(Mem_ship ship, List<Mst_slotitem> mst_item)
		{
			if (mst_item.get_Count() == 0)
			{
				return 0.0;
			}
			if (ship.Stype != 3 && ship.Stype != 4 && ship.Stype != 21)
			{
				return 0.0;
			}
			ILookup<int, Mst_slotitem> lookup = Enumerable.ToLookup<Mst_slotitem, int>(mst_item, (Mst_slotitem x) => x.Id);
			int num = 0;
			if (lookup.Contains(4))
			{
				num += Enumerable.Count<Mst_slotitem>(lookup.get_Item(4));
			}
			if (lookup.Contains(11))
			{
				num += Enumerable.Count<Mst_slotitem>(lookup.get_Item(11));
			}
			int num2 = 0;
			if (lookup.Contains(119))
			{
				num2 += Enumerable.Count<Mst_slotitem>(lookup.get_Item(119));
			}
			if (lookup.Contains(65))
			{
				num2 += Enumerable.Count<Mst_slotitem>(lookup.get_Item(65));
			}
			if (lookup.Contains(139))
			{
				num2 += Enumerable.Count<Mst_slotitem>(lookup.get_Item(139));
			}
			return 1.0 * Math.Sqrt((double)num) + 2.0 * Math.Sqrt((double)num2);
		}

		protected virtual int getHougHitProb(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int tekkouKind)
		{
			double num = 0.0;
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			List<int> list;
			if (!atk_ship.IsEnemy())
			{
				formation = this.F_Data.Formation;
				formation2 = this.E_Data.Formation;
				int deckIdx = this.F_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.F_Data.SlotLevel.get_Item(deckIdx);
				num = (double)this.commandParams.Fspp / 100.0;
			}
			else
			{
				formation = this.E_Data.Formation;
				formation2 = this.F_Data.Formation;
				int deckIdx2 = this.E_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.E_Data.SlotLevel.get_Item(deckIdx2);
			}
			double num2 = 0.0;
			int num3 = 0;
			using (var enumerator = Enumerable.Select(atk_slot, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					num3 += current.obj.Houm;
					num2 += this.getHougSlotPlus_Hit(current.obj, list.get_Item(current.idx));
				}
			}
			double num4 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt((double)atk_ship.Level) * 2.0 + (double)num3;
			double num5 = this.valance2 + num4 + num2;
			double formationParamF = this.formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.HOUGEKI, formation, formation2);
			num5 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num6 = 1.0;
			if (fatigueState == FatigueState.Exaltation)
			{
				num6 = 1.2;
			}
			else if (fatigueState == FatigueState.Light)
			{
				num6 = 0.8;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num6 = 0.5;
			}
			num5 *= num6;
			num5 = this.getHougHitProbUpValue(num5, atk_ship, atk_slot);
			Dictionary<BattleAtackKinds_Day, double> dictionary = new Dictionary<BattleAtackKinds_Day, double>();
			dictionary.Add(BattleAtackKinds_Day.Renzoku, 1.1);
			dictionary.Add(BattleAtackKinds_Day.Sp1, 1.3);
			dictionary.Add(BattleAtackKinds_Day.Sp2, 1.5);
			dictionary.Add(BattleAtackKinds_Day.Sp3, 1.3);
			dictionary.Add(BattleAtackKinds_Day.Sp4, 1.2);
			Dictionary<BattleAtackKinds_Day, double> dictionary2 = dictionary;
			if (dictionary2.ContainsKey(kind))
			{
				num5 *= dictionary2.get_Item(kind);
			}
			num5 *= this.getTekkouKeisu_Hit(tekkouKind);
			double num7 = num5 * num;
			num5 += num7;
			return (int)num5;
		}

		protected virtual double getHougSlotPlus_Hit(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (mstItem.Api_mapbattle_type3 == 5 || mstItem.Api_mapbattle_type3 == 22)
			{
				return result;
			}
			if ((mstItem.Api_mapbattle_type3 == 12 || mstItem.Api_mapbattle_type3 == 13) && mstItem.Houm > 2)
			{
				result = Math.Sqrt((double)slotLevel) * 1.7;
			}
			else if (mstItem.Api_mapbattle_type3 == 21 || mstItem.Api_mapbattle_type3 == 14 || mstItem.Api_mapbattle_type3 == 40 || mstItem.Api_mapbattle_type3 == 16 || mstItem.Api_mapbattle_type3 == 27 || mstItem.Api_mapbattle_type3 == 28 || mstItem.Api_mapbattle_type3 == 17 || mstItem.Api_mapbattle_type3 == 15)
			{
				result = Math.Sqrt((double)slotLevel);
			}
			return result;
		}

		protected virtual double getHougHitProbUpValue(double hit_prob, Mem_ship atk_ship, List<Mst_slotitem> atk_slot)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(atk_ship.Ship_id);
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(10);
			hashSet.Add(3);
			hashSet.Add(9);
			hashSet.Add(4);
			hashSet.Add(21);
			HashSet<int> hashSet2 = hashSet;
			if (!hashSet2.Contains(atk_ship.Stype))
			{
				return hit_prob;
			}
			if (atk_ship.Stype == 9 && mst_ship.Taik > 92)
			{
				return hit_prob;
			}
			Dictionary<int, List<int>> dictionary = new Dictionary<int, List<int>>();
			Dictionary<int, List<int>> arg_A4_0 = dictionary;
			int arg_A4_1 = 1;
			List<int> list = new List<int>();
			list.Add(9);
			arg_A4_0.Add(arg_A4_1, list);
			Dictionary<int, List<int>> arg_BE_0 = dictionary;
			int arg_BE_1 = 2;
			list = new List<int>();
			list.Add(117);
			arg_BE_0.Add(arg_BE_1, list);
			Dictionary<int, List<int>> arg_E0_0 = dictionary;
			int arg_E0_1 = 3;
			list = new List<int>();
			list.Add(105);
			list.Add(8);
			arg_E0_0.Add(arg_E0_1, list);
			Dictionary<int, List<int>> arg_11D_0 = dictionary;
			int arg_11D_1 = 4;
			list = new List<int>();
			list.Add(7);
			list.Add(103);
			list.Add(104);
			list.Add(76);
			list.Add(114);
			arg_11D_0.Add(arg_11D_1, list);
			Dictionary<int, List<int>> arg_146_0 = dictionary;
			int arg_146_1 = 5;
			list = new List<int>();
			list.Add(133);
			list.Add(137);
			arg_146_0.Add(arg_146_1, list);
			Dictionary<int, List<int>> arg_168_0 = dictionary;
			int arg_168_1 = 6;
			list = new List<int>();
			list.Add(4);
			list.Add(11);
			arg_168_0.Add(arg_168_1, list);
			Dictionary<int, List<int>> arg_197_0 = dictionary;
			int arg_197_1 = 7;
			list = new List<int>();
			list.Add(119);
			list.Add(65);
			list.Add(139);
			arg_197_0.Add(arg_197_1, list);
			Dictionary<int, List<int>> dictionary2 = dictionary;
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			ILookup<int, Mst_slotitem> lookup = Enumerable.ToLookup<Mst_slotitem, int>(atk_slot, (Mst_slotitem x) => x.Id);
			using (Dictionary<int, List<int>>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, List<int>> current = enumerator.get_Current();
					int num = 0;
					using (List<int>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int current2 = enumerator2.get_Current();
							if (lookup.Contains(current2))
							{
								num += Enumerable.Count<Mst_slotitem>(lookup.get_Item(current2));
							}
						}
					}
					dictionary3.Add(current.get_Key(), num);
				}
			}
			double num2 = 1.0;
			double num3 = hit_prob;
			if (atk_ship.Level >= 100)
			{
				num2 = 0.6;
			}
			int num4 = dictionary3.get_Item(1);
			int num5 = dictionary3.get_Item(2);
			int num6 = dictionary3.get_Item(3);
			int num7 = dictionary3.get_Item(4);
			int num8 = dictionary3.get_Item(5);
			int num9 = dictionary3.get_Item(6);
			int num10 = dictionary3.get_Item(7);
			if (atk_ship.Stype == 8)
			{
				num3 = num3 - 10.0 * num2 * Math.Sqrt((double)num4) - 5.0 * num2 * Math.Sqrt((double)num6) - 7.0 * num2 * Math.Sqrt((double)num5);
				num3 -= 2.0 * num2 * Math.Sqrt((double)num8);
				if (mst_ship.Yomi.Equals("ビスマルク") || mst_ship.Yomi.Equals("リットリオ・イタリア") || mst_ship.Yomi.Equals("ローマ"))
				{
					num3 += 3.0 * num2 * Math.Sqrt((double)num8);
				}
				num3 += 4.0 * Math.Sqrt((double)num7);
			}
			else if (atk_ship.Stype == 10)
			{
				num3 = num3 - 7.0 * num2 * Math.Sqrt((double)num4) - 3.0 * num2 * Math.Sqrt((double)num5);
				num3 += 2.0 * num2 * Math.Sqrt((double)num8);
				num3 = num3 + 4.0 * Math.Sqrt((double)num7) + 2.0 * Math.Sqrt((double)num6);
			}
			else if (atk_ship.Stype == 9)
			{
				num3 = num3 - 7.0 * num2 * Math.Sqrt((double)num4) - 3.0 * num2 * Math.Sqrt((double)num5);
				num3 += 2.0 * num2 * Math.Sqrt((double)num8);
				num3 = num3 + 2.0 * Math.Sqrt((double)num7) + 2.0 * Math.Sqrt((double)num6);
			}
			else if (atk_ship.Stype == 3 || atk_ship.Stype == 4 || atk_ship.Stype == 21)
			{
				num3 = num3 + 4.0 * Math.Sqrt((double)num9) + 3.0 * Math.Sqrt((double)num10) - 2.0;
			}
			return num3;
		}

		protected int getHougSlotData(List<Mst_slotitem> items)
		{
			int a_slot = 0;
			List<int> b_slot = new List<int>();
			List<Mst_slotitem> list = Enumerable.ToList<Mst_slotitem>(items);
			list.RemoveAll((Mst_slotitem x) => x == null);
			list.ForEach(delegate(Mst_slotitem x)
			{
				if (x.Api_mapbattle_type3 >= 1 && x.Api_mapbattle_type3 <= 3)
				{
					a_slot = x.Id;
					return;
				}
				if (x.Api_mapbattle_type3 == 4)
				{
					b_slot.Add(x.Id);
				}
			});
			if (a_slot > 0)
			{
				return a_slot;
			}
			if (a_slot == 0 && b_slot.get_Count() == 0)
			{
				return 0;
			}
			return b_slot.get_Item(0);
		}

		protected KeyValuePair<BattleAtackKinds_Day, List<int>> getSpAttackKind(Mem_ship ship, List<Mst_slotitem> slotitems)
		{
			if (slotitems.get_Count() == 0)
			{
				BattleAtackKinds_Day arg_2C_0 = BattleAtackKinds_Day.Normal;
				List<int> list = new List<int>();
				list.Add(0);
				return new KeyValuePair<BattleAtackKinds_Day, List<int>>(arg_2C_0, list);
			}
			BattleAtackKinds_Day battleAtackKinds_Day = BattleAtackKinds_Day.Normal;
			List<int> ret_slotitem = new List<int>();
			Func<List<Mst_slotitem>, KeyValuePair<BattleAtackKinds_Day, List<int>>> func = delegate(List<Mst_slotitem> x)
			{
				int hougSlotData = this.getHougSlotData(x);
				ret_slotitem.Add(hougSlotData);
				return new KeyValuePair<BattleAtackKinds_Day, List<int>>(BattleAtackKinds_Day.Normal, ret_slotitem);
			};
			int num = 0;
			BattleBaseData battleBaseData = null;
			Dictionary<int, BattleShipSubInfo> dictionary = null;
			if (ship.IsEnemy())
			{
				num = this.seikuValue[1];
				battleBaseData = this.E_Data;
				dictionary = this.E_SubInfo;
			}
			else
			{
				num = this.seikuValue[0];
				battleBaseData = this.F_Data;
				dictionary = this.F_SubInfo;
			}
			if (num <= 1)
			{
				return func.Invoke(slotitems);
			}
			if (ship.Get_DamageState() >= DamageState.Taiha)
			{
				return func.Invoke(slotitems);
			}
			Dictionary<int, List<Mst_slotitem>> dictionary2 = new Dictionary<int, List<Mst_slotitem>>();
			dictionary2.Add(1, new List<Mst_slotitem>());
			dictionary2.Add(12, new List<Mst_slotitem>());
			dictionary2.Add(10, new List<Mst_slotitem>());
			dictionary2.Add(19, new List<Mst_slotitem>());
			dictionary2.Add(4, new List<Mst_slotitem>());
			Dictionary<int, List<Mst_slotitem>> dictionary3 = dictionary2;
			double num2 = 0.0;
			using (var enumerator = Enumerable.Select(slotitems, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					int api_mapbattle_type = current.obj.Api_mapbattle_type3;
					num2 += (double)current.obj.Saku;
					if (api_mapbattle_type == 1 || api_mapbattle_type == 2 || api_mapbattle_type == 3)
					{
						dictionary3.get_Item(1).Add(current.obj);
					}
					else if (api_mapbattle_type == 12 || api_mapbattle_type == 13)
					{
						dictionary3.get_Item(12).Add(current.obj);
					}
					else if ((api_mapbattle_type == 10 || api_mapbattle_type == 11) && ship.Onslot.get_Item(current.idx) > 0)
					{
						dictionary3.get_Item(10).Add(current.obj);
					}
					else if (api_mapbattle_type == 19 || api_mapbattle_type == 4)
					{
						dictionary3.get_Item(api_mapbattle_type).Add(current.obj);
					}
				}
			}
			if (dictionary3.get_Item(10).get_Count() == 0 || dictionary3.get_Item(1).get_Count() == 0)
			{
				return func.Invoke(slotitems);
			}
			double num3 = 0.0;
			using (var enumerator2 = Enumerable.Select(battleBaseData.ShipData, (Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}).GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					var current2 = enumerator2.get_Current();
					if (current2.obj.IsFight())
					{
						num3 += (double)current2.obj.GetBattleBaseParam().Sakuteki;
						List<Mst_slotitem> list2 = battleBaseData.SlotData.get_Item(current2.ship_idx);
						if (list2.get_Count() != 0)
						{
							using (var enumerator3 = Enumerable.Select(list2, (Mst_slotitem obj, int slot_idx) => new
							{
								obj,
								slot_idx
							}).GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									var current3 = enumerator3.get_Current();
									int num4 = current2.obj.Onslot.get_Item(current3.slot_idx);
									if ((current3.obj.Api_mapbattle_type3 == 10 || current3.obj.Api_mapbattle_type3 == 11) && num4 > 0)
									{
										int num5 = current3.obj.Saku * (int)Math.Sqrt((double)num4);
										num3 += (double)num5;
									}
								}
							}
						}
					}
				}
			}
			double num6 = (double)((int)(Math.Sqrt(num3) + num3 * 0.1));
			int num7 = (int)(Math.Sqrt((double)ship.GetBattleBaseParam().Luck) + 10.0);
			if (num == 3)
			{
				num7 = (int)((double)num7 + 10.0 + (num6 + num2 * 1.6) * 0.7);
			}
			else if (num == 2)
			{
				num7 = (int)((double)num7 + (num6 + num2 * 1.2) * 0.6);
			}
			if (dictionary.get_Item(ship.Rid).DeckIdx == 0)
			{
				num7 += 15;
			}
			Dictionary<BattleAtackKinds_Day, int> dictionary4 = new Dictionary<BattleAtackKinds_Day, int>();
			dictionary4.Add(BattleAtackKinds_Day.Sp4, 150);
			dictionary4.Add(BattleAtackKinds_Day.Sp3, 140);
			dictionary4.Add(BattleAtackKinds_Day.Sp2, 130);
			dictionary4.Add(BattleAtackKinds_Day.Sp1, 120);
			dictionary4.Add(BattleAtackKinds_Day.Renzoku, 130);
			Dictionary<BattleAtackKinds_Day, int> dictionary5 = dictionary4;
			if (dictionary3.get_Item(1).get_Count() >= 2 && dictionary3.get_Item(19).get_Count() >= 1 && num7 > this.randInstance.Next(dictionary5.get_Item(BattleAtackKinds_Day.Sp4)))
			{
				ret_slotitem.Add(dictionary3.get_Item(10).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(1).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(1).get_Item(1).Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp4;
			}
			else if (dictionary3.get_Item(4).get_Count() >= 1 && dictionary3.get_Item(19).get_Count() >= 1 && num7 > this.randInstance.Next(dictionary5.get_Item(BattleAtackKinds_Day.Sp3)))
			{
				ret_slotitem.Add(dictionary3.get_Item(10).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(1).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(19).get_Item(0).Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp3;
			}
			else if (dictionary3.get_Item(4).get_Count() >= 1 && dictionary3.get_Item(12).get_Count() >= 1 && num7 > this.randInstance.Next(dictionary5.get_Item(BattleAtackKinds_Day.Sp2)))
			{
				ret_slotitem.Add(dictionary3.get_Item(10).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(12).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(1).get_Item(0).Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp2;
			}
			else if (dictionary3.get_Item(4).get_Count() >= 1 && num7 > this.randInstance.Next(dictionary5.get_Item(BattleAtackKinds_Day.Sp1)))
			{
				ret_slotitem.Add(dictionary3.get_Item(10).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(1).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(4).get_Item(0).Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Sp1;
			}
			else if (dictionary3.get_Item(1).get_Count() >= 2 && num7 > this.randInstance.Next(dictionary5.get_Item(BattleAtackKinds_Day.Renzoku)))
			{
				ret_slotitem.Add(dictionary3.get_Item(1).get_Item(0).Id);
				ret_slotitem.Add(dictionary3.get_Item(1).get_Item(1).Id);
				battleAtackKinds_Day = BattleAtackKinds_Day.Renzoku;
			}
			if (battleAtackKinds_Day == BattleAtackKinds_Day.Normal)
			{
				return func.Invoke(slotitems);
			}
			return new KeyValuePair<BattleAtackKinds_Day, List<int>>(battleAtackKinds_Day, ret_slotitem);
		}

		protected bool isValidHougeki(Mem_ship ship)
		{
			return ship.IsFight() && !Mst_DataManager.Instance.Mst_stype.get_Item(ship.Stype).IsSubmarine() && ship.GetBattleBaseParam().Houg > 0;
		}

		protected bool IsAirAttackGroup(Mem_ship ship, List<Mst_slotitem> slotData, BattleCommand command)
		{
			if (Mst_DataManager.Instance.Mst_stype.get_Item(ship.Stype).IsMother() || Mst_DataManager.Instance.Mst_stype.get_Item(ship.Stype).IsLandFacillity(Mst_DataManager.Instance.Mst_ship.get_Item(ship.Ship_id).Soku))
			{
				return true;
			}
			if (ship.Stype == 22)
			{
				if (ship.IsEnemy())
				{
					return Enumerable.Any<Mst_slotitem>(slotData, (Mst_slotitem x) => x.Api_mapbattle_type3 == 7 || x.Api_mapbattle_type3 == 8);
				}
				if (command == BattleCommand.Kouku)
				{
					return true;
				}
			}
			return false;
		}

		protected bool CanAirAttack(Mem_ship ship, List<Mst_slotitem> slotData)
		{
			if (!ship.IsFight())
			{
				return false;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			HashSet<int> hashSet2 = hashSet;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			for (int i = 0; i < Enumerable.Count<Mst_slotitem>(slotData); i++)
			{
				Mst_slotitem mst_slotitem = slotData.get_Item(i);
				if (mst_slotitem != null && hashSet2.Contains(mst_slotitem.Api_mapbattle_type3))
				{
					num += ship.Onslot.get_Item(i);
					num2 += mst_slotitem.Baku;
					num3 += mst_slotitem.Raig;
				}
			}
			return num > 0 && this.getAirAtackPow(num2, num3) > 0;
		}

		protected bool CanAirAtack_DamageState(Mem_ship ship)
		{
			DamageState damageState = (ship.Stype != 18) ? DamageState.Tyuuha : DamageState.Taiha;
			return ship.Get_DamageState() < damageState;
		}

		protected virtual int getAirAtackPow(int baku, int raig)
		{
			return (int)(Math.Floor((double)baku * 1.3) + (double)raig) + this.AIR_ATACK_KEISU;
		}

		protected virtual bool isValidAirAtack_To_LandFaccillity(Mem_ship attacker, List<Mst_slotitem> slotitems)
		{
			using (var enumerator = Enumerable.Select(slotitems, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (current.obj.Api_mapbattle_type3 == 7)
					{
						if (attacker.Onslot.get_Item(current.idx) > 0)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}

		private int getTekkouKind(int targetStype, List<Mst_slotitem> attackerSlot)
		{
			int result = 0;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			hashSet.Add(6);
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			hashSet.Add(11);
			hashSet.Add(18);
			HashSet<int> hashSet2 = hashSet;
			if (!hashSet2.Contains(targetStype))
			{
				return result;
			}
			bool haveMain = false;
			bool haveSub = false;
			bool haveDentan = false;
			bool haveTekkou = false;
			attackerSlot.ForEach(delegate(Mst_slotitem mst)
			{
				if (mst.Api_mapbattle_type3 >= 1 && mst.Api_mapbattle_type3 <= 3)
				{
					haveMain = true;
				}
				else if (mst.Api_mapbattle_type3 == 4)
				{
					haveSub = true;
				}
				else if (mst.Api_mapbattle_type3 == 12 || mst.Api_mapbattle_type3 == 13)
				{
					haveDentan = true;
				}
				else if (mst.Api_mapbattle_type3 == 19)
				{
					haveTekkou = true;
				}
			});
			if (haveMain && haveSub && haveDentan && haveTekkou)
			{
				return 4;
			}
			if (haveMain && haveSub && haveTekkou)
			{
				return 3;
			}
			if (haveMain && haveDentan && haveTekkou)
			{
				return 2;
			}
			if (haveMain && haveTekkou)
			{
				return 1;
			}
			return result;
		}

		private double getTekkouKeisu_Attack(int tekkouKind)
		{
			if (tekkouKind == 1)
			{
				return 1.08;
			}
			if (tekkouKind == 2)
			{
				return 1.1;
			}
			if (tekkouKind == 3 || tekkouKind == 4)
			{
				return 1.15;
			}
			return 1.0;
		}

		private double getTekkouKeisu_Hit(int tekkouKind)
		{
			if (tekkouKind == 1)
			{
				return 1.1;
			}
			if (tekkouKind == 2)
			{
				return 1.25;
			}
			if (tekkouKind == 3)
			{
				return 1.2;
			}
			if (tekkouKind == 4)
			{
				return 1.3;
			}
			return 1.0;
		}
	}
}
