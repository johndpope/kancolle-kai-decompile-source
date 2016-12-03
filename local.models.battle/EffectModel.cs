using Common.Enum;
using Server_Common.Formats.Battle;
using System;
using System.Collections.Generic;

namespace local.models.battle
{
	public class EffectModel : BattlePhaseModel
	{
		private DayBattleProductionFmt _fmt;

		protected ShipModel_Battle _next_action_ship;

		public BattleCommand Command
		{
			get
			{
				return this._fmt.productionKind;
			}
		}

		public bool Withdrawal
		{
			get
			{
				return this._fmt.Withdrawal;
			}
		}

		public int MeichuBuff
		{
			get
			{
				return this._fmt.FSPP;
			}
		}

		public int RaiMeichuBuff
		{
			get
			{
				return this._fmt.TSPP;
			}
		}

		public int KaihiBuff
		{
			get
			{
				return this._fmt.RSPP;
			}
		}

		public ShipModel_Battle NextActionShip
		{
			get
			{
				return this._next_action_ship;
			}
		}

		public EffectModel(DayBattleProductionFmt fmt)
		{
			this._fmt = fmt;
			this._data_f = new List<DamageModelBase>();
			this._data_e = new List<DamageModelBase>();
		}

		public override List<ShipModel_Defender> GetDefenders(bool is_friend)
		{
			return new List<ShipModel_Defender>();
		}

		public override string ToString()
		{
			string text = string.Empty;
			text += string.Format("[演出/効果] {0}", this.Command);
			if (this.Withdrawal)
			{
				text += string.Format("[離脱成功]", new object[0]);
			}
			if (this.MeichuBuff > 0)
			{
				text += string.Format(" 命中バフ:{0}%", this.MeichuBuff);
			}
			if (this.KaihiBuff > 0)
			{
				text += string.Format(" 回避バフ:{0}%", this.KaihiBuff);
			}
			if (this.RaiMeichuBuff > 0)
			{
				text += string.Format(" 雷命中バフ:{0}%", this.RaiMeichuBuff);
			}
			if (this._next_action_ship != null)
			{
				text += string.Format(" 次行動艦:{0}", this.NextActionShip);
			}
			return text;
		}
	}
}
