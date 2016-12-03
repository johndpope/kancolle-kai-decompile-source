using System;

namespace local.models.battle
{
	public class BossInsertModel
	{
		private ShipModel_BattleAll _ship;

		public ShipModel_BattleAll Ship
		{
			get
			{
				return this._ship;
			}
		}

		public BossInsertModel(ShipModel_BattleAll boss_ship)
		{
			this._ship = boss_ship;
		}

		public override string ToString()
		{
			return string.Format("{0}(MstId:{1}) 艦種:{2}\n", this._ship.Name, this._ship.MstId, this._ship.ShipTypeName);
		}
	}
}
