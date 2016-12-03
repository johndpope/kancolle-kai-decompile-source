using Common.Enum;
using System;

namespace KCV.BattleCut
{
	public static class BattleCommandExtensions
	{
		public static string GetString(this BattleCommand iCommand)
		{
			string result = string.Empty;
			switch (iCommand + 1)
			{
			case BattleCommand.Sekkin:
				result = "なし";
				break;
			case BattleCommand.Hougeki:
				result = "接近";
				break;
			case BattleCommand.Raigeki:
				result = "砲撃";
				break;
			case BattleCommand.Ridatu:
				result = "雷撃";
				break;
			case BattleCommand.Taisen:
				result = "離脱";
				break;
			case BattleCommand.Kaihi:
				result = "対潜";
				break;
			case BattleCommand.Kouku:
				result = "回避";
				break;
			case BattleCommand.Totugeki:
				result = "航空";
				break;
			case BattleCommand.Tousha:
				result = "突撃";
				break;
			case (BattleCommand)9:
				result = "統射";
				break;
			default:
				return string.Empty;
			}
			return result;
		}
	}
}
