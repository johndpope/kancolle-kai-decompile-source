using System;

namespace KCV.InteriorStore
{
	public enum ISTaskManagerMode
	{
		ISTaskManagerMode_ST,
		ISTaskManagerMode_BEF = -1,
		ISTaskManagerMode_NONE = -1,
		ModeSelect,
		Interior,
		Store,
		ISTaskManagerMode_AFT,
		ISTaskManagerMode_NUM = 3,
		ISTaskManagerMode_ED = 2
	}
}
