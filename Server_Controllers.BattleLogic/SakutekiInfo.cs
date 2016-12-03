using Server_Common;
using Server_Common.Formats.Battle;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Controllers.BattleLogic
{
	public class SakutekiInfo : IDisposable
	{
		public int BasePow;

		public int Attack;

		public int Def;

		public Dictionary<Mem_ship, List<int>> LostTargetOnslot;

		private bool practiceFlag;

		public SakutekiInfo(bool practice)
		{
			this.BasePow = 0;
			this.Attack = 0;
			this.Def = 0;
			this.LostTargetOnslot = new Dictionary<Mem_ship, List<int>>();
			this.practiceFlag = practice;
		}

		public void Dispose()
		{
			this.LostTargetOnslot.Clear();
			this.LostTargetOnslot = null;
		}

		public void SetParametar(BattleBaseData baseData, out List<SearchUsePlane> planes)
		{
			List<Mem_ship> shipData = baseData.ShipData;
			List<List<Mst_slotitem>> slotData = baseData.SlotData;
			Dictionary<int, int[]> slotExperience = baseData.SlotExperience;
			List<double> list = new List<double>();
			list.Add(2.0);
			list.Add(5.0);
			list.Add(8.0);
			list.Add(8.0);
			list.Add(8.0);
			list.Add(8.0);
			List<double> list2 = list;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			planes = new List<SearchUsePlane>();
			using (var enumerator = Enumerable.Select(shipData, (Mem_ship obj, int idx) => new
			{
				obj,
				idx
			}).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					var current = enumerator.get_Current();
					Mem_ship obj3 = current.obj;
					if (obj3.IsFight())
					{
						this.LostTargetOnslot.Add(obj3, new List<int>());
						num += (int)((double)obj3.Sakuteki / list2.get_Item(current.idx));
						if (Mst_DataManager.Instance.Mst_stype.get_Item(obj3.Stype).IsMother())
						{
							num4++;
						}
						List<int> list3 = new List<int>();
						using (var enumerator2 = Enumerable.Select(slotData.get_Item(current.idx), (Mst_slotitem obj, int idx) => new
						{
							obj,
							idx
						}).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								var current2 = enumerator2.get_Current();
								Mst_slotitem obj2 = current2.obj;
								int num5 = obj3.Onslot.get_Item(current2.idx);
								if (num5 > 0)
								{
									if (obj2.Api_mapbattle_type3 == 9 || obj2.Api_mapbattle_type3 == 10 || obj2.Api_mapbattle_type3 == 11 || obj2.Api_mapbattle_type3 == 41)
									{
										int num6 = 0;
										int[] array = null;
										if (slotExperience.TryGetValue(obj3.Slot.get_Item(current2.idx), ref array))
										{
											int slotExpUpValue = this.getSlotExpUpValue(num5, array[0], obj2.Exp_rate);
											array[1] += slotExpUpValue;
											num6 = this.getAlvPlus(num5, array[0]);
										}
										num2 += num5 + num6;
										this.LostTargetOnslot.get_Item(obj3).Add(current2.idx);
										list3.Add(obj2.Id);
									}
									else if (obj2.Api_mapbattle_type3 == 6)
									{
										num3 += num5;
									}
								}
							}
						}
						if (list3.get_Count() > 0)
						{
							planes.Add(new SearchUsePlane(obj3.Rid, list3));
						}
					}
				}
			}
			if (num4 == 1)
			{
				num2 += 30;
			}
			else if (num4 > 1)
			{
				int num7 = (num4 - 1) * 10;
				num2 = num2 + 30 + num7;
			}
			this.BasePow = num;
			this.Attack = num2;
			if (num3 == 0)
			{
				this.Def = 0;
			}
			else if (num3 <= 30)
			{
				this.Def = (int)(1.0 + (double)num3 / 9.0);
			}
			else if (num3 <= 120)
			{
				this.Def = (int)(2.0 + (double)num3 / 20.0);
			}
			else if (num3 > 120)
			{
				this.Def = (int)(6.0 + (double)(num3 - 120) / 40.0);
			}
		}

		private int getSlotExpUpValue(int onslotNum, int slotExp, int expUpRate)
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

		private int getAlvPlus(int onslotNum, int slotExp)
		{
			if (onslotNum <= 0)
			{
				return 0;
			}
			double num = Math.Sqrt((double)slotExp * 0.2);
			if (num >= 25.0)
			{
				num += 5.0;
			}
			if (num >= 55.0)
			{
				num += 10.0;
			}
			if (num >= 100.0)
			{
				num += 15.0;
			}
			return (int)num;
		}
	}
}
