using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Server_Common.Formats.Battle
{
	public class EscapeInfo
	{
		public List<int> EscapeShips;

		public List<int> TowShips;

		private readonly HashSet<int> spItemID;

		public EscapeInfo()
		{
			HashSet<int> hashSet = new HashSet<int>();
			hashSet.Add(107);
			this.spItemID = hashSet;
		}

		public EscapeInfo(List<Mem_ship> ships) : this()
		{
			if (!this.haveFlagShipRequireItem(ships.get_Item(0)))
			{
				return;
			}
			IEnumerable<Mem_ship> enumerable = Enumerable.Skip<Mem_ship>(ships, 1);
			this.setEscapeData(enumerable, enumerable);
		}

		public EscapeInfo(List<Mem_ship> ships, List<Mem_ship> ships_combined) : this()
		{
			if (!this.haveFlagShipRequireItem(ships.get_Item(0)))
			{
				return;
			}
			IEnumerable<Mem_ship> enumerable = Enumerable.Skip<Mem_ship>(ships, 1);
			IEnumerable<Mem_ship> enumerable2 = Enumerable.Skip<Mem_ship>(ships_combined, 1);
			List<Mem_ship> list = new List<Mem_ship>(enumerable);
			list.AddRange(enumerable2);
			this.setEscapeData(list, enumerable2);
		}

		public bool ValidEscape()
		{
			return this.EscapeShips != null && this.EscapeShips.get_Count() != 0 && this.TowShips != null && this.TowShips.get_Count() != 0;
		}

		private bool haveFlagShipRequireItem(Mem_ship flagShip)
		{
			Dictionary<int, int> mstSlotItemNum_OrderId = flagShip.GetMstSlotItemNum_OrderId(this.spItemID);
			return Enumerable.Any<int>(mstSlotItemNum_OrderId.get_Values(), (int x) => x > 0);
		}

		private void setEscapeData(IEnumerable<Mem_ship> ships, IEnumerable<Mem_ship> enableTowShips)
		{
			List<int> list = new List<int>();
			List<int> list2 = new List<int>();
			using (IEnumerator<Mem_ship> enumerator = ships.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Mem_ship current = enumerator.get_Current();
					if (current.IsFight())
					{
						DamageState damageState = current.Get_DamageState();
						bool flag = Enumerable.Contains<Mem_ship>(enableTowShips, current);
						if (damageState == DamageState.Taiha)
						{
							list.Add(current.Rid);
						}
						else if (damageState == DamageState.Normal && current.Stype == 2 && flag)
						{
							list2.Add(current.Rid);
						}
					}
				}
			}
			if (list.get_Count() > 0 && list2.get_Count() > 0)
			{
				this.EscapeShips = list;
				this.TowShips = list2;
			}
		}
	}
}
