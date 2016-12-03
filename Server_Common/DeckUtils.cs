using Common.Enum;
using Server_Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common
{
	public static class DeckUtils
	{
		public static bool IsInNdock(int areaid, List<Mem_ship> ships)
		{
			if (ships == null || ships.get_Count() == 0)
			{
				return false;
			}
			IEnumerable<Mem_ndock> enumerable = Enumerable.Where<Mem_ndock>(Comm_UserDatas.Instance.User_ndock.get_Values(), (Mem_ndock x) => x.Area_id == areaid && x.State == NdockStates.RESTORE);
			if (enumerable == null || Enumerable.Count<Mem_ndock>(enumerable) == 0)
			{
				return false;
			}
			IEnumerable<int> rids = Enumerable.Select<Mem_ship, int>(ships, (Mem_ship ship) => ship.Rid);
			return Enumerable.Any<Mem_ndock>(enumerable, (Mem_ndock ndock) => Enumerable.Contains<int>(rids, ndock.Ship_id));
		}

		public static EscortCheckKinds IsValidChangeEscort(List<Mem_ship> escortShips, Dictionary<int, int> stype_group, int index)
		{
			if (index <= -1)
			{
				return DeckUtils.IsValidChangeEscortDeck(escortShips, stype_group);
			}
			if (stype_group.get_Item(escortShips.get_Item(index).Stype) == 0)
			{
				return EscortCheckKinds.NotEscortShip;
			}
			if (index == 0 && (escortShips.get_Item(index).Stype == 1 || escortShips.get_Item(index).Stype == 2))
			{
				return EscortCheckKinds.NotFlagShip;
			}
			return EscortCheckKinds.OK;
		}

		private static EscortCheckKinds IsValidChangeEscortDeck(List<Mem_ship> escortShips, Dictionary<int, int> stype_group)
		{
			ILookup<int, Mem_ship> lookup = Enumerable.ToLookup<Mem_ship, int>(escortShips, (Mem_ship x) => x.Stype);
			Dictionary<int, int> dictionary = Enumerable.ToDictionary<int, int, int>(Enumerable.Distinct<int>(stype_group.get_Values()), (int value) => value, (int value) => 0);
			List<EscortCheckKinds> list = new List<EscortCheckKinds>();
			Dictionary<EscortCheckKinds, bool> dictionary2 = new Dictionary<EscortCheckKinds, bool>();
			using (IEnumerator enumerator = Enum.GetValues(typeof(EscortCheckKinds)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					object current = enumerator.get_Current();
					EscortCheckKinds escortCheckKinds = (EscortCheckKinds)((int)current);
					list.Add(escortCheckKinds);
					dictionary2.Add(escortCheckKinds, true);
				}
			}
			using (IEnumerator<IGrouping<int, Mem_ship>> enumerator2 = lookup.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					IGrouping<int, Mem_ship> current2 = enumerator2.get_Current();
					int num = stype_group.get_Item(current2.get_Key());
					dictionary.set_Item(num, Enumerable.Count<Mem_ship>(current2));
				}
			}
			int num2 = dictionary.get_Item(0);
			int num3 = dictionary.get_Item(1);
			if (num2 >= 1)
			{
				dictionary2.set_Item(EscortCheckKinds.NotEscortShip, false);
			}
			else if (escortShips.get_Item(0).Stype == 1 || escortShips.get_Item(0).Stype == 2)
			{
				dictionary2.set_Item(EscortCheckKinds.NotFlagShip, false);
			}
			for (int i = 0; i < Enumerable.Count<EscortCheckKinds>(list); i++)
			{
				if (!dictionary2.get_Item(list.get_Item(i)))
				{
					return list.get_Item(i);
				}
			}
			return EscortCheckKinds.OK;
		}
	}
}
