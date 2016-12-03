using Common.Enum;
using Server_Common;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public abstract class BattleLogicBase<T> : IDisposable
	{
		protected bool practiceFlag;

		protected Random randInstance;

		protected FormationDatas formationData;

		protected BattleCommandParams commandParams;

		protected BattleTargetKind battleTargetKind;

		protected readonly double valanceSubmarine1;

		protected readonly double valanceSubmarine2;

		protected readonly double valanceSubmarine3;

		public abstract BattleBaseData F_Data
		{
			get;
		}

		public abstract BattleBaseData E_Data
		{
			get;
		}

		public abstract Dictionary<int, BattleShipSubInfo> F_SubInfo
		{
			get;
		}

		public abstract Dictionary<int, BattleShipSubInfo> E_SubInfo
		{
			get;
		}

		public BattleLogicBase()
		{
			this.randInstance = new Random();
			this.practiceFlag = false;
			this.valanceSubmarine1 = 3.0;
			this.valanceSubmarine2 = 80.0;
			this.valanceSubmarine3 = 1.1;
			this.battleTargetKind = BattleTargetKind.Other;
		}

		public abstract T GetResultData(FormationDatas formation, BattleCommandParams cParam);

		public abstract void Dispose();

		public bool IsAtapSlotItem(int type3No)
		{
			return type3No == 37;
		}

		protected virtual void setTargetingKind(FormationDatas formation)
		{
			if ((formation.F_Formation == BattleFormationKinds1.TanJuu || formation.F_Formation == BattleFormationKinds1.FukuJuu) && (formation.E_Formation == BattleFormationKinds1.TanJuu || formation.E_Formation == BattleFormationKinds1.FukuJuu))
			{
				if (formation.BattleFormation == BattleFormationKinds2.Doukou)
				{
					this.battleTargetKind = BattleTargetKind.Case1;
				}
				else if (formation.BattleFormation == BattleFormationKinds2.Hankou)
				{
					this.battleTargetKind = BattleTargetKind.Case2;
				}
				else if (formation.BattleFormation == BattleFormationKinds2.T_Own)
				{
					this.battleTargetKind = BattleTargetKind.Case3;
				}
				else if (formation.BattleFormation == BattleFormationKinds2.T_Enemy)
				{
					this.battleTargetKind = BattleTargetKind.Case4;
				}
			}
		}

		protected virtual bool isAttackerFromTargetKind(BattleShipSubInfo subInfo)
		{
			return true;
		}

		protected bool RecoveryShip(int idx)
		{
			if (this.practiceFlag)
			{
				return false;
			}
			if (!this.F_Data.LostFlag.get_Item(idx))
			{
				return false;
			}
			if (this.F_Data.ShipData.get_Item(idx).Nowhp > 0 || this.F_Data.ShipData.get_Item(idx).Escape_sts)
			{
				return false;
			}
			int[] array = this.F_Data.ShipData.get_Item(idx).FindRecoveryItem();
			if (!this.F_Data.ShipData.get_Item(idx).UseRecoveryItem(array, idx == 0))
			{
				return false;
			}
			this.F_Data.LostFlag.set_Item(idx, false);
			if (array[0] != 2147483647)
			{
				this.F_Data.SlotData.get_Item(idx).RemoveAt(array[0]);
			}
			return true;
		}

		protected Mem_ship getAtackTarget(Mem_ship attacker, List<Mem_ship> targetShips, bool overKill, bool subMarineFlag, bool rescueFlag, ref BattleDamageKinds dKind)
		{
			Dictionary<int, Mst_stype> stypes = Mst_DataManager.Instance.Mst_stype;
			IEnumerable<Mem_ship> enumerable;
			if (!overKill)
			{
				enumerable = Enumerable.Select(Enumerable.Where(Enumerable.Select(targetShips, (Mem_ship target) => new
				{
					target = target,
					submarineCheck = (subMarineFlag == stypes.get_Item(target.Stype).IsSubmarine())
				}), <>__TranspIdent7 => <>__TranspIdent7.target.IsFight() && <>__TranspIdent7.submarineCheck), <>__TranspIdent7 => <>__TranspIdent7.target);
			}
			else
			{
				enumerable = Enumerable.Select(Enumerable.Where(Enumerable.Select(targetShips, (Mem_ship target) => new
				{
					target = target,
					submarineCheck = (subMarineFlag == stypes.get_Item(target.Stype).IsSubmarine())
				}), <>__TranspIdent8 => !<>__TranspIdent8.target.Escape_sts && <>__TranspIdent8.submarineCheck), <>__TranspIdent8 => <>__TranspIdent8.target);
			}
			dKind = BattleDamageKinds.Normal;
			if (enumerable == null || Enumerable.Count<Mem_ship>(enumerable) == 0)
			{
				return null;
			}
			if (Enumerable.Count<Mem_ship>(enumerable) == 1)
			{
				return Enumerable.First<Mem_ship>(enumerable);
			}
			List<Mem_ship> list;
			if (attacker.IsEnemy())
			{
				list = this.targetFillter(this.E_SubInfo.get_Item(attacker.Rid).DeckIdx, enumerable, this.F_SubInfo);
			}
			else
			{
				list = this.targetFillter(this.F_SubInfo.get_Item(attacker.Rid).DeckIdx, enumerable, this.E_SubInfo);
			}
			Mem_ship mem_ship;
			if (list.get_Count() == 0)
			{
				mem_ship = Enumerable.First<Mem_ship>(Enumerable.OrderBy<Mem_ship, Guid>(Enumerable.ToArray<Mem_ship>(enumerable), (Mem_ship x) => Guid.NewGuid()));
			}
			else
			{
				mem_ship = Enumerable.First<Mem_ship>(Enumerable.OrderBy<Mem_ship, Guid>(list, (Mem_ship x) => Guid.NewGuid()));
			}
			if (Enumerable.Count<Mem_ship>(enumerable) <= 1)
			{
				return mem_ship;
			}
			Mem_ship flagShip = (!attacker.IsEnemy()) ? this.E_Data.ShipData.get_Item(0) : this.F_Data.ShipData.get_Item(0);
			if (!rescueFlag || mem_ship.Rid != flagShip.Rid)
			{
				return mem_ship;
			}
			Dictionary<int, Mst_ship> mShipDict = Mst_DataManager.Instance.Mst_ship;
			Dictionary<int, Mst_stype> mStypeDict = Mst_DataManager.Instance.Mst_stype;
			if (mStypeDict.get_Item(mem_ship.Stype).IsLandFacillity(mShipDict.get_Item(mem_ship.Ship_id).Soku))
			{
				return mem_ship;
			}
			IEnumerable<Mem_ship> enumerable2 = Enumerable.Select(Enumerable.Where(Enumerable.Where(Enumerable.Where(Enumerable.Select(Enumerable.Select(enumerable, (Mem_ship re_target) => new
			{
				re_target = re_target,
				mShipObj = mShipDict.get_Item(re_target.Ship_id)
			}), <>__TranspIdent9 => new
			{
				<>__TranspIdent9 = <>__TranspIdent9,
				mStypeObj = mStypeDict.get_Item(<>__TranspIdent9.re_target.Stype)
			}), <>__TranspIdent10 => <>__TranspIdent10.<>__TranspIdent9.re_target.Get_DamageState() == DamageState.Normal), <>__TranspIdent10 => !<>__TranspIdent10.mStypeObj.IsLandFacillity(<>__TranspIdent10.<>__TranspIdent9.mShipObj.Soku)), <>__TranspIdent10 => <>__TranspIdent10.<>__TranspIdent9.re_target.Rid != flagShip.Rid), <>__TranspIdent10 => <>__TranspIdent10.<>__TranspIdent9.re_target);
			if (!Enumerable.Any<Mem_ship>(enumerable2))
			{
				return mem_ship;
			}
			BattleFormationKinds1 battleFormationKinds = (!mem_ship.IsEnemy()) ? this.F_Data.Formation : this.E_Data.Formation;
			int num = 60;
			if (battleFormationKinds == BattleFormationKinds1.TanJuu)
			{
				num = 45;
			}
			else if (battleFormationKinds == BattleFormationKinds1.Rinkei)
			{
				num = 75;
			}
			if (num > this.randInstance.Next(100))
			{
				return mem_ship;
			}
			dKind = BattleDamageKinds.Rescue;
			return Enumerable.First<Mem_ship>(Enumerable.OrderBy<Mem_ship, Guid>(Enumerable.ToArray<Mem_ship>(enumerable2), (Mem_ship x) => Guid.NewGuid()));
		}

		private List<Mem_ship> targetFillter(int attackerIdx, IEnumerable<Mem_ship> targetShips, Dictionary<int, BattleShipSubInfo> targetSubInfo)
		{
			List<Mem_ship> list = Enumerable.ToList<Mem_ship>(targetShips);
			if (this.battleTargetKind == BattleTargetKind.Other)
			{
				return list;
			}
			HashSet<int> enableIdx = this.getTargetFillterEnableList(attackerIdx, !list.get_Item(0).IsEnemy());
			list.RemoveAll((Mem_ship x) => !enableIdx.Contains(targetSubInfo.get_Item(x.Rid).DeckIdx));
			return list;
		}

		private HashSet<int> getTargetFillterEnableList(int attacker_idx, bool enemyFlag)
		{
			HashSet<int> hashSet;
			if (this.battleTargetKind == BattleTargetKind.Case1)
			{
				if (attacker_idx == 0)
				{
					hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					return hashSet;
				}
				if (attacker_idx == 1)
				{
					hashSet = new HashSet<int>();
					hashSet.Add(0);
					hashSet.Add(1);
					hashSet.Add(2);
					return hashSet;
				}
				if (attacker_idx == 2)
				{
					hashSet = new HashSet<int>();
					hashSet.Add(1);
					hashSet.Add(2);
					hashSet.Add(3);
					return hashSet;
				}
				if (attacker_idx == 3)
				{
					hashSet = new HashSet<int>();
					hashSet.Add(2);
					hashSet.Add(3);
					hashSet.Add(4);
					return hashSet;
				}
				if (attacker_idx == 4)
				{
					hashSet = new HashSet<int>();
					hashSet.Add(3);
					hashSet.Add(4);
					hashSet.Add(5);
					return hashSet;
				}
				if (attacker_idx == 5)
				{
					hashSet = new HashSet<int>();
					hashSet.Add(4);
					hashSet.Add(5);
					return hashSet;
				}
			}
			if (this.battleTargetKind != BattleTargetKind.Case2)
			{
				if (this.battleTargetKind == BattleTargetKind.Case3 || this.battleTargetKind == BattleTargetKind.Case4)
				{
					if (enemyFlag && this.battleTargetKind == BattleTargetKind.Case3)
					{
						hashSet = new HashSet<int>();
						hashSet.Add(0);
						hashSet.Add(1);
						hashSet.Add(2);
						hashSet.Add(3);
						hashSet.Add(4);
						hashSet.Add(5);
						return hashSet;
					}
					if (!enemyFlag && this.battleTargetKind == BattleTargetKind.Case4)
					{
						hashSet = new HashSet<int>();
						hashSet.Add(0);
						hashSet.Add(1);
						hashSet.Add(2);
						hashSet.Add(3);
						hashSet.Add(4);
						hashSet.Add(5);
						return hashSet;
					}
					if (attacker_idx == 0)
					{
						hashSet = new HashSet<int>();
						hashSet.Add(0);
						return hashSet;
					}
					if (attacker_idx == 1)
					{
						hashSet = new HashSet<int>();
						hashSet.Add(0);
						hashSet.Add(1);
						return hashSet;
					}
					if (attacker_idx == 2 || attacker_idx == 3)
					{
						hashSet = new HashSet<int>();
						hashSet.Add(0);
						hashSet.Add(1);
						hashSet.Add(2);
						return hashSet;
					}
					if (attacker_idx == 4)
					{
						hashSet = new HashSet<int>();
						hashSet.Add(1);
						hashSet.Add(2);
						hashSet.Add(3);
						return hashSet;
					}
					if (attacker_idx == 5)
					{
						hashSet = new HashSet<int>();
						hashSet.Add(2);
						hashSet.Add(3);
						hashSet.Add(4);
						return hashSet;
					}
				}
				return new HashSet<int>();
			}
			if (attacker_idx == 0)
			{
				hashSet = new HashSet<int>();
				hashSet.Add(0);
				hashSet.Add(1);
				return hashSet;
			}
			if (attacker_idx == 1)
			{
				hashSet = new HashSet<int>();
				hashSet.Add(0);
				hashSet.Add(1);
				hashSet.Add(2);
				return hashSet;
			}
			if (attacker_idx == 2)
			{
				hashSet = new HashSet<int>();
				hashSet.Add(0);
				hashSet.Add(1);
				hashSet.Add(2);
				return hashSet;
			}
			hashSet = new HashSet<int>();
			hashSet.Add(0);
			hashSet.Add(1);
			hashSet.Add(2);
			hashSet.Add(3);
			hashSet.Add(4);
			hashSet.Add(5);
			return hashSet;
		}

		protected int getBattleAvo(FormationDatas.GetFormationKinds battleState, Mem_ship targetShip)
		{
			double num = (double)targetShip.Kaihi + Math.Sqrt((double)(targetShip.GetBattleBaseParam().Luck * 2));
			BattleFormationKinds1 formation;
			if (!targetShip.IsEnemy())
			{
				formation = this.F_Data.Formation;
				BattleFormationKinds2 battleFormation = this.F_Data.BattleFormation;
				BattleFormationKinds1 formation2 = this.E_Data.Formation;
			}
			else
			{
				formation = this.E_Data.Formation;
				BattleFormationKinds2 battleFormation = this.E_Data.BattleFormation;
				BattleFormationKinds1 formation2 = this.F_Data.Formation;
			}
			num *= this.formationData.GetFormationParamF3(battleState, formation);
			double num2 = (double)((int)num);
			if (num2 >= 65.0)
			{
				double num3 = 55.0 + Math.Sqrt(num2 - 65.0) * 2.0;
				num2 = (double)((int)num3);
			}
			else if (num2 >= 40.0)
			{
				double num4 = 40.0 + Math.Sqrt(num2 - 40.0) * 3.0;
				num2 = (double)((int)num4);
			}
			num2 += this.getAvoHosei(targetShip);
			if (!targetShip.IsEnemy() && this.commandParams != null)
			{
				double num5 = (double)this.commandParams.Rspp / 100.0;
				double num6 = num2 * num5;
				num2 += num6;
			}
			int num7 = 100;
			double num8 = (double)Mst_DataManager.Instance.Mst_ship.get_Item(targetShip.Ship_id).Fuel_max;
			if (num8 != 0.0)
			{
				num7 = (int)((double)targetShip.Fuel / num8 * 100.0);
			}
			if (num7 < 75)
			{
				num2 -= (double)(75 - num7);
			}
			return (int)num2;
		}

		protected int getBattleAvo_Midnight(FormationDatas.GetFormationKinds battleState, Mem_ship targetShip, bool haveSearchLight)
		{
			int battleAvo = this.getBattleAvo(battleState, targetShip);
			double num = (targetShip.Stype != 5 && targetShip.Stype != 6) ? ((double)battleAvo) : ((double)battleAvo + 5.0);
			if (haveSearchLight)
			{
				num *= 0.2;
			}
			return (int)num;
		}

		protected abstract double getAvoHosei(Mem_ship target);

		protected virtual BattleHitStatus getHitStatus(int hitProb, int avoProb, Mem_ship attackShip, Mem_ship targetShip, double cliticalKeisu, bool airAttack)
		{
			double num = (double)(hitProb - avoProb);
			FatigueState fatigueState = targetShip.Get_FatigueState();
			if (num <= 10.0)
			{
				num = 10.0;
			}
			double num2 = 1.0;
			if (fatigueState == FatigueState.Exaltation)
			{
				num2 = 0.7;
			}
			else if (fatigueState == FatigueState.Normal)
			{
				num2 = 1.0;
			}
			else if (fatigueState == FatigueState.Light)
			{
				num2 = 1.2;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num2 = 1.4;
			}
			num *= num2;
			if (num >= 96.0)
			{
				num = 96.0;
			}
			double num3 = 0.0;
			double num4 = 1.0;
			if (airAttack)
			{
				this.setCliticalAlv(ref num, ref num3, ref num4, attackShip);
			}
			int num5 = this.randInstance.Next(100);
			double num6 = Math.Sqrt(num) * cliticalKeisu;
			if ((double)num5 <= num6)
			{
				return BattleHitStatus.Clitical;
			}
			if ((double)num5 > num)
			{
				return BattleHitStatus.Miss;
			}
			return BattleHitStatus.Normal;
		}

		protected virtual double getLandFacciilityKeisu(List<Mst_slotitem> slot_item)
		{
			if (slot_item.Contains(Mst_DataManager.Instance.Mst_Slotitem.get_Item(35)))
			{
				return 2.5;
			}
			return 1.0;
		}

		protected void setCliticalAlv(ref double check_value, ref double cht1, ref double cht2, Mem_ship attacker)
		{
			List<Mst_slotitem> list;
			Dictionary<int, int[]> slotExperience;
			if (attacker.IsEnemy())
			{
				list = this.E_Data.SlotData.get_Item(this.E_SubInfo.get_Item(attacker.Rid).DeckIdx);
				slotExperience = this.E_Data.SlotExperience;
			}
			else
			{
				list = this.F_Data.SlotData.get_Item(this.F_SubInfo.get_Item(attacker.Rid).DeckIdx);
				slotExperience = this.F_Data.SlotExperience;
			}
			int num = 0;
			int num2 = 0;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(11);
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			for (int i = 0; i < attacker.Slot.get_Count(); i++)
			{
				int num3 = attacker.Slot.get_Item(i);
				if (num3 > 0 && attacker.Onslot.get_Item(i) > 0)
				{
					if (hashSet2.Contains(list.get_Item(i).Api_mapbattle_type3))
					{
						int[] array = null;
						if (slotExperience.TryGetValue(num3, ref array))
						{
							if (array[0] >= 10)
							{
							}
							if (array[0] >= 25)
							{
							}
							if (array[0] >= 40)
							{
							}
							if (array[0] >= 55)
							{
							}
							if (array[0] >= 70)
							{
							}
							double num4;
							if (array[0] >= 80)
							{
								num4 = 7.0;
							}
							else
							{
								num4 = 10.0;
							}
							if (i == 0)
							{
								cht1 += num4 * 0.8;
								double num5 = (Math.Sqrt((double)array[0]) + num4) / 100.0;
								cht2 += (double)((int)num5);
							}
							else
							{
								cht1 += num4 * 0.8;
								double num6 = (Math.Sqrt((double)array[0]) + num4) / 200.0;
								cht2 += (double)((int)num6);
							}
							num += array[0];
							num2++;
						}
					}
				}
			}
			cht1 = (double)((int)cht1);
			double num7 = 0.0;
			if (num2 > 0)
			{
				num7 = (double)num / (double)num2;
			}
			if (num7 >= 10.0)
			{
				check_value += (double)((int)Math.Sqrt(num7 * 0.1));
			}
			if (num7 >= 25.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 40.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 55.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 70.0)
			{
				check_value += 1.0;
			}
			if (num7 >= 80.0)
			{
				check_value += 2.0;
			}
			if (num7 >= 100.0)
			{
				check_value += 3.0;
			}
		}

		protected virtual int setDamageValue(BattleHitStatus hitType, int atkPow, int defPow, Mem_ship attacker, Mem_ship target, List<bool> lostFlag)
		{
			if (hitType == BattleHitStatus.Miss)
			{
				return 0;
			}
			if (hitType == BattleHitStatus.Clitical)
			{
				atkPow = (int)((double)atkPow * 1.5);
			}
			double num = (double)atkPow - ((double)defPow * 0.7 + (double)this.randInstance.Next(defPow) * 0.6);
			double num2 = 100.0;
			double num3 = (double)Mst_DataManager.Instance.Mst_ship.get_Item(attacker.Ship_id).Bull_max;
			if (num3 != 0.0)
			{
				num2 = (double)attacker.Bull / num3 * 100.0;
			}
			if (num2 < 50.0)
			{
				num = Math.Floor(num * num2 / 50.0);
			}
			int num4 = (int)num;
			BattleShipSubInfo battleShipSubInfo;
			int deckIdx2;
			if (attacker.IsEnemy())
			{
				battleShipSubInfo = this.E_SubInfo.get_Item(attacker.Rid);
				int deckIdx = battleShipSubInfo.DeckIdx;
				deckIdx2 = this.F_SubInfo.get_Item(target.Rid).DeckIdx;
			}
			else
			{
				battleShipSubInfo = this.F_SubInfo.get_Item(attacker.Rid);
				int deckIdx = battleShipSubInfo.DeckIdx;
				deckIdx2 = this.E_SubInfo.get_Item(target.Rid).DeckIdx;
			}
			if (num4 < 1)
			{
				int num5 = this.randInstance.Next(target.Nowhp);
				num4 = (int)((double)target.Nowhp * 0.06 + (double)num5 * 0.08);
			}
			if (num4 >= target.Nowhp && !target.IsEnemy() && deckIdx2 == 0)
			{
				num4 = (int)((double)target.Nowhp * 0.5 + (double)this.randInstance.Next(target.Nowhp) * 0.3);
			}
			if (lostFlag != null && num4 >= target.Nowhp && !lostFlag.get_Item(deckIdx2))
			{
				num4 = target.Nowhp - 1;
			}
			double lovHoseiDamageKeisu = this.getLovHoseiDamageKeisu(target, num4);
			num4 = (int)((double)num4 * lovHoseiDamageKeisu);
			battleShipSubInfo.TotalDamage += num4;
			target.SubHp(num4);
			return num4;
		}

		private double getLovHoseiDamageKeisu(Mem_ship targetShip, int damage)
		{
			if (targetShip.IsEnemy())
			{
				return 1.0;
			}
			double randDouble = Utils.GetRandDouble(1.0, 100.0, 1.0, 1);
			DamageState damageState = targetShip.Get_DamageState();
			DamageState damageState2 = Mem_ship.Get_DamageState(targetShip.Nowhp - damage, targetShip.Maxhp);
			if (targetShip.Lov >= 330)
			{
				if (randDouble <= 70.0)
				{
					if (damageState == DamageState.Normal && damageState2 == DamageState.Tyuuha)
					{
						return 0.5;
					}
					if (damageState == DamageState.Normal && damageState2 == DamageState.Taiha)
					{
						return 0.5;
					}
				}
				return 1.0;
			}
			if (targetShip.Lov >= 200)
			{
				if (randDouble <= 60.0 && damageState == DamageState.Normal && damageState2 == DamageState.Taiha)
				{
					return 0.55;
				}
				return 1.0;
			}
			else
			{
				if (targetShip.Lov < 100)
				{
					return 1.0;
				}
				if (randDouble <= 50.0 && damageState == DamageState.Normal && damageState2 == DamageState.Taiha)
				{
					return 0.6;
				}
				return 1.0;
			}
		}

		protected virtual KeyValuePair<int, int> getSubMarineAtackKeisu(List<Mem_ship> targetShips, Mem_ship attacker, List<Mst_slotitem> attacker_items, bool midnight)
		{
			if (!Enumerable.Any<Mem_ship>(targetShips, (Mem_ship x) => x.IsFight() && Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsSubmarine()))
			{
				return new KeyValuePair<int, int>(0, 0);
			}
			if (!this.practiceFlag && attacker.IsEnemy() && attacker.Stype == 7)
			{
				string yomi = Mst_DataManager.Instance.Mst_ship.get_Item(attacker.Ship_id).Yomi;
				if (!yomi.Equals("flagship"))
				{
					return new KeyValuePair<int, int>(0, 0);
				}
			}
			if (!midnight && attacker.Ship_id == 352)
			{
				DamageState damageState = attacker.Get_DamageState();
				if (this.isHaveSubmarineAirPlane(attacker_items, attacker.Onslot) && damageState <= DamageState.Tyuuha)
				{
					return new KeyValuePair<int, int>(2, 5);
				}
			}
			if (attacker.GetBattleBaseParam().Taisen > 0)
			{
				return new KeyValuePair<int, int>(1, 10);
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(6);
			hashSet.Add(7);
			hashSet.Add(10);
			hashSet.Add(16);
			hashSet.Add(17);
			HashSet<int> hashSet2 = hashSet;
			if (!midnight && hashSet2.Contains(attacker.Stype) && attacker_items.get_Count() > 0 && this.isHaveSubmarineAirPlane(attacker_items, attacker.Onslot))
			{
				return new KeyValuePair<int, int>(2, 5);
			}
			return new KeyValuePair<int, int>(0, 0);
		}

		private bool isHaveSubmarineAirPlane(List<Mst_slotitem> slotItems, List<int> onSlot)
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(11);
			hashSet.Add(25);
			hashSet.Add(26);
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			using (var enumerator = Enumerable.Select(slotItems, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (current.obj.Tais > 0 && onSlot.get_Item(current.idx) > 0 && hashSet2.Contains(current.obj.Api_mapbattle_type3))
					{
						return true;
					}
				}
			}
			return false;
		}

		protected virtual int getSubmarineAttackValue(KeyValuePair<int, int> submarineKeisu, Mem_ship attacker, List<Mst_slotitem> attackerSlot, List<int> slotLevels)
		{
			if (submarineKeisu.get_Key() == 0)
			{
				return 0;
			}
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			if (!attacker.IsEnemy())
			{
				formation = this.F_Data.Formation;
				battleFormation = this.F_Data.BattleFormation;
				BattleFormationKinds1 formation2 = this.E_Data.Formation;
			}
			else
			{
				formation = this.E_Data.Formation;
				battleFormation = this.E_Data.BattleFormation;
				BattleFormationKinds1 formation2 = this.F_Data.Formation;
			}
			int value = submarineKeisu.get_Value();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(8);
			hashSet.Add(11);
			hashSet.Add(14);
			hashSet.Add(15);
			hashSet.Add(25);
			hashSet.Add(26);
			hashSet.Add(40);
			HashSet<int> hashSet2 = hashSet;
			HashSet<int> hashSet3 = new HashSet<int>();
			int num = 0;
			double num2 = 0.0;
			using (var enumerator = Enumerable.Select(attackerSlot, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (hashSet2.Contains(current.obj.Api_mapbattle_type3))
					{
						num += current.obj.Tais;
						hashSet3.Add(current.obj.Api_mapbattle_type3);
					}
					if ((current.obj.Api_mapbattle_type3 == 14 || current.obj.Api_mapbattle_type3 == 15 || current.obj.Api_mapbattle_type3 == 40) && slotLevels.get_Item(current.idx) > 0)
					{
						num2 += Math.Sqrt((double)slotLevels.get_Item(current.idx));
					}
				}
			}
			double num3 = Math.Sqrt((double)(attacker.GetBattleBaseParam().Taisen * 2));
			double num4 = (double)num * 1.5;
			double num5 = this.valanceSubmarine1 + num3 + num4 + num2;
			double formationParamF = this.formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.SUBMARINE, formation);
			double formationParamBattle = this.formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.SUBMARINE, battleFormation);
			double num6 = 1.0;
			if (hashSet3.Contains(15) && (hashSet3.Contains(14) || hashSet3.Contains(40)))
			{
				num6 = 1.15;
			}
			num5 *= formationParamBattle;
			num5 *= formationParamF;
			num5 *= num6;
			DamageState damageState = attacker.Get_DamageState();
			if (damageState == DamageState.Tyuuha)
			{
				num5 *= 0.7;
			}
			else if (damageState == DamageState.Taiha)
			{
				num5 *= 0.4;
			}
			num5 = Math.Floor(num5);
			if (num5 > 100.0)
			{
				num5 = 100.0 + Math.Sqrt(num5 - 100.0);
			}
			return (int)num5;
		}

		protected int getSubmarineHitProb(Mem_ship attackShip, List<Mst_slotitem> attackSlot, List<int> slotLevels)
		{
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			if (!attackShip.IsEnemy())
			{
				formation = this.F_Data.Formation;
				BattleFormationKinds2 battleFormation = this.F_Data.BattleFormation;
				formation2 = this.E_Data.Formation;
			}
			else
			{
				formation = this.E_Data.Formation;
				BattleFormationKinds2 battleFormation = this.E_Data.BattleFormation;
				formation2 = this.F_Data.Formation;
			}
			int num = 0;
			double num2 = 0.0;
			using (var enumerator = Enumerable.Select(attackSlot, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (current.obj.Api_mapbattle_type3 == 14)
					{
						num += current.obj.Tais;
					}
					if ((current.obj.Api_mapbattle_type3 == 14 || current.obj.Api_mapbattle_type3 == 15 || current.obj.Api_mapbattle_type3 == 40) && slotLevels.get_Item(current.idx) > 0)
					{
						num2 += Math.Sqrt((double)slotLevels.get_Item(current.idx)) * 1.3;
					}
				}
			}
			double num3 = Math.Sqrt((double)attackShip.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt((double)(attackShip.Level * 2)) + (double)num * 2.0;
			double num4 = this.valanceSubmarine2 + num3 + num2;
			double formationParamF = this.formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.SUBMARINE, formation, formation2);
			num4 *= formationParamF;
			FatigueState fatigueState = attackShip.Get_FatigueState();
			double num5 = 1.0;
			if (fatigueState == FatigueState.Exaltation)
			{
				num5 = 1.2;
			}
			else if (fatigueState == FatigueState.Normal)
			{
				num5 = 1.0;
			}
			else if (fatigueState == FatigueState.Light)
			{
				num5 = 0.8;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num5 = 0.5;
			}
			double num6 = num4 * num5;
			if (this.practiceFlag)
			{
				num6 *= 1.5;
			}
			return (int)num6;
		}

		protected int getAtapKeisu(int atap_num)
		{
			if (atap_num == 1)
			{
				return 75;
			}
			if (atap_num == 2)
			{
				return 110;
			}
			if (atap_num == 3)
			{
				return 140;
			}
			if (atap_num >= 4)
			{
				return 160;
			}
			return 0;
		}
	}
}
