using Common.Enum;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public interface IShienModel
	{
		int ShienDeckId
		{
			get;
		}

		ShipModel_Attacker[] ShienShips
		{
			get;
		}

		BattleSupportKinds SupportType
		{
			get;
		}

		List<ShipModel_Defender> GetDefenders(bool is_friend);
	}
}
