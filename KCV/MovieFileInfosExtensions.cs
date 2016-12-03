using System;

namespace KCV
{
	public static class MovieFileInfosExtensions
	{
		public static string GetFilePath(this MovieFileInfos iInfo)
		{
			switch (iInfo)
			{
			case MovieFileInfos.MOVIE_FILE_INFOS_ID_ST:
				return "StreamingAssets/Movies/MV_op.mp4";
			case MovieFileInfos.Startup:
				return "StreamingAssets/Movies/StartUp.mp4";
			case MovieFileInfos.EndingNormal:
				return "StreamingAssets/Movies/ED_Normal.mp4";
			case MovieFileInfos.EndingTrue:
				return "StreamingAssets/Movies/ED_True.mp4";
			default:
				return string.Empty;
			}
		}
	}
}
