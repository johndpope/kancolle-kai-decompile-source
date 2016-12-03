using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class RaigekiModel : BattlePhaseModel, ICommandAction
	{
		private Dictionary<int, DamageModelBase> _attack_to_f;

		private Dictionary<int, DamageModelBase> _attack_to_e;

		public int Count_f
		{
			get
			{
				return this._attack_to_f.get_Count();
			}
		}

		public int Count_e
		{
			get
			{
				return this._attack_to_e.get_Count();
			}
		}

		public RaigekiModel(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Raigeki data)
		{
			this._Init(ships_f, ships_e, data);
		}

		public RaigekiModel(Raigeki data, Dictionary<int, ShipModel_BattleAll> ships)
		{
			List<ShipModel_BattleAll> list = new List<ShipModel_BattleAll>();
			list.Add(null);
			list.Add(null);
			list.Add(null);
			list.Add(null);
			list.Add(null);
			list.Add(null);
			List<ShipModel_BattleAll> list2 = list;
			list = new List<ShipModel_BattleAll>();
			list.Add(null);
			list.Add(null);
			list.Add(null);
			list.Add(null);
			list.Add(null);
			list.Add(null);
			List<ShipModel_BattleAll> list3 = list;
			using (Dictionary<int, ShipModel_BattleAll>.ValueCollection.Enumerator enumerator = ships.get_Values().GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ShipModel_BattleAll current = enumerator.get_Current();
					if (current.IsFriend())
					{
						list2.set_Item(current.Index, current);
					}
					else
					{
						list3.set_Item(current.Index, current);
					}
				}
			}
			this._Init(list2, list3, data);
		}

		public ShipModel_Battle GetFirstActionShip()
		{
			List<ShipModel_Attacker> attackers = this.GetAttackers(true);
			if (attackers != null && attackers.get_Count() > 0)
			{
				return attackers.get_Item(0);
			}
			attackers = this.GetAttackers(false);
			if (attackers != null && attackers.get_Count() > 0)
			{
				return attackers.get_Item(0);
			}
			return null;
		}

		private void _Init(List<ShipModel_BattleAll> ships_f, List<ShipModel_BattleAll> ships_e, Raigeki data)
		{
			this._attack_to_f = this._CreateRaigekiDamageModel(ships_f, ships_e, this._data_e, data.F_Rai);
			this._attack_to_e = this._CreateRaigekiDamageModel(ships_e, ships_f, this._data_f, data.E_Rai);
			for (int i = 0; i < this._data_f.get_Count(); i++)
			{
				if (this._data_f.get_Item(i) != null)
				{
					this._data_f.get_Item(i).__CalcDamage__();
				}
			}
			for (int j = 0; j < this._data_e.get_Count(); j++)
			{
				if (this._data_e.get_Item(j) != null)
				{
					this._data_e.get_Item(j).__CalcDamage__();
				}
			}
		}

		private Dictionary<int, DamageModelBase> _CreateRaigekiDamageModel(List<ShipModel_BattleAll> a_ships, List<ShipModel_BattleAll> d_ships, List<DamageModelBase> data, RaigekiInfo rInfo)
		{
			Dictionary<int, DamageModelBase> dictionary = new Dictionary<int, DamageModelBase>();
			for (int i = 0; i < d_ships.get_Count(); i++)
			{
				ShipModel_BattleAll shipModel_BattleAll = d_ships.get_Item(i);
				if (shipModel_BattleAll != null)
				{
					data.Add(new RaigekiDamageModel(shipModel_BattleAll));
				}
				else
				{
					data.Add(null);
				}
			}
			for (int j = 0; j < rInfo.Target.Length; j++)
			{
				int num = rInfo.Target[j];
				if (num != -1)
				{
					ShipModel_BattleAll shipModel_BattleAll2 = a_ships.get_Item(j);
					DamageModelBase damageModelBase = data.get_Item(num);
					if (damageModelBase == null)
					{
						ShipModel_BattleAll defender = d_ships.get_Item(num);
						damageModelBase = new RaigekiDamageModel(defender);
						data.set_Item(num, damageModelBase);
					}
					((RaigekiDamageModel)damageModelBase).__AddData__(shipModel_BattleAll2, rInfo.Damage[j], rInfo.Clitical[j], rInfo.DamageKind[j]);
					dictionary.set_Item(shipModel_BattleAll2.TmpId, damageModelBase);
				}
			}
			return dictionary;
		}

		public List<ShipModel_Attacker> GetAttackers(bool is_friend)
		{
			List<ShipModel_Attacker> list = new List<ShipModel_Attacker>();
			HashSet<int> hashSet = new HashSet<int>();
			List<DamageModelBase> list2 = (!is_friend) ? this._data_f : this._data_e;
			for (int i = 0; i < list2.get_Count(); i++)
			{
				RaigekiDamageModel raigekiDamageModel = (RaigekiDamageModel)list2.get_Item(i);
				if (raigekiDamageModel != null)
				{
					for (int j = 0; j < raigekiDamageModel.Attackers.get_Count(); j++)
					{
						ShipModel_Attacker shipModel_Attacker = raigekiDamageModel.Attackers.get_Item(j);
						if (!hashSet.Contains(shipModel_Attacker.TmpId))
						{
							hashSet.Add(shipModel_Attacker.TmpId);
							list.Add(shipModel_Attacker);
						}
					}
				}
			}
			return list;
		}

		public override List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			return this.GetDefenders(is_friend, false);
		}

		public List<ShipModel_Defender> GetDefenders(bool is_friend, bool all)
		{
			List<DamageModelBase> list = (!is_friend) ? this._data_e : this._data_f;
			List<RaigekiDamageModel> list2 = list.ConvertAll<RaigekiDamageModel>((DamageModelBase item) => (RaigekiDamageModel)item);
			if (!all)
			{
				list2 = list2.FindAll((RaigekiDamageModel data) => data != null && data.Attackers.get_Count() > 0);
			}
			return list2.ConvertAll<ShipModel_Defender>((RaigekiDamageModel item) => (item != null) ? item.Defender : null);
		}

		public ShipModel_Defender GetAttackTo(int attacker_tmp_id)
		{
			if (this._attack_to_f.ContainsKey(attacker_tmp_id))
			{
				return this._attack_to_f.get_Item(attacker_tmp_id).Defender;
			}
			if (this._attack_to_e.ContainsKey(attacker_tmp_id))
			{
				return this._attack_to_e.get_Item(attacker_tmp_id).Defender;
			}
			return null;
		}

		[Obsolete("GetAttackTo(int attacker_tmp_id) を使用してください", false)]
		public ShipModel_Defender GetAttackTo(ShipModel_Battle attacker)
		{
			return this.GetAttackTo(attacker.TmpId);
		}

		public RaigekiDamageModel GetAttackDamage(int defender_tmp_id)
		{
			List<RaigekiDamageModel> list = this._data_f.ConvertAll<RaigekiDamageModel>((DamageModelBase item) => (RaigekiDamageModel)item);
			RaigekiDamageModel raigekiDamageModel = list.Find((RaigekiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (raigekiDamageModel != null && raigekiDamageModel.Attackers.get_Count() > 0)
			{
				return raigekiDamageModel;
			}
			List<RaigekiDamageModel> list2 = this._data_e.ConvertAll<RaigekiDamageModel>((DamageModelBase item) => (RaigekiDamageModel)item);
			raigekiDamageModel = list2.Find((RaigekiDamageModel r) => r != null && r.Defender.TmpId == defender_tmp_id);
			if (raigekiDamageModel != null && raigekiDamageModel.Attackers.get_Count() > 0)
			{
				return raigekiDamageModel;
			}
			return null;
		}

		[Obsolete("GetAttackTo(int attacker_tmp_id) を使用してください", false)]
		public RaigekiDamageModel GetAttackDamage(int index, bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? this._data_e : this._data_f;
			if (index < list.get_Count())
			{
				return (RaigekiDamageModel)list.get_Item(index);
			}
			return null;
		}

		public List<RaigekiDamageModel> GetAttackDamages(bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? this._data_e : this._data_f;
			List<RaigekiDamageModel> list2 = list.ConvertAll<RaigekiDamageModel>((DamageModelBase item) => (RaigekiDamageModel)item);
			return list2.FindAll((RaigekiDamageModel dmgModel) => dmgModel != null && dmgModel.Attackers.get_Count() > 0);
		}

		public override string ToString()
		{
			string text = string.Empty;
			List<ShipModel_Defender> defenders = this.GetDefenders(true, true);
			for (int i = 0; i < defenders.get_Count(); i++)
			{
				ShipModel_Battle shipModel_Battle = defenders.get_Item(i);
				if (shipModel_Battle != null)
				{
					ShipModel_Defender attackTo = this.GetAttackTo(shipModel_Battle.TmpId);
					if (attackTo != null)
					{
						RaigekiDamageModel attackDamage = this.GetAttackDamage(attackTo.TmpId);
						text += string.Format("{0}({1}) から {2}({3}) へ雷撃 (ダメージ:{4}(c:{7}) {5}{6})\n", new object[]
						{
							shipModel_Battle.Name,
							shipModel_Battle.Index,
							attackTo.Name,
							attackTo.Index,
							attackDamage.GetDamage(shipModel_Battle.TmpId),
							attackDamage.GetHitState(shipModel_Battle.TmpId),
							(!attackDamage.GetProtectEffect(shipModel_Battle.TmpId)) ? string.Empty : "[かばう]",
							attackDamage.__GetDamage__(shipModel_Battle.TmpId)
						});
					}
				}
			}
			defenders = this.GetDefenders(false, true);
			for (int j = 0; j < defenders.get_Count(); j++)
			{
				ShipModel_Battle shipModel_Battle2 = defenders.get_Item(j);
				if (shipModel_Battle2 != null)
				{
					ShipModel_Defender attackTo2 = this.GetAttackTo(shipModel_Battle2.TmpId);
					if (attackTo2 != null)
					{
						RaigekiDamageModel attackDamage2 = this.GetAttackDamage(attackTo2.TmpId);
						text += string.Format("{0}({1}) から {2}({3}) へ雷撃 (ダメージ:{4}(c:{7}) {5}{6})\n", new object[]
						{
							shipModel_Battle2.Name,
							shipModel_Battle2.Index,
							attackTo2.Name,
							attackTo2.Index,
							attackDamage2.GetDamage(shipModel_Battle2.TmpId),
							attackDamage2.GetHitState(shipModel_Battle2.TmpId),
							(!attackDamage2.GetProtectEffect(shipModel_Battle2.TmpId)) ? string.Empty : "[かばう]",
							attackDamage2.__GetDamage__(shipModel_Battle2.TmpId)
						});
					}
				}
			}
			return text;
		}
	}
}
