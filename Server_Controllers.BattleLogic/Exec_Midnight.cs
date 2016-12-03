using Common.Enum;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Midnight : BattleLogicBase<NightBattleFmt>
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected double fValance1;

		protected double fValance2;

		protected double fValance3;

		protected double eValance1;

		protected double eValance2;

		protected double eValance3;

		private int exec_type;

		private List<int> f_AtkIdxs;

		private List<int> e_AtkIdxs;

		private Dictionary<Mst_slotitem, HashSet<Mem_ship>> fTouchPlane = new Dictionary<Mst_slotitem, HashSet<Mem_ship>>();

		private Dictionary<Mst_slotitem, HashSet<Mem_ship>> eTouchPlane = new Dictionary<Mst_slotitem, HashSet<Mem_ship>>();

		private List<int> fSerchLightIdxs = new List<int>();

		private List<int> eSerchLightIdxs = new List<int>();

		private List<int> fFlareIdxs = new List<int>();

		private List<int> eFlareIdxs = new List<int>();

		private int[] seikuValue;

		private Dictionary<BattleAtackKinds_Night, double> spAttackKeisu;

		private Dictionary<BattleAtackKinds_Night, double> spHitProbKeisu;

		private readonly HashSet<int> disableSlotPlusAttackItems;

		private readonly HashSet<int> disableSlotPlusHitItems;

		private readonly HashSet<int> enableSlotPlusHitDentan;

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

		public Exec_Midnight(int type, int[] seikuValue, BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			this._f_Data = myData;
			this._e_Data = enemyData;
			this._f_SubInfo = mySubInfo;
			this._e_SubInfo = enemySubInfo;
			this.practiceFlag = practice;
			this.exec_type = type;
			this.f_AtkIdxs = null;
			this.makeAttackerData(this._f_Data.ShipData, out this.f_AtkIdxs);
			this.e_AtkIdxs = null;
			this.makeAttackerData(this._e_Data.ShipData, out this.e_AtkIdxs);
			if (seikuValue != null)
			{
				this.seikuValue = seikuValue;
			}
			else if (this.exec_type == 2)
			{
				this.seikuValue = new int[]
				{
					1,
					1
				};
			}
			else
			{
				this.seikuValue = new int[2];
			}
			this.makeSpSlotItem(this._f_Data, this.seikuValue[0]);
			this.makeSpSlotItem(this._e_Data, this.seikuValue[1]);
			this.fValance1 = (this.eValance1 = 0.0);
			this.fValance2 = (this.eValance2 = 69.0);
			this.fValance3 = (this.eValance3 = 1.5);
			if (this.exec_type == 1 || this.exec_type == 2)
			{
				Dictionary<BattleAtackKinds_Night, double> dictionary = new Dictionary<BattleAtackKinds_Night, double>();
				dictionary.Add(BattleAtackKinds_Night.Normal, 1.0);
				dictionary.Add(BattleAtackKinds_Night.Syu_Rai, 1.2);
				dictionary.Add(BattleAtackKinds_Night.Rai_Rai, 1.3);
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Fuku, 1.5);
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Syu, 1.75);
				dictionary.Add(BattleAtackKinds_Night.Renzoku, 2.0);
				this.spAttackKeisu = dictionary;
				dictionary = new Dictionary<BattleAtackKinds_Night, double>();
				dictionary.Add(BattleAtackKinds_Night.Normal, 1.0);
				dictionary.Add(BattleAtackKinds_Night.Syu_Rai, 1.1);
				dictionary.Add(BattleAtackKinds_Night.Rai_Rai, 1.5);
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Fuku, 1.65);
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Syu, 1.5);
				dictionary.Add(BattleAtackKinds_Night.Renzoku, 2.0);
				this.spHitProbKeisu = dictionary;
			}
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(12);
			hashSet.Add(13);
			hashSet.Add(21);
			hashSet.Add(14);
			hashSet.Add(40);
			hashSet.Add(16);
			hashSet.Add(27);
			hashSet.Add(28);
			hashSet.Add(17);
			hashSet.Add(15);
			this.disableSlotPlusAttackItems = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(21);
			hashSet.Add(14);
			hashSet.Add(40);
			hashSet.Add(16);
			hashSet.Add(27);
			hashSet.Add(28);
			hashSet.Add(17);
			hashSet.Add(15);
			this.disableSlotPlusHitItems = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(12);
			hashSet.Add(13);
			this.enableSlotPlusHitDentan = hashSet;
		}

		public override void Dispose()
		{
			this.randInstance = null;
			this.f_AtkIdxs.Clear();
			this.e_AtkIdxs.Clear();
			this.spAttackKeisu.Clear();
			this.spHitProbKeisu.Clear();
		}

		public override NightBattleFmt GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			int num = Enumerable.Count<int>(this.f_AtkIdxs);
			int num2 = Enumerable.Count<int>(this.e_AtkIdxs);
			NightBattleFmt nightBattleFmt = new NightBattleFmt(this.F_Data.Deck.Rid, this.F_Data.ShipData, this.E_Data.ShipData);
			this.setTargetingKind(formation);
			this.formationData = formation;
			nightBattleFmt.F_SearchId = this.getSerchLightFirstPos(this.fSerchLightIdxs, this.F_Data.ShipData);
			nightBattleFmt.E_SearchId = this.getSerchLightFirstPos(this.eSerchLightIdxs, this.E_Data.ShipData);
			nightBattleFmt.F_FlareId = this.getFlarePos(this.fFlareIdxs, this.F_Data.ShipData, ref this.fValance2);
			nightBattleFmt.E_FlareId = this.getFlarePos(this.eFlareIdxs, this.E_Data.ShipData, ref this.eValance2);
			if (this.seikuValue[0] >= 1 && this.seikuValue[0] <= 3)
			{
				nightBattleFmt.F_TouchPlane = this.getSyokusetuPlane(this.fTouchPlane);
				this.setTouchPlaneValanceValue(nightBattleFmt.F_TouchPlane, ref this.fValance1, ref this.fValance2, ref this.fValance3);
			}
			if (this.seikuValue[1] >= 1 && this.seikuValue[1] <= 3)
			{
				nightBattleFmt.E_TouchPlane = this.getSyokusetuPlane(this.eTouchPlane);
				this.setTouchPlaneValanceValue(nightBattleFmt.E_TouchPlane, ref this.eValance1, ref this.eValance2, ref this.eValance3);
			}
			int num3 = (num < num2) ? num2 : num;
			for (int i = 0; i < num3; i++)
			{
				if (i >= num && i >= num2)
				{
					return nightBattleFmt;
				}
				if (i < num)
				{
					Hougeki<BattleAtackKinds_Night> hougekiData = this.getHougekiData(this.f_AtkIdxs.get_Item(i), this.F_Data.ShipData.get_Item(this.f_AtkIdxs.get_Item(i)));
					if (hougekiData != null)
					{
						nightBattleFmt.Hougeki.Add(hougekiData);
					}
				}
				if (i < num2)
				{
					Hougeki<BattleAtackKinds_Night> hougekiData2 = this.getHougekiData(this.e_AtkIdxs.get_Item(i), this.E_Data.ShipData.get_Item(this.e_AtkIdxs.get_Item(i)));
					if (hougekiData2 != null)
					{
						nightBattleFmt.Hougeki.Add(hougekiData2);
					}
				}
			}
			return nightBattleFmt;
		}

		private void setTouchPlaneValanceValue(int mst_id, ref double vAttack, ref double vHit, ref double vClitical)
		{
			if (mst_id == 0)
			{
				return;
			}
			int houm = Mst_DataManager.Instance.Mst_Slotitem.get_Item(mst_id).Houm;
			if (houm <= 1)
			{
				vAttack = 5.0;
				vHit *= 1.1;
				vClitical = 1.57;
				return;
			}
			if (houm == 2)
			{
				vAttack = 7.0;
				vHit *= 1.15;
				vClitical = 1.64;
				return;
			}
			if (houm >= 3)
			{
				vAttack = 9.0;
				vHit *= 1.2;
				vClitical = 1.7;
				return;
			}
		}

		private void makeAttackerData(List<Mem_ship> ships, out List<int> atk_idx)
		{
			atk_idx = new List<int>();
			using (var enumerator = Enumerable.Select(ships, (Mem_ship ship, int idx) => new
			{
				ship,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					int num = Mst_DataManager.Instance.Mst_ship.get_Item(current.ship.Ship_id).Houg + Mst_DataManager.Instance.Mst_ship.get_Item(current.ship.Ship_id).Raig;
					if (num > 0)
					{
						atk_idx.Add(current.idx);
					}
				}
			}
		}

		private void makeSpSlotItem(BattleBaseData baseData, int seiku)
		{
			List<int> list = null;
			Dictionary<Mst_slotitem, HashSet<Mem_ship>> dictionary = null;
			List<int> list2 = null;
			if (baseData.ShipData.get_Item(0).IsEnemy())
			{
				list = this.eSerchLightIdxs;
				dictionary = this.eTouchPlane;
				list2 = this.eFlareIdxs;
			}
			else
			{
				list = this.fSerchLightIdxs;
				dictionary = this.fTouchPlane;
				list2 = this.fFlareIdxs;
			}
			List<List<Mst_slotitem>> slotData = baseData.SlotData;
			List<Mem_ship> shipData = baseData.ShipData;
			using (var enumerator = Enumerable.Select(shipData, (Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					if (current.obj.IsFight())
					{
						using (var enumerator2 = Enumerable.Select(slotData.get_Item(current.ship_idx), (Mst_slotitem obj, int slot_idx) => new
						{
							obj,
							slot_idx
						}).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								var current2 = enumerator2.get_Current();
								if (current.obj.Onslot.get_Item(current2.slot_idx) > 0 && current2.obj.Id == 102)
								{
									if (!dictionary.ContainsKey(current2.obj))
									{
										Dictionary<Mst_slotitem, HashSet<Mem_ship>> arg_150_0 = dictionary;
										Mst_slotitem arg_150_1 = current2.obj;
										HashSet<Mem_ship> hashSet = new HashSet<Mem_ship>();
										hashSet.Add(current.obj);
										arg_150_0.Add(arg_150_1, hashSet);
									}
									else
									{
										dictionary.get_Item(current2.obj).Add(current.obj);
									}
								}
								if (current2.obj.Api_mapbattle_type3 == 29)
								{
									if (!list.Contains(current.ship_idx))
									{
										list.Add(current.ship_idx);
									}
								}
								else if (current2.obj.Api_mapbattle_type3 == 33 && !list2.Contains(current.ship_idx))
								{
									list2.Add(current.ship_idx);
								}
							}
						}
					}
				}
			}
		}

		private int getSerchLightFirstPos(List<int> searchInstance, List<Mem_ship> shipInstance)
		{
			using (List<int>.Enumerator enumerator = searchInstance.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (shipInstance.get_Item(current).IsFight())
					{
						return shipInstance.get_Item(current).Rid;
					}
				}
			}
			return 0;
		}

		private int getFlarePos(List<int> flareInstance, List<Mem_ship> shipInstance, ref double valanceHit)
		{
			using (List<int>.Enumerator enumerator = flareInstance.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					int current = enumerator.get_Current();
					if (shipInstance.get_Item(current).IsFight() && shipInstance.get_Item(current).Nowhp > 4)
					{
						if (this.randInstance.Next(100) <= 70)
						{
							valanceHit += 5.0;
							return shipInstance.get_Item(current).Rid;
						}
					}
				}
			}
			return 0;
		}

		private int getSyokusetuPlane(Dictionary<Mst_slotitem, HashSet<Mem_ship>> touchItems)
		{
			IOrderedEnumerable<Mst_slotitem> orderedEnumerable = Enumerable.OrderByDescending<Mst_slotitem, int>(touchItems.get_Keys(), (Mst_slotitem x) => x.Houm);
			using (IEnumerator<Mst_slotitem> enumerator = orderedEnumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_slotitem current = enumerator.get_Current();
					Mst_slotitem mst_slotitem = current;
					using (HashSet<Mem_ship>.Enumerator enumerator2 = touchItems.get_Item(current).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							Mem_ship current2 = enumerator2.get_Current();
							double num = Math.Sqrt((double)mst_slotitem.Saku);
							double num2 = Math.Sqrt((double)current2.Level);
							int num3 = (int)(num * num2);
							if (num3 > this.randInstance.Next(25))
							{
								return mst_slotitem.Id;
							}
						}
					}
				}
			}
			return 0;
		}

		private Hougeki<BattleAtackKinds_Night> getHougekiData(int atk_idx, Mem_ship attacker)
		{
			if (attacker.Get_DamageState() == DamageState.Taiha || !attacker.IsFight())
			{
				return null;
			}
			Func<int, bool> func = null;
			BattleBaseData battleBaseData;
			Dictionary<int, BattleShipSubInfo> dictionary;
			BattleBaseData battleBaseData2;
			Dictionary<int, BattleShipSubInfo> dictionary2;
			List<int> list;
			double cliticalKeisu;
			if (attacker.IsEnemy())
			{
				battleBaseData = this.E_Data;
				dictionary = this.E_SubInfo;
				battleBaseData2 = this.F_Data;
				dictionary2 = this.F_SubInfo;
				func = new Func<int, bool>(base.RecoveryShip);
				list = this.fSerchLightIdxs;
				double num = this.eValance1;
				double num2 = this.eValance2;
				cliticalKeisu = this.eValance3;
			}
			else
			{
				battleBaseData = this.F_Data;
				dictionary = this.F_SubInfo;
				battleBaseData2 = this.E_Data;
				dictionary2 = this.E_SubInfo;
				list = this.eSerchLightIdxs;
				double num = this.fValance1;
				double num2 = this.fValance2;
				cliticalKeisu = this.fValance3;
			}
			if (!this.isAttackerFromTargetKind(dictionary.get_Item(attacker.Rid)))
			{
				return null;
			}
			Hougeki<BattleAtackKinds_Night> hougeki = new Hougeki<BattleAtackKinds_Night>();
			KeyValuePair<int, int> subMarineAtackKeisu = this.getSubMarineAtackKeisu(battleBaseData2.ShipData, attacker, battleBaseData.SlotData.get_Item(atk_idx), true);
			bool flag = false;
			if (subMarineAtackKeisu.get_Key() != 0)
			{
				hougeki.SpType = ((subMarineAtackKeisu.get_Key() != 1) ? BattleAtackKinds_Night.AirAttack : BattleAtackKinds_Night.Bakurai);
				hougeki.Slot_List.Add(0);
				flag = true;
			}
			BattleDamageKinds battleDamageKinds = BattleDamageKinds.Normal;
			Mem_ship atackTarget = base.getAtackTarget(attacker, battleBaseData2.ShipData, false, flag, true, ref battleDamageKinds);
			if (atackTarget == null)
			{
				return null;
			}
			int deckIdx = dictionary2.get_Item(atackTarget.Rid).DeckIdx;
			if (atackTarget.Nowhp > 1 && list.get_Count() > 0 && list.get_Item(0) == deckIdx)
			{
				battleDamageKinds = BattleDamageKinds.Normal;
				atackTarget = base.getAtackTarget(attacker, battleBaseData2.ShipData, false, flag, true, ref battleDamageKinds);
				deckIdx = dictionary2.get_Item(atackTarget.Rid).DeckIdx;
			}
			if (!flag)
			{
				this.setSlotData(atk_idx, attacker, battleBaseData.SlotData.get_Item(atk_idx), atackTarget, hougeki);
			}
			hougeki.Attacker = attacker.Rid;
			int num3 = (hougeki.SpType != BattleAtackKinds_Night.Renzoku) ? 1 : 2;
			HashSet<BattleAtackKinds_Night> hashSet = new HashSet<BattleAtackKinds_Night>();
			hashSet.Add(BattleAtackKinds_Night.Rai_Rai);
			hashSet.Add(BattleAtackKinds_Night.Renzoku);
			hashSet.Add(BattleAtackKinds_Night.Syu_Rai);
			hashSet.Add(BattleAtackKinds_Night.Syu_Syu_Fuku);
			hashSet.Add(BattleAtackKinds_Night.Syu_Syu_Syu);
			HashSet<BattleAtackKinds_Night> hashSet2 = hashSet;
			for (int i = 0; i < num3; i++)
			{
				int soukou = atackTarget.Soukou;
				List<Mst_slotitem> list2 = battleBaseData.SlotData.get_Item(atk_idx);
				int atkPow;
				int hitProb;
				FormationDatas.GetFormationKinds battleState;
				if (flag)
				{
					atkPow = this.getSubmarineAttackValue(subMarineAtackKeisu, attacker, list2, battleBaseData.SlotLevel.get_Item(atk_idx));
					hitProb = base.getSubmarineHitProb(attacker, battleBaseData.SlotData.get_Item(atk_idx), battleBaseData.SlotLevel.get_Item(atk_idx));
					battleState = FormationDatas.GetFormationKinds.SUBMARINE;
				}
				else
				{
					atkPow = this.getMidnightAttackValue(hougeki.SpType, attacker, list2, atackTarget);
					hitProb = this.getMidnightHitProb(hougeki.SpType, attacker, list2, list);
					battleState = FormationDatas.GetFormationKinds.MIDNIGHT;
				}
				int battleAvo_Midnight = base.getBattleAvo_Midnight(battleState, atackTarget, list.Contains(deckIdx));
				BattleHitStatus battleHitStatus = this.getHitStatus(hitProb, battleAvo_Midnight, attacker, atackTarget, cliticalKeisu, false);
				if (battleHitStatus == BattleHitStatus.Miss && hashSet2.Contains(hougeki.SpType))
				{
					battleHitStatus = BattleHitStatus.Normal;
				}
				hougeki.Target.Add(atackTarget.Rid);
				int num4 = this.setDamageValue(battleHitStatus, atkPow, soukou, attacker, atackTarget, battleBaseData2.LostFlag);
				hougeki.Damage.Add(num4);
				hougeki.Clitical.Add(battleHitStatus);
				hougeki.DamageKind.Add(battleDamageKinds);
			}
			if (func != null)
			{
				bool flag2 = func.Invoke(deckIdx);
			}
			return hougeki;
		}

		private int getMidnightAttackValue(BattleAtackKinds_Night kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, Mem_ship def_ship)
		{
			int num = 300;
			List<int> list;
			if (atk_ship.IsEnemy())
			{
				int deckIdx = this.E_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.E_Data.SlotLevel.get_Item(deckIdx);
			}
			else
			{
				int deckIdx2 = this.F_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.F_Data.SlotLevel.get_Item(deckIdx2);
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
					Mst_slotitem obj2 = current.obj;
					if (base.IsAtapSlotItem(obj2.Api_mapbattle_type3))
					{
						num3++;
					}
					num2 += this.getSlotPlus_Attack(obj2, list.get_Item(current.idx));
				}
			}
			double num4 = atk_ship.IsEnemy() ? this.eValance1 : this.fValance1;
			bool flag = Mst_DataManager.Instance.Mst_stype.get_Item(def_ship.Stype).IsLandFacillity(Mst_DataManager.Instance.Mst_ship.get_Item(def_ship.Ship_id).Soku);
			double num5;
			if (flag)
			{
				num5 = num4 + (double)atk_ship.Houg + num2;
				num5 *= this.getLandFacciilityKeisu(atk_slot);
				num5 += (double)base.getAtapKeisu(num3);
			}
			else
			{
				num5 = num4 + (double)atk_ship.Houg + (double)atk_ship.Raig + num2;
			}
			num5 *= this.spAttackKeisu.get_Item(kind);
			DamageState damageState = atk_ship.Get_DamageState();
			double num6 = 1.0;
			if (damageState == DamageState.Tyuuha)
			{
				num6 = 0.7;
			}
			else if (damageState == DamageState.Taiha)
			{
				num6 = 0.4;
			}
			num5 *= num6;
			if (num5 > (double)num)
			{
				num5 = (double)num + Math.Sqrt(num5 - (double)num);
			}
			return (int)num5;
		}

		private double getSlotPlus_Attack(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (this.disableSlotPlusAttackItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			return Math.Sqrt((double)slotLevel);
		}

		private int getMidnightHitProb(BattleAtackKinds_Night kind, Mem_ship atk_ship, List<Mst_slotitem> atk_slot, List<int> deckSearchLight)
		{
			List<int> list;
			if (atk_ship.IsEnemy())
			{
				int deckIdx = this.E_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.E_Data.SlotLevel.get_Item(deckIdx);
			}
			else
			{
				int deckIdx2 = this.F_SubInfo.get_Item(atk_ship.Rid).DeckIdx;
				list = this.F_Data.SlotLevel.get_Item(deckIdx2);
			}
			double num = 0.0;
			int num2 = 0;
			using (var enumerator = Enumerable.Select(atk_slot, (Mst_slotitem obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					num2 += current.obj.Houm;
					num += this.getSlotPlus_HitProb(current.obj, list.get_Item(current.idx));
				}
			}
			double num3 = Math.Sqrt((double)atk_ship.GetBattleBaseParam().Luck * 1.5) + Math.Sqrt((double)atk_ship.Level) * 2.0 + (double)num2;
			double num4 = (!atk_ship.IsEnemy()) ? this.fValance2 : this.eValance2;
			num3 = num4 + num3;
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
			double formationParamF = this.formationData.GetFormationParamF2(FormationDatas.GetFormationKinds.MIDNIGHT, formation, formation2);
			num3 = num3 * this.spHitProbKeisu.get_Item(kind) * formationParamF;
			FatigueState fatigueState = atk_ship.Get_FatigueState();
			double num5 = 1.0;
			if (fatigueState == FatigueState.Exaltation)
			{
				num5 = 1.2;
			}
			else if (fatigueState == FatigueState.Light)
			{
				num5 = 0.8;
			}
			else if (fatigueState == FatigueState.Distress)
			{
				num5 = 0.5;
			}
			num3 *= num5;
			if (atk_ship.Stype == 5 || atk_ship.Stype == 6)
			{
				int num6 = 0;
				if (atk_slot.Contains(Mst_DataManager.Instance.Mst_Slotitem.get_Item(6)))
				{
					num6 = 10;
				}
				else if (atk_slot.Contains(Mst_DataManager.Instance.Mst_Slotitem.get_Item(50)))
				{
					num6 = 15;
				}
				num3 += (double)num6;
			}
			if (deckSearchLight.get_Count() > 0)
			{
				num3 += 7.0;
			}
			num3 = this.getMidnightHitProbUpValue(num3, atk_ship, atk_slot);
			return (int)num3;
		}

		private double getSlotPlus_HitProb(Mst_slotitem mstItem, int slotLevel)
		{
			double result = 0.0;
			if (slotLevel <= 0)
			{
				return result;
			}
			if (this.disableSlotPlusHitItems.Contains(mstItem.Api_mapbattle_type3))
			{
				return 0.0;
			}
			double num;
			if (this.enableSlotPlusHitDentan.Contains(mstItem.Api_mapbattle_type3) && mstItem.Houm >= 3)
			{
				num = 1.6;
			}
			else
			{
				num = 1.3;
			}
			return Math.Sqrt((double)slotLevel) * num;
		}

		private double getMidnightHitProbUpValue(double hit_prob, Mem_ship atk_ship, List<Mst_slotitem> atk_slot)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(atk_ship.Ship_id);
			if (atk_ship.Stype != 8 && atk_ship.Stype != 10 && (atk_ship.Stype != 9 || mst_ship.Taik > 92))
			{
				return hit_prob;
			}
			ILookup<int, Mst_slotitem> lookup = Enumerable.ToLookup<Mst_slotitem, int>(atk_slot, (Mst_slotitem x) => x.Id);
			Dictionary<int, HashSet<int>> dictionary = new Dictionary<int, HashSet<int>>();
			Dictionary<int, HashSet<int>> arg_8C_0 = dictionary;
			int arg_8C_1 = 1;
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(9);
			arg_8C_0.Add(arg_8C_1, hashSet);
			Dictionary<int, HashSet<int>> arg_A7_0 = dictionary;
			int arg_A7_1 = 2;
			hashSet = new HashSet<int>();
			hashSet.Add(117);
			arg_A7_0.Add(arg_A7_1, hashSet);
			Dictionary<int, HashSet<int>> arg_CB_0 = dictionary;
			int arg_CB_1 = 3;
			hashSet = new HashSet<int>();
			hashSet.Add(105);
			hashSet.Add(8);
			arg_CB_0.Add(arg_CB_1, hashSet);
			Dictionary<int, HashSet<int>> arg_10D_0 = dictionary;
			int arg_10D_1 = 4;
			hashSet = new HashSet<int>();
			hashSet.Add(7);
			hashSet.Add(103);
			hashSet.Add(104);
			hashSet.Add(76);
			hashSet.Add(114);
			arg_10D_0.Add(arg_10D_1, hashSet);
			Dictionary<int, HashSet<int>> dictionary2 = dictionary;
			Dictionary<int, int> dictionary3 = new Dictionary<int, int>();
			using (Dictionary<int, HashSet<int>>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, HashSet<int>> current = enumerator.get_Current();
					int num = 0;
					using (HashSet<int>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
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
			if (atk_ship.Stype == 8)
			{
				num3 = num3 - 10.0 * num2 * Math.Sqrt((double)dictionary3.get_Item(1)) - 5.0 * num2 * Math.Sqrt((double)dictionary3.get_Item(3)) - 7.0 * num2 * Math.Sqrt((double)dictionary3.get_Item(2));
				num3 += 4.0 * Math.Sqrt((double)dictionary3.get_Item(4));
			}
			else if (atk_ship.Stype == 10)
			{
				num3 = num3 - 8.0 * num2 * Math.Sqrt((double)dictionary3.get_Item(1)) - 5.0 * num2 * Math.Sqrt((double)dictionary3.get_Item(2));
				num3 = num3 + 4.0 * Math.Sqrt((double)dictionary3.get_Item(4)) + 2.0 * Math.Sqrt((double)dictionary3.get_Item(3));
			}
			else if (atk_ship.Stype == 9)
			{
				num3 = num3 - 7.0 * num2 * Math.Sqrt((double)dictionary3.get_Item(1)) - 3.0 * num2 * Math.Sqrt((double)dictionary3.get_Item(2));
				num3 = num3 + 2.0 * Math.Sqrt((double)dictionary3.get_Item(4)) + 2.0 * Math.Sqrt((double)dictionary3.get_Item(3));
			}
			return num3;
		}

		private void setSlotData(int attackerIdx, Mem_ship attacker, List<Mst_slotitem> atk_slot, Mem_ship target, Hougeki<BattleAtackKinds_Night> setData)
		{
			Mst_stype mst_stype = Mst_DataManager.Instance.Mst_stype.get_Item(target.Stype);
			if (mst_stype.IsSubmarine())
			{
				setData.Slot_List.Add(0);
				setData.SpType = BattleAtackKinds_Night.Normal;
				return;
			}
			if (atk_slot == null || atk_slot.get_Count() == 0)
			{
				setData.Slot_List.Add(0);
				setData.SpType = BattleAtackKinds_Night.Normal;
				return;
			}
			int luck = attacker.GetBattleBaseParam().Luck;
			int num = (int)((double)(luck + 15) + Math.Sqrt((double)attacker.Level) * 0.75);
			if (luck >= 50)
			{
				num = (int)(65.0 + (Math.Sqrt((double)luck) - 50.0) + Math.Sqrt((double)attacker.Level) * 0.8);
			}
			if (Enumerable.Any<Mst_slotitem>(atk_slot, (Mst_slotitem x) => x.Id == 129))
			{
				num += 5;
			}
			List<int> list;
			List<int> list2;
			List<int> list3;
			List<int> list4;
			if (attacker.IsEnemy())
			{
				list = this.eSerchLightIdxs;
				list2 = this.fSerchLightIdxs;
				list3 = this.eFlareIdxs;
				list4 = this.fFlareIdxs;
			}
			else
			{
				list = this.fSerchLightIdxs;
				list2 = this.eSerchLightIdxs;
				list3 = this.fFlareIdxs;
				list4 = this.eFlareIdxs;
			}
			if (list3.get_Count() > 0)
			{
				num += 4;
			}
			if (list4.get_Count() > 0)
			{
				num -= 10;
			}
			if (list.get_Count() > 0)
			{
				num += 7;
			}
			if (list2.get_Count() > 0)
			{
				num -= 5;
			}
			if (attacker.Get_DamageState() == DamageState.Tyuuha)
			{
				num += 18;
			}
			if (attackerIdx == 0)
			{
				num += 15;
			}
			List<int> list5 = new List<int>();
			list5.Add(1);
			list5.Add(2);
			list5.Add(3);
			List<int> list6 = list5;
			list5 = new List<int>();
			list5.Add(4);
			List<int> list7 = list5;
			list5 = new List<int>();
			list5.Add(5);
			list5.Add(32);
			List<int> list8 = list5;
			List<Mst_slotitem> list9 = new List<Mst_slotitem>();
			List<Mst_slotitem> list10 = new List<Mst_slotitem>();
			List<Mst_slotitem> list11 = new List<Mst_slotitem>();
			List<Mst_slotitem> list12 = new List<Mst_slotitem>();
			int soku = Mst_DataManager.Instance.Mst_ship.get_Item(target.Ship_id).Soku;
			using (List<Mst_slotitem>.Enumerator enumerator = atk_slot.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mst_slotitem current = enumerator.get_Current();
					if (list6.Contains(current.Api_mapbattle_type3))
					{
						list9.Add(current);
						list12.Add(current);
					}
					else if (list7.Contains(current.Api_mapbattle_type3))
					{
						list10.Add(current);
						list12.Add(current);
					}
					else if (list8.Contains(current.Api_mapbattle_type3) && !mst_stype.IsLandFacillity(soku))
					{
						list11.Add(current);
						list12.Add(current);
					}
				}
			}
			if (list12.get_Count() == 0)
			{
				setData.Slot_List.Add(0);
				setData.SpType = BattleAtackKinds_Night.Normal;
				return;
			}
			List<BattleAtackKinds_Night> list13 = new List<BattleAtackKinds_Night>();
			Dictionary<BattleAtackKinds_Night, List<int>> dictionary = new Dictionary<BattleAtackKinds_Night, List<int>>();
			if (list9.get_Count() >= 3)
			{
				list13.Add(BattleAtackKinds_Night.Syu_Syu_Syu);
				list5 = new List<int>();
				list5.Add(list9.get_Item(0).Id);
				list5.Add(list9.get_Item(1).Id);
				list5.Add(list9.get_Item(2).Id);
				List<int> list14 = list5;
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Syu, list14);
			}
			if (list9.get_Count() >= 2 && list10.get_Count() >= 1)
			{
				list13.Add(BattleAtackKinds_Night.Syu_Syu_Fuku);
				list5 = new List<int>();
				list5.Add(list9.get_Item(0).Id);
				list5.Add(list9.get_Item(1).Id);
				list5.Add(list10.get_Item(0).Id);
				List<int> list15 = list5;
				dictionary.Add(BattleAtackKinds_Night.Syu_Syu_Fuku, list15);
			}
			if (list11.get_Count() >= 2)
			{
				list13.Add(BattleAtackKinds_Night.Rai_Rai);
				list5 = new List<int>();
				list5.Add(list11.get_Item(0).Id);
				list5.Add(list11.get_Item(1).Id);
				List<int> list16 = list5;
				dictionary.Add(BattleAtackKinds_Night.Rai_Rai, list16);
			}
			if (list11.get_Count() >= 1 && list9.get_Count() >= 1)
			{
				list13.Add(BattleAtackKinds_Night.Syu_Rai);
				list5 = new List<int>();
				list5.Add(list9.get_Item(0).Id);
				list5.Add(list11.get_Item(0).Id);
				List<int> list17 = list5;
				dictionary.Add(BattleAtackKinds_Night.Syu_Rai, list17);
			}
			if (list12.get_Count() >= 2)
			{
				list13.Add(BattleAtackKinds_Night.Renzoku);
				list5 = new List<int>();
				list5.Add(list12.get_Item(0).Id);
				list5.Add(list12.get_Item(1).Id);
				List<int> list18 = list5;
				dictionary.Add(BattleAtackKinds_Night.Renzoku, list18);
			}
			setData.SpType = BattleAtackKinds_Night.Normal;
			setData.Slot_List.Add(list12.get_Item(0).Id);
			Dictionary<BattleAtackKinds_Night, int> dictionary2 = new Dictionary<BattleAtackKinds_Night, int>();
			dictionary2.Add(BattleAtackKinds_Night.Syu_Syu_Syu, 140);
			dictionary2.Add(BattleAtackKinds_Night.Syu_Syu_Fuku, 130);
			dictionary2.Add(BattleAtackKinds_Night.Rai_Rai, 122);
			dictionary2.Add(BattleAtackKinds_Night.Syu_Rai, 115);
			dictionary2.Add(BattleAtackKinds_Night.Renzoku, 110);
			Dictionary<BattleAtackKinds_Night, int> dictionary3 = dictionary2;
			using (List<BattleAtackKinds_Night>.Enumerator enumerator2 = list13.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BattleAtackKinds_Night current2 = enumerator2.get_Current();
					int num2 = this.randInstance.Next(dictionary3.get_Item(current2));
					if (num > num2)
					{
						setData.SpType = current2;
						setData.Slot_List = dictionary.get_Item(current2);
						break;
					}
				}
			}
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
