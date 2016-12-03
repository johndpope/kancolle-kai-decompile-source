using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_SupportHougeki : Exec_Hougeki
	{
		public Exec_SupportHougeki(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice) : base(null, new int[2], myData, mySubInfo, enemyData, enemySubInfo, practice)
		{
			this.AIR_ATACK_KEISU = 15;
			this.valance1 = 4.0;
			this.valance2 = 64.0;
			this.valance3 = 1.0;
		}

		[EditorBrowsable]
		public override HougekiDayBattleFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return null;
		}

		public T GetResultData<T>(FormationDatas formation) where T : Support_HouRai, new()
		{
			this.formationData = formation;
			Support_HouRai hougekiData = this.getHougekiData();
			return (T)((object)hougekiData);
		}

		protected override void setTargetingKind(FormationDatas formationDatas)
		{
			this.battleTargetKind = BattleTargetKind.Other;
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private Support_HouRai getHougekiData()
		{
			BattleBaseData f_Data = this.F_Data;
			BattleBaseData e_Data = this.E_Data;
			List<Mem_ship> list = Enumerable.ToList<Mem_ship>(e_Data.ShipData);
			list.RemoveAll((Mem_ship x) => x.Nowhp <= 0 || Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsSubmarine());
			if (Enumerable.Count<Mem_ship>(list) == 0)
			{
				return null;
			}
			Support_HouRai support_HouRai = new Support_HouRai();
			int i = 0;
			while (i < this.F_Data.ShipData.get_Count())
			{
				Mem_ship mem_ship = f_Data.ShipData.get_Item(i);
				List<Mst_slotitem> list2 = f_Data.SlotData.get_Item(i);
				BattleAtackKinds_Day kind = BattleAtackKinds_Day.Normal;
				if (base.IsAirAttackGroup(mem_ship, list2, BattleCommand.None))
				{
					if (base.CanAirAtack_DamageState(mem_ship))
					{
						if (base.CanAirAttack(mem_ship, list2))
						{
							kind = BattleAtackKinds_Day.AirAttack;
							goto IL_CA;
						}
					}
				}
				else if (base.isValidHougeki(mem_ship))
				{
					goto IL_CA;
				}
				IL_1B3:
				i++;
				continue;
				IL_CA:
				BattleDamageKinds battleDamageKinds = BattleDamageKinds.Normal;
				Mem_ship atackTarget = base.getAtackTarget(mem_ship, list, true, false, true, ref battleDamageKinds);
				if (atackTarget == null)
				{
					goto IL_1B3;
				}
				int deckIdx = this.E_SubInfo.get_Item(atackTarget.Rid).DeckIdx;
				int hougAttackValue = this.getHougAttackValue(kind, mem_ship, list2, atackTarget, 0);
				int soukou = atackTarget.Soukou;
				int hougHitProb = this.getHougHitProb(kind, mem_ship, list2, 0);
				int battleAvo = base.getBattleAvo(FormationDatas.GetFormationKinds.HOUGEKI, atackTarget);
				BattleHitStatus hitStatus = this.getHitStatus(hougHitProb, battleAvo, mem_ship, atackTarget, this.valance3, false);
				int num = this.setDamageValue(hitStatus, hougAttackValue, soukou, mem_ship, atackTarget, e_Data.LostFlag);
				support_HouRai.Damage[deckIdx] += num;
				if (hitStatus != BattleHitStatus.Miss && support_HouRai.Clitical[deckIdx] != BattleHitStatus.Clitical)
				{
					support_HouRai.Clitical[deckIdx] = hitStatus;
				}
				if (support_HouRai.DamageType[deckIdx] != BattleDamageKinds.Rescue)
				{
					support_HouRai.DamageType[deckIdx] = battleDamageKinds;
					goto IL_1B3;
				}
				goto IL_1B3;
			}
			return support_HouRai;
		}

		protected override int getHougAttackValue(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship, int tekkouKind)
		{
			int num = 150;
			int num2 = 0;
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
					num2 += current.obj.Baku;
					num3 += current.obj.Raig;
				}
			}
			double num4 = this.valance1 + (double)atk_ship.Houg;
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(def_ship.Stype).IsLandFacillity(Mst_DataManager.Instance.Mst_ship.get_Item(def_ship.Ship_id).Soku);
			if (flag)
			{
				num4 *= this.getLandFacciilityKeisu(atk_slot);
				num3 = 0;
			}
			if (kind == BattleAtackKinds_Day.AirAttack)
			{
				int airAtackPow = this.getAirAtackPow(num2, num3);
				num4 += (double)airAtackPow;
				num4 = 25.0 + (double)((int)(num4 * 1.5));
			}
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			if (!atk_ship.IsEnemy())
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
			double formationParamBattle = this.formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.HOUGEKI, battleFormation);
			double formationParamF = this.formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.HOUGEKI, formation);
			num4 = num4 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num5 = 1.0;
			if (damageState == DamageState.Tyuuha)
			{
				num5 = 0.7;
			}
			else if (damageState == DamageState.Taiha)
			{
				num5 = 0.4;
			}
			num4 *= num5;
			if (num4 > (double)num)
			{
				num4 = (double)num + Math.Sqrt(num4 - (double)num);
			}
			return (int)num4;
		}

		protected override int getHougHitProb(BattleAtackKinds_Day kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int tekkouKind)
		{
			int num = 0;
			using (var enumerator = Enumerable.Select(atk_slot, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					num += current.obj.Houm;
				}
			}
			double num2 = this.valance2 + Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt((double)atk_ship.Level) * 2.0 + (double)num;
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			if (!atk_ship.IsEnemy())
			{
				formation = this.F_Data.Formation;
				formation2 = this.E_Data.Formation;
			}
			else
			{
				formation = this.E_Data.Formation;
				formation2 = this.F_Data.Formation;
			}
			double formationParamF = this.formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.HOUGEKI, formation, formation2);
			num2 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num3 = 1.0;
			if (fatigueState == FatigueState.Exaltation)
			{
				num3 = 1.2;
			}
			else if (fatigueState == FatigueState.Light)
			{
				num3 = 0.8;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num3 = 0.5;
			}
			num2 *= num3;
			return (int)num2;
		}

		protected override int getAirAtackPow(int baku, int raig)
		{
			return base.getAirAtackPow(baku, raig);
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
