using local.models;
using Server_Controllers;
using Server_Models;
using System;
using System.Collections.Generic;

namespace local.utils
{
	public static class SlotitemUtil
	{
		public enum SlotitemSortKey
		{
			Type3,
			LEVEL_ASCENDING,
			LEVEL_DESCENDING
		}

		public static List<SlotitemModel> __GetAllSlotitems__()
		{
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success)
			{
				List<SlotitemModel> list = new List<SlotitemModel>(api_Result.data.get_Count());
				using (Dictionary<int, Mem_slotitem>.ValueCollection.Enumerator enumerator = api_Result.data.get_Values().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_slotitem current = enumerator.get_Current();
						list.Add(new SlotitemModel(current));
					}
				}
				return list;
			}
			return new List<SlotitemModel>();
		}

		public static List<SlotitemModel> __GetUnsetSlotitems__()
		{
			Api_Result<Dictionary<int, Mem_slotitem>> api_Result = new Api_get_Member().Slotitem();
			if (api_Result.state == Api_Result_State.Success)
			{
				List<SlotitemModel> list = new List<SlotitemModel>(api_Result.data.get_Count());
				using (Dictionary<int, Mem_slotitem>.ValueCollection.Enumerator enumerator = api_Result.data.get_Values().GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Mem_slotitem current = enumerator.get_Current();
						if (current.Equip_flag == Mem_slotitem.enumEquipSts.Unset)
						{
							list.Add(new SlotitemModel(current));
						}
					}
				}
				return list;
			}
			return new List<SlotitemModel>();
		}

		public static List<SlotitemModel> GetSortedList(List<SlotitemModel> slots, SlotitemUtil.SlotitemSortKey sort_key)
		{
			List<SlotitemModel> range = slots.GetRange(0, slots.get_Count());
			SlotitemUtil.Sort(range, sort_key);
			return range;
		}

		public static void Sort(List<SlotitemModel> slots, SlotitemUtil.SlotitemSortKey sort_key)
		{
			switch (sort_key)
			{
			case SlotitemUtil.SlotitemSortKey.Type3:
				slots.Sort(delegate(SlotitemModel x, SlotitemModel y)
				{
					int num = SlotitemUtil._CompType3(x, y);
					if (num != 0)
					{
						return num;
					}
					num = SlotitemUtil._CompMstId(x, y);
					if (num != 0)
					{
						return num;
					}
					return SlotitemUtil._CompGetNo(x, y);
				});
				break;
			case SlotitemUtil.SlotitemSortKey.LEVEL_ASCENDING:
				slots.Sort(delegate(SlotitemModel x, SlotitemModel y)
				{
					int num = SlotitemUtil._CompType3(x, y);
					if (num != 0)
					{
						return num;
					}
					num = SlotitemUtil._CompMstId(x, y);
					if (num != 0)
					{
						return num;
					}
					num = SlotitemUtil._CompLevel(x, y);
					if (num != 0)
					{
						return num;
					}
					return SlotitemUtil._CompGetNo(x, y);
				});
				break;
			case SlotitemUtil.SlotitemSortKey.LEVEL_DESCENDING:
				slots.Sort(delegate(SlotitemModel x, SlotitemModel y)
				{
					int num = SlotitemUtil._CompType3(x, y);
					if (num != 0)
					{
						return num;
					}
					num = SlotitemUtil._CompMstId(x, y);
					if (num != 0)
					{
						return num;
					}
					num = SlotitemUtil._CompLevel(x, y) * -1;
					if (num != 0)
					{
						return num;
					}
					return SlotitemUtil._CompGetNo(x, y);
				});
				break;
			}
		}

		private static int _CompType3(SlotitemModel a, SlotitemModel b)
		{
			if (a.Type3 > b.Type3)
			{
				return 1;
			}
			if (a.Type3 < b.Type3)
			{
				return -1;
			}
			return 0;
		}

		private static int _CompMstId(SlotitemModel a, SlotitemModel b)
		{
			if (a.MstId > b.MstId)
			{
				return 1;
			}
			if (a.MstId < b.MstId)
			{
				return -1;
			}
			return 0;
		}

		private static int _CompMemId(SlotitemModel a, SlotitemModel b)
		{
			if (a.MemId > b.MemId)
			{
				return 1;
			}
			if (a.MemId < b.MemId)
			{
				return -1;
			}
			return 0;
		}

		private static int _CompGetNo(SlotitemModel a, SlotitemModel b)
		{
			if (a.GetNo > b.GetNo)
			{
				return 1;
			}
			if (a.GetNo < b.GetNo)
			{
				return -1;
			}
			return 0;
		}

		private static int _CompLevel(SlotitemModel a, SlotitemModel b)
		{
			if (a.Level > b.Level)
			{
				return 1;
			}
			if (a.Level < b.Level)
			{
				return -1;
			}
			return 0;
		}
	}
}
