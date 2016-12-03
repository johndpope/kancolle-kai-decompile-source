using Common.Enum;
using Common.Struct;
using local.utils;
using Server_Models;
using System;

namespace local.models
{
	public class Reward_Ship : IReward, IReward_Ship
	{
		private ShipModelMst _ship;

		public ShipModelMst Ship
		{
			get
			{
				return this._ship;
			}
		}

		[Obsolete("Ship から取得してください", false)]
		public string Name
		{
			get
			{
				return this._ship.Name;
			}
		}

		public string GreetingText
		{
			get
			{
				return Utils.GetText(TextType.SHIP_GET_TEXT, this._ship.MstId);
			}
		}

		[Obsolete("Ship から取得してください", false)]
		public Point Offset
		{
			get
			{
				return this._ship.Offsets.GetShipDisplayCenter(false);
			}
		}

		public Reward_Ship(Mst_ship mst)
		{
			this._ship = new ShipModelMst(mst);
		}

		public Reward_Ship(int mst_id)
		{
			this._ship = new ShipModelMst(mst_id);
		}

		public override string ToString()
		{
			return string.Format("{0} {1}(ID:{2}) レア度:{3}", new object[]
			{
				this.Ship.ShipTypeName,
				this.Ship.Name,
				this.Ship.MstId,
				this.Ship.Rare
			});
		}
	}
}
