using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Raigeki : BattleLogicBase<Raigeki>
	{
		protected BattleBaseData _f_Data;

		protected BattleBaseData _e_Data;

		protected Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		protected Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected List<int> f_AtkIdxs;

		protected List<int> e_AtkIdxs;

		protected List<int> f_startHp;

		protected List<int> e_startHp;

		protected double valance1;

		protected double valance2;

		protected double valance3;

		protected HashSet<int> enableRaigSlotPlusItems;

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

		public Exec_Raigeki(int atkType, BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			this._f_Data = myData;
			this._e_Data = enemyData;
			this._f_SubInfo = mySubInfo;
			this._e_SubInfo = enemySubInfo;
			this.f_AtkIdxs = new List<int>();
			this.e_AtkIdxs = new List<int>();
			this.f_startHp = new List<int>();
			this.e_startHp = new List<int>();
			this.practiceFlag = practice;
			if (atkType == 1 || atkType == 3)
			{
				this.makeAttackerData(false);
			}
			if (atkType == 2 || atkType == 3)
			{
				this.makeAttackerData(true);
			}
			this.valance1 = 5.0;
			this.valance2 = 85.0;
			this.valance3 = 1.5;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(5);
			hashSet.Add(22);
			this.enableRaigSlotPlusItems = hashSet;
		}

		public override Raigeki GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			this.commandParams = cParam;
			this.setTargetingKind(formation);
			int num = Enumerable.Count<int>(this.f_AtkIdxs);
			int num2 = Enumerable.Count<int>(this.e_AtkIdxs);
			if (num == 0 && num2 == 0)
			{
				return null;
			}
			if (num > 0)
			{
				this.setHitHoseiFromBattleCommand();
			}
			Raigeki raigeki = new Raigeki();
			this.formationData = formation;
			raigeki.F_Rai = this.getRaigekiData(false);
			raigeki.E_Rai = this.getRaigekiData(true);
			using (var enumerator = Enumerable.Select(this.F_Data.ShipData, (Mem_ship obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (current.obj.Nowhp <= 0)
					{
						bool flag = base.RecoveryShip(current.idx);
					}
				}
			}
			return raigeki;
		}

		public override void Dispose()
		{
			this.randInstance = null;
			this.f_AtkIdxs.Clear();
			this.e_AtkIdxs.Clear();
			this.f_startHp.Clear();
			this.e_startHp.Clear();
		}

		protected virtual void setHitHoseiFromBattleCommand()
		{
		}

		protected virtual RaigekiInfo getRaigekiData(bool enemyFlag)
		{
			RaigekiInfo raigekiInfo = new RaigekiInfo();
			List<int> list;
			BattleBaseData battleBaseData;
			Dictionary<int, BattleShipSubInfo> dictionary;
			BattleBaseData battleBaseData2;
			Dictionary<int, BattleShipSubInfo> dictionary2;
			if (enemyFlag)
			{
				list = this.e_AtkIdxs;
				battleBaseData = this.E_Data;
				dictionary = this.E_SubInfo;
				battleBaseData2 = this.F_Data;
				dictionary2 = this.F_SubInfo;
			}
			else
			{
				list = this.f_AtkIdxs;
				battleBaseData = this.F_Data;
				dictionary = this.F_SubInfo;
				battleBaseData2 = this.E_Data;
				dictionary2 = this.E_SubInfo;
			}
			int num = Enumerable.Count<int>(list);
			if (num == 0)
			{
				return raigekiInfo;
			}
			List<Mem_ship> list2 = Enumerable.ToList<Mem_ship>(battleBaseData2.ShipData);
			Dictionary<int, Mst_stype> mst_stype = Mst_DataManager.Instance.Mst_stype;
			Dictionary<int, Mst_ship> mst_ship = Mst_DataManager.Instance.Mst_ship;
			list2.RemoveAll((Mem_ship x) => x.Nowhp <= 0 || mst_stype.get_Item(x.Stype).IsLandFacillity(mst_ship.get_Item(x.Ship_id).Soku));
			if (list2.get_Count() == 0)
			{
				return raigekiInfo;
			}
			for (int i = 0; i < num; i++)
			{
				int num2 = list.get_Item(i);
				Mem_ship mem_ship = battleBaseData.ShipData.get_Item(num2);
				List<Mst_slotitem> atk_slot = battleBaseData.SlotData.get_Item(num2);
				if (this.isAttackerFromTargetKind(dictionary.get_Item(mem_ship.Rid)))
				{
					BattleDamageKinds battleDamageKinds = BattleDamageKinds.Normal;
					Mem_ship atackTarget = base.getAtackTarget(mem_ship, list2, true, false, true, ref battleDamageKinds);
					if (atackTarget != null)
					{
						int deckIdx = dictionary2.get_Item(atackTarget.Rid).DeckIdx;
						int raigAttackValue = this.getRaigAttackValue(mem_ship, atk_slot, atackTarget);
						int soukou = atackTarget.Soukou;
						int raigHitProb = this.getRaigHitProb(mem_ship, atk_slot, raigAttackValue);
						int battleAvo = base.getBattleAvo(FormationDatas.GetFormationKinds.RAIGEKI, atackTarget);
						BattleHitStatus hitStatus = this.getHitStatus(raigHitProb, battleAvo, mem_ship, atackTarget, this.valance3, false);
						int num3 = this.setDamageValue(hitStatus, raigAttackValue, soukou, mem_ship, atackTarget, battleBaseData2.LostFlag);
						raigekiInfo.Damage[num2] = num3;
						raigekiInfo.Target[num2] = deckIdx;
						raigekiInfo.DamageKind[num2] = battleDamageKinds;
						raigekiInfo.Clitical[num2] = hitStatus;
					}
				}
			}
			return raigekiInfo;
		}

		protected virtual int getRaigAttackValue(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship)
		{
			int num = 150;
			BattleFormationKinds1 formation;
			BattleFormationKinds2 battleFormation;
			List<int> list;
			if (!atk_ship.IsEnemy())
			{
				formation = this.F_Data.Formation;
				battleFormation = this.F_Data.BattleFormation;
				list = this.F_Data.SlotLevel.get_Item(this.F_SubInfo.get_Item(atk_ship.Rid).DeckIdx);
				BattleFormationKinds1 formation2 = this.E_Data.Formation;
			}
			else
			{
				formation = this.E_Data.Formation;
				battleFormation = this.E_Data.BattleFormation;
				list = this.E_Data.SlotLevel.get_Item(this.E_SubInfo.get_Item(atk_ship.Rid).DeckIdx);
				BattleFormationKinds1 formation2 = this.F_Data.Formation;
			}
			double num2 = 0.0;
			using (var enumerator = Enumerable.Select(atk_slot, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					num2 += this.getSlotPlus_Attack(current.obj, list.get_Item(current.idx));
				}
			}
			double num3 = this.valance1 + (double)atk_ship.Raig + num2;
			double formationParamBattle = this.formationData.GetFormationParamBattle(FormationDatas.GetFormationKinds.RAIGEKI, battleFormation);
			double formationParamF = this.formationData.GetFormationParamF1(FormationDatas.GetFormationKinds.RAIGEKI, formation);
			num3 = num3 * formationParamBattle * formationParamF;
			DamageState damageState = atk_ship.Get_DamageState();
			double num4 = 1.0;
			if (damageState == DamageState.Tyuuha)
			{
				num4 = 0.8;
			}
			else if (damageState == DamageState.Taiha)
			{
				num4 = 0.0;
			}
			num3 *= num4;
			if (num3 > (double)num)
			{
				num3 = (double)num + Math.Sqrt(num3 - (double)num);
			}
			return (int)num3;
		}

		protected virtual double getSlotPlus_Attack(Mst_slotitem mstItem, int slotLevel)
		{
			if (slotLevel <= 0)
			{
				return 0.0;
			}
			if (!this.enableRaigSlotPlusItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			return Math.Sqrt((double)slotLevel) * 1.2;
		}

		protected virtual int getRaigHitProb(Mem_ship atk_ship, List<Mst_slotitem> atk_slot, int atk_pow)
		{
			double num = 0.0;
			List<int> list;
			BattleFormationKinds1 formation;
			BattleFormationKinds1 formation2;
			if (!atk_ship.IsEnemy())
			{
				list = this.F_Data.SlotLevel.get_Item(this.F_SubInfo.get_Item(atk_ship.Rid).DeckIdx);
				formation = this.F_Data.Formation;
				formation2 = this.E_Data.Formation;
				num = (double)this.commandParams.Tspp / 100.0;
			}
			else
			{
				list = this.E_Data.SlotLevel.get_Item(this.E_SubInfo.get_Item(atk_ship.Rid).DeckIdx);
				formation = this.E_Data.Formation;
				formation2 = this.F_Data.Formation;
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
					num2 += this.getSlotPlus_HitProb(current.obj, list.get_Item(current.idx));
				}
			}
			int raim = Mst_DataManager.Instance.Mst_ship.get_Item(atk_ship.Ship_id).Raim;
			double num4 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt((double)atk_ship.Level) * 2.0 + (double)num3;
			num4 = num4 + (double)((int)((double)atk_pow * 0.2)) + (double)raim;
			num4 = this.valance2 + num4 + num2;
			double formationParamF = this.formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.RAIGEKI, formation, formation2);
			num4 *= formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num5 = 1.0;
			if (fatigueState == FatigueState.Exaltation)
			{
				num5 = 1.3;
			}
			else if (fatigueState == FatigueState.Light)
			{
				num5 = 0.7;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num5 = 0.35;
			}
			num4 *= num5;
			double num6 = num4 * num;
			num4 += num6;
			return (int)num4;
		}

		protected virtual double getSlotPlus_HitProb(Mst_slotitem mstItem, int slotLevel)
		{
			if (slotLevel <= 0)
			{
				return 0.0;
			}
			if (!this.enableRaigSlotPlusItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			return Math.Sqrt((double)slotLevel) * 2.0;
		}

		protected virtual void makeAttackerData(bool enemyFlag)
		{
			List<int> atkInstance = null;
			List<int> hpInstance = null;
			BattleBaseData battleBaseData;
			bool flag;
			if (!enemyFlag)
			{
				atkInstance = this.f_AtkIdxs;
				battleBaseData = this.F_Data;
				hpInstance = this.f_startHp;
				flag = this.isRaigBattleCommand();
			}
			else
			{
				atkInstance = this.e_AtkIdxs;
				battleBaseData = this.E_Data;
				hpInstance = this.e_startHp;
				flag = true;
			}
			if (!flag)
			{
				return;
			}
			int ins_idx = 0;
			battleBaseData.ShipData.ForEach(delegate(Mem_ship x)
			{
				hpInstance.Add(x.Nowhp);
				if (this.isValidRaigeki(x))
				{
					atkInstance.Add(ins_idx);
				}
				ins_idx++;
			});
		}

		protected virtual bool isRaigBattleCommand()
		{
			return true;
		}

		protected virtual bool isValidRaigeki(Mem_ship ship)
		{
			return ship.Get_DamageState() <= DamageState.Shouha && ship.IsFight() && ship.GetBattleBaseParam().Raig > 0;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			List<Mst_slotitem> list;
			List<int> list2;
			if (target.IsEnemy())
			{
				int deckIdx = this.E_SubInfo.get_Item(target.Rid).DeckIdx;
				list = this.E_Data.SlotData.get_Item(deckIdx);
				list2 = this.E_Data.SlotLevel.get_Item(deckIdx);
			}
			else
			{
				int deckIdx2 = this.F_SubInfo.get_Item(target.Rid).DeckIdx;
				list = this.F_Data.SlotData.get_Item(deckIdx2);
				list2 = this.F_Data.SlotLevel.get_Item(deckIdx2);
			}
			double num = 0.0;
			using (var enumerator = Enumerable.Select(list, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (current.obj.Api_mapbattle_type3 == 14 || current.obj.Api_mapbattle_type3 == 40)
					{
						num += Math.Sqrt((double)list2.get_Item(current.idx)) * 1.5;
					}
				}
			}
			return num;
		}
	}
}
