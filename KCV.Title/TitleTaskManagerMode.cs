using System;

namespace KCV.Title
{
	public enum TitleTaskManagerMode
	{
		TitleTaskManagerMode_ST,
		TitleTaskManagerMode_BEF = -1,
		TitleTaskManagerMode_NONE = -1,
		Opening,
		SelectMode,
		Option,
		NewGame,
		LoadGame,
		TitleTaskManagerMode_AFT,
		TitleTaskManagerMode_NUM = 5,
		TitleTaskManagerMode_ED = 4
	}
}
