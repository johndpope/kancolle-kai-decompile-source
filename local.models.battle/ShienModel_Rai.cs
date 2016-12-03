using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class ShienModel_Rai : ShienModel_Hou
	{
		public ShienModel_Rai(DeckModel shien_deck, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, SupportAtack data) : base(shien_deck, ships_f, ships_e, data)
		{
		}

		public override string ToString()
		{
			string text = string.Format("[雷撃支援]\n", new object[0]);
			List<DamageModel> attackDamages = base.GetAttackDamages();
			for (int i = 0; i < attackDamages.get_Count(); i++)
			{
				text += string.Format("{0}", attackDamages.get_Item(i));
			}
			return text;
		}
	}
}
