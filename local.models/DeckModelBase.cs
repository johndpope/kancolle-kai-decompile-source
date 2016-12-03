using Server_Models;
using System;
using System.Collections.Generic;

namespace local.models
{
	public abstract class DeckModelBase
	{
		protected List<ShipModel> _ships;

		public virtual int AreaId
		{
			get
			{
				return 0;
			}
		}

		public virtual string Name
		{
			get
			{
				return string.Empty;
			}
		}

		public virtual int Id
		{
			get
			{
				return 0;
			}
		}

		public string AreaName
		{
			get
			{
				return "海域" + this.AreaId;
			}
		}

		public int Count
		{
			get
			{
				return this._ships.get_Count();
			}
		}

		public bool IsEscortDeckMyself()
		{
			return !(this is DeckModel);
		}

		public virtual bool IsActionEnd()
		{
			return false;
		}

		public bool HasShipMemId(int ship_mem_id)
		{
			return this._ships.Find((ShipModel s) => s.MemId == ship_mem_id) != null;
		}

		public int GetShipIndex(int ship_mem_id)
		{
			return this._ships.FindIndex((ShipModel s) => s.MemId == ship_mem_id);
		}

		public ShipModel GetFlagShip()
		{
			if (this._ships.get_Count() > 0)
			{
				return this._ships.get_Item(0);
			}
			return null;
		}

		public ShipModel GetShip(int index)
		{
			if (index < this._ships.get_Count())
			{
				return this._ships.get_Item(index);
			}
			return null;
		}

		public ShipModel GetShipFromMemId(int mem_id)
		{
			return this._ships.Find((ShipModel ship) => ship.MemId == mem_id);
		}

		public ShipModel[] GetShips()
		{
			return this._ships.ToArray();
		}

		public ShipModel[] GetShips(int length)
		{
			ShipModel[] array = new ShipModel[length];
			for (int i = 0; i < array.Length; i++)
			{
				if (i < this._ships.get_Count())
				{
					array[i] = this._ships.get_Item(i);
				}
				else
				{
					array[i] = null;
				}
			}
			return array;
		}

		public int GetShipCount()
		{
			return this._ships.FindAll((ShipModel ship) => ship != null && ship.NowHp > 0 && !ship.IsEscaped()).get_Count();
		}

		public bool HasRepair()
		{
			return this._ships.Find((ShipModel ship) => ship != null && ship.IsInRepair()) != null;
		}

		public bool HasBling()
		{
			return this._ships.Find((ShipModel ship) => ship != null && ship.IsBling()) != null;
		}

		public List<int> __GetShipMemIds__()
		{
			return this._ships.ConvertAll<int>((ShipModel ship) => ship.MemId);
		}

		public void __CreateShipExpRatesDictionary__(ref Dictionary<int, int> dic)
		{
			for (int i = 0; i < this._ships.get_Count(); i++)
			{
				dic.set_Item(this._ships.get_Item(i).MemId, this._ships.get_Item(i).Exp_Percentage);
			}
		}

		protected void _Update(DeckShips deck_ships, Dictionary<int, ShipModel> ships)
		{
			List<int> list = new List<int>();
			for (int i = 0; i < deck_ships.Count(); i++)
			{
				list.Add(deck_ships[i]);
			}
			this._ships = new List<ShipModel>();
			for (int j = 0; j < list.get_Count(); j++)
			{
				ShipModel shipModel = ships.get_Item(list.get_Item(j));
				this._ships.Add(shipModel);
			}
		}
	}
}
