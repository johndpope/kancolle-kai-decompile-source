using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public abstract class DeckActionResultModel
	{
		protected MissionResultFmt _mission_fmt;

		protected UserInfoModel _user_info;

		protected Dictionary<int, ShipExpModel> _exps;

		public int DeckID
		{
			get
			{
				return this._mission_fmt.Deck.Rid;
			}
		}

		public ShipModel[] Ships
		{
			get
			{
				return this._user_info.GetDeck(this.DeckID).GetShips();
			}
		}

		public string Name
		{
			get
			{
				return this._user_info.Name;
			}
		}

		public int Rank
		{
			get
			{
				return this._user_info.Rank;
			}
		}

		public int Level
		{
			get
			{
				return this._mission_fmt.MemberLevel;
			}
		}

		public string FleetName
		{
			get
			{
				return this._mission_fmt.Deck.Name;
			}
		}

		public int Exp
		{
			get
			{
				return this._mission_fmt.GetMemberExp;
			}
		}

		public ShipExpModel GetShipExpInfo(int ship_mem_id)
		{
			ShipExpModel result;
			this._exps.TryGetValue(ship_mem_id, ref result);
			return result;
		}

		protected void _SetShipExp(Dictionary<int, int> exp_rates_before)
		{
			ShipModel[] ships = this.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				int exp_rate_before;
				exp_rates_before.TryGetValue(shipModel.MemId, ref exp_rate_before);
				int exp;
				this._mission_fmt.GetShipExp.TryGetValue(shipModel.MemId, ref exp);
				List<int> levelup_info;
				this._mission_fmt.LevelUpInfo.TryGetValue(shipModel.MemId, ref levelup_info);
				this._exps.set_Item(shipModel.MemId, new ShipExpModel(exp_rate_before, shipModel, exp, levelup_info));
			}
		}
	}
}
