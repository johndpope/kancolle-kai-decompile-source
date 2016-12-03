using Common.Enum;
using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class Exec_Search : BattleLogicBase<SearchInfo[]>
	{
		private BattleBaseData _f_Data;

		private BattleBaseData _e_Data;

		private Dictionary<int, BattleShipSubInfo> _f_SubInfo;

		private Dictionary<int, BattleShipSubInfo> _e_SubInfo;

		protected SakutekiInfo f_Param;

		protected SakutekiInfo e_Param;

		protected int valance1;

		protected Dictionary<int, int> valanceShipCount;

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

		public Exec_Search(BattleBaseData myData, Dictionary<int, BattleShipSubInfo> mySubInfo, BattleBaseData enemyData, Dictionary<int, BattleShipSubInfo> enemySubInfo, bool practice)
		{
			this._f_Data = myData;
			this._e_Data = enemyData;
			this._f_SubInfo = mySubInfo;
			this._e_SubInfo = enemySubInfo;
			this.practiceFlag = practice;
			this.f_Param = new SakutekiInfo(this.practiceFlag);
			this.e_Param = new SakutekiInfo(this.practiceFlag);
			Dictionary<int, int> dictionary = new Dictionary<int, int>();
			dictionary.Add(1, 0);
			dictionary.Add(2, 0);
			dictionary.Add(3, 1);
			dictionary.Add(4, 2);
			dictionary.Add(5, 3);
			dictionary.Add(6, 4);
			this.valanceShipCount = dictionary;
			this.valance1 = 20;
		}

		public override void Dispose()
		{
			this.randInstance = null;
			this.f_Param.Dispose();
			this.e_Param.Dispose();
		}

		public override SearchInfo[] GetResultData(FormationDatas formation, BattleCommandParams cParam)
		{
			SearchInfo searchInfo = new SearchInfo();
			SearchInfo searchInfo2 = new SearchInfo();
			SearchInfo[] result = new SearchInfo[]
			{
				searchInfo,
				searchInfo2
			};
			this.f_Param.SetParametar(this.F_Data, out searchInfo.UsePlane);
			this.e_Param.SetParametar(this.E_Data, out searchInfo2.UsePlane);
			searchInfo.SearchValue = this.execSearch(this.f_Param, this.e_Param);
			searchInfo2.SearchValue = ((!this.practiceFlag) ? BattleSearchValues.Success : this.execSearch(this.e_Param, this.f_Param));
			if (searchInfo.SearchValue == BattleSearchValues.Success_Lost || searchInfo.SearchValue == BattleSearchValues.Lost)
			{
				this.subSlotExp(this.f_Param, searchInfo.SearchValue);
				this.subOnslot(this.f_Param);
			}
			if (searchInfo2.SearchValue == BattleSearchValues.Success_Lost || searchInfo2.SearchValue == BattleSearchValues.Lost)
			{
				this.subOnslot(this.e_Param);
			}
			return result;
		}

		protected BattleSearchValues execSearch(SakutekiInfo atk_info, SakutekiInfo def_info)
		{
			double randDouble = Utils.GetRandDouble(1.0, 1.4, 0.1, 1);
			int num = Enumerable.Count<KeyValuePair<Mem_ship, List<int>>>(atk_info.LostTargetOnslot);
			int num2 = atk_info.BasePow + this.valanceShipCount.get_Item(num) - this.valance1 + (int)Math.Sqrt((double)atk_info.Attack * 10.0);
			int num3 = atk_info.Attack - (int)((double)def_info.Def * randDouble);
			int num4 = this.randInstance.Next(20);
			if (atk_info.Attack == 0)
			{
				return (num2 <= num4) ? BattleSearchValues.NotFound : BattleSearchValues.Found;
			}
			if (num2 > num4)
			{
				return (num3 <= 0) ? BattleSearchValues.Success_Lost : BattleSearchValues.Success;
			}
			return (num3 <= 0) ? BattleSearchValues.Faile : BattleSearchValues.Lost;
		}

		protected void subOnslot(SakutekiInfo targetInfo)
		{
			using (Dictionary<Mem_ship, List<int>>.Enumerator enumerator = targetInfo.LostTargetOnslot.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<int>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					using (List<int>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int current2 = enumerator2.get_Current();
							int num = this.randInstance.Next(3);
							if (key.Onslot.get_Item(current2) < num)
							{
								num = key.Onslot.get_Item(current2);
							}
							List<int> onslot;
							List<int> expr_73 = onslot = key.Onslot;
							int num2;
							int expr_78 = num2 = current2;
							num2 = onslot.get_Item(num2);
							expr_73.set_Item(expr_78, num2 - num);
						}
					}
				}
			}
		}

		protected void subSlotExp(SakutekiInfo targetInfo, BattleSearchValues serchKind)
		{
			if (serchKind != BattleSearchValues.Lost && serchKind != BattleSearchValues.Success_Lost)
			{
				return;
			}
			Dictionary<int, int[]> slotExperience = this.F_Data.SlotExperience;
			using (Dictionary<Mem_ship, List<int>>.Enumerator enumerator = targetInfo.LostTargetOnslot.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<Mem_ship, List<int>> current = enumerator.get_Current();
					Mem_ship key = current.get_Key();
					using (List<int>.Enumerator enumerator2 = current.get_Value().GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							int current2 = enumerator2.get_Current();
							if (key.Onslot.get_Item(current2) > 0)
							{
								int num = key.Slot.get_Item(current2);
								double num2 = (serchKind != BattleSearchValues.Lost) ? 6.0 : 12.0;
								double randDouble = Utils.GetRandDouble(0.0, num2 - 1.0, 1.0, 1);
								int num3 = slotExperience.get_Item(num)[0];
								int num4 = num3 - (int)(num2 * 0.5 + randDouble * 0.05);
								if (num4 < 0)
								{
									num4 = 0;
								}
								int num5 = num4 - num3;
								slotExperience.get_Item(num)[1] = slotExperience.get_Item(num)[1] + num5;
							}
						}
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
