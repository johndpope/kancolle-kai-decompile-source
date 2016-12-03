using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_SupportRaigeki : Exec_Raigeki
	{
		public Exec_SupportRaigeki(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice) : base(1, myData, mySubInfo, enemyData, enemySubInfo, practice)
		{
			this.valance1 = 8.0;
			this.valance2 = 54.0;
			this.valance3 = 1.2;
		}

		[EditorBrowsable]
		public override Raigeki GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			return null;
		}

		public T GetResultData<T>(FormationDatas formation) where T : Support_HouRai
		{
			this.formationData = formation;
			Support_HouRai raigekiData = this.getRaigekiData();
			return (T)((object)raigekiData);
		}

		protected override void setHitHoseiFromBattleCommand()
		{
		}

		protected override void setTargetingKind(FormationDatas formationDatas)
		{
			this.battleTargetKind = BattleTargetKind.Other;
		}

		protected override void makeAttackerData(bool enemyFlag)
		{
			base.makeAttackerData(enemyFlag);
		}

		protected override bool isRaigBattleCommand()
		{
			return true;
		}

		public override void Dispose()
		{
			base.Dispose();
		}

		private Support_HouRai getRaigekiData()
		{
			BattleBaseData f_Data = this.F_Data;
			BattleBaseData e_Data = this.E_Data;
			List<Mem_ship> list = Enumerable.ToList<Mem_ship>(this.E_Data.ShipData);
			list.RemoveAll((Mem_ship x) => x.Nowhp <= 0 || Mst_DataManager.Instance.Mst_stype.get_Item(x.Stype).IsLandFacillity(Mst_DataManager.Instance.Mst_ship.get_Item(x.Ship_id).Soku));
			if (Enumerable.Count<Mem_ship>(list) == 0)
			{
				return null;
			}
			Support_HouRai support_HouRai = new Support_HouRai();
			using (List<int>.Enumerator enumerator = this.f_AtkIdxs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					Mem_ship mem_ship = f_Data.ShipData.get_Item(current);
					List<Mst_slotitem> atk_slot = f_Data.SlotData.get_Item(current);
					BattleDamageKinds battleDamageKinds = BattleDamageKinds.Normal;
					Mem_ship atackTarget = base.getAtackTarget(mem_ship, list, true, false, true, ref battleDamageKinds);
					if (atackTarget != null)
					{
						int num = e_Data.ShipData.IndexOf(atackTarget);
						int raigAttackValue = this.getRaigAttackValue(mem_ship, atk_slot, atackTarget);
						int soukou = atackTarget.Soukou;
						int raigHitProb = this.getRaigHitProb(mem_ship, atk_slot, raigAttackValue);
						int battleAvo = base.getBattleAvo(FormationDatas.GetFormationKinds.RAIGEKI, atackTarget);
						BattleHitStatus hitStatus = this.getHitStatus(raigHitProb, battleAvo, mem_ship, atackTarget, this.valance3, false);
						int num2 = this.setDamageValue(hitStatus, raigAttackValue, soukou, mem_ship, atackTarget, null);
						support_HouRai.Damage[num] += num2;
						if (hitStatus != BattleHitStatus.Miss && support_HouRai.Clitical[num] != BattleHitStatus.Clitical)
						{
							support_HouRai.Clitical[num] = hitStatus;
						}
						if (support_HouRai.DamageType[num] != BattleDamageKinds.Rescue)
						{
							support_HouRai.DamageType[num] = battleDamageKinds;
						}
					}
				}
			}
			return support_HouRai;
		}

		protected override int getRaigAttackValue(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship)
		{
			int num = 150;
			double num2 = this.valance1 + (double)atk_ship.Raig;
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
			double formationParamBattle = this.formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.RAIGEKI, battleFormation);
			double formationParamF = this.formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.RAIGEKI, formation);
			num2 = num2 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num3 = 1.0;
			if (damageState == DamageState.Tyuuha)
			{
				num3 = 0.8;
			}
			else if (damageState == DamageState.Taiha)
			{
				num3 = 0.0;
			}
			num2 *= num3;
			if (num2 > (double)num)
			{
				num2 = (double)num + Math.Sqrt(num2 - (double)num);
			}
			return (int)num2;
		}

		protected override int getRaigHitProb(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int atk_pow)
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
			double num2 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt((double)atk_ship.Level) * 2.0 + (double)num;
			num2 += (double)((int)((double)atk_pow * 0.35));
			num2 = this.valance2 + num2;
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
			double formationParamF = this.formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.RAIGEKI, formation, formation2);
			num2 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num3 = 1.0;
			if (fatigueState == FatigueState.Exaltation)
			{
				num3 = 1.3;
			}
			else if (fatigueState == FatigueState.Light)
			{
				num3 = 0.7;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num3 = 0.35;
			}
			num2 *= num3;
			return (int)num2;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
