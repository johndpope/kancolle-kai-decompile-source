using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_AirBattle : BattleLogicBase<AirBattle>
	{
		protected BattleBaseData _f_Data;

		protected BattleBaseData _e_Data;

		protected Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		protected Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected Dictionary<Mem_ship, List<FighterInfo>> f_FighterInfo;

		protected Dictionary<Mem_ship, List<FighterInfo>> e_FighterInfo;

		protected bool[] isSearchSuccess;

		protected int[] seikuValue;

		protected double valance1;

		protected double valance2;

		protected double valance3;

		private Mst_slotitem fTouchPlane;

		private Mst_slotitem eTouchPlane;

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

		public Exec_AirBattle(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, SearchInfo[] searchInfo, bool practice)
		{
			this._f_Data = myData;
			this._e_Data = enemyData;
			this._f_SubInfo = mySubInfo;
			this._e_SubInfo = enemySubInfo;
			this.practiceFlag = practice;
			this.setSearchData(searchInfo);
			this.seikuValue = new int[2];
			this.valance1 = 0.25;
			this.valance2 = 95.0;
			this.valance3 = 25.0;
		}

		public override void Dispose()
		{
			if (this.f_FighterInfo != null)
			{
				using (Dictionary<Mem_ship, List<FighterInfo>>.Enumerator enumerator = this.f_FighterInfo.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<Mem_ship, List<FighterInfo>> current = enumerator.get_Current();
						current.get_Value().Clear();
					}
				}
			}
			if (this.e_FighterInfo != null)
			{
				using (Dictionary<Mem_ship, List<FighterInfo>>.Enumerator enumerator2 = this.e_FighterInfo.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<Mem_ship, List<FighterInfo>> current2 = enumerator2.get_Current();
						current2.get_Value().Clear();
					}
				}
			}
			this.randInstance = null;
			this.seikuValue = null;
		}

		public override AirBattle GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			this.commandParams = cParam;
			AirBattle airBattle = new AirBattle();
			if (Enumerable.Any<Mem_ship>(this.F_Data.ShipData, (Mem_ship x) => x.IsFight()))
			{
				if (Enumerable.Any<Mem_ship>(this.E_Data.ShipData, (Mem_ship y) => y.IsFight()))
				{
					this.setTargetingKind(formation);
					if (this.isSearchSuccess[0] && this.isAirBattleCommand())
					{
						this.setPlaneData(this.F_Data, this.F_SubInfo, airBattle.F_PlaneFrom, out this.f_FighterInfo);
					}
					if (this.isSearchSuccess[1])
					{
						this.setPlaneData(this.E_Data, this.E_SubInfo, airBattle.E_PlaneFrom, out this.e_FighterInfo);
					}
					this.formationData = formation;
					airBattle.Air1 = this.getAirBattle1();
					airBattle.Air2 = this.getAirBattle2();
					airBattle.Air3 = this.getAirBattle3(airBattle.Air2);
					airBattle.SetStageFlag();
					return airBattle;
				}
			}
			airBattle.SetStageFlag();
			return airBattle;
		}

		private void setSearchData(SearchInfo[] info)
		{
			this.isSearchSuccess = new bool[2];
			if (info == null)
			{
				this.isSearchSuccess[0] = true;
				this.isSearchSuccess[1] = true;
				return;
			}
			for (int i = 0; i < info.Length; i++)
			{
				if (info[i].SearchValue == BattleSearchValues.Success || info[i].SearchValue == BattleSearchValues.Success_Lost || info[i].SearchValue == BattleSearchValues.Found)
				{
					this.isSearchSuccess[i] = true;
				}
				else
				{
					this.isSearchSuccess[i] = false;
				}
			}
		}

		protected virtual bool isAirBattleCommand()
		{
			return this.F_Data.GetDeckBattleCommand().get_Item(0) == BattleCommand.Kouku;
		}

		protected void setPlaneData(BattleBaseData baseData, Dictionary<int, BattleShipSubInfo> subInfo, List<int> planeFrom, out Dictionary<Mem_ship, List<FighterInfo>> fighterData)
		{
			fighterData = null;
			int count = baseData.ShipData.get_Count();
			if (count <= 0)
			{
				return;
			}
			Dictionary<Mem_ship, List<FighterInfo>> dictionary = new Dictionary<Mem_ship, List<FighterInfo>>();
			using (var enumerator = Enumerable.Select(baseData.SlotData, (List<Mst_slotitem> items, int idx) => new
			{
				items,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					Mem_ship mem_ship = baseData.ShipData.get_Item(current.idx);
					if (mem_ship.IsFight())
					{
						if (this.isAttackerFromTargetKind(subInfo.get_Item(mem_ship.Rid)))
						{
							bool flag = false;
							dictionary.Add(mem_ship, new List<FighterInfo>());
							using (var enumerator2 = Enumerable.Select(current.items, (Mst_slotitem item, int slot_pos) => new
							{
								item,
								slot_pos
							}).GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									var current2 = enumerator2.get_Current();
									if (mem_ship.Onslot.get_Item(current2.slot_pos) > 0 && FighterInfo.ValidFighter(current2.item))
									{
										FighterInfo fighterInfo = new FighterInfo(current2.slot_pos, current2.item);
										dictionary.get_Item(mem_ship).Add(fighterInfo);
										flag = true;
									}
								}
							}
							if (!flag)
							{
								dictionary.Remove(mem_ship);
							}
							else
							{
								planeFrom.Add(mem_ship.Rid);
							}
						}
					}
				}
			}
			if (dictionary.get_Count() > 0)
			{
				fighterData = dictionary;
			}
		}

		public int[] getSeikuValue()
		{
			return new int[]
			{
				this.seikuValue[0],
				this.seikuValue[1]
			};
		}

		protected virtual AirBattle1 getAirBattle1()
		{
			if (this.f_FighterInfo == null && this.e_FighterInfo == null)
			{
				return null;
			}
			AirBattle1 airBattle = new AirBattle1();
			int deckSeikuPow = this.getDeckSeikuPow(this.f_FighterInfo, ref airBattle.F_LostInfo.Count);
			int deckSeikuPow2 = this.getDeckSeikuPow(this.e_FighterInfo, ref airBattle.E_LostInfo.Count);
			int merits = this.getMerits(deckSeikuPow, deckSeikuPow2, ref airBattle.SeikuKind);
			if (airBattle.F_LostInfo.Count > 0 && airBattle.E_LostInfo.Count == 0)
			{
				airBattle.SeikuKind = BattleSeikuKinds.Kakuho;
			}
			if (this.isSearchSuccess[0] && this.isSearchSuccess[1])
			{
				this.battleSeiku(merits, airBattle.E_LostInfo, true);
				this.battleSeiku(merits, airBattle.F_LostInfo, false);
			}
			if (this.seikuValue[0] >= 1 && this.seikuValue[0] <= 3)
			{
				airBattle.F_TouchPlane = this.getSyokusetuInfo(this.seikuValue[0], this.F_Data);
				Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(airBattle.F_TouchPlane, ref this.fTouchPlane);
			}
			if (this.seikuValue[1] >= 1 && this.seikuValue[1] <= 3)
			{
				airBattle.E_TouchPlane = this.getSyokusetuInfo(this.seikuValue[1], this.E_Data);
				Mst_DataManager.Instance.Mst_Slotitem.TryGetValue(airBattle.E_TouchPlane, ref this.eTouchPlane);
			}
			return airBattle;
		}

		protected virtual int getSyokusetuInfo(int seiku, BattleBaseData baseData)
		{
			int num = 0;
			List<Mst_slotitem> list = new List<Mst_slotitem>();
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(8);
			hashSet.Add(9);
			hashSet.Add(10);
			hashSet.Add(41);
			HashSet<int> hashSet2 = hashSet;
			using (var enumerator = Enumerable.Select(baseData.ShipData, (Mem_ship obj, int ship_idx) => new
			{
				obj,
				ship_idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					Mem_ship obj2 = current.obj;
					for (int i = 0; i < Enumerable.Count<int>(obj2.Onslot); i++)
					{
						if (obj2.Onslot.get_Item(i) > 0 && obj2.Slot.get_Item(i) > 0)
						{
							Mst_slotitem mst_slotitem = baseData.SlotData.get_Item(current.ship_idx).get_Item(i);
							if (hashSet2.Contains(mst_slotitem.Api_mapbattle_type3))
							{
								list.Add(mst_slotitem);
							}
							if (mst_slotitem.Api_mapbattle_type3 != 8)
							{
								num += (int)((double)mst_slotitem.Saku * Math.Sqrt((double)obj2.Onslot.get_Item(i)));
							}
						}
					}
				}
			}
			int num2 = 70 - seiku * 15;
			if (num < this.randInstance.Next(num2))
			{
				return 0;
			}
			int slotid = 0;
			int houm = -1;
			list.ForEach(delegate(Mst_slotitem x)
			{
				int num3 = this.randInstance.Next(20 - seiku * 2);
				if (houm < x.Houm && x.Saku > num3)
				{
					slotid = x.Id;
					houm = x.Houm;
				}
			});
			return slotid;
		}

		protected virtual int getDeckSeikuPow(Dictionary<Mem_ship, List<FighterInfo>> fighterData, ref int fighterCount)
		{
			int num = 150;
			int num2 = 0;
			if (fighterData == null)
			{
				return 0;
			}
			if (fighterData.get_Count() == 0)
			{
				return 0;
			}
			Dictionary<int, int[]> dictionary = (!Enumerable.First<KeyValuePair<Mem_ship, List<FighterInfo>>>(fighterData).get_Key().IsEnemy()) ? this.F_Data.SlotExperience : this.E_Data.SlotExperience;
			using (Dictionary<Mem_ship, List<FighterInfo>>.Enumerator enumerator = fighterData.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<FighterInfo>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					using (List<FighterInfo>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FighterInfo current2 = enumerator2.get_Current();
							int num3 = key.Onslot.get_Item(current2.SlotIdx);
							fighterCount += num3;
							int tyku = current2.SlotData.Tyku;
							double num4 = (double)tyku * Math.Sqrt((double)num3);
							double num5 = 0.0;
							int num6 = key.Slot.get_Item(current2.SlotIdx);
							int[] array = null;
							if (dictionary.TryGetValue(num6, ref array))
							{
								num5 = (double)this.getAlvPlusToSeiku(num3, current2.SlotData.Api_mapbattle_type3, array[0]);
								int slotExpUpValueToSeiku = this.getSlotExpUpValueToSeiku(num3, array[0], current2.SlotData.Exp_rate);
								array[1] = array[1] + slotExpUpValueToSeiku;
							}
							num4 += num5;
							num2 += (int)num4;
						}
					}
				}
			}
			if (num2 > num)
			{
				num2 = num + (num2 - num);
			}
			return num2;
		}

		protected virtual int getAlvPlusToSeiku(int onslotNum, int type3No, int slotExp)
		{
			if (onslotNum <= 0)
			{
				return 0;
			}
			double num = Math.Sqrt((double)slotExp * 0.1);
			if (type3No == 6)
			{
				if (slotExp >= 25)
				{
					num += 2.0;
				}
				if (slotExp >= 40)
				{
					num += 3.0;
				}
				if (slotExp >= 55)
				{
					num += 4.0;
				}
				if (slotExp >= 70)
				{
					num += 5.0;
				}
				if (slotExp >= 100)
				{
					num += 8.0;
				}
			}
			else if (type3No == 11)
			{
				if (slotExp >= 25)
				{
					num += 1.0;
				}
				if (slotExp >= 70)
				{
					num += 2.0;
				}
				if (slotExp >= 100)
				{
					num += 3.0;
				}
			}
			return (int)num;
		}

		protected virtual int getSlotExpUpValueToSeiku(int onslotNum, int slotExp, int expUpRate)
		{
			if (onslotNum <= 0)
			{
				return 0;
			}
			double num = (double)expUpRate;
			double num2 = 0.88;
			double num3 = 10.0;
			if (slotExp > 100)
			{
				num3 = 6.0;
			}
			else if (slotExp > 50)
			{
				num3 = 8.0;
			}
			else if (slotExp < 20)
			{
				num3 = 12.0;
			}
			double randDouble = Utils.GetRandDouble(0.0, num3 - 1.0, 1.0, 1);
			double num4 = num * 0.5 + num * randDouble * 0.05 * num2;
			int num5 = slotExp + (int)num4;
			if (num5 > 120)
			{
				num5 = 120;
			}
			return num5 - slotExp;
		}

		protected virtual int getMerits(int f_seiku, int e_seiku, ref BattleSeikuKinds dispSeiku)
		{
			double num = (double)f_seiku * 3.0;
			double num2 = (double)f_seiku * 1.5;
			double num3 = (double)e_seiku * 3.0;
			double num4 = (double)e_seiku * 1.5;
			if ((double)f_seiku >= num3)
			{
				dispSeiku = BattleSeikuKinds.Kakuho;
				this.seikuValue[0] = 3;
				this.seikuValue[1] = 0;
				return 1;
			}
			if (num2 <= (double)e_seiku && (double)e_seiku < num)
			{
				dispSeiku = BattleSeikuKinds.Ressei;
				this.seikuValue[0] = 1;
				this.seikuValue[1] = 2;
				return 7;
			}
			if (num4 <= (double)f_seiku && (double)f_seiku < num3)
			{
				dispSeiku = BattleSeikuKinds.Yuusei;
				this.seikuValue[0] = 2;
				this.seikuValue[1] = 1;
				return 3;
			}
			if ((double)e_seiku >= num)
			{
				dispSeiku = BattleSeikuKinds.Lost;
				this.seikuValue[0] = 0;
				this.seikuValue[1] = 3;
				return 10;
			}
			dispSeiku = BattleSeikuKinds.None;
			this.seikuValue[0] = 0;
			this.seikuValue[1] = 0;
			return 5;
		}

		protected virtual void battleSeiku(int merits, LostPlaneInfo lostFmt, bool enemyFlag)
		{
			if (lostFmt.Count <= 0)
			{
				return;
			}
			Dictionary<int, int[]> dictionary = null;
			Func<double> func = null;
			Dictionary<Mem_ship, List<FighterInfo>> dictionary2;
			if (enemyFlag)
			{
				dictionary = this.E_Data.SlotExperience;
				dictionary2 = this.e_FighterInfo;
				func = (() => (double)this.randInstance.Next(11 - merits + 1) * 0.35 + (double)this.randInstance.Next(11 - merits + 1) * 0.65);
			}
			else
			{
				dictionary = this.F_Data.SlotExperience;
				dictionary2 = this.f_FighterInfo;
				func = delegate
				{
					double max = (double)merits / 3.0;
					return Utils.GetRandDouble(0.0, max, 0.1, 2) + (double)merits / 4.0;
				};
			}
			using (Dictionary<Mem_ship, List<FighterInfo>>.Enumerator enumerator = dictionary2.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<FighterInfo>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					using (List<FighterInfo>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FighterInfo current2 = enumerator2.get_Current();
							int slotIdx = current2.SlotIdx;
							double num = func.Invoke();
							int num2 = (int)Math.Floor((double)key.Onslot.get_Item(slotIdx) * num / 10.0);
							if (num2 > key.Onslot.get_Item(slotIdx))
							{
								num2 = key.Onslot.get_Item(slotIdx);
							}
							int[] array = null;
							if (dictionary.TryGetValue(key.Slot.get_Item(slotIdx), ref array))
							{
								int slotExpSubValueToSeiku = this.getSlotExpSubValueToSeiku(key.Onslot.get_Item(slotIdx), num2, array[0]);
								array[1] = array[1] + slotExpSubValueToSeiku;
							}
							List<int> onslot;
							List<int> expr_157 = onslot = key.Onslot;
							int num3;
							int expr_15C = num3 = slotIdx;
							num3 = onslot.get_Item(num3);
							expr_157.set_Item(expr_15C, num3 - num2);
							lostFmt.LostCount += num2;
						}
					}
				}
			}
		}

		protected virtual int getSlotExpSubValueToSeiku(int onSlot, int lostNum, int slotExp)
		{
			if (onSlot <= 0 || lostNum <= 0)
			{
				return 0;
			}
			double num = (double)onSlot;
			double num2 = (double)lostNum;
			double num3 = (double)slotExp;
			int num4 = (int)(num3 * (num2 / num) * 0.3);
			if (num2 / num > 0.5)
			{
				num4 = (int)(num3 * (num2 / num) * 0.5);
			}
			double num5 = (num4 - 1 > 0) ? Utils.GetRandDouble(0.0, (double)num4, 1.0, 1) : 0.0;
			double num6 = num3 - (double)num4 * 0.5 + num5 * 0.05;
			if (num2 >= num || num6 < 0.0)
			{
				num6 = 0.0;
			}
			return (int)num6 - slotExp;
		}

		protected virtual AirBattle2 getAirBattle2()
		{
			AirBattle2 airBattle = new AirBattle2();
			List<Mem_ship> taikuHaveShip = this.selectTaikuPlane(this.f_FighterInfo, ref airBattle.F_LostInfo.Count);
			List<Mem_ship> taikuHaveShip2 = this.selectTaikuPlane(this.e_FighterInfo, ref airBattle.E_LostInfo.Count);
			bool flag = false;
			if (this.isSearchSuccess[1] && airBattle.E_LostInfo.Count > 0)
			{
				airBattle.F_AntiFire = this.getAntiAirFireAttacker(this.F_Data);
				int deckTaikuPow = this.getDeckTaikuPow(this.F_Data);
				airBattle.E_LostInfo.LostCount = this.battleTaiku(taikuHaveShip2, deckTaikuPow, airBattle.F_AntiFire);
				flag = true;
			}
			if (this.isSearchSuccess[0] && airBattle.F_LostInfo.Count > 0)
			{
				airBattle.E_AntiFire = this.getAntiAirFireAttacker(this.E_Data);
				int deckTaikuPow2 = this.getDeckTaikuPow(this.E_Data);
				airBattle.F_LostInfo.LostCount = this.battleTaiku(taikuHaveShip, deckTaikuPow2, airBattle.E_AntiFire);
				flag = true;
			}
			if (!flag)
			{
				return null;
			}
			return airBattle;
		}

		protected List<Mem_ship> selectTaikuPlane(Dictionary<Mem_ship, List<FighterInfo>> fighter, ref int planeCount)
		{
			if (fighter == null)
			{
				return null;
			}
			List<Mem_ship> list = new List<Mem_ship>();
			using (Dictionary<Mem_ship, List<FighterInfo>>.Enumerator enumerator = fighter.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<FighterInfo>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					int num = 0;
					using (List<FighterInfo>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FighterInfo current2 = enumerator2.get_Current();
							if (current2.ValidTaiku() && key.Onslot.get_Item(current2.SlotIdx) > 0)
							{
								num += key.Onslot.get_Item(current2.SlotIdx);
							}
						}
					}
					if (num > 0)
					{
						planeCount += num;
						list.Add(key);
					}
				}
			}
			return list;
		}

		protected virtual AirFireInfo getAntiAirFireAttacker(BattleBaseData baseData)
		{
			List<Mem_ship> shipData = baseData.ShipData;
			List<List<Mst_slotitem>> slotData = baseData.SlotData;
			List<AirFireInfo> list = new List<AirFireInfo>();
			for (int i = 0; i < shipData.get_Count(); i++)
			{
				if (shipData.get_Item(i).IsFight())
				{
					if (slotData.get_Item(i).get_Count() != 0)
					{
						AirFireInfo antifireKind = this.getAntifireKind(shipData.get_Item(i), slotData.get_Item(i));
						if (antifireKind != null)
						{
							list.Add(antifireKind);
						}
					}
				}
			}
			if (list.get_Count() == 0)
			{
				return null;
			}
			IOrderedEnumerable<AirFireInfo> orderedEnumerable = Enumerable.OrderBy<AirFireInfo, int>(list, (AirFireInfo x) => x.AirFireKind);
			return Enumerable.First<AirFireInfo>(orderedEnumerable);
		}

		private AirFireInfo getAntifireKind(Mem_ship shipData, List<Mst_slotitem> slotData)
		{
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(shipData.Ship_id);
			ILookup<int, Mst_slotitem> lookup = Enumerable.ToLookup<Mst_slotitem, int>(slotData, (Mst_slotitem x) => x.Type4);
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(54);
			HashSet<int> hashSet2 = hashSet;
			hashSet = new HashSet<int>();
			hashSet.Add(428);
			HashSet<int> hashSet3 = hashSet;
			AirFireInfo result = null;
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			dictionary.Add(1, 65);
			dictionary.Add(2, 58);
			dictionary.Add(3, 50);
			dictionary.Add(4, 52);
			dictionary.Add(5, 55);
			dictionary.Add(6, 40);
			dictionary.Add(7, 45);
			dictionary.Add(8, 50);
			dictionary.Add(9, 40);
			dictionary.Add(10, 60);
			dictionary.Add(11, 55);
			dictionary.Add(12, 45);
			dictionary.Add(13, 35);
			Dictionary<int, int> dictionary2 = dictionary;
			int num = (int)Utils.GetRandDouble(0.0, 100.0, 1.0, 1);
			List<int> list = new List<int>();
			if (hashSet2.Contains(mst_ship.Ctype))
			{
				if (!lookup.Contains(16))
				{
					return null;
				}
				List<Mst_slotitem> list2 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(16)).FindAll((Mst_slotitem x) => x.Tyku >= 8);
				List<Mst_slotitem> list3 = new List<Mst_slotitem>();
				if (lookup.Contains(11))
				{
					list3 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(11)).FindAll((Mst_slotitem x) => x.Tyku >= 2);
				}
				if (list2.get_Count() >= 2 && list3.get_Count() >= 1 && num < dictionary2.get_Item(1))
				{
					list.Add(list2.get_Item(0).Id);
					list.Add(list2.get_Item(1).Id);
					list.Add(list3.get_Item(0).Id);
					result = new AirFireInfo(shipData.Rid, 1, list);
				}
				else if (list2.get_Count() >= 1 && list3.get_Count() >= 1 && num < dictionary2.get_Item(2))
				{
					list.Add(list2.get_Item(0).Id);
					list.Add(list3.get_Item(0).Id);
					result = new AirFireInfo(shipData.Rid, 2, list);
				}
				else if (list2.get_Count() >= 2 && num < dictionary2.get_Item(3))
				{
					list.Add(list2.get_Item(0).Id);
					list.Add(list2.get_Item(1).Id);
					result = new AirFireInfo(shipData.Rid, 3, list);
				}
				return result;
			}
			else
			{
				if (hashSet3.Contains(mst_ship.Id) && lookup.Contains(16) && lookup.Contains(15))
				{
					List<Mst_slotitem> list4 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(16));
					List<Mst_slotitem> list5 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(15)).FindAll((Mst_slotitem x) => x.Tyku >= 9);
					List<Mst_slotitem> list6 = new List<Mst_slotitem>();
					if (lookup.Contains(11))
					{
						list6 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(11)).FindAll((Mst_slotitem x) => x.Tyku >= 2);
					}
					if (list5.get_Count() > 0 && list6.get_Count() > 0 && num < dictionary2.get_Item(10))
					{
						list.Add(list4.get_Item(0).Id);
						list.Add(list5.get_Item(0).Id);
						list.Add(list6.get_Item(0).Id);
						return new AirFireInfo(shipData.Rid, 10, list);
					}
					if (list5.get_Count() > 0 && num < dictionary2.get_Item(11))
					{
						list.Add(list4.get_Item(0).Id);
						list.Add(list5.get_Item(0).Id);
						return new AirFireInfo(shipData.Rid, 11, list);
					}
				}
				List<Mst_slotitem> list7 = new List<Mst_slotitem>();
				if (lookup.Contains(16))
				{
					list7 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(16)).FindAll((Mst_slotitem x) => x.Tyku >= 8);
				}
				List<Mst_slotitem> list8 = new List<Mst_slotitem>();
				if (lookup.Contains(11))
				{
					list8 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(11)).FindAll((Mst_slotitem x) => x.Tyku >= 2);
				}
				if (lookup.Contains(30) && lookup.Contains(3) && lookup.Contains(12) && list8.get_Count() >= 1 && num < dictionary2.get_Item(4))
				{
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(3)).Id);
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(12)).Id);
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(30)).Id);
					return new AirFireInfo(shipData.Rid, 4, list);
				}
				if (list7.get_Count() >= 2 && list8.get_Count() >= 1 && num < dictionary2.get_Item(5))
				{
					list.Add(list7.get_Item(0).Id);
					list.Add(list7.get_Item(1).Id);
					list.Add(list8.get_Item(0).Id);
					return new AirFireInfo(shipData.Rid, 5, list);
				}
				if (lookup.Contains(30) && lookup.Contains(3) && lookup.Contains(12) && num < dictionary2.get_Item(6))
				{
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(3)).Id);
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(12)).Id);
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(30)).Id);
					return new AirFireInfo(shipData.Rid, 6, list);
				}
				if (lookup.Contains(30) && lookup.Contains(16) && list8.get_Count() >= 1 && num < dictionary2.get_Item(7))
				{
					Mst_slotitem mst_slotitem = (list7.get_Count() <= 0) ? Enumerable.First<Mst_slotitem>(lookup.get_Item(16)) : list7.get_Item(0);
					list.Add(mst_slotitem.Id);
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(30)).Id);
					list.Add(list8.get_Item(0).Id);
					return new AirFireInfo(shipData.Rid, 7, list);
				}
				if (list7.get_Count() >= 1 && list8.get_Count() >= 1 && num < dictionary2.get_Item(8))
				{
					list.Add(list7.get_Item(0).Id);
					list.Add(list8.get_Item(0).Id);
					return new AirFireInfo(shipData.Rid, 8, list);
				}
				if (lookup.Contains(30) && lookup.Contains(16) && num < dictionary2.get_Item(9))
				{
					Mst_slotitem mst_slotitem2 = (list7.get_Count() <= 0) ? Enumerable.First<Mst_slotitem>(lookup.get_Item(16)) : list7.get_Item(0);
					list.Add(mst_slotitem2.Id);
					list.Add(Enumerable.First<Mst_slotitem>(lookup.get_Item(30)).Id);
					return new AirFireInfo(shipData.Rid, 9, list);
				}
				List<Mst_slotitem> list9 = new List<Mst_slotitem>();
				List<Mst_slotitem> list10 = new List<Mst_slotitem>();
				List<Mst_slotitem> list11 = new List<Mst_slotitem>();
				if (lookup.Contains(15))
				{
					list9 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(15)).FindAll((Mst_slotitem x) => x.Tyku >= 9);
					list10 = Enumerable.ToList<Mst_slotitem>(lookup.get_Item(15)).FindAll((Mst_slotitem x) => x.Tyku >= 3);
					list11.AddRange(list9);
					list11.AddRange(list10);
				}
				if (list11.get_Count() >= 2 && list8.get_Count() >= 1 && num < dictionary2.get_Item(12))
				{
					list.Add(list11.get_Item(0).Id);
					list.Add(list11.get_Item(1).Id);
					list.Add(list8.get_Item(0).Id);
					return new AirFireInfo(shipData.Rid, 12, list);
				}
				if (hashSet3.Contains(mst_ship.Id))
				{
					return result;
				}
				if (list7.get_Count() >= 1 && list9.get_Count() >= 1 && list8.get_Count() >= 1 && num < dictionary2.get_Item(13))
				{
					list.Add(list7.get_Item(0).Id);
					list.Add(list9.get_Item(0).Id);
					list.Add(list8.get_Item(0).Id);
					return new AirFireInfo(shipData.Rid, 13, list);
				}
				return result;
			}
		}

		private double[] getAntifireParam(AirFireInfo info)
		{
			if (info == null)
			{
				return new double[]
				{
					1.0,
					default(double),
					1.0
				};
			}
			Dictionary<int, double[]> dictionary = new Dictionary<int, double[]>();
			dictionary.Add(1, new double[]
			{
				3.0,
				5.0,
				1.75
			});
			dictionary.Add(2, new double[]
			{
				3.0,
				4.0,
				1.7
			});
			dictionary.Add(3, new double[]
			{
				2.0,
				3.0,
				1.6
			});
			dictionary.Add(4, new double[]
			{
				5.0,
				2.0,
				1.5
			});
			dictionary.Add(5, new double[]
			{
				2.0,
				3.0,
				1.55
			});
			dictionary.Add(6, new double[]
			{
				4.0,
				1.0,
				1.5
			});
			dictionary.Add(7, new double[]
			{
				2.0,
				2.0,
				1.35
			});
			dictionary.Add(8, new double[]
			{
				2.0,
				3.0,
				1.45
			});
			dictionary.Add(9, new double[]
			{
				1.0,
				2.0,
				1.3
			});
			dictionary.Add(10, new double[]
			{
				3.0,
				6.0,
				1.65
			});
			dictionary.Add(11, new double[]
			{
				2.0,
				5.0,
				1.5
			});
			dictionary.Add(12, new double[]
			{
				1.0,
				3.0,
				1.25
			});
			dictionary.Add(13, new double[]
			{
				1.0,
				4.0,
				1.35
			});
			Dictionary<int, double[]> dictionary2 = dictionary;
			return dictionary2.get_Item(info.AirFireKind);
		}

		protected virtual int getDeckTaikuPow(BattleBaseData battleBase)
		{
			int num = 0;
			Dictionary<int, double> dictionary = new Dictionary<int, double>();
			dictionary.Add(16, 0.35);
			dictionary.Add(30, 0.35);
			dictionary.Add(12, 0.6);
			dictionary.Add(11, 0.4);
			Dictionary<int, double> dictionary2 = dictionary;
			List<List<Mst_slotitem>> slotData = battleBase.SlotData;
			double num2 = 0.0;
			using (var enumerator = Enumerable.Select(slotData, (List<Mst_slotitem> items, int idx) => new
			{
				items,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					double num3 = 0.0;
					Mem_ship mem_ship = battleBase.ShipData.get_Item(current.idx);
					if (mem_ship.IsFight())
					{
						List<int> list = battleBase.SlotLevel.get_Item(current.idx);
						using (var enumerator2 = Enumerable.Select(current.items, (Mst_slotitem item, int idx) => new
						{
							item,
							idx
						}).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								var current2 = enumerator2.get_Current();
								if (current2.item.Tyku > 0)
								{
									double num4 = 0.0;
									if (current2.item.Id == 9)
									{
										num4 = 0.25;
									}
									else if (!dictionary2.TryGetValue(current2.item.Type4, ref num4))
									{
										num4 = 0.2;
									}
									num3 += (double)current2.item.Tyku * num4;
									int num5 = list.get_Item(current2.idx);
									if (current2.item.Type4 == 16 || current2.item.Type4 == 30)
									{
										double a2Plus = this.getA2Plus(1, current2.item.Tyku, num5);
										num2 += a2Plus;
									}
									else if (num5 > 0 && (current2.item.Api_mapbattle_type3 == 12 || current2.item.Api_mapbattle_type3 == 13) && current2.item.Tyku > 1)
									{
										double a2Plus2 = this.getA2Plus(2, current2.item.Tyku, num5);
										num2 += a2Plus2;
									}
								}
							}
						}
						num += (int)(num3 + num2);
					}
				}
			}
			double formationParamF = this.formationData.GetFormationParamF3(FormationDatas.GetFormationKinds.AIR, battleBase.Formation);
			num = (int)((double)num * formationParamF);
			return num;
		}

		protected virtual int battleTaiku(List<Mem_ship> taikuHaveShip, int deckTyku, AirFireInfo antifire)
		{
			List<Mem_ship> list = null;
			List<List<Mst_slotitem>> list2 = null;
			List<List<int>> list3 = null;
			double num = 0.0;
			Dictionary<int, int[]> dictionary = null;
			IEnumerable<KeyValuePair<Mem_ship, List<FighterInfo>>> enumerable;
			if (taikuHaveShip.get_Item(0).IsEnemy())
			{
				enumerable = Enumerable.Where<KeyValuePair<Mem_ship, List<FighterInfo>>>(this.e_FighterInfo, (KeyValuePair<Mem_ship, List<FighterInfo>> item) => taikuHaveShip.Contains(item.get_Key()));
				dictionary = this.E_Data.SlotExperience;
				list = this.F_Data.ShipData;
				list2 = this.F_Data.SlotData;
				list3 = this.F_Data.SlotLevel;
				num = 0.75;
			}
			else
			{
				enumerable = Enumerable.Where<KeyValuePair<Mem_ship, List<FighterInfo>>>(this.f_FighterInfo, (KeyValuePair<Mem_ship, List<FighterInfo>> item) => taikuHaveShip.Contains(item.get_Key()));
				dictionary = this.F_Data.SlotExperience;
				list = this.E_Data.ShipData;
				list2 = this.E_Data.SlotData;
				list3 = this.E_Data.SlotLevel;
				num = 0.8;
			}
			int num2 = 0;
			IEnumerable<Mem_ship> enumerable2 = Enumerable.Where<Mem_ship>(list, (Mem_ship x) => x.IsFight());
			if (enumerable2 == null || Enumerable.Count<Mem_ship>(enumerable2) == 0)
			{
				return num2;
			}
			using (IEnumerator<KeyValuePair<Mem_ship, List<FighterInfo>>> enumerator = enumerable.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<FighterInfo>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					using (List<FighterInfo>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FighterInfo current2 = enumerator2.get_Current();
							if (key.Onslot.get_Item(current2.SlotIdx) > 0)
							{
								if (current2.ValidTaiku())
								{
									Mem_ship taikuShip = Enumerable.First<Mem_ship>(Enumerable.OrderBy<Mem_ship, Guid>(enumerable2, (Mem_ship x) => Guid.NewGuid()));
									int num3 = list.FindIndex((Mem_ship x) => x.Rid == taikuShip.Rid);
									double shipTaikuPow = this.getShipTaikuPow(taikuShip, list2.get_Item(num3), list3.get_Item(num3));
									double num4 = this.valance1;
									double num5 = (shipTaikuPow + (double)deckTyku) * num4 * num;
									double[] antifireParam = this.getAntifireParam(antifire);
									if (antifire == null && taikuShip.IsEnemy())
									{
										antifireParam[0] = 0.0;
									}
									int num6 = (int)(num5 * (double)this.randInstance.Next(2) * antifireParam[2] + antifireParam[0]);
									int num7 = (int)((double)key.Onslot.get_Item(current2.SlotIdx) * 0.02 * shipTaikuPow * num4 * (double)this.randInstance.Next(2) + antifireParam[1]);
									int num8 = num6 + num7;
									if (num8 > key.Onslot.get_Item(current2.SlotIdx))
									{
										num8 = key.Onslot.get_Item(current2.SlotIdx);
									}
									int[] array = null;
									if (dictionary.TryGetValue(key.Slot.get_Item(current2.SlotIdx), ref array))
									{
										int slotExpSubValueToTaiku = this.getSlotExpSubValueToTaiku(key.Onslot.get_Item(current2.SlotIdx), num8, array[0]);
										array[1] = array[1] + slotExpSubValueToTaiku;
									}
									List<int> onslot;
									List<int> expr_324 = onslot = key.Onslot;
									int num9;
									int expr_32E = num9 = current2.SlotIdx;
									num9 = onslot.get_Item(num9);
									expr_324.set_Item(expr_32E, num9 - num8);
									num2 += num8;
								}
							}
						}
					}
				}
			}
			return num2;
		}

		private double getShipTaikuPow(Mem_ship shipData, List<Mst_slotitem> mst_slotData, List<int> slotLevels)
		{
			double num;
			if (shipData.IsEnemy())
			{
				num = Math.Sqrt((double)shipData.Taiku);
			}
			else
			{
				Ship_GrowValues battleBaseParam = shipData.GetBattleBaseParam();
				num = (double)battleBaseParam.Taiku * 0.5;
			}
			double num2 = 0.0;
			double num3 = 0.0;
			for (int i = 0; i < mst_slotData.get_Count(); i++)
			{
				Mst_slotitem mst_slotitem = mst_slotData.get_Item(i);
				if (mst_slotitem.Tyku > 0)
				{
					int slotLevel = slotLevels.get_Item(i);
					if (mst_slotitem.Type4 == 16 || mst_slotitem.Type4 == 30)
					{
						double a1Plus = this.getA1Plus(1, mst_slotitem.Tyku, slotLevel);
						num3 += a1Plus;
						num2 += (double)mst_slotitem.Tyku * 2.0;
					}
					else if (mst_slotitem.Type4 == 15)
					{
						double a1Plus2 = this.getA1Plus(2, mst_slotitem.Tyku, slotLevel);
						num3 += a1Plus2;
						num2 += (double)mst_slotitem.Tyku * 3.0;
					}
					else if (mst_slotitem.Type4 == 11)
					{
						num2 += (double)mst_slotitem.Tyku * 1.5;
					}
				}
			}
			return num + num2 + num3;
		}

		protected virtual int getSlotExpSubValueToTaiku(int onSlot, int lostNum, int slotExp)
		{
			return this.getSlotExpSubValueToSeiku(onSlot, lostNum, slotExp);
		}

		protected virtual double getA1Plus(int type, int tyku, int slotLevel)
		{
			double num = (tyku <= 7) ? 2.0 : 3.0;
			if (type == 1)
			{
				return num * Math.Sqrt((double)slotLevel) * 0.5;
			}
			if (type == 2)
			{
				return num * Math.Sqrt((double)slotLevel);
			}
			return 0.0;
		}

		protected virtual double getA2Plus(int type, int tyku, int slotLevel)
		{
			if (type == 1)
			{
				double num = (tyku <= 7) ? 2.0 : 3.0;
				return num * Math.Sqrt((double)slotLevel);
			}
			if (type == 2)
			{
				return Math.Sqrt((double)slotLevel) * 1.5;
			}
			return 0.0;
		}

		protected virtual AirBattle3 getAirBattle3(AirBattle2 air2)
		{
			if (air2 == null)
			{
				return null;
			}
			int num = air2.F_LostInfo.Count - air2.F_LostInfo.LostCount;
			int num2 = air2.E_LostInfo.Count - air2.E_LostInfo.LostCount;
			if (num <= 0 && num2 <= 0)
			{
				return null;
			}
			AirBattle3 airBattle = new AirBattle3();
			if (this.isSearchSuccess[0] && num > 0)
			{
				this.setBakuraiPlane(this.f_FighterInfo, airBattle.F_BakugekiPlane, airBattle.F_RaigekiPlane);
				this.battleBakurai(this.F_Data, this.E_Data, this.f_FighterInfo, ref airBattle.E_Bakurai);
			}
			if (this.isSearchSuccess[1] && num2 > 0)
			{
				this.setBakuraiPlane(this.e_FighterInfo, airBattle.E_BakugekiPlane, airBattle.E_RaigekiPlane);
				this.battleBakurai(this.E_Data, this.F_Data, this.e_FighterInfo, ref airBattle.F_Bakurai);
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
							base.RecoveryShip(current.idx);
						}
					}
				}
			}
			return airBattle;
		}

		protected virtual void setBakuraiPlane(Dictionary<Mem_ship, List<FighterInfo>> fighter, List<int> bakuPlane, List<int> raigPlane)
		{
			using (Dictionary<Mem_ship, List<FighterInfo>>.Enumerator enumerator = fighter.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<FighterInfo>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					using (List<FighterInfo>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FighterInfo current2 = enumerator2.get_Current();
							if (key.Onslot.get_Item(current2.SlotIdx) > 0 && current2.ValidBakurai())
							{
								if (current2.Kind == FighterInfo.FighterKinds.BAKU)
								{
									bakuPlane.Add(current2.SlotData.Id);
								}
								else if (current2.Kind == FighterInfo.FighterKinds.RAIG)
								{
									raigPlane.Add(current2.SlotData.Id);
								}
							}
						}
					}
				}
			}
		}

		protected virtual void battleBakurai(BattleBaseData attacker, BattleBaseData target, Dictionary<Mem_ship, List<FighterInfo>> fighter, ref BakuRaiInfo bakurai)
		{
			Mst_slotitem slotitem = (!attacker.ShipData.get_Item(0).IsEnemy()) ? this.fTouchPlane : this.eTouchPlane;
			List<Mem_ship> list = Enumerable.ToList<Mem_ship>(target.ShipData);
			list.RemoveAll((Mem_ship x) => x.Nowhp <= 0);
			using (Dictionary<Mem_ship, List<FighterInfo>>.Enumerator enumerator = fighter.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<FighterInfo>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					using (List<FighterInfo>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							FighterInfo current2 = enumerator2.get_Current();
							if (key.Onslot.get_Item(current2.SlotIdx) > 0 && current2.ValidBakurai())
							{
								BattleDamageKinds battleDamageKinds = BattleDamageKinds.Normal;
								Mem_ship atackTarget = base.getAtackTarget(key, list, true, false, true, ref battleDamageKinds);
								if (atackTarget != null)
								{
									int num = target.ShipData.IndexOf(atackTarget);
									int num2 = this.getBakuraiAtkPow(current2, key.Onslot.get_Item(current2.SlotIdx), atackTarget);
									num2 = (int)((double)num2 * this.getTouchPlaneKeisu(slotitem));
									int soukou = atackTarget.Soukou;
									int hitPorb = this.getHitPorb();
									int battleAvo = base.getBattleAvo(FormationDatas.GetFormationKinds.AIR, atackTarget);
									BattleHitStatus hitStatus = this.getHitStatus(hitPorb, battleAvo, key, atackTarget, 0.2, true);
									int num3 = this.setDamageValue(hitStatus, num2, soukou, key, atackTarget, target.LostFlag);
									bakurai.Damage[num] += num3;
									BattleDamageKinds battleDamageKinds2 = battleDamageKinds;
									if (battleDamageKinds2 == BattleDamageKinds.Rescue && bakurai.DamageType[num] != BattleDamageKinds.Rescue)
									{
										bakurai.DamageType[num] = BattleDamageKinds.Rescue;
									}
									if (bakurai.Clitical[num] != BattleHitStatus.Clitical && hitStatus == BattleHitStatus.Clitical)
									{
										bakurai.Clitical[num] = hitStatus;
									}
									if (current2.Kind == FighterInfo.FighterKinds.BAKU)
									{
										bakurai.IsBakugeki[num] = true;
									}
									else if (current2.Kind == FighterInfo.FighterKinds.RAIG)
									{
										bakurai.IsRaigeki[num] = true;
									}
								}
							}
						}
					}
				}
			}
		}

		protected virtual int getHitPorb()
		{
			return (int)this.valance2;
		}

		protected virtual int getBakuraiAtkPow(FighterInfo fighter, int fighterNum, Mem_ship target)
		{
			if (!fighter.ValidBakurai())
			{
				return 0;
			}
			Mst_ship mst_ship = Mst_DataManager.Instance.Mst_ship.get_Item(target.Ship_id);
			Mst_stype mst_stype = Mst_DataManager.Instance.Mst_stype.get_Item(target.Stype);
			int num = fighter.AttackShipPow;
			if (fighter.Kind == FighterInfo.FighterKinds.RAIG && mst_stype.IsLandFacillity(mst_ship.Soku))
			{
				num = 0;
			}
			int num2 = 150;
			double num3 = this.valance3 + (double)num * Math.Sqrt((double)fighterNum);
			if (fighter.Kind == FighterInfo.FighterKinds.RAIG)
			{
				num3 *= 0.8 + (double)this.randInstance.Next(2) * 0.7;
			}
			if (num3 > (double)num2)
			{
				num3 = (double)num2 + Math.Sqrt(num3 - (double)num2);
			}
			return (int)num3;
		}

		protected virtual double getTouchPlaneKeisu(Mst_slotitem slotitem)
		{
			if (slotitem == null)
			{
				return 1.0;
			}
			if (slotitem.Houm >= 3)
			{
				return 1.2;
			}
			if (slotitem.Houm == 2)
			{
				return 1.17;
			}
			return 1.12;
		}

		protected override double getAvoHosei(Mem_ship target)
		{
			return 0.0;
		}
	}
}
