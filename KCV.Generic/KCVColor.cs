using System;
using UnityEngine;

namespace KCV.Generic
{
	public struct KCVColor
	{
		private static Color _cBlueText = new Color(0f, Mathe.Rate(0f, 255f, 80f), 1f, 0.5f);

		public static readonly Color WarVateransGaugeGreen = new Color(KCVColor.ColorRate(89f), KCVColor.ColorRate(233f), KCVColor.ColorRate(50f), 1f);

		public static readonly Color WarVateransGaugeRed = new Color(KCVColor.ColorRate(227f), KCVColor.ColorRate(44f), KCVColor.ColorRate(44f), 1f);

		public static readonly Color WarVateransEXPGaugeGreen = new Color(KCVColor.ColorRate(163f), KCVColor.ColorRate(233f), KCVColor.ColorRate(208f));

		public static readonly Color BattleBlueLineColor = new Color(0f, KCVColor.ColorRate(80f), 1f, 0.5f);

		public static readonly Color BattleCommandSurfaceBlue = new Color(KCVColor.ColorRate(50f), KCVColor.ColorRate(154f), KCVColor.ColorRate(214f), 1f);

		public static Color blueTextBG
		{
			get
			{
				return KCVColor._cBlueText;
			}
		}

		public static float ColorRate(float val)
		{
			return Mathe.Rate(0f, 255f, val);
		}

		public static Color ConvertColor(float r, float g, float b, float a)
		{
			return new Color(KCVColor.ColorRate(r), KCVColor.ColorRate(g), KCVColor.ColorRate(b), KCVColor.ColorRate(a));
		}
	}
}
