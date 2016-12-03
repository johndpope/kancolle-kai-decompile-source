using Common.Enum;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public abstract class BattlePhaseModel : IBattlePhase
	{
		protected List<DamageModelBase> _data_f;

		protected List<DamageModelBase> _data_e;

		public BattlePhaseModel()
		{
			this._data_f = new List<DamageModelBase>();
			this._data_e = new List<DamageModelBase>();
		}

		public abstract List<ShipModel_Defender> GetDefenders(bool is_friend);

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
			List<ShipModel_Defender> defenders = this.GetDefenders(is_friend);
			return defenders.FindAll((ShipModel_Defender ship) => ship.DamageEventAfter == DamagedStates.Gekichin || ship.DamageEventAfter == DamagedStates.Youin || ship.DamageEventAfter == DamagedStates.Megami);
		}

		public bool HasChuhaEvent()
		{
			return this.HasChuhaEvent(true);
		}

		public bool HasChuhaEvent(bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? this._data_e : this._data_f;
			DamageModelBase damageModelBase = list.Find((DamageModelBase model) => model != null && model.Defender != null && model.Defender.DamageEventAfter == DamagedStates.Tyuuha);
			return damageModelBase != null;
		}

		public bool HasTaihaEvent()
		{
			return this.HasTaihaEvent(true);
		}

		public bool HasTaihaEvent(bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? this._data_e : this._data_f;
			DamageModelBase damageModelBase = list.Find((DamageModelBase model) => model != null && model.Defender != null && model.Defender.DamageEventAfter == DamagedStates.Taiha);
			return damageModelBase != null;
		}

		public bool HasGekichinEvent()
		{
			return this.HasGekichinEvent(true);
		}

		public bool HasGekichinEvent(bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? this._data_e : this._data_f;
			DamageModelBase damageModelBase = list.Find((DamageModelBase model) => model != null && model.Defender != null && (model.Defender.DamageEventAfter == DamagedStates.Gekichin || model.Defender.DamageEventAfter == DamagedStates.Youin || model.Defender.DamageEventAfter == DamagedStates.Megami));
			return damageModelBase != null;
		}

		public bool HasRecoveryEvent()
		{
			return this.HasRecoveryEvent(true);
		}

		public bool HasRecoveryEvent(bool is_friend)
		{
			List<DamageModelBase> list = (!is_friend) ? this._data_e : this._data_f;
			DamageModelBase damageModelBase = list.Find((DamageModelBase model) => model != null && model.Defender != null && (model.Defender.DamageEventAfter == DamagedStates.Youin || model.Defender.DamageEventAfter == DamagedStates.Megami));
			return damageModelBase != null;
		}
	}
}
