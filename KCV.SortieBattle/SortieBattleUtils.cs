using local.models;
using System;

namespace KCV.SortieBattle
{
	public class SortieBattleUtils
	{
		public static float GetLovScaleMagnification(IMemShip model)
		{
			return SortieBattleUtils.GetLovScaleMagnification(SortieBattleUtils.GetLovLevel(model));
		}

		public static float GetLovScaleMagnification(LovLevel iLevel)
		{
			float num = 1f;
			float num2 = 1.5f;
			return (num2 - num) / (float)Enum.GetValues(typeof(LovLevel)).get_Length() * (float)iLevel + num;
		}

		public static LovLevel GetLovLevel(IMemShip model)
		{
			LovLevel result = LovLevel.Normal;
			if (model.Lov >= 200)
			{
				result = LovLevel.Max;
			}
			else if (200 > model.Lov && model.Lov >= 50)
			{
				result = LovLevel.Large;
			}
			else if (50 > model.Lov && model.Lov >= 40)
			{
				result = LovLevel.Small;
			}
			else if (40 > model.Lov)
			{
				result = LovLevel.Normal;
			}
			return result;
		}
	}
}
