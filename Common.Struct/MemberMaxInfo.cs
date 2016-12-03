using System;

namespace Common.Struct
{
	public struct MemberMaxInfo
	{
		public int NowCount;

		public int MaxCount;

		public MemberMaxInfo(int now, int max)
		{
			this.NowCount = now;
			this.MaxCount = max;
		}
	}
}
