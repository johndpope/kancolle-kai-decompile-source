using Common.Enum;
using Common.Struct;
using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class DeckPracticeResultModel : DeckActionResultModel
	{
		private DeckPracticeType _type;

		private Dictionary<int, PowUpInfo> _powup;

		public DeckPracticeType PracticeType
		{
			get
			{
				return this._type;
			}
		}

		public DeckPracticeResultModel(DeckPracticeType type, PracticeDeckResultFmt fmt, UserInfoModel user_info, Dictionary<int, int> exp_rates_before)
		{
			this._type = type;
			this._mission_fmt = fmt.PracticeResult;
			this._powup = fmt.PowerUpData;
			this._user_info = user_info;
			this._exps = new Dictionary<int, ShipExpModel>();
			base._SetShipExp(exp_rates_before);
		}

		public PowUpInfo GetShipPowupInfo(int ship_mem_id)
		{
			PowUpInfo result;
			this._powup.TryGetValue(ship_mem_id, ref result);
			return result;
		}

		public override string ToString()
		{
			string text = string.Format("==[艦隊演習結果]==\n", new object[0]);
			text += string.Format("艦隊 ID:{0}({1}) 艦隊演習タイプ:{2}\n", base.DeckID, base.FleetName, this.PracticeType);
			text += string.Format("提督名:{0} Lv{1}  獲得提督経験値:{2}\n", base.Name, base.Level, base.Exp);
			text += "\n";
			ShipModel[] ships = base.Ships;
			for (int i = 0; i < ships.Length; i++)
			{
				ShipModel shipModel = ships[i];
				ShipExpModel shipExpInfo = base.GetShipExpInfo(shipModel.MemId);
				text += string.Format(" {0}(ID:{1}) {2}\n", shipModel.Name, shipModel.MemId, shipExpInfo);
				PowUpInfo powUpInfo = this._powup.get_Item(shipModel.MemId);
				text += string.Format("   火力上昇:{0} 雷装上昇:{1} 対空上昇:{2} 対潜上昇:{3} 装甲上昇:{4} 回避上昇:{5} 運上昇:{6}", new object[]
				{
					powUpInfo.Karyoku,
					powUpInfo.Raisou,
					powUpInfo.Taiku,
					powUpInfo.Taisen,
					powUpInfo.Soukou,
					powUpInfo.Kaihi,
					powUpInfo.Lucky
				});
				text += "\n";
			}
			return text;
		}
	}
}
