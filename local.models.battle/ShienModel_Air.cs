using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class ShienModel_Air : KoukuuModelBase, IShienModel
	{
		protected int _shien_deck_id;

		protected List<ShipModel_Attacker> _ships_shien;

		public int ShienDeckId
		{
			get
			{
				return this._shien_deck_id;
			}
		}

		public ShipModel_Attacker[] ShienShips
		{
			get
			{
				return this._ships_shien.ToArray();
			}
		}

		public BattleSupportKinds SupportType
		{
			get
			{
				return BattleSupportKinds.AirAtack;
			}
		}

		public ShienModel_Air(DeckModel shien_deck, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, SupportAtack data) : base(ships_f, ships_e, data.AirBattle)
		{
			this._shien_deck_id = shien_deck.Id;
			this._ships_shien = new List<ShipModel_Attacker>();
			ShipModel[] ships = shien_deck.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				this._ships_shien.Add(new __ShipModel_Attacker__(ships[i], i));
			}
			base._Initialize();
		}

		protected override void _CreatePlanes()
		{
			this._planes_f = base.__CreatePlanes(this._ships_shien, this._data.F_PlaneFrom);
			List<ShipModel_Attacker> list = new List<ShipModel_Attacker>();
			for (int i = 0; i < this._ships_e.get_Count(); i++)
			{
				if (this._ships_e.get_Item(i) != null)
				{
					list.Add(this._ships_e.get_Item(i).__CreateAttacker__());
				}
			}
			this._planes_e = base.__CreatePlanes(list, this._data.E_PlaneFrom);
		}

		private string ToString_Plane(ShipModel_Attacker[] ships)
		{
			string text = string.Empty;
			PlaneModelBase[] planes = base.GetPlanes(true);
			if (planes.Length == 0)
			{
				text += string.Format("味方側 航空機無し\n", new object[0]);
			}
			else
			{
				text += string.Format("味方側 ", new object[0]);
				for (int i = 0; i < planes.Length; i++)
				{
					text += string.Format("({0}) ", planes[i]);
				}
				text += string.Format("\n", new object[0]);
			}
			PlaneModelBase[] planes2 = base.GetPlanes(false);
			if (planes2.Length == 0)
			{
				text += string.Format("相手側 航空機無し\n", new object[0]);
			}
			else
			{
				text += string.Format("相手側 ", new object[0]);
				for (int j = 0; j < planes2.Length; j++)
				{
					text += string.Format("({0}) ", planes2[j]);
				}
				text += string.Format("\n", new object[0]);
			}
			return text;
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("--味方側航空機\n", new object[0]);
			text += this.ToString_Plane(this.ShienShips);
			text += base.ToString_Stage1();
			text += base.ToString_Stage2();
			return text + base.ToString_Stage3();
		}
	}
}
