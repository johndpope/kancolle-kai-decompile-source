using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class ShipModel_BattleResult : ShipModel_BattleAll, IMemShip
	{
		private int _lov;

		private ShipExpModel _exp_info;

		public int Lov
		{
			get
			{
				return this._lov;
			}
		}

		public ShipExpModel ExpInfo
		{
			get
			{
				return this._exp_info;
			}
		}

		[Obsolete("ExpInfo を使用してください", false)]
		public int ExpFromBattle
		{
			get
			{
				return this._exp_info.ExpAfter - this._exp_info.ExpBefore;
			}
		}

		public ShipModel_BattleResult(BattleShipFmt fmt, int index, bool is_friend, bool practice) : base(fmt, index, is_friend, practice)
		{
		}

		public bool IsDamaged()
		{
			return base.DamagedFlgEnd;
		}

		public void __InitResultData__(int exp_rate_before, ShipModel after_ship, int exp, List<int> levelup_info)
		{
			this._lov = ((after_ship != null) ? after_ship.Lov : 0);
			this._exp_info = new ShipExpModel(exp_rate_before, after_ship, exp, levelup_info);
		}
	}
}
