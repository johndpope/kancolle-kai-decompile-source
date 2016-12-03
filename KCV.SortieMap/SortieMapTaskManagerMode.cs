using System;

namespace KCV.SortieMap
{
	public enum SortieMapTaskManagerMode
	{
		SortieMapTaskManagerMode_ST,
		SortieMapTaskManagerMode_BEF = -1,
		SortieMapTaskManagerMode_NONE = -1,
		MoveShip,
		Event,
		Formation,
		Result,
		SortieMapTaskManagerMode_AFT,
		SortieMapTaskManagerMode_NUM = 4,
		SortieMapTaskManagerMode_ED = 3
	}
}
