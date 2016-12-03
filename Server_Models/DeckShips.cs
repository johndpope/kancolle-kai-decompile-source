using Server_Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Server_Models
{
	[DataContract(Namespace = "")]
	public class DeckShips
	{
		[DataMember]
		private List<int> ships;

		public int this[int index]
		{
			get
			{
				if (Enumerable.Count<int>(this.ships) - 1 < index)
				{
					return -1;
				}
				return this.ships.get_Item(index);
			}
			set
			{
				if (Enumerable.Count<int>(this.ships) - 1 < index)
				{
					this.ships.Add(value);
				}
				else
				{
					this.ships.set_Item(index, value);
				}
				if (value == -1)
				{
					this.ships.RemoveAt(index);
				}
			}
		}

		public DeckShips()
		{
			this.ships = new List<int>(6);
		}

		public List<Mem_ship> getMemShip()
		{
			List<Mem_ship> ret = new List<Mem_ship>();
			this.ships.ForEach(delegate(int x)
			{
				Mem_ship mem_ship = null;
				if (Comm_UserDatas.Instance.User_ship.TryGetValue(x, ref mem_ship))
				{
					ret.Add(mem_ship);
				}
			});
			return ret;
		}

		public int Count()
		{
			return Enumerable.Count<int>(this.ships);
		}

		public void Clone(out List<int> out_ships)
		{
			out_ships = null;
			out_ships = Enumerable.ToList<int>(this.ships);
		}

		public void Clone(out DeckShips out_ships)
		{
			out_ships = new DeckShips();
			out_ships.ships.AddRange(Enumerable.AsEnumerable<int>(this.ships));
		}

		public int Find(int rid)
		{
			return this.ships.FindIndex((int x) => x == rid);
		}

		public void RemoveRange(int sta, int count)
		{
			this.ships.RemoveRange(sta, count);
		}

		public void Clear()
		{
			this.ships.Clear();
		}

		public bool Equals(DeckShips obj)
		{
			int num = this.Count();
			if (this.Count() != obj.Count())
			{
				return false;
			}
			for (int i = 0; i < num; i++)
			{
				if (this[i] != obj[i])
				{
					return false;
				}
			}
			return true;
		}

		public void RemoveShip(int target)
		{
			this.ships.Remove(target);
		}
	}
}
