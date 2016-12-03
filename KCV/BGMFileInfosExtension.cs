using System;

namespace KCV
{
	public static class BGMFileInfosExtension
	{
		public static string BGMFileName(this BGMFileInfos info)
		{
			if (info == BGMFileInfos.PortTools)
			{
				return "sound_bgm";
			}
			if (info == BGMFileInfos.Strategy)
			{
				return "103";
			}
			if (info != BGMFileInfos.RewardGet)
			{
				int num = (int)info;
				return num.ToString();
			}
			return "RewardGet";
		}
	}
}
