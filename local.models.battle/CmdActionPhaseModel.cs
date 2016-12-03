using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class CmdActionPhaseModel
	{
		private __EffectModel__ _eff;

		private ICommandAction _data_f;

		private ICommandAction _data_e;

		public EffectModel Effect
		{
			get
			{
				return this._eff;
			}
		}

		public ICommandAction Action_f
		{
			get
			{
				return this._data_f;
			}
		}

		public ICommandAction Action_e
		{
			get
			{
				return this._data_e;
			}
		}

		public CmdActionPhaseModel(FromMiddleBattleDayData data, Dictionary<int, ShipModel_BattleAll> ships)
		{
			if (data.Production != null)
			{
				this._eff = new __EffectModel__(data.Production);
			}
			if (data.F_BattleData != null)
			{
				if (data.F_BattleData.FmtType == 1)
				{
					HougekiDayBattleFmt hougekiDayBattleFmt = data.F_BattleData as HougekiDayBattleFmt;
					this._data_f = new HougekiListModel(hougekiDayBattleFmt.AttackData, ships);
				}
				else if (data.F_BattleData.FmtType == 2)
				{
					Raigeki data2 = data.F_BattleData as Raigeki;
					RaigekiModel raigekiModel = new RaigekiModel(data2, ships);
					if (raigekiModel.Count_f > 0 || raigekiModel.Count_e > 0)
					{
						this._data_f = raigekiModel;
					}
				}
			}
			if (data.E_BattleData != null)
			{
				if (data.E_BattleData.FmtType == 1)
				{
					HougekiDayBattleFmt hougekiDayBattleFmt2 = data.E_BattleData as HougekiDayBattleFmt;
					this._data_e = new HougekiListModel(hougekiDayBattleFmt2.AttackData, ships);
				}
				else if (data.E_BattleData.FmtType == 2)
				{
					Raigeki data3 = data.E_BattleData as Raigeki;
					RaigekiModel raigekiModel2 = new RaigekiModel(data3, ships);
					if (raigekiModel2.Count_f > 0 || raigekiModel2.Count_e > 0)
					{
						this._data_e = raigekiModel2;
					}
				}
			}
		}

		public ShipModel_Battle GetFirstActionShip()
		{
			ShipModel_Battle shipModel_Battle = this._GetFirstActionShip(this.Action_f);
			if (shipModel_Battle == null)
			{
				shipModel_Battle = this._GetFirstActionShip(this.Action_e);
			}
			return shipModel_Battle;
		}

		public bool HasAction()
		{
			return this.Effect != null || this.Action_f != null || this.Action_e != null;
		}

		public HougekiListModel GetHougeki_f()
		{
			if (this._data_f != null && this._data_f is HougekiListModel)
			{
				return this._data_f as HougekiListModel;
			}
			return null;
		}

		public RaigekiModel GetRaigeki_f()
		{
			if (this._data_f != null && this._data_f is RaigekiModel)
			{
				return this._data_f as RaigekiModel;
			}
			return null;
		}

		public HougekiListModel GetHougeki_e()
		{
			if (this._data_e != null && this._data_e is HougekiListModel)
			{
				return this._data_e as HougekiListModel;
			}
			return null;
		}

		public RaigekiModel GetRaigeki_e()
		{
			if (this._data_e != null && this._data_e is RaigekiModel)
			{
				return this._data_e as RaigekiModel;
			}
			return null;
		}

		private ShipModel_Battle _GetFirstActionShip(ICommandAction cmdAction)
		{
			HougekiListModel hougekiListModel = cmdAction as HougekiListModel;
			if (hougekiListModel != null && hougekiListModel.Count > 0)
			{
				return hougekiListModel.GetData(0).Attacker;
			}
			RaigekiModel raigekiModel = cmdAction as RaigekiModel;
			if (raigekiModel != null)
			{
				return raigekiModel.GetFirstActionShip();
			}
			return null;
		}

		public override string ToString()
		{
			string text = "[戦闘指揮による行動]\n";
			text += string.Format("{0}\n", this._eff);
			text += string.Format("{0}\n", this._data_f);
			return text + string.Format("{0}", this._data_e);
		}
	}
}
