using Common.Enum;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class FormationDatas : IDisposable
	{
		public enum GetFormationKinds
		{
			AIR,
			HOUGEKI,
			RAIGEKI,
			MIDNIGHT,
			SUBMARINE
		}

		private BattleFormationKinds1 f_Formation;

		private BattleFormationKinds1 e_Formation;

		private BattleFormationKinds2 battleFormation;

		private Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>> paramF1;

		private Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>> paramF2;

		private Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>> paramF3;

		private Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds2, double>> paramBattleFormattion;

		public BattleFormationKinds1 F_Formation
		{
			get
			{
				return this.f_Formation;
			}
			private set
			{
				this.f_Formation = value;
			}
		}

		public BattleFormationKinds1 E_Formation
		{
			get
			{
				return this.e_Formation;
			}
			private set
			{
				this.e_Formation = value;
			}
		}

		public BattleFormationKinds2 BattleFormation
		{
			get
			{
				return this.battleFormation;
			}
			private set
			{
				this.battleFormation = value;
			}
		}

		public FormationDatas(BattleBaseData fBaseData, BattleBaseData eBaseData, bool practiceFlag)
		{
			this.F_Formation = fBaseData.Formation;
			this.E_Formation = ((!practiceFlag) ? eBaseData.Formation : this.selectEnemyFormation(eBaseData.ShipData.get_Count()));
			this.BattleFormation = this.selectBattleFormation2();
			this.setParamater();
		}

		public void Dispose()
		{
		}

		public BattleFormationKinds2 AfterAirBattle_RewriteBattleFormation2(BattleBaseData fBaseData)
		{
			if (this.BattleFormation == BattleFormationKinds2.T_Enemy)
			{
				using (var enumerator = Enumerable.Select(fBaseData.SlotData, (List<Mst_slotitem> list, int ship_idx) => new
				{
					list,
					ship_idx
				}).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						var current = enumerator.get_Current();
						Mem_ship mem_ship = fBaseData.ShipData.get_Item(current.ship_idx);
						using (var enumerator2 = Enumerable.Select(current.list, (Mst_slotitem obj, int slot_idx) => new
						{
							obj,
							slot_idx
						}).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								var current2 = enumerator2.get_Current();
								if (mem_ship.Onslot.get_Item(current2.slot_idx) > 0)
								{
									if (current2.obj.Id == 54)
									{
										this.BattleFormation = BattleFormationKinds2.Hankou;
										return this.BattleFormation;
									}
								}
							}
						}
					}
				}
			}
			return this.BattleFormation;
		}

		public double GetFormationParamF1(FormationDatas.GetFormationKinds kind, BattleFormationKinds1 attacker)
		{
			return this.paramF1.get_Item(kind).get_Item(attacker);
		}

		public double GetFormationParamF2(FormationDatas.GetFormationKinds kind, BattleFormationKinds1 attacker, BattleFormationKinds1 defencer)
		{
			if (kind == FormationDatas.GetFormationKinds.HOUGEKI)
			{
				if (attacker == BattleFormationKinds1.FukuJuu && defencer == BattleFormationKinds1.TanOu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.Teikei && defencer == BattleFormationKinds1.TanJuu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.TanOu && defencer == BattleFormationKinds1.Teikei)
				{
					return 1.2;
				}
			}
			else if (kind == FormationDatas.GetFormationKinds.SUBMARINE)
			{
				if (attacker == BattleFormationKinds1.FukuJuu && defencer == BattleFormationKinds1.TanOu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.Teikei && defencer == BattleFormationKinds1.TanJuu)
				{
					return 1.2;
				}
				if (attacker == BattleFormationKinds1.TanOu && defencer == BattleFormationKinds1.Teikei)
				{
					return 1.2;
				}
			}
			return this.paramF2.get_Item(kind).get_Item(attacker);
		}

		public double GetFormationParamF3(FormationDatas.GetFormationKinds kind, BattleFormationKinds1 attacker)
		{
			return this.paramF3.get_Item(kind).get_Item(attacker);
		}

		public double GetFormationParamBattle(FormationDatas.GetFormationKinds kind, BattleFormationKinds2 formation)
		{
			return this.paramBattleFormattion.get_Item(kind).get_Item(formation);
		}

		private BattleFormationKinds1 selectEnemyFormation(int enemyCount)
		{
			Dictionary<int, List<BattleFormationKinds1>> dictionary = new Dictionary<int, List<BattleFormationKinds1>>();
			Dictionary<int, List<BattleFormationKinds1>> arg_1B_0 = dictionary;
			int arg_1B_1 = 1;
			List<BattleFormationKinds1> list = new List<BattleFormationKinds1>();
			list.Add(BattleFormationKinds1.TanJuu);
			arg_1B_0.Add(arg_1B_1, list);
			Dictionary<int, List<BattleFormationKinds1>> arg_3C_0 = dictionary;
			int arg_3C_1 = 2;
			list = new List<BattleFormationKinds1>();
			list.Add(BattleFormationKinds1.TanJuu);
			list.Add(BattleFormationKinds1.Teikei);
			arg_3C_0.Add(arg_3C_1, list);
			Dictionary<int, List<BattleFormationKinds1>> arg_65_0 = dictionary;
			int arg_65_1 = 3;
			list = new List<BattleFormationKinds1>();
			list.Add(BattleFormationKinds1.TanJuu);
			list.Add(BattleFormationKinds1.Teikei);
			list.Add(BattleFormationKinds1.Teikei);
			arg_65_0.Add(arg_65_1, list);
			Dictionary<int, List<BattleFormationKinds1>> arg_96_0 = dictionary;
			int arg_96_1 = 4;
			list = new List<BattleFormationKinds1>();
			list.Add(BattleFormationKinds1.TanJuu);
			list.Add(BattleFormationKinds1.Teikei);
			list.Add(BattleFormationKinds1.TanOu);
			list.Add(BattleFormationKinds1.Rinkei);
			arg_96_0.Add(arg_96_1, list);
			Dictionary<int, List<BattleFormationKinds1>> dictionary2 = dictionary;
			int num = (enemyCount <= 4) ? enemyCount : 4;
			if (dictionary2.get_Item(num).get_Count() == 1)
			{
				return dictionary2.get_Item(num).get_Item(0);
			}
			List<BattleFormationKinds1> list2 = dictionary2.get_Item(num);
			var <>__AnonType = Enumerable.First(Enumerable.OrderBy(Enumerable.Select(list2, (BattleFormationKinds1 value) => new
			{
				value
			}), x => Guid.NewGuid()));
			return <>__AnonType.value;
		}

		private BattleFormationKinds2 selectBattleFormation2()
		{
			List<BattleFormationKinds2> list = new List<BattleFormationKinds2>();
			Dictionary<BattleFormationKinds2, int> dictionary = new Dictionary<BattleFormationKinds2, int>();
			dictionary.Add(BattleFormationKinds2.T_Enemy, 10);
			dictionary.Add(BattleFormationKinds2.T_Own, 15);
			dictionary.Add(BattleFormationKinds2.Hankou, 30);
			dictionary.Add(BattleFormationKinds2.Doukou, 45);
			Dictionary<BattleFormationKinds2, int> dictionary2 = dictionary;
			using (Dictionary<BattleFormationKinds2, int>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<BattleFormationKinds2, int> current = enumerator.get_Current();
					BattleFormationKinds2 key = current.get_Key();
					for (int i = 0; i < current.get_Value(); i++)
					{
						list.Add(key);
					}
				}
			}
			var <>__AnonType = Enumerable.First(Enumerable.OrderBy(Enumerable.Select(list, (BattleFormationKinds2 value) => new
			{
				value
			}), x => Guid.NewGuid()));
			return <>__AnonType.value;
		}

		private void setParamater()
		{
			Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>> dictionary = new Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>>();
			Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>> dictionary2 = new Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>>();
			Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>> dictionary3 = new Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds1, double>>();
			Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds2, double>> dictionary4 = new Dictionary<FormationDatas.GetFormationKinds, Dictionary<BattleFormationKinds2, double>>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(FormationDatas.GetFormationKinds)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					FormationDatas.GetFormationKinds getFormationKinds = (FormationDatas.GetFormationKinds)((int)current);
					dictionary.Add(getFormationKinds, new Dictionary<BattleFormationKinds1, double>());
					dictionary2.Add(getFormationKinds, new Dictionary<BattleFormationKinds1, double>());
					dictionary3.Add(getFormationKinds, new Dictionary<BattleFormationKinds1, double>());
					dictionary4.Add(getFormationKinds, new Dictionary<BattleFormationKinds2, double>());
					using (IEnumerator enumerator2 = Enum.GetValues(typeof(BattleFormationKinds1)).GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							object current2 = enumerator2.get_Current();
							BattleFormationKinds1 battleFormationKinds = (BattleFormationKinds1)((int)current2);
							dictionary.get_Item(getFormationKinds).Add(battleFormationKinds, 1.0);
							dictionary2.get_Item(getFormationKinds).Add(battleFormationKinds, 1.0);
							dictionary3.get_Item(getFormationKinds).Add(battleFormationKinds, 1.0);
						}
					}
					using (IEnumerator enumerator3 = Enum.GetValues(typeof(BattleFormationKinds2)).GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							object current3 = enumerator3.get_Current();
							BattleFormationKinds2 battleFormationKinds2 = (BattleFormationKinds2)((int)current3);
							dictionary4.get_Item(getFormationKinds).Add(battleFormationKinds2, 1.0);
						}
					}
				}
			}
			dictionary.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.FukuJuu, 0.8);
			dictionary.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.Rinkei, 0.7);
			dictionary.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.Teikei, 0.6);
			dictionary.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.TanOu, 0.6);
			dictionary.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.FukuJuu, 0.8);
			dictionary.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.Rinkei, 0.7);
			dictionary.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.Teikei, 0.6);
			dictionary.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.TanOu, 0.6);
			dictionary.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.FukuJuu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.Rinkei, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.Teikei, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.TanOu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.FukuJuu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.Rinkei, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.Teikei, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.TanOu, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.TanJuu, 0.6);
			dictionary.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.FukuJuu, 0.8);
			dictionary.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.Rinkei, 1.2);
			dictionary.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.Teikei, 1.0);
			dictionary.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.TanOu, 1.3);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.FukuJuu, 1.2);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.Rinkei, 1.0);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.Teikei, 1.2);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.TanOu, 1.2);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.FukuJuu, 0.8);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.Rinkei, 0.4);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.Teikei, 0.6);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.TanOu, 0.3);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.FukuJuu, 0.9);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.Rinkei, 0.7);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.Teikei, 0.8);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.TanOu, 0.8);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.FukuJuu, 0.9);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.Rinkei, 0.7);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.Teikei, 0.8);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.TanOu, 0.8);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.FukuJuu, 1.2);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.Rinkei, 1.0);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.Teikei, 1.2);
			dictionary2.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.TanOu, 1.2);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.FukuJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.Rinkei, 1.1);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.Teikei, 1.2);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds1.TanOu, 1.3);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.FukuJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.Rinkei, 1.1);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.Teikei, 1.3);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds1.TanOu, 1.4);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.FukuJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.Rinkei, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.Teikei, 1.1);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.MIDNIGHT).set_Item(BattleFormationKinds1.TanOu, 1.2);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.FukuJuu, 1.2);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.Rinkei, 1.6);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.Teikei, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.AIR).set_Item(BattleFormationKinds1.TanOu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.TanJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.FukuJuu, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.Rinkei, 1.0);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.Teikei, 1.3);
			dictionary3.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds1.TanOu, 1.1);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds2.T_Enemy, 0.6);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds2.T_Own, 1.2);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds2.Hankou, 0.8);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.HOUGEKI).set_Item(BattleFormationKinds2.Doukou, 1.0);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds2.T_Enemy, 0.6);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds2.T_Own, 1.2);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds2.Hankou, 0.8);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.RAIGEKI).set_Item(BattleFormationKinds2.Doukou, 1.0);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds2.T_Enemy, 0.6);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds2.T_Own, 1.2);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds2.Hankou, 0.8);
			dictionary4.get_Item(FormationDatas.GetFormationKinds.SUBMARINE).set_Item(BattleFormationKinds2.Doukou, 1.0);
			this.paramF1 = dictionary;
			this.paramF2 = dictionary2;
			this.paramF3 = dictionary3;
			this.paramBattleFormattion = dictionary4;
		}
	}
}
