using Server_Common.Formats;
using System;
using System.Collections.Generic;

namespace local.models
{
	public class TurnResultModel : PhaseResultModel
	{
		public List<RadingResultData> RadingResult
		{
			get
			{
				return this._data.RadingResult;
			}
		}

		public TurnResultModel(TurnWorkResult data) : base(data)
		{
		}

		public override string ToString()
		{
			string text = string.Format("[ターン終了フェーズ]: \n", new object[0]);
			text += string.Format("=通商破壊=\n", new object[0]);
			if (this.RadingResult == null)
			{
				text += "なし";
			}
			else
			{
				for (int i = 0; i < this.RadingResult.get_Count(); i++)
				{
					RadingResultData radingResultData = this.RadingResult.get_Item(i);
					text += string.Format("[通商破壊結果]\n海域{0} 攻撃種別:{1} 輸送船{2}隻(移動中の船を除く)から{3}隻ロスト\n", new object[]
					{
						radingResultData.AreaId,
						radingResultData.AttackKind,
						radingResultData.BeforeNum,
						radingResultData.BreakNum
					});
					text += string.Format("海上護衛艦隊の対潜/対空能力:{0}\n", radingResultData.DeckAttackPow);
					for (int j = 0; j < radingResultData.RadingDamage.get_Count(); j++)
					{
						text += string.Format("\t海上護衛艦隊(MemId:{0})は{1}のダメージ({2})", radingResultData.RadingDamage.get_Item(j).Rid, radingResultData.RadingDamage.get_Item(j).Damage, radingResultData.RadingDamage.get_Item(j).DamageState);
					}
				}
			}
			return text;
		}
	}
}
