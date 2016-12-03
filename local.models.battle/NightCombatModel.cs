using local.managers;
using Server_Common.Formats.Battle;
using System;

namespace local.models.battle
{
	public class NightCombatModel
	{
		private NightBattleFmt _fmt;

		private ShipModel_BattleAll _flare_f;

		private ShipModel_BattleAll _flare_e;

		private ShipModel_BattleAll _searchlight_f;

		private ShipModel_BattleAll _searchlight_e;

		private RationModel _ration;

		public NightCombatModel(BattleManager bManager, NightBattleFmt fmt, RationModel ration)
		{
			this._fmt = fmt;
			this._flare_f = bManager.GetShip(this._fmt.F_FlareId);
			this._flare_e = bManager.GetShip(this._fmt.E_FlareId);
			this._searchlight_f = bManager.GetShip(this._fmt.F_SearchId);
			this._searchlight_e = bManager.GetShip(this._fmt.E_SearchId);
			this._ration = ration;
		}

		public ShipModel_BattleAll GetFlareShip(bool is_friend)
		{
			return (!is_friend) ? this._flare_e : this._flare_f;
		}

		public ShipModel_BattleAll GetSearchLightShip(bool is_friend)
		{
			return (!is_friend) ? this._searchlight_e : this._searchlight_f;
		}

		public SlotitemModel_Battle GetTouchPlane(bool is_friend)
		{
			int num = (!is_friend) ? this._fmt.E_TouchPlane : this._fmt.F_TouchPlane;
			if (num > 0)
			{
				return new SlotitemModel_Battle(num);
			}
			return null;
		}

		public RationModel GetRationData()
		{
			return this._ration;
		}

		public override string ToString()
		{
			string text = string.Format("[夜戦演出]\n", new object[0]);
			if (this.GetFlareShip(true) != null)
			{
				text += string.Format("味方側 照明弾使用:{0}\n", this.GetFlareShip(true));
			}
			if (this.GetFlareShip(false) != null)
			{
				text += string.Format("相手側 照明弾使用:{0}\n", this.GetFlareShip(false));
			}
			if (this.GetSearchLightShip(true) != null)
			{
				text += string.Format("味方側 探照灯使用:{0}\n", this.GetSearchLightShip(true));
			}
			if (this.GetSearchLightShip(false) != null)
			{
				text += string.Format("相手側 照明弾使用:{0}\n", this.GetSearchLightShip(false));
			}
			if (this.GetTouchPlane(true) != null)
			{
				text += string.Format("味方側 夜間触接使用:{0}\n", this.GetTouchPlane(true));
			}
			if (this.GetTouchPlane(false) != null)
			{
				text += string.Format("相手側 夜間触接使用:{0}\n", this.GetTouchPlane(false));
			}
			if (this.GetRationData() != null)
			{
				text += string.Format("{0}\n", this.GetRationData());
			}
			return text;
		}
	}
}
