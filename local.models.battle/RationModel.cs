using Common.Enum;
using Server_Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace local.models.battle
{
	public class RationModel
	{
		private List<ShipModel_Eater> _ships;

		private List<ShipModel_Eater> _shared;

		public List<ShipModel_Eater> EatingShips
		{
			get
			{
				return this._ships;
			}
		}

		public List<ShipModel_Eater> SharedShips
		{
			get
			{
				return this._shared;
			}
		}

		public RationModel(List<ShipModel_BattleAll> ships_f, Dictionary<int, List<Mst_slotitem>> data)
		{
			this._ships = new List<ShipModel_Eater>();
			this._shared = new List<ShipModel_Eater>();
			for (int i = 0; i < ships_f.get_Count(); i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships_f.get_Item(i);
				if (shipModel_BattleAll != null && data.ContainsKey(shipModel_BattleAll.TmpId))
				{
					ShipModel_Eater shipModel_Eater = shipModel_BattleAll.__CreateEater__();
					if (i > 0)
					{
						ShipModel_Eater shipModel_Eater2 = this._GetSharedShip(data, ships_f.get_Item(i - 1));
						if (shipModel_Eater2 != null)
						{
							this._shared.Add(shipModel_Eater2);
						}
					}
					if (i < ships_f.get_Count() - 1)
					{
						ShipModel_Eater shipModel_Eater3 = this._GetSharedShip(data, ships_f.get_Item(i + 1));
						if (shipModel_Eater3 != null)
						{
							this._shared.Add(shipModel_Eater3);
						}
					}
					this._ships.Add(shipModel_Eater);
				}
			}
			Enumerable.Distinct<ShipModel_Eater>(this._ships);
		}

		private ShipModel_Eater _GetSharedShip(Dictionary<int, List<Mst_slotitem>> data, ShipModel_BattleAll candidate)
		{
			if (candidate == null)
			{
				return null;
			}
			if (candidate.DmgStateEnd == DamageState_Battle.Gekichin)
			{
				return null;
			}
			if (candidate.IsEscape())
			{
				return null;
			}
			if (data.ContainsKey(candidate.TmpId))
			{
				return null;
			}
			return candidate.__CreateEater__();
		}

		public override string ToString()
		{
			string text = "[ == 戦闘糧食フェーズ == ]\n";
			for (int i = 0; i < this.EatingShips.get_Count(); i++)
			{
				ShipModel_Eater shipModel_Eater = this.EatingShips.get_Item(i);
				text += string.Format("[戦闘糧食 使用艦]{0}\n", shipModel_Eater);
			}
			for (int j = 0; j < this.SharedShips.get_Count(); j++)
			{
				ShipModel_Eater shipModel_Eater2 = this.SharedShips.get_Item(j);
				text += string.Format("[戦闘糧食 分配艦]{0}\n", shipModel_Eater2);
			}
			return text;
		}
	}
}
