using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class ShienModel_Hou : IBattlePhase, IShienModel
	{
		protected List<ShipModel_BattleAll> _ships_f;

		protected List<ShipModel_BattleAll> _ships_e;

		protected SupportAtack _data;

		protected int _shien_deck_id;

		protected List<ShipModel_Attacker> _ships_shien;

		protected List<DamageModel> _dmg_data;

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
				return this._data.SupportType;
			}
		}

		public ShienModel_Hou(DeckModel shien_deck, List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, SupportAtack data)
		{
			this._shien_deck_id = shien_deck.Id;
			this._ships_shien = new List<ShipModel_Attacker>();
			ShipModel[] ships = shien_deck.GetShips();
			for (int i = 0; i < ships.Length; i++)
			{
				this._ships_shien.Add(new __ShipModel_Attacker__(ships[i], i));
			}
			this._ships_f = ships_f;
			this._ships_e = ships_e;
			this._data = data;
			this._dmg_data = new List<DamageModel>();
			for (int j = 0; j < ships_e.get_Count(); j++)
			{
				ShipModel_BattleAll shipModel_BattleAll = ships_e.get_Item(j);
				if (shipModel_BattleAll == null)
				{
					this._dmg_data.Add(null);
				}
				else
				{
					DamageModel damageModel = new DamageModel(shipModel_BattleAll);
					int damage = data.Hourai.Damage[j];
					BattleHitStatus hitstate = data.Hourai.Clitical[j];
					BattleDamageKinds dmgkind = data.Hourai.DamageType[j];
					damageModel.__AddData__(damage, hitstate, dmgkind);
					damageModel.__CalcDamage__();
					this._dmg_data.Add(damageModel);
				}
			}
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			List<ShipModel_Defender> list = new List<ShipModel_Defender>();
			if (is_friend)
			{
				return new List<ShipModel_Defender>();
			}
			for (int i = 0; i < this._dmg_data.get_Count(); i++)
			{
				if (this._dmg_data.get_Item(i) != null)
				{
					ShipModel_Defender defender = this._dmg_data.get_Item(i).Defender;
					if (defender.DmgStateBefore != DamageState_Battle.Gekichin && !defender.IsEscape())
					{
						list.Add(defender);
					}
				}
			}
			return list;
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, DamagedStates damage_event)
		{
			List<ShipModel_Defender> defenders = this.GetDefenders(is_friend);
			return defenders.FindAll((ShipModel_Defender ship) => ship.DamageEventAfter == damage_event);
		}

		public List<ShipModel_Defender> GetGekichinShips()
		{
			return this.GetGekichinShips(true);
		}

		public List<ShipModel_Defender> GetGekichinShips(bool is_friend)
		{
			List<ShipModel_Defender> defenders = this.GetDefenders(true);
			return defenders.FindAll((ShipModel_Defender ship) => ship.DamageEventAfter == DamagedStates.Gekichin || ship.DamageEventAfter == DamagedStates.Youin || ship.DamageEventAfter == DamagedStates.Megami);
		}

		public DamageModel GetAttackDamage(int defender_tmp_id)
		{
			return this._dmg_data.Find((DamageModel d) => d != null && d.Defender.TmpId == defender_tmp_id);
		}

		public List<DamageModel> GetAttackDamages()
		{
			return this._dmg_data.GetRange(0, this._dmg_data.get_Count());
		}

		public bool HasChuhaEvent()
		{
			return this.HasChuhaEvent(true);
		}

		public bool HasChuhaEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = this._dmg_data.Find((DamageModel model) => model != null && model.Defender != null && model.Defender.DamageEventAfter == DamagedStates.Tyuuha);
			return damageModel != null;
		}

		public bool HasTaihaEvent()
		{
			return this.HasTaihaEvent(true);
		}

		public bool HasTaihaEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = this._dmg_data.Find((DamageModel model) => model != null && model.Defender != null && model.Defender.DamageEventAfter == DamagedStates.Taiha);
			return damageModel != null;
		}

		public bool HasGekichinEvent()
		{
			return this.HasGekichinEvent(true);
		}

		public bool HasGekichinEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = this._dmg_data.Find((DamageModel model) => model != null && model.Defender != null && (model.Defender.DamageEventAfter == DamagedStates.Gekichin || model.Defender.DamageEventAfter == DamagedStates.Youin || model.Defender.DamageEventAfter == DamagedStates.Megami));
			return damageModel != null;
		}

		public bool HasRecoveryEvent()
		{
			return this.HasRecoveryEvent(true);
		}

		public bool HasRecoveryEvent(bool is_friend)
		{
			if (is_friend)
			{
				return false;
			}
			DamageModel damageModel = this._dmg_data.Find((DamageModel model) => model != null && model.Defender != null && (model.Defender.DamageEventAfter == DamagedStates.Youin || model.Defender.DamageEventAfter == DamagedStates.Megami));
			return damageModel != null;
		}

		public override string ToString()
		{
			string text = string.Format("[砲撃支援]\n", new object[0]);
			List<DamageModel> attackDamages = this.GetAttackDamages();
			for (int i = 0; i < attackDamages.get_Count(); i++)
			{
				text += string.Format("{0}", attackDamages.get_Item(i));
			}
			return text;
		}
	}
}
